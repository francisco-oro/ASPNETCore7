using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        public List<CityWeather> CitiesWeathers { get; set; } = new List<CityWeather>()
        {
            new CityWeather()
            {
                CityUniqueCode = "LDN", CityName = "London", _DateAndTime = "2030-01-01 8:00",
                TemperatureFahrenheit = 36
            },
            new CityWeather()
            {
                CityUniqueCode = "NYC", CityName = "London", _DateAndTime = "2030-01-01 3:00",
                TemperatureFahrenheit = 60
            },
            new CityWeather()
            {
                CityUniqueCode = "PAR", CityName = "Paris", _DateAndTime = "2030-01-01 9:00", TemperatureFahrenheit = 82
            }
        };

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(CitiesWeathers);
        }

        [HttpGet("weather/{cityCode}")]
        public IActionResult CityWeatherDetails(string? cityCode)
        {
            CityWeather? cityWeather = CitiesWeathers.Where(cw => cw.CityUniqueCode.Equals(cityCode)).FirstOrDefault();
            if (cityWeather != null)
            {
                return View(cityWeather);
            }

            return NotFound();
        }
}
}
