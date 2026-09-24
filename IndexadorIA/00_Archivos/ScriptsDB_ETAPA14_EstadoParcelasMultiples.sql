-- ETAPA 14: Nuevo estado de control "Parcelas Multiples"
-- Agrega el estado cdEstado=6 al proceso CONTROL para permitir marcar registros
-- que poseen múltiples parcelas asociadas.

INSERT INTO [dbo].[TD_ESTADOS]
		   ([dsProceso]
		   ,[cdEstado]
		   ,[dsEstado])
	 VALUES
		   ('CONTROL'
		   ,6
		   ,'Parcelas Multiples')
GO
