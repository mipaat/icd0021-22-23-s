#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class WrongPlaylistTagType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Playlists");
            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "Playlists",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Playlists");
            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "Playlists",
                type: "jsonb",
                nullable: true);
        }
    }
}
