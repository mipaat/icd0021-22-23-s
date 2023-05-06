using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class NoInternalPrivacyStatusForHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalPrivacyStatus",
                table: "VideoHistories");

            migrationBuilder.DropColumn(
                name: "InternalPrivacyStatus",
                table: "PlaylistHistories");

            migrationBuilder.DropColumn(
                name: "InternalPrivacyStatus",
                table: "CommentHistories");

            migrationBuilder.DropColumn(
                name: "InternalPrivacyStatus",
                table: "AuthorHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternalPrivacyStatus",
                table: "VideoHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalPrivacyStatus",
                table: "PlaylistHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalPrivacyStatus",
                table: "CommentHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalPrivacyStatus",
                table: "AuthorHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
