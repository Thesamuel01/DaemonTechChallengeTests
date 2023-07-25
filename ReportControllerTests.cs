using Microsoft.AspNetCore.Mvc;
using DaemonTechChallenge.Controllers;
using DaemonTechChallenge.DTOs;
using DaemonTechChallenge.Services;
using Moq;

namespace DaemonTechChallengeTests;

public class ReportControllerTests
{
    [Fact]
    public async Task GetReports_ValidCnpj_ReturnsOkResult()
    {
        // Arrange
        var mockService = new Mock<IReportService>();
        mockService.Setup(service => service.GetReportsAsync("12345678901234", null, null))
            .ReturnsAsync(new List<DailyReportDTO>
            {
                new DailyReportDTO(1, "12345678901234", new DateTime(2023, 1, 1), 1000, 1.5m, 2000, 100, 50, 10),
                new DailyReportDTO(2, "12345678901234", new DateTime(2023, 1, 2), 2000, 2.5m, 3000, 200, 100, 20),
                new DailyReportDTO(3, "12345678901234", new DateTime(2023, 1, 3), 3000, 3.5m, 4000, 300, 150, 30),
            });

        var controller = new ReportController(mockService.Object);

        // Act
        var result = await controller.GetReports("12345678901234", null, null);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetReports_InvalidCnpj_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<IReportService>();
        var controller = new ReportController(mockService.Object);

        // Act
        var result = await controller.GetReports("", null, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("O parâmetro CNPJ é obrigatório.", badRequestResult.Value);
    }
}
