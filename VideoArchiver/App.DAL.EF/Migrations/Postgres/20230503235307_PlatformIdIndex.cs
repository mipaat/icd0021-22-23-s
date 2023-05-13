#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class PlatformIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Videos_Platform_IdOnPlatform",
                table: "Videos",
                columns: new[] { "Platform", "IdOnPlatform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_Platform_IdOnPlatform",
                table: "Playlists",
                columns: new[] { "Platform", "IdOnPlatform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Platform_IdOnPlatform",
                table: "Comments",
                columns: new[] { "Platform", "IdOnPlatform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Platform_IdOnPlatform",
                table: "Authors",
                columns: new[] { "Platform", "IdOnPlatform" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Videos_Platform_IdOnPlatform",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_Platform_IdOnPlatform",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Platform_IdOnPlatform",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Authors_Platform_IdOnPlatform",
                table: "Authors");
        }
    }
}
