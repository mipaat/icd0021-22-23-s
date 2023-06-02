using App.BLL.Base;
using App.BLL.Contracts;
using AutoMapper;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeService<TYouTubeService> : BaseService<TYouTubeService>
{
    protected readonly YouTubeUow YouTubeUow;

    protected YoutubeClient YouTubeExplodeClient => YouTubeUow.YouTubeExplodeClient;

    protected YouTubeService YouTubeApiService => YouTubeUow.YouTubeApiService;

    protected YoutubeDL YoutubeDl => YouTubeUow.YoutubeDl;

    protected YouTubeSettings Config => YouTubeUow.YouTubeConfig;

    protected YouTubeContext Context => YouTubeUow.Context;

    protected IMapper Mapper => YouTubeUow.Mapper;

    protected BaseYouTubeService(IServiceUow serviceUow, ILogger<TYouTubeService> logger, YouTubeUow youTubeUow) :
        base(serviceUow, logger)
    {
        YouTubeUow = youTubeUow;
    }
}