using Microsoft.AspNetCore.Mvc;
using SingNature.Data;
using SingNature.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SingNature.Controllers
{
    public class SearchController : Controller
    {
        private readonly HttpClient _httpClient;

        public SearchController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("/Search/Results")]
        public async Task<IActionResult> Results(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }

            var sightings = await FetchSightings(keyword);
            var species = await FetchSpecies(keyword);

            var viewModel = new SearchResultsViewModel
            {
                Keyword = keyword,
                Sightings = sightings,
                Species = species
            };

            return View(viewModel);
        }

        private async Task<List<Sighting>> FetchSightings(string keyword)
        {
            var apiUrl = $"https://localhost:5076/api/sightings/search/{keyword}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            return JsonConvert.DeserializeObject<List<Sighting>>(response) ?? new List<Sighting>();
        }

        private async Task<List<Specie>> FetchSpecies(string keyword)
        {
            var apiUrl = $"https://localhost:5076/api/species/search/{keyword}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            return JsonConvert.DeserializeObject<List<Specie>>(response) ?? new List<Specie>();
        }
    }
}