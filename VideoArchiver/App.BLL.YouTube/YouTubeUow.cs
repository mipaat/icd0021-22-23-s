using System.Configuration;
using App.BLL.YouTube.Services;
using App.Contracts.DAL;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube;

public class YouTubeUow : IDisposable
{
    public readonly IAppUnitOfWork Uow;
    public readonly YouTubeContext Context;

    private readonly IServiceProvider _services;

    private YoutubeClient? _youTubeExplodeClient;
    public YoutubeClient YouTubeExplodeClient => _youTubeExplodeClient ??= new YoutubeClient();

    private YouTubeService? _youTubeApiService;

    public YouTubeService YouTubeApiService => _youTubeApiService ??= new YouTubeService(
        new BaseClientService.Initializer
        {
            ApiKey = Config.ApiKey,
            ApplicationName = Config.ApplicationName
        });

    private readonly IConfiguration _config;

    public YouTubeSettings Config => _config.GetRequiredSection(YouTubeSettings.SectionKey).Get<YouTubeSettings>() ??
                                     throw new ConfigurationErrorsException("Failed to read YouTube configuration!");

    public YouTubeUow(IAppUnitOfWork uow, IConfiguration config, YouTubeContext youTubeContext,
        IServiceProvider services)
    {
        Uow = uow;
        Context = youTubeContext;
        _services = services; // The IServiceProvider should be the IServiceProvider for the current scope, not the root IServiceProvider
        _config = config;
    }

    private YoutubeDL? _youtubeDl;
    public YoutubeDL YoutubeDl => _youtubeDl ??= new YoutubeDL();

    private SubmitService? _submitService;
    public SubmitService SubmitService => _submitService ??= _services.GetRequiredService<SubmitService>();

    private AuthorService? _authorService;
    public AuthorService AuthorService => _authorService ??= _services.GetRequiredService<AuthorService>();

    private PlaylistService? _playlistService;
    public PlaylistService PlaylistService => _playlistService ??= _services.GetRequiredService<PlaylistService>();

    private CommentService? _commentService;
    public CommentService CommentService => _commentService ??= _services.GetRequiredService<CommentService>();

    private VideoService? _videoService;
    public VideoService VideoService => _videoService ??= _services.GetRequiredService<VideoService>();

    public void Dispose()
    {
        YouTubeApiService.Dispose();
    }
}