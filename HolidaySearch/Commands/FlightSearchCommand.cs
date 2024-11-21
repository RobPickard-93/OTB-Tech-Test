using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Interfaces.Repositories;
using HolidaySearch.Models;
using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;

namespace HolidaySearch.Commands
{
    public class FlightSearchCommand : IFlightSearchCommand
    {
        private readonly IFlightRepository _flightRepository;

        public FlightSearchCommand(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<Result<FlightSearchResponse>> Execute(FlightSearchRequest request)
        {
            var result = new Result<FlightSearchResponse>();

            var allFlights = await _flightRepository.GetFlights();

            var matchingFlights = allFlights.Where(f =>
                f.From == request.From &&
                f.To == request.To &&
                f.DepartureDate == request.DepartureDate);

            if(!matchingFlights.Any())
            {
                result.IsSuccessful = false;
                result.Message = Constants.NoFlightsFoundError;
            }
            else
            {
                result.IsSuccessful = true;
                result.SearchResults = matchingFlights.Select(f => new FlightSearchResponse
                {
                    Id = f.Id,
                    Airline = f.Airline,
                    DepartureDate = f.DepartureDate,
                    From = f.From,
                    Price = f.Price,
                    To = f.To
                });
            }

            return result;
        }
    }
}
