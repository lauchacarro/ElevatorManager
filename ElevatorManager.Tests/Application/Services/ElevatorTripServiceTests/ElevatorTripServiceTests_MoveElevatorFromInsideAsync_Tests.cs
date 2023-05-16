using ElevatorManager.Application.Commons;
using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;

using Moq;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripServiceTests_MoveElevatorFromInsideAsync_Tests
    {

        [Fact]
        public async Task When_InvalidFloor_Expected_FailResult()
        {
            // Arrange
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<IElevatorTripRepository>();
            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(-1);

            // Act
            var result = await service.MoveElevatorFromInsideAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.FloorGratherThanZero, result.Error);
        }

        [Fact]
        public async Task When_ElevatorAlreadyOnRequestedFloor_Expected_FailResult()
        {
            // Arrange
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<IElevatorTripRepository>();
            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(0);

            // Act
            var result = await service.MoveElevatorFromInsideAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.ElevatorAlreadyOnRequestedFloor, result.Error);
        }


        [Fact]
        public async Task When_ButtonAlreadyPressed_Expected_FailResult()
        {
            // Arrange

            int secondsLater = 2;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);


            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<IElevatorTripRepository>();

            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(5);


            dateTimeServiceMock.Setup(d => d.GetNow()).Returns(requestTime.AddSeconds(secondsLater));

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 5, Priority.High)
                {
                    Id = Guid.NewGuid()
                },

            };


            repositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);



            // Act
            var result = await service.MoveElevatorFromInsideAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorMessages.ButtonAlreadyPressed, result.Error);
        }


        [Fact]
        public async Task MoveElevatorAsync_ValidRequest_ReturnsSuccessResultWithElevatorTripDto()
        {
            // Arrange


            int secondsLater = 0;

            DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);



            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<IElevatorTripRepository>();

            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(5);

            dateTimeServiceMock.Setup(d => d.GetNow()).Returns(requestTime.AddSeconds(secondsLater));

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(requestTime, 1, 3, Priority.High)
                {
                    Id = Guid.NewGuid()
                },

            };


            repositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);


            // Act
            var result = await service.MoveElevatorFromInsideAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.IsType<ElevatorTripDto>(result.Value);
        }

    }
}
