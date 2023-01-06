using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SunriseBlazor.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SunriseBlazor.Services
{
    public class SunriseRepo
    {
        private SunriseDbContext _db;

        public SunriseRepo(SunriseDbContext db)
        {
            _db = db;
        }

        public async Task<City> GetCityLocationAsync(string name)
        {
            var city = await _db.Cities.FirstOrDefaultAsync(c => c.Name == name);
            return city;
        }

        public async Task<SunriseItem> GetDayFromApiAsync(string SunDate, City city)
        {
            var _client = new HttpClient();
            var isDST = DateTime.Parse(SunDate).IsDaylightSavingTime();

            string uri = $"https://api.sunrise-sunset.org/json?lat={city.Latitude.ToString()}&lng={city.Longitude.ToString()}&date={SunDate}";
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            Root wd = await response.Content.ReadFromJsonAsync<Root>();

            string dateInputUp = wd.results.Sunrise;
            string dateInputDown = wd.results.Sunset;
            DateTime Up = DateTime.Parse(dateInputUp);
            DateTime Down = DateTime.Parse(dateInputDown);
            DateTime changedTimeUp;
            DateTime changedTimeDown;

            if (!isDST) // Summertime
            {
                changedTimeUp = Up.AddHours(1);
                changedTimeDown = Down.AddHours(1);
            }
            else // Wintertime
            {
                changedTimeUp = Up.AddHours(2);
                changedTimeDown = Down.AddHours(2);
            }

            var sunUpOrDownTime = new SunriseItem
            {
                Sunrise = $"{changedTimeUp.ToString("HH:mm")}",
                Sunset = $"{changedTimeDown.ToString("HH:mm")}",
                Datum = DateTime.Parse(SunDate),
                OriginalSunrise = Up.ToString("HH:mm"),
                OriginalSunset = Down.ToString("HH:mm"),
                SummerWinter = isDST,
                CityId = city.CityId,
                CityName = city.Name,
                CityLatitude = city.Latitude,
                CityLongitude = city.Longitude
            };

            return sunUpOrDownTime;
        }

        public async Task<List<SunriseItem>> PutSunrisesAsync(List<SunriseItem> sunrises)
        {
            var sunriseOutput = new List<SunriseItem>();

            foreach (var sunrise in sunrises)
            {
                // Kolla om datum vid lat/long existerar redan
                var existingDate = await _db.SunTime.Select(d => d)
                    .Where(d => d.CityLatitude == sunrise.CityLatitude
                    && d.CityLongitude == sunrise.CityLongitude
                    && d.Datum == sunrise.Datum
                    ).FirstOrDefaultAsync();

                // Städa bort existerande för att ersättas med uppdaterad information
                if (existingDate is not null)
                    _db.SunTime.Remove(existingDate);

                await _db.SunTime.AddAsync(sunrise);
                sunriseOutput.Add(sunrise);
            }

            await _db.SaveChangesAsync();

            return sunriseOutput;
        }

        public async Task<List<SunriseItem>> GetLatestDatesAsync()
        {
            // Plocka senaste datum för varje stad
            var dates = await _db.SunTime.GroupBy(o => o.CityName)
                .Select(o => o.OrderByDescending(o => o.Datum)
                .FirstOrDefault())
                .ToListAsync();

            return dates;
        }

        public async Task<List<string>> GetAllCitiesAsync()
        {
            var cities = await _db.Cities
                .OrderBy(o => o.Name)
                .Select(o => o.Name)
                .ToListAsync();

            return cities;
        }

        public async Task RemoveAllForLocationAsync(string city)
        {
            var cities = await _db.SunTime.Where(o => o.CityName == city).ToListAsync();
            _db.SunTime.RemoveRange(cities);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(string city)
        {
            var result = await _db.Cities.Where(o => o.Name == city).FirstOrDefaultAsync();

            if (result is not null)
            {
                _db.Cities.Remove(result);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> AddLocationAsync(string name, double lon, double lat)
        {
            var city = await _db.Cities.Where(o => o.Name == name).FirstOrDefaultAsync();

            if (city is not null)
                return false;

            else
            {
                city = new City { Latitude = lat, Longitude = lon, Name = name };
                await _db.Cities.AddAsync(city);
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CityExistsAsync(string name)
        {
            var city = await _db.Cities.Where(o => o.Name == name).FirstOrDefaultAsync();

            // Platsnamnet existerar redan
            if (city is not null)
                return true;
            else
                return false;
        }
        public async Task<City?> GetLocationFromApiAsync(string name)
        {
            var _client = new HttpClient();
            City city = new();

            if (String.IsNullOrEmpty(name))
                return null;

            else
                name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLower());

            //if (await CityExistsAsync(name))
            //    return null;

            try
            {
                string uri = $"https://geocode.maps.co/search?q={name}";
                var response = await _client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                List<GeoItem> wd = JsonConvert.DeserializeObject<List<GeoItem>>(await response.Content.ReadAsStringAsync());

                wd[0].lat.Replace(',', '.');
                wd[0].lon.Replace(',', '.');

				city.Latitude = double.Parse(wd[0].lat, CultureInfo.InvariantCulture);
                city.Longitude = double.Parse(wd[0].lon, CultureInfo.InvariantCulture);
                city.Name = name;
            }

            catch (Exception e)
            {
                return null;
            }

            return city;
        }
    }
}
