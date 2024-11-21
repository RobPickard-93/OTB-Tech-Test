using HolidaySearch.Models.Entity;

namespace HolidaySearch.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetHotels(IEnumerable<string> localAirports, DateTimeOffset arrivalDate, int duration) => throw new NotImplementedException();
    }
}
