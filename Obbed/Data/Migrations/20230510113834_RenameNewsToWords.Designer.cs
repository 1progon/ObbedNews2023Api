﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Obbed.Data;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230510113834_RenameNewsToWords")]
    partial class RenameNewsToWords
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Obbed.Models.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenExpire")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Obbed.Models.Middle.UserWordFavorite", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "WordId");

                    b.HasIndex("WordId");

                    b.ToTable("UserNewsFavorites");
                });

            modelBuilder.Entity("Obbed.Models.Middle.UserWordLike", b =>
                {
                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("LikeType")
                        .HasColumnType("integer");

                    b.HasKey("WordId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserNewsLikes");
                });

            modelBuilder.Entity("Obbed.Models.Payment.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sign")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Obbed.Models.Payment.PayPalOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PayPalOrderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PayPalOrderStatus")
                        .HasColumnType("integer");

                    b.Property<string>("PayPalRequestId")
                        .HasColumnType("text");

                    b.Property<int>("PaymentPlan")
                        .HasColumnType("integer");

                    b.Property<int>("PaymentSystem")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PayPalOrders");
                });

            modelBuilder.Entity("Obbed.Models.Payment.Price", b =>
                {
                    b.Property<int>("System")
                        .HasColumnType("integer");

                    b.Property<int>("Plan")
                        .HasColumnType("integer");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Sum")
                        .HasColumnType("numeric");

                    b.HasKey("System", "Plan", "CurrencyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("Obbed.Models.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "slug");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Obbed.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Obbed.Models.Words.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<long?>("ParentCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "slug");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Obbed.Models.Words.ParentCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "slug");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("ParentCategories");
                });

            modelBuilder.Entity("Obbed.Models.Words.Word", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Article")
                        .HasColumnType("text");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "createdAt");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long>("DislikesCount")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsFree")
                        .HasColumnType("boolean");

                    b.Property<long>("LikesCount")
                        .HasColumnType("bigint");

                    b.Property<long>("LikesRate")
                        .HasColumnType("bigint");

                    b.Property<string>("MainImage")
                        .HasColumnType("text");

                    b.Property<string>("MainThumb")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("NewsLink")
                        .HasColumnType("text");

                    b.Property<bool>("Popular")
                        .HasColumnType("boolean");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "slug");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "updatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("News");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Dislikes")
                        .HasColumnType("bigint");

                    b.Property<long>("Likes")
                        .HasColumnType("bigint");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ParentCommentId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.HasIndex("WordId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordVideo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "createdAt");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Filename")
                        .HasColumnType("text");

                    b.Property<string>("Folder")
                        .HasColumnType("text");

                    b.Property<bool>("IsFree")
                        .HasColumnType("boolean");

                    b.Property<string>("MainThumb")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("RemoteUrl")
                        .HasColumnType("text");

                    b.Property<long>("SectionId")
                        .HasColumnType("bigint");

                    b.Property<int>("SortNumber")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "updatedAt");

                    b.Property<string>("VideoLength")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.HasIndex("WordId");

                    b.ToTable("NewsVideos");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordVideoSection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<bool>("IsFree")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<int>("SortNumber")
                        .HasColumnType("integer");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("NewsVideoSections");
                });

            modelBuilder.Entity("TagWord", b =>
                {
                    b.Property<long>("TagsId")
                        .HasColumnType("bigint");

                    b.Property<long>("WordListId")
                        .HasColumnType("bigint");

                    b.HasKey("TagsId", "WordListId");

                    b.HasIndex("WordListId");

                    b.ToTable("TagWord");
                });

            modelBuilder.Entity("Obbed.Models.Middle.UserWordFavorite", b =>
                {
                    b.HasOne("Obbed.Models.User", "User")
                        .WithMany("UserNewsFavorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obbed.Models.Words.Word", "Word")
                        .WithMany("UserNewsFavorites")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Obbed.Models.Middle.UserWordLike", b =>
                {
                    b.HasOne("Obbed.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obbed.Models.Words.Word", "Word")
                        .WithMany("UserNewsLikes")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Obbed.Models.Payment.PayPalOrder", b =>
                {
                    b.HasOne("Obbed.Models.User", "User")
                        .WithMany("PayPalOrders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Obbed.Models.Payment.Price", b =>
                {
                    b.HasOne("Obbed.Models.Payment.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Obbed.Models.User", b =>
                {
                    b.HasOne("Obbed.Models.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("Obbed.Models.User", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Obbed.Models.Words.Category", b =>
                {
                    b.HasOne("Obbed.Models.Words.ParentCategory", "ParentCategory")
                        .WithMany("Categories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Obbed.Models.Words.Word", b =>
                {
                    b.HasOne("Obbed.Models.Words.Category", "Category")
                        .WithMany("WordList")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordComment", b =>
                {
                    b.HasOne("Obbed.Models.Words.WordComment", "ParentComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("Obbed.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obbed.Models.Words.Word", "Word")
                        .WithMany("Comments")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("User");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordVideo", b =>
                {
                    b.HasOne("Obbed.Models.Words.WordVideoSection", "Section")
                        .WithMany("Videos")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obbed.Models.Words.Word", "Word")
                        .WithMany()
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordVideoSection", b =>
                {
                    b.HasOne("Obbed.Models.Words.Word", "Word")
                        .WithMany("VideoSections")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("TagWord", b =>
                {
                    b.HasOne("Obbed.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Obbed.Models.Words.Word", null)
                        .WithMany()
                        .HasForeignKey("WordListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Obbed.Models.Account", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("Obbed.Models.User", b =>
                {
                    b.Navigation("PayPalOrders");

                    b.Navigation("UserNewsFavorites");
                });

            modelBuilder.Entity("Obbed.Models.Words.Category", b =>
                {
                    b.Navigation("WordList");
                });

            modelBuilder.Entity("Obbed.Models.Words.ParentCategory", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Obbed.Models.Words.Word", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserNewsFavorites");

                    b.Navigation("UserNewsLikes");

                    b.Navigation("VideoSections");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordComment", b =>
                {
                    b.Navigation("ChildComments");
                });

            modelBuilder.Entity("Obbed.Models.Words.WordVideoSection", b =>
                {
                    b.Navigation("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
