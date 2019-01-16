using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    public partial class alerttb_Article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Article",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldDefaultValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Article",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Article");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Article",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldDefaultValue: 1);
        }
    }
}
