using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AidatTakipSistemi.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Daireler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Blok = table.Column<string>(type: "TEXT", nullable: false),
                    Kat = table.Column<int>(type: "INTEGER", nullable: false),
                    DaireNo = table.Column<int>(type: "INTEGER", nullable: false),
                    SahipAdi = table.Column<string>(type: "TEXT", nullable: false),
                    SakinAdi = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Daireler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aidatlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tutar = table.Column<decimal>(type: "TEXT", nullable: false),
                    Donem = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SonOdemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OdendiMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    DaireId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aidatlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aidatlar_Daireler_DaireId",
                        column: x => x.DaireId,
                        principalTable: "Daireler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OdenenTutar = table.Column<decimal>(type: "TEXT", nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AidatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odemeler_Aidatlar_AidatId",
                        column: x => x.AidatId,
                        principalTable: "Aidatlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aidatlar_DaireId",
                table: "Aidatlar",
                column: "DaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_AidatId",
                table: "Odemeler",
                column: "AidatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odemeler");

            migrationBuilder.DropTable(
                name: "Aidatlar");

            migrationBuilder.DropTable(
                name: "Daireler");
        }
    }
}
