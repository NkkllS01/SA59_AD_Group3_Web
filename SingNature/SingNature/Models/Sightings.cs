using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingNature.Models
{
    public class Sightings
    {
        [Key]
        public int SightingId {get; set;}
        [Required]
        public int UserId {get; set;}
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public int SpecieId {get; set;}
        [MaxLength(500)] 
        public string? Details {get; set;}
        public string? ImageUrl {get; set;}
        [Required]
        public decimal Latitude {get; set;}
        [Required]
        public decimal Longitude {get; set;}
        [Required]
        public SightingStatus Status {get; set;} 

        public Sightings(int sightingId, int userId, DateTime date, int specieId, string details, string imageUrl, decimal latitude, decimal longitude, SightingStatus status)
        {
            SightingId = sightingId;
            UserId = userId;
            Date = date;
            SpecieId = specieId;
            Details = details;
            ImageUrl = imageUrl;
            Latitude = latitude;
            Longitude = longitude;
            Status = status;
        }
    }

    public enum SightingStatus
    {
        Active,
        Inactive
    }

}