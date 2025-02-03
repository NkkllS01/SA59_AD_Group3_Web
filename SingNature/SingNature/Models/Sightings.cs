using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingNature.Models
{
    public class Sightings
    {
        public int sightingId {get; set;}
        public int userId {get; set;}
        public DateOnly date {get; set;}
        public int specieId {get; set;}
        public string details {get; set;}
        public string imageUrl {get; set;}
        public string latitude {get; set;}
        public string longitude {get; set;}
        public string status {get; set;}

    }
}