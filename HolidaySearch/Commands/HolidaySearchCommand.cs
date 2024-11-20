using HolidaySearch.Interfaces;
using HolidaySearch.Models;

namespace HolidaySearch.Commands
{
    public class HolidaySearchCommand : IHolidaySearchCommand
    {
        private readonly IFindFlightsCommand _findFlightsCommand;
        private readonly IFindHotelsCommand _findHotelsCommand;

        public HolidaySearchCommand(IFindHotelsCommand findHotelsCommand, IFindFlightsCommand findFlightsCommand)
        {
            _findHotelsCommand = findHotelsCommand;
            _findFlightsCommand = findFlightsCommand;
        }

        public async Task<HolidaySearchResult> Execute(HolidaySearchRequest request)
        {
            var result = new HolidaySearchResult();

            var matchingFlights = await _findFlightsCommand.Execute(
                request.DepartingFrom, 
                request.TravellingTo, 
                request.DepartureDate
            );

            if (!matchingFlights?.Any() ?? false) 
            {
                result.IsSuccessful = false;
                result.Message = Constants.NoFlightsFoundError;
                return result;
            }

            var matchingHotels = await _findHotelsCommand.Execute(
                matchingFlights!.Select(f => f.To),
                request.DepartureDate,
                request.Duration
            );

            if (!matchingHotels?.Any() ?? false) 
            {
                result.IsSuccessful = false;
                result.Message = Constants.NoHotelsFoundError;
                return result;
            }

            var reponses = new List<HolidaySearchResponse>();

            foreach(var hotel in matchingHotels!)
            {
                reponses.AddRange(
                    matchingFlights!.Where(flight => hotel.LocalAirports.Contains(flight.To))
                    .Select(flight => new HolidaySearchResponse
                    {
                        HotelName = hotel.Name,
                        HotelId = hotel.Id,
                        DepartingFrom = flight.From,
                        TravellingTo = flight.To,
                        FightId = flight.Id,
                        FlightPrice = flight.Price,
                        HotelPrice = hotel.PricePerNight * hotel.Nights
                    }));
            }

            result.SearchResults = reponses.OrderBy(r => r.TotalPrice);
            result.IsSuccessful = true;

            return result;
        }
    }
}
