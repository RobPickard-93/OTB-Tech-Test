using HolidaySearch;
using HolidaySearch.Commands;
using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Interfaces.Repositories;
using HolidaySearch.Models.Entity;
using HolidaySearch.Models.Requests;
using Moq;

namespace HolidaySearchTests.UnitTests
{
    public class FlightSearchCommandTests
    {
        private Mock<IFlightRepository> _flightRepoMock = new Mock<IFlightRepository>(MockBehavior.Strict);
        private IFlightSearchCommand? _flightSearchCommand;

        [SetUp]
        public void Setup()
        {
            IEnumerable<Flight> subjects =
            [
                new Flight
                {
                    Id = 1,
                    Airline = "First Class Air",
                    From = "MAN",
                    To = "TFS",
                    Price = 470,
                    DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.Zero)
                },
                new Flight
                {
                    Id = 2,
                    Airline = "Oceanic Airlines",
                    From = "MAN",
                    To = "TFS",
                    Price = 245,
                    DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.Zero)
                },
                new Flight
                {
                    Id = 3,
                    Airline = "Trans American Airlines",
                    From = "MAN",
                    To = "TFS",
                    Price = 170,
                    DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.Zero)
                }
            ];

            _flightRepoMock.Setup(x => x.GetFlights())
                .Returns(() => Task.FromResult(subjects));

            _flightSearchCommand = new FlightSearchCommand(_flightRepoMock.Object);
        }

        [Test]
        public async Task Given_FlightSearchRequest_With_Flights_Found_Then_Results_Returned()
        {
            // Arrange
            var expectedSuccess = true;
            var expectedResultCount = 3;
            var request = new FlightSearchRequest
            {
                From = "MAN",
                To = "TFS",
                DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.Zero)
            };

            // Act
            var result = await _flightSearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedResultCount));
        }

        [Test]
        public async Task Given_FlightSearchRequest_When_No_Flights_Found_Then_Error_Returned()
        {
            // Arrange
            IEnumerable<Flight> subjects = Enumerable.Empty<Flight>();
            _flightRepoMock.Setup(x => x.GetFlights())
                .Returns(() => Task.FromResult(subjects));

            var expectedSuccess = false;
            var expectedResultCount = 0;
            var request = new FlightSearchRequest
            {
                From = "MAN",
                To = "TFS",
                DepartureDate = new DateTimeOffset(2023, 07, 01, 0, 0, 0, TimeSpan.Zero)
            };

            // Act
            var result = await _flightSearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedResultCount));
            Assert.That(result.Message, Is.EqualTo(Constants.NoFlightsFoundError));
        }
    }
}
