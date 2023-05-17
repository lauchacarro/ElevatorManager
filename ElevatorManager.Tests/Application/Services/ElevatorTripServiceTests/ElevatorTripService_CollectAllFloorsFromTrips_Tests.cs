using AutoFixture.Xunit2;

using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Tests.Helpers;

using Moq;

using Xunit;

namespace ElevatorManager.Tests.Application.Services.ElevatorTripServiceTests
{
    public class ElevatorTripService_CollectAllFloorsFromTrips_Tests
    {
        [Fact]
        public void Should_ReturnAllFloorsFromTrips_When_HaveALotOfTrips()
        {

            // Arrange

            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>
            {
                new ElevatorTrip(FakeValues.Now, FakeValues.Zero, 5, Priority.High)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(FakeValues.Now, FakeValues.Zero, 3, Priority.Low)
                {
                    Id = Guid.NewGuid()
                },
                new ElevatorTrip(FakeValues.Now, FakeValues.Zero, 7, Priority.Low)
                {
                    Id = Guid.NewGuid()
                }
            };

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);


            // Act

            var actualFloors = ElevatorTripService.CollectAllFloorsFromTrips(trips);

            // Assert

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


            Assert.Collection(expectedFloors,
                expectedFloor1 =>
                {
                    Assert.Equal(expectedFloor1.TripId, actualFloors.ElementAt(0).TripId);
                    Assert.Equal(expectedFloor1.Floor, actualFloors.ElementAt(0).Floor);
                },
                expectedFloor2 =>
                {
                    Assert.Equal(expectedFloor2.TripId, actualFloors.ElementAt(1).TripId);
                    Assert.Equal(expectedFloor2.Floor, actualFloors.ElementAt(1).Floor);
                },
                expectedFloor3 =>
                {
                    Assert.Equal(expectedFloor3.TripId, actualFloors.ElementAt(2).TripId);
                    Assert.Equal(expectedFloor3.Floor, actualFloors.ElementAt(2).Floor);
                },
                expectedFloor4 =>
                {
                    Assert.Equal(expectedFloor4.TripId, actualFloors.ElementAt(3).TripId);
                    Assert.Equal(expectedFloor4.Floor, actualFloors.ElementAt(3).Floor);
                },
                expectedFloor5 =>
                {
                    Assert.Equal(expectedFloor5.TripId, actualFloors.ElementAt(4).TripId);
                    Assert.Equal(expectedFloor5.Floor, actualFloors.ElementAt(4).Floor);
                },
                expectedFloor6 =>
                {
                    Assert.Equal(expectedFloor6.TripId, actualFloors.ElementAt(5).TripId);
                    Assert.Equal(expectedFloor6.Floor, actualFloors.ElementAt(5).Floor);
                },
                expectedFloor7 =>
                {
                    Assert.Equal(expectedFloor7.TripId, actualFloors.ElementAt(6).TripId);
                    Assert.Equal(expectedFloor7.Floor, actualFloors.ElementAt(6).Floor);
                }
            );

        }

        [Theory, AutoData]
        public void Should_ReturnOneFloor_When_HaveOneTrip(ElevatorTrip elevatorTrip)
        {

            // Arrange

            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>
            {
                elevatorTrip
            };

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);


            // Act

            var actualFloors = ElevatorTripService.CollectAllFloorsFromTrips(trips);

            // Assert

            var expectedFloors = new List<TripFloor>
            {
                new TripFloor(trips[0].Id, elevatorTrip.DestinationFloor)
            };


            Assert.Single(actualFloors);

            Assert.Collection(expectedFloors,
                expectedFloor1 =>
                {
                    Assert.Equal(expectedFloor1.TripId, actualFloors.ElementAt(0).TripId);
                    Assert.Equal(expectedFloor1.Floor, actualFloors.ElementAt(0).Floor);
                }
            );
        }

        [Fact]
        public void Should_ReturnEmptyList_When_DontHaveTrips()
        {

            // Arrange

            var elevatorTripRepositoryMock = new Mock<IElevatorTripRepository>();

            var trips = new List<ElevatorTrip>();

            elevatorTripRepositoryMock.Setup(m => m.GetLastTripsAsync()).ReturnsAsync(trips);


            // Act

            var actualFloors = ElevatorTripService.CollectAllFloorsFromTrips(trips);

            // Assert


            Assert.Empty(actualFloors);

        }
    }
}
