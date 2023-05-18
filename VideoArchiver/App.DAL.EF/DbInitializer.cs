using App.Common;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.DAL.EF;

public class DbInitializer : IDbInitializer
{
    private readonly AbstractAppDbContext _dbContext;

    public DbInitializer(AbstractAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void RunDbInit(DbInitSettings config)
    {
        if (_dbContext.Database.ProviderName?.Contains("InMemory") ?? false)
        {
            return;
        }

        if (config.DropDatabase)
        {
            _dbContext.Database.EnsureDeleted();
        }

        if (config.EnsureCreated)
        {
            _dbContext.Database.EnsureCreated();
        }

        if (config.Migrate)
        {
            _dbContext.Database.Migrate();
        }
    }
}