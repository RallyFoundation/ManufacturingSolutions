CREATE TABLE [dbo].[HardwareBindingReport] (
    [ProductKeyID]           BIGINT           NOT NULL,
    [HardwareHash]           NVARCHAR (128)   NULL,
    [OEMOptionalInfo]        XML              NULL,
    [Status]                 BIT              NULL,
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [ReasonCode]             NVARCHAR (2)     NULL,
    [ReasonCodeDescription]  NVARCHAR (256)   NULL
);









