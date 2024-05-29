using System;
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
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoolName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoolName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoolName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeStringName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeStringName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeStringName3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StyleConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dark = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StyleConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntCustom1 = table.Column<int>(type: "int", nullable: false),
                    IntCustom2 = table.Column<int>(type: "int", nullable: false),
                    IntCustom3 = table.Column<int>(type: "int", nullable: false),
                    StringCustom1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringCustom2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StringCustom3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCustom1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCustom2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCustom3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoolCustom1 = table.Column<bool>(type: "bit", nullable: false),
                    BoolCustom2 = table.Column<bool>(type: "bit", nullable: false),
                    BoolCustom3 = table.Column<bool>(type: "bit", nullable: false),
                    LargeDescriptionCustom1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeDescriptionCustom2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeDescriptionCustom3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ItemId",
                table: "Comments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CollectionId",
                table: "Items",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ItemId",
                table: "Likes",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "StyleConnections");

            migrationBuilder.DropTable(
                name: "TagConnections");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");
        }
    }
}
