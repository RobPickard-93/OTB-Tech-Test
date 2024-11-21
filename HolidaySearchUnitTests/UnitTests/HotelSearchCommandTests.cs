using HolidaySearch;
using HolidaySearch.Commands;
using HolidaySearch.Interfaces.Commands;
using HolidaySearch.Interfaces.Repositories;
using HolidaySearch.Models.Entity;
using HolidaySearch.Models.Requests;
using Moq;

namespace HolidaySearchTests.UnitTests
{
    public class HotelSearchCommandTests
    {
        private Mock<IHotelRepository> _hotelRepoMock = new Mock<IHotelRepository>(MockBehavior.Strict);
        private IHotelSearchCommand? _hotelSearchCommand;

        [SetUp]
        public void Setup()
        {
            IEnumerable<Hotel> subjects =
            [
                new Hotel
                {
                    Id = 1,
                    Name = "Iberostar Grand Portals Nous",
                    ArrivalDate = new DateTimeOffset(2022, 11, 05, 0, 0, 0, TimeSpan.Zero),
                    PricePerNight = 100,
                    LocalAirports = ["TFS"],
                    Nights = 7
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Laguna Park 2",
                    ArrivalDate = new DateTimeOffset(2022, 11, 05, 0, 0, 0, TimeSpan.Zero),
                    PricePerNight = 50,
                    LocalAirports = ["TFS"],
                    Nights = 7
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Sol Katmandu Park & Resort",
                    ArrivalDate = new DateTimeOffset(2022, 11, 05, 0, 0, 0, TimeSpan.Zero),
                    PricePerNight = 59,
                    LocalAirports = ["TFS"],
                    Nights = 7
                }
            ];

            _hotelRepoMock.Setup(x => x.GetHotels())
                .Returns(() => Task.FromResult(subjects));

            _hotelSearchCommand = new HotelSearchCommand(_hotelRepoMock.Object);
        }

        [Test]
        public async Task Given_HotelSearchRequest_With_Matching_Terms_Then_Results_Returned()
        {
            // Arrange
            var expectedSuccess = true;
            var expectedResultCount = 3;
            var request = new HotelSearchRequest
            {
                LocalAirports = ["TFS"],
                ArrivalDate = new DateTimeOffset(2022, 11, 05, 0, 0, 0, TimeSpan.Zero),
                Duration = 7
            };

            // Act
            var result = await _hotelSearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedResultCount));
        }

        [Test]
        public async Task Given_HotelSearchRequest_With_No_Matching_Terms_Then_Error_Returned()
        {
            // Arrange
            IEnumerable<Hotel> subjects = Enumerable.Empty<Hotel>();
            _hotelRepoMock.Setup(x => x.GetHotels())
                .Returns(() => Task.FromResult(subjects));

            var expectedSuccess = false;
            var expectedResultCount = 0;
            var request = new HotelSearchRequest
            {
                LocalAirports = ["TFS"],
                ArrivalDate = new DateTimeOffset(2022, 11, 05, 0, 0, 0, TimeSpan.Zero),
                Duration = 7
            };

            // Act
            var result = await _hotelSearchCommand!.Execute(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccessful, Is.EqualTo(expectedSuccess));
            Assert.That(result.SearchResults.Count(), Is.EqualTo(expectedResultCount));
            Assert.That(result.Message, Is.EqualTo(Constants.NoHotelsFoundError));
        }
    }
}
