CREATE TABLE [dbo].[ComputerBuildReportKey] (
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [ProductKeyID]           BIGINT           NOT NULL,
    [HardwareHash]           NVARCHAR (512)   NOT NULL,
    [OEMOptionalInfo]        XML              NULL,
    [ReasonCode]             NVARCHAR (2)     NULL,
    [ReasonCodeDescription]  NVARCHAR (160)   NULL
);

