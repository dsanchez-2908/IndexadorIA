-- ========================================
-- Script de Corrección Etapa 3
-- ========================================
-- Correcciones:
-- 1. Actualizar estado cdEstado=2 de "Procesado" a "Procesando"
-- 2. Crear tabla TD_LOGS para registro de eventos y errores

USE IndexadorIA;
GO

-- ========================================
-- 1. Corregir estado "Procesado" a "Procesando"
-- ========================================
UPDATE TD_ESTADOS
SET dsEstado = 'Procesando'
WHERE dsProceso = 'ARCHIVO_ORIGINAL' AND cdEstado = 2;

-- Si no existe, crearlo
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_ORIGINAL' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('ARCHIVO_ORIGINAL', 2, 'Procesando');
	PRINT 'Estado ARCHIVO_ORIGINAL cdEstado=2 "Procesando" creado';
END
ELSE
BEGIN
	PRINT 'Estado ARCHIVO_ORIGINAL cdEstado=2 actualizado a "Procesando"';
END
GO

-- ========================================
-- 2. Crear tabla TD_LOGS
-- ========================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_LOGS')
BEGIN
	CREATE TABLE [dbo].[TD_LOGS](
		[cdLog] [bigint] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[dsNivel] [nvarchar](20) NOT NULL,
		[dsModulo] [nvarchar](100) NULL,
		[dsMensaje] [nvarchar](max) NOT NULL,
		[dsExcepcion] [nvarchar](max) NULL,
		[cdUsuario] [int] NULL,
		[dsUsuario] [nvarchar](50) NULL,
		[feRegistro] [datetime] NOT NULL DEFAULT GETDATE(),

		CONSTRAINT CHK_NIVEL_LOG CHECK (dsNivel IN ('DEBUG', 'INFO', 'WARNING', 'ERROR', 'CRITICAL'))
	);

	-- Índice para búsquedas por fecha
	CREATE INDEX IX_LOGS_FECHA ON TD_LOGS(feRegistro DESC);

	-- Índice para búsquedas por nivel
	CREATE INDEX IX_LOGS_NIVEL ON TD_LOGS(dsNivel);

	-- Índice para búsquedas por módulo
	CREATE INDEX IX_LOGS_MODULO ON TD_LOGS(dsModulo);

	PRINT 'Tabla TD_LOGS creada exitosamente';
END
ELSE
BEGIN
	PRINT 'Tabla TD_LOGS ya existe';
END
GO

PRINT '========================================';
PRINT 'Script de corrección ejecutado exitosamente';
PRINT '========================================';
GO
