using App.Contracts.DAL;
using Google.Apis.YouTube.v3;
using YoutubeExplode;

namespace App.BLL.YouTube;

public abstract class BaseYouTubeService
{
    protected readonly YouTubeUow YouTubeUow;

    protected YoutubeClient YouTubeExplodeClient => YouTubeUow.YouTubeExplodeClient;

    protected YouTubeService YouTubeApiService => YouTubeUow.YouTubeApiService;

    protected YouTubeSettings Config => YouTubeUow.Config;

    protected IAppUnitOfWork Uow => YouTubeUow.Uow;

    protected BaseYouTubeService(YouTubeUow youTubeUow)
    {
        YouTubeUow = youTubeUow;
    }
}