using ElevatorManager.Application.Services;
using ElevatorManager.Domain.Services;

using Microsoft.Extensions.DependencyInjection;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "ElevatorManager.Tests")]

namespace ElevatorManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IElevatorTripService, ElevatorTripService>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}
