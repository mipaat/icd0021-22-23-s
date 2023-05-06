using App.BLL.Extensions;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Base;
using App.Domain.NotMapped;

namespace App.BLL;

public class EntityUpdateHandler
{
    private readonly IAppUnitOfWork _uow;

    public EntityUpdateHandler(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // TODO: Other update methods

    public void UpdateComment(Comment comment, Comment newCommentData)
    {
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

        UpdateBaseEntity(comment, newCommentData);

        if (changed)
        {
            _uow.CommentHistories.Add(commentHistory);
        }
    }

    public void UpdateVideo(Video video, Video newVideoData)
    {
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

        // TODO: Custom logic for updating these
        video.Captions = UpdateValueIgnoreNull(video.Captions, newVideoData.Captions, ref changed);
        video.AutomaticCaptions =
            UpdateValueIgnoreNull(video.AutomaticCaptions, newVideoData.AutomaticCaptions, ref changed);
        video.Thumbnails = UpdateValueIgnoreNull(video.Thumbnails, newVideoData.Thumbnails, ref changed);
        video.Tags = UpdateValueIgnoreNull(video.Tags, newVideoData.Tags, ref changed);
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

        UpdateBaseEntity(video, newVideoData);

        if (changed)
        {
            _uow.VideoHistories.Add(videoHistory);
        }
    }

    private void UpdateBaseEntity<TEntity>(TEntity entity, TEntity newEntityData)
        where TEntity : BaseArchiveEntityNonMonitored
    {
        entity.CreatedAt ??= newEntityData.CreatedAt;
        entity.UpdatedAt ??= newEntityData.UpdatedAt ?? DateTime.UtcNow;

        // TODO: Signal status change service
        entity.PrivacyStatus ??= newEntityData.PrivacyStatus;
        entity.IsAvailable = newEntityData.IsAvailable;
        entity.InternalPrivacyStatus = newEntityData.InternalPrivacyStatus;

        entity.Etag = newEntityData.Etag ?? entity.Etag;
        entity.LastFetchOfficial = newEntityData.LastFetchOfficial ?? entity.LastFetchOfficial;
        entity.LastSuccessfulFetchOfficial = newEntityData.LastSuccessfulFetchOfficial ?? entity.LastSuccessfulFetchOfficial;
        entity.LastFetchUnofficial = newEntityData.LastFetchUnofficial ?? entity.LastFetchUnofficial;
        entity.LastSuccessfulFetchUnofficial = newEntityData.LastSuccessfulFetchUnofficial ?? entity.LastSuccessfulFetchUnofficial;
    }

    private static void UpdateChanged(ref bool changed, bool addition)
    {
        changed = UpdateChanged(changed, addition);
    }

    private static bool UpdateChanged(bool changed, bool addition)
    {
        return changed || addition;
    }

    private static T? UpdateValueIgnoreNull<T>(T? oldValue, T? newValue, ref bool changed,
        CustomUpdateFunc<T>? customUpdateFunc = null)
    {
        if (newValue == null) return oldValue;
        if (oldValue == null) return newValue;
        if (customUpdateFunc != null) return customUpdateFunc(oldValue, newValue, ref changed);
        UpdateChanged(changed, !oldValue.Equals(newValue));
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
        if (newValue.IsUnspecified && oldLanguage != null)
        {
            UpdateChanged(ref changed,
                newValue[LangString.UnknownCulture] ==
                oldValue.GetValueOrDefault(LangString.UnknownCulture) ||
                newValue[LangString.UnknownCulture] == oldValue.GetValueOrDefault(oldLanguage));
            result[oldLanguage] = newValue[LangString.UnknownCulture];
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