using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemOption_MenuItem_MenuItemId",
                table: "MenuItemOption");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemVariation_MenuItem_MenuItemId",
                table: "MenuItemVariation");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemVariation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemOption",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemOption_MenuItem_MenuItemId",
                table: "MenuItemOption",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemVariation_MenuItem_MenuItemId",
                table: "MenuItemVariation",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemOption_MenuItem_MenuItemId",
                table: "MenuItemOption");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemVariation_MenuItem_MenuItemId",
                table: "MenuItemVariation");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemVariation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemId",
                table: "MenuItemOption",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemOption_MenuItem_MenuItemId",
                table: "MenuItemOption",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemVariation_MenuItem_MenuItemId",
                table: "MenuItemVariation",
                column: "MenuItemId",
                principalTable: "MenuItem",
                principalColumn: "Id");
        }
    }
}
