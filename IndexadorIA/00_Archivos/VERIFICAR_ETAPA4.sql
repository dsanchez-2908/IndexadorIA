-- =============================================
-- Script de Verificación - ETAPA 4
-- Descripción: Verifica que todos los objetos estén creados correctamente
-- =============================================

USE IndexadorDB
GO

PRINT ''
PRINT '===== VERIFICACIÓN DE OBJETOS ETAPA 4 ====='
PRINT ''

-- =============================================
-- 1. VERIFICAR SECUENCIA SEQ_LOTE
-- =============================================
PRINT '1. Verificando SECUENCIA SEQ_LOTE...'
IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SEQ_LOTE')
BEGIN
	PRINT '   ✓ Secuencia SEQ_LOTE existe'
	SELECT 
		'SEQ_LOTE' AS Nombre,
		current_value AS ValorActual,
		increment AS Incremento,
		minimum_value AS ValorMinimo,
		maximum_value AS ValorMaximo
	FROM sys.sequences 
	WHERE name = 'SEQ_LOTE'
END
ELSE
BEGIN
	PRINT '   ✗ ERROR: Secuencia SEQ_LOTE NO existe'
	PRINT '   → Ejecute el script ScriptsDB_ETAPA4_PreparacionLotes.sql'
END
PRINT ''

-- =============================================
-- 2. VERIFICAR TABLA TD_LOTE
-- =============================================
PRINT '2. Verificando TABLA TD_LOTE...'
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE')
BEGIN
	PRINT '   ✓ Tabla TD_LOTE existe'

	-- Mostrar estructura
	SELECT 
		c.name AS Columna,
		t.name AS TipoDato,
		c.max_length AS Longitud,
		c.is_nullable AS Nulable
	FROM sys.columns c
	INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
	WHERE c.object_id = OBJECT_ID('TD_LOTE')
	ORDER BY c.column_id

	-- Mostrar cantidad de registros
	DECLARE @CountLotes INT
	SELECT @CountLotes = COUNT(*) FROM TD_LOTE
	PRINT '   Registros en TD_LOTE: ' + CAST(@CountLotes AS VARCHAR(10))
END
ELSE
BEGIN
	PRINT '   ✗ ERROR: Tabla TD_LOTE NO existe'
	PRINT '   → Ejecute el script ScriptsDB_ETAPA4_PreparacionLotes.sql'
END
PRINT ''

-- =============================================
-- 3. VERIFICAR TABLA TD_LOTE_ARCHIVOS
-- =============================================
PRINT '3. Verificando TABLA TD_LOTE_ARCHIVOS...'
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE_ARCHIVOS')
BEGIN
	PRINT '   ✓ Tabla TD_LOTE_ARCHIVOS existe'

	-- Mostrar cantidad de registros
	DECLARE @CountRelaciones INT
	SELECT @CountRelaciones = COUNT(*) FROM TD_LOTE_ARCHIVOS
	PRINT '   Registros en TD_LOTE_ARCHIVOS: ' + CAST(@CountRelaciones AS VARCHAR(10))
END
ELSE
BEGIN
	PRINT '   ✗ ERROR: Tabla TD_LOTE_ARCHIVOS NO existe'
	PRINT '   → Ejecute el script ScriptsDB_ETAPA4_PreparacionLotes.sql'
END
PRINT ''

-- =============================================
-- 4. VERIFICAR ESTADOS
-- =============================================
PRINT '4. Verificando ESTADOS...'

-- Estado ARCHIVO_PAGINA cdEstado=2
IF EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 2)
	PRINT '   ✓ Estado ARCHIVO_PAGINA/2 existe'
ELSE
	PRINT '   ✗ ERROR: Estado ARCHIVO_PAGINA/2 NO existe'

-- Estados LOTE
IF EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 1)
	PRINT '   ✓ Estado LOTE/1 existe'
ELSE
	PRINT '   ✗ ERROR: Estado LOTE/1 NO existe'

IF EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 2)
	PRINT '   ✓ Estado LOTE/2 existe'
ELSE
	PRINT '   ✗ ERROR: Estado LOTE/2 NO existe'

IF EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 3)
	PRINT '   ✓ Estado LOTE/3 existe'
ELSE
	PRINT '   ✗ ERROR: Estado LOTE/3 NO existe'

