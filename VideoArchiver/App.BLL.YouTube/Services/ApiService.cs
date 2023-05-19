using App.BLL.Config;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.DTO;
using App.Common;
using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Services;

public class ApiService : BaseYouTubeService<ApiService>
{
    public ApiService(ServiceUow serviceUow, ILogger<ApiService> logger, YouTubeUow youTubeUow, IMapper mapper) : base(serviceUow,
        logger, youTubeUow, mapper)
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

    private async Task<VideoCategoryListResponse> FetchVideoCategories(IEnumerable<string> categoryIds, string uiCulture = "en-US")
    {
        var request = YouTubeApiService.VideoCategories.List(part: "snippet");
        request.Hl = uiCulture;
        request.Id = string.Join(',', categoryIds);

        return await request.ExecuteAsync(); // Looks like category requests don't support pagination???
    }

    public async Task<ICollection<BasicCategoryData>> FetchVideoCategories(IList<string> categoryIds)
    {
        if (categoryIds.Count == 0) return new List<BasicCategoryData>();
        var supportedUiCultures = ServiceUow.Config.GetSupportedUiCultures();

        var result = categoryIds.Select(c => new BasicCategoryData
        {
            Name = new LangString(),
            IdOnPlatform = c,
        }).ToList();
        await Context.IncrementApiUsage(supportedUiCultures.Count);
        foreach (var uiCulture in supportedUiCultures)
        {
            var response = await FetchVideoCategories(categoryIds, uiCulture);
            foreach (var fetchedCategory in response.Items)
            {
                var category = result.First(c => c.IdOnPlatform == fetchedCategory.Id);
                category.Name[uiCulture] = fetchedCategory.Snippet.Title;
                category.IsAssignable = fetchedCategory.Snippet.Assignable ?? false;
            }
        }

        return result;
    }
}