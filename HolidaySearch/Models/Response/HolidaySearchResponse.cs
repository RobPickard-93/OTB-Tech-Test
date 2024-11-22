namespace HolidaySearch.Models.Response
{
    public class HolidaySearchResponse
    {
        public int FightId { get; set; }
        public int HotelId { get; set; }
        public string DepartingFrom { get; set; } = string.Empty;
        public string TravellingTo { get; set; } = string.Empty;
        public decimal FlightPrice { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public decimal HotelPrice { get; set; }
        public decimal TotalPrice => FlightPrice + HotelPrice;
    }
}
