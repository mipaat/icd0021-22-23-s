using System.Configuration;
using System.Runtime.InteropServices;
using App.Contracts.DAL;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using YoutubeDLSharp;
using YoutubeExplode;

namespace App.BLL.YouTube;

public class YouTubeUow
{
    public readonly IAppUnitOfWork Uow;
    private YoutubeClient? _youTubeExplodeClient;

    public YoutubeClient YouTubeExplodeClient => _youTubeExplodeClient ??= new YoutubeClient();

    private YouTubeService? _youTubeApiService;

    public YouTubeService YouTubeApiService => _youTubeApiService ??= new YouTubeService(new BaseClientService.Initializer
    {
        ApiKey = Config.ApiKey,
        ApplicationName = Config.ApplicationName
    });

    private readonly IConfiguration _config;

    public YouTubeSettings Config => _config.GetRequiredSection(YouTubeSettings.SectionKey).Get<YouTubeSettings>() ??
                                     throw new ConfigurationErrorsException("Failed to read YouTube configuration!");

    public YouTubeUow(IAppUnitOfWork uow, IConfiguration config)
    {
        Uow = uow;
        _config = config;
    }

    private SubmitService? _submitService;
    public SubmitService SubmitService => _submitService ??= new SubmitService(this);

    private AuthorService? _authorService;
    public AuthorService AuthorService => _authorService ??= new AuthorService(this);

    private PlaylistService? _playlistService;
    public PlaylistService PlaylistService => _playlistService ??= new PlaylistService(this);

    private YoutubeDL? _youtubeDl;
    public YoutubeDL YoutubeDl => _youtubeDl ??= new YoutubeDL();
}