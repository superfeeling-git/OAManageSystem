using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsSysMenuRole_OmsSysMenu_OmsSysMenuMenuID",
                table: "OmsSysMenuRole");

            migrationBuilder.DropIndex(
                name: "IX_OmsSysMenuRole_OmsSysMenuMenuID",
                table: "OmsSysMenuRole");

            migrationBuilder.DropColumn(
                name: "OmsSysMenuMenuID",
                table: "OmsSysMenuRole");

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_MenuID",
                table: "OmsSysMenuRole",
                column: "MenuID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsSysMenuRole_OmsSysMenu_MenuID",
                table: "OmsSysMenuRole",
                column: "MenuID",
                principalTable: "OmsSysMenu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsSysMenuRole_OmsSysMenu_MenuID",
                table: "OmsSysMenuRole");

            migrationBuilder.DropIndex(
                name: "IX_OmsSysMenuRole_MenuID",
                table: "OmsSysMenuRole");

            migrationBuilder.AddColumn<int>(
                name: "OmsSysMenuMenuID",
                table: "OmsSysMenuRole",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_OmsSysMenuMenuID",
                table: "OmsSysMenuRole",
                column: "OmsSysMenuMenuID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsSysMenuRole_OmsSysMenu_OmsSysMenuMenuID",
                table: "OmsSysMenuRole",
                column: "OmsSysMenuMenuID",
                principalTable: "OmsSysMenu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
