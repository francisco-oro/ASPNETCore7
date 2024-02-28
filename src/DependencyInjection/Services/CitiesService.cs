using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private readonly List<string> _cities;
        private readonly Guid _serviceInstanceId;
        public Guid ServiceInstanceID
        {
            get
            {
                return _serviceInstanceId;
            }
        }
        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid();
            _cities = new List<string>()
            {
                "London",
                "Paris",
                "New York",
                "Tokyo",
                "Rome"
            }; 
            // TO DO: Add logic to open the db connection
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public List<string> GetCountries()
        {
            return _cities;
        }

        public void Dispose()
        {
            // TO DO: add logic to close db connection
        }
    }
}
