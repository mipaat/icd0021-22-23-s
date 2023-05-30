using System.ComponentModel.DataAnnotations;

#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class LinkSubmissionViewModel
{
    [Display(Name = nameof(Link), Prompt = nameof(Link) + "Prompt", ResourceType = typeof(App.Resources.WebApp.ViewModels.LinkSubmissionViewModel))]
    public string Link { get; set; } = default!;
    [Display(Name = nameof(SubmitPlaylist), ResourceType = typeof(App.Resources.WebApp.ViewModels.LinkSubmissionViewModel))]
    public bool SubmitPlaylist { get; set; }
}