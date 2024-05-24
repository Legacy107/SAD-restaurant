using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuItemCategoryId",
                table: "MenuItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MenuItemCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuItemCategoryId",
                table: "MenuItem",
                column: "MenuItemCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_MenuItemCategory_MenuItemCategoryId",
                table: "MenuItem",
                column: "MenuItemCategoryId",
                principalTable: "MenuItemCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_MenuItemCategory_MenuItemCategoryId",
                table: "MenuItem");

            migrationBuilder.DropTable(
                name: "MenuItemCategory");

            migrationBuilder.DropIndex(
                name: "IX_MenuItem_MenuItemCategoryId",
                table: "MenuItem");

            migrationBuilder.DropColumn(
                name: "MenuItemCategoryId",
                table: "MenuItem");
        }
    }
}
