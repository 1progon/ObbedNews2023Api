using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNewsLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "News");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "News",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
