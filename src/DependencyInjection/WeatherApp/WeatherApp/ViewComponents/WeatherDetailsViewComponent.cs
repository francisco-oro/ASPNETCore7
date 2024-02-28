using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.ViewComponents
{
    [ViewComponent]
    public class WeatherDetailsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeather cityWeather)
        {
            return View("Weather", cityWeather);
        }
    }
}
