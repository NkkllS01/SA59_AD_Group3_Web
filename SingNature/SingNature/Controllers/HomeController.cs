using Microsoft.AspNetCore.Mvc;
using SingNature.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SingNature.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Hardcode the categories for now
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Bees" },
                new Category { CategoryId = 2, CategoryName = "Mushrooms" },
                new Category { CategoryId = 3, CategoryName = "Monitor Lizards" }
            };

            return View(categories);
        }
    }
}
