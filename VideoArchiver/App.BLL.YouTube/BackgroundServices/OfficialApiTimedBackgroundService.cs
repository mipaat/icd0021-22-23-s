using App.BLL.YouTube.Base;
using App.Contracts.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.BackgroundServices;

public class
    OfficialApiTimedBackgroundService : BaseYouTubeLockedTimedBackgroundService<OfficialApiTimedBackgroundService>
{
    public OfficialApiTimedBackgroundService(ILogger<OfficialApiTimedBackgroundService> logger,
        IServiceProvider services, YouTubeContext youTubeContext) :
        base(logger, services, TimeSpan.FromHours(1), youTubeContext)
    {
    }

    protected override Task AfterLockedWork()
    {
        if (YouTubeContext.ApiUsage >= 9990)
        {
            Logger.LogInformation("Stopped official YouTube data fetch due to API quota limit");
        }
        else
        {
            Logger.LogInformation("Finished official YouTube data fetch");
        }

        return Task.CompletedTask;
    }

    protected override async Task DoLockedWork(object? state)
    {
        Logger.LogInformation("Starting official YouTube data fetch");

        var i = 0;
        var neverFetchedVideosCanContinue = true;
        var neverFetchedPlaylistsCanContinue = true;
        var videosCanContinue = true;
        var playlistsCanContinue = true;

        while (YouTubeContext.ApiUsage < 9990 &&
               (neverFetchedVideosCanContinue || videosCanContinue ||
                neverFetchedPlaylistsCanContinue || playlistsCanContinue))
        {
            using var scope = Services.CreateScope();
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
                    await youTubeUow.PlaylistService.UpdateAddedNeverFetchedPlaylistsDataOfficial();
                changesMade = true;
            }
            else if (cycle == 2 && videosCanContinue)
            {
                videosCanContinue = await youTubeUow.VideoService.UpdateAddedVideosOfficial();
                changesMade = true;
            }
            else if (cycle == 3 && playlistsCanContinue)
            {
                playlistsCanContinue = await youTubeUow.PlaylistService.UpdateAddedPlaylistsDataOfficial();
                changesMade = true;
            }

            if (changesMade)
            {
                await uow.SaveChangesAsync();
            }

            i++;
        }
    }
}