using System.ComponentModel.DataAnnotations;

namespace SmartEnergyMeterAPI.Models
{
    public class EnergyReading
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double Voltage { get; set; }

        [Required]
        public double Current { get; set; }

        [Required]
        public double Power { get; set; }

        [Required]
        public double Frequency { get; set; }

        [Required]
        public double PowerFactor { get; set; }

        public string DeviceId { get; set; } = "ESP8266_01";
    }
}
