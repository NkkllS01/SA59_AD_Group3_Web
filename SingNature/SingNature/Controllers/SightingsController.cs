using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    [Route("api/sightings")]
    [ApiController]
    public class SightingsController : ControllerBase
    {
        private readonly SightingsDAO _sightingsDAO;

        public SightingsController()
        {
            _sightingsDAO = new SightingsDAO();
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
        public IActionResult CreateSighting([FromBody] Sighting? sighting)
        {
            Console.WriteLine($"Received sighting: {JsonSerializer.Serialize(sighting)}");
            if (sighting == null)
            {
                return BadRequest("Sighting object is null.");
            }
            if (sighting.SpecieId <= 0)
            {
                    return BadRequest("Invalid SpecieId or SpecieName provided.");
            }
            try
            {
                var createdSighting = _sightingsDAO.CreateSighting(sighting);
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
    }
}