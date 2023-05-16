using ElevatorManager.Domain.Dtos;
using ElevatorManager.Domain.Services;

using ElevatorManager.Domain.Entities;

namespace ElevatorManager.Api.Routes
{
    public static class ElevatorTripRoutes
    {
        const string PATH = "ElevatorTrips";

        public static IEndpointRouteBuilder MapElevatorTrips(this IEndpointRouteBuilder builder)
        {

            var group = builder.MapGroup(PATH);


            group.MapGet("Status", async (IElevatorTripService service) =>
            {
                var result = await service.GetCurrentTripAsync();
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });


            group.MapPost("Inside", async (IElevatorTripService service, MoveElevatorRequest request) =>
               {
                   var result = await service.MoveElevatorFromInsideAsync(request);
                   return result.Success ? Results.Ok(result) : Results.BadRequest(result);
               });

            group.MapPost("Outside", async (IElevatorTripService service, MoveElevatorRequest request) =>
            {
                var result = await service.MoveElevatorFromOutsideAsync(request);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            return group;
        }
    }
}
