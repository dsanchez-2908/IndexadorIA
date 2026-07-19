-- =============================================
-- ETAPA 5: Preparación de Imágenes
-- Conversión PDF → JPG con Recorte y OCR
-- =============================================

USE IndexadorIA;
GO

PRINT '================================================';
PRINT 'ETAPA 5: Preparación de Imágenes';
PRINT 'Inicio: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '================================================';
GO

-- =============================================
-- 1. ESTADOS NUEVOS
-- =============================================

PRINT '';
PRINT '1. Agregando nuevos estados...';
GO

-- El estado LOTE cdEstado=2 ya fue creado en ETAPA 4 como "En proceso"
-- Lo actualizaremos para que sea más descriptivo
UPDATE TD_ESTADOS 
SET dsEstado = 'Imágenes preparadas'
WHERE dsProceso = 'LOTE' AND cdEstado = 2;
PRINT '  ✓ Estado LOTE cdEstado=2 actualizado a "Imágenes preparadas"';
GO

-- Estado para TD_ARCHIVOS_PAGINAS: Imagen preparada (cdEstado=3)
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 3)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('ARCHIVO_PAGINA', 3, 'Imagen preparada');
	PRINT '  ✓ Estado ARCHIVO_PAGINA cdEstado=3 "Imagen preparada" creado';
END
ELSE
BEGIN
	PRINT '  ℹ Estado ARCHIVO_PAGINA cdEstado=3 ya existe';
END
GO

-- =============================================
-- 2. TABLA TD_PAGINA_OCR
-- =============================================

PRINT '';
PRINT '2. Creando tabla TD_PAGINA_OCR...';
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_PAGINA_OCR')
BEGIN
	CREATE TABLE TD_PAGINA_OCR
	(
		id                  INT IDENTITY(1,1) NOT NULL,
		cdArchivoPagina     INT NOT NULL,
		txtResultadoOCR     NVARCHAR(MAX) NULL,
		feOCR               DATETIME NOT NULL DEFAULT GETDATE(),

		CONSTRAINT PK_TD_PAGINA_OCR PRIMARY KEY CLUSTERED (id),
		CONSTRAINT FK_TD_PAGINA_OCR_ARCHIVO_PAGINA FOREIGN KEY (cdArchivoPagina)
			REFERENCES TD_ARCHIVOS_PAGINAS(cdArchivoPagina)
	);

	PRINT '  ✓ Tabla TD_PAGINA_OCR creada correctamente';
END
ELSE
BEGIN
	PRINT '  ℹ Tabla TD_PAGINA_OCR ya existe';
END
GO

-- =============================================
-- 3. ÍNDICES
-- =============================================

PRINT '';
PRINT '3. Creando índices...';
GO

-- Índice en cdArchivoPagina para búsquedas rápidas
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_PAGINA_OCR_cdArchivoPagina' AND object_id = OBJECT_ID('TD_PAGINA_OCR'))
BEGIN
	CREATE NONCLUSTERED INDEX IX_TD_PAGINA_OCR_cdArchivoPagina
	ON TD_PAGINA_OCR(cdArchivoPagina);

	PRINT '  ✓ Índice IX_TD_PAGINA_OCR_cdArchivoPagina creado';
END
ELSE
BEGIN
	PRINT '  ℹ Índice IX_TD_PAGINA_OCR_cdArchivoPagina ya existe';
END
GO

-- Índice en feOCR para consultas por fecha
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_PAGINA_OCR_feOCR' AND object_id = OBJECT_ID('TD_PAGINA_OCR'))
BEGIN
	CREATE NONCLUSTERED INDEX IX_TD_PAGINA_OCR_feOCR
	ON TD_PAGINA_OCR(feOCR DESC);

	PRINT '  ✓ Índice IX_TD_PAGINA_OCR_feOCR creado';
END
ELSE
BEGIN
	PRINT '  ℹ Índice IX_TD_PAGINA_OCR_feOCR ya existe';
END
GO

-- =============================================
-- 4. VERIFICACIÓN FINAL
-- =============================================

PRINT '';
PRINT '4. Verificación final...';
GO

-- Verificar estados
DECLARE @countEstados INT;
SELECT @countEstados = COUNT(*) 
FROM TD_ESTADOS 
WHERE (dsProceso = 'LOTE' AND cdEstado = 2)
   OR (dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 3);

PRINT '  Estados verificados: ' + CAST(@countEstados AS VARCHAR);

-- Verificar tabla
IF OBJECT_ID('TD_PAGINA_OCR', 'U') IS NOT NULL
	PRINT '  ✓ Tabla TD_PAGINA_OCR existe';
ELSE
	PRINT '  ✗ ERROR: Tabla TD_PAGINA_OCR no existe';

-- Verificar índices
DECLARE @countIndices INT;
SELECT @countIndices = COUNT(*) 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('TD_PAGINA_OCR') 
  AND name IN ('IX_TD_PAGINA_OCR_cdArchivoPagina', 'IX_TD_PAGINA_OCR_feOCR');

PRINT '  Índices creados en TD_PAGINA_OCR: ' + CAST(@countIndices AS VARCHAR);

-- Verificar FK
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_PAGINA_OCR_ARCHIVO_PAGINA')
	PRINT '  ✓ Foreign Key FK_TD_PAGINA_OCR_ARCHIVO_PAGINA existe';
ELSE
	PRINT '  ✗ ERROR: Foreign Key FK_TD_PAGINA_OCR_ARCHIVO_PAGINA no existe';

PRINT '';
PRINT '================================================';
PRINT 'ETAPA 5: Completada exitosamente';
PRINT 'Fin: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '================================================';
GO
