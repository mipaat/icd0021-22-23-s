using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Common;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class AuthorService : BaseYouTubeService<AuthorService>
{
    private readonly Dictionary<string, AuthorBasic> _cachedAuthors = new();

    public AuthorService(ServiceUow serviceUow, ILogger<AuthorService> logger, YouTubeUow youTubeUow, IMapper mapper) : base(serviceUow,
        logger, youTubeUow, mapper)
    {
    }

    public async Task AddAndSetAuthorIfNotSet(Video video, VideoData videoData)
    {
        var author = await AddOrGetAuthor(videoData);
        await Uow.VideoAuthors.SetVideoAuthor(video.Id, author.Id);
    }

    public async Task AddAndSetAuthorIfNotSet(Playlist playlist, VideoData playlistData)
    {
        var author = await AddOrGetAuthor(playlistData);
        await Uow.PlaylistAuthors.SetPlaylistAuthor(playlist.Id, author.Id);
    }

    private async Task<AuthorBasic> AddOrGetAuthor(VideoData videoData)
    {
        return await AddOrGetAuthor(videoData.ChannelID, () => videoData.ToDalAuthor());
    }

    private async Task<AuthorBasic> AddOrGetAuthor(string id, Func<Author> newAuthorFunc)
    {
        return (await AddOrGetAuthors(new[] { new AuthorFetchArg(id, newAuthorFunc) })).First();
    }

    internal async Task<ICollection<AuthorBasic>> AddOrGetAuthors(IEnumerable<AuthorFetchArg> authorFetchArgs)
    {
        ICollection<AuthorBasic> authors = new List<AuthorBasic>();
        var notCachedIds = new List<AuthorFetchArg>();
        foreach (var arg in authorFetchArgs)
        {
            var author = _cachedAuthors.GetValueOrDefault(arg.AuthorId);
            if (author != null)
            {
                authors.Add(author);
            }
            else
            {
                notCachedIds.Add(arg);
            }
        }

        var fetchedAuthors = await Uow.Authors.GetAllBasicByIdsOnPlatformAsync(notCachedIds.Select(e => e.AuthorId), EPlatform.YouTube);

        foreach (var arg in notCachedIds)
        {
            var fetchedAuthor = fetchedAuthors.FirstOrDefault(a => a.IdOnPlatform == arg.AuthorId);
            if (fetchedAuthor != null)
            {
                _cachedAuthors.TryAdd(arg.AuthorId, fetchedAuthor);
                authors.Add(fetchedAuthor);
            }
            else
            {
                var author = arg.NewAuthorFunc();
                var channel = await YouTubeExplodeClient.Channels.GetAsync(author.IdOnPlatform);
                author.ProfileImages = new ImageFileList();
                foreach (var thumbnail in channel.Thumbnails)
                {
                    author.ProfileImages.Add(new ImageFile
                    {
                        Height = thumbnail.Resolution.Height,
                        Width = thumbnail.Resolution.Width,
                        Url = thumbnail.Url,
                        Platform = EPlatform.YouTube,
                    });
                }
                await ServiceUow.ImageService.UpdateProfileImages(author);
                Uow.Authors.Add(author);
                var basicAuthor = Mapper.Map<AuthorBasic>(author);
                _cachedAuthors.TryAdd(arg.AuthorId, basicAuthor);
                authors.Add(basicAuthor);
            }
        }

        return authors;
    }
}

internal record AuthorFetchArg(string AuthorId, Func<Author> NewAuthorFunc);
