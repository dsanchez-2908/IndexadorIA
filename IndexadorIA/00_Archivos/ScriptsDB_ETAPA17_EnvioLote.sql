-- =============================================
-- ETAPA 17: Envio de Lote (Etapa 5 del proceso de Auditoria)
-- - Agrega el estado de lote cdEstado=11 "Enviado"
-- - Agrega el estado de pagina (ARCHIVO_PAGINA) cdEstado=8 "Enviado"
-- - Agrega el parametro RUTA_FINAL_ENVIO
-- - Agrega las columnas TD_LOTE.feEnvio, cdUsuarioEnvio, dsCarpetaEnvio, dsObservacionesEnvio
-- Usados por la pantalla FrmEnvioLote
-- =============================================

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'LOTE' AND cdEstado = 11)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('LOTE'
			   ,11
			   ,'Enviado')
END
GO

IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_PAGINA' AND cdEstado = 8)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('ARCHIVO_PAGINA'
			   ,8
			   ,'Enviado')
END
GO

IF NOT EXISTS (SELECT 1 FROM TD_PARAMETROS WHERE dsClaveParametro = 'RUTA_FINAL_ENVIO')
BEGIN
	INSERT INTO [dbo].[TD_PARAMETROS]
			   ([dsClaveParametro]
			   ,[dsValorParametro]
			   ,[dsDescripcion]
			   ,[feUltimaModificacion]
			   ,[cdUsuarioModificacion])
		 VALUES
			   ('RUTA_FINAL_ENVIO'
			   ,''
			   ,'Ruta final de los archivos a enviar'
			   ,GETDATE()
			   ,1)
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'feEnvio')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [feEnvio] DATETIME NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'cdUsuarioEnvio')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [cdUsuarioEnvio] INT NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'dsCarpetaEnvio')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [dsCarpetaEnvio] NVARCHAR(260) NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_LOTE]') AND name = 'dsObservacionesEnvio')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD [dsObservacionesEnvio] NVARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_TD_LOTE_USUARIO_ENVIO')
BEGIN
	ALTER TABLE [dbo].[TD_LOTE]
	ADD CONSTRAINT [FK_TD_LOTE_USUARIO_ENVIO] FOREIGN KEY([cdUsuarioEnvio])
		REFERENCES [dbo].[TD_USUARIOS] ([cdUsuario])
END
GO

SELECT * FROM TD_ESTADOS WHERE dsProceso IN ('LOTE', 'ARCHIVO_PAGINA') ORDER BY dsProceso, cdEstado;
GO
