using App.BLL.DTO.Entities;

namespace App.BLL.Contracts;

public interface IPlatformVideoPresentationHandler
{
    public bool CanHandle(Video video);
    public bool CanHandle(BasicVideoWithAuthor video);
    public void Handle(VideoWithAuthorAndComments video);
    public void Handle(BasicVideoWithAuthor video);
    public void Handle(VideoWithAuthor video);
}