using Microsoft.AspNetCore.Mvc;
using SingNature.Models;
using SingNature.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace SingNature.Controllers
{
[Route("Warning")]
public class WarningController : Controller
{
    private readonly WarningDAO _warningDAO;
    private readonly SightingsDAO _sightingDAO;

    public WarningController()
    {
        _warningDAO = new WarningDAO();
        _sightingDAO = new SightingsDAO(); // Assuming you have this DAO for Sightings
    }

    public IActionResult WarningList()
    {
        var warnings = _warningDAO.GetAllWarnings().ToList();

        // Fetch and add the specieName dynamically for each warning
        foreach (var warning in warnings)
        {
            if (warning.Source == SingNature.Models.Warning.WarningSource.SIGHTING)
            {
                // Check if SightingId has a value
                if (warning.SightingId.HasValue)
                {
                    var sighting = _sightingDAO.GetSightingById(warning.SightingId.Value);
                    if (sighting != null)
                    {
                        warning.SpecieName = sighting.SpecieName; // Dynamically adding specieName to the warning object
                    }
                }
            }
        }

        return View(warnings);
    }
}
}