using App.Common;
using App.DAL.Contracts;
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

        if (config.Migrate)
        {
            _dbContext.Database.Migrate();
        }
    }
}