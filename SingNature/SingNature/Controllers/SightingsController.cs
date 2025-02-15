using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;
using System.Collections.Generic;
using SingNature.Services;
using System.Text.Json;

namespace SingNature.Controllers
{
    [Route("Sightings")] 
    public class SightingsController : Controller
    {
        private readonly SightingsDAO _sightingsDAO;
        private readonly S3Service _s3Service;
        private readonly ILogger<SightingsController> _logger;

        public SightingsController(SightingsDAO sightingsDAO, S3Service s3Service, ILogger<SightingsController> logger)
        {
            _sightingsDAO = new SightingsDAO();
            _s3Service = s3Service ?? throw new ArgumentNullException(nameof(s3Service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var sightings = _sightingsDAO.GetAllSightings();
            var viewModel = new SightingListViewModel { Sightings = sightings };
            return View(viewModel);
        }

        [Route("Sightings/Details/{id}")]
        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var sighting = _sightingsDAO.GetSightingById(id);

            if (sighting == null)
            {
                return NotFound("Sighting not found.");
            }
            return View(sighting);
        }





        [HttpGet("Create")]
        public IActionResult Create()
        {

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access to Create. Redirecting to Login.");
                return RedirectToAction("Login", "User");
            }

            return View("ReportSighting");
        }


        [HttpGet("{id}")]
        public IActionResult GetSightingById(int id)
        {
            var sighting = _sightingsDAO.GetSightingById(id);
            if (sighting == null)
            {
                return NotFound(new { Message = "Sighting not found." });
            }
            return Ok(sighting);
        }

        [HttpPost("UploadSighting")]
        public async Task<IActionResult> UploadSighting(
     [FromForm] int specieId,
     [FromForm] string details,
     [FromForm] IFormFile file,
     [FromForm] double latitude,
     [FromForm] double longitude)
        {

            int? userId = HttpContext.Session.GetInt32("UserId");


            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access attempt to UploadSighting. Returning JSON 401...");
                return Unauthorized(new { Message = "You must be logged in to upload a sighting.", RedirectUrl = Url.Action("Login", "User") });
            }


            _logger.LogInformation($" Upload request received: UserId={userId}, SpecieId={specieId}, Details={details}");

            if (file == null || file.Length == 0)
            {
                _logger.LogError("No file uploaded.");
                return BadRequest(new { Message = "File is required." });
            }

            if (specieId <= 0 || string.IsNullOrWhiteSpace(details))
            {
                _logger.LogError("Missing required fields.");
                return BadRequest(new { Message = "SpecieId, File, and Details are required." });
            }

            try
            {
                var fileStream = file.OpenReadStream();
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var contentType = file.ContentType;

                _logger.LogInformation($" Uploading file: {fileName}");

                var fileUrl = await _s3Service.UploadFileAsync(fileStream, fileName, contentType);
                if (string.IsNullOrEmpty(fileUrl))
                {
                    _logger.LogError("File upload failed.");
                    return StatusCode(500, new { Message = "Failed to upload the file." });
                }

                var sighting = new Sighting
                {
                    UserId = userId.Value,
                    SpecieId = specieId,
                    Date = DateTime.UtcNow,
                    Details = details,
                    ImageUrl = fileUrl,
                    Latitude = Convert.ToDecimal(latitude),
                    Longitude = Convert.ToDecimal(longitude),
                };

                _logger.LogInformation($" Saving sighting record: {JsonSerializer.Serialize(sighting)}");

                var createdSighting = _sightingsDAO.CreateSighting(sighting);
                return createdSighting != null
                    ? CreatedAtAction(nameof(GetSightingById), new { id = createdSighting.SightingId }, createdSighting)
                    : StatusCode(500, new { Message = "Failed to save sighting." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading sighting: {ex.Message}");
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}