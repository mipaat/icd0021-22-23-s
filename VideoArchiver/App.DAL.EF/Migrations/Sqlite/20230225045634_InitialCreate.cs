using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IgdbId = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    BoxArtUrl = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    Etag = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSuccessfulFetch = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Etag = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSuccessfulFetch = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AddedToArchiveAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Monitor = table.Column<bool>(type: "INTEGER", nullable: false),
                    Download = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultLanguage = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    DefaultAudioLanguage = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    BitrateBps = table.Column<int>(type: "INTEGER", nullable: true),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    DislikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    CommentCount = table.Column<int>(type: "INTEGER", nullable: true),
                    HasCaptions = table.Column<bool>(type: "INTEGER", nullable: true),
                    Captions = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IsLivestreamRecording = table.Column<bool>(type: "INTEGER", nullable: true),
                    StreamId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    LivestreamStartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LivestreamEndedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LocalVideoFiles = table.Column<string>(type: "TEXT", nullable: true),
                    PrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Etag = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSuccessfulFetch = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AddedToArchiveAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Monitor = table.Column<bool>(type: "INTEGER", nullable: false),
                    Download = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", nullable: true),
                    ProfileImages = table.Column<string>(type: "TEXT", nullable: true),
                    Banners = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Etag = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSuccessfulFetch = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AddedToArchiveAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Monitor = table.Column<bool>(type: "INTEGER", nullable: false),
                    Download = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastValidAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistHistories_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IgdbId = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    BoxArtUrl = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    FromTimecode = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    ToTimecode = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    ValidSince = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoGames_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultLanguage = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    DefaultAudioLanguage = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    BitrateBps = table.Column<int>(type: "INTEGER", nullable: true),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    DislikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    CommentCount = table.Column<int>(type: "INTEGER", nullable: true),
                    HasCaptions = table.Column<bool>(type: "INTEGER", nullable: true),
                    Captions = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    IsLivestreamRecording = table.Column<bool>(type: "INTEGER", nullable: true),
                    StreamId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    LivestreamStartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LivestreamEndedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LocalVideoFiles = table.Column<string>(type: "TEXT", nullable: true),
                    LastValidAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoHistories_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", nullable: true),
                    ProfileImages = table.Column<string>(type: "TEXT", nullable: true),
                    Banners = table.Column<string>(type: "TEXT", nullable: true),
                    Thumbnails = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastValidAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorHistories_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorPubSubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LeasedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LeaseDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorPubSubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorPubSubs_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    SubscriberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubscriptionTargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorSubscriptions_Authors_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorSubscriptions_Authors_SubscriptionTargetId",
                        column: x => x.SubscriptionTargetId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsAuthors = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsVideos = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupportsPlaylists = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAssignable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Platform = table.Column<string>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Authors_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReplyTargetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConversationRootId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    DislikeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyCount = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAtVideoTimecode = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    InternalPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Etag = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    LastFetched = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FetchSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    AddedToArchiveAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ConversationRootId",
                        column: x => x.ConversationRootId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ReplyTargetId",
                        column: x => x.ReplyTargetId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: false),
                    ExpiresIn = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    TokenType = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalUserTokens_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistAuthors_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistSubscriptions_Authors_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistSubscriptions_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AddedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RemovedById = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistVideos_Authors_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistVideos_Authors_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistVideos_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueueItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    IdOnPlatform = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Monitor = table.Column<bool>(type: "INTEGER", nullable: false),
                    Download = table.Column<bool>(type: "INTEGER", nullable: false),
                    WebHookUrl = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    WebhookSecret = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    WebhookData = table.Column<string>(type: "TEXT", nullable: true),
                    AddedById = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueItems_AspNetUsers_AddedById",
                        column: x => x.AddedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueueItems_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueueItems_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueueItems_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueueItems_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PreviousAvailability = table.Column<bool>(type: "INTEGER", nullable: true),
                    NewAvailability = table.Column<bool>(type: "INTEGER", nullable: true),
                    PreviousPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    NewPrivacyStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusChangeEvents_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusChangeEvents_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusChangeEvents_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoAuthors_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoUploadNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoUploadNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoUploadNotifications_Authors_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoUploadNotifications_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutoAssign = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorCategories_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RatedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RaterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorRatings_Authors_RatedId",
                        column: x => x.RatedId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorRatings_Authors_RaterId",
                        column: x => x.RaterId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorRatings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutoAssign = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistCategories_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistRatings_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistRatings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistRatings_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutoAssign = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoCategories_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoRatings_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoRatings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoRatings_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentReplyNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReplyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReplyNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReplyNotifications_Authors_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentReplyNotifications_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentReplyNotifications_Comments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistVideoPositionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaylistVideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    ValidSince = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistVideoPositionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistVideoPositionHistories_PlaylistVideos_PlaylistVideoId",
                        column: x => x.PlaylistVideoId,
                        principalTable: "PlaylistVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StatusChangeEventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusChangeNotifications_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusChangeNotifications_StatusChangeEvents_StatusChangeEventId",
                        column: x => x.StatusChangeEventId,
                        principalTable: "StatusChangeEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorCategories_AuthorId",
                table: "AuthorCategories",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorCategories_CategoryId",
                table: "AuthorCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorHistories_AuthorId",
                table: "AuthorHistories",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorPubSubs_AuthorId",
                table: "AuthorPubSubs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRatings_CategoryId",
                table: "AuthorRatings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRatings_RatedId",
                table: "AuthorRatings",
                column: "RatedId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRatings_RaterId",
                table: "AuthorRatings",
                column: "RaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSubscriptions_SubscriberId",
                table: "AuthorSubscriptions",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSubscriptions_SubscriptionTargetId",
                table: "AuthorSubscriptions",
                column: "SubscriptionTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplyNotifications_CommentId",
                table: "CommentReplyNotifications",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplyNotifications_ReceiverId",
                table: "CommentReplyNotifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplyNotifications_ReplyId",
                table: "CommentReplyNotifications",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ConversationRootId",
                table: "Comments",
                column: "ConversationRootId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyTargetId",
                table: "Comments",
                column: "ReplyTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId",
                table: "Comments",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserTokens_AuthorId",
                table: "ExternalUserTokens",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserTokens_UserId",
                table: "ExternalUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistAuthors_AuthorId",
                table: "PlaylistAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistAuthors_PlaylistId",
                table: "PlaylistAuthors",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCategories_CategoryId",
                table: "PlaylistCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCategories_PlaylistId",
                table: "PlaylistCategories",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistHistories_PlaylistId",
                table: "PlaylistHistories",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistRatings_AuthorId",
                table: "PlaylistRatings",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistRatings_CategoryId",
                table: "PlaylistRatings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistRatings_PlaylistId",
                table: "PlaylistRatings",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSubscriptions_PlaylistId",
                table: "PlaylistSubscriptions",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSubscriptions_SubscriberId",
                table: "PlaylistSubscriptions",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideoPositionHistories_PlaylistVideoId",
                table: "PlaylistVideoPositionHistories",
                column: "PlaylistVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideos_AddedById",
                table: "PlaylistVideos",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideos_PlaylistId",
                table: "PlaylistVideos",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideos_RemovedById",
                table: "PlaylistVideos",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVideos_VideoId",
                table: "PlaylistVideos",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_AddedById",
                table: "QueueItems",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_ApprovedById",
                table: "QueueItems",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_AuthorId",
                table: "QueueItems",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_PlaylistId",
                table: "QueueItems",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_VideoId",
                table: "QueueItems",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeEvents_AuthorId",
                table: "StatusChangeEvents",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeEvents_PlaylistId",
                table: "StatusChangeEvents",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeEvents_VideoId",
                table: "StatusChangeEvents",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeNotifications_ReceiverId",
                table: "StatusChangeNotifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeNotifications_StatusChangeEventId",
                table: "StatusChangeNotifications",
                column: "StatusChangeEventId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAuthors_AuthorId",
                table: "VideoAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAuthors_VideoId",
                table: "VideoAuthors",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCategories_CategoryId",
                table: "VideoCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoCategories_VideoId",
                table: "VideoCategories",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_GameId",
                table: "VideoGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_VideoId",
                table: "VideoGames",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoHistories_VideoId",
                table: "VideoHistories",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRatings_AuthorId",
                table: "VideoRatings",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRatings_CategoryId",
                table: "VideoRatings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoRatings_VideoId",
                table: "VideoRatings",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoUploadNotifications_ReceiverId",
                table: "VideoUploadNotifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoUploadNotifications_VideoId",
                table: "VideoUploadNotifications",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuthorCategories");

            migrationBuilder.DropTable(
                name: "AuthorHistories");

            migrationBuilder.DropTable(
                name: "AuthorPubSubs");

            migrationBuilder.DropTable(
                name: "AuthorRatings");

            migrationBuilder.DropTable(
                name: "AuthorSubscriptions");

            migrationBuilder.DropTable(
                name: "CommentReplyNotifications");

            migrationBuilder.DropTable(
                name: "ExternalUserTokens");

            migrationBuilder.DropTable(
                name: "PlaylistAuthors");

            migrationBuilder.DropTable(
                name: "PlaylistCategories");

            migrationBuilder.DropTable(
                name: "PlaylistHistories");

            migrationBuilder.DropTable(
                name: "PlaylistRatings");

            migrationBuilder.DropTable(
                name: "PlaylistSubscriptions");

            migrationBuilder.DropTable(
                name: "PlaylistVideoPositionHistories");

            migrationBuilder.DropTable(
                name: "QueueItems");

            migrationBuilder.DropTable(
                name: "StatusChangeNotifications");

            migrationBuilder.DropTable(
                name: "VideoAuthors");

            migrationBuilder.DropTable(
                name: "VideoCategories");

            migrationBuilder.DropTable(
                name: "VideoGames");

            migrationBuilder.DropTable(
                name: "VideoHistories");

            migrationBuilder.DropTable(
                name: "VideoRatings");

            migrationBuilder.DropTable(
                name: "VideoUploadNotifications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "PlaylistVideos");

            migrationBuilder.DropTable(
                name: "StatusChangeEvents");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
