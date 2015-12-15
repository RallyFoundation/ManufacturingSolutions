CREATE TABLE [dbo].[FulfillmentInfo] (
    [FulfillmentNumber] CHAR (10)        NOT NULL,
    [SoldToCustomerID]  CHAR (10) COLLATE SQL_Latin1_General_CP1_CS_AS       NOT NULL,
    [OrderUniqueID]     UNIQUEIDENTIFIER NOT NULL,
    [FulfillmentStatus] TINYINT          NOT NULL,
    [ResponseData]      NVARCHAR (MAX)   NULL,
	[Tags] [nvarchar](200) NULL,
    [CreatedDateUTC]       DATETIME         NOT NULL,
    [ModifiedDateUTC]      DATETIME         NOT NULL
);





