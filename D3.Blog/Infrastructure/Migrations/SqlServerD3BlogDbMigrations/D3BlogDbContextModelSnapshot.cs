﻿// <auto-generated />
using System;
using Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    [DbContext(typeof(D3BlogDbContext))]
    partial class D3BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime");

                    b.Property<int>("AddUserId");

                    b.Property<int>("ArticleCategoryId");

                    b.Property<string>("Author");

                    b.Property<int>("CollectedCount");

                    b.Property<string>("Content");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsPublish");

                    b.Property<bool>("IsRed");

                    b.Property<bool>("IsSlide");

                    b.Property<bool>("IsTop");

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<int?>("ModifyUserId");

                    b.Property<int>("PromitCount");

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoKeyword");

                    b.Property<string>("SeoTitle");

                    b.Property<int>("Sort");

                    b.Property<string>("Source");

                    b.Property<string>("Title");

                    b.Property<int?>("VerifyUserId");

                    b.Property<int>("ViewCount");

                    b.HasKey("Id");

                    b.HasIndex("ArticleCategoryId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.ArticleCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icon");

                    b.Property<bool>("IsDelete");

                    b.Property<int?>("ParentId");

                    b.Property<string>("SeoDes");

                    b.Property<string>("SeoKeywords");

                    b.Property<string>("SeoTitle");

                    b.Property<int>("Sort");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("ArticleCategory");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnName("Birthday");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(200);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Article", b =>
                {
                    b.HasOne("D3.Blog.Domain.Entitys.ArticleCategory", "ArticleCategory")
                        .WithMany("Article")
                        .HasForeignKey("ArticleCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}