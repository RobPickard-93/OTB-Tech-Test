﻿namespace HolidaySearch.Models.Entity
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset ArrivalDate { get; set; }
        public decimal PricePerNight { get; set; }
        public IList<string> LocalAirports { get; set; } = [];
        public int Nights { get; set; }
    }
}
