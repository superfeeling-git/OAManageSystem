using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "OmsUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "OmsUser");
        }
    }
}
