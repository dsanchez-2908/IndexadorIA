-- =============================================
-- Script: ETAPA 4 - Preparación de Lotes
-- Descripción: Crea tablas y estados para agrupar páginas en lotes
-- Fecha: 2026-01-13
-- =============================================

USE IndexadorDB
GO

-- =============================================
-- 1. AGREGAR NUEVOS ESTADOS
-- =============================================

-- Estado para archivo_pagina: Pendiente de Preparar imágenes
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('ARCHIVO_PAGINA', 2, 'Pendiente de Preparar imágenes')
END
GO

-- Estados para lote
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 1)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 1, 'Pendiente de Preparar imágenes')
END
GO

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 2)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 2, 'En proceso')
END
GO

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 3, 'Finalizado')
END
GO

-- =============================================
-- 2. CREAR TABLA TD_LOTE
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'TD_LOTE') AND type = 'U')
BEGIN
	CREATE TABLE TD_LOTE
	(
		cdLote              INT IDENTITY(1,1) NOT NULL,
		dsNombreLote        VARCHAR(50) NOT NULL,
		nuCantidadArchivos  INT NOT NULL DEFAULT 0,
		cdEstadoLote        INT NOT NULL DEFAULT 1,
		feAltaLote          DATETIME NOT NULL DEFAULT GETDATE(),
		cdUsuarioAltaLote   INT NULL,

		CONSTRAINT PK_TD_LOTE PRIMARY KEY CLUSTERED (cdLote),
		CONSTRAINT FK_TD_LOTE_USUARIO FOREIGN KEY (cdUsuarioAltaLote) REFERENCES TD_USUARIOS(cdUsuario)
	)

	PRINT 'Tabla TD_LOTE creada exitosamente'
END
ELSE
BEGIN
	PRINT 'Tabla TD_LOTE ya existe'
END
GO

-- =============================================
-- 3. CREAR ÍNDICES PARA TD_LOTE
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_dsNombreLote')
BEGIN
	CREATE UNIQUE INDEX IX_TD_LOTE_dsNombreLote ON TD_LOTE(dsNombreLote)
	PRINT 'Índice IX_TD_LOTE_dsNombreLote creado'
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_cdEstadoLote')
BEGIN
	CREATE INDEX IX_TD_LOTE_cdEstadoLote ON TD_LOTE(cdEstadoLote)
	PRINT 'Índice IX_TD_LOTE_cdEstadoLote creado'
END
GO

-- =============================================
-- 4. CREAR TABLA DE RELACIÓN TD_LOTE_ARCHIVOS
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'TD_LOTE_ARCHIVOS') AND type = 'U')
BEGIN
	CREATE TABLE TD_LOTE_ARCHIVOS
	(
		id                  INT IDENTITY(1,1) NOT NULL,
		cdLote              INT NOT NULL,
		cdArchivoPagina     INT NOT NULL,

		CONSTRAINT PK_TD_LOTE_ARCHIVOS PRIMARY KEY CLUSTERED (id),
		CONSTRAINT FK_TD_LOTE_ARCHIVOS_Lote FOREIGN KEY (cdLote) REFERENCES TD_LOTE(cdLote),
		CONSTRAINT FK_TD_LOTE_ARCHIVOS_Pagina FOREIGN KEY (cdArchivoPagina) REFERENCES TD_ARCHIVOS_PAGINAS(cdArchivoPagina)
	)

	PRINT 'Tabla TD_LOTE_ARCHIVOS creada exitosamente'
END
ELSE
BEGIN
	PRINT 'Tabla TD_LOTE_ARCHIVOS ya existe'
END
GO

-- =============================================
-- 5. CREAR ÍNDICES PARA TD_LOTE_ARCHIVOS
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_ARCHIVOS_cdLote')
BEGIN
	CREATE INDEX IX_TD_LOTE_ARCHIVOS_cdLote ON TD_LOTE_ARCHIVOS(cdLote)
	PRINT 'Índice IX_TD_LOTE_ARCHIVOS_cdLote creado'
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_ARCHIVOS_cdArchivoPagina')
BEGIN
	CREATE UNIQUE INDEX IX_TD_LOTE_ARCHIVOS_cdArchivoPagina ON TD_LOTE_ARCHIVOS(cdArchivoPagina)
	PRINT 'Índice IX_TD_LOTE_ARCHIVOS_cdArchivoPagina creado (único - una página solo puede estar en un lote)'
END
GO

-- =============================================
-- 6. CREAR SECUENCIA PARA NOMBRES DE LOTE
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'SEQ_LOTE') AND type = 'SO')
BEGIN
	CREATE SEQUENCE SEQ_LOTE
		START WITH 1
		INCREMENT BY 1
		MINVALUE 1
		MAXVALUE 99999999
		NO CYCLE
	PRINT 'Secuencia SEQ_LOTE creada'
END
ELSE
BEGIN
	PRINT 'Secuencia SEQ_LOTE ya existe'
END
GO

-- =============================================
-- 7. VERIFICACIÓN DE ESTADOS
-- =============================================

PRINT ''
PRINT '===== ESTADOS CONFIGURADOS ====='
SELECT dsProceso, cdEstado, dsEstado 
FROM TD_ESTADOS 
WHERE dsProceso IN ('ARCHIVO_PAGINA', 'LOTE')
ORDER BY dsProceso, cdEstado
GO

PRINT ''
PRINT '===== SCRIPT ETAPA 4 COMPLETADO ====='
GO
