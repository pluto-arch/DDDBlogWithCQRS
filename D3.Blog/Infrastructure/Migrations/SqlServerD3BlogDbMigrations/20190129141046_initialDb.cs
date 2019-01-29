using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 120, nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 120, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 120, nullable: true),
                    SeoDes = table.Column<string>(maxLength: 200, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 300, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 300, nullable: true),
                    ContentMd = table.Column<string>(type: "text", nullable: false),
                    ContentHtml = table.Column<string>(type: "text", nullable: false),
                    ViewCount = table.Column<int>(nullable: false, defaultValue: 0),
                    CollectedCount = table.Column<int>(nullable: false, defaultValue: 0),
                    PromitCount = table.Column<int>(nullable: false, defaultValue: 0),
                    Author = table.Column<string>(maxLength: 120, nullable: true),
                    Source = table.Column<int>(maxLength: 100, nullable: true),
                    ExternalUrl = table.Column<string>(maxLength: 300, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    SeoTitle = table.Column<string>(maxLength: 100, nullable: true),
                    SeoKeyword = table.Column<string>(maxLength: 100, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 250, nullable: true),
                    AddUserId = table.Column<int>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    ModifyUserId = table.Column<int>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    IsTop = table.Column<bool>(nullable: false, defaultValue: false),
                    IsSlide = table.Column<bool>(nullable: false, defaultValue: false),
                    IsRed = table.Column<bool>(nullable: false, defaultValue: false),
                    IsPublish = table.Column<bool>(nullable: false, defaultValue: false),
                    VerifyUserId = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories",
                        column: x => x.CategoryID,
                        principalTable: "ArticleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_CategoryID",
                table: "Article",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ArticleCategory");
        }
    }
}
