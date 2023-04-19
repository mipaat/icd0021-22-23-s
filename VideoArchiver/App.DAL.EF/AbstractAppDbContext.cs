using App.Domain;
using App.Domain.Converters;
using App.Domain.Enums;
using App.Domain.Identity;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AbstractAppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AbstractAppDbContext(DbContextOptions<AbstractAppDbContext> options) : this(options as DbContextOptions)
    {
    }

    public AbstractAppDbContext(DbContextOptions options) : base(options)
    {
    }

    // ASP.NET Core Identity entities' DbSets are mapped in parent class
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public DbSet<Video> Videos { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<Playlist> Playlists { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    public DbSet<VideoAuthor> VideoAuthors { get; set; } = default!;
    public DbSet<PlaylistAuthor> PlaylistAuthors { get; set; } = default!;
    public DbSet<PlaylistVideo> PlaylistVideos { get; set; } = default!;
    public DbSet<VideoGame> VideoGames { get; set; } = default!;

    public DbSet<VideoUploadNotification> VideoUploadNotifications { get; set; } = default!;
    public DbSet<CommentReplyNotification> CommentReplyNotifications { get; set; } = default!;
    public DbSet<PlaylistSubscription> PlaylistSubscriptions { get; set; } = default!;
    public DbSet<AuthorSubscription> AuthorSubscriptions { get; set; } = default!;
    public DbSet<StatusChangeEvent> StatusChangeEvents { get; set; } = default!;
    public DbSet<StatusChangeNotification> StatusChangeNotifications { get; set; } = default!;
    public DbSet<AuthorPubSub> AuthorPubSubs { get; set; } = default!;
    public DbSet<ExternalUserToken> ExternalUserTokens { get; set; } = default!;
    public DbSet<QueueItem> QueueItems { get; set; } = default!;

    public DbSet<VideoCategory> VideoCategories { get; set; } = default!;
    public DbSet<AuthorCategory> AuthorCategories { get; set; } = default!;
    public DbSet<PlaylistCategory> PlaylistCategories { get; set; } = default!;

    public DbSet<VideoRating> VideoRatings { get; set; } = default!;
    public DbSet<PlaylistRating> PlaylistRatings { get; set; } = default!;
    public DbSet<AuthorRating> AuthorRatings { get; set; } = default!;

    public DbSet<PlaylistVideoPositionHistory> PlaylistVideoPositionHistories { get; set; } = default!;
    public DbSet<AuthorHistory> AuthorHistories { get; set; } = default!;
    public DbSet<PlaylistHistory> PlaylistHistories { get; set; } = default!;
    public DbSet<VideoHistory> VideoHistories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Platform>()
            .HaveConversion<PlatformConverter>()
            .HaveMaxLength(64);
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<DateTimeConverter>();
    }
}