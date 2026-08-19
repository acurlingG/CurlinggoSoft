USE [CURLINGgo_DB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Auditoria]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auditoria](
	[AuditoriaID] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioID] [nvarchar](450) NULL,
	[TablaAfectada] [varchar](128) NOT NULL,
	[RegistroID] [nvarchar](100) NULL,
	[Operacion] [varchar](20) NOT NULL,
	[ValoresAnterioresJson] [nvarchar](max) NULL,
	[ValoresNuevosJson] [nvarchar](max) NULL,
	[FechaEvento] [datetime2](0) NOT NULL,
	[DireccionIP] [varchar](45) NULL,
	[CorrelationID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED 
(
	[AuditoriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cantones]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cantones](
	[CantonID] [int] NOT NULL,
	[ProvinciaID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[CodigoDTA] [char](3) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Cantones] PRIMARY KEY CLUSTERED 
(
	[CantonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Cantones_CodigoDTA] UNIQUE NONCLUSTERED 
(
	[CodigoDTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Cantones_ID_Provincia] UNIQUE NONCLUSTERED 
(
	[CantonID] ASC,
	[ProvinciaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Cantones_Provincia_Nombre] UNIQUE NONCLUSTERED 
(
	[ProvinciaID] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoriasServicio]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriasServicio](
	[CategoriaID] [int] IDENTITY(1,1) NOT NULL,
	[NombreCategoria] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_CategoriasServicio] PRIMARY KEY CLUSTERED 
(
	[CategoriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_CategoriasServicio_Nombre] UNIQUE NONCLUSTERED 
(
	[NombreCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientesPerfil]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientesPerfil](
	[ClienteID] [nvarchar](450) NOT NULL,
	[ProvinciaID] [int] NOT NULL,
	[CantonID] [int] NOT NULL,
	[DistritoID] [int] NOT NULL,
	[DireccionExacta] [nvarchar](300) NOT NULL,
	[Latitud] [decimal](9, 6) NULL,
	[Longitud] [decimal](9, 6) NULL,
	[UbicacionGeo]  AS (case when [Latitud] IS NOT NULL AND [Longitud] IS NOT NULL then [geography]::Point([Latitud],[Longitud],(4326))  end) PERSISTED,
	[FechaActualizacion] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_ClientesPerfil] PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetallePrecioReserva]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallePrecioReserva](
	[DetallePrecioID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[Concepto] [nvarchar](300) NOT NULL,
	[TipoConcepto] [varchar](30) NOT NULL,
	[Monto] [decimal](12, 2) NOT NULL,
	[PreguntaServicioID] [int] NULL,
	[OpcionPreguntaID] [int] NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_DetallePrecioReserva] PRIMARY KEY CLUSTERED 
(
	[DetallePrecioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DireccionesCliente]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DireccionesCliente](
	[DireccionID] [bigint] IDENTITY(1,1) NOT NULL,
	[ClienteID] [nvarchar](450) NOT NULL,
	[NombreDireccion] [nvarchar](80) NOT NULL,
	[ProvinciaID] [int] NOT NULL,
	[CantonID] [int] NOT NULL,
	[DistritoID] [int] NOT NULL,
	[DireccionExacta] [nvarchar](300) NOT NULL,
	[Latitud] [decimal](9, 6) NULL,
	[Longitud] [decimal](9, 6) NULL,
	[UbicacionGeo]  AS (case when [Latitud] IS NOT NULL AND [Longitud] IS NOT NULL then [geography]::Point([Latitud],[Longitud],(4326))  end) PERSISTED,
	[EsPrincipal] [bit] NOT NULL,
	[Activa] [bit] NOT NULL,
	[FechaCreacion] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_DireccionesCliente] PRIMARY KEY CLUSTERED 
(
	[DireccionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Direcciones_Cliente_Nombre] UNIQUE NONCLUSTERED 
(
	[ClienteID] ASC,
	[NombreDireccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DisponibilidadTecnico]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisponibilidadTecnico](
	[DisponibilidadID] [bigint] IDENTITY(1,1) NOT NULL,
	[TecnicoID] [nvarchar](450) NOT NULL,
	[DiaSemana] [tinyint] NOT NULL,
	[HoraInicio] [time](0) NOT NULL,
	[HoraFin] [time](0) NOT NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_DisponibilidadTecnico] PRIMARY KEY CLUSTERED 
(
	[DisponibilidadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Disponibilidad_Tecnico_Dia_Hora] UNIQUE NONCLUSTERED 
(
	[TecnicoID] ASC,
	[DiaSemana] ASC,
	[HoraInicio] ASC,
	[HoraFin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Distritos]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distritos](
	[DistritoID] [int] NOT NULL,
	[CantonID] [int] NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[CodigoDTA] [char](5) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Distritos] PRIMARY KEY CLUSTERED 
(
	[DistritoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Distritos_Canton_Nombre] UNIQUE NONCLUSTERED 
(
	[CantonID] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Distritos_CodigoDTA] UNIQUE NONCLUSTERED 
(
	[CodigoDTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Distritos_ID_Canton] UNIQUE NONCLUSTERED 
(
	[DistritoID] ASC,
	[CantonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadosOfertaTecnico]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadosOfertaTecnico](
	[EstadoOfertaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EstadosOfertaTecnico] PRIMARY KEY CLUSTERED 
(
	[EstadoOfertaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EstadosOferta_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadosPago]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadosPago](
	[EstadoPagoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EstadosPago] PRIMARY KEY CLUSTERED 
(
	[EstadoPagoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EstadosPago_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadosReserva]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadosReserva](
	[EstadoReservaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[OrdenFlujo] [int] NOT NULL,
 CONSTRAINT [PK_EstadosReserva] PRIMARY KEY CLUSTERED 
(
	[EstadoReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EstadosReserva_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EstadosReserva_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Evaluaciones]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evaluaciones](
	[EvaluacionID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[EvaluadorUsuarioID] [nvarchar](450) NOT NULL,
	[EvaluadoUsuarioID] [nvarchar](450) NULL,
	[ServicioID] [int] NULL,
	[TipoEvaluacionID] [int] NOT NULL,
	[Puntuacion] [tinyint] NOT NULL,
	[Comentario] [nvarchar](1000) NULL,
	[FechaEvaluacion] [datetime2](0) NOT NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_Evaluaciones] PRIMARY KEY CLUSTERED 
(
	[EvaluacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialEstadosReserva]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialEstadosReserva](
	[HistorialID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[EstadoAnteriorID] [int] NULL,
	[EstadoNuevoID] [int] NOT NULL,
	[FechaCambio] [datetime2](0) NOT NULL,
	[UsuarioModificadorID] [nvarchar](450) NULL,
	[Observaciones] [nvarchar](500) NULL,
 CONSTRAINT [PK_HistorialEstadosReserva] PRIMARY KEY CLUSTERED 
(
	[HistorialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntentosPago]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntentosPago](
	[IntentoPagoID] [bigint] IDENTITY(1,1) NOT NULL,
	[PagoID] [bigint] NOT NULL,
	[MetodoPagoID] [int] NOT NULL,
	[EstadoPagoID] [int] NOT NULL,
	[MontoIntento] [decimal](12, 2) NOT NULL,
	[ReferenciaComprobante] [nvarchar](150) NULL,
	[ReferenciaProveedor] [nvarchar](200) NULL,
	[FechaIntento] [datetime2](0) NOT NULL,
	[MensajeProveedor] [nvarchar](500) NULL,
 CONSTRAINT [PK_IntentosPago] PRIMARY KEY CLUSTERED 
(
	[IntentoPagoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogsIntervencionOperativa]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogsIntervencionOperativa](
	[LogID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NULL,
	[TipoEvento] [varchar](100) NOT NULL,
	[DatosEntradaJson] [nvarchar](max) NULL,
	[DecisionTomada] [nvarchar](500) NOT NULL,
	[ModeloVersion] [varchar](100) NULL,
	[UsuarioIntervencionID] [nvarchar](450) NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_LogsIntervencionOperativa] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuPermisos]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuPermisos](
	[MenuID] [bigint] NOT NULL,
	[PermisoID] [int] NOT NULL,
 CONSTRAINT [PK_MenuPermisos] PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC,
	[PermisoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[MenuID] [bigint] IDENTITY(1,1) NOT NULL,
	[MenuPadreID] [bigint] NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](300) NULL,
	[Icono] [nvarchar](100) NULL,
	[Orden] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetodosPago]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetodosPago](
	[MetodoPagoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_MetodosPago] PRIMARY KEY CLUSTERED 
(
	[MetodoPagoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_MetodosPago_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_MetodosPago_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificaciones]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificaciones](
	[NotificacionID] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioID] [nvarchar](450) NOT NULL,
	[ReservaID] [bigint] NULL,
	[OfertaTecnicoID] [bigint] NULL,
	[TipoNotificacion] [varchar](40) NOT NULL,
	[Titulo] [nvarchar](200) NOT NULL,
	[Mensaje] [nvarchar](1000) NOT NULL,
	[Leida] [bit] NOT NULL,
	[FechaCreacion] [datetime2](0) NOT NULL,
	[FechaLectura] [datetime2](0) NULL,
 CONSTRAINT [PK_Notificaciones] PRIMARY KEY CLUSTERED 
(
	[NotificacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OfertasTecnico]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfertasTecnico](
	[OfertaTecnicoID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[TecnicoID] [nvarchar](450) NOT NULL,
	[EstadoOfertaID] [int] NOT NULL,
	[DistanciaMetros] [decimal](12, 2) NULL,
	[OrdenOferta] [int] NULL,
	[FechaEnvio] [datetime2](0) NOT NULL,
	[FechaExpiracion] [datetime2](0) NULL,
	[FechaRespuesta] [datetime2](0) NULL,
	[Mensaje] [nvarchar](500) NULL,
 CONSTRAINT [PK_OfertasTecnico] PRIMARY KEY CLUSTERED 
(
	[OfertaTecnicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Ofertas_Reserva_Tecnico] UNIQUE NONCLUSTERED 
(
	[ReservaID] ASC,
	[TecnicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpcionesPregunta]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpcionesPregunta](
	[OpcionPreguntaID] [int] IDENTITY(1,1) NOT NULL,
	[PreguntaServicioID] [int] NOT NULL,
	[TextoOpcion] [nvarchar](300) NOT NULL,
	[Valor] [nvarchar](100) NULL,
	[Orden] [int] NOT NULL,
	[Activa] [bit] NOT NULL,
	[AjustePrecio] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_OpcionesPregunta] PRIMARY KEY CLUSTERED 
(
	[OpcionPreguntaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Opciones_Pregunta_Orden] UNIQUE NONCLUSTERED 
(
	[PreguntaServicioID] ASC,
	[Orden] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[PagoID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[MontoTotal] [decimal](12, 2) NOT NULL,
	[ComisionPlataforma] [decimal](12, 2) NOT NULL,
	[MontoNetoTecnico] [decimal](12, 2) NOT NULL,
	[Moneda] [char](3) NOT NULL,
	[ProveedorPago] [varchar](50) NULL,
	[IdempotencyKey] [uniqueidentifier] NULL,
	[FechaCreacion] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED 
(
	[PagoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permisos]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permisos](
	[PermisoID] [int] IDENTITY(1,1) NOT NULL,
	[CodigoPermiso] [varchar](100) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Descripcion] [nvarchar](300) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Permisos] PRIMARY KEY CLUSTERED 
(
	[PermisoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Permisos_Codigo] UNIQUE NONCLUSTERED 
(
	[CodigoPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Permisos_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PreguntasServicio]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PreguntasServicio](
	[PreguntaServicioID] [int] IDENTITY(1,1) NOT NULL,
	[ServicioID] [int] NOT NULL,
	[TextoPregunta] [nvarchar](500) NOT NULL,
	[TipoRespuesta] [varchar](20) NOT NULL,
	[Obligatoria] [bit] NOT NULL,
	[Orden] [int] NOT NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_PreguntasServicio] PRIMARY KEY CLUSTERED 
(
	[PreguntaServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Preguntas_Servicio_Orden] UNIQUE NONCLUSTERED 
(
	[ServicioID] ASC,
	[Orden] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provincias]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provincias](
	[ProvinciaID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[CodigoDTA] [char](1) NOT NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_Provincias] PRIMARY KEY CLUSTERED 
(
	[ProvinciaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Provincias_CodigoDTA] UNIQUE NONCLUSTERED 
(
	[CodigoDTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Provincias_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RespuestasReserva]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RespuestasReserva](
	[RespuestaReservaID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReservaID] [bigint] NOT NULL,
	[PreguntaServicioID] [int] NOT NULL,
	[OpcionPreguntaID] [int] NULL,
	[RespuestaTexto] [nvarchar](2000) NULL,
	[FechaRespuesta] [datetime2](0) NOT NULL,
	[AjustePrecioAplicado] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_RespuestasReserva] PRIMARY KEY CLUSTERED 
(
	[RespuestaReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_RespuestasReserva_Reserva_Pregunta] UNIQUE NONCLUSTERED 
(
	[ReservaID] ASC,
	[PreguntaServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RespuestasReservaOpciones]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RespuestasReservaOpciones](
	[RespuestaReservaID] [bigint] NOT NULL,
	[OpcionPreguntaID] [int] NOT NULL,
 CONSTRAINT [PK_RespuestasReservaOpciones] PRIMARY KEY CLUSTERED 
(
	[RespuestaReservaID] ASC,
	[OpcionPreguntaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicios]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[ServicioID] [int] IDENTITY(1,1) NOT NULL,
	[CategoriaID] [int] NOT NULL,
	[SubcategoriaID] [int] NULL,
	[NombreServicio] [nvarchar](150) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
	[TarifaDiagnosticoBase] [decimal](12, 2) NOT NULL,
	[TiempoEstimadoMinutos] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Moneda] [char](3) NOT NULL,
 CONSTRAINT [PK_Servicios] PRIMARY KEY CLUSTERED 
(
	[ServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Servicios_Categoria_Nombre] UNIQUE NONCLUSTERED 
(
	[CategoriaID] ASC,
	[NombreServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SolicitudesReserva]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolicitudesReserva](
	[ReservaID] [bigint] IDENTITY(1,1) NOT NULL,
	[CodigoSeguimiento] [uniqueidentifier] NOT NULL,
	[ClienteID] [nvarchar](450) NOT NULL,
	[TecnicoID] [nvarchar](450) NULL,
	[ServicioID] [int] NOT NULL,
	[EstadoReservaID] [int] NOT NULL,
	[DireccionID] [bigint] NULL,
	[ProvinciaID] [int] NULL,
	[CantonID] [int] NULL,
	[DistritoID] [int] NULL,
	[MontoBaseCotizado] [decimal](12, 2) NOT NULL,
	[DuracionEstimadaMinutos] [int] NOT NULL,
	[FechaHoraProgramada] [datetime2](0) NOT NULL,
	[LatitudServicio] [decimal](9, 6) NULL,
	[LongitudServicio] [decimal](9, 6) NULL,
	[UbicacionGeoServicio]  AS (case when [LatitudServicio] IS NOT NULL AND [LongitudServicio] IS NOT NULL then [geography]::Point([LatitudServicio],[LongitudServicio],(4326))  end) PERSISTED,
	[FechaHoraSolicitud] [datetime2](0) NOT NULL,
	[FechaHoraCompletada] [datetime2](0) NULL,
	[DireccionServicio] [nvarchar](300) NOT NULL,
	[DescripcionProblema] [nvarchar](2000) NOT NULL,
	[NotasCliente] [nvarchar](1000) NULL,
	[FechaModificacion] [datetime2](0) NULL,
	[MontoAjustes] [decimal](12, 2) NOT NULL,
	[MontoTotalCotizado] [decimal](12, 2) NOT NULL,
	[Moneda] [char](3) NOT NULL,
 CONSTRAINT [PK_SolicitudesReserva] PRIMARY KEY CLUSTERED 
(
	[ReservaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_SolicitudesReserva_Codigo] UNIQUE NONCLUSTERED 
(
	[CodigoSeguimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubcategoriasServicio]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubcategoriasServicio](
	[SubcategoriaID] [int] IDENTITY(1,1) NOT NULL,
	[CategoriaID] [int] NOT NULL,
	[NombreSubcategoria] [nvarchar](120) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_SubcategoriasServicio] PRIMARY KEY CLUSTERED 
(
	[SubcategoriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Subcategorias_Categoria_Nombre] UNIQUE NONCLUSTERED 
(
	[CategoriaID] ASC,
	[NombreSubcategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TecnicoEspecialidades]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TecnicoEspecialidades](
	[TecnicoID] [nvarchar](450) NOT NULL,
	[ServicioID] [int] NOT NULL,
	[AniosExperiencia] [int] NOT NULL,
 CONSTRAINT [PK_TecnicoEspecialidades] PRIMARY KEY NONCLUSTERED 
(
	[TecnicoID] ASC,
	[ServicioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TecnicosPerfil]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TecnicosPerfil](
	[TecnicoID] [nvarchar](450) NOT NULL,
	[IdentificacionCedula] [nvarchar](30) NOT NULL,
	[EstadoVerificacion] [varchar](20) NOT NULL,
	[CalificacionPromedio] [decimal](3, 2) NOT NULL,
	[Disponible] [bit] NOT NULL,
	[ProvinciaCoberturaID] [int] NULL,
	[CantonCoberturaID] [int] NULL,
	[LatitudActual] [decimal](9, 6) NULL,
	[LongitudActual] [decimal](9, 6) NULL,
	[UbicacionGeoActual]  AS (case when [LatitudActual] IS NOT NULL AND [LongitudActual] IS NOT NULL then [geography]::Point([LatitudActual],[LongitudActual],(4326))  end) PERSISTED,
	[FechaVerificacion] [datetime2](0) NULL,
 CONSTRAINT [PK_TecnicosPerfil] PRIMARY KEY CLUSTERED 
(
	[TecnicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Tecnicos_Cedula] UNIQUE NONCLUSTERED 
(
	[IdentificacionCedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TecnicosUbicacionActual]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TecnicosUbicacionActual](
	[TecnicoUbicacionID] [bigint] IDENTITY(1,1) NOT NULL,
	[TecnicoID] [nvarchar](450) NOT NULL,
	[Latitud] [decimal](9, 6) NOT NULL,
	[Longitud] [decimal](9, 6) NOT NULL,
	[UbicacionGeo]  AS ([geography]::Point([Latitud],[Longitud],(4326))) PERSISTED,
	[FechaActualizacion] [datetime2](0) NOT NULL,
	[Activa] [bit] NOT NULL,
 CONSTRAINT [PK_TecnicosUbicacionActual] PRIMARY KEY CLUSTERED 
(
	[TecnicoUbicacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TecnicosUbicacion_Tecnico] UNIQUE NONCLUSTERED 
(
	[TecnicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposEvaluacion]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposEvaluacion](
	[TipoEvaluacionID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](40) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_TiposEvaluacion] PRIMARY KEY CLUSTERED 
(
	[TipoEvaluacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TiposEvaluacion_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioID] [nvarchar](450) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellidos] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](30) NULL,
	[EstadoUsuario] [varchar](20) NOT NULL,
	[FechaCreacion] [datetime2](0) NOT NULL,
	[UltimoAcceso] [datetime2](0) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[UsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditoria] ADD  CONSTRAINT [DF_Auditoria_Fecha]  DEFAULT (sysdatetime()) FOR [FechaEvento]
GO
ALTER TABLE [dbo].[Cantones] ADD  CONSTRAINT [DF_Cantones_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[CategoriasServicio] ADD  CONSTRAINT [DF_Categorias_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[ClientesPerfil] ADD  CONSTRAINT [DF_ClientesPerfil_Fecha]  DEFAULT (sysdatetime()) FOR [FechaActualizacion]
GO
ALTER TABLE [dbo].[DetallePrecioReserva] ADD  CONSTRAINT [DF_DetallePrecioReserva_Fecha]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[DireccionesCliente] ADD  CONSTRAINT [DF_Direcciones_Principal]  DEFAULT ((0)) FOR [EsPrincipal]
GO
ALTER TABLE [dbo].[DireccionesCliente] ADD  CONSTRAINT [DF_Direcciones_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[DireccionesCliente] ADD  CONSTRAINT [DF_Direcciones_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[DisponibilidadTecnico] ADD  CONSTRAINT [DF_Disponibilidad_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Distritos] ADD  CONSTRAINT [DF_Distritos_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Evaluaciones] ADD  CONSTRAINT [DF_Evaluaciones_Fecha]  DEFAULT (sysdatetime()) FOR [FechaEvaluacion]
GO
ALTER TABLE [dbo].[Evaluaciones] ADD  CONSTRAINT [DF_Evaluaciones_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[HistorialEstadosReserva] ADD  CONSTRAINT [DF_HistorialReserva_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCambio]
GO
ALTER TABLE [dbo].[IntentosPago] ADD  CONSTRAINT [DF_IntentosPago_Fecha]  DEFAULT (sysdatetime()) FOR [FechaIntento]
GO
ALTER TABLE [dbo].[LogsIntervencionOperativa] ADD  CONSTRAINT [DF_Logs_Fecha]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_Orden]  DEFAULT ((0)) FOR [Orden]
GO
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[MetodosPago] ADD  CONSTRAINT [DF_MetodosPago_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  CONSTRAINT [DF_Notificaciones_Leida]  DEFAULT ((0)) FOR [Leida]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  CONSTRAINT [DF_Notificaciones_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[OfertasTecnico] ADD  CONSTRAINT [DF_Ofertas_FechaEnvio]  DEFAULT (sysdatetime()) FOR [FechaEnvio]
GO
ALTER TABLE [dbo].[OpcionesPregunta] ADD  CONSTRAINT [DF_Opciones_Orden]  DEFAULT ((1)) FOR [Orden]
GO
ALTER TABLE [dbo].[OpcionesPregunta] ADD  CONSTRAINT [DF_Opciones_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[OpcionesPregunta] ADD  CONSTRAINT [DF_OpcionesPregunta_AjustePrecio]  DEFAULT ((0)) FOR [AjustePrecio]
GO
ALTER TABLE [dbo].[Pagos] ADD  CONSTRAINT [DF_Pagos_Moneda]  DEFAULT ('CRC') FOR [Moneda]
GO
ALTER TABLE [dbo].[Pagos] ADD  CONSTRAINT [DF_Pagos_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Permisos] ADD  CONSTRAINT [DF_Permisos_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[PreguntasServicio] ADD  CONSTRAINT [DF_Preguntas_Obligatoria]  DEFAULT ((1)) FOR [Obligatoria]
GO
ALTER TABLE [dbo].[PreguntasServicio] ADD  CONSTRAINT [DF_Preguntas_Orden]  DEFAULT ((1)) FOR [Orden]
GO
ALTER TABLE [dbo].[PreguntasServicio] ADD  CONSTRAINT [DF_Preguntas_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Provincias] ADD  CONSTRAINT [DF_Provincias_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[RespuestasReserva] ADD  CONSTRAINT [DF_RespuestasReserva_Fecha]  DEFAULT (sysdatetime()) FOR [FechaRespuesta]
GO
ALTER TABLE [dbo].[RespuestasReserva] ADD  CONSTRAINT [DF_RespuestasReserva_AjustePrecio]  DEFAULT ((0)) FOR [AjustePrecioAplicado]
GO
ALTER TABLE [dbo].[Servicios] ADD  CONSTRAINT [DF_Servicios_Tiempo]  DEFAULT ((60)) FOR [TiempoEstimadoMinutos]
GO
ALTER TABLE [dbo].[Servicios] ADD  CONSTRAINT [DF_Servicios_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Servicios] ADD  CONSTRAINT [DF_Servicios_Moneda]  DEFAULT ('CRC') FOR [Moneda]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_Codigo]  DEFAULT (newsequentialid()) FOR [CodigoSeguimiento]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_Duracion]  DEFAULT ((60)) FOR [DuracionEstimadaMinutos]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_FechaSolicitud]  DEFAULT (sysdatetime()) FOR [FechaHoraSolicitud]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_Descripcion]  DEFAULT (N'Pendiente de descripción') FOR [DescripcionProblema]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_MontoAjustes]  DEFAULT ((0)) FOR [MontoAjustes]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_MontoTotal]  DEFAULT ((0)) FOR [MontoTotalCotizado]
GO
ALTER TABLE [dbo].[SolicitudesReserva] ADD  CONSTRAINT [DF_Reservas_Moneda]  DEFAULT ('CRC') FOR [Moneda]
GO
ALTER TABLE [dbo].[SubcategoriasServicio] ADD  CONSTRAINT [DF_Subcategorias_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[TecnicoEspecialidades] ADD  CONSTRAINT [DF_TecEsp_Experiencia]  DEFAULT ((1)) FOR [AniosExperiencia]
GO
ALTER TABLE [dbo].[TecnicosPerfil] ADD  CONSTRAINT [DF_Tecnicos_EstadoVerificacion]  DEFAULT ('Pendiente') FOR [EstadoVerificacion]
GO
ALTER TABLE [dbo].[TecnicosPerfil] ADD  CONSTRAINT [DF_Tecnicos_Calificacion]  DEFAULT ((0.00)) FOR [CalificacionPromedio]
GO
ALTER TABLE [dbo].[TecnicosPerfil] ADD  CONSTRAINT [DF_Tecnicos_Disponible]  DEFAULT ((1)) FOR [Disponible]
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual] ADD  CONSTRAINT [DF_TecnicosUbicacion_Fecha]  DEFAULT (sysdatetime()) FOR [FechaActualizacion]
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual] ADD  CONSTRAINT [DF_TecnicosUbicacion_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Estado]  DEFAULT ('Activo') FOR [EstadoUsuario]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_FechaCreacion]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Auditoria]  WITH CHECK ADD  CONSTRAINT [FK_Auditoria_Usuario] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[Auditoria] CHECK CONSTRAINT [FK_Auditoria_Usuario]
GO
ALTER TABLE [dbo].[Cantones]  WITH CHECK ADD  CONSTRAINT [FK_Cantones_Provincias] FOREIGN KEY([ProvinciaID])
REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[Cantones] CHECK CONSTRAINT [FK_Cantones_Provincias]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [FK_ClientesPerfil_CantonProvincia] FOREIGN KEY([CantonID], [ProvinciaID])
REFERENCES [dbo].[Cantones] ([CantonID], [ProvinciaID])
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [FK_ClientesPerfil_CantonProvincia]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [FK_ClientesPerfil_DistritoCanton] FOREIGN KEY([DistritoID], [CantonID])
REFERENCES [dbo].[Distritos] ([DistritoID], [CantonID])
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [FK_ClientesPerfil_DistritoCanton]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [FK_ClientesPerfil_Provincia] FOREIGN KEY([ProvinciaID])
REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [FK_ClientesPerfil_Provincia]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [FK_ClientesPerfil_Usuarios] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [FK_ClientesPerfil_Usuarios]
GO
ALTER TABLE [dbo].[DetallePrecioReserva]  WITH CHECK ADD  CONSTRAINT [FK_DetallePrecioReserva_Opcion] FOREIGN KEY([OpcionPreguntaID])
REFERENCES [dbo].[OpcionesPregunta] ([OpcionPreguntaID])
GO
ALTER TABLE [dbo].[DetallePrecioReserva] CHECK CONSTRAINT [FK_DetallePrecioReserva_Opcion]
GO
ALTER TABLE [dbo].[DetallePrecioReserva]  WITH CHECK ADD  CONSTRAINT [FK_DetallePrecioReserva_Pregunta] FOREIGN KEY([PreguntaServicioID])
REFERENCES [dbo].[PreguntasServicio] ([PreguntaServicioID])
GO
ALTER TABLE [dbo].[DetallePrecioReserva] CHECK CONSTRAINT [FK_DetallePrecioReserva_Pregunta]
GO
ALTER TABLE [dbo].[DetallePrecioReserva]  WITH CHECK ADD  CONSTRAINT [FK_DetallePrecioReserva_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[DetallePrecioReserva] CHECK CONSTRAINT [FK_DetallePrecioReserva_Reserva]
GO
ALTER TABLE [dbo].[DireccionesCliente]  WITH CHECK ADD  CONSTRAINT [FK_Direcciones_CantonProvincia] FOREIGN KEY([CantonID], [ProvinciaID])
REFERENCES [dbo].[Cantones] ([CantonID], [ProvinciaID])
GO
ALTER TABLE [dbo].[DireccionesCliente] CHECK CONSTRAINT [FK_Direcciones_CantonProvincia]
GO
ALTER TABLE [dbo].[DireccionesCliente]  WITH CHECK ADD  CONSTRAINT [FK_Direcciones_Cliente] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[ClientesPerfil] ([ClienteID])
GO
ALTER TABLE [dbo].[DireccionesCliente] CHECK CONSTRAINT [FK_Direcciones_Cliente]
GO
ALTER TABLE [dbo].[DireccionesCliente]  WITH CHECK ADD  CONSTRAINT [FK_Direcciones_DistritoCanton] FOREIGN KEY([DistritoID], [CantonID])
REFERENCES [dbo].[Distritos] ([DistritoID], [CantonID])
GO
ALTER TABLE [dbo].[DireccionesCliente] CHECK CONSTRAINT [FK_Direcciones_DistritoCanton]
GO
ALTER TABLE [dbo].[DireccionesCliente]  WITH CHECK ADD  CONSTRAINT [FK_Direcciones_Provincia] FOREIGN KEY([ProvinciaID])
REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[DireccionesCliente] CHECK CONSTRAINT [FK_Direcciones_Provincia]
GO
ALTER TABLE [dbo].[DisponibilidadTecnico]  WITH CHECK ADD  CONSTRAINT [FK_Disponibilidad_Tecnico] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[TecnicosPerfil] ([TecnicoID])
GO
ALTER TABLE [dbo].[DisponibilidadTecnico] CHECK CONSTRAINT [FK_Disponibilidad_Tecnico]
GO
ALTER TABLE [dbo].[Distritos]  WITH CHECK ADD  CONSTRAINT [FK_Distritos_Cantones] FOREIGN KEY([CantonID])
REFERENCES [dbo].[Cantones] ([CantonID])
GO
ALTER TABLE [dbo].[Distritos] CHECK CONSTRAINT [FK_Distritos_Cantones]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [FK_Evaluaciones_Evaluado] FOREIGN KEY([EvaluadoUsuarioID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [FK_Evaluaciones_Evaluado]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [FK_Evaluaciones_Evaluador] FOREIGN KEY([EvaluadorUsuarioID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [FK_Evaluaciones_Evaluador]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [FK_Evaluaciones_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [FK_Evaluaciones_Reserva]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [FK_Evaluaciones_Servicio] FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [FK_Evaluaciones_Servicio]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [FK_Evaluaciones_Tipo] FOREIGN KEY([TipoEvaluacionID])
REFERENCES [dbo].[TiposEvaluacion] ([TipoEvaluacionID])
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [FK_Evaluaciones_Tipo]
GO
ALTER TABLE [dbo].[HistorialEstadosReserva]  WITH CHECK ADD  CONSTRAINT [FK_Historial_EstadoAnterior] FOREIGN KEY([EstadoAnteriorID])
REFERENCES [dbo].[EstadosReserva] ([EstadoReservaID])
GO
ALTER TABLE [dbo].[HistorialEstadosReserva] CHECK CONSTRAINT [FK_Historial_EstadoAnterior]
GO
ALTER TABLE [dbo].[HistorialEstadosReserva]  WITH CHECK ADD  CONSTRAINT [FK_Historial_EstadoNuevo] FOREIGN KEY([EstadoNuevoID])
REFERENCES [dbo].[EstadosReserva] ([EstadoReservaID])
GO
ALTER TABLE [dbo].[HistorialEstadosReserva] CHECK CONSTRAINT [FK_Historial_EstadoNuevo]
GO
ALTER TABLE [dbo].[HistorialEstadosReserva]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[HistorialEstadosReserva] CHECK CONSTRAINT [FK_Historial_Reserva]
GO
ALTER TABLE [dbo].[HistorialEstadosReserva]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Usuario] FOREIGN KEY([UsuarioModificadorID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[HistorialEstadosReserva] CHECK CONSTRAINT [FK_Historial_Usuario]
GO
ALTER TABLE [dbo].[IntentosPago]  WITH CHECK ADD  CONSTRAINT [FK_IntentosPago_Estado] FOREIGN KEY([EstadoPagoID])
REFERENCES [dbo].[EstadosPago] ([EstadoPagoID])
GO
ALTER TABLE [dbo].[IntentosPago] CHECK CONSTRAINT [FK_IntentosPago_Estado]
GO
ALTER TABLE [dbo].[IntentosPago]  WITH CHECK ADD  CONSTRAINT [FK_IntentosPago_Metodo] FOREIGN KEY([MetodoPagoID])
REFERENCES [dbo].[MetodosPago] ([MetodoPagoID])
GO
ALTER TABLE [dbo].[IntentosPago] CHECK CONSTRAINT [FK_IntentosPago_Metodo]
GO
ALTER TABLE [dbo].[IntentosPago]  WITH CHECK ADD  CONSTRAINT [FK_IntentosPago_Pago] FOREIGN KEY([PagoID])
REFERENCES [dbo].[Pagos] ([PagoID])
GO
ALTER TABLE [dbo].[IntentosPago] CHECK CONSTRAINT [FK_IntentosPago_Pago]
GO
ALTER TABLE [dbo].[LogsIntervencionOperativa]  WITH CHECK ADD  CONSTRAINT [FK_Logs_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[LogsIntervencionOperativa] CHECK CONSTRAINT [FK_Logs_Reserva]
GO
ALTER TABLE [dbo].[LogsIntervencionOperativa]  WITH CHECK ADD  CONSTRAINT [FK_Logs_Usuario] FOREIGN KEY([UsuarioIntervencionID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[LogsIntervencionOperativa] CHECK CONSTRAINT [FK_Logs_Usuario]
GO
ALTER TABLE [dbo].[MenuPermisos]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermisos_Menu] FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menus] ([MenuID])
GO
ALTER TABLE [dbo].[MenuPermisos] CHECK CONSTRAINT [FK_MenuPermisos_Menu]
GO
ALTER TABLE [dbo].[MenuPermisos]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermisos_Permiso] FOREIGN KEY([PermisoID])
REFERENCES [dbo].[Permisos] ([PermisoID])
GO
ALTER TABLE [dbo].[MenuPermisos] CHECK CONSTRAINT [FK_MenuPermisos_Permiso]
GO
ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [FK_Menus_MenuPadre] FOREIGN KEY([MenuPadreID])
REFERENCES [dbo].[Menus] ([MenuID])
GO
ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [FK_Menus_MenuPadre]
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD  CONSTRAINT [FK_Notificaciones_Oferta] FOREIGN KEY([OfertaTecnicoID])
REFERENCES [dbo].[OfertasTecnico] ([OfertaTecnicoID])
GO
ALTER TABLE [dbo].[Notificaciones] CHECK CONSTRAINT [FK_Notificaciones_Oferta]
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD  CONSTRAINT [FK_Notificaciones_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[Notificaciones] CHECK CONSTRAINT [FK_Notificaciones_Reserva]
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD  CONSTRAINT [FK_Notificaciones_Usuario] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[Notificaciones] CHECK CONSTRAINT [FK_Notificaciones_Usuario]
GO
ALTER TABLE [dbo].[OfertasTecnico]  WITH CHECK ADD  CONSTRAINT [FK_Ofertas_Estado] FOREIGN KEY([EstadoOfertaID])
REFERENCES [dbo].[EstadosOfertaTecnico] ([EstadoOfertaID])
GO
ALTER TABLE [dbo].[OfertasTecnico] CHECK CONSTRAINT [FK_Ofertas_Estado]
GO
ALTER TABLE [dbo].[OfertasTecnico]  WITH CHECK ADD  CONSTRAINT [FK_Ofertas_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[OfertasTecnico] CHECK CONSTRAINT [FK_Ofertas_Reserva]
GO
ALTER TABLE [dbo].[OfertasTecnico]  WITH CHECK ADD  CONSTRAINT [FK_Ofertas_Tecnico] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[TecnicosPerfil] ([TecnicoID])
GO
ALTER TABLE [dbo].[OfertasTecnico] CHECK CONSTRAINT [FK_Ofertas_Tecnico]
GO
ALTER TABLE [dbo].[OpcionesPregunta]  WITH CHECK ADD  CONSTRAINT [FK_Opciones_Pregunta] FOREIGN KEY([PreguntaServicioID])
REFERENCES [dbo].[PreguntasServicio] ([PreguntaServicioID])
GO
ALTER TABLE [dbo].[OpcionesPregunta] CHECK CONSTRAINT [FK_Opciones_Pregunta]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_Reserva]
GO
ALTER TABLE [dbo].[PreguntasServicio]  WITH CHECK ADD  CONSTRAINT [FK_Preguntas_Servicio] FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[PreguntasServicio] CHECK CONSTRAINT [FK_Preguntas_Servicio]
GO
ALTER TABLE [dbo].[RespuestasReserva]  WITH CHECK ADD  CONSTRAINT [FK_RespuestasReserva_Opcion] FOREIGN KEY([OpcionPreguntaID])
REFERENCES [dbo].[OpcionesPregunta] ([OpcionPreguntaID])
GO
ALTER TABLE [dbo].[RespuestasReserva] CHECK CONSTRAINT [FK_RespuestasReserva_Opcion]
GO
ALTER TABLE [dbo].[RespuestasReserva]  WITH CHECK ADD  CONSTRAINT [FK_RespuestasReserva_Pregunta] FOREIGN KEY([PreguntaServicioID])
REFERENCES [dbo].[PreguntasServicio] ([PreguntaServicioID])
GO
ALTER TABLE [dbo].[RespuestasReserva] CHECK CONSTRAINT [FK_RespuestasReserva_Pregunta]
GO
ALTER TABLE [dbo].[RespuestasReserva]  WITH CHECK ADD  CONSTRAINT [FK_RespuestasReserva_Reserva] FOREIGN KEY([ReservaID])
REFERENCES [dbo].[SolicitudesReserva] ([ReservaID])
GO
ALTER TABLE [dbo].[RespuestasReserva] CHECK CONSTRAINT [FK_RespuestasReserva_Reserva]
GO
ALTER TABLE [dbo].[RespuestasReservaOpciones]  WITH CHECK ADD  CONSTRAINT [FK_RespReservaOpciones_Opcion] FOREIGN KEY([OpcionPreguntaID])
REFERENCES [dbo].[OpcionesPregunta] ([OpcionPreguntaID])
GO
ALTER TABLE [dbo].[RespuestasReservaOpciones] CHECK CONSTRAINT [FK_RespReservaOpciones_Opcion]
GO
ALTER TABLE [dbo].[RespuestasReservaOpciones]  WITH CHECK ADD  CONSTRAINT [FK_RespReservaOpciones_Respuesta] FOREIGN KEY([RespuestaReservaID])
REFERENCES [dbo].[RespuestasReserva] ([RespuestaReservaID])
GO
ALTER TABLE [dbo].[RespuestasReservaOpciones] CHECK CONSTRAINT [FK_RespReservaOpciones_Respuesta]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [FK_Servicios_Categoria] FOREIGN KEY([CategoriaID])
REFERENCES [dbo].[CategoriasServicio] ([CategoriaID])
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [FK_Servicios_Categoria]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [FK_Servicios_Subcategoria] FOREIGN KEY([SubcategoriaID])
REFERENCES [dbo].[SubcategoriasServicio] ([SubcategoriaID])
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [FK_Servicios_Subcategoria]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_CantonProvincia] FOREIGN KEY([CantonID], [ProvinciaID])
REFERENCES [dbo].[Cantones] ([CantonID], [ProvinciaID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_CantonProvincia]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Cliente] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[ClientesPerfil] ([ClienteID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Cliente]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Direccion] FOREIGN KEY([DireccionID])
REFERENCES [dbo].[DireccionesCliente] ([DireccionID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Direccion]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_DistritoCanton] FOREIGN KEY([DistritoID], [CantonID])
REFERENCES [dbo].[Distritos] ([DistritoID], [CantonID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_DistritoCanton]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Estado] FOREIGN KEY([EstadoReservaID])
REFERENCES [dbo].[EstadosReserva] ([EstadoReservaID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Estado]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Provincia] FOREIGN KEY([ProvinciaID])
REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Provincia]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Servicio] FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Servicio]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Tecnico] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[TecnicosPerfil] ([TecnicoID])
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [FK_Reservas_Tecnico]
GO
ALTER TABLE [dbo].[SubcategoriasServicio]  WITH CHECK ADD  CONSTRAINT [FK_Subcategorias_Categoria] FOREIGN KEY([CategoriaID])
REFERENCES [dbo].[CategoriasServicio] ([CategoriaID])
GO
ALTER TABLE [dbo].[SubcategoriasServicio] CHECK CONSTRAINT [FK_Subcategorias_Categoria]
GO
ALTER TABLE [dbo].[TecnicoEspecialidades]  WITH CHECK ADD  CONSTRAINT [FK_TecEsp_Servicio] FOREIGN KEY([ServicioID])
REFERENCES [dbo].[Servicios] ([ServicioID])
GO
ALTER TABLE [dbo].[TecnicoEspecialidades] CHECK CONSTRAINT [FK_TecEsp_Servicio]
GO
ALTER TABLE [dbo].[TecnicoEspecialidades]  WITH CHECK ADD  CONSTRAINT [FK_TecEsp_Tecnico] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[TecnicosPerfil] ([TecnicoID])
GO
ALTER TABLE [dbo].[TecnicoEspecialidades] CHECK CONSTRAINT [FK_TecEsp_Tecnico]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [FK_TecnicosPerfil_CantonProvincia] FOREIGN KEY([CantonCoberturaID], [ProvinciaCoberturaID])
REFERENCES [dbo].[Cantones] ([CantonID], [ProvinciaID])
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [FK_TecnicosPerfil_CantonProvincia]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [FK_TecnicosPerfil_Provincia] FOREIGN KEY([ProvinciaCoberturaID])
REFERENCES [dbo].[Provincias] ([ProvinciaID])
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [FK_TecnicosPerfil_Provincia]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [FK_TecnicosPerfil_Usuarios] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[Usuarios] ([UsuarioID])
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [FK_TecnicosPerfil_Usuarios]
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual]  WITH CHECK ADD  CONSTRAINT [FK_TecnicosUbicacion_Tecnico] FOREIGN KEY([TecnicoID])
REFERENCES [dbo].[TecnicosPerfil] ([TecnicoID])
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual] CHECK CONSTRAINT [FK_TecnicosUbicacion_Tecnico]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_AspNetUsers_UsuarioID] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_AspNetUsers_UsuarioID]
GO
ALTER TABLE [dbo].[Auditoria]  WITH CHECK ADD  CONSTRAINT [CK_Auditoria_Operacion] CHECK  (([Operacion]='SECURITY' OR [Operacion]='LOGOUT' OR [Operacion]='LOGIN' OR [Operacion]='DELETE' OR [Operacion]='UPDATE' OR [Operacion]='INSERT'))
GO
ALTER TABLE [dbo].[Auditoria] CHECK CONSTRAINT [CK_Auditoria_Operacion]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [CK_ClientesPerfil_Latitud] CHECK  (([Latitud] IS NULL OR [Latitud]>=(-90) AND [Latitud]<=(90)))
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [CK_ClientesPerfil_Latitud]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [CK_ClientesPerfil_Longitud] CHECK  (([Longitud] IS NULL OR [Longitud]>=(-180) AND [Longitud]<=(180)))
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [CK_ClientesPerfil_Longitud]
GO
ALTER TABLE [dbo].[ClientesPerfil]  WITH CHECK ADD  CONSTRAINT [CK_ClientesPerfil_UbicacionCompleta] CHECK  (([Latitud] IS NULL AND [Longitud] IS NULL OR [Latitud] IS NOT NULL AND [Longitud] IS NOT NULL))
GO
ALTER TABLE [dbo].[ClientesPerfil] CHECK CONSTRAINT [CK_ClientesPerfil_UbicacionCompleta]
GO
ALTER TABLE [dbo].[DetallePrecioReserva]  WITH CHECK ADD  CONSTRAINT [CK_DetallePrecioReserva_Monto] CHECK  (([Monto]>=(0)))
GO
ALTER TABLE [dbo].[DetallePrecioReserva] CHECK CONSTRAINT [CK_DetallePrecioReserva_Monto]
GO
ALTER TABLE [dbo].[DetallePrecioReserva]  WITH CHECK ADD  CONSTRAINT [CK_DetallePrecioReserva_Tipo] CHECK  (([TipoConcepto]='RECARGO' OR [TipoConcepto]='DESCUENTO' OR [TipoConcepto]='AJUSTE' OR [TipoConcepto]='BASE'))
GO
ALTER TABLE [dbo].[DetallePrecioReserva] CHECK CONSTRAINT [CK_DetallePrecioReserva_Tipo]
GO
ALTER TABLE [dbo].[DireccionesCliente]  WITH CHECK ADD  CONSTRAINT [CK_Direcciones_Coordenadas] CHECK  (([Latitud] IS NULL AND [Longitud] IS NULL OR [Latitud] IS NOT NULL AND [Longitud] IS NOT NULL))
GO
ALTER TABLE [dbo].[DireccionesCliente] CHECK CONSTRAINT [CK_Direcciones_Coordenadas]
GO
ALTER TABLE [dbo].[DisponibilidadTecnico]  WITH CHECK ADD  CONSTRAINT [CK_Disponibilidad_Dia] CHECK  (([DiaSemana]>=(1) AND [DiaSemana]<=(7)))
GO
ALTER TABLE [dbo].[DisponibilidadTecnico] CHECK CONSTRAINT [CK_Disponibilidad_Dia]
GO
ALTER TABLE [dbo].[DisponibilidadTecnico]  WITH CHECK ADD  CONSTRAINT [CK_Disponibilidad_Horas] CHECK  (([HoraInicio]<[HoraFin]))
GO
ALTER TABLE [dbo].[DisponibilidadTecnico] CHECK CONSTRAINT [CK_Disponibilidad_Horas]
GO
ALTER TABLE [dbo].[Evaluaciones]  WITH CHECK ADD  CONSTRAINT [CK_Evaluaciones_Puntuacion] CHECK  (([Puntuacion]>=(1) AND [Puntuacion]<=(5)))
GO
ALTER TABLE [dbo].[Evaluaciones] CHECK CONSTRAINT [CK_Evaluaciones_Puntuacion]
GO
ALTER TABLE [dbo].[IntentosPago]  WITH CHECK ADD  CONSTRAINT [CK_IntentosPago_Monto] CHECK  (([MontoIntento]>=(0)))
GO
ALTER TABLE [dbo].[IntentosPago] CHECK CONSTRAINT [CK_IntentosPago_Monto]
GO
ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [CK_Menus_NoSelfParent] CHECK  (([MenuPadreID] IS NULL OR [MenuPadreID]<>[MenuID]))
GO
ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [CK_Menus_NoSelfParent]
GO
ALTER TABLE [dbo].[OfertasTecnico]  WITH CHECK ADD  CONSTRAINT [CK_Ofertas_Distancia] CHECK  (([DistanciaMetros] IS NULL OR [DistanciaMetros]>=(0)))
GO
ALTER TABLE [dbo].[OfertasTecnico] CHECK CONSTRAINT [CK_Ofertas_Distancia]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Comision] CHECK  (([ComisionPlataforma]>=(0) AND [ComisionPlataforma]<=[MontoTotal]))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Comision]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Moneda] CHECK  (([Moneda]='USD' OR [Moneda]='CRC'))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Moneda]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_MontoTotal] CHECK  (([MontoTotal]>=(0)))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_MontoTotal]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Neto] CHECK  (([MontoNetoTecnico]>=(0) AND [MontoNetoTecnico]=([MontoTotal]-[ComisionPlataforma])))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Neto]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Neto_v3] CHECK  (([MontoNetoTecnico]>=(0) AND [MontoNetoTecnico]=([MontoTotal]-[ComisionPlataforma])))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Neto_v3]
GO
ALTER TABLE [dbo].[PreguntasServicio]  WITH CHECK ADD  CONSTRAINT [CK_Preguntas_Tipo] CHECK  (([TipoRespuesta]='FECHA' OR [TipoRespuesta]='MULTIPLE' OR [TipoRespuesta]='OPCION' OR [TipoRespuesta]='SI_NO' OR [TipoRespuesta]='NUMERO' OR [TipoRespuesta]='TEXTO'))
GO
ALTER TABLE [dbo].[PreguntasServicio] CHECK CONSTRAINT [CK_Preguntas_Tipo]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [CK_Servicios_Moneda] CHECK  (([Moneda]='USD' OR [Moneda]='CRC'))
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [CK_Servicios_Moneda]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [CK_Servicios_Tarifa] CHECK  (([TarifaDiagnosticoBase]>=(0)))
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [CK_Servicios_Tarifa]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [CK_Servicios_Tiempo] CHECK  (([TiempoEstimadoMinutos]>(0)))
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [CK_Servicios_Tiempo]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_CoordenadasServicio] CHECK  (([LatitudServicio] IS NULL AND [LongitudServicio] IS NULL OR [LatitudServicio] IS NOT NULL AND [LongitudServicio] IS NOT NULL))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_CoordenadasServicio]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_DireccionCompleta] CHECK  (([DireccionID] IS NOT NULL AND [ProvinciaID] IS NOT NULL AND [CantonID] IS NOT NULL AND [DistritoID] IS NOT NULL AND nullif(ltrim(rtrim([DescripcionProblema])),N'') IS NOT NULL))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_DireccionCompleta]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_Duracion] CHECK  (([DuracionEstimadaMinutos]>(0)))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_Duracion]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_FechaCompletada] CHECK  (([FechaHoraCompletada] IS NULL OR [FechaHoraCompletada]>=[FechaHoraSolicitud]))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_FechaCompletada]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_Moneda] CHECK  (([Moneda]='USD' OR [Moneda]='CRC'))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_Moneda]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_Monto] CHECK  (([MontoBaseCotizado]>=(0)))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_Monto]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_MontoAjustes] CHECK  (([MontoAjustes] IS NOT NULL))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_MontoAjustes]
GO
ALTER TABLE [dbo].[SolicitudesReserva]  WITH CHECK ADD  CONSTRAINT [CK_Reservas_MontoTotal] CHECK  (([MontoTotalCotizado]>=(0)))
GO
ALTER TABLE [dbo].[SolicitudesReserva] CHECK CONSTRAINT [CK_Reservas_MontoTotal]
GO
ALTER TABLE [dbo].[TecnicoEspecialidades]  WITH CHECK ADD  CONSTRAINT [CK_TecEsp_Experiencia] CHECK  (([AniosExperiencia]>=(0)))
GO
ALTER TABLE [dbo].[TecnicoEspecialidades] CHECK CONSTRAINT [CK_TecEsp_Experiencia]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_Calificacion] CHECK  (([CalificacionPromedio]>=(0) AND [CalificacionPromedio]<=(5)))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_Calificacion]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_Cobertura] CHECK  (([ProvinciaCoberturaID] IS NULL AND [CantonCoberturaID] IS NULL OR [ProvinciaCoberturaID] IS NOT NULL AND [CantonCoberturaID] IS NOT NULL))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_Cobertura]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_EstadoVerificacion] CHECK  (([EstadoVerificacion]='Suspendido' OR [EstadoVerificacion]='Rechazado' OR [EstadoVerificacion]='Aprobado' OR [EstadoVerificacion]='Pendiente'))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_EstadoVerificacion]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_Latitud] CHECK  (([LatitudActual] IS NULL OR [LatitudActual]>=(-90) AND [LatitudActual]<=(90)))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_Latitud]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_Longitud] CHECK  (([LongitudActual] IS NULL OR [LongitudActual]>=(-180) AND [LongitudActual]<=(180)))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_Longitud]
GO
ALTER TABLE [dbo].[TecnicosPerfil]  WITH CHECK ADD  CONSTRAINT [CK_Tecnicos_UbicacionCompleta] CHECK  (([LatitudActual] IS NULL AND [LongitudActual] IS NULL OR [LatitudActual] IS NOT NULL AND [LongitudActual] IS NOT NULL))
GO
ALTER TABLE [dbo].[TecnicosPerfil] CHECK CONSTRAINT [CK_Tecnicos_UbicacionCompleta]
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual]  WITH CHECK ADD  CONSTRAINT [CK_TecnicosUbicacion_Latitud] CHECK  (([Latitud]>=(-90) AND [Latitud]<=(90)))
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual] CHECK CONSTRAINT [CK_TecnicosUbicacion_Latitud]
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual]  WITH CHECK ADD  CONSTRAINT [CK_TecnicosUbicacion_Longitud] CHECK  (([Longitud]>=(-180) AND [Longitud]<=(180)))
GO
ALTER TABLE [dbo].[TecnicosUbicacionActual] CHECK CONSTRAINT [CK_TecnicosUbicacion_Longitud]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [CK_Usuarios_Estado] CHECK  (([EstadoUsuario]='Bloqueado' OR [EstadoUsuario]='Inactivo' OR [EstadoUsuario]='Activo'))
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [CK_Usuarios_Estado]
GO
/****** Object:  StoredProcedure [dbo].[usp_Evaluacion_Crear]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Evaluacion_Crear]
    @ReservaID BIGINT,
    @EvaluadorUsuarioID NVARCHAR(450),
    @EvaluadoUsuarioID NVARCHAR(450)=NULL,
    @ServicioID INT=NULL,
    @TipoEvaluacionID INT,
    @Puntuacion TINYINT,
    @Comentario NVARCHAR(1000)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CodigoTipo VARCHAR(40),
            @ClienteID NVARCHAR(450),
            @TecnicoID NVARCHAR(450),
            @ServicioReservaID INT,
            @EstadoCodigo VARCHAR(30);

    SELECT
        @ClienteID = r.ClienteID,
        @TecnicoID = r.TecnicoID,
        @ServicioReservaID = r.ServicioID,
        @EstadoCodigo = er.Codigo
    FROM dbo.SolicitudesReserva r
    INNER JOIN dbo.EstadosReserva er ON er.EstadoReservaID = r.EstadoReservaID
    WHERE r.ReservaID=@ReservaID;

    IF @ClienteID IS NULL
        THROW 50006, 'La reserva no existe.', 1;

    IF @EstadoCodigo <> 'COMPLETADA'
        THROW 50010, 'Solo se puede evaluar una reserva completada.', 1;

    SELECT @CodigoTipo = Codigo
    FROM dbo.TiposEvaluacion
    WHERE TipoEvaluacionID=@TipoEvaluacionID;

    IF @CodigoTipo IS NULL
        THROW 50011, 'El tipo de evaluacion no existe.', 1;

    IF @ServicioID IS NOT NULL AND @ServicioID <> @ServicioReservaID
        THROW 50012, 'El servicio de la evaluacion no coincide con la reserva.', 1;

    IF @CodigoTipo = 'CLIENTE_TECNICO'
    BEGIN
        IF @EvaluadorUsuarioID <> @ClienteID OR @TecnicoID IS NULL OR @EvaluadoUsuarioID <> @TecnicoID
            THROW 50013, 'CLIENTE_TECNICO requiere que el cliente de la reserva evalúe al tecnico asignado.', 1;
    END
    ELSE IF @CodigoTipo = 'TECNICO_CLIENTE'
    BEGIN
        IF @TecnicoID IS NULL OR @EvaluadorUsuarioID <> @TecnicoID OR @EvaluadoUsuarioID <> @ClienteID
            THROW 50014, 'TECNICO_CLIENTE requiere que el tecnico asignado evalúe al cliente.', 1;
    END
    ELSE IF @CodigoTipo = 'CLIENTE_SERVICIO'
    BEGIN
        IF @EvaluadorUsuarioID <> @ClienteID OR @EvaluadoUsuarioID IS NOT NULL
            THROW 50015, 'CLIENTE_SERVICIO requiere que el cliente evalúe el servicio sin usuario evaluado.', 1;
        SET @ServicioID = @ServicioReservaID;
    END
    ELSE IF @CodigoTipo = 'TECNICO_SERVICIO'
    BEGIN
        IF @TecnicoID IS NULL OR @EvaluadorUsuarioID <> @TecnicoID OR @EvaluadoUsuarioID IS NOT NULL
            THROW 50016, 'TECNICO_SERVICIO requiere que el tecnico evalúe el servicio sin usuario evaluado.', 1;
        SET @ServicioID = @ServicioReservaID;
    END
    ELSE
        THROW 50017, 'Tipo de evaluacion no soportado.', 1;

    INSERT INTO dbo.Evaluaciones
        (ReservaID,EvaluadorUsuarioID,EvaluadoUsuarioID,ServicioID,
         TipoEvaluacionID,Puntuacion,Comentario)
    VALUES
        (@ReservaID,@EvaluadorUsuarioID,@EvaluadoUsuarioID,@ServicioID,
         @TipoEvaluacionID,@Puntuacion,@Comentario);

    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS EvaluacionID;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_OfertaTecnico_Aceptar]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_OfertaTecnico_Aceptar]
    @OfertaTecnicoID BIGINT,@TecnicoID NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; BEGIN TRANSACTION;
    DECLARE @ReservaID BIGINT,@Aceptada INT,@Pendiente INT,@EstadoSolicitada INT,@EstadoAsignada INT;
    SELECT @Aceptada=EstadoOfertaID FROM dbo.EstadosOfertaTecnico WHERE Codigo='ACEPTADA';
    SELECT @Pendiente=EstadoOfertaID FROM dbo.EstadosOfertaTecnico WHERE Codigo='PENDIENTE';
    SELECT @EstadoSolicitada=EstadoReservaID FROM dbo.EstadosReserva WHERE Codigo='SOLICITADA';
    SELECT @EstadoAsignada=EstadoReservaID FROM dbo.EstadosReserva WHERE Codigo='ASIGNADA';
    SELECT @ReservaID=ReservaID FROM dbo.OfertasTecnico WITH(UPDLOCK,HOLDLOCK) WHERE OfertaTecnicoID=@OfertaTecnicoID AND TecnicoID=@TecnicoID AND EstadoOfertaID=@Pendiente;
    IF @ReservaID IS NULL THROW 50020,'La oferta no existe, ya fue respondida o no pertenece al tecnico.',1;
    IF NOT EXISTS (SELECT 1 FROM dbo.SolicitudesReserva WITH(UPDLOCK,HOLDLOCK) WHERE ReservaID=@ReservaID AND EstadoReservaID=@EstadoSolicitada) THROW 50021,'La reserva ya fue asignada a otro tecnico o no esta disponible.',1;
    UPDATE dbo.OfertasTecnico SET EstadoOfertaID=@Aceptada,FechaRespuesta=SYSDATETIME() WHERE OfertaTecnicoID=@OfertaTecnicoID;
    UPDATE dbo.OfertasTecnico SET EstadoOfertaID=(SELECT EstadoOfertaID FROM dbo.EstadosOfertaTecnico WHERE Codigo='RECHAZADA'),FechaRespuesta=SYSDATETIME() WHERE ReservaID=@ReservaID AND OfertaTecnicoID<>@OfertaTecnicoID AND EstadoOfertaID=@Pendiente;
    UPDATE dbo.SolicitudesReserva SET TecnicoID=@TecnicoID,EstadoReservaID=@EstadoAsignada,FechaModificacion=SYSDATETIME() WHERE ReservaID=@ReservaID;
    INSERT INTO dbo.HistorialEstadosReserva(ReservaID,EstadoAnteriorID,EstadoNuevoID,UsuarioModificadorID,Observaciones) VALUES(@ReservaID,@EstadoSolicitada,@EstadoAsignada,@TecnicoID,N'Oferta aceptada por el técnico.');
    COMMIT;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_OfertaTecnico_Crear]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_OfertaTecnico_Crear]
    @ReservaID BIGINT,@TecnicoID NVARCHAR(450),@DistanciaMetros DECIMAL(12,2)=NULL,@FechaExpiracion DATETIME2(0)=NULL,@Mensaje NVARCHAR(500)=NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; BEGIN TRANSACTION;
    DECLARE @Pendiente INT; SELECT @Pendiente=EstadoOfertaID FROM dbo.EstadosOfertaTecnico WHERE Codigo='PENDIENTE';
    IF NOT EXISTS (SELECT 1 FROM dbo.SolicitudesReserva WHERE ReservaID=@ReservaID AND EstadoReservaID IN (SELECT EstadoReservaID FROM dbo.EstadosReserva WHERE Codigo='SOLICITADA')) THROW 50017,'La reserva no esta disponible para ofertas.',1;
    IF NOT EXISTS (SELECT 1 FROM dbo.TecnicoEspecialidades te INNER JOIN dbo.SolicitudesReserva r ON r.ServicioID=te.ServicioID WHERE te.TecnicoID=@TecnicoID AND r.ReservaID=@ReservaID) THROW 50018,'El tecnico no tiene la especialidad requerida.',1;
    IF EXISTS (SELECT 1 FROM dbo.OfertasTecnico WHERE ReservaID=@ReservaID AND TecnicoID=@TecnicoID) THROW 50019,'Ya existe una oferta para este tecnico.',1;
    INSERT INTO dbo.OfertasTecnico(ReservaID,TecnicoID,EstadoOfertaID,DistanciaMetros,FechaExpiracion,Mensaje) VALUES(@ReservaID,@TecnicoID,@Pendiente,@DistanciaMetros,@FechaExpiracion,@Mensaje);
    DECLARE @OfertaID BIGINT=CONVERT(BIGINT,SCOPE_IDENTITY());
    INSERT INTO dbo.Notificaciones(UsuarioID,ReservaID,OfertaTecnicoID,TipoNotificacion,Titulo,Mensaje)
    VALUES(@TecnicoID,@ReservaID,@OfertaID,'NUEVA_OFERTA',N'Nueva solicitud de servicio',COALESCE(@Mensaje,N'Tienes una nueva solicitud de servicio disponible.'));
    COMMIT; SELECT @OfertaID AS OfertaTecnicoID;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_BuscarTecnicosDisponibles]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Reserva_BuscarTecnicosDisponibles]
    @ReservaID BIGINT,
    @RadioKm DECIMAL(8,2)=20,
    @MaxTecnicos INT=10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ServicioID INT,
            @FechaHora DATETIME2(0),
            @Geo geography,
            @Duracion INT,
            @DiaSemana TINYINT;

    SELECT
        @ServicioID=ServicioID,
        @FechaHora=FechaHoraProgramada,
        @Geo=UbicacionGeoServicio,
        @Duracion=DuracionEstimadaMinutos
    FROM dbo.SolicitudesReserva
    WHERE ReservaID=@ReservaID;

    IF @ServicioID IS NULL
        THROW 50016,'La reserva no existe.',1;

    IF @Geo IS NULL
        THROW 50045,'La reserva no tiene una ubicacion valida.',1;

    SET @DiaSemana=CAST((DATEDIFF(DAY,CONVERT(date,'19000101',112),CAST(@FechaHora AS date)) % 7)+1 AS TINYINT);

    SELECT TOP (@MaxTecnicos)
        t.TecnicoID,
        u.Nombre,
        u.Apellidos,
        t.CalificacionPromedio,
        CAST(tu.UbicacionGeo.STDistance(@Geo) AS DECIMAL(12,2)) AS DistanciaMetros
    FROM dbo.TecnicosUbicacionActual tu
    INNER JOIN dbo.TecnicosPerfil t
        ON t.TecnicoID=tu.TecnicoID
    INNER JOIN dbo.Usuarios u
        ON u.UsuarioID=t.TecnicoID
    INNER JOIN dbo.TecnicoEspecialidades te
        ON te.TecnicoID=t.TecnicoID
       AND te.ServicioID=@ServicioID
    WHERE tu.Activa=1
      AND t.EstadoVerificacion='Aprobado'
      AND t.Disponible=1
      AND tu.UbicacionGeo.STDistance(@Geo) <= (@RadioKm*1000)
      AND EXISTS
      (
          SELECT 1
          FROM dbo.DisponibilidadTecnico dt
          WHERE dt.TecnicoID=t.TecnicoID
            AND dt.DiaSemana=@DiaSemana
            AND dt.Activa=1
            AND CAST(@FechaHora AS time) >= dt.HoraInicio
            AND CAST(DATEADD(MINUTE,@Duracion,@FechaHora) AS time) <= dt.HoraFin
            AND CAST(DATEADD(MINUTE,@Duracion,@FechaHora) AS date)=CAST(@FechaHora AS date)
      )
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.SolicitudesReserva r
          INNER JOIN dbo.EstadosReserva er
              ON er.EstadoReservaID=r.EstadoReservaID
          WHERE r.TecnicoID=t.TecnicoID
            AND er.Codigo IN ('ASIGNADA','EN_CAMINO','EN_PROCESO')
            AND DATEADD(MINUTE,r.DuracionEstimadaMinutos,r.FechaHoraProgramada) > @FechaHora
            AND DATEADD(MINUTE,@Duracion,@FechaHora) > r.FechaHoraProgramada
      )
    ORDER BY tu.UbicacionGeo.STDistance(@Geo),
             t.CalificacionPromedio DESC;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_CalcularCotizacion]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================================
   13E. COTIZACION
   ============================================================================ */

