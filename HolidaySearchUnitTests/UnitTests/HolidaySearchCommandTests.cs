using HolidaySearch;
using HolidaySearch.Commands;
using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Models;
using HolidaySearch.Models.Requests;
using HolidaySearch.Models.Response;
using Moq;

namespace HolidaySearchTests.UnitTests
{
    public class HolidaySearchCommandTests
    {
        private Mock<IFlightSearchCommand> _findFlightsCommandMock = new Mock<IFlightSearchCommand>(MockBehavior.Strict);
        private Mock<IHotelSearchCommand> _findHotelsCommandMock = new Mock<IHotelSearchCommand>(MockBehavior.Strict);
        private IHolidaySearchCommand? _holidaySearchCommand;

        [SetUp]
        public void Setup()
        {
            IEnumerable<FlightSearchResponse> flightSubjects = new List<FlightSearchResponse>
            {
                 new FlightSearchResponse
                {
                    Id = 1,
                    Airline = "Test Flight",
                    DepartureDate = DateTimeOffset.UtcNow.Date,
                    From = "MAN",
                    To = "NEW YORK",
                    Price = 100
                }
            };

            _findFlightsCommandMock
                .Setup(m => m.Execute(It.IsAny<FlightSearchRequest>()))
                .Returns(() => Task.FromResult(
                    new Result<FlightSearchResponse>
                    {
                        IsSuccessful = true,
                        SearchResults = flightSubjects
                    }));

            IEnumerable<HotelSearchResponse> hotelSubjects = new List<HotelSearchResponse>
            {
                new HotelSearchResponse
                {
                    Id = 1,
                    ArrivalDate = DateTimeOffset.Now.Date,
                    LocalAirports = ["NEW YORK"],
                    Name = "Test Expensive Hotel",
                    Nights = 7,
                    PricePerNight = 1000
                },
                new HotelSearchResponse
                {
                    Id = 2,
                    ArrivalDate = DateTimeOffset.Now.Date,
                    LocalAirports = ["NEW YORK"],
                    Name = "Test Cheap Hotel",
                    Nights = 7,
                    PricePerNight = 10
                }
            };

            _findHotelsCommandMock
                .Setup(m => m.Execute(It.IsAny<HotelSearchRequest>()))
                .Returns(() => Task.FromResult(
                    new Result<HotelSearchResponse>
                    {
                        IsSuccessful = true,
                        SearchResults = hotelSubjects
                    }));


            _holidaySearchCommand = new HolidaySearchCommand(_findHotelsCommandMock.Object, _findFlightsCommandMock.Object);
        }

        [Test]
        public async Task Given_Valid_HolidaySearchRequest_Then_Results_Returned_Ordered_By_TotalPrice()
        {
            // Arrange
            var expectedSuccess = true;
            var expectedResultCount = 2;
            var expectedHotelId = 2;
            var request = new HolidaySearchRequest
            {
                DepartingFrom = "MAN",
                TravellingTo = "NEW YORK",
                DepartureDate = DateTimeOffset.UtcNow.Date,
                Duration = 7
            };

            // Act
            var result = await _holidaySearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(expectedSuccess, Is.EqualTo(result.IsSuccessful));
            Assert.That(expectedResultCount, Is.EqualTo(result.SearchResults.Count()));
            Assert.That(expectedHotelId, Is.EqualTo(result.SearchResults.First().HotelId));
        }

        [Test]
        public async Task Given_Valid_HolidaySearchRequest_Then_Results_TotalPrice_Is_Sum_Of_Hotel_Price_And_Flight_Price()
        {
            // Arrange
            var request = new HolidaySearchRequest
            {
                DepartingFrom = "MAN",
                TravellingTo = "NEW YORK",
                DepartureDate = DateTimeOffset.UtcNow.Date,
                Duration = 7
            };

            // Act
            var result = await _holidaySearchCommand!.Execute(request);
            var topResult = result.SearchResults.First();
            var expectedTotalPrice = topResult.HotelPrice + topResult.FlightPrice;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(topResult.TotalPrice, Is.EqualTo(expectedTotalPrice));
        }

        [Test]
        public async Task Given_Valid_HolidaySearchRequest_When_No_Flights_Found_Then_Returns_Error()
        {
            // Arrange
            var expectedSuccess = false;
            var request = new HolidaySearchRequest();
            var expectedErrorMessage = Constants.NoFlightsFoundError;
            var expectedSearchResultsCount = 0;

            _findFlightsCommandMock
                .Setup(m => m.Execute(It.IsAny<FlightSearchRequest>()))
                .Returns(() => Task.FromResult(new Result<FlightSearchResponse>
                {
                    IsSuccessful = false,
                    Message = expectedErrorMessage
                }));

            // Act
            var result = await _holidaySearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.Message, Is.EqualTo(expectedErrorMessage));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedSearchResultsCount));
        }

        [Test]
        public async Task Given_Valid_HolidaySearchRequest_When_No_Hotels_Found_Then_Returns_Error()
        {
            // Arrange
            var expectedSuccess = false;
            var request = new HolidaySearchRequest();
            var expectedSearchResultsCount = 0;
            var expectedErrorMessage = Constants.NoHotelsFoundError;

            _findHotelsCommandMock
                .Setup(m => m.Execute(It.IsAny<HotelSearchRequest>()))
                .Returns(() => Task.FromResult(new Result<HotelSearchResponse>
                {
                    IsSuccessful = false,
                    Message = expectedErrorMessage
                }));

            // Act
            var result = await _holidaySearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.Message, Is.EqualTo(expectedErrorMessage));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedSearchResultsCount));
        }
    }
}
