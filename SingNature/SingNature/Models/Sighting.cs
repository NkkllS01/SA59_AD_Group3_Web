using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingNature.Models
{
    public class Sighting
    {
        [Key]
        public int SightingId {get; set;}
        [Required]
        public int UserId {get; set;}
        public string? UserName {get; set;} = string.Empty;
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public int SpecieId {get; set;}
        public string? SpecieName {get; set;} = string.Empty;
        [MaxLength(500)] 
        public string? Details {get; set;} = string.Empty;
        public string? ImageUrl {get; set;} = string.Empty;
        [Required]
        public decimal Latitude {get; set;}
        [Required]
        public decimal Longitude {get; set;}
        // [Required]
        // public SightingStatus Status {get; set;} 

        public Sighting() { }

        public Sighting(int sightingId, int userId, string userName, DateTime date, int specieId, string specieName, string? details, string? imageUrl, decimal latitude, decimal longitude)
        {
            SightingId = sightingId;
            UserId = userId;
            UserName = userName;
            Date = date;
            SpecieId = specieId;
            SpecieName = specieName;
            Details = details ?? string.Empty;
            ImageUrl = imageUrl ?? string.Empty;
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    // public enum SightingStatus
    // {
    //     Active,
    //     Inactive
    // }

}