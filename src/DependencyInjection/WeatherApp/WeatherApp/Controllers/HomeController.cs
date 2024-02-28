using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(_weatherService.GetWeatherDetails());
        }

        [HttpGet("weather/{cityCode}")]
        public IActionResult CityWeatherDetails(string? cityCode)
        {
            CityWeather? cityWeather = _weatherService.GetWeatherByCityCode(cityCode);
            if (cityWeather != null)
            {
                return View(cityWeather);
            }

            return NotFound();
        }
}
}
