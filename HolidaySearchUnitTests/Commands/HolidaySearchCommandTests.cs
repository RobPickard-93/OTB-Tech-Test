using HolidaySearch;
using HolidaySearch.Commands;
using HolidaySearch.Interfaces;
using HolidaySearch.Models;
using Moq;

namespace HolidaySearchUnitTests.Commands
{
    public class HolidaySearchCommandTests
    {
        private Mock<IFindFlightsCommand> _findFlightsCommandMock = new Mock<IFindFlightsCommand>(MockBehavior.Strict);
        private Mock<IFindHotelsCommand> _findHotelsCommandMock = new Mock<IFindHotelsCommand>(MockBehavior.Strict);
        private IHolidaySearchCommand? _holidaySearchCommand;

        [SetUp]
        public void Setup()
        {
            IEnumerable<Flight> flightSubjects = new List<Flight>
            {
                 new Flight
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
                .Setup(m => m.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(() => Task.FromResult(flightSubjects));

            IEnumerable<Hotel> hotelSubjects = new List<Hotel>
            {
                new Hotel
                {
                    Id = 1,
                    ArrivalDate = DateTimeOffset.Now.Date,
                    LocalAirports = ["NEW YORK"],
                    Name = "Test Expensive Hotel",
                    Nights = 7,
                    PricePerNight = 1000
                },
                new Hotel
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
                .Setup(m => m.Execute(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
                .Returns(() => Task.FromResult(hotelSubjects));


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
            Assert.That(expectedSuccess.Equals(result.IsSuccessful));
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
            Assert.That(expectedTotalPrice, Is.EqualTo(topResult.TotalPrice));
        }

        [Test]
        public async Task When_No_Flights_Found_Then_Returns_Error()
        {
            // Arrange
            IEnumerable<Flight> subjects = [];
            _findFlightsCommandMock
                .Setup(m => m.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(() => Task.FromResult(subjects));

            var expectedErrorMessage = Constants.NoFlightsFoundError;
            var expectedSuccess = false;
            var request = new HolidaySearchRequest();

            // Act
            var result = await _holidaySearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(expectedSuccess, Is.EqualTo(result.IsSuccessful));
            Assert.That(expectedErrorMessage, Is.EqualTo(result.Message));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task When_No_Hotels_Found_Then_Returns_Error()
        {
            // Arrange
            IEnumerable<Hotel> subjects = [];
            _findHotelsCommandMock
                .Setup(m => m.Execute(It.IsAny<IEnumerable<string>>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
                .Returns(() => Task.FromResult(subjects));

            var expectedErrorMessage = Constants.NoHotelsFoundError;
            var expectedSuccess = false;
            var request = new HolidaySearchRequest();

            // Act
            var result = await _holidaySearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(expectedSuccess, Is.EqualTo(result.IsSuccessful));
            Assert.That(expectedErrorMessage, Is.EqualTo(result.Message));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(0));
        }
    }
}
