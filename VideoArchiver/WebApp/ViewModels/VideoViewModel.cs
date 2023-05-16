using App.BLL.DTO.Entities;

#pragma warning disable 1591
namespace WebApp.ViewModels;

public class VideoViewModel
{
    public VideoWithAuthorAndComments Video { get; set; } = default!;
}