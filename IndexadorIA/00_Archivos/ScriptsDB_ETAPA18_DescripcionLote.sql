-- =============================================
-- ETAPA 18: Descripcion de Lote
-- - Agrega la columna TD_LOTE.dsDescripcion VARCHAR(100)
-- Usada en la grilla dgvLotes de FrmAsignacionLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'dsDescripcion')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [dsDescripcion] VARCHAR(100) NULL
END
GO
