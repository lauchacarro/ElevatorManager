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
    public class ElevatorTripServiceTests_MoveElevatorFromInsideAsync_Tests
    {
        [Fact]
        public async Task Should_ReturnSuccessWithPriorityHigh_When_Valid()
        {
            // Arrange

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(d => d.GetNow())
                .Returns(FakeValues.RequestTime);


            var repositoryMock = new Mock<IElevatorTripRepository>();

            var service = new ElevatorTripService(dateTimeServiceMock.Object, repositoryMock.Object);
            var request = new MoveElevatorRequest(1);

            // Act
            var result = await service.MoveElevatorFromInsideAsync(request);

            // Assert
            Assert.Equal(Priority.High, result.Value.Priority);
        }

    }
}
