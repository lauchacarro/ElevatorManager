using ElevatorManager.Application.Commons;
using ElevatorManager.Application.Extensions;
using ElevatorManager.Application.Mappings;
using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Entities;
using ElevatorManager.Domain.Enums;
using ElevatorManager.Domain.Repositories;
using ElevatorManager.Domain.Services;

namespace ElevatorManager.Application.Services
{
    internal class ElevatorTripService : IElevatorTripService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IElevatorTripRepository _repository;

        public ElevatorTripService(IDateTimeService dateTimeService, IElevatorTripRepository repository)
        {
            _dateTimeService = dateTimeService;
            _repository = repository;
        }

        public async Task<Result<ElevatorTripCurrentStatusDto>> GetCurrentTripAsync()
        {
            var now = _dateTimeService.GetNow();

            var trips = await _repository.GetLastTripsAsync();

            if (!trips.Any())
                return Result.Ok(ElevatorTripCurrentStatusDto.Default);


            var firstTrip = trips.First();



            // Genero una nueva lista que contenga al principio el último destino para saber de cual piso arranca a moverse

            var previousTrip = await GetPreviousTripAsync(firstTrip);

            List<ElevatorTrip> completeTrips = new()
            {
                previousTrip
            };

            completeTrips.AddRange(trips);




            var diffSeconds = now - firstTrip.RequestTime;


            var allFloors = CollectAllFloorsFromTrips(completeTrips);

            var floors = allFloors.FilterFloorsByTime(diffSeconds);


            ElevatorTripCurrentStatusDto result = GetElevatorTripStatus(floors, completeTrips);


            return Result.Ok(result);

        }

        internal async Task<ElevatorTrip> GetPreviousTripAsync(ElevatorTrip firstTrip)
        {
            if (firstTrip.NumberTrip > 1)
            {
                var prevTrips = await _repository.GetTripsByNumberAsync(firstTrip.NumberTrip - 1);

                var lastPrevTrip = prevTrips.Last();

                return lastPrevTrip;
            }
            else
            {
                return ElevatorTrip.Default;
            }
        }

        /// <summary>
        /// Devuelve un objeto ElevatorTripCurrentStatusDto que representa el estado actual del viaje del ascensor.
        /// </summary>
        /// <param name="floors"></param>
        /// <param name="trips"></param>
        /// <returns></returns>
        internal static ElevatorTripCurrentStatusDto GetElevatorTripStatus(IEnumerable<TripFloor> floors, IEnumerable<ElevatorTrip> trips)
        {
            ElevatorTripCurrentStatusDto result;

            // Si hay pisos pendientes, se obtienen el piso actual, los pisos pendientes y una lista de objetos ElevatorTripDto

            if (floors.Any())
            {
                var currentFloor = floors.First().Floor;
                var pendientFloors = floors.Skip(1).Select(x => x.Floor);

                var tripsResult = trips.Skip(1).Where(x => floors.Select(x => x.TripId).Contains(x.Id)).Select(x => x.ConvertToDto());

                result = new(currentFloor, pendientFloors, tripsResult);
            }
            else
            {
                // Si no hay pisos pendientes, se obtiene el último piso de destino y se crea una lista con un solo objeto ElevatorTripDto para representar ese último viaje.

                var currentFloor = trips.Last().DestinationFloor;

                var tripsResult = new ElevatorTripDto[] { trips.Last().ConvertToDto() };

                result = new(currentFloor, Enumerable.Empty<int>(), tripsResult);
            }

            return result;
        }



        /// <summary>
        /// Este método toma una colección de viajes de ascensor y genera una lista de pisos visitados en esos viajes
        /// </summary>
        /// <param name="trips">Los viajes del ascensor</param>
        /// <returns>Lista de todos los pisos visitados en esos viajes</returns>
        internal static List<TripFloor> CollectAllFloorsFromTrips(IEnumerable<ElevatorTrip> trips)
        {

            List<TripFloor> result = new();

            for (int i = 0; i < trips.Count() - 1; i++)
            {
                //Se calcula la diferencia en valor absoluto entre el piso de destino del viaje actual y el piso de destino del siguiente viaje

                var trip = trips.ElementAt(i);
                var nextTrip = trips.ElementAt(i + 1);

                result.Add(new(trip.Id, trip.DestinationFloor));

                int diff = Math.Abs(trip.DestinationFloor - nextTrip.DestinationFloor);


                // Si la diferencia es mayor que 1, significa que hay pisos intermedios entre los dos viajes. En este caso, se agrega un nuevo objeto TripFloor para cada piso intermedio.

                if (diff > 1)
                {
                    int step = trip.DestinationFloor < nextTrip.DestinationFloor ? 1 : -1;
                    for (int j = 1; j < diff; j++)
                    {
                        result.Add(new(trip.Id, trip.DestinationFloor + step * j));
                    }
                }
            }

            var lastTrip = trips.Last();

            result.Add(new(lastTrip.Id, lastTrip.DestinationFloor));

            return result;
        }



        public Task<Result<ElevatorTripDto>> MoveElevatorFromInsideAsync(MoveElevatorRequest request)
        {
            return MoveElevatorAsync(request, Priority.High);
        }

        public Task<Result<ElevatorTripDto>> MoveElevatorFromOutsideAsync(MoveElevatorRequest request)
        {
            return MoveElevatorAsync(request, Priority.Low);
        }

        private async Task<Result<ElevatorTripDto>> MoveElevatorAsync(MoveElevatorRequest request, Priority priority)
        {
            var now = _dateTimeService.GetNow();

            if (request.Floor < 0)
            {
                return Result.Fail<ElevatorTripDto>(ErrorMessages.FloorGratherThanZero);
            }


            var currentTripResult = await GetCurrentTripAsync();

            var currentTrip = currentTripResult.Value;

            var nextNumberTrip = currentTrip.CalculateNextNumberTrip();

            if (currentTrip.CurrentFloor == request.Floor)
            {
                return Result.Fail<ElevatorTripDto>(ErrorMessages.ElevatorAlreadyOnRequestedFloor);
            }

            bool alreadyPressed = currentTrip.IsAlreadyPressed(request, priority);

            if (alreadyPressed)
            {
                return Result.Fail<ElevatorTripDto>(ErrorMessages.ButtonAlreadyPressed);
            }


            ElevatorTrip elevatorTrip = new(now, nextNumberTrip, request.Floor, priority);

            await _repository.AddAsync(elevatorTrip);

            return Result.Ok(elevatorTrip.ConvertToDto());
        }
    }
}
