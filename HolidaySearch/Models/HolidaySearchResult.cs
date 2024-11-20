namespace HolidaySearch.Models
{
    public class HolidaySearchResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<HolidaySearchResponse> SearchResults { get; set; } = [];
    }
}
