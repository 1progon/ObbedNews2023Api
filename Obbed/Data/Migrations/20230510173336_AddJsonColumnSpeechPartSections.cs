using Microsoft.EntityFrameworkCore.Migrations;
using Obbed.Models.Words.Dictionary;

#nullable disable

namespace ObbedNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonColumnSpeechPartSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<WordSection>(
                name: "WordSection",
                table: "Words",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordSection",
                table: "Words");
        }
    }
}
