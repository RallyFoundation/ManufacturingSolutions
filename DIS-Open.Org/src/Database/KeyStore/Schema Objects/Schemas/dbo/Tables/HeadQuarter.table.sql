CREATE TABLE [dbo].[HeadQuarter] (
    [HeadQuarterID]  [INT]            IDENTITY (1, 1) NOT NULL,
    [DisplayName] [NVARCHAR](20)  NULL,
	[CertSubject] [NVARCHAR](128) NULL,
	[CertThumbPrint] [NVARCHAR] (128) NULL,
    [ServiceHostUrl] [NVARCHAR](200)  NULL,
    [UserName]       [NVARCHAR](10)   COLLATE SQL_Latin1_General_CP1_CS_AS  NULL,
    [AccessKey]      [NVARCHAR](50)   NULL,
    [Description]    [NVARCHAR](50)   NULL,
	[IsCentralizedMode]    BIT              NOT NULL,
	[IsCarbonCopy]   BIT              NOT NULL
);

