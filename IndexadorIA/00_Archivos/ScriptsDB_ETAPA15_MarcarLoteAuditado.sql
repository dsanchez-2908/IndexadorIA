-- =============================================
-- ETAPA 15: Marcar Lote como Auditado (Etapa 3 del proceso de Auditoria)
-- - Agrega el estado de lote cdEstado=9 "Pendiente de Finalizar"
-- - Agrega las columnas TD_LOTE.feAuditado y TD_LOTE.cdUsuarioAuditado
-- Usados por la pantalla FrmAuditarLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 9)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('LOTE'
			   ,9
			   ,'Pendiente de Finalizar')
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'feAuditado')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [feAuditado] DATETIME NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioAuditado')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [cdUsuarioAuditado] INT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_AUDITADO')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_AUDITADO] FOREIGN KEY([cdUsuarioAuditado])
		REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
