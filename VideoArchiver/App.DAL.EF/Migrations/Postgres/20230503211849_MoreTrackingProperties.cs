using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class MoreTrackingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCommentsFetch",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVideosFetch",
                table: "Playlists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommentsFetch",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastVideosFetch",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Comments");
        }
    }
}
