using HolidaySearch.Models.Entity;

namespace HolidaySearch.Interfaces.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetFlights(string from, string to, DateTimeOffset departureDate) => throw new NotImplementedException();
    }
}
