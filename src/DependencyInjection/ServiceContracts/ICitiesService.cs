namespace ServiceContracts
{
    public interface ICitiesService
    {
        Guid ServiceInstanceID { get; }
        List<string> GetCountries();
        List<string> GetCities();
    }
}
