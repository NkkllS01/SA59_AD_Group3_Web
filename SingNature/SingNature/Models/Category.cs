using System.ComponentModel.DataAnnotations;

namespace SingNature.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }

        public List<Species> Species { get; set; } // Navigation property for species in this category
    }
}