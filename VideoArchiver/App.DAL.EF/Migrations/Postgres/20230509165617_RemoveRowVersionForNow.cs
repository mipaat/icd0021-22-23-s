using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class RemoveRowVersionForNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoUploadNotifications");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoRatings");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoHistories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoCategories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VideoAuthors");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "StatusChangeNotifications");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "StatusChangeEvents");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "QueueItems");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistVideos");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistVideoPositionHistories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistSubscriptions");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistRatings");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistHistories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistCategories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PlaylistAuthors");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "ExternalUserTokens");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CommentReplyNotifications");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CommentHistories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AuthorSubscriptions");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AuthorRatings");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AuthorPubSubs");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AuthorHistories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "AuthorCategories");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "ApiQuotaUsages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoUploadNotifications",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Videos",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoRatings",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoHistories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoGames",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoCategories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VideoAuthors",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "StatusChangeNotifications",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "StatusChangeEvents",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RefreshTokens",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "QueueItems",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistVideos",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistVideoPositionHistories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistSubscriptions",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Playlists",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistRatings",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistHistories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistCategories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PlaylistAuthors",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Games",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "ExternalUserTokens",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Comments",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CommentReplyNotifications",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CommentHistories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Categories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AuthorSubscriptions",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Authors",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AuthorRatings",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AuthorPubSubs",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AuthorHistories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "AuthorCategories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "ApiQuotaUsages",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }
    }
}
