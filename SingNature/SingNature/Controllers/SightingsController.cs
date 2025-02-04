using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public ActionResult<List<Sightings>> GetAllSightings()
        {
            var sightings = _sightingsDAO.GetAllSightings();
            if (sightings == null || sightings.Count == 0)
            {
                return NotFound("No sightings found.");
            }
            return Ok(sightings);
        }

        [HttpGet("{id}")]
        public ActionResult<Sightings> GetSightingById(int id)
        {
            var sighting = _sightingsDAO.GetSightingById(id);
            if (sighting == null) 
            {
                return NotFound("Sighting not found.");
            }
            return Ok(sighting);
        }
    }
}