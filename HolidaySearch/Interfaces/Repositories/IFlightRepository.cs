using HolidaySearch.Models.Entity;

namespace HolidaySearch.Interfaces.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetFlights() => throw new NotImplementedException();
    }
}
