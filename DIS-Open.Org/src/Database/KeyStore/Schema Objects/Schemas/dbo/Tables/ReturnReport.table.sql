CREATE TABLE [dbo].[ReturnReport] (
    [CustomerReturnUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [ReturnUniqueID]         UNIQUEIDENTIFIER NULL,
    [MSReturnNumber]         NCHAR (10)       NULL,
    [ReturnDateUTC]          DATETIME         NULL,
    [OEMRMADateUTC]          DATETIME         NULL,
    [OEMRMANumber]           NVARCHAR (35)    NOT NULL,
    [SoldToCustomerName]     NVARCHAR (40)    NULL,
    [OEMRMADate]             DATETIME         NOT NULL,
    [SoldToCustomerID]       NVARCHAR (10)    NOT NULL,
    [ReturnNoCredit]         BIT              NOT NULL,
    [ReportStatus]           INT              NOT NULL
);



