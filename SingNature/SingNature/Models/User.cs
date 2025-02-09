using System.ComponentModel.DataAnnotations;

namespace authorization.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public bool SubscribeWarning { get; set; } = false;
        public bool SubscribeNewsletter { get; set; } = false;
    }
}
