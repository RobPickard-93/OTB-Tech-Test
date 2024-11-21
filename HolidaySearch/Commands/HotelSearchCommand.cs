using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Interfaces.Repositories;
using HolidaySearch.Models;
using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;

namespace HolidaySearch.Commands
{
    public class HotelSearchCommand : IHotelSearchCommand
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelSearchCommand(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelSearchResponse>> Execute(HotelSearchRequest request)
        {
            var result = new Result<HotelSearchResponse>();

            var allHotels = await _hotelRepository.GetHotels(request.LocalAirports, request.ArrivalDate, request.Duration);

            if (!allHotels.Any())
            {
                result.IsSuccessful = false;
                result.Message = Constants.NoHotelsFoundError;
            }
            else
            {
                result.IsSuccessful = true;
                result.SearchResults = allHotels.Select(h => new HotelSearchResponse
                {
                    Id = h.Id,
                    ArrivalDate = h.ArrivalDate,
                    LocalAirports = h.LocalAirports,
                    Name = h.Name,
                    Nights = h.Nights,
                    PricePerNight = h.PricePerNight
                });
            }

            return result;
        }
    }
}
