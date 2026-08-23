-- ============================================================================
-- Agrega la columna dsModelo a TD_TOKEN para registrar el modelo de OpenAI
-- utilizado en cada llamada (ej. gpt-4o-mini), usado luego en el reporte de
-- tokens consumidos.
-- ============================================================================
USE [IndexadorIA]
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID(N'[dbo].[TD_TOKEN]') AND name = 'dsModelo'
)
BEGIN
	ALTER TABLE [dbo].[TD_TOKEN] ADD [dsModelo] [varchar](100) NULL
	PRINT 'Columna dsModelo agregada a TD_TOKEN.'
END
ELSE
BEGIN
	PRINT 'Columna dsModelo ya existe en TD_TOKEN.'
END
GO
