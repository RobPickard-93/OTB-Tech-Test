namespace HolidaySearch.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Airline { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
    }
}
// ASSUMPTION - price is decimal, test data only holds whole numbers
// ASSUMPTION - departure date holds offsets, this becomes important when BST could determine the difference between 11pm and midnight
