using ElevatorManager.Domain.Repositories;
using ElevatorManager.Infrastructure.Data;
using ElevatorManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("Database"));

            services.AddScoped<IElevatorTripRepository, ElevatorTripRepository>();

            return services;
        }
    }
}
