-- =============================================
-- Agrega columnas a TD_LOTE para registrar fecha/hora y usuario que finalizó
-- el control del lote (al marcar el lote como "Pendiente de Finalizar" desde
-- FrmVerLote.btnMarcarLoteCompletado). Se usan nombres específicos
-- (feFinControl / cdUsuarioFinControl) en lugar de un nombre genérico, para
-- evitar que en el futuro otros cambios de estado del lote (por ejemplo los
-- que hace OpenAIBL durante el procesamiento) pisen por error esta
-- información al reutilizar LoteDAL.ActualizarEstado.
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'feFinControl')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD feFinControl DATETIME NULL

	PRINT 'Columna feFinControl agregada a TD_LOTE'
END
ELSE
BEGIN
	PRINT 'La columna feFinControl ya existe en TD_LOTE'
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioFinControl')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD cdUsuarioFinControl INT NULL

	PRINT 'Columna cdUsuarioFinControl agregada a TD_LOTE'
END
ELSE
BEGIN
	PRINT 'La columna cdUsuarioFinControl ya existe en TD_LOTE'
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_FIN_CONTROL')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_FIN_CONTROL] FOREIGN KEY([cdUsuarioFinControl])
	REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])

	PRINT 'FK FK_TD_LOTE_USUARIO_FIN_CONTROL creada'
END
ELSE
BEGIN
	PRINT 'La FK FK_TD_LOTE_USUARIO_FIN_CONTROL ya existe'
END
GO
