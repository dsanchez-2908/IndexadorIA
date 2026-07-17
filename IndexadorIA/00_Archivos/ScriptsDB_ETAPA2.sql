/*
==============================================================================
SCRIPT: ScriptsDB_ETAPA2.sql
DESCRIPCIÓN: Script de creación de tablas y datos para Etapa 2
			 - TD_PROYECTOS
			 - TD_PARAMETROS
			 - TD_ARCHIVOS_ORIGINAL
			 - Estados para ARCHIVO_ORIGINAL
FECHA: 2026-07-11
==============================================================================
*/

USE IndexadorIA;
GO

PRINT '============================================================';
PRINT 'INICIANDO SCRIPT ETAPA 2';
PRINT '============================================================';

-- ============================================================
-- 1. CREAR TABLA TD_PROYECTOS
-- ============================================================
PRINT 'Verificando tabla TD_PROYECTOS...';

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_PROYECTOS]') AND type in (N'U'))
BEGIN
	PRINT 'Creando tabla TD_PROYECTOS...';

	CREATE TABLE [dbo].[TD_PROYECTOS](
		[cdProyecto] [int] IDENTITY(1,1) NOT NULL,
		[dsProyecto] [nvarchar](255) NOT NULL,
		[snActivo] [bit] NOT NULL DEFAULT 1,
		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),
		[cdUsuarioAlta] [int] NOT NULL,
		[feUltimaModificacion] [datetime] NULL,
		[cdUsuarioModificacion] [int] NULL,
		CONSTRAINT [PK_TD_PROYECTOS] PRIMARY KEY CLUSTERED ([cdProyecto] ASC)
	);

	PRINT 'Tabla TD_PROYECTOS creada exitosamente.';
END
ELSE
BEGIN
	PRINT 'Tabla TD_PROYECTOS ya existe.';
END
GO

-- ============================================================
-- 2. CREAR TABLA TD_PARAMETROS
-- ============================================================
PRINT 'Verificando tabla TD_PARAMETROS...';

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_PARAMETROS]') AND type in (N'U'))
BEGIN
	PRINT 'Creando tabla TD_PARAMETROS...';

	CREATE TABLE [dbo].[TD_PARAMETROS](
		[cdParametro] [int] IDENTITY(1,1) NOT NULL,
		[dsClaveParametro] [nvarchar](100) NOT NULL,
		[dsValorParametro] [nvarchar](max) NULL,
		[dsDescripcion] [nvarchar](500) NULL,
		[feUltimaModificacion] [datetime] NOT NULL DEFAULT GETDATE(),
		[cdUsuarioModificacion] [int] NULL,
		CONSTRAINT [PK_TD_PARAMETROS] PRIMARY KEY CLUSTERED ([cdParametro] ASC),
		CONSTRAINT [UQ_TD_PARAMETROS_CLAVE] UNIQUE ([dsClaveParametro])
	);

	PRINT 'Tabla TD_PARAMETROS creada exitosamente.';
END
ELSE
BEGIN
	PRINT 'Tabla TD_PARAMETROS ya existe.';
END
GO

-- ============================================================
-- 3. CREAR TABLA TD_ARCHIVOS_ORIGINAL
-- ============================================================
PRINT 'Verificando tabla TD_ARCHIVOS_ORIGINAL...';

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_ARCHIVOS_ORIGINAL]') AND type in (N'U'))
BEGIN
	PRINT 'Creando tabla TD_ARCHIVOS_ORIGINAL...';

	CREATE TABLE [dbo].[TD_ARCHIVOS_ORIGINAL](
		[cdArchivo] [int] IDENTITY(1,1) NOT NULL,
		[cdProyecto] [int] NOT NULL,
		[dsNombreArchivo] [nvarchar](255) NOT NULL,
		[dsExtension] [nvarchar](255) NOT NULL,
		[dsRutaCompleta] [nvarchar](1000) NOT NULL,
		[dsNombreUltimaCarpeta] [nvarchar](255) NULL,
		[nuCantidadPaginas] [int] NOT NULL,
		[cdEstadoArchivo] [int] NOT NULL,
		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),
		[cdUsuarioAlta] [int] NOT NULL,
		[feUltimaModificacion] [datetime] NULL,
		[cdUsuarioModificacion] [int] NULL,
		[nuTamanoBytes] [bigint] NOT NULL,
		[feModificacionArchivo] [datetime] NOT NULL,
		CONSTRAINT [PK_TD_ARCHIVOS_ORIGINAL] PRIMARY KEY CLUSTERED ([cdArchivo] ASC)
	);

	PRINT 'Tabla TD_ARCHIVOS_ORIGINAL creada exitosamente.';
END
ELSE
BEGIN
	PRINT 'Tabla TD_ARCHIVOS_ORIGINAL ya existe.';
END
GO

-- ============================================================
-- 4. AGREGAR ESTADOS PARA ARCHIVO_ORIGINAL
-- ============================================================
PRINT 'Agregando estados para ARCHIVO_ORIGINAL...';

-- Verificar si ya existe el proceso ARCHIVO_ORIGINAL
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_ORIGINAL')
BEGIN
	PRINT 'Insertando estados para proceso ARCHIVO_ORIGINAL...';

	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES 
		(N'ARCHIVO_ORIGINAL', 1, N'Pendiente de Procesar');

	PRINT 'Estados para ARCHIVO_ORIGINAL insertados.';
