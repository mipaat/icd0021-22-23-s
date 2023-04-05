using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AbstractAppDbContext>
{
    public AbstractAppDbContext CreateDbContext(string[] args)
    {
        return CreateDbContext();
    }

    private static AbstractAppDbContext CreateDbContext(IConfiguration? configuration = null)
    {
        configuration ??= GetDefaultConfig();
        var connectionInfos = GetConnectionInfos(configuration);
        foreach (var connectionInfo in connectionInfos)
        {
            return connectionInfo.Provider switch
            {
                EDbProvider.Postgres => new PostgresAppDbContext(
                    ConfigureDbOptions<PostgresAppDbContext>(connectionInfo).Options),
                EDbProvider.Sqlite => new SqliteAppDbContext(
                    ConfigureDbOptions<SqliteAppDbContext>(connectionInfo).Options),
                _ => throw new UnsupportedDatabaseProviderException(connectionInfo)
            };
        }

        throw new Exception("No DB connection information found!"); // Should never be thrown
    }

    public static IConfiguration GetDefaultConfig()
    {
        var builder = WebApplication.CreateBuilder();
        return builder.Configuration;
    }

    private static string GetLocalDbSqlitePath(IConfiguration configuration)
    {
        var appName = configuration["AppName"];
        return appName == null ? GetLocalDbSqlitePath() : GetLocalDbSqlitePath(appName);
    }

    public static string GetLocalDbSqlitePath(string appName = "ICD0021_22-23_VideoArchiver")
    {
        var directorySeparator = Path.DirectorySeparatorChar;
        var sqliteRepoDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                  directorySeparator + appName +
                                  directorySeparator + "Data" +
                                  directorySeparator;

        Directory.CreateDirectory(sqliteRepoDirectory);

        return sqliteRepoDirectory + "app.db";
    }

    public static void RegisterDbContext(IServiceCollection services, IConfiguration? configuration = null)
    {
        configuration ??= GetDefaultConfig();
        services.AddScoped<AbstractAppDbContext>(_ => CreateDbContext(configuration));

        var connectionInfos = GetConnectionInfos(configuration);
        var encounteredProviders = new List<EDbProvider>();
        foreach (var connectionInfo in connectionInfos)
        {
            var provider = connectionInfo.Provider;
            if (encounteredProviders.Contains(provider)) continue;
            encounteredProviders.Add(provider);

            void OptionsAction(DbContextOptionsBuilder optionsBuilder) =>
                ConfigureDbOptions(connectionInfo, optionsBuilder);

            switch (provider)
            {
                case EDbProvider.Postgres:
                    services.AddDbContext<PostgresAppDbContext>(OptionsAction);
                    break;
                case EDbProvider.Sqlite:
                    services.AddDbContext<SqliteAppDbContext>(OptionsAction);
                    break;
            }
        }
    }

    private static DbContextOptionsBuilder<T> ConfigureDbOptions<T>(ConnectionInfo connectionInfo,
        DbContextOptionsBuilder<T>? optionsBuilder = null) where T : AbstractAppDbContext
    {
        optionsBuilder ??= new DbContextOptionsBuilder<T>();
        ConfigureDbOptions(connectionInfo, optionsBuilder as DbContextOptionsBuilder);
        return optionsBuilder;
    }

    private static DbContextOptionsBuilder ConfigureDbOptions(ConnectionInfo connectionInfo,
        DbContextOptionsBuilder? optionsBuilder = null)
    {
        optionsBuilder ??= new DbContextOptionsBuilder();
        switch (connectionInfo.Provider)
        {
            case EDbProvider.Postgres:
                optionsBuilder.UseNpgsql(connectionInfo.ConnectionString);
                break;
            case EDbProvider.Sqlite:
                optionsBuilder.UseSqlite(connectionInfo.ConnectionString);
                break;
            default:
                throw new UnsupportedDatabaseProviderException(connectionInfo);
        }

        return optionsBuilder;
    }

    private static List<ConnectionInfo> GetConnectionInfos(IConfiguration configuration)
    {
        var result = new List<ConnectionInfo>();

        var postgresConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                                       configuration.GetConnectionString("PostgresConnection");
        if (postgresConnectionString != null)
            result.Add(new ConnectionInfo(EDbProvider.Postgres, postgresConnectionString));

        var sqlitePath = GetLocalDbSqlitePath(configuration);
        result.Add(new ConnectionInfo(EDbProvider.Sqlite, $"Data source={sqlitePath}"));

        return result;
    }
}

internal enum EDbProvider
{
    Postgres,
    Sqlite
}

internal record ConnectionInfo(EDbProvider Provider, string ConnectionString);

internal class UnsupportedDatabaseProviderException : Exception
{
    public UnsupportedDatabaseProviderException(ConnectionInfo connectionInfo) : this(connectionInfo.Provider)
    {
    }

    private UnsupportedDatabaseProviderException(EDbProvider dbProvider) : base(
        $"Unsupported database provider: {dbProvider}!")
    {
    }
}