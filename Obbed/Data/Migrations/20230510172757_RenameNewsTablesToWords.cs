using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameNewsTablesToWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_WordId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Categories_CategoryId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideos_NewsVideoSections_SectionId",
                table: "NewsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideos_News_WordId",
                table: "NewsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsVideoSections_News_WordId",
                table: "NewsVideoSections");

            migrationBuilder.DropForeignKey(
                name: "FK_TagWord_News_WordListId",
                table: "TagWord");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsFavorites_News_WordId",
                table: "UserNewsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsFavorites_Users_UserId",
                table: "UserNewsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsLikes_News_WordId",
                table: "UserNewsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNewsLikes_Users_UserId",
                table: "UserNewsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNewsLikes",
                table: "UserNewsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNewsFavorites",
                table: "UserNewsFavorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsVideoSections",
                table: "NewsVideoSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsVideos",
                table: "NewsVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            migrationBuilder.RenameTable(
                name: "UserNewsLikes",
                newName: "UserWordsLikes");

            migrationBuilder.RenameTable(
                name: "UserNewsFavorites",
                newName: "UserWordsFavorites");

            migrationBuilder.RenameTable(
                name: "NewsVideoSections",
                newName: "WordsVideoSections");

            migrationBuilder.RenameTable(
                name: "NewsVideos",
                newName: "WordsVideos");

            migrationBuilder.RenameTable(
                name: "News",
                newName: "Words");

            migrationBuilder.RenameIndex(
                name: "IX_UserNewsLikes_UserId",
                table: "UserWordsLikes",
                newName: "IX_UserWordsLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNewsFavorites_WordId",
                table: "UserWordsFavorites",
                newName: "IX_UserWordsFavorites_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideoSections_WordId",
                table: "WordsVideoSections",
                newName: "IX_WordsVideoSections_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideos_WordId",
                table: "WordsVideos",
                newName: "IX_WordsVideos_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsVideos_SectionId",
                table: "WordsVideos",
                newName: "IX_WordsVideos_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_News_Slug",
                table: "Words",
                newName: "IX_Words_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_News_CategoryId",
                table: "Words",
                newName: "IX_Words_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWordsLikes",
                table: "UserWordsLikes",
                columns: new[] { "WordId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWordsFavorites",
                table: "UserWordsFavorites",
                columns: new[] { "UserId", "WordId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordsVideoSections",
                table: "WordsVideoSections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordsVideos",
                table: "WordsVideos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Words",
                table: "Words",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Words_WordId",
                table: "Comments",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagWord_Words_WordListId",
                table: "TagWord",
                column: "WordListId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordsFavorites_Users_UserId",
                table: "UserWordsFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordsFavorites_Words_WordId",
                table: "UserWordsFavorites",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordsLikes_Users_UserId",
                table: "UserWordsLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWordsLikes_Words_WordId",
                table: "UserWordsLikes",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Categories_CategoryId",
                table: "Words",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordsVideos_WordsVideoSections_SectionId",
                table: "WordsVideos",
                column: "SectionId",
                principalTable: "WordsVideoSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordsVideos_Words_WordId",
                table: "WordsVideos",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordsVideoSections_Words_WordId",
                table: "WordsVideoSections",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Words_WordId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_TagWord_Words_WordListId",
                table: "TagWord");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordsFavorites_Users_UserId",
                table: "UserWordsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordsFavorites_Words_WordId",
                table: "UserWordsFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordsLikes_Users_UserId",
                table: "UserWordsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWordsLikes_Words_WordId",
                table: "UserWordsLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Words_Categories_CategoryId",
                table: "Words");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsVideos_WordsVideoSections_SectionId",
                table: "WordsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsVideos_Words_WordId",
                table: "WordsVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsVideoSections_Words_WordId",
                table: "WordsVideoSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WordsVideoSections",
                table: "WordsVideoSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WordsVideos",
                table: "WordsVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Words",
                table: "Words");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWordsLikes",
                table: "UserWordsLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWordsFavorites",
                table: "UserWordsFavorites");

            migrationBuilder.RenameTable(
                name: "WordsVideoSections",
                newName: "NewsVideoSections");

            migrationBuilder.RenameTable(
                name: "WordsVideos",
                newName: "NewsVideos");

            migrationBuilder.RenameTable(
                name: "Words",
                newName: "News");

            migrationBuilder.RenameTable(
                name: "UserWordsLikes",
                newName: "UserNewsLikes");

            migrationBuilder.RenameTable(
                name: "UserWordsFavorites",
                newName: "UserNewsFavorites");

            migrationBuilder.RenameIndex(
                name: "IX_WordsVideoSections_WordId",
                table: "NewsVideoSections",
                newName: "IX_NewsVideoSections_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_WordsVideos_WordId",
                table: "NewsVideos",
                newName: "IX_NewsVideos_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_WordsVideos_SectionId",
                table: "NewsVideos",
                newName: "IX_NewsVideos_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Words_Slug",
                table: "News",
                newName: "IX_News_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Words_CategoryId",
                table: "News",
                newName: "IX_News_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWordsLikes_UserId",
                table: "UserNewsLikes",
                newName: "IX_UserNewsLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWordsFavorites_WordId",
                table: "UserNewsFavorites",
                newName: "IX_UserNewsFavorites_WordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsVideoSections",
                table: "NewsVideoSections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsVideos",
                table: "NewsVideos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNewsLikes",
                table: "UserNewsLikes",
                columns: new[] { "WordId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNewsFavorites",
                table: "UserNewsFavorites",
                columns: new[] { "UserId", "WordId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_WordId",
                table: "Comments",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Categories_CategoryId",
                table: "News",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsVideos_NewsVideoSections_SectionId",
                table: "NewsVideos",
                column: "SectionId",
                principalTable: "NewsVideoSections",
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
                name: "FK_TagWord_News_WordListId",
                table: "TagWord",
                column: "WordListId",
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
                name: "FK_UserNewsFavorites_Users_UserId",
                table: "UserNewsFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsLikes_News_WordId",
                table: "UserNewsLikes",
                column: "WordId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNewsLikes_Users_UserId",
                table: "UserNewsLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
