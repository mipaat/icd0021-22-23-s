using App.Contracts.DAL;
using Google.Apis.YouTube.v3;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeService
{
    protected readonly YouTubeUow YouTubeUow;

    protected YoutubeClient YouTubeExplodeClient => YouTubeUow.YouTubeExplodeClient;

    protected YouTubeService YouTubeApiService => YouTubeUow.YouTubeApiService;

    protected YoutubeDL YoutubeDl => YouTubeUow.YoutubeDl;

    protected YouTubeSettings Config => YouTubeUow.Config;

    protected IAppUnitOfWork Uow => YouTubeUow.Uow;
    protected YouTubeContext Context => YouTubeUow.Context;

    protected BaseYouTubeService(YouTubeUow youTubeUow)
    {
        YouTubeUow = youTubeUow;
    }
}