using ElevatorManager.Application.Services;

namespace ElevatorManager.Tests.Application.Services.DateTimeServiceTests
{
    public class DateTimeServiceTests_GetNow_Tests
    {
        [Fact]
        public void Should_ReturnSameDateTime_When_GetNow()
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
