/*
================================================================================
 CURLINGgo - Registro de Técnico (Wizard de solicitud + aprobación)
================================================================================
 Objetivo: crear el "expediente" de un aspirante a técnico (SolicitudesTecnico)
 y sus tablas satélite, SIN tocar TecnicosPerfil / TecnicoEspecialidades, que
 siguen representando exclusivamente al técnico YA APROBADO Y OPERATIVO.

 Relación conceptual:
   Usuarios (1) ---- (0..1) SolicitudesTecnico ---- (0..1) TecnicosPerfil
   (un usuario puede tener una solicitud en curso; si es aprobada, se crea/
	actualiza su TecnicosPerfil mediante el SP de aprobación transaccional)

 Tablas creadas en este script:
   1. EstadosSolicitudTecnico   (catálogo de estados del flujo)
   2. SolicitudesTecnico        (tabla principal / expediente)
   3. SolicitudTecnicoEspecialidades (servicios + años de experiencia declarados)
   4. SolicitudTecnicoCobertura      (provincia/cantón/distrito + radio)
   5. TiposDocumentoTecnico     (catálogo de documentos requeridos)
   6. SolicitudTecnicoDocumentos      (archivos subidos por el aspirante)
   7. SolicitudTecnicoBackgroundCheck (estado de verificación de antecedentes)

 No modifica: Usuarios, TecnicosPerfil, TecnicoEspecialidades, Servicios,
 CategoriasServicio, Provincias, Cantones, Distritos.
================================================================================
*/

USE [CURLINGgo_DB]
GO

