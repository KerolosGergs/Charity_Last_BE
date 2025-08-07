using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class pages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPages_AspNetUsers_CreatedBy",
                table: "DynamicPages");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPages_AspNetUsers_UpdatedBy",
                table: "DynamicPages");

            migrationBuilder.DropIndex(
                name: "IX_DynamicPages_CreatedBy",
                table: "DynamicPages");

            migrationBuilder.DropIndex(
                name: "IX_DynamicPages_UpdatedBy",
                table: "DynamicPages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DynamicPages_CreatedBy",
                table: "DynamicPages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPages_UpdatedBy",
                table: "DynamicPages",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPages_AspNetUsers_CreatedBy",
                table: "DynamicPages",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPages_AspNetUsers_UpdatedBy",
                table: "DynamicPages",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
