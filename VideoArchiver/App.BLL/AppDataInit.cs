using App.Common.Enums;
using App.DAL.Contracts;
using App.DAL.DTO.Entities;

namespace App.BLL;

public class AppDataInit
{
    private readonly IAppUnitOfWork _uow;

    public AppDataInit(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    private async Task SeedAppDataCategoriesAsync()
    {
        var defaultCategories = new List<CategoryWithCreator>
        {
            new()
            {
                Id = Guid.Parse("FC5BA375-0E05-476B-A245-75AA66F830A2"),
                Name = "Gaming",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("F78C7F9B-63E3-4E8A-82BC-48F2937BA4DC"),
                Name = "Music",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("3964B212-38E4-4254-A011-9FF710AE4193"),
                Name = "Sports",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("048BF0D3-A153-43D0-95E6-9CAA1331EA09"),
                Name = "Vlog",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("1F667BAB-7D29-4D97-99B4-22FBB0D62923"),
                Name = "Education",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("FC9EF0AB-1D17-4F55-9DE7-94B6FD11F191"),
                Name = "Comedy",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("1E0D03D9-59FE-4BB6-AF42-8A6F0F979EBA"),
                Name = "Animation",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("50A0C818-2A4E-459B-8392-B2A68963C120"),
                Name = "Science",
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
        };

        foreach (var category in defaultCategories)
        {
            if (await _uow.Categories.ExistsAsync(category.Id)) continue;
            _uow.Categories.Add(category);
        }
    }

    public async Task SeedAppData()
    {
        await SeedAppDataCategoriesAsync();
    }
}