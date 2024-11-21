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

        // Consider moving this, or adding an explicit test for it
        public decimal TotalPrice => FlightPrice + HotelPrice;
    }

    // ASSUMPTION - total price is hotel price + flight price, this isn't confirmed anywhere and doesn't account for any additional charges/surcharges.etc
    // ASSUMPTION - best value result is the cheapest result (ie: lowest total price), not necessarily the quickest flight or otherwise
}
