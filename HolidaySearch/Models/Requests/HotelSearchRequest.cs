namespace HolidaySearch.Models.Requests
{
    public class HotelSearchRequest
    {
        public IEnumerable<string> LocalAirports { get; set; } = [];
        public DateTimeOffset ArrivalDate { get; set; }
        public int Duration { get; set; }
    }
}
