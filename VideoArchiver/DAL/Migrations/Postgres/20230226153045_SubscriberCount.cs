using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
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
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "AuthorHistories",
                type: "integer",
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
