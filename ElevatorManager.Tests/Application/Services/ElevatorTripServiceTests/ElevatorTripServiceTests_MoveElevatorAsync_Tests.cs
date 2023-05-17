using ElevatorManager.Application.Commons;
using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;
using ElevatorManager.Tests.Helpers;

using Moq;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripServiceTests_MoveElevatorAsync_Tests
    {

        [Fact]
        public async Task Should_ReturnError_When_FloorLessThanZero()
        {
            // Arrange
            var dateTimeServiceMock = new Mock<IDateTimeService>();

            var repositoryMock = new Mock<IElevatorTripRepository>();

            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);

            var request = new MoveElevatorRequest(-1);

            // Act
            var result = await service.MoveElevatorAsync(request, default);

            // Assert

            Assert.Equal(ErrorMessages.FloorMustBeGratherThanZero, result.Error);
        }

        [Fact]
        public async Task Should_ReturnError_When_ElevatorAlreadyOnRequestedFloor()
        {
            // Arrange
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<IElevatorTripRepository>();
            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(0);

            // Act
            var result = await service.MoveElevatorAsync(request, default);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.ElevatorAlreadyOnRequestedFloor, result.Error);
        }


        [Fact]
        public async Task Should_ReturnError_When_ButtonAlreadyPressed()
        {
            // Arrange

            DateTime requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



            var repositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 10, Priority.High)
                {
                    Id = Guid.NewGuid()
                },

            };

            repositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);


            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(10);

            // Act

            var result = await service.MoveElevatorAsync(request, Priority.High);

            // Assert

            Assert.Equal(ErrorMessages.ButtonAlreadyPressed, result.Error);
        }


        [Fact]
        public async Task Should_ReturnSuccessResultWithElevatorTripDto_When_DontHaveTrips()
        {
            // Arrange

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(FakeValues.RequestTime);



            var repositoryMock = new Mock<IElevatorTripRepository>();

            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);

            var request = new MoveElevatorRequest(1);



            // Act

            var result = await service.MoveElevatorAsync(request, default);

            // Assert

            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.IsType<ElevatorTripDto>(result.Value);
        }


        [Fact]
        public async Task Should_ReturnSuccessWithSameNumberTrip_When_HaveTripsAndPendientFloors()
        {
            // Arrange

            var requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



            var repositoryMock = new Mock<IElevatorTripRepository>();


            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 10, default)
                {
                    Id = Guid.NewGuid()
                },

            };


            repositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);



            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);

            var request = new MoveElevatorRequest(5);



            // Act

            var result = await service.MoveElevatorAsync(request, default);

            // Assert

            Assert.Equal(1, result.Value.NumberTrip);
        }


        [Fact]
        public async Task Should_ReturnSuccessWithNewNumberTrip_When_HaveTripsAndDontHavePendientFloors()
        {
            // Arrange

            var requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



            var repositoryMock = new Mock<IElevatorTripRepository>();


            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 4, default)
                {
                    Id = Guid.NewGuid()
                },

            };


            repositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);



            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);

            var request = new MoveElevatorRequest(5);



            // Act

            var result = await service.MoveElevatorAsync(request, default);

            // Assert

            Assert.Equal(2, result.Value.NumberTrip);
        }

        [Fact]
        public async Task Should_ReturnSuccessWithNumberTripAsOne_When_DontHaveTrips()
        {
            // Arrange

            var requestTime = FakeValues.RequestTime;

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(requestTime.AddSeconds(FakeValues.SecondsThatPassed));



            var repositoryMock = new Mock<IElevatorTripRepository>();


            var trips = new List<ElevatorTrip>();


            repositoryMock.Setup(m => m.GetLastTripsAsync())
                .ReturnsAsync(trips);



            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);

            var request = new MoveElevatorRequest(5);



            // Act

            var result = await service.MoveElevatorAsync(request, default);

            // Assert

            Assert.Equal(1, result.Value.NumberTrip);
        }

    }
}
