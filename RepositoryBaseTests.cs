using DaemonTechChallenge.Data;
using DaemonTechChallenge.Database;
using DaemonTechChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace DaemonTechChallengeTests;

public class RepositoryBaseTests
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Add_ShouldAddEntityToContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        var entity = new DailyReport
        {
            Id = 1,
            CnpjFundo = "12345678901234",
            DtComptc = new DateTime(2023, 1, 1),
            VlTotal = 1000,
            VlQuota = 1.5m,
            VlPatrimLiq = 2000,
            CaptcDia = 100,
            ResgDia = 50,
            NrCotst = 10
        };

        // Act
        repository.Add(entity);
        await repository.SaveChangesAsync();

        // Assert
        var addedEntity = await context.FindAsync<DailyReport>(entity.Id);
        Assert.NotNull(addedEntity);
        Assert.Equal(entity.CnpjFundo, addedEntity.CnpjFundo);
        Assert.Equal(entity.DtComptc, addedEntity.DtComptc);
    }

    [Fact]
    public async Task AddRange_ShouldAddMultipleEntitiesToContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);
        var entities = new List<DailyReport>
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
        };

        // Act
        repository.AddRange(entities);
        await repository.SaveChangesAsync();

        // Assert
        foreach (var entity in entities)
        {
            Assert.Contains(entity, context.DailyReport);
        }
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntityFromContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        var entity = new DailyReport
        {
            CnpjFundo = "12345678901234",
            DtComptc = new DateTime(2023, 1, 1),
            VlTotal = 1000,
            VlQuota = 1.5m,
            VlPatrimLiq = 2000,
            CaptcDia = 100,
            ResgDia = 50,
            NrCotst = 10
        };
        context.DailyReport.Add(entity);
        await context.SaveChangesAsync();

        // Act
        repository.Delete(entity);
        await repository.SaveChangesAsync();

        // Assert
        Assert.DoesNotContain(entity, context.DailyReport);
    }

    [Fact]
    public async Task DeleteRange_ShouldRemoveMultipleEntitiesFromContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        var entities = new List<DailyReport>
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
        };

        context.DailyReport.AddRange(entities);
        await context.SaveChangesAsync();

        // Act
        repository.DeleteRange(entities);
        await repository.SaveChangesAsync();

        // Assert
        foreach (var entity in entities)
        {
            Assert.DoesNotContain(entity, context.DailyReport);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateEntityInContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        var entity = new DailyReport
        {
            CnpjFundo = "12345678901234",
            DtComptc = new DateTime(2023, 1, 1),
            VlTotal = 1000,
            VlQuota = 1.5m,
            VlPatrimLiq = 2000,
            CaptcDia = 100,
            ResgDia = 50,
            NrCotst = 10
        };

        context.DailyReport.Add(entity);
        await context.SaveChangesAsync();

        // Modify entity properties here
        entity.VlTotal = 2000;
        entity.VlQuota = 2.5m;

        // Act
        repository.Update(entity);
        await repository.SaveChangesAsync();

        // Assert
        var updatedEntity = await context.DailyReport.FindAsync(entity.Id);
        Assert.Equal(entity.VlTotal, updatedEntity?.VlTotal);
        Assert.Equal(entity.VlQuota, updatedEntity?.VlQuota);
    }

    [Fact]
    public async Task UpdateRange_ShouldUpdateMultipleEntitiesInContext()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        var entities = new List<DailyReport>
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
    };
        context.DailyReport.AddRange(entities);
        await context.SaveChangesAsync();

        // Modify properties of the entities here
        entities[0].VlTotal = 5000;
        entities[0].VlQuota = 5.0m;
        entities[1].VlTotal = 6000;
        entities[1].VlQuota = 6.0m;

        // Act
        repository.UpdateRange(entities);
        await repository.SaveChangesAsync();

        // Assert
        foreach (var entity in entities)
        {
            var updatedEntity = await context.DailyReport.FindAsync(entity.Id);
            Assert.Equal(entity.VlTotal, updatedEntity?.VlTotal);
            Assert.Equal(entity.VlQuota, updatedEntity?.VlQuota);
        }
    }

    [Fact]
    public void GetQueryable_ShouldReturnQueryable()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var repository = new RepositoryBase(context);

        // Act
        var queryable = repository.GetQueryable<DailyReport>();

        // Assert
        Assert.NotNull(queryable);
        Assert.IsAssignableFrom<IQueryable<DailyReport>>(queryable);
    }
}