CREATE   PROCEDURE [dbo].[usp_Reserva_CalcularCotizacion]
    @ServicioID INT,
    @OpcionIDs NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PrecioBase DECIMAL(12,2),
            @Moneda CHAR(3);

    SELECT
        @PrecioBase = TarifaDiagnosticoBase,
        @Moneda = Moneda
    FROM dbo.Servicios
    WHERE ServicioID=@ServicioID
      AND Activo=1;

    IF @PrecioBase IS NULL
        THROW 51001, 'El servicio no existe o esta inactivo.', 1;

    DECLARE @Ajustes TABLE
    (
        OpcionPreguntaID INT PRIMARY KEY,
        PreguntaServicioID INT NOT NULL,
        TextoOpcion NVARCHAR(300) NOT NULL,
        AjustePrecio DECIMAL(12,2) NOT NULL
    );

    IF NULLIF(LTRIM(RTRIM(@OpcionIDs)),N'') IS NOT NULL
    BEGIN
        INSERT INTO @Ajustes
            (OpcionPreguntaID,PreguntaServicioID,TextoOpcion,AjustePrecio)
        SELECT
            o.OpcionPreguntaID,
            o.PreguntaServicioID,
            o.TextoOpcion,
            o.AjustePrecio
        FROM dbo.OpcionesPregunta o
        INNER JOIN dbo.PreguntasServicio p
            ON p.PreguntaServicioID=o.PreguntaServicioID
        INNER JOIN STRING_SPLIT(@OpcionIDs,',') s
            ON TRY_CONVERT(INT,LTRIM(RTRIM(s.value)))=o.OpcionPreguntaID
        WHERE o.Activa=1
          AND p.Activa=1
          AND p.ServicioID=@ServicioID;
    END;

    /* Si se recibieron IDs, todos deben pertenecer al servicio solicitado. */
    IF NULLIF(LTRIM(RTRIM(@OpcionIDs)),N'') IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM STRING_SPLIT(@OpcionIDs,',') s
           WHERE TRY_CONVERT(INT,LTRIM(RTRIM(s.value))) IS NOT NULL
             AND NOT EXISTS
             (
                 SELECT 1
                 FROM @Ajustes a
                 WHERE a.OpcionPreguntaID=TRY_CONVERT(INT,LTRIM(RTRIM(s.value)))
             )
       )
        THROW 51002, 'Una o mas opciones no pertenecen al servicio, estan inactivas o no existen.', 1;

    DECLARE @MontoAjustes DECIMAL(12,2)=
        ISNULL((SELECT SUM(AjustePrecio) FROM @Ajustes),0);

    IF @PrecioBase + @MontoAjustes < 0
        THROW 51003, 'El precio calculado no puede ser negativo.', 1;

    SELECT
        @ServicioID AS ServicioID,
        @PrecioBase AS PrecioBase,
        @MontoAjustes AS MontoAjustes,
        @PrecioBase + @MontoAjustes AS PrecioTotal,
        @Moneda AS Moneda;

    SELECT
        OpcionPreguntaID,
        PreguntaServicioID,
        TextoOpcion,
        AjustePrecio
    FROM @Ajustes
    ORDER BY PreguntaServicioID,OpcionPreguntaID;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_CambiarEstado]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Reserva_CambiarEstado]
    @ReservaID BIGINT,
    @EstadoNuevoID INT,
    @UsuarioModificadorID NVARCHAR(450),
    @Observaciones NVARCHAR(500)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @EstadoAnteriorID INT,
            @CodigoAnterior VARCHAR(30),
            @CodigoNuevo VARCHAR(30);

    SELECT
        @EstadoAnteriorID = r.EstadoReservaID,
        @CodigoAnterior = ea.Codigo
    FROM dbo.SolicitudesReserva r WITH (UPDLOCK,ROWLOCK)
    INNER JOIN dbo.EstadosReserva ea ON ea.EstadoReservaID = r.EstadoReservaID
    WHERE r.ReservaID = @ReservaID;

    IF @EstadoAnteriorID IS NULL
        THROW 50004, 'La reserva no existe.', 1;

    SELECT @CodigoNuevo = Codigo
    FROM dbo.EstadosReserva
    WHERE EstadoReservaID = @EstadoNuevoID;

    IF @CodigoNuevo IS NULL
        THROW 50005, 'El estado nuevo no existe.', 1;

    IF @EstadoAnteriorID = @EstadoNuevoID
        THROW 50007, 'La reserva ya se encuentra en ese estado.', 1;

    /* Maquina de estados de CURLINGgo */
    IF NOT (
        (@CodigoAnterior = 'SOLICITADA' AND @CodigoNuevo IN ('ASIGNADA','CANCELADA'))
        OR (@CodigoAnterior = 'ASIGNADA' AND @CodigoNuevo IN ('EN_CAMINO','CANCELADA'))
        OR (@CodigoAnterior = 'EN_CAMINO' AND @CodigoNuevo IN ('EN_PROCESO','CANCELADA'))
        OR (@CodigoAnterior = 'EN_PROCESO' AND @CodigoNuevo IN ('COMPLETADA','CANCELADA'))
    )
        THROW 50008, 'Transicion de estado no permitida para la reserva.', 1;

    IF @CodigoNuevo = 'ASIGNADA'
       AND NOT EXISTS (SELECT 1 FROM dbo.SolicitudesReserva WHERE ReservaID=@ReservaID AND TecnicoID IS NOT NULL)
        THROW 50009, 'Una reserva debe tener tecnico asignado antes de pasar a ASIGNADA.', 1;

    UPDATE dbo.SolicitudesReserva
    SET EstadoReservaID=@EstadoNuevoID,
        FechaModificacion=SYSDATETIME(),
        FechaHoraCompletada=
            CASE WHEN @CodigoNuevo='COMPLETADA'
                 THEN COALESCE(FechaHoraCompletada,SYSDATETIME())
                 ELSE FechaHoraCompletada END
    WHERE ReservaID=@ReservaID;

    INSERT INTO dbo.HistorialEstadosReserva
        (ReservaID,EstadoAnteriorID,EstadoNuevoID,UsuarioModificadorID,Observaciones)
    VALUES
        (@ReservaID,@EstadoAnteriorID,@EstadoNuevoID,@UsuarioModificadorID,@Observaciones);

    COMMIT TRANSACTION;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_Crear]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Reserva_Crear]
    @ClienteID NVARCHAR(450),
    @ServicioID INT,
    @DireccionID BIGINT,
    @FechaHoraProgramada DATETIME2(0),
    @DescripcionProblema NVARCHAR(2000),
    @MontoBaseCotizado DECIMAL(12,2)=NULL,
    @NotasCliente NVARCHAR(1000)=NULL,
    @OpcionIDs NVARCHAR(MAX)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    DECLARE
        @EstadoSolicitada INT,
        @CategoriaID INT,
        @SubcategoriaID INT,
        @Duracion INT,
        @PrecioBase DECIMAL(12,2),
        @MontoAjustes DECIMAL(12,2)=0,
        @MontoTotal DECIMAL(12,2),
        @Moneda CHAR(3),
        @ProvinciaID INT,
        @CantonID INT,
        @DistritoID INT,
        @DireccionServicio NVARCHAR(300),
        @Lat DECIMAL(9,6),
        @Lon DECIMAL(9,6);

    SELECT @EstadoSolicitada=EstadoReservaID
    FROM dbo.EstadosReserva
    WHERE Codigo='SOLICITADA';

    IF @EstadoSolicitada IS NULL
        THROW 50048, 'No existe el estado SOLICITADA.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.ClientesPerfil WHERE ClienteID=@ClienteID)
        THROW 50002, 'El cliente no existe.', 1;

    SELECT
        @CategoriaID=CategoriaID,
        @SubcategoriaID=SubcategoriaID,
        @Duracion=TiempoEstimadoMinutos,
        @PrecioBase=TarifaDiagnosticoBase,
        @Moneda=Moneda
    FROM dbo.Servicios
    WHERE ServicioID=@ServicioID
      AND Activo=1;

    IF @CategoriaID IS NULL
        THROW 50003, 'El servicio no existe o esta inactivo.', 1;

    IF @SubcategoriaID IS NULL
       OR NOT EXISTS (
            SELECT 1 FROM dbo.SubcategoriasServicio
            WHERE SubcategoriaID=@SubcategoriaID
              AND CategoriaID=@CategoriaID
              AND Activa=1
       )
        THROW 50012, 'El servicio no tiene una subcategoria valida.', 1;

    SELECT
        @ProvinciaID=ProvinciaID,
        @CantonID=CantonID,
        @DistritoID=DistritoID,
        @DireccionServicio=DireccionExacta,
        @Lat=Latitud,
        @Lon=Longitud
    FROM dbo.DireccionesCliente
    WHERE DireccionID=@DireccionID
      AND ClienteID=@ClienteID
      AND Activa=1;

    IF @DireccionServicio IS NULL
        THROW 50013, 'La direccion seleccionada no existe, no pertenece al cliente o esta inactiva.', 1;

    IF @Lat IS NULL OR @Lon IS NULL
        THROW 50044, 'La direccion seleccionada debe tener coordenadas para calcular tecnicos cercanos.', 1;

    IF @FechaHoraProgramada <= SYSDATETIME()
        THROW 50014, 'La fecha y hora de la visita debe ser futura.', 1;

    IF NULLIF(LTRIM(RTRIM(@DescripcionProblema)),N'') IS NULL
        THROW 50015, 'Debe indicar una descripcion del problema.', 1;

    /* La tarifa base se toma siempre del catalogo. El cliente no puede alterarla. */
    IF @MontoBaseCotizado IS NOT NULL
       AND @MontoBaseCotizado <> @PrecioBase
        THROW 50049, 'La tarifa base enviada no coincide con la tarifa vigente del servicio.', 1;

    DECLARE @SelectedOptions TABLE
    (
        OpcionPreguntaID INT PRIMARY KEY,
        PreguntaServicioID INT NOT NULL,
        TipoRespuesta VARCHAR(20) NOT NULL,
        TextoOpcion NVARCHAR(300) NOT NULL,
        AjustePrecio DECIMAL(12,2) NOT NULL
    );

    IF NULLIF(LTRIM(RTRIM(@OpcionIDs)),N'') IS NOT NULL
    BEGIN
        INSERT INTO @SelectedOptions
            (OpcionPreguntaID,PreguntaServicioID,TipoRespuesta,TextoOpcion,AjustePrecio)
        SELECT
            o.OpcionPreguntaID,
            o.PreguntaServicioID,
            p.TipoRespuesta,
            o.TextoOpcion,
            o.AjustePrecio
        FROM dbo.OpcionesPregunta o
        INNER JOIN dbo.PreguntasServicio p
            ON p.PreguntaServicioID=o.PreguntaServicioID
        INNER JOIN STRING_SPLIT(@OpcionIDs,',') s
            ON TRY_CONVERT(INT,LTRIM(RTRIM(s.value)))=o.OpcionPreguntaID
        WHERE o.Activa=1
          AND p.Activa=1
          AND p.ServicioID=@ServicioID;
    END;

    /* Todas las opciones enviadas deben ser validas para este servicio. */
    IF NULLIF(LTRIM(RTRIM(@OpcionIDs)),N'') IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM STRING_SPLIT(@OpcionIDs,',') s
           WHERE TRY_CONVERT(INT,LTRIM(RTRIM(s.value))) IS NOT NULL
             AND NOT EXISTS
             (
                 SELECT 1
                 FROM @SelectedOptions x
                 WHERE x.OpcionPreguntaID=TRY_CONVERT(INT,LTRIM(RTRIM(s.value)))
             )
       )
        THROW 50050, 'Una o mas opciones seleccionadas no pertenecen al servicio o estan inactivas.', 1;

    /* Una pregunta de seleccion simple no puede recibir dos opciones. */
    IF EXISTS
    (
        SELECT PreguntaServicioID
        FROM @SelectedOptions
        WHERE TipoRespuesta <> 'MULTIPLE'
        GROUP BY PreguntaServicioID
        HAVING COUNT(*) > 1
    )
        THROW 50051, 'Una pregunta de seleccion simple tiene mas de una opcion seleccionada.', 1;

    SELECT @MontoAjustes=ISNULL(SUM(AjustePrecio),0)
    FROM @SelectedOptions;

    SET @MontoTotal=@PrecioBase+@MontoAjustes;

    IF @MontoTotal < 0
        THROW 50052, 'El precio final no puede ser negativo.', 1;

    INSERT INTO dbo.SolicitudesReserva
        (ClienteID,ServicioID,EstadoReservaID,DireccionID,ProvinciaID,CantonID,DistritoID,
         MontoBaseCotizado,MontoAjustes,MontoTotalCotizado,Moneda,
         DuracionEstimadaMinutos,FechaHoraProgramada,
         LatitudServicio,LongitudServicio,DireccionServicio,DescripcionProblema,NotasCliente)
    VALUES
        (@ClienteID,@ServicioID,@EstadoSolicitada,@DireccionID,@ProvinciaID,@CantonID,@DistritoID,
         @PrecioBase,@MontoAjustes,@MontoTotal,@Moneda,
         @Duracion,@FechaHoraProgramada,@Lat,@Lon,@DireccionServicio,@DescripcionProblema,@NotasCliente);

    DECLARE @ReservaID BIGINT=CONVERT(BIGINT,SCOPE_IDENTITY());
    DECLARE @Codigo UNIQUEIDENTIFIER=
        (SELECT CodigoSeguimiento FROM dbo.SolicitudesReserva WHERE ReservaID=@ReservaID);

    INSERT INTO dbo.HistorialEstadosReserva
        (ReservaID,EstadoAnteriorID,EstadoNuevoID,UsuarioModificadorID,Observaciones)
    VALUES
        (@ReservaID,NULL,@EstadoSolicitada,@ClienteID,N'Reserva creada por el cliente.');

    /* Guarda una respuesta por pregunta. Para MULTIPLE, las opciones se
       almacenan en RespuestasReservaOpciones. */
    DECLARE @MapeoRespuestas TABLE
    (
        RespuestaReservaID BIGINT NOT NULL,
        PreguntaServicioID INT NOT NULL
    );

    INSERT INTO dbo.RespuestasReserva
        (ReservaID,PreguntaServicioID,OpcionPreguntaID,RespuestaTexto,AjustePrecioAplicado)
    OUTPUT inserted.RespuestaReservaID, inserted.PreguntaServicioID
        INTO @MapeoRespuestas(RespuestaReservaID,PreguntaServicioID)
    SELECT
        @ReservaID,
        x.PreguntaServicioID,
        CASE WHEN x.TipoRespuesta='MULTIPLE' THEN NULL ELSE MIN(x.OpcionPreguntaID) END,
        CASE WHEN x.TipoRespuesta='MULTIPLE' THEN N'MULTIPLE' ELSE NULL END,
        SUM(x.AjustePrecio)
    FROM @SelectedOptions x
    GROUP BY x.PreguntaServicioID,x.TipoRespuesta;

    INSERT INTO dbo.RespuestasReservaOpciones
        (RespuestaReservaID,OpcionPreguntaID)
    SELECT m.RespuestaReservaID,x.OpcionPreguntaID
    FROM @MapeoRespuestas m
    INNER JOIN @SelectedOptions x
        ON x.PreguntaServicioID=m.PreguntaServicioID
    WHERE x.TipoRespuesta='MULTIPLE';

    /* Snapshot legible del calculo de precio. */
    INSERT INTO dbo.DetallePrecioReserva
        (ReservaID,Concepto,TipoConcepto,Monto,PreguntaServicioID,OpcionPreguntaID)
    VALUES
        (@ReservaID,N'Tarifa base del servicio','BASE',@PrecioBase,NULL,NULL);

    INSERT INTO dbo.DetallePrecioReserva
        (ReservaID,Concepto,TipoConcepto,Monto,PreguntaServicioID,OpcionPreguntaID)
    SELECT
        @ReservaID,
        x.TextoOpcion,
        CASE
            WHEN x.AjustePrecio < 0 THEN 'DESCUENTO'
            WHEN x.AjustePrecio > 0 THEN 'RECARGO'
            ELSE 'AJUSTE'
        END,
        ABS(x.AjustePrecio),
        x.PreguntaServicioID,
        x.OpcionPreguntaID
    FROM @SelectedOptions x
    WHERE x.AjustePrecio <> 0;

    COMMIT TRANSACTION;

    SELECT
        @ReservaID AS ReservaID,
        @Codigo AS CodigoSeguimiento,
        @PrecioBase AS PrecioBase,
        @MontoAjustes AS MontoAjustes,
        @MontoTotal AS MontoTotalCotizado,
        @Moneda AS Moneda;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_ObtenerDetallePrecio]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[usp_Reserva_ObtenerDetallePrecio]
    @ReservaID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.DetallePrecioID,
        d.ReservaID,
        d.Concepto,
        d.TipoConcepto,
        d.Monto,
        d.PreguntaServicioID,
        d.OpcionPreguntaID,
        d.FechaRegistro
    FROM dbo.DetallePrecioReserva d
    WHERE d.ReservaID=@ReservaID
    ORDER BY d.DetallePrecioID;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_ObtenerHorariosDisponibles]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Reserva_ObtenerHorariosDisponibles]
    @ServicioID INT,
    @DireccionID BIGINT,
    @Fecha DATE,
    @IntervaloMinutos INT=30,
    @RadioKm DECIMAL(8,2)=20
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Duracion INT,@Geo geography,@DiaSemana TINYINT;
    SELECT @Duracion=TiempoEstimadoMinutos FROM dbo.Servicios WHERE ServicioID=@ServicioID AND Activo=1;
    IF @Duracion IS NULL THROW 50042,'El servicio no existe o esta inactivo.',1;
    SELECT @Geo=UbicacionGeo FROM dbo.DireccionesCliente WHERE DireccionID=@DireccionID AND Activa=1;
    IF @Geo IS NULL THROW 50043,'La direccion no existe o no tiene coordenadas.',1;
    SET @DiaSemana=CAST((DATEDIFF(DAY,CONVERT(date,'19000101',112),@Fecha) % 7)+1 AS TINYINT);

    ;WITH Horas AS
    (
        SELECT CAST('06:00:00' AS time(0)) AS Hora
        UNION ALL
        SELECT CAST(DATEADD(MINUTE,@IntervaloMinutos,Hora) AS time(0))
        FROM Horas WHERE Hora < CAST('22:00:00' AS time(0))
    )
    SELECT h.Hora AS HoraInicio,
           CAST(DATEADD(MINUTE,@Duracion,DATEADD(SECOND,DATEDIFF(SECOND,CAST('00:00:00' AS time),h.Hora),CAST(@Fecha AS datetime2))) AS time(0)) AS HoraFin
    FROM Horas h
    WHERE EXISTS
    (
        SELECT 1
        FROM dbo.TecnicosPerfil t
        INNER JOIN dbo.TecnicoEspecialidades te ON te.TecnicoID=t.TecnicoID AND te.ServicioID=@ServicioID
        INNER JOIN dbo.DisponibilidadTecnico dt ON dt.TecnicoID=t.TecnicoID AND dt.DiaSemana=@DiaSemana AND dt.Activa=1
        WHERE t.EstadoVerificacion='Aprobado' AND t.Disponible=1 AND t.UbicacionGeoActual IS NOT NULL
          AND t.UbicacionGeoActual.STDistance(@Geo) <= (@RadioKm*1000)
          AND h.Hora >= dt.HoraInicio
          AND CAST(DATEADD(MINUTE,@Duracion,DATEADD(SECOND,DATEDIFF(SECOND,CAST('00:00:00' AS time),h.Hora),CAST(@Fecha AS datetime2))) AS time(0)) <= dt.HoraFin
          AND NOT EXISTS
          (
              SELECT 1 FROM dbo.SolicitudesReserva r
              INNER JOIN dbo.EstadosReserva er ON er.EstadoReservaID=r.EstadoReservaID
              WHERE r.TecnicoID=t.TecnicoID AND er.Codigo IN ('ASIGNADA','EN_CAMINO','EN_PROCESO')
                AND DATEADD(MINUTE,r.DuracionEstimadaMinutos,r.FechaHoraProgramada) > DATEADD(SECOND,DATEDIFF(SECOND,CAST('00:00:00' AS time),h.Hora),CAST(@Fecha AS datetime2))
                AND DATEADD(MINUTE,@Duracion,DATEADD(SECOND,DATEDIFF(SECOND,CAST('00:00:00' AS time),h.Hora),CAST(@Fecha AS datetime2))) > r.FechaHoraProgramada
          )
    )
    ORDER BY h.Hora
    OPTION (MAXRECURSION 100);
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Reserva_ObtenerPorCodigo]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Reserva_ObtenerPorCodigo]
    @CodigoSeguimiento UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.ReservaID,
        r.CodigoSeguimiento,
        r.ClienteID,
        r.TecnicoID,
        r.ServicioID,
        c.CategoriaID,
        c.NombreCategoria,
        sc.SubcategoriaID,
        sc.NombreSubcategoria,
        s.NombreServicio,
        r.DireccionID,
        r.ProvinciaID,
        r.CantonID,
        r.DistritoID,
        r.LatitudServicio,
        r.LongitudServicio,
        r.DescripcionProblema,
        r.DuracionEstimadaMinutos,
        er.Codigo AS EstadoCodigo,
        er.Nombre AS EstadoNombre,
        r.MontoBaseCotizado,
        r.MontoAjustes,
        r.MontoTotalCotizado,
        r.Moneda,
        r.FechaHoraProgramada,
        r.FechaHoraSolicitud,
        r.FechaHoraCompletada,
        r.DireccionServicio,
        r.NotasCliente
    FROM dbo.SolicitudesReserva r
    INNER JOIN dbo.Servicios s ON s.ServicioID=r.ServicioID
    INNER JOIN dbo.CategoriasServicio c ON c.CategoriaID=s.CategoriaID
    LEFT JOIN dbo.SubcategoriasServicio sc ON sc.SubcategoriaID=s.SubcategoriaID
    INNER JOIN dbo.EstadosReserva er ON er.EstadoReservaID=r.EstadoReservaID
    WHERE r.CodigoSeguimiento=@CodigoSeguimiento;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Servicio_Crear]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Servicio_Crear]
    @CategoriaID INT,
    @SubcategoriaID INT,
    @NombreServicio NVARCHAR(150),
    @Descripcion NVARCHAR(500)=NULL,
    @TarifaDiagnosticoBase DECIMAL(12,2),
    @TiempoEstimadoMinutos INT=60,
    @Moneda CHAR(3)='CRC'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.CategoriasServicio WHERE CategoriaID=@CategoriaID AND Activa=1)
        THROW 50001, 'La categoria no existe o esta inactiva.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.SubcategoriasServicio
        WHERE SubcategoriaID=@SubcategoriaID
          AND CategoriaID=@CategoriaID
          AND Activa=1
    )
        THROW 50011, 'La subcategoria no existe, esta inactiva o no pertenece a la categoria seleccionada.', 1;

    IF @TarifaDiagnosticoBase < 0
        THROW 50046, 'La tarifa base no puede ser negativa.', 1;

    IF @Moneda NOT IN ('CRC','USD')
        THROW 50047, 'La moneda debe ser CRC o USD.', 1;

    INSERT INTO dbo.Servicios
        (CategoriaID,SubcategoriaID,NombreServicio,Descripcion,
         TarifaDiagnosticoBase,TiempoEstimadoMinutos,Moneda)
    VALUES
        (@CategoriaID,@SubcategoriaID,@NombreServicio,@Descripcion,
         @TarifaDiagnosticoBase,@TiempoEstimadoMinutos,@Moneda);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ServicioID;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Servicio_ListarActivos]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Servicio_ListarActivos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.ServicioID,
        c.CategoriaID,
        c.NombreCategoria,
        sc.SubcategoriaID,
        sc.NombreSubcategoria,
        s.NombreServicio,
        s.Descripcion,
        s.TarifaDiagnosticoBase,
        s.Moneda,
        s.TiempoEstimadoMinutos
    FROM dbo.Servicios s
    INNER JOIN dbo.CategoriasServicio c ON c.CategoriaID=s.CategoriaID
    LEFT JOIN dbo.SubcategoriasServicio sc ON sc.SubcategoriaID=s.SubcategoriaID
    WHERE s.Activo=1
      AND c.Activa=1
      AND (sc.SubcategoriaID IS NULL OR sc.Activa=1)
    ORDER BY c.NombreCategoria, sc.NombreSubcategoria, s.NombreServicio;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_Tecnico_ActualizarUbicacion]    Script Date: 19/08/2026 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[usp_Tecnico_ActualizarUbicacion]
    @TecnicoID NVARCHAR(450),
    @Latitud DECIMAL(9,6),
    @Longitud DECIMAL(9,6)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.TecnicosPerfil WHERE TecnicoID=@TecnicoID)
        THROW 52001,'El tecnico no existe.',1;

    IF @Latitud NOT BETWEEN -90 AND 90
        THROW 52002,'La latitud no es valida.',1;

    IF @Longitud NOT BETWEEN -180 AND 180
        THROW 52003,'La longitud no es valida.',1;

    BEGIN TRANSACTION;

    UPDATE dbo.TecnicosPerfil
    SET LatitudActual=@Latitud,
        LongitudActual=@Longitud
    WHERE TecnicoID=@TecnicoID;

    IF EXISTS (SELECT 1 FROM dbo.TecnicosUbicacionActual WHERE TecnicoID=@TecnicoID)
        UPDATE dbo.TecnicosUbicacionActual
        SET Latitud=@Latitud,
            Longitud=@Longitud,
            FechaActualizacion=SYSDATETIME(),
            Activa=1
        WHERE TecnicoID=@TecnicoID;
    ELSE
        INSERT INTO dbo.TecnicosUbicacionActual
            (TecnicoID,Latitud,Longitud,FechaActualizacion,Activa)
        VALUES
            (@TecnicoID,@Latitud,@Longitud,SYSDATETIME(),1);

    COMMIT;

    SELECT
        TecnicoID,
        Latitud,
        Longitud,
        FechaActualizacion
    FROM dbo.TecnicosUbicacionActual
    WHERE TecnicoID=@TecnicoID;
END

GO
