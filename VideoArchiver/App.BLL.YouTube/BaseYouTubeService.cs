using App.Contracts.DAL;
using Google.Apis.YouTube.v3;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube;

public abstract class BaseYouTubeService
{
    protected readonly YouTubeUow YouTubeUow;

    protected YoutubeClient YouTubeExplodeClient => YouTubeUow.YouTubeExplodeClient;

    protected YouTubeService YouTubeApiService => YouTubeUow.YouTubeApiService;

    protected YoutubeDL YoutubeDl => YouTubeUow.YoutubeDl;

    protected YouTubeSettings Config => YouTubeUow.Config;

    protected IAppUnitOfWork Uow => YouTubeUow.Uow;

    protected BaseYouTubeService(YouTubeUow youTubeUow)
    {
        YouTubeUow = youTubeUow;
    }
}