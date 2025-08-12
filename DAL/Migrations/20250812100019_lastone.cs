using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class lastone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_UserId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_UserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Complaints",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Complaints",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ApplicationUserId",
                table: "Complaints",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_ApplicationUserId",
                table: "Complaints",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Complaints",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_UserId",
                table: "Complaints",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_UserId",
                table: "Complaints",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
