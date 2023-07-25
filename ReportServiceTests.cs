using Microsoft.EntityFrameworkCore;
using DaemonTechChallenge.Services;
using DaemonTechChallenge.Data;
using DaemonTechChallenge.Models;
using DaemonTechChallenge.Database;

namespace DaemonTechChallengeTests;

public class ReportServiceTests
{
    private DbContextOptions<AppDbContext> GetInMemoryOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public async Task GetReportsAsync_ReturnsCorrectReports()
    {
        // Arrange
        using (var context = new AppDbContext(GetInMemoryOptions("GetReportsAsync_ReturnsCorrectReports")))
        {
            context.AddRange(new List<DailyReport>
            {
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 1),
                    VlTotal = 1000,
                    VlQuota = 1.5m,
                    VlPatrimLiq = 2000,
                    CaptcDia = 100,
                    ResgDia = 50,
                    NrCotst = 10
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 2),
                    VlTotal = 2000,
                    VlQuota = 2.5m,
                    VlPatrimLiq = 3000,
                    CaptcDia = 200,
                    ResgDia = 100,
                    NrCotst = 20
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 3),
                    VlTotal = 3000,
                    VlQuota = 3.5m,
                    VlPatrimLiq = 4000,
                    CaptcDia = 300,
                    ResgDia = 150,
                    NrCotst = 30
                },
            });
            context.SaveChanges();
        }

        var repository = new RepositoryBase(new AppDbContext(GetInMemoryOptions("GetReportsAsync_ReturnsCorrectReports")));
        var service = new ReportService(repository);

        // Act
        var reports = await service.GetReportsAsync("12345678901234", null, null);

        // Assert
        Assert.Equal(3, reports.Count);
    }

    [Fact]
    public async Task GetReportsAsync_ReturnsCorrectReportsWithStartDateAndEndDate()
    {
        // Arrange
        using (var context = new AppDbContext(GetInMemoryOptions("GetReportsAsync_ReturnsCorrectReportsWithStartDateAndEndDate")))
        {
            context.AddRange(new List<DailyReport>
            {
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 1),
                    VlTotal = 1000,
                    VlQuota = 1.5m,
                    VlPatrimLiq = 2000,
                    CaptcDia = 100,
                    ResgDia = 50,
                    NrCotst = 10
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 2),
                    VlTotal = 2000,
                    VlQuota = 2.5m,
                    VlPatrimLiq = 3000,
                    CaptcDia = 200,
                    ResgDia = 100,
                    NrCotst = 20
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 3),
                    VlTotal = 3000,
                    VlQuota = 3.5m,
                    VlPatrimLiq = 4000,
                    CaptcDia = 300,
                    ResgDia = 150,
                    NrCotst = 30
                },
            });
            context.SaveChanges();
        }

        var repository = new RepositoryBase(new AppDbContext(GetInMemoryOptions("GetReportsAsync_ReturnsCorrectReportsWithStartDateAndEndDate")));
        var service = new ReportService(repository);

        // Act
        var startDate = new DateTime(2023, 1, 2);
        var endDate = new DateTime(2023, 1, 3);
        var reports = await service.GetReportsAsync("12345678901234", startDate, endDate);

        // Assert
        Assert.Single(reports);
    }

    [Fact]
    public async Task GetReportsAsync_ThrowsExceptionIfCnpjIsNullOrEmpty()
    {
        // Arrange
        using (var context = new AppDbContext(GetInMemoryOptions("GetReportsAsync_ThrowsExceptionIfCnpjIsNullOrEmpty")))
        {
            context.AddRange(new List<DailyReport>
            {
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 1),
                    VlTotal = 1000,
                    VlQuota = 1.5m,
                    VlPatrimLiq = 2000,
                    CaptcDia = 100,
                    ResgDia = 50,
                    NrCotst = 10
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 2),
                    VlTotal = 2000,
                    VlQuota = 2.5m,
                    VlPatrimLiq = 3000,
                    CaptcDia = 200,
                    ResgDia = 100,
                    NrCotst = 20
                },
                new DailyReport {
                    CnpjFundo = "12345678901234",
                    DtComptc = new DateTime(2023, 1, 3),
                    VlTotal = 3000,
                    VlQuota = 3.5m,
                    VlPatrimLiq = 4000,
                    CaptcDia = 300,
                    ResgDia = 150,
                    NrCotst = 30
                },
            });
            context.SaveChanges();
        }

        var repository = new RepositoryBase(new AppDbContext(GetInMemoryOptions("GetReportsAsync_ThrowsExceptionIfCnpjIsNullOrEmpty")));
        var service = new ReportService(repository);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetReportsAsync(null, null, null));
        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetReportsAsync("", null, null));
    }
}
