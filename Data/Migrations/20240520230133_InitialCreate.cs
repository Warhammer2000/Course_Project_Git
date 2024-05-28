using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProjectItems.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (NEWID(), 'User', 'USER')");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE NormalizedName = 'USER'");
		}
    }
}
