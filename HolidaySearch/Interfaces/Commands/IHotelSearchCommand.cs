using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;

namespace HolidaySearch.Interfaces.Commands
{
    public interface IHotelSearchCommand : ICommand<HotelSearchRequest, HotelSearchResponse>
    {
    }
}
