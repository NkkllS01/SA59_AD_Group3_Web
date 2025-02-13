namespace SingNature.Models
{
    public class Warning
    {
        public int WarningId { get; set; }
        public WarningSource Source { get; set; }
        public int? SightingId { get; set; } 
        public string? Cluster { get; set; }
        public string? AlertLevel { get; set; }
        public object SpecieName { get; internal set; }

        public enum WarningSource
        {
            SIGHTING,
            DENGUE
        }
    }
}
