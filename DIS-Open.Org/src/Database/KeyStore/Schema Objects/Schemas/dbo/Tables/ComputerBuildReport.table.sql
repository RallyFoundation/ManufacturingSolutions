CREATE TABLE [dbo].[ComputerBuildReport] (
    [MSReportUniqueID]       UNIQUEIDENTIFIER NULL,
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [MSReceivedDateUTC]      DATETIME         NULL,
    [SoldToCustomerID]       NVARCHAR (10) COLLATE SQL_Latin1_General_CP1_CS_AS   NOT NULL,
    [ReceivedFromCustomerID] NVARCHAR (10) COLLATE SQL_Latin1_General_CP1_CS_AS   NOT NULL,
    [CBRAckFileTotal]        INT              NULL,
    [CBRAckFileNumber]       INT              NULL,
    [CBRStatus]              INT              NOT NULL,
    [CreatedDateUTC]         DATETIME         NOT NULL,
    [ModifiedDateUTC]        DATETIME         NOT NULL
);





