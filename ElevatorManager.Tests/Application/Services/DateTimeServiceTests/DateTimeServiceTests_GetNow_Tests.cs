using ElevatorManager.Application.Services;

namespace ElevatorManager.Tests.Application.Services.DateTimeServiceTests
{
    public class DateTimeServiceTests_GetNow_Tests
    {
        [Fact]
        public void GetNow_ReturnsCurrentDateTime()
        {
            // Arrange
            var dateTimeService = new DateTimeService();

            // Act
            DateTime result = dateTimeService.GetNow();

            // Assert
            DateTime expected = DateTime.Now;
            Assert.Equal(expected, result, TimeSpan.FromSeconds(1)); // Allow 1 second tolerance for test execution time
        }
    }




}
