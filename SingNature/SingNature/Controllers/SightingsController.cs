using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;
using System.Collections.Generic;

namespace SingNature.Controllers
{
    public class SightingsController : Controller
    {
        private readonly SightingsDAO _sightingsDAO;

        public SightingsController()
        {
            _sightingsDAO = new SightingsDAO();
        }

        public IActionResult List()
        {
            var sightings = _sightingsDAO.GetAllSightings();
            var viewModel = new SightingListViewModel { Sightings = sightings };
            return View("SightingList", viewModel);
        }
        [Route("Sightings/Details/{id}")]
        public IActionResult Details(int id)
        {
            var sighting = _sightingsDAO.GetSightingById(id);

            if (sighting == null)
            {
                return NotFound("Sighting not found.");
            }
            return View("SightingDetail",sighting);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sighting sighting)
        {
            if (!ModelState.IsValid)
            {
                return View(sighting);
            }

            var createdSighting = _sightingsDAO.CreateSighting(sighting);
            if (createdSighting == null)
            {
                ModelState.AddModelError("", "Error creating sighting.");
                return View(sighting);
            }

            return RedirectToAction("List");
        }
    }
}