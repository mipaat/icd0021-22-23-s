#pragma warning disable 1591
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using App.BLL.Extensions;
using App.BLL.Identity.Extensions;
using App.BLL.YouTube;
using App.BLL.YouTube.Extensions;
using App.Contracts.DAL;
using App.Domain.Enums;
using App.Domain.Identity;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using DAL;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utils;
using WebApp.Config;
using WebApp.MyLibraries.ModelBinders;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        AppDbContextFactory.RegisterDbContext(builder.Services, builder.Configuration);
        builder.Services.AddScoped<IAppUnitOfWork>(provider =>
            new AppUnitOfWork(provider.GetRequiredService<AbstractAppDbContext>())
                .AddDefaultConcurrencyConflictResolvers(provider));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddCustomIdentity();

        var jwtSettings = builder.Configuration.GetRequiredSection(JwtSettings.SectionKey).Get<JwtSettings>();

        builder.Services
            .AddAuthentication()
            .AddCookie(options => { options.SlidingExpiration = true; })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        builder.Services.AddControllersWithViews()
            .AddMvcOptions(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider<Platform>());
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        const string corsAllowAllName = "CorsAllowAll";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsAllowAllName, policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        builder.Services.AddAutoMapper(
            typeof(Public.DTO.AutoMapperConfig)
        );

        var apiVersioningBuilder = builder.Services.AddApiVersioning((Asp.Versioning.ApiVersioningOptions options) =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen();

        var useHttpLogging = builder.Configuration.GetValue<bool>("Logging:HTTP:Enabled");
        if (useHttpLogging)
        {
            builder.Services.AddHttpLogging(logging => { logging.LoggingFields = HttpLoggingFields.All; });
        }

        // -------------------------------------
        // Register and set up BLL services
        // -------------------------------------

        builder.Services.AddGeneralBll();
        SetupDependencies.DownloadAndSetupDependencies(builder.Configuration).Wait();
        builder.Services.AddYouTube();

        // -------------------------------------

        // App created
        var app = builder.Build();

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures),
            SupportedUICultures = CultureInfo.GetCultures(CultureTypes.AllCultures),
        };
        app.UseRequestLocalization(localizationOptions);

        if (useHttpLogging)
        {
            app.UseHttpLogging();
        }

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

        app.UseCors(corsAllowAllName);

        app.UseAuthorization();

        app.MapAreaControllerRoute(name: "crud", areaName: "Crud", pattern: "Crud/{controller}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName);
            }
        });

        app.Run();
    }

    private static void SetupAppData(IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<AbstractAppDbContext>().RaiseIfNull();

        var logger = serviceScope.ServiceProvider.GetService<ILogger<IApplicationBuilder>>().RaiseIfNull();

        if (context.Database.ProviderName!.Contains("InMemory"))
        {
            logger.LogInformation("In memory DB provider detected, skipping data setup");
            return;
        }

        var dataInitSettings = configuration.GetRequiredSection(DataInitializationSettings.SectionKey)
            .Get<DataInitializationSettings>();
        if (dataInitSettings != null)
        {
            if (dataInitSettings.DropDatabase)
            {
                logger.LogWarning("Drop database");
                AppDataInit.DropDatabase(context);
            }

            if (dataInitSettings.MigrateDatabase)
            {
                logger.LogInformation("Migrate database");
                AppDataInit.MigrateDatabase(context);
            }

            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>().RaiseIfNull();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>().RaiseIfNull();

            if (dataInitSettings.SeedIdentity)
            {
                logger.LogInformation("Seed identity data");
                AppDataInit.SeedIdentityAsync(userManager, roleManager).Wait();
            }

            if (dataInitSettings.SeedAppData)
            {
                logger.LogInformation("Seed application data");
                AppDataInit.SeedAppData(context);
            }

            if (dataInitSettings.SeedDemoIdentity)
            {
                logger.LogInformation("Seed demo identity data");
                AppDataInit.SeedDemoIdentityAsync(userManager, roleManager,
                    dataInitSettings.SeedIdentity).Wait();
            }

            context.SaveChanges();
        }
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