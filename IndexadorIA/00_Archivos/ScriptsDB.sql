-- Script de creación de tablas para IndexadorIA
-- Base de Datos: IndexadorIA
-- Servidor: localhost\SQLEXPRESS

USE IndexadorIA
GO

-- ============================================
-- ETAPA 1: SEGURIDAD Y AUTENTICACIÓN
-- ============================================

-- ============================================
-- Tabla: TD_ESTADOS
-- Descripción: Almacena todos los estados utilizados en la solución
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_ESTADOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_ESTADOS(
		idEstado INT IDENTITY(1,1) PRIMARY KEY,
		dsProceso VARCHAR(50) NOT NULL,
		cdEstado INT NOT NULL,
		dsEstado VARCHAR(50) NOT NULL
	)
END
GO

-- ============================================
-- Tabla: TD_USUARIOS
-- Descripción: Almacena los usuarios del sistema
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_USUARIOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_USUARIOS(
		cdUsuario INT IDENTITY(1,1) PRIMARY KEY,
		dsUsuario VARCHAR(50) NOT NULL UNIQUE,
		dsClave VARCHAR(255) NOT NULL,
		dsNombreCompleto VARCHAR(100) NOT NULL,
		snClaveTemporal BIT NOT NULL DEFAULT 1,
		snPrimerIngreso BIT NOT NULL DEFAULT 1,
		cdEstado INT NOT NULL,
		feAlta DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioAlta INT NULL
	)
END
GO

-- ============================================
-- ETAPA 2: INGRESO Y PROCESAMIENTO DE ARCHIVOS
-- ============================================

-- ============================================
-- Tabla: TD_PROYECTOS
-- Descripción: Proyectos de indexación con configuración
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_PROYECTOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_PROYECTOS(
		cdProyecto INT IDENTITY(1,1) PRIMARY KEY,
		dsProyecto VARCHAR(100) NOT NULL,
		dsDescripcion VARCHAR(500) NULL,
		dsRutaOrigen VARCHAR(500) NOT NULL,
		dsRutaTrabajo VARCHAR(500) NOT NULL,
		nuTamanoLote INT NOT NULL DEFAULT 50,
		nuCoordenadaX INT NULL,
		nuCoordenadaY INT NULL,
		nuAncho INT NULL,
		nuAlto INT NULL,
		cdEstado INT NOT NULL,
		feCreacion DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioCreacion INT NOT NULL,
		feModificacion DATETIME NULL,
		cdUsuarioModificacion INT NULL
	)
END
GO

-- ============================================
-- Tabla: TD_ARCHIVOS
-- Descripción: Archivos originales (PDF o JPG)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_ARCHIVOS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_ARCHIVOS(
		cdArchivo INT IDENTITY(1,1) PRIMARY KEY,
		cdProyecto INT NOT NULL,
		dsNombreOriginal VARCHAR(255) NOT NULL,
		dsRutaCompleta VARCHAR(500) NOT NULL,
		dsTipoArchivo VARCHAR(10) NOT NULL, -- PDF, JPG, JPEG
		nuTamanoBytes BIGINT NOT NULL,
		nuPaginasTotal INT NOT NULL DEFAULT 1,
		cdEstado INT NOT NULL,
		feIngreso DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioIngreso INT NOT NULL,
		feProcesamiento DATETIME NULL,
		dsObservaciones VARCHAR(500) NULL,
		FOREIGN KEY (cdProyecto) REFERENCES TD_PROYECTOS(cdProyecto)
	)
END
GO

-- ============================================
-- Tabla: TD_PAGINAS
-- Descripción: Páginas extraídas y procesadas
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_PAGINAS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_PAGINAS(
		cdPagina INT IDENTITY(1,1) PRIMARY KEY,
		cdArchivo INT NOT NULL,
		nuNumeroPagina INT NOT NULL,
		dsRutaImagenOriginal VARCHAR(500) NOT NULL,
		dsRutaImagenRotada VARCHAR(500) NULL,
		dsRutaImagenRecortada VARCHAR(500) NULL,
		nuGradosRotacion INT NOT NULL DEFAULT 0,
		snRotada BIT NOT NULL DEFAULT 0,
		snRecortada BIT NOT NULL DEFAULT 0,
		nuAnchoOriginal INT NULL,
		nuAltoOriginal INT NULL,
		cdEstado INT NOT NULL,
		feCreacion DATETIME NOT NULL DEFAULT GETDATE(),
		feProcesamiento DATETIME NULL,
		dsObservaciones VARCHAR(500) NULL,
		FOREIGN KEY (cdArchivo) REFERENCES TD_ARCHIVOS(cdArchivo)
	)
