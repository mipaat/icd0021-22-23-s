using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Domain;
using App.Domain.Enums;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class PlaylistService : BaseYouTubeService<PlaylistService>
{
    public PlaylistService(ServiceUow serviceUow, ILogger<PlaylistService> logger, YouTubeUow youTubeUow) : base(
        serviceUow, logger, youTubeUow)
    {
    }

    public async Task<VideoData> FetchPlaylistDataYtdl(string id, CancellationToken ct = default)
    {
        var playlistResult = await YoutubeDl.RunVideoDataFetch(Url.ToPlaylistUrl(id), ct: ct);
        if (playlistResult is not { Success: true })
        {
            ct.ThrowIfCancellationRequested();

            var failedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, Platform.YouTube);
            if (failedPlaylist != null)
            {
                await YouTubeUow.ServiceUow.StatusChangeService.Push(new StatusChangeEvent(failedPlaylist, null,
                    false));
            }

            throw new PlaylistNotFoundOnPlatformException(id, Platform.YouTube);
        }

        return playlistResult.Data;
    }

    public async Task<Playlist> AddOrGetPlaylist(string id)
    {
        var playlist = await Uow.Playlists.GetByIdOnPlatformAsync(id, Platform.YouTube);
        if (playlist != null)
        {
            return playlist;
        }

        return await AddPlaylist(id);
    }

    public async Task<Playlist> AddPlaylist(string id)
    {
        return await AddPlaylist(await FetchPlaylistDataYtdl(id));
    }

    public async Task<Playlist> AddPlaylist(VideoData playlistData)
    {
        var playlist = playlistData.ToDomainPlaylist();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(playlist, playlistData);
        await ServiceUow.ImageService.UpdateThumbnails(playlist);

        await AddAndSetVideos(playlist, playlistData);
        Uow.Playlists.Add(playlist);

        return playlist;
    }

    // TODO: Update playlist items!!!

    public async Task UpdateAddedPlaylistUnofficial(Playlist playlist)
    {
        // TODO: write this and use
    }

    public async Task<bool> UpdateAddedNeverFetchedPlaylistsOfficial()
    {
        lock (Context.PlaylistUpdateLock)
        {
            if (Context.PlaylistUpdateOngoing) return false;
            Context.PlaylistUpdateOngoing = true;
        }

        var playlists = await Uow.Playlists.GetAllNotOfficiallyFetched(Platform.YouTube);
        var result = await UpdateAddedPlaylistsOfficial(playlists);

        lock (Context.PlaylistUpdateLock)
        {
            Context.PlaylistUpdateOngoing = false;
        }

        return result;
    }

    public async Task<bool> UpdateAddedPlaylistsOfficial()
    {
        lock (Context.PlaylistUpdateLock)
        {
            if (Context.PlaylistUpdateOngoing) return false;
            Context.PlaylistUpdateOngoing = true;
        }

        var playlists =
            await Uow.Playlists.GetAllBeforeOfficialApiFetch(Platform.YouTube,
                DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), 50);
        var result = await UpdateAddedPlaylistsOfficial(playlists);

        lock (Context.PlaylistUpdateLock)
        {
            Context.PlaylistUpdateOngoing = false;
        }

        return result;
    }

    private async Task<bool> UpdateAddedPlaylistsOfficial(ICollection<Playlist> playlists)
    {
        if (playlists.Count == 0) return false;
        var fetchedPlaylists =
            await YouTubeUow.ApiService.FetchPlaylists(playlists.Select(p => p.IdOnPlatform).ToList());
        foreach (var playlist in playlists)
        {
            playlist.LastFetchOfficial = DateTime.UtcNow;
            var fetchedPlaylist = fetchedPlaylists.Items.SingleOrDefault(p => p.Id == playlist.IdOnPlatform);
            if (fetchedPlaylist == null) continue;
            playlist.LastSuccessfulFetchOfficial = playlist.LastFetchOfficial;
            var domainPlaylist = fetchedPlaylist.ToDomainPlaylist(playlist.Thumbnails);
            await ServiceUow.ImageService.UpdateThumbnails(domainPlaylist);
            await ServiceUow.EntityUpdateService.UpdatePlaylist(playlist, domainPlaylist);
        }

        return playlists.Count == 50;
    }

    public async Task AddAndSetVideos(Playlist playlist, VideoData playlistData)
    {
        var position = 0;
        playlist.LastVideosFetch = DateTime.UtcNow;
        foreach (var video in playlistData.Entries)
        {
            var domainVideo = await YouTubeUow.VideoService.AddOrGetVideo(video.ID);
            var playlistVideo = new PlaylistVideo
            {
                Playlist = playlist,
                Video = domainVideo,

                Position = position++,
            };

            Uow.PlaylistVideos.Add(playlistVideo);
        }
    }
}