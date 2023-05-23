using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDraftBooleanColumnOnWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Words",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Words");
        }
    }
}
