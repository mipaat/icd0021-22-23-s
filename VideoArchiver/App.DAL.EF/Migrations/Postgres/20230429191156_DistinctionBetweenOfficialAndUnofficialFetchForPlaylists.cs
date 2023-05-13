#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class DistinctionBetweenOfficialAndUnofficialFetchForPlaylists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetched",
                table: "Playlists",
                type: "timestamp with time zone",
                nullable: true);
            
            migrationBuilder.RenameColumn(
                name: "LastFetched",
                table: "Playlists",
                newName: "LastFetchOfficial");
            
            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetch",
                table: "Playlists",
                newName: "LastSuccessfulFetchOfficial");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchUnofficial",
                table: "Playlists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessfulFetchUnofficial",
                table: "Playlists",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchUnofficial",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "LastSuccessfulFetchUnofficial",
                table: "Playlists");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetchOfficial",
                table: "Playlists",
                nullable: false,
                type: "timestamp with time zone",
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.RenameColumn(
                name: "LastFetchOfficial",
                table: "Playlists",
                newName: "LastFetched");

            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetchOfficial",
                table: "Playlists",
                newName: "LastSuccessfulFetch");
        }
    }
}
