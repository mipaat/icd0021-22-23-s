using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class PlaylistService : BaseYouTubeService<PlaylistService>
{
    public PlaylistService(ServiceUow serviceUow, ILogger<PlaylistService> logger, YouTubeUow youTubeUow, IMapper mapper) : base(
        serviceUow, logger, youTubeUow, mapper)
    {
    }

    public async Task<VideoData> FetchPlaylistDataYtdl(string id, CancellationToken ct = default)
    {
        var playlistResult = await YoutubeDl.RunVideoDataFetch(Url.ToPlaylistUrl(id), ct: ct);
        if (playlistResult is not { Success: true })
        {
            ct.ThrowIfCancellationRequested();

            var failedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
            if (failedPlaylist != null)
            {
                await YouTubeUow.ServiceUow.StatusChangeService.Push(new StatusChangeEvent(failedPlaylist, null,
                    false));
            }

            throw new PlaylistNotFoundOnPlatformException(id, EPlatform.YouTube);
        }

        return playlistResult.Data;
    }

    public async Task<Playlist> AddOrGetPlaylist(string id)
    {
        var playlist = await Uow.Playlists.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
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
        var playlist = playlistData.ToDalPlaylist();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(playlist, playlistData);
        await ServiceUow.ImageService.UpdateThumbnails(playlist);

        await AddAndSetVideos(playlist, playlistData);
        Uow.Playlists.Add(playlist);

        return playlist;
    }

    public async Task<int> UpdateAddedPlaylistsContentsUnofficial(int limit)
    {
        var playlists = await Uow.Playlists.GetAllWithContentsUpdatedBefore(EPlatform.YouTube,
            DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), limit);
        foreach (var playlist in playlists)
        {
            await UpdateAddedPlaylistContentsUnofficial(playlist);
        }

        return playlists.Count;
    }

    private async Task UpdateAddedPlaylistContentsUnofficial(PlaylistWithBasicVideoData playlist)
    {
        var fetched = await FetchPlaylistDataYtdl(playlist.IdOnPlatform);
        var position = 0;
        foreach (var entry in fetched.Entries)
        {
            var existingPlaylistVideo = playlist.PlaylistVideos.SingleOrDefault(e => e.Video.IdOnPlatform == entry.ID);
            if (existingPlaylistVideo != null)
            {
                if (position != existingPlaylistVideo.Position)
                {
                    Uow.PlaylistVideoPositionHistories.Add(new PlaylistVideoPositionHistory
                    {
                        PlaylistVideo = existingPlaylistVideo,
                        Position = existingPlaylistVideo.Position,
                        ValidUntil = DateTime.UtcNow,
                    });
                    existingPlaylistVideo.Position = position;
                }
            }
            else
            {
                var video = await YouTubeUow.VideoService.AddOrGetVideo(entry.ID);
                playlist.PlaylistVideos.Add(new BasicPlaylistVideo
                {
                    AddedAt = DateTime.UtcNow,
                    Position = position,
                    Video = new BasicVideoData
                    {
                        Id = video.Id,
                        IdOnPlatform = video.IdOnPlatform,
                        Platform = video.Platform,
                    }
                });
            }

            position++;
        }

        foreach (var playlistVideo in playlist.PlaylistVideos)
        {
            if (fetched.Entries.All(e => e.ID != playlistVideo.Video.IdOnPlatform))
            {
                playlistVideo.RemovedAt = DateTime.UtcNow;
            }
        }

        Uow.Playlists.UpdateContents(playlist);
    }

    public async Task<bool> UpdateAddedNeverFetchedPlaylistsDataOfficial()
    {
        lock (Context.PlaylistUpdateLock)
        {
            if (Context.PlaylistUpdateOngoing) return false;
            Context.PlaylistUpdateOngoing = true;
        }

        var playlists = await Uow.Playlists.GetAllNotOfficiallyFetched(EPlatform.YouTube);
        var result = await UpdateAddedPlaylistsDataOfficial(playlists);

        lock (Context.PlaylistUpdateLock)
        {
            Context.PlaylistUpdateOngoing = false;
        }

        return result;
    }

    public async Task<bool> UpdateAddedPlaylistsDataOfficial()
    {
        lock (Context.PlaylistUpdateLock)
        {
            if (Context.PlaylistUpdateOngoing) return false;
            Context.PlaylistUpdateOngoing = true;
        }

        var playlists =
            await Uow.Playlists.GetAllBeforeOfficialApiFetch(EPlatform.YouTube,
                DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), 50);
        var result = await UpdateAddedPlaylistsDataOfficial(playlists);

        lock (Context.PlaylistUpdateLock)
        {
            Context.PlaylistUpdateOngoing = false;
        }

        return result;
    }

    private async Task<bool> UpdateAddedPlaylistsDataOfficial(ICollection<Playlist> playlists)
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
            var domainPlaylist = fetchedPlaylist.ToDalPlaylist(playlist.Thumbnails);
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
                PlaylistId = playlist.Id,
                Video = domainVideo,
                VideoId = domainVideo.Id,

                Position = position++,
            };

            Uow.PlaylistVideos.Add(playlistVideo);
        }
    }
}