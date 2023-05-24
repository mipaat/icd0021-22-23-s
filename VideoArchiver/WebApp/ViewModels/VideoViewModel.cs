using App.BLL.DTO.Entities;

#pragma warning disable 1591
namespace WebApp.ViewModels;

public class VideoViewModel
{
    public bool EmbedView { get; set; }
    public VideoWithAuthorAndComments Video { get; set; } = default!;
    public int CommentsPage { get; set; }
    public int CommentsLimit { get; set; }
}