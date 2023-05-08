using App.BLL.YouTube.Base;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Services;

public class ApiService : BaseYouTubeService<ApiService>
{
    public ApiService(ServiceUow serviceUow, ILogger<ApiService> logger, YouTubeUow youTubeUow) : base(serviceUow,
        logger, youTubeUow)
    {
    }

    public async Task<VideoListResponse> FetchVideos(IList<string> videoIds)
    {
        if (videoIds.Count == 0) throw new ArgumentException($"At least one video id is required", nameof(videoIds));
        if (videoIds.Count > 50)
            throw new ArgumentException($"Can't fetch more than 50 videos at once", nameof(videoIds));
        var request = YouTubeApiService.Videos.List(
            "id,contentDetails,localizations,liveStreamingDetails,player,recordingDetails,snippet,statistics,status,topicDetails");
        request.MaxResults = 50;
        request.Id = string.Join(",", videoIds);
        await Context.IncrementApiUsage();
        return await request.ExecuteAsync();
    }

    public async Task<PlaylistListResponse> FetchPlaylists(IList<string> playlistIds)
    {
        if (playlistIds.Count == 0) throw new ArgumentException($"At least one playlist id is required", nameof(playlistIds));
        if (playlistIds.Count > 50) throw new ArgumentException($"Can't fetch more than 50 playlists at once", nameof(playlistIds));
        var request = YouTubeApiService.Playlists.List(
            "id,contentDetails,localizations,player,snippet,status");
        request.MaxResults = 50;
        request.Id = string.Join(",", playlistIds);
        await Context.IncrementApiUsage();
        return await request.ExecuteAsync();
    }
}