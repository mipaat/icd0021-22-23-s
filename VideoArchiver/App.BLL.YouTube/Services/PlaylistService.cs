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
    public PlaylistService(YouTubeUow youTubeUow, ILogger<PlaylistService> logger) : base(youTubeUow, logger)
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
                // TODO: Status changes and notifications (Add StatusChange BG service to general BLL?)???
                failedPlaylist.PrivacyStatus = null;
                failedPlaylist.IsAvailable = false;
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

        await AddAndSetVideos(playlist, playlistData);
        Uow.Playlists.Add(playlist);

        return playlist;
    }

    public async Task AddAndSetVideos(Playlist playlist, VideoData playlistData)
    {
        var position = 0;
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