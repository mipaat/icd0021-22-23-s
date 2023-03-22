using DAL;
using Domain.Enums;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using WebApp.MyLibraries.ModelBinders;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Any(s =>
                string.Equals(s, "PrintSqlitePath", StringComparison.CurrentCultureIgnoreCase)))
        {
            Console.WriteLine($"Default local SQLite DB path = '{AppDbContextFactory.GetLocalDbSqlitePath()}'");
            return;
        }

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        AppDbContextFactory.RegisterDbContext(builder.Services, builder.Configuration);
        builder.Services.AddScoped<RepositoryContext>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AbstractAppDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
        builder.Services.AddControllersWithViews()
            .AddMvcOptions(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider<Platform>());
            });
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });

        var app = builder.Build();

        SetupAppData(app, app.Configuration);

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

        app.MapAreaControllerRoute(name: "crud", areaName: "Crud", pattern: "Crud/{controller}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }

    private static T RaiseIfNull<T>(T? dependency, string? dependencyDisplayName = null)
    {
        if (dependency != null) return dependency;
        throw new ApplicationException($"Failed to initialize {dependencyDisplayName ?? typeof(T).FullName}");
    }

    private static void SetupAppData(IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = RaiseIfNull(serviceScope.ServiceProvider.GetService<AbstractAppDbContext>());

        var logger = RaiseIfNull(serviceScope.ServiceProvider.GetService<ILogger<IApplicationBuilder>>());

        if (context.Database.ProviderName!.Contains("InMemory"))
        {
            logger.LogInformation("In memory DB provider detected, skipping data setup");
            return;
        }

        const string dataInitConfigSection = "DataInitialization";
        
        if (configuration.GetValue<bool>($"{dataInitConfigSection}:DropDatabase"))
        {
            logger.LogWarning("Drop database");
            AppDataInit.DropDatabase(context);
        }

        if (configuration.GetValue<bool>($"{dataInitConfigSection}:MigrateDatabase"))
        {
            logger.LogInformation("Migrate database");
            AppDataInit.MigrateDatabase(context);
        }

        using var userManager = RaiseIfNull(serviceScope.ServiceProvider.GetService<UserManager<User>>());
        using var roleManager = RaiseIfNull(serviceScope.ServiceProvider.GetService<RoleManager<Role>>());

        if (configuration.GetValue<bool>($"{dataInitConfigSection}:SeedIdentity"))
        {
            logger.LogInformation("Seed identity data");
            AppDataInit.SeedIdentity(userManager, roleManager);
        }

        if (configuration.GetValue<bool>($"{dataInitConfigSection}:SeedAppData"))
        {
            logger.LogInformation("Seed application data");
            AppDataInit.SeedAppData(context);
        }

        if (configuration.GetValue<bool>($"{dataInitConfigSection}:SeedDemoIdentity"))
        {
            logger.LogInformation("Seed demo identity data");
            AppDataInit.SeedDemoIdentity(userManager, roleManager,
                configuration.GetValue<bool>($"{dataInitConfigSection}:SeedIdentity"));
        }

        context.SaveChanges();
    }

    // May be used for creating DB context at design time (migrations etc)
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                AppDbContextFactory.RegisterDbContext(services, configuration);
            });
    }
}