namespace HolidaySearch.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }
        public decimal PricePerNight { get; set; }
        public IList<string> LocalAirports { get; set; } = [];
        public int Nights { get; set; }
    }

    // ASSUMPTION - PricePerNight is decimal, test data only holds whole numbers
    // ASSUMPTION - ArrivalDate date holds offsets, this becomes important when BST could determine the difference between 11pm and midnight
    // TODO - Consider renaming Nights to something more clear
}
