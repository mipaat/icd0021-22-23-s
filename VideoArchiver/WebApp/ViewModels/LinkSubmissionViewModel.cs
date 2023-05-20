#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class LinkSubmissionViewModel
{
    public string Link { get; set; } = default!;
    public bool SubmitPlaylist { get; set; }
}