using ElevatorManager.Domain.Enums;

namespace ElevatorManager.Domain.Dtos
{
    public record ElevatorTripDto(
        Guid Id,
        DateTime RequestTime,
        int NumberTrip,
        int DestinationFloor,
        Priority Priority);
}
