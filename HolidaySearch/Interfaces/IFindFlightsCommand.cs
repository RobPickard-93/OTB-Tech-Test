using HolidaySearch.Models;

namespace HolidaySearch.Interfaces
{
    public interface IFindFlightsCommand
    {
        Task<IEnumerable<Flight>> Execute(string from, string to, DateTimeOffset departureDate);
    }
}
