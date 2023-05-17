using AutoFixture;

using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Infrastructure.Repositories;
using ElevatorManager.Tests.Helpers;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorManager.Tests.Infrastructure.Repositories.ElevatorTripRepositoryTests
{
    public class ElevatorTripRepository_GetLastTripsAsync_Tests
    {

        [Fact]
        public async Task Should_ReturnTrips_When_HaveTrips()
        {
            // Arrange

            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);


            var tripFake1 = new ElevatorTrip(FakeValues.RequestTime, FakeValues.Zero, FakeValues.Zero, Priority.Low);

            await context.ElevatorTrips.AddAsync(tripFake1);

            await context.SaveChangesAsync();

            // Act

            var result = await repository.GetLastTripsAsync();

            // Assert

            var expectedTrips = new List<ElevatorTrip> { tripFake1 };


            Assert.Collection(expectedTrips,
                expectedTrip1 => Assert.Equal(expectedTrip1.Id, result.ElementAt(0).Id)
            );
        }


        [Fact]
        public async Task Should_ReturnTripsInOrder_When_HaveALotOfTrips()
        {
            // Arrange

            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);


            DateTime requestTime = FakeValues.RequestTime;
     

            var tripFake1 = new ElevatorTrip(requestTime,2, FakeValues.Zero, Priority.Low);

            var tripFake2 = new ElevatorTrip(requestTime.AddSeconds(5), 2, FakeValues.Zero, Priority.High);

            var tripFake3 = new ElevatorTrip(requestTime.AddSeconds(2), 2, FakeValues.Zero, Priority.High);

            var tripFake4 = new ElevatorTrip(requestTime.AddSeconds(2), 1, FakeValues.Zero, Priority.High);

            await context.ElevatorTrips.AddRangeAsync(tripFake1, tripFake2, tripFake3, tripFake4);

            await context.SaveChangesAsync();

            // Act

            var result = await repository.GetLastTripsAsync();

            // Assert

            var expectedTrips = new List<ElevatorTrip> { tripFake3, tripFake2, tripFake1 };


            Assert.Collection(expectedTrips,
                expectedTrip1 => Assert.Equal(expectedTrip1.Id, result.ElementAt(0).Id),
                expectedTrip2 => Assert.Equal(expectedTrip2.Id, result.ElementAt(1).Id),
                expectedTrip3 => Assert.Equal(expectedTrip3.Id, result.ElementAt(2).Id)
            );
        }

        [Fact]
        public async Task Should_ReturnEmptyList_When_DontHaveTrips()
        {
            // Arrange

            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);

            // Act

            var result = await repository.GetLastTripsAsync();

            // Assert

            Assert.Empty(result);
        }
    }
}
