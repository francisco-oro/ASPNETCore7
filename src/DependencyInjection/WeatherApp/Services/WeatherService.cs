using ServiceContracts;
using WeatherApp.Models;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        private readonly List<CityWeather> _cityWeatherList;

        public WeatherService()
        {
            _cityWeatherList = new List<CityWeather>()
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
                    CityUniqueCode = "PAR", CityName = "Paris", _DateAndTime = "2030-01-01 9:00",
                    TemperatureFahrenheit = 82
                }
            };
        }
        public List<CityWeather> GetWeatherDetails()
        {
            return _cityWeatherList;
        }

        public CityWeather? GetWeatherByCityCode(string cityCode)
        {
            return _cityWeatherList.FirstOrDefault(c => c.CityUniqueCode.Equals(cityCode));
        }
    }
}