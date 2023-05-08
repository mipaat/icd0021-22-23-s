using App.BLL.Extensions;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Base;
using App.Domain.NotMapped;

namespace App.BLL.Services;

public class EntityUpdateService
{
    private readonly ServiceUow? _serviceUow;
    private IAppUnitOfWork? Uow => _serviceUow?.Uow;

    public EntityUpdateService(ServiceUow? serviceUow)
    {
        _serviceUow = serviceUow;
    }

    // TODO: Other update methods

    public async Task UpdateComment(Comment comment, Comment newCommentData)
    {
        if (newCommentData.Etag != null && newCommentData.Etag == comment.Etag) return;

        var changed = false;
        var commentHistory = comment.ToHistory();

        comment.Content = UpdateValueIgnoreNull(comment.Content, newCommentData.Content, ref changed);

        comment.LikeCount = UpdateValueIgnoreNull(comment.LikeCount, newCommentData.LikeCount, ref changed);
        comment.DislikeCount = UpdateValueIgnoreNull(comment.DislikeCount, newCommentData.DislikeCount, ref changed);
        comment.ReplyCount = UpdateValueIgnoreNull(comment.ReplyCount, newCommentData.ReplyCount, ref changed);
        comment.IsFavorited = UpdateValueIgnoreNull(comment.IsFavorited, newCommentData.IsFavorited, ref changed);
        comment.AuthorIsCreator ??= newCommentData.AuthorIsCreator;

        comment.CreatedAtVideoTimecode ??= newCommentData.CreatedAtVideoTimecode;
        comment.DeletedAt ??= newCommentData.DeletedAt;

        await UpdateBaseEntity(comment, newCommentData);

        if (changed)
        {
            Uow?.CommentHistories.Add(commentHistory);
        }
    }

    public async Task UpdateVideo(Video video, Video newVideoData)
    {
        if (newVideoData.Etag != null && newVideoData.Etag == video.Etag) return;

        // TODO: Signal download service for captions/thumbnails/video files if changed?
        var changed = false;
        var videoHistory = video.ToHistory();

        video.Title = UpdateLangString(video.Title, newVideoData.Title, ref changed, video.DefaultLanguage);
        video.Description =
            UpdateLangString(video.Description, newVideoData.Description, ref changed, video.DefaultLanguage);

        video.Duration = UpdateValueIgnoreNull(video.Duration, newVideoData.Duration, ref changed);

        video.ViewCount = UpdateValueIgnoreNull(video.ViewCount, newVideoData.ViewCount, ref changed);
        video.LikeCount = UpdateValueIgnoreNull(video.LikeCount, newVideoData.LikeCount, ref changed);
        video.DislikeCount = UpdateValueIgnoreNull(video.DislikeCount, newVideoData.DislikeCount, ref changed);
        video.CommentCount = UpdateValueIgnoreNull(video.CommentCount, newVideoData.CommentCount, ref changed);

        // TODO: Custom logic
        video.Captions = UpdateValueIgnoreNull(video.Captions, newVideoData.Captions, ref changed);
        // TODO: Custom logic
        video.AutomaticCaptions =
            UpdateValueIgnoreNull(video.AutomaticCaptions, newVideoData.AutomaticCaptions, ref changed);
        video.Thumbnails = UpdateThumbnails(video.Thumbnails, newVideoData.Thumbnails, ref changed);
        // TODO: Custom logic
        video.Tags = UpdateValueIgnoreNull(video.Tags, newVideoData.Tags, ref changed);
        // TODO: Custom logic
        video.LocalVideoFiles = UpdateValueIgnoreNull(video.LocalVideoFiles, newVideoData.LocalVideoFiles, ref changed);

        video.IsLivestreamRecording =
            UpdateValueIgnoreNull(video.IsLivestreamRecording, newVideoData.IsLivestreamRecording, ref changed);
        video.StreamId = UpdateValueIgnoreNull(video.StreamId, newVideoData.StreamId, ref changed);
        video.LivestreamStartedAt =
            UpdateValueIgnoreNull(video.LivestreamStartedAt, newVideoData.LivestreamStartedAt, ref changed);
        video.LivestreamEndedAt =
            UpdateValueIgnoreNull(video.LivestreamEndedAt, newVideoData.LivestreamEndedAt, ref changed);

        video.PublishedAt = UpdateValueIgnoreNull(video.PublishedAt, newVideoData.PublishedAt, ref changed);
        video.RecordedAt = UpdateValueIgnoreNull(video.RecordedAt, newVideoData.RecordedAt, ref changed);

        video.LastCommentsFetch = newVideoData.LastCommentsFetch ?? video.LastCommentsFetch;

        await UpdateBaseEntity(video, newVideoData);

        if (changed)
        {
            Uow?.VideoHistories.Add(videoHistory);
        }
    }

    public async Task UpdatePlaylist(Playlist playlist, Playlist newPlaylistData)
    {
        if (newPlaylistData.Etag != null && newPlaylistData.Etag == playlist.Etag) return;

        var changed = false;
        var playlistHistory = playlist.ToHistory();

        playlist.Title = UpdateLangString(playlist.Title, newPlaylistData.Title, ref changed, playlist.DefaultLanguage);
        playlist.Description = UpdateLangString(playlist.Description, newPlaylistData.Description, ref changed,
            playlist.DefaultLanguage);

        playlist.Thumbnails = UpdateThumbnails(playlist.Thumbnails, newPlaylistData.Thumbnails, ref changed);
        // TODO: Custom logic
        playlist.Tags = UpdateValueIgnoreNull(playlist.Tags, newPlaylistData.Tags, ref changed);

        playlist.PublishedAt = UpdateValueIgnoreNull(playlist.PublishedAt, newPlaylistData.PublishedAt, ref changed);
        playlist.LastVideosFetch = newPlaylistData.LastVideosFetch ?? playlist.LastVideosFetch;

        await UpdateBaseEntity(playlist, newPlaylistData);

        if (changed)
        {
            Uow?.PlaylistHistories.Add(playlistHistory);
        }
    }

