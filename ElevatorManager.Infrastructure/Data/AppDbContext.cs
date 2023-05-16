using ElevatorManager.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ElevatorManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ElevatorTrip> ElevatorTrips => Set<ElevatorTrip>();
    }
}
