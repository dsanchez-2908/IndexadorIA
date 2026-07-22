-- =============================================
-- Script: Agregar columna snResultadoProcesado a TD_BATCH_TRACKING
-- Descripción: Permite marcar batches cuyos resultados ya fueron procesados
-- Fecha: 2026-07-22
-- =============================================

USE [IndexadorIA]
GO

PRINT 'Agregando columna snResultadoProcesado a TD_BATCH_TRACKING...'

-- Verificar si la columna ya existe
IF NOT EXISTS (
	SELECT 1 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'TD_BATCH_TRACKING' 
	AND COLUMN_NAME = 'snResultadoProcesado'
)
BEGIN
	ALTER TABLE [dbo].[TD_BATCH_TRACKING]
	ADD [snResultadoProcesado] CHAR(2) NOT NULL DEFAULT 'NO'
	CONSTRAINT CHK_RESULTADO_PROCESADO CHECK (snResultadoProcesado IN ('SI', 'NO'))

	PRINT 'Columna snResultadoProcesado agregada exitosamente'
END
ELSE
BEGIN
	PRINT 'La columna snResultadoProcesado ya existe'
END
GO

-- Actualizar registros existentes donde el estado es 'completed' y hay output file
-- (asumimos que si hay output file ya fue procesado antes)
UPDATE [dbo].[TD_BATCH_TRACKING]
SET snResultadoProcesado = 'SI'
WHERE dsEstado = 'completed' 
  AND dsOutputFileId IS NOT NULL
  AND dsOutputFileId <> ''
GO

PRINT 'Registros existentes actualizados'
GO

SELECT 'Verificación:' as Accion, 
	   COUNT(*) as Total,
	   SUM(CASE WHEN snResultadoProcesado = 'SI' THEN 1 ELSE 0 END) as Procesados,
	   SUM(CASE WHEN snResultadoProcesado = 'NO' THEN 1 ELSE 0 END) as Pendientes
FROM TD_BATCH_TRACKING
GO
