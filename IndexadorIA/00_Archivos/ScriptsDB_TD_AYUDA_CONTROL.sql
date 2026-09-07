-- =============================================
-- Crea la tabla TD_AYUDA_CONTROL, usada para almacenar el texto de ayuda que
-- el administrador configura para cada uno de los 9 campos del panel de
-- detalle de FrmVerLote (Categoría de Plano, Tipo de Plano, Número de Plano,
-- Expediente, Sección, Manzana, Parcela, Dirección y Observaciones).
-- La tabla contiene un único registro global (no está asociada a un lote ni
-- a un proyecto en particular).
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_AYUDA_CONTROL')
BEGIN
	CREATE TABLE [dbo].[TD_AYUDA_CONTROL]
	(
		cdAyudaControl        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		dsCategoriaPlano      VARCHAR(MAX) NULL,
		dsTipoPlano           VARCHAR(MAX) NULL,
		dsExpediente          VARCHAR(MAX) NULL,
		dsSeccion             VARCHAR(MAX) NULL,
		dsManzana             VARCHAR(MAX) NULL,
		dsParcela             VARCHAR(MAX) NULL,
		dsDireccion           VARCHAR(MAX) NULL,
		dsNumeroPlano         VARCHAR(MAX) NULL,
		dsObservaciones       VARCHAR(MAX) NULL
	)

	PRINT 'Tabla TD_AYUDA_CONTROL creada'
END
ELSE
BEGIN
	PRINT 'La tabla TD_AYUDA_CONTROL ya existe'
END
GO
