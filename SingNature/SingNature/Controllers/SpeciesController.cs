using Microsoft.AspNetCore.Mvc;
using SingNature.Models;
using SingNature.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SingNature.Controllers
{
[Route("Species")]
public class SpeciesController : Controller
{
    private readonly SpeciesDAO _speciesDAO;

    public SpeciesController()
    {
        _speciesDAO = new SpeciesDAO(); 
    }

        [HttpGet("SpeciesList/{categoryId}")]
        public IActionResult SpeciesList(int CategoryId)
        {
            var species = _speciesDAO.GetSpeciesByCategoryId(CategoryId);

            if (species == null || !species.Any())
            {
                return NotFound("No species found for this category.");
            }

            return View(species); // Pass species data to the view
        }
        
       [HttpGet("SpeciesDetail/{specieId}")]
       public IActionResult SpeciesDetail(int specieId)
        {
            var species = _speciesDAO.GetSpeciesById(specieId);

            if (species == null)
            {
                return NotFound("Species not found.");
            }

            return View(species);
        }

        [HttpGet("SpeciesCategory")]
        public IActionResult SpeciesCategory()
        {
        var categories = _speciesDAO.GetSpeciesCategory(); 

        return View(categories);
        }

    }
}
