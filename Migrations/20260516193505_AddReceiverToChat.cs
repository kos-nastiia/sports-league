using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsLeague.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiverToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "ChatMessages",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "ChatMessages");
        }
    }
}
