using System.Collections.Generic;

namespace SingNature.Models
{
    public class SearchResultsViewModel
    {
        public string Keyword { get; set; }
        public List<Sightings> Sightings { get; set; }
        public List<Species> Species { get; set; }
    }
}