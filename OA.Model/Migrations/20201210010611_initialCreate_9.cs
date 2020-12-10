using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsSysMenuRole_OmsRoles_OmsRolesId",
                table: "OmsSysMenuRole");

            migrationBuilder.DropIndex(
                name: "IX_OmsSysMenuRole_OmsRolesId",
                table: "OmsSysMenuRole");

            migrationBuilder.DropColumn(
                name: "OmsRolesId",
                table: "OmsSysMenuRole");

            migrationBuilder.AlterColumn<long>(
                name: "RoleID",
                table: "OmsSysMenuRole",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_RoleID",
                table: "OmsSysMenuRole",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsSysMenuRole_OmsRoles_RoleID",
                table: "OmsSysMenuRole",
                column: "RoleID",
                principalTable: "OmsRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsSysMenuRole_OmsRoles_RoleID",
                table: "OmsSysMenuRole");

            migrationBuilder.DropIndex(
                name: "IX_OmsSysMenuRole_RoleID",
                table: "OmsSysMenuRole");

            migrationBuilder.AlterColumn<int>(
                name: "RoleID",
                table: "OmsSysMenuRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "OmsRolesId",
                table: "OmsSysMenuRole",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_OmsRolesId",
                table: "OmsSysMenuRole",
                column: "OmsRolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsSysMenuRole_OmsRoles_OmsRolesId",
                table: "OmsSysMenuRole",
                column: "OmsRolesId",
                principalTable: "OmsRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
