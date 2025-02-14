using System.Collections.Generic;

namespace SingNature.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Park> Parks { get; set; }
        public Warning LatestWarning { get; set; }
    }
}
