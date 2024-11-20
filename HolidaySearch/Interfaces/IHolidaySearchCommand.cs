using HolidaySearch.Models;

namespace HolidaySearch.Interfaces
{
    public interface IHolidaySearchCommand
    {
        Task<HolidaySearchResult> Execute(HolidaySearchRequest request);
    }
}
