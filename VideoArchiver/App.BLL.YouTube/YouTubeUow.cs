using App.BLL.Services;
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

    public readonly ServiceUow ServiceUow;

    public YouTubeUow(ServiceUow serviceUow, YouTubeContext context)
    {
        ServiceUow = serviceUow;
        Context = context;
    }

    public IAppUnitOfWork Uow => ServiceUow.Uow;
    private IConfiguration Config => ServiceUow.Config;
    private IServiceProvider Services => ServiceUow.Services;

    private YoutubeDL? _youtubeDl;
    public YoutubeDL YoutubeDl => _youtubeDl ??= new YoutubeDL();

    private SubmitService? _submitService;
    public SubmitService SubmitService => _submitService ??= Services.GetRequiredService<SubmitService>();

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