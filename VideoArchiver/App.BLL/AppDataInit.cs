using System.Globalization;
using System.Resources;
using App.BLL.Config;
using App.Common;
using App.Common.Enums;
using App.DAL.Contracts;
using App.DAL.DTO.Entities;
using Microsoft.Extensions.Configuration;
using AppDataResources = App.Resources.App.BLL.AppDataInit;

namespace App.BLL;

public class AppDataInit
{
    private readonly IAppUnitOfWork _uow;
    private readonly Dictionary<CultureInfo, ResourceSet> _cultureResourceSets;

    public AppDataInit(IAppUnitOfWork uow, IConfiguration configuration)
    {
        _uow = uow;
        _cultureResourceSets = new Dictionary<CultureInfo, ResourceSet>();
        foreach (var culture in configuration.GetSupportedUiCultures())
        {
            var resourceSet = AppDataResources.ResourceManager.GetResourceSet(culture, true, true);
            if (resourceSet != null)
            {
                _cultureResourceSets[culture] = resourceSet;
            }
        }
    }

    private LangString GetCategoryName(string resourceName, string fallback)
    {
        var result = new LangString();
        foreach (var kvp in _cultureResourceSets)
        {
            result[kvp.Key.Name] = kvp.Value.GetString(resourceName) ?? fallback;
        }

        if (result.Count == 0)
        {
            result["en-US"] = fallback;
        }

        return result;
    }

    private async Task SeedAppDataCategoriesAsync()
    {
        var defaultCategories = new List<CategoryWithCreator>
        {
            new()
            {
                Id = Guid.Parse("FC5BA375-0E05-476B-A245-75AA66F830A2"),
                Name = GetCategoryName(nameof(AppDataResources.Gaming), "Gaming"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("F78C7F9B-63E3-4E8A-82BC-48F2937BA4DC"),
                Name = GetCategoryName(nameof(AppDataResources.Music), "Music"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("3964B212-38E4-4254-A011-9FF710AE4193"),
                Name = GetCategoryName(nameof(AppDataResources.Sports), "Sports"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("048BF0D3-A153-43D0-95E6-9CAA1331EA09"),
                Name = GetCategoryName(nameof(AppDataResources.Vlog), "Vlog"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("1F667BAB-7D29-4D97-99B4-22FBB0D62923"),
                Name = GetCategoryName(nameof(AppDataResources.Education), "Education"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("FC9EF0AB-1D17-4F55-9DE7-94B6FD11F191"),
                Name = GetCategoryName(nameof(AppDataResources.Comedy), "Comedy"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("1E0D03D9-59FE-4BB6-AF42-8A6F0F979EBA"),
                Name = GetCategoryName(nameof(AppDataResources.Animation), "Animation"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
            new()
            {
                Id = Guid.Parse("50A0C818-2A4E-459B-8392-B2A68963C120"),
                Name = GetCategoryName(nameof(AppDataResources.Science), "Science"),
                IsAssignable = true,
                IsPublic = true,
                Platform = EPlatform.This,
            },
        };

        foreach (var category in defaultCategories)
        {
            var existingCategory = await _uow.Categories.GetByIdAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.IsPublic = category.IsPublic;
                _uow.Categories.Update(existingCategory);
            }
            else
            {
                _uow.Categories.Add(category);
            }
        }
    }

    public async Task SeedAppData()
    {
        await SeedAppDataCategoriesAsync();
    }
}