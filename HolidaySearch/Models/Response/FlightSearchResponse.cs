namespace HolidaySearch.Models.Response
{
    public class FlightSearchResponse
    {
        public int Id { get; set; }
        public string Airline { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
    }
}
