using App.BLL.Extensions;
using App.Contracts.DAL;
using App.Domain;

namespace App.BLL;

public class EntityUpdateHandler
{
    private readonly IAppUnitOfWork _uow;

    public EntityUpdateHandler(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // TODO: Other update methods

    public void UpdateVideo(Video video, Video newVideoData)
    {
        var changed = false;
        var videoHistory = video.ToHistory();

        if (newVideoData.Title != null && !newVideoData.Title.IsUnspecifiedVersionOf(video.Title))
        {
            changed = video.Title != null && !newVideoData.Title.IsSpecifiedVersionOf(video.Title);
            video.Title = newVideoData.Title;
        }

        // TODO: The rest of the update

        if (changed)
        {
            _uow.VideoHistories.Add(videoHistory);
        }
    }
}