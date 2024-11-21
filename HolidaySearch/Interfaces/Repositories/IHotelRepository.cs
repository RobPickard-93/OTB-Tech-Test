using HolidaySearch.Models.Entity;

namespace HolidaySearch.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetHotels() => throw new NotImplementedException();
    }
}
