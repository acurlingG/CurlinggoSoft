using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlinggoSoft.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracionDespacho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionDespacho",
                columns: table => new
                {
                    ConfiguracionDespachoID = table.Column<int>(type: "int", nullable: false),
                    RadioKm = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    MaxTecnicos = table.Column<int>(type: "int", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionDespacho", x => x.ConfiguracionDespachoID);
                });

            migrationBuilder.InsertData(
                table: "ConfiguracionDespacho",
                columns: new[] { "ConfiguracionDespachoID", "FechaActualizacion", "MaxTecnicos", "RadioKm" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 20.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionDespacho");
        }
    }
}
