using ElevatorManager.Infrastructure.Data;

using ElevatorManager.Domain.Entities;
using ElevatorManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ElevatorManager.Tests.Helpers;
using AutoFixture.Xunit2;

namespace ElevatorManager.Tests.Infrastructure.Repositories.ElevatorTripRepositoryTests
{
    public class ElevatorTripRepository_AddAsync_Tests
    {


        [Theory, AutoData]
        public async Task Should_ReturnSameTrip_When_AddTrip(ElevatorTrip elevatorTrip)
        {
            // Arrange
            using AppDbContext context = SetupHelpers.SetupAppDbContext();

            var repository = new ElevatorTripRepository(context);

            // Act
            var result = await repository.AddAsync(elevatorTrip);

            // Assert
            Assert.Equal(elevatorTrip, result);
 
        }
    }
}