END
ELSE
BEGIN
	PRINT 'Estados para ARCHIVO_ORIGINAL ya existen.';
END
GO

-- ============================================================
-- 5. CREAR FOREIGN KEYS
-- ============================================================
PRINT 'Creando relaciones de integridad...';

-- FK TD_PROYECTOS -> TD_USUARIOS (usuario alta)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PROYECTOS_USUARIO_ALTA')
BEGIN
	ALTER TABLE [dbo].[TD_PROYECTOS]
	ADD CONSTRAINT [FK_PROYECTOS_USUARIO_ALTA] 
	FOREIGN KEY([cdUsuarioAlta]) REFERENCES [dbo].[TD_USUARIOS]([cdUsuario]);

	PRINT 'FK_PROYECTOS_USUARIO_ALTA creada.';
END

-- FK TD_PROYECTOS -> TD_USUARIOS (usuario modificación)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PROYECTOS_USUARIO_MODIFICACION')
BEGIN
	ALTER TABLE [dbo].[TD_PROYECTOS]
	ADD CONSTRAINT [FK_PROYECTOS_USUARIO_MODIFICACION] 
	FOREIGN KEY([cdUsuarioModificacion]) REFERENCES [dbo].[TD_USUARIOS]([cdUsuario]);

	PRINT 'FK_PROYECTOS_USUARIO_MODIFICACION creada.';
END

-- FK TD_PARAMETROS -> TD_USUARIOS (usuario modificación)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PARAMETROS_USUARIO_MODIFICACION')
BEGIN
	ALTER TABLE [dbo].[TD_PARAMETROS]
	ADD CONSTRAINT [FK_PARAMETROS_USUARIO_MODIFICACION] 
	FOREIGN KEY([cdUsuarioModificacion]) REFERENCES [dbo].[TD_USUARIOS]([cdUsuario]);

	PRINT 'FK_PARAMETROS_USUARIO_MODIFICACION creada.';
END

-- FK TD_ARCHIVOS_ORIGINAL -> TD_PROYECTOS
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_PROYECTO')
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_ORIGINAL]
	ADD CONSTRAINT [FK_ARCHIVOS_ORIGINAL_PROYECTO] 
	FOREIGN KEY([cdProyecto]) REFERENCES [dbo].[TD_PROYECTOS]([cdProyecto]);

	PRINT 'FK_ARCHIVOS_ORIGINAL_PROYECTO creada.';
END

-- FK TD_ARCHIVOS_ORIGINAL -> TD_ESTADOS
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_ESTADO')
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_ORIGINAL]
	ADD CONSTRAINT [FK_ARCHIVOS_ORIGINAL_ESTADO] 
	FOREIGN KEY([cdEstadoArchivo]) REFERENCES [dbo].[TD_ESTADOS]([idEstado]);

	PRINT 'FK_ARCHIVOS_ORIGINAL_ESTADO creada.';
END

-- FK TD_ARCHIVOS_ORIGINAL -> TD_USUARIOS (usuario alta)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_USUARIO_ALTA')
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_ORIGINAL]
	ADD CONSTRAINT [FK_ARCHIVOS_ORIGINAL_USUARIO_ALTA] 
	FOREIGN KEY([cdUsuarioAlta]) REFERENCES [dbo].[TD_USUARIOS]([cdUsuario]);

	PRINT 'FK_ARCHIVOS_ORIGINAL_USUARIO_ALTA creada.';
END

-- FK TD_ARCHIVOS_ORIGINAL -> TD_USUARIOS (usuario modificación)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ARCHIVOS_ORIGINAL_USUARIO_MODIFICACION')
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_ORIGINAL]
	ADD CONSTRAINT [FK_ARCHIVOS_ORIGINAL_USUARIO_MODIFICACION] 
	FOREIGN KEY([cdUsuarioModificacion]) REFERENCES [dbo].[TD_USUARIOS]([cdUsuario]);

	PRINT 'FK_ARCHIVOS_ORIGINAL_USUARIO_MODIFICACION creada.';
END

GO

-- ============================================================
-- 6. VERIFICACIÓN FINAL
-- ============================================================
PRINT '';
PRINT '============================================================';
PRINT 'VERIFICACIÓN DE ESTRUCTURA ETAPA 2';
PRINT '============================================================';

DECLARE @conteoProyectos INT, @conteoParametros INT, @conteoArchivos INT, @conteoEstados INT;

SELECT @conteoProyectos = COUNT(*) FROM TD_PROYECTOS;
SELECT @conteoParametros = COUNT(*) FROM TD_PARAMETROS;
SELECT @conteoArchivos = COUNT(*) FROM TD_ARCHIVOS_ORIGINAL;
SELECT @conteoEstados = COUNT(*) FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_ORIGINAL';

PRINT 'Conteo de proyectos: ' + CAST(@conteoProyectos AS VARCHAR);
PRINT 'Conteo de parámetros: ' + CAST(@conteoParametros AS VARCHAR);
PRINT 'Conteo de archivos originales: ' + CAST(@conteoArchivos AS VARCHAR);
PRINT 'Estados ARCHIVO_ORIGINAL: ' + CAST(@conteoEstados AS VARCHAR);

PRINT '';
PRINT '============================================================';
PRINT 'SCRIPT ETAPA 2 COMPLETADO EXITOSAMENTE';
PRINT '============================================================';

GO
