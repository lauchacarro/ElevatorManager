using ElevatorManager.Domain.Enums;

namespace ElevatorManager.Domain.Entities
{
    public class ElevatorTrip
    {
        public ElevatorTrip(DateTime requestTime, int numberTrip, int destinationFloor, Priority priority)
        {
            RequestTime = requestTime;
            NumberTrip = numberTrip;
            DestinationFloor = destinationFloor;
            Priority = priority;
        }

        public Guid Id { get; set; }
        public DateTime RequestTime { get; set; }
        public int NumberTrip { get; set; }
        public int DestinationFloor { get; set; }
        public Priority Priority { get; set; }

        public static readonly ElevatorTrip Default = new(default, 0, 0, Priority.High);
    }
}
