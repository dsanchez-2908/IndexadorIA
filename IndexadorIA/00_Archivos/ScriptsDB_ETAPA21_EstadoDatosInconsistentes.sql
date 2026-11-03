-- Agrega el nuevo estado de control "Datos Inconsistentes" (cdEstadoControl = 7), utilizado
-- por FrmFinalizarLote para marcar automaticamente los registros Controlados que no pasan
-- las validaciones de formato de Expediente, Seccion, Manzana, Parcela o Numero de Plano
-- al momento de finalizar el lote.
INSERT INTO [dbo].[TD_ESTADOS]
		   ([dsProceso]
		   ,[cdEstado]
		   ,[dsEstado])
	 VALUES
		   ('CONTROL'
		   ,7
		   ,'Datos Inconsistentes')
GO
