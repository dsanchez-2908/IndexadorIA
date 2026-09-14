-- =============================================
-- ETAPA 16: Finalizar Lote (Etapa 4 del proceso de Auditoria)
-- - Agrega el estado de lote cdEstado=10 "Pendiente de Enviar"
-- - Agrega las columnas TD_LOTE.feFinalizado y TD_LOTE.cdUsuarioFinalizado
-- Usados por la pantalla FrmFinalizarLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 10)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('LOTE'
			   ,10
			   ,'Pendiente de Enviar')
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'feFinalizado')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [feFinalizado] DATETIME NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioFinalizado')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [cdUsuarioFinalizado] INT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_FINALIZADO')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_FINALIZADO] FOREIGN KEY([cdUsuarioFinalizado])
		REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
