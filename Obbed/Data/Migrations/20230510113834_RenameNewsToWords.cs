using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameNewsToWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_NewsId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideos_News_NewsId",
                table: "NewsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideoSections_News_NewsId",
                table: "NewsVideoSections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsFavorites_News_NewsId",
                table: "UserNewsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsLikes_News_NewsId",
                table: "UserNewsLikes");

            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "UserNewsLikes",
                newName: "WordId");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "UserNewsFavorites",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNewsFavorites_NewsId",
                table: "UserNewsFavorites",
                newName: "IX_UserNewsFavorites_WordId");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsVideoSections",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideoSections_NewsId",
                table: "NewsVideoSections",
                newName: "IX_NewsVideoSections_WordId");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsVideos",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideos_NewsId",
                table: "NewsVideos",
                newName: "IX_NewsVideos_WordId");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "Comments",
                newName: "WordId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_NewsId",
                table: "Comments",
                newName: "IX_Comments_WordId");

            migrationBuilder.CreateTable(
                name: "TagWord",
                columns: table => new
                {
                    TagsId = table.Column<long>(type: "bigint", nullable: false),
                    WordListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagWord", x => new { x.TagsId, x.WordListId });
                    table.ForeignKey(
                        name: "FK_TagWord_News_WordListId",
                        column: x => x.WordListId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagWord_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagWord_WordListId",
                table: "TagWord",
                column: "WordListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_WordId",
                table: "Comments",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVideos_News_WordId",
                table: "NewsVideos",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVideoSections_News_WordId",
                table: "NewsVideoSections",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsFavorites_News_WordId",
                table: "UserNewsFavorites",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsLikes_News_WordId",
                table: "UserNewsLikes",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_WordId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideos_News_WordId",
                table: "NewsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideoSections_News_WordId",
                table: "NewsVideoSections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsFavorites_News_WordId",
                table: "UserNewsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsLikes_News_WordId",
                table: "UserNewsLikes");

            migrationBuilder.DropTable(
                name: "TagWord");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "UserNewsLikes",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "UserNewsFavorites",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNewsFavorites_WordId",
                table: "UserNewsFavorites",
                newName: "IX_UserNewsFavorites_NewsId");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "NewsVideoSections",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideoSections_WordId",
                table: "NewsVideoSections",
                newName: "IX_NewsVideoSections_NewsId");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "NewsVideos",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideos_WordId",
                table: "NewsVideos",
                newName: "IX_NewsVideos_NewsId");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "Comments",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_WordId",
                table: "Comments",
                newName: "IX_Comments_NewsId");

            migrationBuilder.CreateTable(
                name: "NewsTag",
                columns: table => new
                {
                    NewsListId = table.Column<long>(type: "bigint", nullable: false),
                    TagsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTag", x => new { x.NewsListId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_NewsTag_News_NewsListId",
                        column: x => x.NewsListId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagsId",
                table: "NewsTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_NewsId",
                table: "Comments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVideos_News_NewsId",
                table: "NewsVideos",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVideoSections_News_NewsId",
                table: "NewsVideoSections",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsFavorites_News_NewsId",
                table: "UserNewsFavorites",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsLikes_News_NewsId",
                table: "UserNewsLikes",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
