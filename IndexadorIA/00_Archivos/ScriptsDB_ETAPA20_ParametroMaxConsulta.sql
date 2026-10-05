-- =============================================
-- ETAPA 20: Parametro Cantidad Maxima de Registros de Consulta de Planos
-- - Agrega en TD_PARAMETROS la clave MAX_CONSULTA, usada por FrmConsultaPlanos
--   (boton Buscar) para limitar la cantidad maxima de registros a mostrar.
-- =============================================

IF NOT EXISTS (SELECT 1 FROM [dbo].[TD_PARAMETROS] WHERE [dsClaveParametro] = 'MAX_CONSULTA')
BEGIN
	INSERT INTO [dbo].[TD_PARAMETROS]
			   ([dsClaveParametro]
			   ,[dsValorParametro]
			   ,[dsDescripcion]
			   ,[feUltimaModificacion]
			   ,[cdUsuarioModificacion])
		 VALUES
			   ('MAX_CONSULTA'
			   ,'100000'
			   ,'Cantidad Maximo de Registro de consulta de planos'
			   ,GETDATE()
			   ,1)
END
GO
