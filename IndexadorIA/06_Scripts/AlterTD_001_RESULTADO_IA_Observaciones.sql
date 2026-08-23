-- Agrega el campo opcional dsObservaciones a TD_001_RESULTADO_IA
-- Permite que el usuario ingrese comentarios libres, especialmente al marcar
-- una página/los datos como ILEGIBLE.
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TD_001_RESULTADO_IA' AND COLUMN_NAME='dsObservaciones')
BEGIN
	ALTER TABLE TD_001_RESULTADO_IA ADD dsObservaciones VARCHAR(MAX) NULL;
END
GO
