using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;

namespace ElevatorManager.Application.Mappings
{
    public static class ElevatorTripMappingExtensions
    {
        public static ElevatorTripDto ConvertToDto(this ElevatorTrip elevatorTrip)
            => new(elevatorTrip.Id,
                elevatorTrip.RequestTime,
                elevatorTrip.NumberTrip,
                elevatorTrip.DestinationFloor,
                elevatorTrip.Priority);
    }
}
