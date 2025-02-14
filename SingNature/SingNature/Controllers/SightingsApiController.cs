using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;
using Amazon.S3;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace SingNature.Controllers
{

    [Route("api/sightings")]
    [ApiController]
    public class SightingsApiController : ControllerBase
    {
        private readonly SightingsDAO _sightingsDAO;
        private readonly IAmazonS3 _s3Client;

        public SightingsApiController(IAmazonS3 s3Client)
        {
            _sightingsDAO = new SightingsDAO();
            _s3Client = s3Client;
        }

        [HttpGet]
        public ActionResult<List<Sighting>> GetAllSightings()
        {
            var sightings = _sightingsDAO.GetAllSightings();
            if (sightings == null || sightings.Count == 0)
            {
                return NotFound("No sightings found.");
            }
            return Ok(sightings);
        }

        [HttpGet("{id}")]
        public ActionResult<Sighting> GetSightingById(int id)
        {
            var sighting = _sightingsDAO.GetSightingById(id);
            if (sighting == null) 
            {
                return NotFound("Sighting not found.");
            }
            return Ok(sighting);
        }

        [HttpGet("search/{keyword}")]
         public ActionResult<List<Sighting>> GetSightingsByKeyword(string keyword)
        {
            var sightings = _sightingsDAO.GetSightingsByKeyword(keyword);
            if (sightings == null || sightings.Count == 0) 
            {
                return NotFound("No sightings found.");
            }
            return Ok(sightings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSighting([FromForm] string sighting, 
            [FromForm] IFormFile file)
        {
            Console.WriteLine($"Received sighting: {JsonSerializer.Serialize(sighting)}");
            var sightingObject = JsonSerializer.Deserialize<Sighting>(sighting, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (sightingObject == null)
                {
                    return BadRequest("Sighting object is null.");
                }
            Console.WriteLine($"Parsed Sighting Object: {JsonSerializer.Serialize(sightingObject)}");
    
            if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
    
            if (sightingObject.SpecieId <= 0)
                {
                    return BadRequest("Invalid SpecieId or SpecieName provided.");
                }

            try
            {
                var fileUrl = await UploadFileToSpace(file);
                sightingObject.ImageUrl = fileUrl;
        
                var createdSighting = _sightingsDAO.CreateSighting(sightingObject);
                if (createdSighting != null)
                    {
                        return CreatedAtAction(nameof(GetSightingById), new { id = createdSighting.SightingId }, createdSighting);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sighting: {ex.Message}");
                return StatusCode(500, "Error creating sighting.");
            }

            return NoContent();
        }

        private async Task<string> UploadFileToSpace(IFormFile file)
        {
            var bucketName = "image-database";
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };
    
            await _s3Client.PutObjectAsync(putRequest);

            return$"https://{bucketName}.sgp1.cdn.digitaloceanspaces.com/{fileName}";
        }   
    }
}
