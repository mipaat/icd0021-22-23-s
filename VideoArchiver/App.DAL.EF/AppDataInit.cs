using App.Domain;
using App.Domain.Identity;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class AppDataInit
{
    public static void MigrateDatabase(AbstractAppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DropDatabase(AbstractAppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void SeedIdentity(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        var adminUserData = new BasicUserData(Guid.Parse("61F32996-082F-4B41-B1F5-20071452EF41"), "admin@test.com",
            "admin123");
        var adminUser = GetOrCreateUser(userManager, adminUserData);

        var adminRoleData = new BasicRoleData(Guid.Parse("FC151E0A-D22E-4F31-A750-FA7E71AC87FA"), "Admin");
        var adminRole = GetOrCreateRole(roleManager, adminRoleData);

        if (!userManager.IsInRoleAsync(adminUser, adminRoleData.Name).Result)
        {
            var result = userManager.AddToRoleAsync(adminUser, adminRoleData.Name).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    $"Failed to add role {adminRole} to user {adminUser} - {result.Errors.ToLogString()}");
            }
        }
    }

    public static void SeedDemoIdentity(UserManager<User> userManager, RoleManager<Role> roleManager,
        bool identitySeeded = false)
    {
        if (!identitySeeded)
        {
            SeedIdentity(userManager, roleManager);
        }

        var demoUserData =
            new BasicUserData(Guid.Parse("6514614B-F64E-409D-884C-768EB1DE19F7"), "demo@test.com", "demo123");
        GetOrCreateUser(userManager, demoUserData);
    }

    private static User GetOrCreateUser(UserManager<User> userManager, BasicUserData userData)
    {
        var user = userManager.FindByIdAsync(userData.Id.ToString()).Result;
        if (user == null)
        {
            user = new User
            {
                Id = userData.Id,
                Email = userData.Email,
                EmailConfirmed = true,
                UserName = userData.Email,
            };
            var result = userManager.CreateAsync(user, userData.Password).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    $"Failed to create user from {userData} - {result.Errors.ToLogString()}");
            }
        }

        return user;
    }

    private static string ToLogString(this IEnumerable<IdentityError> errors)
    {
        return $"[{string.Join(", ", errors.Select(e => $"{e.Code} - {e.Description}"))}]";
    }

    private static Role GetOrCreateRole(RoleManager<Role> roleManager, BasicRoleData roleData)
    {
        var role = roleManager.FindByIdAsync(roleData.Id.ToString()).Result;
        if (role == null)
        {
            role = new Role
            {
                Id = roleData.Id,
                Name = roleData.Name,
            };
            var result = roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    $"Failed to create role from {roleData} - {result.Errors.ToLogString()}");
            }
        }

        return role;
    }

    public static void SeedAppData(AbstractAppDbContext context)
    {
        SeedAppDataCategories(context);
    }

    private static void SeedAppDataCategories(AbstractAppDbContext context)
    {
        var defaultCategories = new List<Category>
        {
            new()
            {
                Id = Guid.Parse("FC5BA375-0E05-476B-A245-75AA66F830A2"),
                Name = "Gaming",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("F78C7F9B-63E3-4E8A-82BC-48F2937BA4DC"),
                Name = "Music",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("3964B212-38E4-4254-A011-9FF710AE4193"),
                Name = "Sports",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("048BF0D3-A153-43D0-95E6-9CAA1331EA09"),
                Name = "Vlog",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("1F667BAB-7D29-4D97-99B4-22FBB0D62923"),
                Name = "Education",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("FC9EF0AB-1D17-4F55-9DE7-94B6FD11F191"),
                Name = "Comedy",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("1E0D03D9-59FE-4BB6-AF42-8A6F0F979EBA"),
                Name = "Animation",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
            new()
            {
                Id = Guid.Parse("50A0C818-2A4E-459B-8392-B2A68963C120"),
                Name = "Science",
                IsAssignable = true,
                IsPublic = true,
                SupportsVideos = true,
                SupportsAuthors = true,
                SupportsPlaylists = true,
            },
        };

        foreach (var category in defaultCategories)
        {
            var existingCategory = context.Categories.Find(category.Id);
            if (existingCategory == null)
            {
                context.Categories.Add(category);
            }
        }
    }
}

internal record BasicUserData(Guid Id, string Email, string Password);

internal record BasicRoleData(Guid Id, string Name);