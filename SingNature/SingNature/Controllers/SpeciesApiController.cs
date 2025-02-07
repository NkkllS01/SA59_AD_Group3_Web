using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    [Route("api/species")]
    [ApiController]
    public class SpeciesApiController : ControllerBase
    {
        private readonly SpeciesDAO _speciesDAO;

        public SpeciesApiController()
        {
            _speciesDAO = new SpeciesDAO();
        }

        [HttpGet("search/{keyword}")]
         public ActionResult<List<Species>> GetSpeciesByKeyword(string keyword)
        {
            var species = _speciesDAO.GetSpeciesByKeyword(keyword);
            if (species == null || species.Count == 0) 
            {
                return NotFound("No matching species found.");
            }
            return Ok(species);
        }

        [HttpGet("{specieId}")]
        public ActionResult<Species> GetSpeciesById(int specieId)
        {
         if (specieId <= 0)
         {
            return BadRequest("Invalid species ID.");
         }

        var species = _speciesDAO.GetSpeciesById(specieId);
        if (species == null)
        {
            return NotFound("Species not found.");
        }

            return Ok(species);
        }

        [HttpGet("category/{categoryId}")]
        public ActionResult<List<Species>> GetSpeciesByCategory(int categoryId)
        {
            var species = _speciesDAO.GetSpeciesByCategoryId(categoryId);
            if (species == null || species.Count == 0)
            {
                return NotFound("No species found for this category.");
            }
            return Ok(species);
        }
    }
}