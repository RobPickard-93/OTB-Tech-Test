using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Models;
using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;

namespace HolidaySearch.Commands
{
    public class HolidaySearchCommand : IHolidaySearchCommand
    {
        private readonly IFlightSearchCommand _findFlightsCommand;
        private readonly IHotelSearchCommand _findHotelsCommand;

        public HolidaySearchCommand(IHotelSearchCommand findHotelsCommand, IFlightSearchCommand findFlightsCommand)
        {
            _findHotelsCommand = findHotelsCommand;
            _findFlightsCommand = findFlightsCommand;
        }

        public async Task<Result<HolidaySearchResponse>> Execute(HolidaySearchRequest request)
        {
            var result = new Result<HolidaySearchResponse>();

            var flightResult = await _findFlightsCommand.Execute(
                new FlightSearchRequest
                {
                    DepartureDate = request.DepartureDate,
                    To = request.TravellingTo,
                    From = request.DepartingFrom
                }
            );

            if (!flightResult.IsSuccessful) 
            {
                result.IsSuccessful = false;
                result.Message = flightResult.Message;
                return result;
            }

            var hotelResult = await _findHotelsCommand.Execute(
                new HotelSearchRequest
                {
                    LocalAirports = flightResult!.SearchResults.Select(f => f.To),
                    ArrivalDate = request.DepartureDate,
                    Duration = request.Duration
                }
            );

            if (!hotelResult.IsSuccessful) 
            {
                result.IsSuccessful = false;
                result.Message = hotelResult.Message;
                return result;
            }

            var reponses = new List<HolidaySearchResponse>();

            foreach(var hotel in hotelResult.SearchResults)
            {
                reponses.AddRange(
                    flightResult.SearchResults.Where(flight => hotel.LocalAirports.Contains(flight.To))
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
