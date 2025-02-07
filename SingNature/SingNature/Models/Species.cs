
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

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public Species() { }

        public Species(int specieId, string specieName, string description, string highlights, int categoryId)
        {
            SpecieId = specieId;
            SpecieName = specieName;
            Description = description;
            Highlights = highlights;
            CategoryId = categoryId;
        }

        public Species(int specieId, string specieName, string description, string highlights, Category category)
        {
            SpecieId = specieId;
            SpecieName = specieName;
            Description = description;
            Highlights = highlights;
            Category = category;
            CategoryId = category?.CategoryId ?? 0;  // This ensures CategoryId is correctly assigned if Category is provided
        }
    }
}
