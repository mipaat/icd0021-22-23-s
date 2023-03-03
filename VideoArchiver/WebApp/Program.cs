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