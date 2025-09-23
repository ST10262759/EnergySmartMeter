namespace SmartEnergyMeterAPI.Models
{
    public class EnergyReadingDto
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Power { get; set; }
        public double Frequency { get; set; }
        public double PowerFactor { get; set; }
        public string? DeviceId { get; set; }
    }
}
