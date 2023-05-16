using ElevatorManager.Domain.Dtos;

namespace ElevatorManager.Domain.Services
{
    public interface IElevatorTripService
    {
        Task<Result<ElevatorTripDto>> MoveElevatorFromInsideAsync(MoveElevatorRequest request);
        Task<Result<ElevatorTripDto>> MoveElevatorFromOutsideAsync(MoveElevatorRequest request);

        Task<Result<ElevatorTripCurrentStatusDto>> GetCurrentTripAsync();
    }
}
