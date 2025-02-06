
using System.ComponentModel.DataAnnotations;

namespace SingNature.Models
{
    public class Species
    {
        [Key]
        public int SpecieId {get; set;}
        [Required]
        public string SpecieName {get; set;}
        [Required]
        public string Description {get; set;}
        [Required]
        public string Highlights {get; set;}

        public Species() { }

        public Species(int specieId, string specieName, string description, string highlights)
        {
            SpecieId = specieId;
            SpecieName = specieName;
            Description = description;
            Highlights = highlights;
        }
    }
}