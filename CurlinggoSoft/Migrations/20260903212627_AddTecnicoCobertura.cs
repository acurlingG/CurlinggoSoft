using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurlinggoSoft.Migrations
{
    /// <inheritdoc />
    public partial class AddTecnicoCobertura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CalificacionPromedio",
                table: "ClientesPerfil",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "EstadosSolicitudTecnico",
                columns: table => new
                {
                    EstadoSolicitudTecnicoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosSolicitudTecnico", x => x.EstadoSolicitudTecnicoID);
                });

            migrationBuilder.CreateTable(
                name: "MensajesReserva",
                columns: table => new
                {
                    MensajeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaID = table.Column<long>(type: "bigint", nullable: false),
                    EmisorUsuarioID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReceptorUsuarioID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajesReserva", x => x.MensajeID);
                    table.ForeignKey(
                        name: "FK_MensajesReserva_SolicitudesReserva_ReservaID",
                        column: x => x.ReservaID,
                        principalTable: "SolicitudesReserva",
                        principalColumn: "ReservaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MensajesReserva_Usuarios_EmisorUsuarioID",
                        column: x => x.EmisorUsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MensajesReserva_Usuarios_ReceptorUsuarioID",
                        column: x => x.ReceptorUsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TecnicoCobertura",
                columns: table => new
                {
                    TecnicoCoberturaID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TecnicoID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProvinciaID = table.Column<int>(type: "int", nullable: false),
                    CantonID = table.Column<int>(type: "int", nullable: false),
                    DistritoID = table.Column<int>(type: "int", nullable: true),
                    RadioCoberturaKm = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TecnicoCobertura", x => x.TecnicoCoberturaID);
                    table.ForeignKey(
                        name: "FK_TecnicoCobertura_Cantones_CantonID",
                        column: x => x.CantonID,
                        principalTable: "Cantones",
                        principalColumn: "CantonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TecnicoCobertura_Distritos_DistritoID",
                        column: x => x.DistritoID,
                        principalTable: "Distritos",
                        principalColumn: "DistritoID");
                    table.ForeignKey(
                        name: "FK_TecnicoCobertura_Provincias_ProvinciaID",
                        column: x => x.ProvinciaID,
                        principalTable: "Provincias",
                        principalColumn: "ProvinciaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TecnicoCobertura_TecnicosPerfil_TecnicoID",
                        column: x => x.TecnicoID,
                        principalTable: "TecnicosPerfil",
                        principalColumn: "TecnicoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiposDocumentoTecnico",
                columns: table => new
                {
                    TipoDocumentoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Obligatorio = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDocumentoTecnico", x => x.TipoDocumentoID);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesTecnico",
                columns: table => new
                {
                    SolicitudTecnicoID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CodigoSolicitud = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EstadoSolicitudTecnicoID = table.Column<int>(type: "int", nullable: false),
                    TipoSolicitud = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TieneLicencia = table.Column<bool>(type: "bit", nullable: true),
                    TipoLicencia = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TieneVehiculo = table.Column<bool>(type: "bit", nullable: true),
                    TipoVehiculo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModalidadTrabajo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CantidadAyudantes = table.Column<int>(type: "int", nullable: true),
                    EquipoHabitual = table.Column<bool>(type: "bit", nullable: true),
                    TieneSeguro = table.Column<bool>(type: "bit", nullable: true),
                    TipoSeguro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NecesitaAccesibilidad = table.Column<bool>(type: "bit", nullable: true),
                    DetalleAccesibilidad = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaRevision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisadoPor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaDecision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoRechazo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ObservacionesAdmin = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesTecnico", x => x.SolicitudTecnicoID);
                    table.ForeignKey(
                        name: "FK_SolicitudesTecnico_EstadosSolicitudTecnico_EstadoSolicitudTecnicoID",
                        column: x => x.EstadoSolicitudTecnicoID,
                        principalTable: "EstadosSolicitudTecnico",
                        principalColumn: "EstadoSolicitudTecnicoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesTecnico_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudTecnicoBackgroundCheck",
                columns: table => new
                {
                    BackgroundCheckID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudTecnicoID = table.Column<long>(type: "bigint", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaAutorizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Resultado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RevisadoPor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaRevision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudTecnicoBackgroundCheck", x => x.BackgroundCheckID);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoBackgroundCheck_SolicitudesTecnico_SolicitudTecnicoID",
                        column: x => x.SolicitudTecnicoID,
                        principalTable: "SolicitudesTecnico",
                        principalColumn: "SolicitudTecnicoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudTecnicoCobertura",
                columns: table => new
                {
                    SolicitudTecnicoCoberturaID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudTecnicoID = table.Column<long>(type: "bigint", nullable: false),
                    ProvinciaID = table.Column<int>(type: "int", nullable: false),
                    CantonID = table.Column<int>(type: "int", nullable: false),
                    DistritoID = table.Column<int>(type: "int", nullable: true),
                    RadioCoberturaKm = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudTecnicoCobertura", x => x.SolicitudTecnicoCoberturaID);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoCobertura_Cantones_CantonID",
                        column: x => x.CantonID,
                        principalTable: "Cantones",
                        principalColumn: "CantonID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoCobertura_Distritos_DistritoID",
                        column: x => x.DistritoID,
                        principalTable: "Distritos",
                        principalColumn: "DistritoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoCobertura_Provincias_ProvinciaID",
                        column: x => x.ProvinciaID,
                        principalTable: "Provincias",
                        principalColumn: "ProvinciaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoCobertura_SolicitudesTecnico_SolicitudTecnicoID",
                        column: x => x.SolicitudTecnicoID,
                        principalTable: "SolicitudesTecnico",
                        principalColumn: "SolicitudTecnicoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudTecnicoDocumentos",
                columns: table => new
                {
                    SolicitudTecnicoDocumentoID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudTecnicoID = table.Column<long>(type: "bigint", nullable: false),
                    TipoDocumentoID = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoDocumento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RevisadoPor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaRevision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudTecnicoDocumentos", x => x.SolicitudTecnicoDocumentoID);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoDocumentos_SolicitudesTecnico_SolicitudTecnicoID",
                        column: x => x.SolicitudTecnicoID,
                        principalTable: "SolicitudesTecnico",
                        principalColumn: "SolicitudTecnicoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoDocumentos_TiposDocumentoTecnico_TipoDocumentoID",
                        column: x => x.TipoDocumentoID,
                        principalTable: "TiposDocumentoTecnico",
                        principalColumn: "TipoDocumentoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudTecnicoEspecialidades",
                columns: table => new
                {
                    SolicitudTecnicoEspecialidadID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudTecnicoID = table.Column<long>(type: "bigint", nullable: false),
                    ServicioID = table.Column<int>(type: "int", nullable: false),
                    AniosExperiencia = table.Column<int>(type: "int", nullable: false),
                    DescripcionExperiencia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudTecnicoEspecialidades", x => x.SolicitudTecnicoEspecialidadID);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoEspecialidades_Servicios_ServicioID",
                        column: x => x.ServicioID,
                        principalTable: "Servicios",
                        principalColumn: "ServicioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudTecnicoEspecialidades_SolicitudesTecnico_SolicitudTecnicoID",
                        column: x => x.SolicitudTecnicoID,
                        principalTable: "SolicitudesTecnico",
                        principalColumn: "SolicitudTecnicoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadosSolicitudTecnico_Codigo",
                table: "EstadosSolicitudTecnico",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MensajesReserva_EmisorUsuarioID",
                table: "MensajesReserva",
                column: "EmisorUsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_MensajesReserva_ReceptorUsuarioID",
                table: "MensajesReserva",
                column: "ReceptorUsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_MensajesReserva_ReservaID",
                table: "MensajesReserva",
                column: "ReservaID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTecnico_CodigoSolicitud",
                table: "SolicitudesTecnico",
                column: "CodigoSolicitud",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTecnico_EstadoSolicitudTecnicoID",
                table: "SolicitudesTecnico",
                column: "EstadoSolicitudTecnicoID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTecnico_UsuarioID",
                table: "SolicitudesTecnico",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoBackgroundCheck_SolicitudTecnicoID",
                table: "SolicitudTecnicoBackgroundCheck",
                column: "SolicitudTecnicoID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoCobertura_CantonID",
                table: "SolicitudTecnicoCobertura",
                column: "CantonID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoCobertura_DistritoID",
                table: "SolicitudTecnicoCobertura",
                column: "DistritoID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoCobertura_ProvinciaID",
                table: "SolicitudTecnicoCobertura",
                column: "ProvinciaID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoCobertura_SolicitudTecnicoID",
                table: "SolicitudTecnicoCobertura",
                column: "SolicitudTecnicoID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoDocumentos_SolicitudTecnicoID",
                table: "SolicitudTecnicoDocumentos",
                column: "SolicitudTecnicoID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoDocumentos_TipoDocumentoID",
                table: "SolicitudTecnicoDocumentos",
                column: "TipoDocumentoID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoEspecialidades_ServicioID",
                table: "SolicitudTecnicoEspecialidades",
                column: "ServicioID");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTecnicoEspecialidades_SolicitudTecnicoID",
                table: "SolicitudTecnicoEspecialidades",
                column: "SolicitudTecnicoID");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicoCobertura_CantonID",
                table: "TecnicoCobertura",
                column: "CantonID");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicoCobertura_DistritoID",
                table: "TecnicoCobertura",
                column: "DistritoID");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicoCobertura_ProvinciaID",
                table: "TecnicoCobertura",
                column: "ProvinciaID");

            migrationBuilder.CreateIndex(
                name: "IX_TecnicoCobertura_TecnicoID",
                table: "TecnicoCobertura",
                column: "TecnicoID");

            migrationBuilder.CreateIndex(
                name: "IX_TiposDocumentoTecnico_Codigo",
                table: "TiposDocumentoTecnico",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensajesReserva");

            migrationBuilder.DropTable(
                name: "SolicitudTecnicoBackgroundCheck");

            migrationBuilder.DropTable(
                name: "SolicitudTecnicoCobertura");

            migrationBuilder.DropTable(
                name: "SolicitudTecnicoDocumentos");

            migrationBuilder.DropTable(
                name: "SolicitudTecnicoEspecialidades");

            migrationBuilder.DropTable(
                name: "TecnicoCobertura");

            migrationBuilder.DropTable(
                name: "TiposDocumentoTecnico");

            migrationBuilder.DropTable(
                name: "SolicitudesTecnico");

            migrationBuilder.DropTable(
                name: "EstadosSolicitudTecnico");

            migrationBuilder.DropColumn(
                name: "CalificacionPromedio",
                table: "ClientesPerfil");
        }
    }
}
