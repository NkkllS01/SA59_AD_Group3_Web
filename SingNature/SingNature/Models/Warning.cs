namespace SingNature.Models
{
    public class Warning
    {
        public int WarningId { get; set; }
        public WarningSource Source { get; set; }
        public int? SightingId { get; set; }  // Nullable if not applicable (e.g., for DENGUE warnings)
        public string Cluster { get; set; }
        public string AlertLevel { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Specie { get; set; }    // Nullable if not applicable (e.g., for DENGUE warnings)

    
    
        public enum WarningSource
        {
            SIGHTING,
            DENGUE
        }
    }
}
