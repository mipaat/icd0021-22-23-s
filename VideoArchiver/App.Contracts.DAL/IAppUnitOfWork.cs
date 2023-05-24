using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IBaseUnitOfWork
{
    public IAuthorRepository Authors { get; }
    public IAuthorCategoryRepository AuthorCategories { get; }
    public ICategoryRepository Categories { get; }
    public IAuthorHistoryRepository AuthorHistories { get; }
    public IAuthorRatingRepository AuthorRatings { get; }
    public ICommentRepository Comments { get; }
    public ICommentHistoryRepository CommentHistories { get; }
    public IPlaylistAuthorRepository PlaylistAuthors { get; }
    public IPlaylistCategoryRepository PlaylistCategories { get; }
    public IPlaylistRepository Playlists { get; }
    public IPlaylistHistoryRepository PlaylistHistories { get; }
    public IPlaylistRatingRepository PlaylistRatings { get; }
    public IPlaylistSubscriptionRepository PlaylistSubscriptions { get; }
    public IPlaylistVideoPositionHistoryRepository PlaylistVideoPositionHistories { get; }
    public IPlaylistVideoRepository PlaylistVideos { get; }
    public IQueueItemRepository QueueItems { get; }
    public IStatusChangeEventRepository StatusChangeEvents { get; }
    public IStatusChangeNotificationRepository StatusChangeNotifications { get; }
    public IVideoAuthorRepository VideoAuthors { get; }
    public IVideoCategoryRepository VideoCategories { get; }
    public IVideoHistoryRepository VideoHistories { get; }
    public IVideoRatingRepository VideoRatings { get; }
    public IVideoRepository Videos { get; }

    public IRefreshTokenRepository RefreshTokens { get; }
    public IUserRepository Users { get; }
    
    public IApiQuotaUsageRepository ApiQuotaUsages { get; }
    public IEntityAccessPermissionRepository EntityAccessPermissions { get; }
}