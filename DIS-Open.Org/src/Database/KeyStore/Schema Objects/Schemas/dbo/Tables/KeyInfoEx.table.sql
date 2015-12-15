CREATE TABLE [dbo].[KeyInfoEx] (
    [ProductKeyID]         BIGINT       NOT NULL,
    [KeyType]              INT          NULL,
    [SSID]                 INT          NULL,
    [HQID]                 INT          NULL,
    [IsInProgress]         BIT          NOT NULL,
    [ShouldCarbonCopy]     BIT          NULL
);



