-- =============================================
-- ETAPA 7: Control y Finalización
-- Agrega el estado de lote cdEstado=4 "Listo para Control y Finalización"
-- usado por OpenAIBL al finalizar el procesamiento de resultados de un lote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 4)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 4, 'Listo para Control y Finalización')
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
