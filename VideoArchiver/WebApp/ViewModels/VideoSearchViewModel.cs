using App.BLL.DTO.Entities;
using App.Common.Enums;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace WebApp.ViewModels;

public class VideoSearchViewModel
{
    public ICollection<VideoWithAuthor> Videos { get; set; } = new List<VideoWithAuthor>();

    [BindProperty(SupportsGet = true)]
    public CategoryPickerPartialViewModel? CategoryPickerViewModel { get; set; }

    [BindProperty(SupportsGet = true)] public EPlatform? PlatformQuery { get; set; }
    [BindProperty(SupportsGet = true)] public string? NameQuery { get; set; }
    [BindProperty(SupportsGet = true)] public string? AuthorQuery { get; set; }
}