PRINT ''
PRINT 'Detalle de estados:'
SELECT dsProceso, cdEstado, dsEstado 
FROM TD_ESTADOS 
WHERE dsProceso IN ('ARCHIVO_PAGINA', 'LOTE')
ORDER BY dsProceso, cdEstado
PRINT ''

-- =============================================
-- 5. VERIFICAR ÍNDICES
-- =============================================
PRINT '5. Verificando ÍNDICES...'

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_dsNombreLote')
	PRINT '   ✓ Índice IX_TD_LOTE_dsNombreLote existe'
ELSE
	PRINT '   ✗ Índice IX_TD_LOTE_dsNombreLote NO existe'

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_cdEstadoLote')
	PRINT '   ✓ Índice IX_TD_LOTE_cdEstadoLote existe'
ELSE
	PRINT '   ✗ Índice IX_TD_LOTE_cdEstadoLote NO existe'

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_ARCHIVOS_cdLote')
	PRINT '   ✓ Índice IX_TD_LOTE_ARCHIVOS_cdLote existe'
ELSE
	PRINT '   ✗ Índice IX_TD_LOTE_ARCHIVOS_cdLote NO existe'

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TD_LOTE_ARCHIVOS_cdArchivoPagina')
	PRINT '   ✓ Índice IX_TD_LOTE_ARCHIVOS_cdArchivoPagina existe (UNIQUE)'
ELSE
	PRINT '   ✗ Índice IX_TD_LOTE_ARCHIVOS_cdArchivoPagina NO existe'

PRINT ''

-- =============================================
-- 6. VERIFICAR ARCHIVOS DISPONIBLES
-- =============================================
PRINT '6. Verificando ARCHIVOS DISPONIBLES para agrupar...'

DECLARE @CountDisponibles INT
SELECT @CountDisponibles = COUNT(*)
FROM TD_ARCHIVOS_PAGINAS a
LEFT JOIN TD_ARCHIVOS_ORIGINAL b ON a.cdArchivoOriginal = b.cdArchivo
WHERE b.cdProyecto = 1 AND a.cdEstado = 1

PRINT '   Archivos página en estado 1 (Pendiente de Agrupar): ' + CAST(@CountDisponibles AS VARCHAR(10))

IF @CountDisponibles > 0
BEGIN
	PRINT '   ✓ Hay archivos disponibles para crear lotes'

	-- Mostrar muestra de los primeros 5
	PRINT ''
	PRINT '   Muestra de archivos disponibles (primeros 5):'
	SELECT TOP 5
		a.cdArchivoPagina,
		a.dsNombreArchivoPagina,
		a.feAlta,
		b.dsNombreArchivo
	FROM TD_ARCHIVOS_PAGINAS a
	LEFT JOIN TD_ARCHIVOS_ORIGINAL b ON a.cdArchivoOriginal = b.cdArchivo
	WHERE b.cdProyecto = 1 AND a.cdEstado = 1
	ORDER BY a.feAlta DESC
END
ELSE
BEGIN
	PRINT '   ℹ No hay archivos disponibles'
	PRINT '   → Primero debe ejecutar "Separación de Imágenes"'
END

PRINT ''

-- =============================================
-- 7. RESUMEN FINAL
-- =============================================
PRINT '===== RESUMEN ====='

DECLARE @TodoOK BIT = 1

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SEQ_LOTE')
	SET @TodoOK = 0

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE')
	SET @TodoOK = 0

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE_ARCHIVOS')
	SET @TodoOK = 0

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 2)
	SET @TodoOK = 0

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado IN (1,2,3))
	SET @TodoOK = 0

IF @TodoOK = 1
BEGIN
	PRINT '✓✓✓ TODOS LOS OBJETOS ESTÁN CORRECTAMENTE CREADOS ✓✓✓'
	PRINT ''
	PRINT 'La funcionalidad de Preparación de Lotes está lista para usar.'
END
ELSE
BEGIN
	PRINT '✗✗✗ FALTAN OBJETOS POR CREAR ✗✗✗'
	PRINT ''
	PRINT 'Ejecute el script: ScriptsDB_ETAPA4_PreparacionLotes.sql'
	PRINT 'Ubicación: IndexadorIA\00_Archivos\ScriptsDB_ETAPA4_PreparacionLotes.sql'
END

PRINT ''
PRINT '===== FIN DE VERIFICACIÓN ====='
GO
