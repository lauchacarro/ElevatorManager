using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;

using Moq;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripService_CollectAllFloorsFromTrips_Tests
    {
        [Fact]
        public void Success()
        {
            int secondsLater = 8;

            DateTime firstRequestTime = new DateTime(2023, 05, 18, 12, 20, 13);

            var dateTimeServiceMock = new Mock<IDateTimeService>();

            dateTimeServiceMock.Setup(m => m.GetNow()).Returns(firstRequestTime.AddSeconds(secondsLater));

            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            // Arrange
            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(DateTime.Now, 0, 5, Priority.High)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(DateTime.Now, 0, 3, Priority.Low)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(DateTime.Now, 0, 7, Priority.Low)
                {
                    Id = Guid.NewGuid()
                }
            };

            var expectedFloors = new List<TripFloor>
            {
                new TripFloor(trips[0].Id, 5),
                new TripFloor(trips[0].Id, 4),
                new TripFloor(trips[1].Id, 3),
                new TripFloor(trips[1].Id, 4),
                new TripFloor(trips[1].Id, 5),
                new TripFloor(trips[1].Id, 6),
                new TripFloor(trips[2].Id, 7)
            };

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);


            var actualFloors = ElevatorTripService.CollectAllFloorsFromTrips(trips);

            for (var i = 0; i < actualFloors.Count; i++)
            {
                Assert.Equal(expectedFloors[i].TripId, actualFloors[i].TripId);
                Assert.Equal(expectedFloors[i].Floor, actualFloors[i].Floor);
            }

        }
    }
}
