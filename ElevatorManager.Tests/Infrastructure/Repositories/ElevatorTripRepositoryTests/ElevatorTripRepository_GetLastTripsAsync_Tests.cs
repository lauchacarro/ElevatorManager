using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Infrastructure.Repositories;

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
        private readonly DbContextOptions<AppDbContext> _options;

        public ElevatorTripRepository_GetLastTripsAsync_Tests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetLastTripsAsync_Should_ReturnLastTripsInOrder()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {

                DateTime requestTime = new DateTime(2023, 05, 18, 12, 20, 13);

                var elevatorTrip1 = new ElevatorTrip(requestTime, 2, default, Priority.Low);

                var elevatorTrip2 = new ElevatorTrip(requestTime.AddSeconds(5), 2, default, Priority.High);

                var elevatorTrip3 = new ElevatorTrip(requestTime.AddSeconds(2), 2, default, Priority.High);

                var elevatorTrip4 = new ElevatorTrip(requestTime.AddSeconds(2), 1, default, Priority.High);

                await context.ElevatorTrips.AddRangeAsync(elevatorTrip1, elevatorTrip2, elevatorTrip3, elevatorTrip4);

                await context.SaveChangesAsync();

                // Act

                var repository = new ElevatorTripRepository(context);

                var result = await repository.GetLastTripsAsync();

                // Assert
                var expectedTrips = new List<ElevatorTrip> { elevatorTrip3, elevatorTrip2, elevatorTrip1 };



                for (var i = 0; i < result.Count(); i++)
                {
                    Assert.Equal(expectedTrips[i].Id, result.ElementAt(i).Id);
                }
            }
        }
    }
}
