-- ========================================
-- Script Etapa 3: Separación de Páginas
-- ========================================
-- Este script crea las tablas necesarias para la separación de páginas
-- de archivos PDF/JPG y el registro de rotaciones aplicadas.

USE IndexadorIA;
GO

-- ========================================
-- 1. Crear estado para ARCHIVO_PAGINA
-- ========================================
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('ARCHIVO_PAGINA', 1, 'Pendiente asignar Lote');
	PRINT 'Estado ARCHIVO_PAGINA creado exitosamente';
END
ELSE
BEGIN
	PRINT 'Estado ARCHIVO_PAGINA ya existe';
END
GO

-- ========================================
-- 2. Crear tabla TD_ARCHIVOS_PAGINAS
-- ========================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_ARCHIVOS_PAGINAS')
BEGIN
	CREATE TABLE TD_ARCHIVOS_PAGINAS
	(
		cdArchivoPagina         INT IDENTITY(1,1) PRIMARY KEY,
		cdArchivoOriginal       INT NOT NULL,
		nuPagina                INT NOT NULL,
		dsNombreArchivoPagina   VARCHAR(50) NOT NULL,
		dsRutaCompleta          VARCHAR(500) NOT NULL,
		snGirada                CHAR(2) NOT NULL DEFAULT 'NO',
		snPosibleBlanca         CHAR(2) NOT NULL DEFAULT 'NO',
		dsProceso               VARCHAR(50) NOT NULL DEFAULT 'ARCHIVO_PAGINA',
		cdEstado                INT NOT NULL,
		feAlta                  DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioAlta           INT NOT NULL,

		CONSTRAINT FK_ARCHIVOS_PAGINAS_ORIGINAL 
			FOREIGN KEY (cdArchivoOriginal) 
			REFERENCES TD_ARCHIVOS_ORIGINAL(cdArchivo),

		CONSTRAINT FK_ARCHIVOS_PAGINAS_ESTADO 
			FOREIGN KEY (dsProceso, cdEstado) 
			REFERENCES TD_ESTADOS(dsProceso, cdEstado),

		CONSTRAINT FK_ARCHIVOS_PAGINAS_USUARIO 
			FOREIGN KEY (cdUsuarioAlta) 
			REFERENCES TD_USUARIOS(cdUsuario),

		CONSTRAINT CHK_GIRADA CHECK (snGirada IN ('SI', 'NO')),
		CONSTRAINT CHK_POSIBLE_BLANCA CHECK (snPosibleBlanca IN ('SI', 'NO'))
	);

	-- Índice para búsquedas por archivo original
	CREATE INDEX IX_ARCHIVOS_PAGINAS_ORIGINAL ON TD_ARCHIVOS_PAGINAS(cdArchivoOriginal);

	-- Índice para búsquedas por estado
	CREATE INDEX IX_ARCHIVOS_PAGINAS_ESTADO ON TD_ARCHIVOS_PAGINAS(dsProceso, cdEstado);

	PRINT 'Tabla TD_ARCHIVOS_PAGINAS creada exitosamente';
END
ELSE
BEGIN
	PRINT 'Tabla TD_ARCHIVOS_PAGINAS ya existe';
END
GO

-- ========================================
-- 3. Crear tabla TD_ROTACIONES
-- ========================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_ROTACIONES')
BEGIN
	CREATE TABLE TD_ROTACIONES
	(
		cdRotacion              INT IDENTITY(1,1) PRIMARY KEY,
		cdArchivoPagina         INT NOT NULL,
		snRotacionAutomatica    CHAR(2) NOT NULL DEFAULT 'NO',
		nuRotacionAplicada      INT NOT NULL DEFAULT 0,
		snRotacionManual        CHAR(2) NOT NULL DEFAULT 'NO',
		feRotacion              DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuario               INT NOT NULL,

		CONSTRAINT FK_ROTACIONES_PAGINA 
			FOREIGN KEY (cdArchivoPagina) 
			REFERENCES TD_ARCHIVOS_PAGINAS(cdArchivoPagina),

		CONSTRAINT FK_ROTACIONES_USUARIO 
			FOREIGN KEY (cdUsuario) 
			REFERENCES TD_USUARIOS(cdUsuario),

		CONSTRAINT CHK_ROTACION_AUTOMATICA CHECK (snRotacionAutomatica IN ('SI', 'NO')),
		CONSTRAINT CHK_ROTACION_MANUAL CHECK (snRotacionManual IN ('SI', 'NO')),
		CONSTRAINT CHK_GRADOS_ROTACION CHECK (nuRotacionAplicada IN (0, 90, 180, 270))
	);

	-- Índice para búsquedas por archivo de página
	CREATE INDEX IX_ROTACIONES_PAGINA ON TD_ROTACIONES(cdArchivoPagina);

	PRINT 'Tabla TD_ROTACIONES creada exitosamente';
END
ELSE
BEGIN
	PRINT 'Tabla TD_ROTACIONES ya existe';
END
GO

-- ========================================
-- 4. Crear tabla de secuencia global para nombres de archivo
-- ========================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_SECUENCIA_ARCHIVOS')
BEGIN
	CREATE TABLE TD_SECUENCIA_ARCHIVOS
	(
		cdSecuencia             INT IDENTITY(1,1) PRIMARY KEY,
		nuSecuenciaActual       INT NOT NULL DEFAULT 0,
		feUltimaActualizacion   DATETIME NOT NULL DEFAULT GETDATE()
	);

	-- Insertar registro inicial
	INSERT INTO TD_SECUENCIA_ARCHIVOS (nuSecuenciaActual) VALUES (0);

	PRINT 'Tabla TD_SECUENCIA_ARCHIVOS creada exitosamente';
END
ELSE
BEGIN
	PRINT 'Tabla TD_SECUENCIA_ARCHIVOS ya existe';
END
GO

-- ========================================
-- 5. Crear stored procedure para obtener siguiente secuencia
-- ========================================
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'SP_OBTENER_SIGUIENTE_SECUENCIA')
BEGIN
	DROP PROCEDURE SP_OBTENER_SIGUIENTE_SECUENCIA;
END
GO

CREATE PROCEDURE SP_OBTENER_SIGUIENTE_SECUENCIA
	@cantidadSolicitada INT = 1,
	@secuenciaInicial INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Actualizar y obtener la secuencia de forma atómica
	UPDATE TD_SECUENCIA_ARCHIVOS
	SET @secuenciaInicial = nuSecuenciaActual + 1,
		nuSecuenciaActual = nuSecuenciaActual + @cantidadSolicitada,
		feUltimaActualizacion = GETDATE()
	WHERE cdSecuencia = 1;

	RETURN @secuenciaInicial;
END
GO

PRINT '========================================';
PRINT 'Script ETAPA3 ejecutado exitosamente';
PRINT '========================================';
GO
