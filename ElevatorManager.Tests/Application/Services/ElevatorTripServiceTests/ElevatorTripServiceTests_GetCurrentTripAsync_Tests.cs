using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;
using ElevatorManager.Tests.Helpers;

using Moq;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripServiceTests_GetCurrentTripAsync_Tests
    {
        [Fact]
        public async Task Should_ReturnCurrentAndPendientFloors_When_SecondsThatPassedIsLessThanTotalFloors()
        {
            // Arrange

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));




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



            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync(prevTrips);

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);


            // Act


            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(6, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 7 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task Should_ReturnCurrentAndPendientFloors_When_SecondsThatPassedIsLessThanTotalFloorsAndDontHavePrevTrips()
        {
            // Arrange

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



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


            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);


            // Act


            var actual = await service.GetCurrentTripAsync();


            // Assert

            Assert.Equal(4, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 5, 6, 7 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task Should_ReturnCurrentFloorIsTheLastOfPrevTrip_When_SecondsThatPassedIsZeroAndHaveTwoTrips()
        {
            // Arrange

            const int SecondsThatPassed = FakeValues.Zero;

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(SecondsThatPassed));



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


            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync(prevTrips);

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);


            // Act


            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(8, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 9, 10 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task Should_ReturnCurrentFloorIsZero_When_SecondsThatPassedIsZeroAndHaveOnlyOneTrip()
        {
            // Arrange

            const int SecondsThatPassed = FakeValues.Zero;

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(SecondsThatPassed));



            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 2, default)
                {
                    Id = Guid.NewGuid()
                },
            };


            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);


            // Act


            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(0, actual!.Value.CurrentFloor);
            Assert.Equal(new int[] { 1, 2 }, actual!.Value.PendientFloors);
        }

        [Fact]
        public async Task Should_ReturnCurrentFloorIsTheLast_When_SecondsThatPassedIsGratherThanTotalFloorsAndHaveTwoTrips()
        {
            // Arrange

            const int SecondsThatPassed = 20;

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(SecondsThatPassed));



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



            elevatorTripRepositoryMock.Setup(m => m.GetTripsByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync(prevTrips);

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);


            // Act


            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(10, actual!.Value.CurrentFloor);
            Assert.Empty(actual!.Value.PendientFloors);
        }



        [Fact]
        public async Task Should_ReturnCurrentFloorIsZero_When_DontHaveTrips()
        {
            // Arrange

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>();

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);

            ElevatorTripService service = new(dateTimeServiceMock.Object, elevatorTripRepositoryMock.Object);

            // Act


            var actual = await service.GetCurrentTripAsync();

            // Assert

            Assert.Equal(FakeValues.Zero, actual!.Value.CurrentFloor);
            Assert.Empty(actual!.Value.PendientFloors);
        }
    }
}
