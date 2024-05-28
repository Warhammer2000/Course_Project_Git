using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectItems.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIconClassToCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Collections");
        }
    }
}
