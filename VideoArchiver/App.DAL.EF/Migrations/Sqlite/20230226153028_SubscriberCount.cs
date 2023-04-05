using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class SubscriberCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "AuthorHistories",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriberCount",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "SubscriberCount",
                table: "AuthorHistories");
        }
    }
}
