using HolidaySearch.Commands;
using HolidaySearch.Interfaces.Repositories;
using HolidaySearch.Models;
using HolidaySearch.Models.Entity;
using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;
using Moq;
using System.Text.Json;

namespace HolidaySearchTests.IntegrationTests
{
    public class HolidaySearchCommandTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new Mock<IHotelRepository>();
        private readonly Mock<IFlightRepository> _flightRepositoryMock = new Mock<IFlightRepository>();

        [SetUp]
        public void Setup()
        {
            _hotelRepositoryMock.Setup(x => x.GetHotels())
                .Returns(
                    Task.FromResult(
                        GetJsonTestData<Hotel>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/TestData", "hotels.json"))));
            _flightRepositoryMock.Setup(x => x.GetFlights())
                .Returns(
                    Task.FromResult(
                        GetJsonTestData<Flight>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/TestData", "flights.json"))));
        }

        public static IEnumerable<TestCaseData> TestCases =
        [
            new TestCaseData(
                new HolidaySearchRequest
                {
                    DepartingFrom = "MAN",
                    TravellingTo = "AGP",
                    DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.FromHours(1)),
                    Duration = 7
                },
                new Result<HolidaySearchResponse> {
                    IsSuccessful = true,
                    SearchResults = [
                        new HolidaySearchResponse 
                        {
                            FightId = 2,
                            DepartingFrom = "MAN",
                            TravellingTo = "AGP",
                            FlightPrice = 245,
                            HotelId = 9,
                            HotelName = "Nh Malaga",
                            HotelPrice = 83 * 7
                        }
                    ]
                }
            )
        ];

        [TestCaseSource(nameof(TestCases))]
        [Theory]
        public async Task Search_Returns_Results_When_Provided_With_Valid_Search_Terms(HolidaySearchRequest request, Result<HolidaySearchResponse> expectedResult)
        {
            var flightSearchCommand = new FlightSearchCommand(_flightRepositoryMock.Object);
            var hotelSearchCommand = new HotelSearchCommand(_hotelRepositoryMock.Object);
            var holidaySearchCommand = new HolidaySearchCommand(hotelSearchCommand, flightSearchCommand);

            var result = await holidaySearchCommand.Execute(request);

            Assert.That(result.IsSuccessful, Is.True);
            Assert.That(result.SearchResults.First().HotelId, Is.EqualTo(expectedResult.SearchResults.First().HotelId));
            Assert.That(result.SearchResults.First().FightId, Is.EqualTo(expectedResult.SearchResults.First().FightId));
            Assert.That(result.SearchResults.First().TotalPrice, Is.EqualTo(expectedResult.SearchResults.First().TotalPrice));
        }

        private static IEnumerable<T> GetJsonTestData<T>(string path)
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<IEnumerable<T>>(json, options) ?? Enumerable.Empty<T>();
        }

    }
}
