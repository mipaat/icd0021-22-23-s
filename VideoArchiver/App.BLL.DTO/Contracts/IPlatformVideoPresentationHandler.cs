using App.BLL.DTO.Entities;

namespace App.BLL.DTO.Contracts;

public interface IPlatformVideoPresentationHandler
{
    public bool CanHandle(Video video);
    public VideoWithAuthorAndComments Handle(VideoWithAuthorAndComments video);
}