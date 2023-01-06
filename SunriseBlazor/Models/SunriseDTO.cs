using System.ComponentModel.DataAnnotations;

namespace SunriseBlazor.Models
{
    public class SunriseDTO
    {
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public DateTime start { get; set; }
        [Required]
        public DateTime end { get; set; }
        [Required]
        public string? Location { get; set; }
    }
}
