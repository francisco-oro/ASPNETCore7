namespace WeatherApp.Models
{
    public class CityWeather
    {
        public string CityUniqueCode { get; set; }
        public string CityName { get; set; }

        public string _DateAndTime { get; set; }

        public DateTime GetDateAndTime()
        {
            return DateTime.Parse(_DateAndTime);
        }

        public int TemperatureFahrenheit  { get; set; }


    }
}
