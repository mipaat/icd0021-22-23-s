using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Enums;
using App.Common.Enums;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace WebApp.ViewModels;

public class VideoSearchViewModel
{
    public List<BasicVideoWithAuthor> Videos { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public CategoryPickerPartialViewModel? CategoryPickerViewModel { get; set; }

    [BindProperty(SupportsGet = true)] public EPlatform? PlatformQuery { get; set; }
    [Display(Name = nameof(NameQuery) + "Name", Prompt = nameof(NameQuery) + "Prompt", ResourceType = typeof(App.Resources.WebApp.ViewModels.VideoSearchViewModel))]
    [BindProperty(SupportsGet = true)] public string? NameQuery { get; set; }
    [Display(Name = nameof(AuthorQuery) + "Name", Prompt = nameof(AuthorQuery) + "Prompt", ResourceType = typeof(App.Resources.WebApp.ViewModels.VideoSearchViewModel))]
    [BindProperty(SupportsGet = true)] public string? AuthorQuery { get; set; }

    [BindProperty(SupportsGet = true)] public int Page { get; set; }
    [BindProperty(SupportsGet = true)] public int Limit { get; set; } = 50;

    [BindProperty(SupportsGet = true)]
    [Display(Name = nameof(SortingOptions) + "Name", ResourceType = typeof(App.Resources.WebApp.ViewModels.VideoSearchViewModel))]
    public EVideoSortingOptions SortingOptions { get; set; } = EVideoSortingOptions.CreatedAt;
    [Display(Name = nameof(Descending) + "Name", ResourceType = typeof(App.Resources.WebApp.ViewModels.VideoSearchViewModel))]
    [BindProperty(SupportsGet = true)] public bool Descending { get; set; }
}