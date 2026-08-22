-- =====================================================================
-- Actualización: Calificación bidireccional (Cliente<->Técnico) y
-- Mensajería bilateral por reserva.
-- Ejecutar manualmente en CURLINGgo_DB, o usar como referencia si en vez
-- de esto prefieres generar la migración con:
--   dotnet ef migrations add AgregarCalificacionYMensajeria
--   dotnet ef database update
-- (en cuyo caso NO ejecutes este script para las tablas/columnas, solo
-- para el seed de TiposEvaluacion y el ALTER PROCEDURE de más abajo).
-- =====================================================================
USE [CURLINGgo_DB]
GO

-- ---------------------------------------------------------------------
-- 1) Calificación del Cliente (nueva columna, simétrica a TecnicosPerfil)
-- ---------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.columns
			   WHERE object_id = OBJECT_ID('dbo.ClientesPerfil') AND name = 'CalificacionPromedio')
BEGIN
	ALTER TABLE [dbo].[ClientesPerfil]
		ADD [CalificacionPromedio] [decimal](3,2) NOT NULL CONSTRAINT DF_ClientesPerfil_CalificacionPromedio DEFAULT (0.00);
END
GO
-- IMPORTANTE: este ALTER va en un batch SEPARADO (después del GO de arriba).
-- Si el ADD COLUMN y el ADD CONSTRAINT CHECK van en el mismo batch, SQL
-- Server compila ambas sentencias ANTES de ejecutar la primera, y la
-- columna todavía no existe en el catálogo al resolver el CHECK -> error
-- "Invalid column name 'CalificacionPromedio'" (esto fue lo que pasó).
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Clientes_Calificacion')
BEGIN
	ALTER TABLE [dbo].[ClientesPerfil]
		ADD CONSTRAINT [CK_Clientes_Calificacion] CHECK ([CalificacionPromedio] >= (0) AND [CalificacionPromedio] <= (5));
END
GO

-- ---------------------------------------------------------------------
-- 2) Tabla de mensajería bilateral por reserva
-- ---------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MensajesReserva' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
	CREATE TABLE [dbo].[MensajesReserva](
		[MensajeID] [bigint] IDENTITY(1,1) NOT NULL,
		[ReservaID] [bigint] NOT NULL,
		[EmisorUsuarioID] [nvarchar](450) NOT NULL,
		[ReceptorUsuarioID] [nvarchar](450) NOT NULL,
		[Texto] [nvarchar](1000) NOT NULL,
		[FechaEnvio] [datetime2](7) NOT NULL CONSTRAINT DF_MensajesReserva_FechaEnvio DEFAULT (SYSDATETIME()),
		[Leido] [bit] NOT NULL CONSTRAINT DF_MensajesReserva_Leido DEFAULT (0),
		CONSTRAINT [PK_MensajesReserva] PRIMARY KEY CLUSTERED ([MensajeID] ASC)
	);

	ALTER TABLE [dbo].[MensajesReserva]
		ADD CONSTRAINT [FK_MensajesReserva_SolicitudesReserva] FOREIGN KEY ([ReservaID])
		REFERENCES [dbo].[SolicitudesReserva] ([ReservaID]);

	ALTER TABLE [dbo].[MensajesReserva]
		ADD CONSTRAINT [FK_MensajesReserva_Emisor] FOREIGN KEY ([EmisorUsuarioID])
		REFERENCES [dbo].[Usuarios] ([UsuarioID]);

	ALTER TABLE [dbo].[MensajesReserva]
		ADD CONSTRAINT [FK_MensajesReserva_Receptor] FOREIGN KEY ([ReceptorUsuarioID])
		REFERENCES [dbo].[Usuarios] ([UsuarioID]);

	CREATE INDEX [IX_MensajesReserva_ReservaID] ON [dbo].[MensajesReserva] ([ReservaID], [FechaEnvio]);
END
GO
-- (CREATE TABLE con FKs inline habría tenido el mismo problema de
-- resolución diferida si las FK se hubieran puesto como ALTER TABLE
-- separados en el mismo batch sin GO; aquí ya quedaron todas dentro del
-- mismo bloque CREATE TABLE + ALTER TABLE seguido de un solo GO al final,
-- lo cual SQL Server sí resuelve correctamente porque CREATE TABLE deja
-- la tabla y columnas visibles de inmediato dentro del mismo batch para
-- los ALTER TABLE que le siguen.)

-- ---------------------------------------------------------------------
-- 3) Seed de TiposEvaluacion (bidireccional)
-- ---------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM [dbo].[TiposEvaluacion] WHERE Codigo = 'CLIENTE_A_TECNICO')
	INSERT INTO [dbo].[TiposEvaluacion] (Codigo, Nombre) VALUES ('CLIENTE_A_TECNICO', 'Cliente califica al Técnico');

IF NOT EXISTS (SELECT 1 FROM [dbo].[TiposEvaluacion] WHERE Codigo = 'TECNICO_A_CLIENTE')
	INSERT INTO [dbo].[TiposEvaluacion] (Codigo, Nombre) VALUES ('TECNICO_A_CLIENTE', 'Técnico califica al Cliente');
GO

-- ---------------------------------------------------------------------
-- 4) Evitar evaluaciones duplicadas: un evaluador solo puede calificar
--    una vez por reserva y tipo de evaluación.
-- ---------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Evaluaciones_Reserva_Evaluador_Tipo')
BEGIN
	CREATE UNIQUE INDEX [UX_Evaluaciones_Reserva_Evaluador_Tipo]
		ON [dbo].[Evaluaciones] ([ReservaID], [EvaluadorUsuarioID], [TipoEvaluacionID]);
