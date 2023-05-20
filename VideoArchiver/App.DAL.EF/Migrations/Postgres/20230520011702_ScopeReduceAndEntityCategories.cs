using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ScopeReduceAndEntityCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorPubSubs");

            migrationBuilder.DropTable(
                name: "AuthorSubscriptions");

            migrationBuilder.DropTable(
                name: "CommentReplyNotifications");

            migrationBuilder.DropTable(
                name: "ExternalUserTokens");

            migrationBuilder.DropTable(
                name: "VideoGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropColumn(
                name: "AutoAssign",
                table: "VideoCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedById",
                table: "VideoCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedById",
                table: "PlaylistCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedById",
                table: "AuthorCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoCategories_AssignedById",
                table: "VideoCategories",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCategories_AssignedById",
                table: "PlaylistCategories",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorCategories_AssignedById",
                table: "AuthorCategories",
                column: "AssignedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorCategories_Authors_AssignedById",
                table: "AuthorCategories",
                column: "AssignedById",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistCategories_Authors_AssignedById",
                table: "PlaylistCategories",
                column: "AssignedById",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCategories_Authors_AssignedById",
                table: "VideoCategories",
                column: "AssignedById",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorCategories_Authors_AssignedById",
                table: "AuthorCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistCategories_Authors_AssignedById",
                table: "PlaylistCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCategories_Authors_AssignedById",
                table: "VideoCategories");

            migrationBuilder.DropIndex(
                name: "IX_VideoCategories_AssignedById",
                table: "VideoCategories");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistCategories_AssignedById",
                table: "PlaylistCategories");

            migrationBuilder.DropIndex(
                name: "IX_AuthorCategories_AssignedById",
                table: "AuthorCategories");

            migrationBuilder.DropColumn(
                name: "AssignedById",
                table: "VideoCategories");

            migrationBuilder.DropColumn(
                name: "AssignedById",
                table: "PlaylistCategories");

            migrationBuilder.DropColumn(
                name: "AssignedById",
                table: "AuthorCategories");

            migrationBuilder.AddColumn<bool>(
                name: "AutoAssign",
                table: "VideoCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AuthorPubSubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaseDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    LeasedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Secret = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionTargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastFetched = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Platform = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
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
                name: "CommentReplyNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReplyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "ExternalUserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    ExpiresIn = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    Scope = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    TokenType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
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
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxArtUrl = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    Etag = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IgdbId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    LastFetched = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSuccessfulFetch = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: true),
                    VideoId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxArtUrl = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    FromTimecode = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IdOnPlatform = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IgdbId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Platform = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ToTimecode = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ValidSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_AuthorPubSubs_AuthorId",
                table: "AuthorPubSubs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSubscriptions_SubscriberId",
                table: "AuthorSubscriptions",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSubscriptions_SubscriptionTargetId",
                table: "AuthorSubscriptions",
                column: "SubscriptionTargetId");

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
                name: "IX_ExternalUserTokens_AuthorId",
                table: "ExternalUserTokens",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserTokens_UserId",
                table: "ExternalUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_GameId",
                table: "VideoGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_VideoId",
                table: "VideoGames",
                column: "VideoId");
        }
    }
}
