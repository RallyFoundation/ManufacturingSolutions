CREATE TABLE [dbo].[ComputerBuildReport] (
    [MSReportUniqueID]       UNIQUEIDENTIFIER NULL,
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [MSReceivedDateUTC]      DATE             NULL,
    [SoldToCustomerID]       NVARCHAR (50)    NULL,
    [ReceivedFromCustomerID] NVARCHAR (50)    NULL,
    [CBRAckFileTotal]        INT              NULL,
    [CBRAckFileNumber]       INT              NULL,
    [Status]                 BIT              NULL
);













