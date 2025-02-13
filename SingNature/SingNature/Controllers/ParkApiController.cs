using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    [Route("api/parks")]
    [ApiController]
    public class ParkApiController : ControllerBase
    {
        private readonly ParkDAO _parkDAO;

        public ParkApiController()
        {
            _parkDAO = new ParkDAO();
        }

        [HttpGet("{parkId}")]
        public ActionResult<Park> GetParkById(int parkId)
        {
            if (parkId <= 0)
            {
                return BadRequest("Invalid park ID.");
            }

            var park = _parkDAO.GetParkById(parkId);

            if (park == null)
            {
                return NotFound("Park not found.");
            }

            return Ok(park);
        }

        [HttpGet]
        public ActionResult<List<Park>> GetAllParks()
        {
            var parks = _parkDAO.GetAllParks();

            if (parks == null || parks.Count == 0)
            {
                return NotFound("No parks found");
            }
            return Ok(parks);
        }

    }
}
