using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackton.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class columnprocessedat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "Video",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "Video");
        }
    }
}
