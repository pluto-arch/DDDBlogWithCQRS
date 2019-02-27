using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    public partial class addPostGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Customer",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Article",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostSeries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(nullable: true),
                    OwinUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSeries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_GroupId",
                table: "Article",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Group",
                table: "Article",
                column: "GroupId",
                principalTable: "PostSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Group",
                table: "Article");

            migrationBuilder.DropTable(
                name: "PostSeries");

            migrationBuilder.DropIndex(
                name: "IX_Article_GroupId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Article");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);
        }
    }
}
