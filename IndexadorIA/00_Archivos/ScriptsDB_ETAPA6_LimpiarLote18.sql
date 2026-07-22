-- =============================================
-- Script: Limpiar datos mal parseados de lote 18
-- Descripción: Elimina resultados con JSON sin parsear
-- Fecha: 2026-07-22
-- =============================================

USE [IndexadorIA]
GO

PRINT 'Eliminando resultados mal parseados del lote 18...'

-- Eliminar tokens asociados
DELETE FROM TD_TOKEN 
WHERE cdResultado IN (SELECT cdResultado FROM TD_001_RESULTADO_IA WHERE cdLote = 18)
GO

-- Eliminar resultados
DELETE FROM TD_001_RESULTADO_IA WHERE cdLote = 18
GO

-- Resetear estado de páginas del lote 18 a "Listo para Procesar" (cdEstado = 2)
UPDATE TD_ARCHIVOS_PAGINAS
SET cdEstado = 2
WHERE cdArchivoPagina IN (
	SELECT ap.cdArchivoPagina 
	FROM TD_ARCHIVOS_PAGINAS ap
	INNER JOIN TD_LOTE_ARCHIVOS la ON ap.cdArchivoPagina = la.cdArchivoPagina
	WHERE la.cdLote = 18
)
GO

-- Resetear estado del lote 18 a "Listo para Procesar" (cdEstado = 2)
UPDATE TD_LOTE
SET cdEstadoLote = 2
WHERE cdLote = 18
GO

PRINT 'Lote 18 limpiado y listo para reprocesar'
GO

SELECT 'Verificación:' as Accion, COUNT(*) as Registros FROM TD_001_RESULTADO_IA WHERE cdLote = 18
UNION ALL
SELECT 'Tokens eliminados:', COUNT(*) FROM TD_TOKEN WHERE cdResultado IN (SELECT cdResultado FROM TD_001_RESULTADO_IA WHERE cdLote = 18)
GO
