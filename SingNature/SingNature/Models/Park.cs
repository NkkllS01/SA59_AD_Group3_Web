namespace SingNature.Models
{
    public class Park
    {
        public int ParkId { get; set; }

        public string ParkName { get; set;}

        public string ParkRegion { get; set; }

        public string ParkType { get; set; }

        public string? ImageUrl {get; set;} = string.Empty;

        public string ParkDescription { get; set;}

        public string OpeningHours { get; set;}

        public double Latitude { get; set; }

        public double Longitude { get; set; }

    }
}