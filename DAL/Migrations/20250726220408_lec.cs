using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class lec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Consultations_ConsultationId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Speaker",
                table: "Lectures");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Consultations_ConsultationId",
                table: "Lectures",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Consultations_ConsultationId",
                table: "Lectures");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Speaker",
                table: "Lectures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Consultations_ConsultationId",
                table: "Lectures",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
