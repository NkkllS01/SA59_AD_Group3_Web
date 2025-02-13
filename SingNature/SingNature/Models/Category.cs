using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingNature.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }

        //public string ImageUrl { get; set; }

        public List<Specie> Species { get; set; } // Navigation property for species in this category
    }
}