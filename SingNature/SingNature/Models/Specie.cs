
using System.ComponentModel.DataAnnotations;

namespace SingNature.Models
{
    public class Specie
    {
        [Key]
        public int SpecieId {get; set;}
        
        [Required]
        public string SpecieName {get; set;}

        [Required]
        public string ImageUrl {get; set;}
        
        [Required]
        public string Description {get; set;}
        
        [Required]
        public string Highlights {get; set;}

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public Specie() { }

        public Specie(int specieId, string specieName, string imageUrl, string description, string highlights, int categoryId)
        {
            SpecieId = specieId;
            SpecieName = specieName;
            ImageUrl = imageUrl;
            Description = description;
            Highlights = highlights;
            CategoryId = categoryId;
        }

        public Specie(int specieId, string specieName, string imageUrl, string description, string highlights, Category category)
        {
            SpecieId = specieId;
            SpecieName = specieName;
            ImageUrl = imageUrl;
            Description = description;
            Highlights = highlights;
            Category = category;
            CategoryId = category?.CategoryId ?? 0;
        }
    }
}
