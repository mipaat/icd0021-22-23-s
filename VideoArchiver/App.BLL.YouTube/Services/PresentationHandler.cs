using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.Common.Enums;

namespace App.BLL.YouTube.Services;

public class PresentationHandler : IPlatformVideoPresentationHandler
{
    public bool CanHandle(Video video)
    {
        return video.Platform == EPlatform.YouTube;
    }

    public VideoWithAuthorAndComments Handle(VideoWithAuthorAndComments video)
    {
        video.Url = Url.ToVideoUrl(video.IdOnPlatform);
        video.EmbedUrl = Url.ToVideoEmbedUrl(video.IdOnPlatform);
        return video;
    }
}