﻿
using System.Text.Json.Serialization;

namespace HolidaySearch.Models.Entity
{
    public class Flight
    {
        public int Id { get; set; }
        public string Airline { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [JsonPropertyName("departure_date")]
        public DateTimeOffset DepartureDate { get; set; }
    }
}
