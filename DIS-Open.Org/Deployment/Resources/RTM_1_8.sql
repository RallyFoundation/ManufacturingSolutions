IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataUpdateReport]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataUpdateReport](
	[MSUpdateUniqueID] [uniqueidentifier] NULL,
	[CustomerUpdateUniqueID] [uniqueidentifier] NOT NULL,
	[MSReceivedDateUTC] [datetime] NULL,
	[SoldToCustomerID] [nvarchar](10) NOT NULL,
	[ReceivedFromCustomerID] [nvarchar](10) NOT NULL,
	[TotalLineItems] [int] NULL,
	[OHRStatus] [int] NOT NULL,
	[CreatedDateUTC] [datetime] NOT NULL,
	[ModifiedDateUTC] [datetime] NOT NULL,
	CONSTRAINT [PK_ OHRDataUpdate] PRIMARY KEY CLUSTERED 
(
	[CustomerUpdateUniqueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataUpdateReportKey]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataUpdateReportKey](
	[CustomerUpdateUniqueID] [uniqueidentifier] NOT NULL,
	[ProductKeyID] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](80) NOT NULL,
	[ReasonCode] [nvarchar](4) NULL,
	[ReasonCodeDescription] [nvarchar](160) NULL,
 CONSTRAINT [PK_OHRDataUpdateKey] PRIMARY KEY CLUSTERED 
(
	[CustomerUpdateUniqueID] ASC,
	[ProductKeyID] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate] FOREIGN KEY([CustomerUpdateUniqueID])
REFERENCES [dbo].[DataUpdateReport] ([CustomerUpdateUniqueID])


ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate]


ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])


ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo]
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.KeyState'),'KeyState','PRECISION') = 20
BEGIN
        ALTER TABLE KeyState  
        ALTER COLUMN [KeyState] nvarchar(30) NOT NULL
		INSERT [dbo].[KeyState] ([KeyStateId],[KeyState])  VALUES (13, N'ActivationEnabledPendingUpdate')
END
GO