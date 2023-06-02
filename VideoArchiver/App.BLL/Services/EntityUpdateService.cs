using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.Extensions;
using App.DAL.DTO.Base;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common;
using App.DAL.Contracts;

namespace App.BLL.Services;

public class EntityUpdateService : IEntityUpdateService
{
    private readonly IServiceUow? _serviceUow;
    private IAppUnitOfWork? Uow => _serviceUow?.Uow;

    public EntityUpdateService(IServiceUow? serviceUow)
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

        comment.LikeCount = StatisticsUpdater.UpdateStatistic(comment.LikeCount, newCommentData.LikeCount, comment,
            newCommentData,
            ref changed);
        comment.LikeCount = UpdateValueIgnoreNull(comment.LikeCount, newCommentData.LikeCount, ref changed);
        comment.DislikeCount = UpdateValueIgnoreNull(comment.DislikeCount, newCommentData.DislikeCount, ref changed);
        comment.ReplyCount = UpdateValueIgnoreNull(comment.ReplyCount, newCommentData.ReplyCount, ref changed);
        comment.IsFavorited = UpdateValueIgnoreNull(comment.IsFavorited, newCommentData.IsFavorited, ref changed);
        comment.AuthorIsCreator ??= newCommentData.AuthorIsCreator;

        comment.CreatedAtVideoTimecode ??= newCommentData.CreatedAtVideoTimecode;
        comment.DeletedAt ??= newCommentData.DeletedAt;

        await UpdateBaseEntity(comment, newCommentData);
        Uow?.Comments.Update(comment);

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

        if (video.Duration != null && newVideoData.Duration != null)
        {
            UpdateChanged(ref changed, newVideoData.Duration > video.Duration.Value.Add(TimeSpan.FromSeconds(5)) ||
                                       newVideoData.Duration < video.Duration.Value.Subtract(TimeSpan.FromSeconds(5)));
        }

        video.Duration = newVideoData.Duration ?? video.Duration;

        video.ViewCount =
            StatisticsUpdater.UpdateStatistic(video.ViewCount, newVideoData.ViewCount, video, newVideoData,
                ref changed);
        video.LikeCount =
            StatisticsUpdater.UpdateStatistic(video.LikeCount, newVideoData.LikeCount, video, newVideoData,
                ref changed);
        video.DislikeCount =
            StatisticsUpdater.UpdateStatistic(video.DislikeCount, newVideoData.DislikeCount, video, newVideoData,
                ref changed);
        video.CommentCount =
            StatisticsUpdater.UpdateStatistic(video.CommentCount, newVideoData.CommentCount, video, newVideoData,
                ref changed);

        video.Captions = UpdateCaptions(video.Captions, newVideoData.Captions, ref changed);
        video.AutomaticCaptions =
            UpdateCaptions(video.AutomaticCaptions, newVideoData.AutomaticCaptions, ref changed);
        video.Thumbnails = UpdateThumbnails(video.Thumbnails, newVideoData.Thumbnails, ref changed);
        video.Tags = UpdateStringListUnordered(video.Tags, newVideoData.Tags, ref changed);
        video.LocalVideoFiles = UpdateVideoFiles(video.LocalVideoFiles, newVideoData.LocalVideoFiles, ref changed);

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
        Uow?.Videos.Update(video);

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
        playlist.Tags = UpdateStringListUnordered(playlist.Tags, newPlaylistData.Tags, ref changed);

        playlist.PublishedAt = UpdateValueIgnoreNull(playlist.PublishedAt, newPlaylistData.PublishedAt, ref changed);
        playlist.LastVideosFetch = newPlaylistData.LastVideosFetch ?? playlist.LastVideosFetch;

        await UpdateBaseEntity(playlist, newPlaylistData);
        Uow?.Playlists.Update(playlist);

