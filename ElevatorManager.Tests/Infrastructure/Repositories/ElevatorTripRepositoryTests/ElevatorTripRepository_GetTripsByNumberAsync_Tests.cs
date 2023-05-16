using ElevatorManager.Domain.Entities;
using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace ElevatorManager.Tests.Infrastructure.Repositories.ElevatorTripRepositoryTests
{
    public class ElevatorTripRepository_GetTripsByNumberAsync_Tests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ElevatorTripRepository_GetTripsByNumberAsync_Tests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetTripsByNumberAsync_Should_ReturnTripsWithGivenNumberInOrder()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new ElevatorTripRepository(context);

                DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

                var elevatorTrip1 = new ElevatorTrip(requestTime, 1, default, Domain.Enums.Priority.Low);

                var elevatorTrip2 = new ElevatorTrip(requestTime.AddSeconds(5), 2, default, Domain.Enums.Priority.High);

                var elevatorTrip3 = new ElevatorTrip(requestTime.AddSeconds(2), 2, default, Domain.Enums.Priority.High);


                await context.ElevatorTrips.AddRangeAsync(elevatorTrip1, elevatorTrip2, elevatorTrip3);

                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetTripsByNumberAsync(1);

                // Assert
                var expectedTrips = new List<ElevatorTrip> { elevatorTrip1 };



                Assert.Equal(expectedTrips[0].Id, result.ElementAt(0).Id);

            }
        }

    }
}
