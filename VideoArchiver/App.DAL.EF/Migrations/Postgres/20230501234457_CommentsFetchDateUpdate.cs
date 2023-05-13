#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.EF.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class CommentsFetchDateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FetchSuccess",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LastFetched",
                table: "Comments");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchOfficial",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchUnofficial",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessfulFetchOfficial",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessfulFetchUnofficial",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchOfficial",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LastFetchUnofficial",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LastSuccessfulFetchOfficial",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LastSuccessfulFetchUnofficial",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "FetchSuccess",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetched",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
