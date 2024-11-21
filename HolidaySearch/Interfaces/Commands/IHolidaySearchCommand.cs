using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;

namespace HolidaySearch.Interfaces.Commands
{
    public interface IHolidaySearchCommand : ICommand<HolidaySearchRequest, HolidaySearchResponse>
    {
    }
}
