using Microsoft.AspNetCore.Mvc;
using SingNature.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SingNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly WarningService _warningService;

        // Inject WarningService to get the latest warning
        public HomeController(WarningService warningService)
        {
            _warningService = warningService;
        }

        public IActionResult Index()
        {
            var latestWarning = _warningService.GetLatestWarning();

            Console.WriteLine($"Passing warning to view: {(latestWarning != null ? latestWarning.WarningId.ToString() : "No active warning")}");

            if (latestWarning == null)
            {
                Console.WriteLine("No active warning available.");
            }

            // Hardcode the categories and parks for now
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Bees" },
                new Category { CategoryId = 2, CategoryName = "Mushrooms" },
                new Category { CategoryId = 3, CategoryName = "Monitor Lizards" }
            };

            var parks = new List<Park>
            {
                new Park { ParkId = 1, ParkName = "A" },
                new Park { ParkId = 2, ParkName = "B" },
                new Park { ParkId = 3, ParkName = "C" }
            };

            // Create the HomeViewModel with categories, parks, and the latest warning
            var model = new HomeViewModel
            {
                Categories = categories,
                Parks = parks,
                LatestWarning = latestWarning
            };

            // Pass the model to the view
            return View(model);
        }
    }
}
