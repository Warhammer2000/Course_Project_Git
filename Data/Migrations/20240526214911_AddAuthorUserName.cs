using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectItems.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorUserName",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorUserName",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorUserName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuthorUserName",
                table: "Collections");
        }
    }
}