-- ============================================================================
-- 1. EstadosSolicitudTecnico
-- ============================================================================
CREATE TABLE [dbo].[EstadosSolicitudTecnico](
	[EstadoSolicitudTecnicoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](300) NULL,
	[Orden] [int] NOT NULL,
	[Activo] [bit] NOT NULL CONSTRAINT [DF_EstadosSolicitudTecnico_Activo] DEFAULT (1),
	CONSTRAINT [PK_EstadosSolicitudTecnico] PRIMARY KEY CLUSTERED ([EstadoSolicitudTecnicoID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EstadosSolicitudTecnico]
	ADD CONSTRAINT [UQ_EstadosSolicitudTecnico_Codigo] UNIQUE ([Codigo])
GO

-- Seed de estados del flujo (orden = posición esperada en el proceso)
INSERT INTO [dbo].[EstadosSolicitudTecnico] ([Codigo], [Nombre], [Descripcion], [Orden]) VALUES
('BORRADOR',               'Borrador',                  'El aspirante está completando el formulario.', 1),
('ENVIADA',                'Enviada',                    'El aspirante envió la solicitud para revisión.', 2),
('EN_REVISION',            'En revisión',                'Un administrador está revisando la solicitud.', 3),
('INFO_REQUERIDA',         'Información requerida',      'Se solicitó información adicional al aspirante.', 4),
('BACKGROUND_PENDIENTE',   'Background pendiente',       'Falta autorizar/iniciar la verificación de antecedentes.', 5),
('BACKGROUND_EN_PROCESO',  'Background en proceso',      'La verificación de antecedentes está en curso.', 6),
('APROBADA',               'Aprobada',                   'La solicitud fue aprobada; el técnico ya está activo.', 7),
('RECHAZADA',              'Rechazada',                  'La solicitud fue rechazada.', 8),
('CANCELADA',              'Cancelada',                  'El aspirante canceló su solicitud.', 9)
GO

-- ============================================================================
-- 2. SolicitudesTecnico (tabla principal / expediente del aspirante)
-- ============================================================================
CREATE TABLE [dbo].[SolicitudesTecnico](
	[SolicitudTecnicoID] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioID] [nvarchar](450) NOT NULL,
	[CodigoSolicitud] [varchar](30) NOT NULL,
	[EstadoSolicitudTecnicoID] [int] NOT NULL,

	[Identificacion] [nvarchar](30) NOT NULL,

	-- Movilidad
	[TieneLicencia] [bit] NULL,
	[TipoLicencia] [varchar](30) NULL,          -- MOTO / AUTOMOVIL / AMBAS / OTRA
	[TieneVehiculo] [bit] NULL,
	[TipoVehiculo] [varchar](30) NULL,          -- MOTOCICLETA / AUTOMOVIL / CAMIONETA / OTRO

	-- Equipo de trabajo
	[ModalidadTrabajo] [varchar](30) NULL,      -- SOLO / UN_AYUDANTE / DOS_O_MAS
	[CantidadAyudantes] [int] NULL,
	[EquipoHabitual] [bit] NULL,

	-- Seguro
	[TieneSeguro] [bit] NULL,
	[TipoSeguro] [varchar](50) NULL,            -- RIESGOS_TRABAJO / SEGURO_VOLUNTARIO / SEGURO_PRIVADO / OTRO

	-- Accesibilidad (NULL = no respondió)
	[NecesitaAccesibilidad] [bit] NULL,
	[DetalleAccesibilidad] [nvarchar](1000) NULL,

	-- Auditoría del expediente
	[FechaCreacion] [datetime2](0) NOT NULL CONSTRAINT [DF_SolicitudesTecnico_FechaCreacion] DEFAULT (SYSDATETIME()),
	[FechaUltimaActualizacion] [datetime2](0) NULL,
	[FechaEnvio] [datetime2](0) NULL,
	[FechaRevision] [datetime2](0) NULL,
	[RevisadoPor] [nvarchar](450) NULL,
	[FechaDecision] [datetime2](0) NULL,
	[MotivoRechazo] [nvarchar](1000) NULL,
	[ObservacionesAdmin] [nvarchar](2000) NULL,

	CONSTRAINT [PK_SolicitudesTecnico] PRIMARY KEY CLUSTERED ([SolicitudTecnicoID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudesTecnico]
	ADD CONSTRAINT [UQ_SolicitudesTecnico_CodigoSolicitud] UNIQUE ([CodigoSolicitud])
GO

-- Un usuario solo puede tener UNA solicitud "viva" (no rechazada/cancelada) a la vez.
-- Se controla en la lógica de aplicación; aquí solo se índica por rendimiento.
CREATE NONCLUSTERED INDEX [IX_SolicitudesTecnico_UsuarioID] ON [dbo].[SolicitudesTecnico]([UsuarioID] ASC)
GO
CREATE NONCLUSTERED INDEX [IX_SolicitudesTecnico_EstadoSolicitudTecnicoID] ON [dbo].[SolicitudesTecnico]([EstadoSolicitudTecnicoID] ASC)
GO

ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [FK_SolicitudesTecnico_Usuarios]
	FOREIGN KEY([UsuarioID]) REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[SolicitudesTecnico] CHECK CONSTRAINT [FK_SolicitudesTecnico_Usuarios]
GO

ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [FK_SolicitudesTecnico_EstadosSolicitudTecnico]
	FOREIGN KEY([EstadoSolicitudTecnicoID]) REFERENCES [dbo].[EstadosSolicitudTecnico] ([EstadoSolicitudTecnicoID])
GO
ALTER TABLE [dbo].[SolicitudesTecnico] CHECK CONSTRAINT [FK_SolicitudesTecnico_EstadosSolicitudTecnico]
GO

ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [CK_SolicitudesTecnico_TipoLicencia]
	CHECK ([TipoLicencia] IS NULL OR [TipoLicencia] IN ('MOTO','AUTOMOVIL','AMBAS','OTRA'))
GO
ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [CK_SolicitudesTecnico_TipoVehiculo]
	CHECK ([TipoVehiculo] IS NULL OR [TipoVehiculo] IN ('MOTOCICLETA','AUTOMOVIL','CAMIONETA','OTRO'))
GO
ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [CK_SolicitudesTecnico_ModalidadTrabajo]
	CHECK ([ModalidadTrabajo] IS NULL OR [ModalidadTrabajo] IN ('SOLO','UN_AYUDANTE','DOS_O_MAS'))
GO
ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [CK_SolicitudesTecnico_TipoSeguro]
	CHECK ([TipoSeguro] IS NULL OR [TipoSeguro] IN ('RIESGOS_TRABAJO','SEGURO_VOLUNTARIO','SEGURO_PRIVADO','OTRO'))
GO
ALTER TABLE [dbo].[SolicitudesTecnico] WITH CHECK ADD CONSTRAINT [CK_SolicitudesTecnico_CantidadAyudantes]
	CHECK ([CantidadAyudantes] IS NULL OR [CantidadAyudantes] BETWEEN 1 AND 50)
GO

-- ============================================================================
-- 3. SolicitudTecnicoEspecialidades
-- ============================================================================
CREATE TABLE [dbo].[SolicitudTecnicoEspecialidades](
	[SolicitudTecnicoEspecialidadID] [bigint] IDENTITY(1,1) NOT NULL,
	[SolicitudTecnicoID] [bigint] NOT NULL,
	[ServicioID] [int] NOT NULL,
	[AniosExperiencia] [int] NOT NULL CONSTRAINT [DF_SolicitudTecnicoEspecialidades_Anios] DEFAULT (0),
	[DescripcionExperiencia] [nvarchar](1000) NULL,
	CONSTRAINT [PK_SolicitudTecnicoEspecialidades] PRIMARY KEY CLUSTERED ([SolicitudTecnicoEspecialidadID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades]
	ADD CONSTRAINT [UQ_SolicitudTecnicoEspecialidad_Solicitud_Servicio] UNIQUE ([SolicitudTecnicoID],[ServicioID])
GO

ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoEspecialidades_SolicitudesTecnico]
	FOREIGN KEY([SolicitudTecnicoID]) REFERENCES [dbo].[SolicitudesTecnico] ([SolicitudTecnicoID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades] CHECK CONSTRAINT [FK_SolicitudTecnicoEspecialidades_SolicitudesTecnico]
GO

ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoEspecialidades_Servicios]
	FOREIGN KEY([ServicioID]) REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades] CHECK CONSTRAINT [FK_SolicitudTecnicoEspecialidades_Servicios]
GO

ALTER TABLE [dbo].[SolicitudTecnicoEspecialidades] WITH CHECK ADD CONSTRAINT [CK_SolicitudTecnicoEspecialidades_Anios]
	CHECK ([AniosExperiencia] BETWEEN 0 AND 60)
GO

-- ============================================================================
-- 4. SolicitudTecnicoCobertura
-- ============================================================================
CREATE TABLE [dbo].[SolicitudTecnicoCobertura](
	[SolicitudTecnicoCoberturaID] [bigint] IDENTITY(1,1) NOT NULL,
	[SolicitudTecnicoID] [bigint] NOT NULL,
	[ProvinciaID] [int] NOT NULL,
	[CantonID] [int] NOT NULL,
	[DistritoID] [int] NULL,
	[RadioCoberturaKm] [decimal](5,2) NULL,
	CONSTRAINT [PK_SolicitudTecnicoCobertura] PRIMARY KEY CLUSTERED ([SolicitudTecnicoCoberturaID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudTecnicoCobertura] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoCobertura_SolicitudesTecnico]
	FOREIGN KEY([SolicitudTecnicoID]) REFERENCES [dbo].[SolicitudesTecnico] ([SolicitudTecnicoID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SolicitudTecnicoCobertura] CHECK CONSTRAINT [FK_SolicitudTecnicoCobertura_SolicitudesTecnico]
GO

ALTER TABLE [dbo].[SolicitudTecnicoCobertura] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoCobertura_Provincias]
	FOREIGN KEY([ProvinciaID]) REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[SolicitudTecnicoCobertura] CHECK CONSTRAINT [FK_SolicitudTecnicoCobertura_Provincias]
GO

ALTER TABLE [dbo].[SolicitudTecnicoCobertura] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoCobertura_Cantones]
	FOREIGN KEY([CantonID]) REFERENCES [dbo].[Cantones] ([CantonID])
GO
ALTER TABLE [dbo].[SolicitudTecnicoCobertura] CHECK CONSTRAINT [FK_SolicitudTecnicoCobertura_Cantones]
GO

ALTER TABLE [dbo].[SolicitudTecnicoCobertura] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoCobertura_Distritos]
	FOREIGN KEY([DistritoID]) REFERENCES [dbo].[Distritos] ([DistritoID])
GO
ALTER TABLE [dbo].[SolicitudTecnicoCobertura] CHECK CONSTRAINT [FK_SolicitudTecnicoCobertura_Distritos]
GO

CREATE NONCLUSTERED INDEX [IX_SolicitudTecnicoCobertura_SolicitudTecnicoID] ON [dbo].[SolicitudTecnicoCobertura]([SolicitudTecnicoID] ASC)
GO

-- ============================================================================
-- 5. TiposDocumentoTecnico (catálogo)
-- ============================================================================
CREATE TABLE [dbo].[TiposDocumentoTecnico](
	[TipoDocumentoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](300) NULL,
	[Obligatorio] [bit] NOT NULL CONSTRAINT [DF_TiposDocumentoTecnico_Obligatorio] DEFAULT (0),
	[Activo] [bit] NOT NULL CONSTRAINT [DF_TiposDocumentoTecnico_Activo] DEFAULT (1),
	CONSTRAINT [PK_TiposDocumentoTecnico] PRIMARY KEY CLUSTERED ([TipoDocumentoID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TiposDocumentoTecnico]
	ADD CONSTRAINT [UQ_TiposDocumentoTecnico_Codigo] UNIQUE ([Codigo])
GO

INSERT INTO [dbo].[TiposDocumentoTecnico] ([Codigo],[Nombre],[Descripcion],[Obligatorio]) VALUES
('IDENTIFICACION', 'Identificación',        'Cédula o documento de identidad vigente.', 1),
('LICENCIA',        'Licencia de conducir',  'Requerida si el aspirante indicó que tiene licencia.', 0),
('CERTIFICACION',   'Certificación técnica', 'Certificación relacionada con la(s) especialidad(es).', 0),
('SEGURO',          'Comprobante de seguro', 'Requerido si el aspirante indicó que cuenta con seguro.', 0),
('OTRO',            'Otro documento',        'Cualquier otro respaldo adicional.', 0)
GO

-- ============================================================================
-- 6. SolicitudTecnicoDocumentos
-- ============================================================================
CREATE TABLE [dbo].[SolicitudTecnicoDocumentos](
	[SolicitudTecnicoDocumentoID] [bigint] IDENTITY(1,1) NOT NULL,
	[SolicitudTecnicoID] [bigint] NOT NULL,
	[TipoDocumentoID] [int] NOT NULL,
	[NombreArchivo] [nvarchar](255) NOT NULL,
	[RutaArchivo] [nvarchar](500) NOT NULL,
	[FechaCarga] [datetime2](0) NOT NULL CONSTRAINT [DF_SolicitudTecnicoDocumentos_FechaCarga] DEFAULT (SYSDATETIME()),
	[EstadoDocumento] [varchar](30) NOT NULL CONSTRAINT [DF_SolicitudTecnicoDocumentos_Estado] DEFAULT ('PENDIENTE'),
	[RevisadoPor] [nvarchar](450) NULL,
	[FechaRevision] [datetime2](0) NULL,
	[Observaciones] [nvarchar](1000) NULL,
	CONSTRAINT [PK_SolicitudTecnicoDocumentos] PRIMARY KEY CLUSTERED ([SolicitudTecnicoDocumentoID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudTecnicoDocumentos] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoDocumentos_SolicitudesTecnico]
	FOREIGN KEY([SolicitudTecnicoID]) REFERENCES [dbo].[SolicitudesTecnico] ([SolicitudTecnicoID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SolicitudTecnicoDocumentos] CHECK CONSTRAINT [FK_SolicitudTecnicoDocumentos_SolicitudesTecnico]
GO

ALTER TABLE [dbo].[SolicitudTecnicoDocumentos] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoDocumentos_TiposDocumentoTecnico]
	FOREIGN KEY([TipoDocumentoID]) REFERENCES [dbo].[TiposDocumentoTecnico] ([TipoDocumentoID])
GO
ALTER TABLE [dbo].[SolicitudTecnicoDocumentos] CHECK CONSTRAINT [FK_SolicitudTecnicoDocumentos_TiposDocumentoTecnico]
GO

ALTER TABLE [dbo].[SolicitudTecnicoDocumentos] WITH CHECK ADD CONSTRAINT [CK_SolicitudTecnicoDocumentos_Estado]
	CHECK ([EstadoDocumento] IN ('PENDIENTE','APROBADO','RECHAZADO'))
GO

CREATE NONCLUSTERED INDEX [IX_SolicitudTecnicoDocumentos_SolicitudTecnicoID] ON [dbo].[SolicitudTecnicoDocumentos]([SolicitudTecnicoID] ASC)
GO

-- ============================================================================
-- 7. SolicitudTecnicoBackgroundCheck
-- ============================================================================
CREATE TABLE [dbo].[SolicitudTecnicoBackgroundCheck](
	[BackgroundCheckID] [bigint] IDENTITY(1,1) NOT NULL,
	[SolicitudTecnicoID] [bigint] NOT NULL,
	[Estado] [varchar](30) NOT NULL CONSTRAINT [DF_SolicitudTecnicoBackgroundCheck_Estado] DEFAULT ('PENDIENTE'),
	[FechaAutorizacion] [datetime2](0) NULL,
	[FechaInicio] [datetime2](0) NULL,
	[FechaFinalizacion] [datetime2](0) NULL,
	[Resultado] [varchar](30) NULL,
	[RevisadoPor] [nvarchar](450) NULL,
	[FechaRevision] [datetime2](0) NULL,
	[Observaciones] [nvarchar](2000) NULL,
	CONSTRAINT [PK_SolicitudTecnicoBackgroundCheck] PRIMARY KEY CLUSTERED ([BackgroundCheckID] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolicitudTecnicoBackgroundCheck]
	ADD CONSTRAINT [UQ_SolicitudTecnicoBackgroundCheck_Solicitud] UNIQUE ([SolicitudTecnicoID])
GO

ALTER TABLE [dbo].[SolicitudTecnicoBackgroundCheck] WITH CHECK ADD CONSTRAINT [FK_SolicitudTecnicoBackgroundCheck_SolicitudesTecnico]
	FOREIGN KEY([SolicitudTecnicoID]) REFERENCES [dbo].[SolicitudesTecnico] ([SolicitudTecnicoID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SolicitudTecnicoBackgroundCheck] CHECK CONSTRAINT [FK_SolicitudTecnicoBackgroundCheck_SolicitudesTecnico]
GO

ALTER TABLE [dbo].[SolicitudTecnicoBackgroundCheck] WITH CHECK ADD CONSTRAINT [CK_SolicitudTecnicoBackgroundCheck_Estado]
	CHECK ([Estado] IN ('PENDIENTE','AUTORIZADO','EN_PROCESO','COMPLETADO','APROBADO','RECHAZADO','REQUIERE_REVISION'))
GO
ALTER TABLE [dbo].[SolicitudTecnicoBackgroundCheck] WITH CHECK ADD CONSTRAINT [CK_SolicitudTecnicoBackgroundCheck_Resultado]
	CHECK ([Resultado] IS NULL OR [Resultado] IN ('APROBADO','RECHAZADO','REQUIERE_REVISION'))
GO

-- ============================================================================
-- 8. SP de aprobación transaccional: usp_SolicitudTecnico_Aprobar
-- ============================================================================
-- Convierte una SolicitudTecnico en un TecnicoPerfil operativo + sus
-- TecnicoEspecialidades, todo dentro de una transacción. Si algo falla, se
-- hace ROLLBACK y la solicitud queda intacta.
CREATE OR ALTER PROCEDURE [dbo].[usp_SolicitudTecnico_Aprobar]
	@SolicitudTecnicoID BIGINT,
	@RevisadoPor NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @UsuarioID NVARCHAR(450);
	DECLARE @EstadoAprobadaID INT;
	DECLARE @ProvinciaPrincipal INT;
	DECLARE @CantonPrincipal INT;

	SELECT @UsuarioID = UsuarioID FROM dbo.SolicitudesTecnico WHERE SolicitudTecnicoID = @SolicitudTecnicoID;
	IF @UsuarioID IS NULL
	BEGIN
		RAISERROR('La solicitud de técnico especificada no existe.', 16, 1);
		RETURN;
	END

	SELECT @EstadoAprobadaID = EstadoSolicitudTecnicoID FROM dbo.EstadosSolicitudTecnico WHERE Codigo = 'APROBADA';

	-- Toma la primera cobertura registrada como provincia/cantón principal
	-- de TecnicosPerfil (que solo admite una cobertura "principal").
	SELECT TOP (1) @ProvinciaPrincipal = ProvinciaID, @CantonPrincipal = CantonID
	FROM dbo.SolicitudTecnicoCobertura
	WHERE SolicitudTecnicoID = @SolicitudTecnicoID
	ORDER BY SolicitudTecnicoCoberturaID;

	BEGIN TRANSACTION;

	BEGIN TRY
		-- 1. Crear o actualizar TecnicosPerfil
		IF EXISTS (SELECT 1 FROM dbo.TecnicosPerfil WHERE TecnicoID = @UsuarioID)
		BEGIN
			UPDATE dbo.TecnicosPerfil
			SET IdentificacionCedula = st.Identificacion,
				EstadoVerificacion = 'Aprobado',
				ProvinciaCoberturaID = @ProvinciaPrincipal,
				CantonCoberturaID = @CantonPrincipal,
				FechaVerificacion = SYSDATETIME()
			FROM dbo.TecnicosPerfil tp
			INNER JOIN dbo.SolicitudesTecnico st ON st.SolicitudTecnicoID = @SolicitudTecnicoID
			WHERE tp.TecnicoID = @UsuarioID;
		END
		ELSE
		BEGIN
			INSERT INTO dbo.TecnicosPerfil (TecnicoID, IdentificacionCedula, EstadoVerificacion, CalificacionPromedio, Disponible, ProvinciaCoberturaID, CantonCoberturaID, FechaVerificacion)
			SELECT @UsuarioID, st.Identificacion, 'Aprobado', 0.00, 1, @ProvinciaPrincipal, @CantonPrincipal, SYSDATETIME()
			FROM dbo.SolicitudesTecnico st
			WHERE st.SolicitudTecnicoID = @SolicitudTecnicoID;
		END

		-- 2. Copiar especialidades declaradas hacia TecnicoEspecialidades
		MERGE dbo.TecnicoEspecialidades AS destino
		USING (
			SELECT ServicioID, AniosExperiencia
			FROM dbo.SolicitudTecnicoEspecialidades
			WHERE SolicitudTecnicoID = @SolicitudTecnicoID
		) AS origen
		ON destino.TecnicoID = @UsuarioID AND destino.ServicioID = origen.ServicioID
		WHEN MATCHED THEN
			UPDATE SET AniosExperiencia = origen.AniosExperiencia
		WHEN NOT MATCHED THEN
			INSERT (TecnicoID, ServicioID, AniosExperiencia)
			VALUES (@UsuarioID, origen.ServicioID, origen.AniosExperiencia);

		-- 3. Asignar rol Técnico en Identity (si no lo tiene ya)
		INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
		SELECT @UsuarioID, r.Id
		FROM dbo.AspNetRoles r
		WHERE r.Name = 'Tecnico'
		  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUserRoles ur WHERE ur.UserId = @UsuarioID AND ur.RoleId = r.Id);

		-- 4. Activar usuario
		UPDATE dbo.Usuarios SET EstadoUsuario = 'Activo' WHERE UsuarioID = @UsuarioID;

		-- 5. Marcar la solicitud como aprobada
		UPDATE dbo.SolicitudesTecnico
		SET EstadoSolicitudTecnicoID = @EstadoAprobadaID,
			RevisadoPor = @RevisadoPor,
			FechaRevision = SYSDATETIME(),
			FechaDecision = SYSDATETIME()
		WHERE SolicitudTecnicoID = @SolicitudTecnicoID;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO
