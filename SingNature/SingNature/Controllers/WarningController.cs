using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    [Route("api/warnings")]
    [ApiController]
    public class WarningController : ControllerBase
    {
        private readonly WarningDAO _warningDAO;

        public WarningController()
        {
            _warningDAO = new WarningDAO();
        }

        [HttpGet("{warningId}")]
        public ActionResult<Warning> GetWarningById(int warningId)
        {
            if (warningId <= 0)
            {
                return BadRequest("Invalid warning ID.");
            }

            var warning = _warningDAO.GetWarningById(warningId);

            if (warning == null)
            {
                return NotFound("Warning not found.");
            }

            return Ok(warning);
        }

        [HttpGet]
        public ActionResult<List<Warning>> GetAllWarnings()
        {
            var warnings = _warningDAO.GetAllWarnings()
                .Select(w => new
                {
                    w.WarningId,
                    Source = w.Source.ToString(), 
                    w.SightingId,
                    w.Cluster,
                    w.AlertLevel,
                    w.Date,
                    w.Description,
                    w.Specie
                })
                .ToList();

            if (warnings == null || warnings.Count == 0)
            {
                return NotFound("No warnings found");
            }
            return Ok(warnings);
        }

    }
}