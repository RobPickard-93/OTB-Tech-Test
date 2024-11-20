using HolidaySearch.Models;

namespace HolidaySearch.Interfaces
{
    public interface IFindHotelsCommand
    {
        Task<IEnumerable<Hotel>> Execute(IEnumerable<string> localAirports, DateTimeOffset arrivalDate, int duration);
    } 
}
