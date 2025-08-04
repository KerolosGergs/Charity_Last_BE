using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class lece : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AspNetUsers_CreatedBy",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CreatedBy",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Lectures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Lectures",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Lectures",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Lectures",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CreatedBy",
                table: "Lectures",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AspNetUsers_CreatedBy",
                table: "Lectures",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
