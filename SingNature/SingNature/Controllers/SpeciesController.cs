using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    [Route("api/species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly SpeciesDAO _speciesDAO;

        public SpeciesController()
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

    }
}
