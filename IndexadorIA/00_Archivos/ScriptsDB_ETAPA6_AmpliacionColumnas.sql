-- =============================================
-- Script: Ampliación de columna dsExpediente
-- Descripción: Amplía dsExpediente para almacenar JSON completo de OpenAI
-- Fecha: 2026-07-21
-- =============================================

USE [IndexadorIA]
GO

PRINT 'Ampliando columna dsExpediente en TD_001_RESULTADO_IA...'

-- Ampliar dsExpediente de nvarchar(100) a nvarchar(MAX) para almacenar JSON completo
ALTER TABLE [dbo].[TD_001_RESULTADO_IA]
ALTER COLUMN [dsExpediente] [nvarchar](MAX) NULL
GO

PRINT 'Columna dsExpediente ampliada exitosamente a nvarchar(MAX)'
GO
