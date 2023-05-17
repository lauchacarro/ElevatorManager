using AutoFixture.Xunit2;

using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Infrastructure.Repositories;
using ElevatorManager.Tests.Helpers;

namespace ElevatorManager.Tests.Infrastructure.Repositories.ElevatorTripRepositoryTests
{
    public class ElevatorTripRepository_GetTripsByNumberAsync_Tests
    {


        [Theory, AutoData]
        public async Task Should_ReturnTripsWithGivenNumberTrip_When_HaveALotOfTrips(int numberTrip)
        {
            // Arrange

            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);

            var tripFake1 = new ElevatorTrip(FakeValues.RequestTime, numberTrip, FakeValues.Zero, Priority.Low);

            var tripFake2 = new ElevatorTrip(FakeValues.RequestTime, FakeValues.Zero, FakeValues.Zero, Priority.Low);


            await context.ElevatorTrips.AddRangeAsync(tripFake1, tripFake2);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTripsByNumberAsync(numberTrip);

            // Assert

            Assert.Equal(tripFake1.Id, result.ElementAt(0).Id);
        }

        [Theory, AutoData]
        public async Task Should_ReturnEmptyList_When_DontHaveTripsWithGivenNumber(int numberTrip)
        {
            // Arrange

            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);

            var tripFake1 = new ElevatorTrip(FakeValues.RequestTime, FakeValues.Zero, FakeValues.Zero, Priority.Low);

            var tripFake2 = new ElevatorTrip(FakeValues.RequestTime, FakeValues.Zero, FakeValues.Zero, Priority.Low);


            await context.ElevatorTrips.AddRangeAsync(tripFake1, tripFake2);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTripsByNumberAsync(numberTrip);

            // Assert

            Assert.Empty(result);
        }

    }
}
