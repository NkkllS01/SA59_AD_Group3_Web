using System.Collections.Generic;

namespace SingNature.Models
{
    public class SearchResultsViewModel
    {
        public string Keyword { get; set; }
        public List<Sighting> Sightings { get; set; }
        public List<Specie> Species { get; set; }
    }
}