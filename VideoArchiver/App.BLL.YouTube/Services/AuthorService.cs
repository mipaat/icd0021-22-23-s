﻿using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class AuthorService : BaseYouTubeService<AuthorService>
{
    private readonly Dictionary<string, Author> _cachedAuthors = new();

    public AuthorService(ServiceUow serviceUow, ILogger<AuthorService> logger, YouTubeUow youTubeUow, IMapper mapper) : base(serviceUow,
        logger, youTubeUow, mapper)
    {
    }

    public async Task AddAndSetAuthorIfNotSet(Video domainVideo, VideoData videoData)
    {
        var domainAuthor = await AddOrGetAuthor(videoData);
        await Uow.VideoAuthors.SetVideoAuthor(domainVideo, domainAuthor);
    }

    public async Task AddAndSetAuthorIfNotSet(Comment domainComment, CommentData commentData)
    {
        var domainAuthor = await AddOrGetAuthor(commentData);
        domainComment.Author = domainAuthor;
    }

    public async Task AddAndSetAuthorIfNotSet(Playlist domainPlaylist, VideoData playlistData)
    {
        var domainAuthor = await AddOrGetAuthor(playlistData);
        await Uow.PlaylistAuthors.SetPlaylistAuthor(domainPlaylist, domainAuthor);
    }

    private async Task<Author> AddOrGetAuthor(VideoData videoData)
    {
        return await AddOrGetAuthor(videoData.ChannelID, () => videoData.ToDalAuthor());
    }

    private async Task<Author> AddOrGetAuthor(CommentData commentData)
    {
        return await AddOrGetAuthor(commentData.AuthorID, commentData.ToDalAuthor);
    }

    private async Task<Author> AddOrGetAuthor(string id, Func<Author> newAuthorFunc)
    {
        var author = await GetDbOrCachedAuthor(id);
        if (author == null)
        {
            author = newAuthorFunc();
            Uow.Authors.Add(author);
            await ServiceUow.ImageService.UpdateProfileImages(author);
            if (!_cachedAuthors.ContainsKey(id))
            {
                _cachedAuthors[id] = author;
            }
        }

        return author;
    }

    private async Task<Author?> GetDbOrCachedAuthor(string id)
    {
        var author = _cachedAuthors.GetValueOrDefault(id);
        if (author == null)
        {
            author = await Uow.Authors.GetByIdOnPlatformAsync(id, Platform.YouTube);
            if (author != null && !_cachedAuthors.ContainsKey(id))
            {
                _cachedAuthors[id] = author;
            }
        }

        return author;
    }
}