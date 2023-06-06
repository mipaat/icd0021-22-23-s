using System.Configuration;
using App.BLL.Identity.Config;
using App.Common;
using App.DAL.Contracts;
using App.DAL.EF;
using Base.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Tests.Helpers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveService<AbstractAppDbContext>();

            services.AddScoped<AbstractAppDbContext>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection") ??
                                       config.GetValue<string>("ConnectionStrings:PostgresConnection") ??
                                       throw new ConfigurationErrorsException("Failed to get DB connection string");
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
                connectionStringBuilder["Database"] = connectionStringBuilder.Database + "_testing";
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {
                        "ConnectionStrings:DefaultConnection", connectionStringBuilder.ConnectionString
                    }
                });
                return AppDbContextFactory.CreateDbContext(configBuilder.Build());
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            // Override configuration values
            var defaultConfig = scopedServices.GetRequiredService<IConfiguration>();
            var testingDownloadsPath =
                (defaultConfig.GetValue<string>(AppPaths.DownloadsPathConfigKey) ?? "downloads") + "_testing";
            var inMemoryOverrideConfig = new Dictionary<string, string?>
            {
                {
                    AppPaths.DownloadsPathConfigKey, testingDownloadsPath
                },
                {
                    $"{DbInitSettings.SectionKey}:{nameof(DbInitSettings.Migrate)}",
                    "false"
                },
                {
                    "InitializeAppData", "false"
                },
                {
                    $"{IdentityAppDataInitSettings.SectionKey}:{nameof(IdentityAppDataInitSettings.SeedIdentity)}",
                    "true"
                },
                {
                    $"{IdentityAppDataInitSettings.SectionKey}:{nameof(IdentityAppDataInitSettings.SeedDemoIdentity)}",
                    "false"
                },
                {
                    $"{IdentityAppDataInitSettings.SectionKey}:{nameof(IdentityAppDataInitSettings.AdminPassword)}",
                    "admin123"
                },
                {
                    $"{IdentityAppDataInitSettings.SectionKey}:{nameof(IdentityAppDataInitSettings.SuperAdminPassword)}",
                    "root123"
                },
            };
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddConfiguration(defaultConfig);
            configBuilder.AddInMemoryCollection(inMemoryOverrideConfig);
            var modifiedConfig = configBuilder.Build();
            _configuration = modifiedConfig;
            services.RemoveService<IConfiguration>();
            services.AddSingleton<IConfiguration>(modifiedConfig);

            var webHostEnvironment = scopedServices.GetRequiredService<IWebHostEnvironment>();
            _contentRootPath = webHostEnvironment.ContentRootPath;

            EnsureDownloadsDirectoryDeleted();

            var dbContext = scopedServices.GetRequiredService<AbstractAppDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        });
    }

    private IConfiguration? _configuration;
    private string? _contentRootPath;

    private void EnsureDownloadsDirectoryDeleted()
    {
        var downloadsPath = _configuration?.GetValue<string>(AppPaths.DownloadsPathConfigKey);
        if (_contentRootPath != null && downloadsPath != null)
        {
            downloadsPath = Path.Join(_contentRootPath, downloadsPath);
        }

        if (downloadsPath != null && downloadsPath.EndsWith("_testing") && Directory.Exists(downloadsPath))
        {
            Directory.Delete(downloadsPath, true);
        }
    }

    private bool _disposed;

    private void BaseDispose()
    {
        if (_disposed) return;
        EnsureDownloadsDirectoryDeleted();
        _configuration = null;
    }

    protected override void Dispose(bool disposing)
    {
        BaseDispose();
        base.Dispose(disposing);
        _disposed = true;
    }

    public override async ValueTask DisposeAsync()
    {
        BaseDispose();
        await base.DisposeAsync();
        _disposed = true;
    }
}