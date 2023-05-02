using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ExtraCommentProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AuthorIsCreator",
                table: "Comments",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorited",
                table: "Comments",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubscriberCount",
                table: "Authors",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorIsCreator",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsFavorited",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriberCount",
                table: "Authors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
