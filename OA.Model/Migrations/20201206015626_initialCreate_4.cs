using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Model.Migrations
{
    public partial class initialCreate_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_CategoryID",
                table: "OmsBlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OmsBlogCategory",
                table: "OmsBlogCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OmsBlog",
                table: "OmsBlog");

            migrationBuilder.RenameTable(
                name: "OmsBlog",
                newName: "OmsBlogs");

            migrationBuilder.RenameColumn(
                name: "AddTime",
                table: "OmsBlogs",
                newName: "CreateTime");

            migrationBuilder.RenameIndex(
                name: "IX_OmsBlog_CategoryID",
                table: "OmsBlogs",
                newName: "IX_OmsBlogs_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "BlogTitle",
                table: "OmsBlogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "OmsBlogs",
                type: "smalldatetime",
                nullable: false,
                defaultValueSql: "getdate()",
                comment: "博客创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryID",
                table: "OmsBlogCategory",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OmsBlogID",
                table: "OmsBlogs",
                column: "BlogID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryID_BlogID",
                table: "OmsBlogs",
                column: "CategoryID",
                principalTable: "OmsBlogCategory",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryID_BlogID",
                table: "OmsBlogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryID",
                table: "OmsBlogCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OmsBlogID",
                table: "OmsBlogs");

            migrationBuilder.RenameTable(
                name: "OmsBlogs",
                newName: "OmsBlog");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "OmsBlog",
                newName: "AddTime");

            migrationBuilder.RenameIndex(
                name: "IX_OmsBlogs_CategoryID",
                table: "OmsBlog",
                newName: "IX_OmsBlog_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "BlogTitle",
                table: "OmsBlog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddTime",
                table: "OmsBlog",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldDefaultValueSql: "getdate()",
                oldComment: "博客创建时间");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OmsBlogCategory",
                table: "OmsBlogCategory",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OmsBlog",
                table: "OmsBlog",
                column: "BlogID");

            migrationBuilder.AddForeignKey(
                name: "FK_OmsBlog_OmsBlogCategory_CategoryID",
                table: "OmsBlog",
                column: "CategoryID",
                principalTable: "OmsBlogCategory",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
