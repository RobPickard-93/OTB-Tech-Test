namespace HolidaySearch.Models
{
    public class Result<TResponse>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<TResponse> SearchResults { get; set; } = [];
    }
}
