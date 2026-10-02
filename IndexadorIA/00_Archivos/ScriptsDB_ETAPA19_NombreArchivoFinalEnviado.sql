-- =============================================
-- ETAPA 19: Nombre de Archivo Final Enviado
-- - Agrega la columna TD_ARCHIVOS_PAGINAS.dsNombreArchivoFinalEnviado VARCHAR(500)
-- Usada por el proceso de Envio (FrmEnvioLote / LoteEnvioBL) para dejar registrado
-- el nombre de PDF efectivamente entregado en la carpeta de Entrega, en los casos
-- en que debio renombrarse por colision con otro archivo ya existente en destino
-- (dsNombreArchivoFinal queda intacto, reflejando el nombre generado en la
-- finalizacion del lote).
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TD_ARCHIVOS_PAGINAS]') AND name = 'dsNombreArchivoFinalEnviado')
BEGIN
	ALTER TABLE [dbo].[TD_ARCHIVOS_PAGINAS]
	ADD [dsNombreArchivoFinalEnviado] VARCHAR(500) NULL
END
GO
