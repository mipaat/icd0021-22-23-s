#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class DistinctionBetweenOfficialUnofficialFetch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetched",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);
            
            migrationBuilder.RenameColumn(
                name: "LastFetched",
                table: "Videos",
                newName: "LastFetchOfficial");
            
            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetch",
                table: "Videos",
                newName: "LastSuccessfulFetchOfficial");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchUnofficial",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessfulFetchUnofficial",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchUnofficial",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastSuccessfulFetchUnofficial",
                table: "Videos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetchOfficial",
                table: "Videos",
                nullable: false,
                type: "timestamp with time zone",
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.RenameColumn(
                name: "LastFetchOfficial",
                table: "Videos",
                newName: "LastFetched");

            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetchOfficial",
                table: "Videos",
                newName: "LastSuccessfulFetch");
        }
    }
}
