namespace HolidaySearch.Models.Requests
{
    public class FlightSearchRequest
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTimeOffset DepartureDate { get; set; }
    }
}
