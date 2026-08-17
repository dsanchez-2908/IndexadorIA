-- =============================================
-- ETAPA 10: Asignacion de Lotes
-- Agrega el estado de lote cdEstado=6 "Controlando"
-- y la columna TD_LOTE.cdUsuarioAsignado, usados
-- por la pantalla FrmAsignacionLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 6)
BEGIN
	INSERT INTO TD_ESTADOS (dsProceso, cdEstado, dsEstado)
	VALUES ('LOTE', 6, 'Controlando')
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioAsignado')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [cdUsuarioAsignado] INT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_ASIGNADO')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_ASIGNADO] FOREIGN KEY([cdUsuarioAsignado])
		REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
