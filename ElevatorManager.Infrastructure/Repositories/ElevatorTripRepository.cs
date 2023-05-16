using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace ElevatorManager.Infrastructure.Repositories
{
    public class ElevatorTripRepository : IElevatorTripRepository
    {
        private readonly AppDbContext _context;

        public ElevatorTripRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ElevatorTrip> AddAsync(ElevatorTrip elevatorTrip)
        {
            await _context.AddAsync(elevatorTrip);
            await _context.SaveChangesAsync();

            return elevatorTrip;
        }

        public async Task<IEnumerable<ElevatorTrip>> GetLastTripsAsync()
        {
            return await _context.ElevatorTrips
                .Where(x => x.NumberTrip == _context.ElevatorTrips.Max(x => x.NumberTrip))
                .OrderByDescending(x => x.Priority)
                .ThenBy(x => x.RequestTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ElevatorTrip>> GetTripsByNumberAsync(int numberTrip)
        {
            return await _context.ElevatorTrips
                .Where(x => x.NumberTrip == numberTrip)
                .OrderByDescending(x => x.Priority)
                .ThenBy(x => x.RequestTime)
                .ToListAsync();
        }
    }
}
