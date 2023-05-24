using App.BLL.DTO.Entities;
using App.Common.Enums;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace WebApp.ViewModels;

public class VideoSearchViewModel
{
    public ICollection<BasicVideoWithAuthor> Videos { get; set; } = new List<BasicVideoWithAuthor>();

    [BindProperty(SupportsGet = true)]
    public CategoryPickerPartialViewModel? CategoryPickerViewModel { get; set; }

    [BindProperty(SupportsGet = true)] public EPlatform? PlatformQuery { get; set; }
    [BindProperty(SupportsGet = true)] public string? NameQuery { get; set; }
    [BindProperty(SupportsGet = true)] public string? AuthorQuery { get; set; }

    [BindProperty(SupportsGet = true)] public int Page { get; set; }
    [BindProperty(SupportsGet = true)] public int Limit { get; set; } = 50;

    [BindProperty(SupportsGet = true)]
    public EVideoSortingOptions SortingOptions { get; set; } = EVideoSortingOptions.CreatedAt;
    [BindProperty(SupportsGet = true)] public bool Descending { get; set; }
}