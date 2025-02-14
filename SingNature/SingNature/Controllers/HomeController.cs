using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;

namespace SingNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly WarningService _warningService;
        private readonly SpeciesDAO _speciesDAO;
        private readonly ParkDAO _parkDAO;

        // Use only this constructor for dependency injection
        public HomeController(WarningService warningService, SpeciesDAO speciesDAO, ParkDAO parkDAO)
        {
            _warningService = warningService;
            _speciesDAO = speciesDAO;
            _parkDAO = parkDAO;
        }

        public IActionResult Index()
        {
            var latestWarning = _warningService.GetLatestWarning();

            Console.WriteLine($"Passing warning to view: {(latestWarning != null ? latestWarning.WarningId.ToString() : "No active warning")}");

            if (latestWarning == null)
            {
                Console.WriteLine("No active warning available.");
            }

            var categories = _speciesDAO.GetSpeciesCategory();
            var parks = _parkDAO.GetAllParks().Take(3).ToList();

            var model = new HomeViewModel
            {
                Categories = categories,
                Parks = parks,
                LatestWarning = latestWarning
            };

            return View(model);
        }
    }
}
