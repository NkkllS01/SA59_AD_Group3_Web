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
        _speciesDAO = new SpeciesDAO(); // Assume DAO handles data fetching
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

    /* Add image from Digital Ocean later
    var baseImageUrl = "https://my-bucket-name.nyc3.digitaloceanspaces.com/species/";
    var imageUrl = baseImageUrl + species.SpecieName.ToLower().Replace(" ", "_") + ".jpg";
    ViewData["ImageUrl"] = imageUrl;
    */

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
