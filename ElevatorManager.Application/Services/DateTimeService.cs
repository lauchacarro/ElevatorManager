using ElevatorManager.Domain.Services;

namespace ElevatorManager.Application.Services
{
    internal class DateTimeService : IDateTimeService
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
