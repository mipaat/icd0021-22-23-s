#pragma warning disable 1591
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using App.BLL;
using App.BLL.Config;
using App.BLL.DTO;
using App.BLL.Extensions;
using App.BLL.Identity;
using App.BLL.Identity.Config;
using App.BLL.Identity.Extensions;
using App.BLL.YouTube;
using App.BLL.YouTube.Extensions;
using App.Common.Exceptions;
using App.Contracts.DAL;
using App.DAL.EF;
using Asp.Versioning.ApiExplorer;
using AutoMapper;
using Base.WebHelpers;
using Base.WebHelpers.ModelBinders;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using ApiVersion = Asp.Versioning.ApiVersion;
using AutoMapperConfig = App.DAL.DTO.AutoMapperConfig;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        AppDbContextFactory.RegisterDbContext(builder.Services, builder.Configuration);
        builder.Services.AddScoped<IAppUnitOfWork>(provider =>
            new AppUnitOfWork(provider.GetRequiredService<AbstractAppDbContext>(),
                    provider.GetRequiredService<IMapper>())
                .AddDefaultConcurrencyConflictResolvers(provider));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddRazorPages(); // For account management
        builder.Services.AddCustomIdentity();

        builder.Services.AddScoped<IdentityAppDataInit>();
        builder.Services.AddScoped<IDbInitializer, DbInitializer>();
        builder.Services.AddScoped<AppDataInit>();

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

        builder.Services.DisableApiErrorRedirects();

        builder.Services.AddControllersWithViews()
            .AddCommaSeparatedArrayModelBinderProvider()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        const string corsAllowAllName = "CorsAllowAll";
        const string corsAllowCredentialsName = "CorsAllowCredentials";
        builder.Services.AddCors(options =>
        {
            var allowCredentialsOrigins = builder.Configuration.GetSection("AllowedCorsCredentialOrigins")
                .Get<ICollection<string>>();
            options.AddPolicy(corsAllowCredentialsName, policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();

                if (allowCredentialsOrigins is { Count: > 0 })
                {
                    policy.WithOrigins(allowCredentialsOrigins.ToArray());
                }

                policy.AllowCredentials();
            });
            options.AddPolicy(corsAllowAllName, policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        builder.Services.AddLocalization();

        builder.Services.AddAutoMapper((serviceProvider, mapperConfigurationExpression) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                mapperConfigurationExpression.AddProfile(new Public.DTO.AutoMapperConfig(httpContextAccessor));
            },
            typeof(AutoMapperConfig),
            typeof(App.BLL.DTO.AutoMapperConfig)
        );
        builder.Services.AddBllMappers();

        var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
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

        var defaultCulture = new CultureInfo("en-US").UseConstantDateTime();
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => c.UseConstantDateTime())
                .ToList(),
            SupportedUICultures = app.Configuration.GetSupportedUiCultures(),
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
        var profileImagesDirectoryPath = Path.Combine(builder.Environment.ContentRootPath, "downloads",
            "profile_images");
        Utils.Utils.EnsureDirectoryExists(profileImagesDirectoryPath);
        var thumbnailsDirectoryPath = Path.Combine(builder.Environment.ContentRootPath, "downloads", "thumbnails");
        Utils.Utils.EnsureDirectoryExists(thumbnailsDirectoryPath);
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider =
                new PhysicalFileProvider(profileImagesDirectoryPath),
            RequestPath = "/downloads/profile_images",
        });
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider =
                new PhysicalFileProvider(thumbnailsDirectoryPath),
            RequestPath = "/downloads/thumbnails",
        });

        app.UseRouting();

        app.UseCors(corsAllowAllName);
        app.UseCors(corsAllowCredentialsName);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAreaControllerRoute(name: "admin", areaName: "Admin",
            pattern: "Admin/{controller}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
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

        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        });

        app.Run();
    }

    private static void SetupAppData(IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var uow = scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>();

        var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
        dbInitializer?.RunDbInit(configuration.GetDbInitSettings());

        var identityAppDataInit = scope.ServiceProvider.GetRequiredService<IdentityAppDataInit>();
        identityAppDataInit.RunInitAsync().Wait();

        if (configuration.GetValue<bool>("InitializeAppData"))
        {
            var appDataInitializer = scope.ServiceProvider.GetRequiredService<AppDataInit>();
            appDataInitializer.SeedAppData().Wait();
        }

        uow.SaveChangesAsync().Wait();
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