        if (changed)
        {
            Uow?.PlaylistHistories.Add(playlistHistory);
        }
    }

    private ImageFileList? UpdateThumbnails(ImageFileList? thumbnails, ImageFileList? newThumbnails,
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

    private static CaptionsDictionary? UpdateCaptions(CaptionsDictionary? oldValue, CaptionsDictionary? newValue,
        ref bool changed)
    {
        return UpdateValueIgnoreNull(oldValue, newValue, ref changed, UpdateCaptionsInternal);
    }

    private static CaptionsDictionary UpdateCaptionsInternal(CaptionsDictionary oldValue, CaptionsDictionary newValue,
        ref bool changed)
    {
        var valueComparer = new Domain.Comparers.CaptionsDictionaryValueComparer();
        UpdateChanged(ref changed, !valueComparer.EqualsExpression.Compile().Invoke(oldValue, newValue));
        return newValue;
    }

    private static List<string>? UpdateStringListUnordered(List<string>? oldValue, List<string>? newValue,
        ref bool changed)
    {
        return UpdateValueIgnoreNull(oldValue, newValue, ref changed, UpdateStringListUnorderedInternal);
    }

    private static List<string> UpdateStringListUnorderedInternal(List<string> oldValue, List<string> newValue, ref bool changed)
    {
        var valueComparer = new Domain.Comparers.StringListUnorderedValueComparer();
        UpdateChanged(ref changed, !valueComparer.EqualsExpression.Compile().Invoke(oldValue, newValue));
        return newValue;
    }

    private static List<VideoFile>? UpdateVideoFiles(List<VideoFile>? oldValue, List<VideoFile>? newValue,
        ref bool changed)
    {
        return UpdateValueIgnoreNull(oldValue, newValue, ref changed, UpdateVideoFilesInternal);
    }

    private static List<VideoFile> UpdateVideoFilesInternal(List<VideoFile> oldValue, List<VideoFile> newValue,
        ref bool changed)
    {
        if (oldValue.Count != newValue.Count)
        {
            changed = true;
            return newValue;
        }

        foreach (var videoFile in oldValue.OrderBy(v => v.FilePath))
        {
            if (newValue.Any(v => v.FilePath == videoFile.FilePath)) continue;
            changed = true;
            return newValue;
        }

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

internal static class StatisticsUpdater
{
    private static bool IsChangeNotMild(long oldValue, long newValue) =>
        (newValue > oldValue * 1.2 || newValue < oldValue / 1.2) &&
        (newValue > oldValue + 20 || newValue < oldValue - 20);

    private static bool IsChangeNotTooFrequent(BaseArchiveEntityNonMonitored oldEntity,
        BaseArchiveEntityNonMonitored newEntity) =>
        oldEntity.LastSuccessfulFetchUnofficial <
        (newEntity.LastSuccessfulFetchUnofficial ?? DateTime.UtcNow)
        .Subtract(TimeSpan.FromDays(30));

    private static bool IsChangeSignificant(long oldValue, long newValue) =>
        newValue > oldValue * 2 || newValue < oldValue / 2 ||
        newValue > oldValue + 100000 || newValue < oldValue - 20000;

    public static long? UpdateStatistic(long? oldValue, long? newValue, BaseArchiveEntityNonMonitored oldEntity,
        BaseArchiveEntityNonMonitored newEntity, ref bool changed)
    {
        if (newValue == null) return oldValue;
        if (oldValue == null) return newValue;
        changed = changed ||
                  (IsChangeNotTooFrequent(oldEntity, newEntity) && IsChangeNotMild(oldValue.Value, newValue.Value)) ||
                  IsChangeSignificant(oldValue.Value, newValue.Value);
        return newValue;
    }

    private static bool IsChangeNotMild(int oldValue, int newValue) =>
        (newValue > oldValue * 1.2 || newValue < oldValue / 1.2) &&
        (newValue > oldValue + 20 || newValue < oldValue - 20);

    private static bool IsChangeSignificant(int oldValue, int newValue) =>
        newValue > oldValue * 2 || newValue < oldValue / 2 ||
        newValue > oldValue + 100000 || newValue < oldValue - 20000;

    public static int? UpdateStatistic(int? oldValue, int? newValue, BaseArchiveEntityNonMonitored oldEntity,
        BaseArchiveEntityNonMonitored newEntity, ref bool changed)
    {
        if (newValue == null) return oldValue;
        if (oldValue == null) return newValue;
        changed = changed ||
                  (IsChangeNotTooFrequent(oldEntity, newEntity) && IsChangeNotMild(oldValue.Value, newValue.Value)) ||
                  IsChangeSignificant(oldValue.Value, newValue.Value);
        return newValue;
    }
}