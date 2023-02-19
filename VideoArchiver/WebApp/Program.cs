using DAL;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureDbOptions(builder.Configuration, builder.Services);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }

    // Used for creating DB context at design time (migrations etc)
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                ConfigureDbOptions(configuration, services);
            });
    }

    private static void ConfigureDbOptions(IConfiguration configuration, IServiceCollection services)
    {
        var postgresConnectionString = configuration.GetConnectionString("DefaultConnection") ?? configuration.GetConnectionString("PostgresConnection");
        if (postgresConnectionString == null)
        {
            var sqlitePath = GetLocalDbSqlitePath();
            Console.WriteLine($"No connection string found, using local DB at {sqlitePath}");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data source={sqlitePath}"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(postgresConnectionString));
        }
    }

    private static string GetLocalDbSqlitePath()
    {
        var directorySeparator = Path.DirectorySeparatorChar;
        var sqliteRepoDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                  directorySeparator + "ICD0021_22-23_VideoArchiver" +
                                  directorySeparator + "Data" +
                                  directorySeparator;

        Directory.CreateDirectory(sqliteRepoDirectory);

        return sqliteRepoDirectory + "app.db";
    }
}