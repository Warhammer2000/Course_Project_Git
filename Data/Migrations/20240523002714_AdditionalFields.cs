using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectItems.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.AddColumn<string>(
		        name: "AdditionalFields",
		        table: "Items",
		        type: "nvarchar(max)",
		        nullable: true);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropColumn(
		        name: "AdditionalFields",
		        table: "Items");
		}
    }
}
