using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OmsSysMenu",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    ParentPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmsSysMenu", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "OmsSysMenuRole",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    OmsSysMenuMenuID = table.Column<int>(type: "int", nullable: true),
                    OmsRolesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmsSysMenuRole", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OmsSysMenuRole_OmsRoles_OmsRolesId",
                        column: x => x.OmsRolesId,
                        principalTable: "OmsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OmsSysMenuRole_OmsSysMenu_OmsSysMenuMenuID",
                        column: x => x.OmsSysMenuMenuID,
                        principalTable: "OmsSysMenu",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_OmsRolesId",
                table: "OmsSysMenuRole",
                column: "OmsRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_OmsSysMenuRole_OmsSysMenuMenuID",
                table: "OmsSysMenuRole",
                column: "OmsSysMenuMenuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OmsSysMenuRole");

            migrationBuilder.DropTable(
                name: "OmsSysMenu");
        }
    }
}
