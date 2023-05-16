using ElevatorManager.Domain.Entities;

namespace ElevatorManager.Domain.Repositories
{
    public interface IElevatorTripRepository
    {
        Task<IEnumerable<ElevatorTrip>> GetTripsByNumberAsync(int numberTrip);
        Task<IEnumerable<ElevatorTrip>> GetLastTripsAsync();
        Task<ElevatorTrip> AddAsync(ElevatorTrip elevatorTrip);
    }
}
