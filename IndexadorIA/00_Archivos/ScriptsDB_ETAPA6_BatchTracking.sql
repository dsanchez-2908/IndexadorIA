-- Script para tracking de batches de OpenAI
-- ETAPA 6 - Tabla de seguimiento de batches

USE IndexadorIA;
GO

-- Tabla para guardar información de batches en proceso
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TD_BATCH_TRACKING]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TD_BATCH_TRACKING](
		[cdBatchTracking] [int] IDENTITY(1,1) NOT NULL,
		[cdLote] [int] NOT NULL,
		[dsBatchId] [varchar](100) NOT NULL,
		[dsFileId] [varchar](100) NULL,
		[dsEstado] [varchar](50) NULL,  -- validating, in_progress, finalizing, completed, failed, etc.
		[nuTotalRequests] [int] NULL,
		[nuCompletedRequests] [int] NULL,
		[nuFailedRequests] [int] NULL,
		[dsOutputFileId] [varchar](100) NULL,
		[dsErrorFileId] [varchar](100) NULL,
		[feCreated] [datetime] NULL,
		[feInProgress] [datetime] NULL,
		[feCompleted] [datetime] NULL,
		[feFailed] [datetime] NULL,
		[feAlta] [datetime] NOT NULL DEFAULT GETDATE(),
		[feUltimaConsulta] [datetime] NULL,
		CONSTRAINT [PK_TD_BATCH_TRACKING] PRIMARY KEY CLUSTERED ([cdBatchTracking] ASC),
		CONSTRAINT [FK_BATCH_TRACKING_LOTE] FOREIGN KEY([cdLote]) 
			REFERENCES [dbo].[TD_LOTE] ([cdLote])
	) ON [PRIMARY];

	PRINT 'Tabla TD_BATCH_TRACKING creada exitosamente';
END
ELSE
BEGIN
	PRINT 'La tabla TD_BATCH_TRACKING ya existe';
END
GO

-- Índice para búsqueda rápida por lote
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_BATCH_TRACKING_LOTE' AND object_id = OBJECT_ID('TD_BATCH_TRACKING'))
BEGIN
	CREATE NONCLUSTERED INDEX [IX_BATCH_TRACKING_LOTE] 
	ON [dbo].[TD_BATCH_TRACKING]([cdLote] ASC);

	PRINT 'Índice IX_BATCH_TRACKING_LOTE creado';
END
GO

-- Índice para búsqueda por batch_id
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_BATCH_TRACKING_BATCHID' AND object_id = OBJECT_ID('TD_BATCH_TRACKING'))
BEGIN
	CREATE NONCLUSTERED INDEX [IX_BATCH_TRACKING_BATCHID] 
	ON [dbo].[TD_BATCH_TRACKING]([dsBatchId] ASC);

	PRINT 'Índice IX_BATCH_TRACKING_BATCHID creado';
END
GO

PRINT 'Script de tracking de batches completado';
GO
