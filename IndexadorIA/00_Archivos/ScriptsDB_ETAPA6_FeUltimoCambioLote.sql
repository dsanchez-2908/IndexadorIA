-- =============================================
-- Agrega la columna feUltimoCambio a TD_LOTE
-- Usada para registrar la fecha/hora del ultimo cambio de estado relevante
-- del lote (por ejemplo, la fecha de asignacion de auditoria).
-- =============================================

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID(N'TD_LOTE') AND name = 'feUltimoCambio'
)
BEGIN
	ALTER TABLE TD_LOTE ADD feUltimoCambio DATETIME NULL

	PRINT 'Columna feUltimoCambio agregada a TD_LOTE'
END
ELSE
BEGIN
	PRINT 'La columna feUltimoCambio ya existe en TD_LOTE'
END
GO
