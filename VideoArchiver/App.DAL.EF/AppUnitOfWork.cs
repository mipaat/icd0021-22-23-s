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
    public IAuthorSubscriptionRepository AuthorSubscriptions => _authorSubscriptions ??= new AuthorSubscriptionRepository(DbContext);

    private ICommentRepository? _comments;
    public ICommentRepository Comments => _comments ??= new CommentRepository(DbContext);

    private IVideoRepository? _videos;
    public IVideoRepository Videos => _videos ??= new VideoRepository(DbContext);

    private ICommentReplyNotificationRepository? _commentReplyNotifications;
    public ICommentReplyNotificationRepository CommentReplyNotifications => _commentReplyNotifications ??= new CommentReplyNotificationRepository(DbContext);

    private IExternalUserTokenRepository? _externalUserTokens;
    public IExternalUserTokenRepository ExternalUserTokens => _externalUserTokens ??= new ExternalUserTokenRepository(DbContext);

    private IGameRepository? _games;
    public IGameRepository Games => _games ??= new GameRepository(DbContext);

    private IPlaylistAuthorRepository? _playlistAuthors;
    public IPlaylistAuthorRepository PlaylistAuthors => _playlistAuthors ??= new PlaylistAuthorRepository(DbContext);

    private IPlaylistRepository? _playlists;
    public IPlaylistRepository Playlists => _playlists ??= new PlaylistRepository(DbContext);

    private IPlaylistCategoryRepository? _playlistCategories;
    public IPlaylistCategoryRepository PlaylistCategories => _playlistCategories ??= new PlaylistCategoryRepository(DbContext);
}