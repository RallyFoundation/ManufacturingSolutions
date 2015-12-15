CREATE TABLE [dbo].[ReturnReport] (
    [ReturnUniqueID]     UNIQUEIDENTIFIER NOT NULL,
    [MSReturnNumber]     NCHAR (10)       NULL,
    [ReturnDateUTC]      DATETIME         NULL,
    [OEMRMADateUTC]      DATETIME         NOT NULL,
    [OEMRMANumber]       NVARCHAR (35)    NOT NULL,
    [SoldToCustomerName] NVARCHAR (40)    NULL,
    [OEMRMADate]         DATETIME         NOT NULL,
    [SoldToCustomerID]   NVARCHAR (10)    NOT NULL,
    [ReturnNoCredit]     BIT              NOT NULL,
    [status]             BIT              NULL
);



