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
    }
}