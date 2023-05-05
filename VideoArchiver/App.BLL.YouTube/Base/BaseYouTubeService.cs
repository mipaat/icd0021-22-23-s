using App.Contracts.DAL;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeService<TYouTubeService>
{
    protected readonly YouTubeUow YouTubeUow;
    protected readonly ILogger<TYouTubeService> Logger;

    protected YoutubeClient YouTubeExplodeClient => YouTubeUow.YouTubeExplodeClient;

    protected YouTubeService YouTubeApiService => YouTubeUow.YouTubeApiService;

    protected YoutubeDL YoutubeDl => YouTubeUow.YoutubeDl;

    protected YouTubeSettings Config => YouTubeUow.Config;

    protected IAppUnitOfWork Uow => YouTubeUow.Uow;
    protected YouTubeContext Context => YouTubeUow.Context;

    protected BaseYouTubeService(YouTubeUow youTubeUow, ILogger<TYouTubeService> logger)
    {
        YouTubeUow = youTubeUow;
        Logger = logger;
    }
}