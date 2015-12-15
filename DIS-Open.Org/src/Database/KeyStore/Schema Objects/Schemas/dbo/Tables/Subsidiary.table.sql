CREATE TABLE [dbo].[Subsidiary](
    [SSID]            [INT]          IDENTITY(1,1)  NOT NULL,
    [DisplayName]  [NVARCHAR](20) NULL,
    [ServiceHostUrl]  [NVARCHAR](200)  NULL,
	[UserName]		  [NVARCHAR](10)   COLLATE SQL_Latin1_General_CP1_CS_AS  NULL,
	[AccessKey]       [NVARCHAR](50) NULL,
    [Type]            [NVARCHAR](20) NOT NULL,
    [Description]     [NVARCHAR](50) NULL
);


