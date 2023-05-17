using ElevatorManager.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace ElevatorManager.Tests.Helpers
{
    public static class SetupHelpers
    {
        public static AppDbContext SetupAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            return context;
        }
    }
}
