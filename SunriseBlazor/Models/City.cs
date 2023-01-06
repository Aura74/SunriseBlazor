using System.ComponentModel.DataAnnotations;

namespace SunriseBlazor.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; } = String.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
