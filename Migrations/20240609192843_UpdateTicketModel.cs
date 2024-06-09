using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectItems.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JiraIssueKey",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JiraIssueUrl",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "JiraIssueKey",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "JiraIssueUrl",
                table: "Tickets");
        }
    }
}
