-- ETAPA 22: Validador API SMP (Finalizacion de Planos)
-- Tabla cabecera: 1 fila por cdResultado (se sobrescribe al reprocesar)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_SMP_VALIDACION')
BEGIN
	CREATE TABLE TD_SMP_VALIDACION
	(
		cdValidacion        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TD_SMP_VALIDACION PRIMARY KEY,
		cdResultado         INT NOT NULL,
		cdLote              INT NULL,
		cdArchivoPagina     INT NULL,
		dsSeccion           VARCHAR(50) NULL,
		dsManzana           VARCHAR(50) NULL,
		dsParcela           VARCHAR(50) NULL,
		dsSmpConsultado     VARCHAR(30) NULL,
		dsEstado            VARCHAR(20) NOT NULL,   -- vigente, no_vigente, no_existente, error, OMITIDO
		dsMensaje           NVARCHAR(MAX) NULL,
		dsMotivoOmision     VARCHAR(200) NULL,
		nuHttpStatus        INT NULL,
		dsJson              NVARCHAR(MAX) NULL,
		feConsulta          DATETIME NULL,
		feAlta              DATETIME NOT NULL CONSTRAINT DF_TD_SMP_VALIDACION_feAlta DEFAULT GETDATE(),
		feModificacion      DATETIME NULL
	);

	CREATE UNIQUE INDEX UX_TD_SMP_VALIDACION_cdResultado ON TD_SMP_VALIDACION (cdResultado);
	CREATE INDEX IX_TD_SMP_VALIDACION_cdLote ON TD_SMP_VALIDACION (cdLote);
	CREATE INDEX IX_TD_SMP_VALIDACION_dsEstado ON TD_SMP_VALIDACION (dsEstado);
END
GO

-- Tabla detalle: 1 fila por parcela vigente devuelta por la API
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_SMP_VALIDACION_PARCELA')
BEGIN
	CREATE TABLE TD_SMP_VALIDACION_PARCELA
	(
		cdValidacionParcela INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TD_SMP_VALIDACION_PARCELA PRIMARY KEY,
		cdValidacion        INT NOT NULL,
		dsSmp               VARCHAR(30) NOT NULL,
		dsSurgeDe           NVARCHAR(500) NULL,
		feRegistro          DATE NULL,
		dsCaso              NVARCHAR(100) NULL,
		dsObservacion       NVARCHAR(MAX) NULL,
		dsDirecciones       NVARCHAR(MAX) NULL,
		CONSTRAINT FK_TD_SMP_VALIDACION_PARCELA_VALIDACION FOREIGN KEY (cdValidacion)
			REFERENCES TD_SMP_VALIDACION (cdValidacion) ON DELETE CASCADE
	);

	CREATE INDEX IX_TD_SMP_VALIDACION_PARCELA_cdValidacion ON TD_SMP_VALIDACION_PARCELA (cdValidacion);
END
GO
