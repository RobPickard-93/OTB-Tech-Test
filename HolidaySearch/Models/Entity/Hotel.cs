using System.Text.Json.Serialization;

namespace HolidaySearch.Models.Entity
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("arrival_date")]
        public DateTimeOffset ArrivalDate { get; set; }
        [JsonPropertyName("price_per_night")]
        public decimal PricePerNight { get; set; }
        [JsonPropertyName("local_airports")]
        public IList<string> LocalAirports { get; set; } = [];
        public int Nights { get; set; }
    }
}
