using App.Contracts.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.BackgroundServices;

public class OfficialApiTimedBackgroundService : IHostedService, IDisposable
{
    private readonly ILogger<OfficialApiTimedBackgroundService> _logger;
    private Timer? _timer;
    private readonly object _lock = new();
    private bool _isRunning;
    private readonly YouTubeContext _context;
    private readonly IServiceProvider _services;

    public OfficialApiTimedBackgroundService(ILogger<OfficialApiTimedBackgroundService> logger, YouTubeContext context,
        IServiceProvider services)
    {
        _logger = logger;
        _context = context;
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting official YouTube video data fetch background service");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? _)
    {
        lock (_lock)
        {
            if (_isRunning)
            {
                _logger.LogInformation(
                    "Previous execution of official YouTube video data fetch still running. Skipping execution.");
                return;
            }

            _isRunning = true;
        }

        try
        {
            _logger.LogInformation("Starting official YouTube data fetch");

            var i = 0;
            var neverFetchedVideosCanContinue = true;
            var neverFetchedPlaylistsCanContinue = true;
            var videosCanContinue = true;
            var playlistsCanContinue = true;

            while (_context.ApiUsage < 9990 &&
                   (neverFetchedVideosCanContinue || videosCanContinue ||
                    neverFetchedPlaylistsCanContinue || playlistsCanContinue))
            {
                using var scope = _services.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>();
                var youTubeUow = scope.ServiceProvider.GetRequiredService<YouTubeUow>();

                var changesMade = false;

                var cycle = i % 4;
                if (cycle == 0 && neverFetchedVideosCanContinue)
                {
                    neverFetchedVideosCanContinue =
                        await youTubeUow.VideoService.UpdateAddedNeverFetchedVideosOfficial();
                    changesMade = true;
                }
                else if (cycle == 1 && neverFetchedPlaylistsCanContinue)
                {
                    neverFetchedPlaylistsCanContinue =
                        await youTubeUow.PlaylistService.UpdateAddedNeverFetchedPlaylistsOfficial();
                    changesMade = true;
                }
                else if (cycle == 2 && videosCanContinue)
                {
                    videosCanContinue = await youTubeUow.VideoService.UpdateAddedVideosOfficial();
                    changesMade = true;
                }
                else if (cycle == 3 && playlistsCanContinue)
                {
                    playlistsCanContinue = await youTubeUow.PlaylistService.UpdateAddedPlaylistsOfficial();
                    changesMade = true;
                }

                if (changesMade)
                {
                    await uow.SaveChangesAsync();
                }

                i++;
            }
        }
        finally
        {
            lock (_lock)
            {
                _isRunning = false;
            }
        }

        if (_context.ApiUsage >= 9990)
        {
            _logger.LogInformation("Stopped official YouTube data fetch due to API quota limit");
        }
        else
        {
            _logger.LogInformation("Finished official YouTube data fetch");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping official YouTube video data fetch background service");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}