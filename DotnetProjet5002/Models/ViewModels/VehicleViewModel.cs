namespace DotnetProjet5.Models.ViewModels
{
    public class VehicleViewModel
    {
        public string CodeVin { get; set; }
        public int Year { get; set; }
        
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public bool Availability { get; set; }
        public DateTime AvailabilityDate { get; set; }
        public string AvailabilityDateFormatted => AvailabilityDate.ToString("dd/MM/yyyy"); // Format de date sans heure
    }
}
