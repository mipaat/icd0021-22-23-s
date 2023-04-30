using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AuthorDistinctionBetweenOfficialAndUnofficialFetch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetched",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: true);
            
            migrationBuilder.RenameColumn(
                name: "LastFetched",
                table: "Authors",
                newName: "LastFetchOfficial");
            
            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetch",
                table: "Authors",
                newName: "LastSuccessfulFetchOfficial");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchUnofficial",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSuccessfulFetchUnofficial",
                table: "Authors",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchUnofficial",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "LastSuccessfulFetchUnofficial",
                table: "Authors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastFetchOfficial",
                table: "Authors",
                nullable: false,
                type: "timestamp with time zone",
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.RenameColumn(
                name: "LastFetchOfficial",
                table: "Authors",
                newName: "LastFetched");

            migrationBuilder.RenameColumn(
                name: "LastSuccessfulFetchOfficial",
                table: "Authors",
                newName: "LastSuccessfulFetch");
        }
    }
}