    public ImageFileList? UpdateThumbnails(ImageFileList? thumbnails, ImageFileList? newThumbnails,
        ref bool changed)
    {
        return UpdateValueIgnoreNull(thumbnails, newThumbnails, ref changed,
            (thumbnailsI, newThumbnailsI) => thumbnailsI.All(newThumbnailsI.Contains));
    }

    private async Task UpdateBaseEntity<TEntity>(TEntity entity, TEntity newEntityData)
        where TEntity : BaseArchiveEntityNonMonitored
    {
        entity.CreatedAt ??= newEntityData.CreatedAt;
        entity.UpdatedAt ??= newEntityData.UpdatedAt ?? DateTime.UtcNow;

        var statusChangeEvent = entity switch
        {
            Video video => new StatusChangeEvent(video, newEntityData.PrivacyStatus, newEntityData.IsAvailable,
                newEntityData.UpdatedAt),
            Playlist playlist => new StatusChangeEvent(playlist, newEntityData.PrivacyStatus, newEntityData.IsAvailable,
                newEntityData.UpdatedAt),
            Author author => new StatusChangeEvent(author, newEntityData.PrivacyStatus, newEntityData.IsAvailable,
                newEntityData.UpdatedAt),
            _ => null
        };

        if (statusChangeEvent != null && _serviceUow != null)
        {
            await _serviceUow.StatusChangeService.Push(statusChangeEvent);
        }

        entity.PrivacyStatus ??= newEntityData.PrivacyStatus;
        entity.IsAvailable = newEntityData.IsAvailable;
        entity.InternalPrivacyStatus = newEntityData.InternalPrivacyStatus;

        entity.Etag = newEntityData.Etag ?? entity.Etag;
        entity.LastFetchOfficial = newEntityData.LastFetchOfficial ?? entity.LastFetchOfficial;
        entity.LastSuccessfulFetchOfficial =
            newEntityData.LastSuccessfulFetchOfficial ?? entity.LastSuccessfulFetchOfficial;
        entity.LastFetchUnofficial = newEntityData.LastFetchUnofficial ?? entity.LastFetchUnofficial;
        entity.LastSuccessfulFetchUnofficial =
            newEntityData.LastSuccessfulFetchUnofficial ?? entity.LastSuccessfulFetchUnofficial;
    }

    private static void UpdateChanged(ref bool changed, bool addition)
    {
        changed = changed || addition;
    }

    private static T? UpdateValueIgnoreNull<T>(T? oldValue, T? newValue, ref bool changed,
        Func<T, T, bool> customChangedFunc)
    {
        if (newValue == null) return oldValue;
        if (oldValue == null) return newValue;
        UpdateChanged(ref changed, customChangedFunc(oldValue, newValue));
        return newValue;
    }

    private static T? UpdateValueIgnoreNull<T>(T? oldValue, T? newValue, ref bool changed,
        CustomUpdateFunc<T>? customUpdateFunc = null)
    {
        if (newValue == null) return oldValue;
        if (oldValue == null) return newValue;
        if (customUpdateFunc != null) return customUpdateFunc(oldValue, newValue, ref changed);
        UpdateChanged(ref changed, !oldValue.Equals(newValue));
        return newValue;
    }

    private static LangString? UpdateLangString(LangString? oldValue, LangString? newValue, ref bool changed,
        string? oldLanguage)
    {
        return UpdateValueIgnoreNull(oldValue, newValue, ref changed,
            (LangString valueOld, LangString valueNew, ref bool changedInner) =>
                UpdateLangStringInternal(valueOld, valueNew, ref changedInner, oldLanguage));
    }

    private static LangString UpdateLangStringInternal(LangString oldValue, LangString newValue, ref bool changed,
        string? oldLanguage)
    {
        var result = oldValue;
        if (newValue.IsUnspecifiedVersionOf(oldValue)) return result;
        if (newValue.IsUnspecified)
        {
            if (oldLanguage != null)
            {
                UpdateChanged(ref changed,
                    newValue[LangString.UnknownCulture] !=
                    oldValue.GetValueOrDefault(LangString.UnknownCulture) ||
                    newValue[LangString.UnknownCulture] != oldValue.GetValueOrDefault(oldLanguage));
                result[oldLanguage] = newValue[LangString.UnknownCulture];
            }
            else
            {
                UpdateChanged(ref changed,
                    newValue[LangString.UnknownCulture] !=
                    oldValue.GetValueOrDefault(LangString.UnknownCulture));
                result[LangString.UnknownCulture] = newValue[LangString.UnknownCulture];
            }
        }
        else
        {
            UpdateChanged(ref changed, !newValue.IsSpecifiedVersionOf(oldValue));
            result = newValue;
        }

        return result;
    }
}

internal delegate T CustomUpdateFunc<T>(T oldValue, T newValue, ref bool changed);