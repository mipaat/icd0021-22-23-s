using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.YouTube.Services;
using App.Common;
using App.Common.Enums;
using AutoMapper;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube;

public class YouTubeUow : BaseAppUowContainer, IDisposable
{
    public readonly YouTubeContext Context;

    private YoutubeClient? _youTubeExplodeClient;
    public YoutubeClient YouTubeExplodeClient => _youTubeExplodeClient ??= new YoutubeClient();

    private YouTubeService? _youTubeApiService;

    public YouTubeService YouTubeApiService => _youTubeApiService ??= new YouTubeService(
        new BaseClientService.Initializer
        {
            ApiKey = YouTubeConfig.ApiKey,
            ApplicationName = YouTubeConfig.ApplicationName
        });

    public YouTubeSettings YouTubeConfig => YouTubeSettings.FromConfiguration(Config);

    public readonly IServiceUow ServiceUow;

    public readonly IMapper Mapper;

    public YouTubeUow(IServiceUow serviceUow, YouTubeContext context, IMapper mapper) : base(serviceUow.Uow)
    {
        ServiceUow = serviceUow;
        Context = context;
        Mapper = mapper;
    }

    private IConfiguration Config => ServiceUow.Config;
    private IServiceProvider Services => ServiceUow.Services;

    private YoutubeDL? _youtubeDl;

    public YoutubeDL YoutubeDl =>
        _youtubeDl ??= new YoutubeDL
        {
            OutputFolder = AppPaths.GetVideosDirectory(EPlatform.YouTube, Config),
            RestrictFilenames = true,
            OverwriteFiles = false,
        };

    private AuthorService? _authorService;
    public AuthorService AuthorService => _authorService ??= Services.GetRequiredService<AuthorService>();

    private PlaylistService? _playlistService;
    public PlaylistService PlaylistService => _playlistService ??= Services.GetRequiredService<PlaylistService>();

    private CommentService? _commentService;
    public CommentService CommentService => _commentService ??= Services.GetRequiredService<CommentService>();

    private VideoService? _videoService;
    public VideoService VideoService => _videoService ??= Services.GetRequiredService<VideoService>();

    private ApiService? _apiService;
    public ApiService ApiService => _apiService ??= Services.GetRequiredService<ApiService>();

    public void Dispose()
    {
        YouTubeApiService.Dispose();
    }
}