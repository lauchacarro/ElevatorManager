using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Domain.Entities;
using ElevatorManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace ElevatorManager.Tests.Infrastructure.Repositories.ElevatorTripRepositoryTests
{
    public class ElevatorTripRepository_AddAsync_Tests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ElevatorTripRepository_AddAsync_Tests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_AddElevatorTripToDatabase()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var repository = new ElevatorTripRepository(context);
                var elevatorTrip = new ElevatorTrip(default, default, default, default);

                // Act
                var result = await repository.AddAsync(elevatorTrip);

                // Assert
                Assert.Equal(elevatorTrip, result);
                Assert.NotEqual(Guid.Empty, result.Id);

                var tripsInDatabase = await context.ElevatorTrips.ToListAsync();
                Assert.Single(tripsInDatabase);
                Assert.Equal(elevatorTrip, tripsInDatabase.First());
            }
        }
    }
}
