CREATE TABLE [dbo].[ReturnReportKey] (
    [ReturnUniqueID]              UNIQUEIDENTIFIER NOT NULL,
    [OEMRMALineNumber]            INT              NOT NULL,
    [ReturnTypeID]                NCHAR (3)        NOT NULL,
    [ProductKeyID]                BIGINT           NOT NULL,
    [MSReturnLineNumber]          INT              NULL,
    [LicensablePartNumber]        NVARCHAR (16)    NULL,
    [ReturnReasonCode]            NVARCHAR (2)     NULL,
    [ReturnReasonCodeDescription] NVARCHAR (40)    NULL
);



