namespace HolidaySearch.Models.Requests
{
    public class HolidaySearchRequest
    {
        public string DepartingFrom { get; set; } = string.Empty;
        public string TravellingTo { get; set; } = string.Empty;
        public DateTimeOffset DepartureDate { get; set; }
        public int Duration { get; set; }
    }
}
