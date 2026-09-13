-- =============================================
-- ETAPA 14: Asignacion de Auditoria (Etapa 1 del proceso de Auditoria)
-- - Cambia la descripcion de TD_ESTADOS (LOTE, 7) de
--   "Pendiente de Finalizar" a "Pendiente Asignar Auditoria"
-- - Agrega el estado de lote cdEstado=8 "Auditando"
-- - Agrega las columnas TD_LOTE.cdUsuarioAuditor y TD_LOTE.feFinAuditoria
-- Usados por la pantalla FrmAsignarAuditoria
-- =============================================

UPDATE TD_ESTADOS
SET dsEstado = 'Pendiente Asignar Auditoria'
WHERE dsProceso = 'LOTE' AND cdEstado = 7;
GO

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 8)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('LOTE'
			   ,8
			   ,'Auditando')
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioAuditor')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [cdUsuarioAuditor] INT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'feFinAuditoria')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [feFinAuditoria] DATETIME NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_AUDITOR')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_AUDITOR] FOREIGN KEY([cdUsuarioAuditor])
		REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso = 'LOTE' ORDER BY cdEstado;
GO
