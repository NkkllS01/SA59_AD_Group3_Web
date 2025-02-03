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
    public class SightingsController : Controller
    {
        [HttpGet]
        public ActionResult<List<Sightings>> GetAllSightings()
        {
            var sightings = SinghtingsDAO.GetAllSightings();
            if (sightings == null || sightings.Count == 0)
            {
                return NotFound("No sightings found.");
            }
            return Ok(sightings);
        }

        [HttpGet("{id}")]
        public ActionResult<Sightings> GetSightingById(int id)
        {
            var sighting = SinghtingsDAO.GetSightingById(id);
            if (sighting == null) 
            {
                return NotFound("Sighting not found.");
            }
            return Ok(sighting);
        }
        
    }
}