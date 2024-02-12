using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        public List<CityWeather> CitiesWeathers { get; set; } = new List<CityWeather>()
        {
            new CityWeather() { CityUniqueCode = "LDN", CityName = "London", DateAndTime = "2030-01-01 8:00",  TemperatureFahrenheit = 33 }, 
            new CityWeather() {CityUniqueCode = "NYC", CityName = "London", DateAndTime = "2030-01-01 3:00",  TemperatureFahrenheit = 60}, 
            new CityWeather() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = "2030-01-01 9:00",  TemperatureFahrenheit = 82
            }
        } ;

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
