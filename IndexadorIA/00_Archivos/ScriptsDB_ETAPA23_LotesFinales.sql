-- ETAPA 23: Lotes Finales (Finalizacion de Planos)
-- Lote final: agrupa archivos ORIGINALES (TD_ARCHIVOS_ORIGINAL) asignados a un usuario Data Entry.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE_FINAL')
BEGIN
	CREATE TABLE TD_LOTE_FINAL
	(
		cdLoteFinal         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TD_LOTE_FINAL PRIMARY KEY,
		dsNombreLote        VARCHAR(100) NOT NULL,
		cdUsuarioAsignado   INT NOT NULL,
		cdUsuarioAlta       INT NOT NULL,
		cdEstadoLote        INT NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_estado DEFAULT 1,  -- 1=Asignado, 2=Finalizado
		nuCantidadArchivos  INT NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_cant DEFAULT 0,
		feAlta              DATETIME NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_feAlta DEFAULT GETDATE(),
		feUltimoCambio      DATETIME NULL,
		feFinalizacion      DATETIME NULL,
		CONSTRAINT FK_TD_LOTE_FINAL_UsuarioAsignado FOREIGN KEY (cdUsuarioAsignado) REFERENCES TD_USUARIOS (cdUsuario),
		CONSTRAINT FK_TD_LOTE_FINAL_UsuarioAlta FOREIGN KEY (cdUsuarioAlta) REFERENCES TD_USUARIOS (cdUsuario)
	);
	CREATE INDEX IX_TD_LOTE_FINAL_usuario ON TD_LOTE_FINAL (cdUsuarioAsignado, cdEstadoLote);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE_FINAL_ARCHIVOS')
BEGIN
	CREATE TABLE TD_LOTE_FINAL_ARCHIVOS
	(
		cdLoteFinalArchivo  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TD_LOTE_FINAL_ARCHIVOS PRIMARY KEY,
		cdLoteFinal         INT NOT NULL,
		cdArchivoOriginal   INT NOT NULL,   -- TD_ARCHIVOS_ORIGINAL.cdArchivo
		cdEstado            INT NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_ARCH_estado DEFAULT 1,  -- 1=Pendiente, 2=Finalizado, 3=Devuelto para Actualizar SMP
		feFinalizacion      DATETIME NULL,
		feUltimoCambio      DATETIME NULL,
		CONSTRAINT FK_TD_LOTE_FINAL_ARCH_Lote FOREIGN KEY (cdLoteFinal) REFERENCES TD_LOTE_FINAL (cdLoteFinal)
	);
	CREATE INDEX IX_TD_LOTE_FINAL_ARCH_lote ON TD_LOTE_FINAL_ARCHIVOS (cdLoteFinal, cdEstado);
	CREATE INDEX IX_TD_LOTE_FINAL_ARCH_original ON TD_LOTE_FINAL_ARCHIVOS (cdArchivoOriginal, cdEstado);
END
GO