END
GO

-- ---------------------------------------------------------------------
-- 5) usp_Evaluacion_Crear — validación de reglas de negocio bidireccional
--    NOTA: este CREATE OR ALTER reemplaza el procedimiento existente.
--    Si tu versión actual tiene lógica adicional (por ejemplo, disparar
--    notificaciones), agrégala de vuelta antes de ejecutar en producción.
-- ---------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[usp_Evaluacion_Crear]
	@ReservaID BIGINT,
	@EvaluadorUsuarioID NVARCHAR(450),
	@EvaluadoUsuarioID NVARCHAR(450) = NULL,
	@ServicioID INT = NULL,
	@TipoEvaluacionID INT,
	@Puntuacion TINYINT,
	@Comentario NVARCHAR(1000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @Puntuacion NOT BETWEEN 1 AND 5
	BEGIN
		RAISERROR('La puntuación debe estar entre 1 y 5.', 16, 1);
		RETURN;
	END

	DECLARE @ClienteID NVARCHAR(450), @TecnicoID NVARCHAR(450), @EstadoCodigo VARCHAR(30);

	SELECT @ClienteID = r.ClienteID, @TecnicoID = r.TecnicoID, @EstadoCodigo = er.Codigo
	FROM dbo.SolicitudesReserva r
	JOIN dbo.EstadosReserva er ON er.EstadoReservaID = r.EstadoReservaID
	WHERE r.ReservaID = @ReservaID;

	IF @ClienteID IS NULL
	BEGIN
		RAISERROR('La reserva indicada no existe.', 16, 1);
		RETURN;
	END

	IF @EstadoCodigo <> 'COMPLETADA'
	BEGIN
		RAISERROR('Solo se puede calificar una reserva que ya fue COMPLETADA.', 16, 1);
		RETURN;
	END

	DECLARE @TipoCodigo VARCHAR(40);
	SELECT @TipoCodigo = Codigo FROM dbo.TiposEvaluacion WHERE TipoEvaluacionID = @TipoEvaluacionID;

	IF @TipoCodigo = 'CLIENTE_A_TECNICO'
	BEGIN
		IF @EvaluadorUsuarioID <> @ClienteID
		BEGIN
			RAISERROR('Solo el cliente de esta reserva puede calificar al técnico.', 16, 1);
			RETURN;
		END
		IF @EvaluadoUsuarioID IS NULL SET @EvaluadoUsuarioID = @TecnicoID;
		IF @EvaluadoUsuarioID <> @TecnicoID OR @TecnicoID IS NULL
		BEGIN
			RAISERROR('Esta reserva no tiene ese técnico asignado.', 16, 1);
			RETURN;
		END
	END
	ELSE IF @TipoCodigo = 'TECNICO_A_CLIENTE'
	BEGIN
		IF @EvaluadorUsuarioID <> @TecnicoID
		BEGIN
			RAISERROR('Solo el técnico asignado a esta reserva puede calificar al cliente.', 16, 1);
			RETURN;
		END
		IF @EvaluadoUsuarioID IS NULL SET @EvaluadoUsuarioID = @ClienteID;
		IF @EvaluadoUsuarioID <> @ClienteID
		BEGIN
			RAISERROR('Ese cliente no corresponde a esta reserva.', 16, 1);
			RETURN;
		END
	END
	ELSE
	BEGIN
		RAISERROR('Tipo de evaluación no soportado.', 16, 1);
		RETURN;
	END

	IF EXISTS (SELECT 1 FROM dbo.Evaluaciones
			   WHERE ReservaID = @ReservaID AND EvaluadorUsuarioID = @EvaluadorUsuarioID AND TipoEvaluacionID = @TipoEvaluacionID)
	BEGIN
		RAISERROR('Ya evaluaste esta reserva.', 16, 1);
		RETURN;
	END

	INSERT INTO dbo.Evaluaciones
		(ReservaID, EvaluadorUsuarioID, EvaluadoUsuarioID, ServicioID, TipoEvaluacionID, Puntuacion, Comentario, FechaEvaluacion, Activa)
	VALUES
		(@ReservaID, @EvaluadorUsuarioID, @EvaluadoUsuarioID, @ServicioID, @TipoEvaluacionID, @Puntuacion, @Comentario, SYSDATETIME(), 1);

	-- Recalcular y actualizar el promedio del evaluado
	DECLARE @Promedio DECIMAL(3,2);

	IF @TipoCodigo = 'CLIENTE_A_TECNICO'
	BEGIN
		SELECT @Promedio = ROUND(AVG(CAST(Puntuacion AS DECIMAL(4,2))), 2)
		FROM dbo.Evaluaciones
		WHERE EvaluadoUsuarioID = @EvaluadoUsuarioID AND TipoEvaluacionID = @TipoEvaluacionID AND Activa = 1;

		UPDATE dbo.TecnicosPerfil SET CalificacionPromedio = @Promedio WHERE TecnicoID = @EvaluadoUsuarioID;
	END
	ELSE
	BEGIN
		SELECT @Promedio = ROUND(AVG(CAST(Puntuacion AS DECIMAL(4,2))), 2)
		FROM dbo.Evaluaciones
		WHERE EvaluadoUsuarioID = @EvaluadoUsuarioID AND TipoEvaluacionID = @TipoEvaluacionID AND Activa = 1;

		UPDATE dbo.ClientesPerfil SET CalificacionPromedio = @Promedio WHERE ClienteID = @EvaluadoUsuarioID;
	END

	SELECT SCOPE_IDENTITY() AS EvaluacionID;
END
GO
