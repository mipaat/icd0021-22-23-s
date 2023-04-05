using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using DAL.Repositories.EntityRepositories;

namespace DAL;

public class AppUnitOfWork : BaseUnitOfWork<AbstractAppDbContext>, IAppUnitOfWork
{
    public AppUnitOfWork(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    // TODO: Replace repository return types when C# supports return type covariance for interfaces

    private IAuthorRepository? _authors;
    public IAuthorRepository Authors => _authors ??= new AuthorRepository(DbContext);

    private IAuthorCategoryRepository? _authorCategories;
    public IAuthorCategoryRepository AuthorCategories => _authorCategories ??= new AuthorCategoryRepository(DbContext);

    private ICategoryRepository? _categories;
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(DbContext);

    private IAuthorHistoryRepository? _authorHistories;
    public IAuthorHistoryRepository AuthorHistories => _authorHistories ??= new AuthorHistoryRepository(DbContext);

    private IAuthorPubSubRepository? _authorPubSubs;
    public IAuthorPubSubRepository AuthorPubSubs => _authorPubSubs ??= new AuthorPubSubRepository(DbContext);

    private IAuthorRatingRepository? _authorRatings;
    public IAuthorRatingRepository AuthorRatings => _authorRatings ??= new AuthorRatingRepository(DbContext);

    private IAuthorSubscriptionRepository? _authorSubscriptions;

    public IAuthorSubscriptionRepository AuthorSubscriptions =>
        _authorSubscriptions ??= new AuthorSubscriptionRepository(DbContext);

    private ICommentRepository? _comments;
    public ICommentRepository Comments => _comments ??= new CommentRepository(DbContext);

    private ICommentReplyNotificationRepository? _commentReplyNotifications;

    public ICommentReplyNotificationRepository CommentReplyNotifications =>
        _commentReplyNotifications ??= new CommentReplyNotificationRepository(DbContext);

    private IExternalUserTokenRepository? _externalUserTokens;

    public IExternalUserTokenRepository ExternalUserTokens =>
        _externalUserTokens ??= new ExternalUserTokenRepository(DbContext);

    private IGameRepository? _games;
    public IGameRepository Games => _games ??= new GameRepository(DbContext);

    private IPlaylistAuthorRepository? _playlistAuthors;
    public IPlaylistAuthorRepository PlaylistAuthors => _playlistAuthors ??= new PlaylistAuthorRepository(DbContext);

    private IPlaylistCategoryRepository? _playlistCategories;

    public IPlaylistCategoryRepository PlaylistCategories =>
        _playlistCategories ??= new PlaylistCategoryRepository(DbContext);

    private IPlaylistHistoryRepository? _playlistHistories;

    public IPlaylistHistoryRepository PlaylistHistories =>
        _playlistHistories ??= new PlaylistHistoryRepository(DbContext);

    private IPlaylistRatingRepository? _playlistRatings;
    public IPlaylistRatingRepository PlaylistRatings => _playlistRatings ??= new PlaylistRatingRepository(DbContext);

    private IPlaylistRepository? _playlists;
    public IPlaylistRepository Playlists => _playlists ??= new PlaylistRepository(DbContext);

    private IPlaylistSubscriptionRepository? _playlistSubscriptions;

    public IPlaylistSubscriptionRepository PlaylistSubscriptions =>
        _playlistSubscriptions ??= new PlaylistSubscriptionRepository(DbContext);

    private IPlaylistVideoPositionHistoryRepository? _playlistVideoPositionHistories;

    public IPlaylistVideoPositionHistoryRepository PlaylistVideoPositionHistories => _playlistVideoPositionHistories ??=
        new PlaylistVideoPositionHistoryRepository(DbContext);

    private IPlaylistVideoRepository? _playlistVideos;
    public IPlaylistVideoRepository PlaylistVideos => _playlistVideos ??= new PlaylistVideoRepository(DbContext);

    private IQueueItemRepository? _queueItems;
    public IQueueItemRepository QueueItems => _queueItems ??= new QueueItemRepository(DbContext);

    private IStatusChangeEventRepository? _statusChangeEvents;

    public IStatusChangeEventRepository StatusChangeEvents =>
        _statusChangeEvents ??= new StatusChangeEventRepository(DbContext);

    private IStatusChangeNotificationRepository? _statusChangeNotifications;

    public IStatusChangeNotificationRepository StatusChangeNotifications =>
        _statusChangeNotifications ??= new StatusChangeNotificationRepository(DbContext);

    private IVideoAuthorRepository? _videoAuthors;
    public IVideoAuthorRepository VideoAuthors => _videoAuthors ??= new VideoAuthorRepository(DbContext);

    private IVideoCategoryRepository? _videoCategories;
    public IVideoCategoryRepository VideoCategories => _videoCategories ??= new VideoCategoryRepository(DbContext);

    private IVideoGameRepository? _videoGames;
    public IVideoGameRepository VideoGames => _videoGames ??= new VideoGameRepository(DbContext);

    private IVideoHistoryRepository? _videoHistories;
    public IVideoHistoryRepository VideoHistories => _videoHistories ??= new VideoHistoryRepository(DbContext);

    private IVideoRatingRepository? _videoRatings;
    public IVideoRatingRepository VideoRatings => _videoRatings ??= new VideoRatingRepository(DbContext);

    private IVideoRepository? _videos;
    public IVideoRepository Videos => _videos ??= new VideoRepository(DbContext);

    private IVideoUploadNotificationRepository? _videoUploadNotifications;

    public IVideoUploadNotificationRepository VideoUploadNotifications =>
        _videoUploadNotifications ??= new VideoUploadNotificationRepository(DbContext);
}