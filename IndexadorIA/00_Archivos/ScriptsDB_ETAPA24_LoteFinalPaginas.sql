-- ETAPA 24: Paginas finalizadas del Control Final (Finalizacion de Planos)
-- Registra cada pagina ya incluida en un PDF final, para que no vuelva a aparecer en la grilla del Data Entry.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TD_LOTE_FINAL_PAGINAS')
BEGIN
	CREATE TABLE TD_LOTE_FINAL_PAGINAS
	(
		cdLoteFinalPagina   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TD_LOTE_FINAL_PAGINAS PRIMARY KEY,
		cdLoteFinal         INT NOT NULL,
		cdArchivoPagina     INT NOT NULL,
		dsNombreArchivoFinal VARCHAR(500) NOT NULL,
		dsRutaArchivoFinal  VARCHAR(500) NOT NULL,
		snIlegible          CHAR(2) NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_PAG_ileg DEFAULT 'NO',
		cdUsuario           INT NOT NULL,
		feFinalizacion      DATETIME NOT NULL CONSTRAINT DF_TD_LOTE_FINAL_PAG_fe DEFAULT GETDATE(),
		CONSTRAINT FK_TD_LOTE_FINAL_PAG_Lote FOREIGN KEY (cdLoteFinal) REFERENCES TD_LOTE_FINAL (cdLoteFinal)
	);
	CREATE UNIQUE INDEX UX_TD_LOTE_FINAL_PAG_pagina ON TD_LOTE_FINAL_PAGINAS (cdArchivoPagina);
	CREATE INDEX IX_TD_LOTE_FINAL_PAG_lote ON TD_LOTE_FINAL_PAGINAS (cdLoteFinal);
END
GO
