using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authorization.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(100)]
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(8)]
        public string Mobile { get; set; }

        [Required]
        public bool Newsletter { get; set; }

        [Required]
        public bool Warning { get; set; }

        [Required]
        [Column(TypeName = "ENUM('Public', 'Staff', 'Pest Control Company')")]
        public string Type { get; set; }

        [Required]
        [Column(TypeName = "ENUM('Active', 'Inactive')")]
        public string Status { get; set; }
    }
}
