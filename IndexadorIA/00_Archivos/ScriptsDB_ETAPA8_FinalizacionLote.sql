-- =============================================
-- ETAPA 8: Finalizacion de Lote
-- Agrega el estado de lote cdEstado=5 "Completado"
-- usado al ejecutar "Marcar Lote Completado" en FrmVerLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 5)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 5, 'Completado')
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
