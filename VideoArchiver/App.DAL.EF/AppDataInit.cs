using App.Domain;
using App.Domain.Identity;
using App.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class AppDataInit
{
    private static readonly BasicRoleData AdminRoleData =
        new(Guid.Parse("FC151E0A-D22E-4F31-A750-FA7E71AC87FA"), RoleNames.Admin);

    private static readonly BasicRoleData HelperRoleData =
        new(Guid.Parse("8F9D122E-9B7E-4879-94E0-75D3422BD403"), RoleNames.Helper);

    public static void MigrateDatabase(AbstractAppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DropDatabase(AbstractAppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        await GetOrCreateRoleAsync(roleManager, AdminRoleData);
        await GetOrCreateRoleAsync(roleManager, HelperRoleData);
    }

    public static async Task SeedIdentityAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        var adminUserData = new BasicUserData(Guid.Parse("61F32996-082F-4B41-B1F5-20071452EF41"), "admin",
            "admin123", true);
        var adminUser = await GetOrCreateUserAsync(userManager, adminUserData);

        await SeedRolesAsync(roleManager);
        var adminRole = await GetOrCreateRoleAsync(roleManager, AdminRoleData);

        if (!await userManager.IsInRoleAsync(adminUser, AdminRoleData.Name))
        {
            var result = await userManager.AddToRoleAsync(adminUser, AdminRoleData.Name);
            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    $"Failed to add role {adminRole} to user {adminUser} - {result.Errors.ToLogString()}");
            }
        }
    }

    public static async Task SeedDemoIdentityAsync(UserManager<User> userManager, RoleManager<Role> roleManager,
        bool identitySeeded = false)
    {
        if (!identitySeeded)
        {
            await SeedIdentityAsync(userManager, roleManager);
        }

        var demoUserData =
            new BasicUserData(Guid.Parse("6514614B-F64E-409D-884C-768EB1DE19F7"), "demo", "demo123", true);
        await GetOrCreateUserAsync(userManager, demoUserData);
    }

    private static async Task<User> GetOrCreateUserAsync(UserManager<User> userManager, BasicUserData userData)
    {
        var user = await userManager.FindByIdAsync(userData.Id.ToString());
        if (user == null)
        {
            user = new User
            {
                Id = userData.Id,
                UserName = userData.UserName,
                IsApproved = userData.IsApproved,
            };
            var result = await userManager.CreateAsync(user, userData.Password);
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

    private static async Task<Role> GetOrCreateRoleAsync(RoleManager<Role> roleManager, BasicRoleData roleData)
    {
        var role = await roleManager.FindByIdAsync(roleData.Id.ToString());
        if (role == null)
        {
            role = new Role
            {
                Id = roleData.Id,
                Name = roleData.Name,
            };
            var result = await roleManager.CreateAsync(role);
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

internal record BasicUserData(Guid Id, string UserName, string Password, bool IsApproved);

internal record BasicRoleData(Guid Id, string Name);