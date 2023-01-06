using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlazor.Models
{
    public class SunriseItem
    {
        public int Id { get; set; }
        public string? Sunrise { get; set; }//tid
        public string? Sunset { get; set; }//tid
        public string? OriginalSunrise { get; set; }//tid
        public string? OriginalSunset { get; set; }//tid
        public bool SummerWinter { get; set; }//bool
        //public City City { get; set; } = new();
        public int CityId { get; set; }
        public string CityName { get; set; }
        public double CityLatitude { get; set; }
        public double CityLongitude { get; set; }
        public DateTime Datum { get; set; }//Datum

    }
}





