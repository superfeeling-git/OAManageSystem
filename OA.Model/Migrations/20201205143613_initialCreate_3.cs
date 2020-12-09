using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_OmsBlogCategoryCategoryID",
                table: "OmsBlog");

            migrationBuilder.DropIndex(
                name: "IX_OmsBlog_OmsBlogCategoryCategoryID",
                table: "OmsBlog");

            migrationBuilder.DropColumn(
                name: "OmsBlogCategoryCategoryID",
                table: "OmsBlog");

            migrationBuilder.CreateIndex(
                name: "IX_OmsBlog_CategoryID",
                table: "OmsBlog",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_CategoryID",
                table: "OmsBlog",
                column: "CategoryID",
                principalTable: "OmsBlogCategory",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_CategoryID",
                table: "OmsBlog");

            migrationBuilder.DropIndex(
                name: "IX_OmsBlog_CategoryID",
                table: "OmsBlog");

            migrationBuilder.AddColumn<int>(
                name: "OmsBlogCategoryCategoryID",
                table: "OmsBlog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OmsBlog_OmsBlogCategoryCategoryID",
                table: "OmsBlog",
                column: "OmsBlogCategoryCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_OmsBlogCategoryCategoryID",
                table: "OmsBlog",
                column: "OmsBlogCategoryCategoryID",
                principalTable: "OmsBlogCategory",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
