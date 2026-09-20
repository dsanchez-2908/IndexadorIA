-- Alta del nuevo estado de control "Casos Especiales" (cdEstadoControl = 5)
-- usado por FrmVerLote.btnGuardarCasosEspeciales cuando la Parcela tiene letras
-- o representa mas de una parcela.
IF NOT EXISTS (SELECT 1 FROM TD_ESTADOS WHERE dsProceso = 'CONTROL' AND cdEstado = 5)
BEGIN
	INSERT INTO [dbo].[TD_ESTADOS]
			   ([dsProceso]
			   ,[cdEstado]
			   ,[dsEstado])
		 VALUES
			   ('CONTROL'
			   ,5
			   ,'Casos Especiales')
END
GO
