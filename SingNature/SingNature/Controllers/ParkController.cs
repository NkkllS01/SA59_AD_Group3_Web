using Microsoft.AspNetCore.Mvc;
using SingNature.Models;
using SingNature.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SingNature.Controllers
{
    [Route("Parks")]
    public class ParkController : Controller
    {
        private readonly ParkDAO _parkDAO;

        public ParkController()
        {
            _parkDAO = new ParkDAO();
        }

        [HttpGet("ParkList")]
        public IActionResult ParkList()
        {
            var parks = _parkDAO.GetAllParks(); 
            return View(parks);  
        }

        [HttpGet("ParkDetail/{parkId}")]
        public IActionResult ParkDetail(int parkId)
        {
            var park = _parkDAO.GetParkById(parkId); 
            if (park == null)
            {
                return NotFound("No park found with the given ID");
            }
            return View(park);
        }
    }
}