END
GO

-- ============================================
-- Tabla: TD_LOTES
-- Descripción: Lotes para procesamiento por IA
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTES]') AND type in (N'U'))
BEGIN
	CREATE TABLE TD_LOTES(
		cdLote INT IDENTITY(1,1) PRIMARY KEY,
		cdProyecto INT NOT NULL,
		dsNombreLote VARCHAR(100) NOT NULL,
		nuCantidadPaginas INT NOT NULL DEFAULT 0,
		cdEstado INT NOT NULL,
		feCreacion DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioCreacion INT NOT NULL,
		feEnvioBatch DATETIME NULL,
		feFinalizacion DATETIME NULL,
		dsBatchId VARCHAR(100) NULL,
		dsResultado VARCHAR(MAX) NULL,
		FOREIGN KEY (cdProyecto) REFERENCES TD_PROYECTOS(cdProyecto)
	)
END
GO

-- ============================================
-- Tabla: TR_LOTE_PAGINAS
-- Descripción: Relación entre lotes y páginas
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TR_LOTE_PAGINAS]') AND type in (N'U'))
BEGIN
	CREATE TABLE TR_LOTE_PAGINAS(
		cdLote INT NOT NULL,
		cdPagina INT NOT NULL,
		nuOrden INT NOT NULL,
		feAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
		PRIMARY KEY (cdLote, cdPagina),
		FOREIGN KEY (cdLote) REFERENCES TD_LOTES(cdLote),
		FOREIGN KEY (cdPagina) REFERENCES TD_PAGINAS(cdPagina)
	)
END
GO

-- ============================================
-- Insertar Estados para ETAPA 1
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'USUARIOS' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('USUARIOS', 1, 'Activo')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'USUARIOS' AND cdEstado = 0)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('USUARIOS', 0, 'Inactivo')
END
GO

-- ============================================
-- Insertar Estados para ETAPA 2
-- ============================================

-- Estados para PROYECTOS
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PROYECTOS' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PROYECTOS', 1, 'Activo')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PROYECTOS' AND cdEstado = 0)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PROYECTOS', 0, 'Inactivo')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PROYECTOS' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PROYECTOS', 2, 'Completado')
END
GO

-- Estados para ARCHIVOS
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVOS' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ARCHIVOS', 1, 'Pendiente')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVOS' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ARCHIVOS', 2, 'Procesando')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVOS' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ARCHIVOS', 3, 'Completado')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVOS' AND cdEstado = 9)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('ARCHIVOS', 9, 'Error')
END
GO

-- Estados para PAGINAS
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 1, 'Pendiente')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 2, 'Rotada')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 3, 'Recortada')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 4)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 4, 'En Lote')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 5)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 5, 'Procesada')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'PAGINAS' AND cdEstado = 9)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('PAGINAS', 9, 'Error')
END
GO

-- Estados para LOTES
IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTES' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('LOTES', 1, 'Pendiente')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTES' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('LOTES', 2, 'Enviado')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTES' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('LOTES', 3, 'Procesando')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTES' AND cdEstado = 4)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('LOTES', 4, 'Completado')
END
GO

IF NOT EXISTS (SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTES' AND cdEstado = 9)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado) VALUES ('LOTES', 9, 'Error')
END
GO

-- ============================================
-- Insertar Usuario Administrador Inicial
-- Usuario: admin
-- Clave: 123 (SHA256: a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3)
-- ============================================
IF NOT EXISTS (SELECT * FROM TD_USUARIOS WHERE dsUsuario = 'admin')
BEGIN
	INSERT INTO TD_USUARIOS (dsUsuario, dsClave, dsNombreCompleto, snClaveTemporal, snPrimerIngreso, cdEstado, feAlta, cdUsuarioAlta)
	VALUES ('admin', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Administrador del Sistema', 0, 0, 1, GETDATE(), NULL)
END
GO

-- ============================================
-- Stored Procedures
-- ============================================

-- SP para validar login
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_VALIDAR_LOGIN]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[SP_VALIDAR_LOGIN]
GO

CREATE PROCEDURE SP_VALIDAR_LOGIN
	@dsUsuario VARCHAR(50),
	@dsClave VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		cdUsuario,
		dsUsuario,
		dsNombreCompleto,
		snClaveTemporal,
		snPrimerIngreso,
		cdEstado
	FROM TD_USUARIOS
	WHERE dsUsuario = @dsUsuario 
		AND dsClave = @dsClave
		AND cdEstado = 1
END
GO
