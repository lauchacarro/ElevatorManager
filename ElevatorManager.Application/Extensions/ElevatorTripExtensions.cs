using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Enums;

namespace ElevatorManager.Application.Extensions
{
    public static class ElevatorTripExtensions
    {
        public static int CalculateNextNumberTrip(this ElevatorTripCurrentStatusDto currentTrip)
        {
            if (currentTrip.PendientFloors.Any())
            {
                return currentTrip.Tríps.Last().NumberTrip;
            }
            else
            {
                return currentTrip.Tríps.Any() ?
                currentTrip.Tríps.Last().NumberTrip + 1 :
                1;
            }
            
        }

        public static bool IsAlreadyPressed(this ElevatorTripCurrentStatusDto currentTrip, MoveElevatorRequest request, Priority priority)
        {
            return currentTrip.Tríps.Any(x => x.DestinationFloor == request.Floor && x.Priority == priority);
        }

        /// <summary>
        /// Devuelve una lista que contiene solo los elementos que están después o en el mismo índice de segundos especificado
        /// </summary>
        /// <param name="floors">Lista de todos los pisos visitados</param>
        /// <param name="diffSeconds">Total de segundos transcurridos</param>
        /// <returns>Devuelve el piso actual y los pendientes basados en el tiempo.</returns>
        internal static List<TripFloor> FilterFloorsByTime(this IEnumerable<TripFloor> floors, TimeSpan diffSeconds)
        {

            var result = floors.Where((_, i) => i >= diffSeconds.TotalSeconds);

            return result.ToList();

        }
    }
}
