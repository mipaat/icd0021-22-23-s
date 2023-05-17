using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.DAL.EF.Repositories.EntityRepositories;
using App.DAL.EF.Repositories.EntityRepositories.Identity;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUnitOfWork : BaseUnitOfWork<AbstractAppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;

    public AppUnitOfWork(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    // TODO: Replace repository return types when C# supports return type covariance for interfaces

    private IAuthorRepository? _authors;
    public IAuthorRepository Authors => _authors ??= new AuthorRepository(DbContext, _mapper, this);

    private IAuthorCategoryRepository? _authorCategories;

    public IAuthorCategoryRepository AuthorCategories =>
        _authorCategories ??= new AuthorCategoryRepository(DbContext, _mapper, this);

    private ICategoryRepository? _categories;
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(DbContext, _mapper, this);

    private IAuthorHistoryRepository? _authorHistories;

    public IAuthorHistoryRepository AuthorHistories =>
        _authorHistories ??= new AuthorHistoryRepository(DbContext, _mapper, this);

    private IAuthorPubSubRepository? _authorPubSubs;
    public IAuthorPubSubRepository AuthorPubSubs => _authorPubSubs ??= new AuthorPubSubRepository(DbContext, _mapper, this);

    private IAuthorRatingRepository? _authorRatings;
    public IAuthorRatingRepository AuthorRatings => _authorRatings ??= new AuthorRatingRepository(DbContext, _mapper, this);

    private IAuthorSubscriptionRepository? _authorSubscriptions;

    public IAuthorSubscriptionRepository AuthorSubscriptions =>
        _authorSubscriptions ??= new AuthorSubscriptionRepository(DbContext, _mapper, this);

    private ICommentRepository? _comments;
    public ICommentRepository Comments => _comments ??= new CommentRepository(DbContext, _mapper, this);

    private ICommentHistoryRepository? _commentHistories;

    public ICommentHistoryRepository CommentHistories =>
        _commentHistories ??= new CommentHistoryRepository(DbContext, _mapper, this);

    private ICommentReplyNotificationRepository? _commentReplyNotifications;

    public ICommentReplyNotificationRepository CommentReplyNotifications =>
        _commentReplyNotifications ??= new CommentReplyNotificationRepository(DbContext, _mapper, this);

    private IExternalUserTokenRepository? _externalUserTokens;

    public IExternalUserTokenRepository ExternalUserTokens =>
        _externalUserTokens ??= new ExternalUserTokenRepository(DbContext, _mapper, this);

    private IGameRepository? _games;
    public IGameRepository Games => _games ??= new GameRepository(DbContext, _mapper, this);

    private IPlaylistAuthorRepository? _playlistAuthors;

    public IPlaylistAuthorRepository PlaylistAuthors =>
        _playlistAuthors ??= new PlaylistAuthorRepository(DbContext, _mapper, this);

    private IPlaylistCategoryRepository? _playlistCategories;

    public IPlaylistCategoryRepository PlaylistCategories =>
        _playlistCategories ??= new PlaylistCategoryRepository(DbContext, _mapper, this);

    private IPlaylistHistoryRepository? _playlistHistories;

    public IPlaylistHistoryRepository PlaylistHistories =>
        _playlistHistories ??= new PlaylistHistoryRepository(DbContext, _mapper, this);

    private IPlaylistRatingRepository? _playlistRatings;

    public IPlaylistRatingRepository PlaylistRatings =>
        _playlistRatings ??= new PlaylistRatingRepository(DbContext, _mapper, this);

    private IPlaylistRepository? _playlists;
    public IPlaylistRepository Playlists => _playlists ??= new PlaylistRepository(DbContext, _mapper, this);

    private IPlaylistSubscriptionRepository? _playlistSubscriptions;

    public IPlaylistSubscriptionRepository PlaylistSubscriptions =>
        _playlistSubscriptions ??= new PlaylistSubscriptionRepository(DbContext, _mapper, this);

    private IPlaylistVideoPositionHistoryRepository? _playlistVideoPositionHistories;

    public IPlaylistVideoPositionHistoryRepository PlaylistVideoPositionHistories => _playlistVideoPositionHistories ??=
        new PlaylistVideoPositionHistoryRepository(DbContext, _mapper, this);

    private IPlaylistVideoRepository? _playlistVideos;

    public IPlaylistVideoRepository PlaylistVideos =>
        _playlistVideos ??= new PlaylistVideoRepository(DbContext, _mapper, this);

    private IQueueItemRepository? _queueItems;
    public IQueueItemRepository QueueItems => _queueItems ??= new QueueItemRepository(DbContext, _mapper, this);

    private IStatusChangeEventRepository? _statusChangeEvents;

    public IStatusChangeEventRepository StatusChangeEvents =>
        _statusChangeEvents ??= new StatusChangeEventRepository(DbContext, _mapper, this);

    private IStatusChangeNotificationRepository? _statusChangeNotifications;

    public IStatusChangeNotificationRepository StatusChangeNotifications =>
        _statusChangeNotifications ??= new StatusChangeNotificationRepository(DbContext, _mapper, this);

    private IVideoAuthorRepository? _videoAuthors;
    public IVideoAuthorRepository VideoAuthors => _videoAuthors ??= new VideoAuthorRepository(DbContext, _mapper, this);

    private IVideoCategoryRepository? _videoCategories;

    public IVideoCategoryRepository VideoCategories =>
        _videoCategories ??= new VideoCategoryRepository(DbContext, _mapper, this);

    private IVideoGameRepository? _videoGames;
    public IVideoGameRepository VideoGames => _videoGames ??= new VideoGameRepository(DbContext, _mapper, this);

    private IVideoHistoryRepository? _videoHistories;
    public IVideoHistoryRepository VideoHistories => _videoHistories ??= new VideoHistoryRepository(DbContext, _mapper, this);

    private IVideoRatingRepository? _videoRatings;
    public IVideoRatingRepository VideoRatings => _videoRatings ??= new VideoRatingRepository(DbContext, _mapper, this);

    private IVideoRepository? _videos;
    public IVideoRepository Videos => _videos ??= new VideoRepository(DbContext, _mapper, this);

    private IVideoUploadNotificationRepository? _videoUploadNotifications;

    public IVideoUploadNotificationRepository VideoUploadNotifications =>
        _videoUploadNotifications ??= new VideoUploadNotificationRepository(DbContext, _mapper, this);

    private IApiQuotaUsageRepository? _apiQuotaUsages;

    public IApiQuotaUsageRepository ApiQuotaUsages =>
        _apiQuotaUsages ??= new ApiQuotaUsageRepository(DbContext, _mapper, this);

    private IEntityAccessPermissionRepository? _entityAccessPermissions;
    public IEntityAccessPermissionRepository EntityAccessPermissions => _entityAccessPermissions ??=
        new EntityAccessPermissionRepository(DbContext, _mapper, this);

    // Identity
    private IRefreshTokenRepository? _refreshTokens;
    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(DbContext, _mapper, this);

    private IUserRepository? _users;
    public IUserRepository Users => _users ??= new UserRepository(DbContext, _mapper, this);
}