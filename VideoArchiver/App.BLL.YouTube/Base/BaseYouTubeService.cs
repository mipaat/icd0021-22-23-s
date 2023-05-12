using App.BLL.Base;
using App.BLL.DTO.Mappers;
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

    public VideoMapper VideoMapper => YouTubeUow.VideoMapper;
    public AuthorMapper AuthorMapper => YouTubeUow.AuthorMapper;
    public PlaylistMapper PlaylistMapper => YouTubeUow.PlaylistMapper;
    public QueueItemMapper QueueItemMapper => YouTubeUow.QueueItemMapper;

    protected BaseYouTubeService(ServiceUow serviceUow, ILogger<TYouTubeService> logger, YouTubeUow youTubeUow,
        IMapper mapper) :
        base(serviceUow, logger, mapper)
    {
        YouTubeUow = youTubeUow;
    }
}