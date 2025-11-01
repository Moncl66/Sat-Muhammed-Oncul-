using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AidatTakipSistemi.Migrations
{
    /// <inheritdoc />
    public partial class DaireKullaniciIliskisiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Daireler",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Daireler_ApplicationUserId",
                table: "Daireler",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Daireler_AspNetUsers_ApplicationUserId",
                table: "Daireler",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Daireler_AspNetUsers_ApplicationUserId",
                table: "Daireler");

            migrationBuilder.DropIndex(
                name: "IX_Daireler_ApplicationUserId",
                table: "Daireler");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Daireler");
        }
    }
}
