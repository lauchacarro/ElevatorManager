namespace ElevatorManager.Domain.Dtos;

public record ElevatorTripCurrentStatusDto(int CurrentFloor, IEnumerable<int> PendientFloors, IEnumerable<ElevatorTripDto> Tríps)
{
    public readonly static ElevatorTripCurrentStatusDto Default = new(
                                0,
                                Enumerable.Empty<int>(),
                                Enumerable.Empty<ElevatorTripDto>());
}
