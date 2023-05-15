using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.BackgroundServices;

public class PlaylistTimedBackgroundService : BaseYouTubeLockedTimedBackgroundService<PlaylistTimedBackgroundService>
{
    public PlaylistTimedBackgroundService(ILogger<PlaylistTimedBackgroundService> logger, IServiceProvider services,
        YouTubeContext youTubeContext) :
        base(logger, services, TimeSpan.FromDays(1), youTubeContext)
    {
    }

    protected override async Task DoLockedWork(object? state)
    {
        var amountUpdated = 50;
        while (amountUpdated == 50)
        {
            using var scope = Services.CreateScope();
            var youTubeUow = scope.GetYouTubeUow();

            amountUpdated = await youTubeUow.PlaylistService.UpdateAddedPlaylistsContentsUnofficial(50);
            await youTubeUow.SaveChangesAsync();
        }
    }
}