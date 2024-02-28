using WeatherApp.Models;

namespace WeatherApp.ServiceContracts
{
    public interface IWeatherService
    {
        /// <summary>
        /// Get all CityWeather entries
        /// </summary>
        /// <returns>A list of CityWeather objects that contains weather details of cities</returns>
        List<CityWeather> GetWeatherDetails();

        /// <summary>
        /// Get the CityWeather details of the given city code
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns>An object of CityWeather based on the given city code</returns>
        CityWeather? GetWeatherByCityCode(string cityCode);
    }
}
