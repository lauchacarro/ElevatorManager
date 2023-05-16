using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;

using Moq;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripServiceTests_GetCurrentTripAsync_Tests
    {
        [Fact]
        public async Task When_SecondsLaterIsLessThanTotalFloors_Expect_CurrentFloorAndPendientFloors()
        {
            // Arrange

            int secondsLater = 8;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var prevTrips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 6, default)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(requestTime, 1, 8, default)
                {
                    Id = Guid.NewGuid()
                }
            };

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 2, 5, default)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(requestTime, 2, 3, default)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(requestTime, 2, 7, default)
                {
                    Id = Guid.NewGuid()
                }
            };



            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>())).ReturnsAsync(prevTrips);
            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(6, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 7 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task When_SecondsLaterIsLessThanTotalFloorsAndOnlyOneTrip_Expect_CurrentFloorAndPendientFloors()
        {
            // Arrange

            int secondsLater = 8;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();



            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 5, default)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(requestTime, 1, 3, default)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(requestTime, 1, 7, default)
                {
                    Id = Guid.NewGuid()
                }
            };


            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();


            // Assert

            Assert.Equal(4, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 5, 6, 7 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task When_SecondsLaterIsZeroAndHaveTwoTrips_Expect_CurrentFloorIsTheLastOfPrevTrip()
        {
            // Arrange

            int secondsLater = 0;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var prevTrips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 8, default)
                {
                    Id = Guid.NewGuid()
                }
            };

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 2, 10, default)
                {
                    Id = Guid.NewGuid()
                },
            };



            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>())).ReturnsAsync(prevTrips);
            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(8, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 9, 10 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task When_SecondsLaterIsZeroAndHaveOnlyOneTrip_Expect_CurrentFloorIsZero()
        {
            // Arrange

            int secondsLater = 0;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();



            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 2, default)
                {
                    Id = Guid.NewGuid()
                },
            };


            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(0, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 1, 2 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task When_SecondsLaterGratherThanTotalFloorsAndHaveTwoTrips_Expect_CurrentFloorIsTheLast()
        {
            // Arrange

            int secondsLater = 20;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var prevTrips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 8, default)
                {
                    Id = Guid.NewGuid()
                }
            };

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 2, 10, default)
                {
                    Id = Guid.NewGuid()
                },
            };



            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>())).ReturnsAsync(prevTrips);
            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(10, actual!.Value.CurrentFloor);
            Assert.Equal(Array.Empty<int>(), actual!.Value.PendientFloors);
        }



        [Fact]
        public async Task When_DontHaveTrips_Expect_CurrentFloorIsZero()
        {
            // Arrange

            int secondsLater = 8;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(requestTime.AddSeconds(secondsLater));


            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();


            var trips = new List<ElevatorTrip>
            {

            };


            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);

            // Act

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(0, actual!.Value.CurrentFloor);
            Assert.Equal(Array.Empty<int>(), actual!.Value.PendientFloors);
        }
    }
}
