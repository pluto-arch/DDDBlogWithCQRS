using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    public partial class alertTBArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Article");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Article",
                newName: "ContentMd");

            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "Article",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Article",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 120);

            migrationBuilder.AddColumn<string>(
                name: "ContentHtml",
                table: "Article",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Article",
                nullable: false,
                defaultValue: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentHtml",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Article");

            migrationBuilder.RenameColumn(
                name: "ContentMd",
                table: "Article",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Article",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Article",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Article",
                nullable: false,
                defaultValue: 0);
        }
    }
}
