
/****** Object:  Table [dbo].[KeyExportLog]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyExportLog](
    [ExportLogID] [int] IDENTITY(1,1) NOT NULL,
    [ExportTo] [nvarchar](20) NOT NULL,
    [ExportType] [nvarchar](20) NOT NULL,
    [KeyCount] [int] NOT NULL,
    [FileName] [nvarchar](300) NOT NULL,
    [IsEncrypted] [bit] NOT NULL,
    [FileContent] [xml] NOT NULL,
    [CreateBy] [nvarchar](50) NOT NULL,
    [CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_KeyExportLog] PRIMARY KEY CLUSTERED 
(
    [ExportLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
    [UserID] [int] IDENTITY(1,1) NOT NULL,
    [Password] [nvarchar](128) NOT NULL,
    [PasswordRev] [int] NOT NULL,
    [Salt] [char](10) NULL,
    [LoginID] [nvarchar](20) NOT NULL,
    [Department] [nvarchar](50) NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](50) NULL,
    [CreateDate] [datetime] NOT NULL,
    [UpdateDate] [datetime] NOT NULL,
    [FirstName] [nvarchar](20) NULL,
    [SecondName] [nvarchar](20) NULL,
    [Position] [nvarchar](20) NULL,
    [Language] [nvarchar](15) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
    [UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_LoginID] ON [dbo].[User] 
(
    [LoginID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([UserID], [Password], [PasswordRev], [Salt], [LoginID], [Department], [Phone], [Email], [CreateDate], [UpdateDate], [FirstName], [SecondName], [Position], [Language]) VALUES (1, N'40bd001563085fc35165329ea1ff5c5ecbdbbeef', 1, NULL, N'Admin', NULL, NULL, NULL, GETDATE(), GETDATE(), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[Subsidiary]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subsidiary](
    [SSID] [int] IDENTITY(1,1) NOT NULL,
    [DisplayName] [nvarchar](20) NULL,
    [ServiceHostUrl] [nvarchar](200) NULL,
    [UserName] [nvarchar](10) NULL,
    [AccessKey] [nvarchar](50) NULL,
    [Type] [nvarchar](20) NOT NULL,
    [Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Subsidiary] PRIMARY KEY CLUSTERED 
(
    [SSID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
    [RoleID] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] [nvarchar](20) NOT NULL,
    [Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
    [RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Role] ON
INSERT [dbo].[Role] ([RoleID], [RoleName], [Description]) VALUES (1, N'Manager', NULL)
INSERT [dbo].[Role] ([RoleID], [RoleName], [Description]) VALUES (2, N'Operator', NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF
/****** Object:  Table [dbo].[Category]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
    [CategoryID] [int] IDENTITY(1,1) NOT NULL,
    [CategoryName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
    [CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (1, N'System')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2, N'Operation')
SET IDENTITY_INSERT [dbo].[Category] OFF

/****** Object:  Table [dbo].[ComputerBuildReport]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComputerBuildReport](
    [MSReportUniqueID] [uniqueidentifier] NULL,
    [CustomerReportUniqueID] [uniqueidentifier] NOT NULL,
    [MSReceivedDateUTC] [datetime] NULL,
    [SoldToCustomerID] [nvarchar](10) NOT NULL,
    [ReceivedFromCustomerID] [nvarchar](10) NOT NULL,
    [CBRAckFileTotal] [int] NULL,
    [CBRAckFileNumber] [int] NULL,
    [CBRStatus] [int] NOT NULL,
    [CreatedDateUTC] [datetime] NOT NULL,
    [ModifiedDateUTC] [datetime] NOT NULL,
 CONSTRAINT [PK_ComputerBuildReport] PRIMARY KEY CLUSTERED 
(
    [CustomerReportUniqueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HeadQuarter]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeadQuarter](
    [HeadQuarterID] [int] IDENTITY(1,1) NOT NULL,
    [DisplayName] [nvarchar](20) NULL,
    [CertSubject] [nvarchar](128) NULL,
    [CertThumbPrint] [nvarchar](128) NULL,
    [ServiceHostUrl] [nvarchar](200) NULL,
    [UserName] [nvarchar](10) NULL,
    [AccessKey] [nvarchar](50) NULL,
    [Description] [nvarchar](50) NULL,
    [IsCentralizedMode] [bit] NOT NULL,
    [IsCarbonCopy] [bit] NOT NULL,
 CONSTRAINT [PK_HeadQuarter] PRIMARY KEY CLUSTERED 
(
    [HeadQuarterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FulfillmentInfo]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FulfillmentInfo](
    [FulfillmentNumber] [char](10) NOT NULL,
    [SoldToCustomerID] [char](10) NOT NULL,
    [OrderUniqueID] [uniqueidentifier] NOT NULL,
    [FulfillmentStatus] [tinyint] NOT NULL,
    [ResponseData] [nvarchar](max) NULL,
    [CreatedDateUTC] [datetime] NOT NULL,
    [ModifiedDateUTC] [datetime] NOT NULL,
	[Tags] [nvarchar](200) NULL,
 CONSTRAINT [PK_FulfillmentInfo] PRIMARY KEY CLUSTERED 
(
    [FulfillmentNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Configuration]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuration](
    [ConfigurationID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Value] [xml] NOT NULL,
    [Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
    [ConfigurationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ReturnReport]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnReport](
    [CustomerReturnUniqueID] [uniqueidentifier] NOT NULL,
    [ReturnUniqueID] [uniqueidentifier] NULL,
    [MSReturnNumber] [nchar](10) NULL,
    [ReturnDateUTC] [datetime] NULL,
    [OEMRMADateUTC] [datetime] NULL,
    [OEMRMANumber] [nvarchar](35) NOT NULL,
    [SoldToCustomerName] [nvarchar](40) NULL,
    [OEMRMADate] [datetime] NOT NULL,
    [SoldToCustomerID] [nvarchar](10) NOT NULL,
    [ReturnNoCredit] [bit] NOT NULL,
    [ReportStatus] [int] NOT NULL,
 CONSTRAINT [PK_ReturnReport] PRIMARY KEY CLUSTERED 
(
    [CustomerReturnUniqueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductKeyInfo](
    [ProductKeyID] [bigint] NOT NULL,
    [ProductKey] [nvarchar](50) NULL,
    [ProductKeyStateID] [tinyint] NOT NULL,
    [ProductKeyState] [nvarchar](30) NULL,
    [HardwareID] [nvarchar](512) NULL,
    [OEMPartNumber] [nvarchar](35) NULL,
    [SoldToCustomerName] [nvarchar](80) NULL,
    [OrderUniqueID] [uniqueidentifier] NULL,
    [SoldToCustomerID] [char](10) NULL,
    [CallOffReferenceNumber] [nvarchar](35) NULL,
    [OEMPONumber] [nvarchar](35) NULL,
    [MSOrderNumber] [nvarchar](10) NULL,
    [LicensablePartNumber] [nvarchar](16) NULL,
    [Quantity] [int] NULL,
    [SKUID] [nvarchar](50) NULL,
    [ReturnReasonCode] [nvarchar](10) NULL,
    [CreatedDate] [datetime] NULL,
    [ModifiedDate] [datetime] NULL,
    [MSOrderLineNumber] [int] NULL,
    [OEMPODateUTC] [datetime] NULL,
    [ShipToCustomerID] [char](10) NULL,
    [ShipToCustomerName] [nvarchar](80) NULL,
    [LicensableName] [nvarchar](40) NULL,
    [OEMPOLineNumber] [nvarchar](6) NULL,
    [CallOffLineNumber] [nvarchar](6) NULL,
    [FulfillmentResendIndicator] [bit] NULL,
    [FulfillmentNumber] [char](10) NULL,
    [FulfilledDateUTC] [datetime] NULL,
    [FulfillmentCreateDateUTC] [datetime] NULL,
    [EndItemPartNumber] [nvarchar](18) NULL,
    [ZPC_MODEL_SKU] [nvarchar](64) NULL,
    [ZMANUF_GEO_LOC] [nvarchar](10) NULL,
    [ZPGM_ELIG_VALUES] [nvarchar](48) NULL,
    [ZOEM_EXT_ID] [nvarchar](16) NULL,
    [ZCHANNEL_REL_ID] [nvarchar](32) NULL,
	[ZFRM_FACTOR_CL1] [nvarchar](64) NULL,
	[ZFRM_FACTOR_CL2] [nvarchar](64) NULL,
	[ZSCREEN_SIZE] [nvarchar](32) NULL,
	[ZTOUCH_SCREEN] [nvarchar](32) NULL,
    [TrackingInfo] [nvarchar](1024) NULL,
	[Tags] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_ProductKeyInfo] PRIMARY KEY CLUSTERED 
(
    [ProductKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_FulfilledDateUTC] ON [dbo].[ProductKeyInfo] 
(
    [FulfilledDateUTC] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_LicensablePartNumber] ON [dbo].[ProductKeyInfo] 
(
    [LicensablePartNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_MSOrderNumber] ON [dbo].[ProductKeyInfo] 
(
    [MSOrderNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_OEMPartNumber] ON [dbo].[ProductKeyInfo] 
(
    [OEMPartNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_OEMPONumber] ON [dbo].[ProductKeyInfo] 
(
    [OEMPONumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductKeyInfo_ProductKey] ON [dbo].[ProductKeyInfo] 
(
    [ProductKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ProductKeyStateID] ON [dbo].[ProductKeyInfo] 
(
    [ProductKeyStateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZCHANNEL_REL_ID] ON [dbo].[ProductKeyInfo] 
(
    [ZCHANNEL_REL_ID] ASC
)
WHERE ([ZCHANNEL_REL_ID] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZMANUF_GEO_LOC] ON [dbo].[ProductKeyInfo] 
(
    [ZMANUF_GEO_LOC] ASC
)
WHERE ([ZMANUF_GEO_LOC] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZOEM_EXT_ID] ON [dbo].[ProductKeyInfo] 
(
    [ZOEM_EXT_ID] ASC
)
WHERE ([ZOEM_EXT_ID] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZPC_MODEL_SKU] ON [dbo].[ProductKeyInfo] 
(
    [ZPC_MODEL_SKU] ASC
)
WHERE ([ZPC_MODEL_SKU] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZPGM_ELIG_VALUES] ON [dbo].[ProductKeyInfo] 
(
    [ZPGM_ELIG_VALUES] ASC
)
WHERE ([ZPGM_ELIG_VALUES] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZFRM_FACTOR_CL1] ON [dbo].[ProductKeyInfo] 
(
    [ZFRM_FACTOR_CL1] ASC
)
WHERE ([ZFRM_FACTOR_CL1] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZFRM_FACTOR_CL2] ON [dbo].[ProductKeyInfo] 
(
    [ZFRM_FACTOR_CL2] ASC
)
WHERE ([ZFRM_FACTOR_CL2] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZSCREEN_SIZE] ON [dbo].[ProductKeyInfo] 
(
    [ZSCREEN_SIZE] ASC
)
WHERE ([ZSCREEN_SIZE] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZTOUCH_SCREEN] ON [dbo].[ProductKeyInfo] 
(
    [ZTOUCH_SCREEN] ASC
)
WHERE ([ZTOUCH_SCREEN] IS NOT NULL)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
    [LogID] [int] IDENTITY(1,1) NOT NULL,
    [EventID] [int] NULL,
    [Priority] [int] NOT NULL,
    [Severity] [nvarchar](32) NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
    [Timestamp] [datetime] NOT NULL,
    [MachineName] [nvarchar](32) NOT NULL,
    [AppDomainName] [nvarchar](512) NOT NULL,
    [ProcessID] [nvarchar](256) NOT NULL,
    [ProcessName] [nvarchar](512) NOT NULL,
    [ThreadName] [nvarchar](512) NULL,
    [Win32ThreadId] [nvarchar](128) NULL,
    [Message] [nvarchar](1500) NULL,
    [FormattedMessage] [ntext] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
    [LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log_Severity] ON [dbo].[Log] 
(
    [Severity] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log_Timestamp] ON [dbo].[Log] 
(
    [Timestamp] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log_Title] ON [dbo].[Log] 
(
    [Title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyTypeConfiguration]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyTypeConfiguration](
    [KeyTypeConfigurationId] [int] IDENTITY(1,1) NOT NULL,
    [HeadQuarterId] [int] NULL,
    [LicensablePartNumber] [nvarchar](16) NOT NULL,
    [Maximum] [int] NULL,
    [Minimum] [int] NULL,
    [KeyType] [int] NULL,
    CONSTRAINT [PK_KeyTypeConfiguration] PRIMARY KEY CLUSTERED 
(
    [KeyTypeConfigurationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_KeyTypeConfiguration_HQID_PartNumber] ON [dbo].[KeyTypeConfiguration]
(
    [HeadQuarterId] ASC,
    [LicensablePartNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeySyncNotification]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeySyncNotification](
    [ProductKeyID] [bigint] NOT NULL,
    [ProductKeyStateID] [tinyint] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_KeySyncNotification] PRIMARY KEY CLUSTERED 
(
    [ProductKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyOperationHistory]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyOperationHistory](
    [OperationID] [int] IDENTITY(1,1) NOT NULL,
    [ProductKeyID] [bigint] NOT NULL,
    [ProductKey] [nvarchar](29) NOT NULL,
    [HardwareHash] [nvarchar](512) NULL,
    [KeyStateFrom] [tinyint] NOT NULL,
    [KeyStateTo] [tinyint] NOT NULL,
    [Operator] [nvarchar](20) NOT NULL,
    [Message] [nvarchar](200) NOT NULL,
    [CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_KeyOperationHistory] PRIMARY KEY CLUSTERED 
(
    [OperationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KeyOperationHistory_ProductKey] ON [dbo].[KeyOperationHistory] 
(
    [ProductKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyInfoEx]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyInfoEx](
    [ProductKeyID] [bigint] NOT NULL,
    [KeyType] [int] NULL,
    [SSID] [int] NULL,
    [HQID] [int] NULL,
    [IsInProgress] [bit] NOT NULL,
    [ShouldCarbonCopy] [bit] NULL,
 CONSTRAINT [PK_KeyInfoEx] PRIMARY KEY CLUSTERED 
(
    [ProductKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_HQID] ON [dbo].[KeyInfoEx] 
(
    [HQID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_IsInProgress] ON [dbo].[KeyInfoEx] 
(
    [IsInProgress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_KeyType] ON [dbo].[KeyInfoEx] 
(
    [KeyType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_SSID] ON [dbo].[KeyInfoEx] 
(
    [SSID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyHistory]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyHistory](
    [ProductKeyID] [bigint] NOT NULL,
    [ProductKeyStateID] [tinyint] NOT NULL,
    [StateChangeDate] [datetime] NOT NULL,
 CONSTRAINT [PK_KeyHistory] PRIMARY KEY CLUSTERED 
(
    [ProductKeyID] ASC,
    [ProductKeyStateID] ASC,
    [StateChangeDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComputerBuildReportKey]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComputerBuildReportKey](
    [CustomerReportUniqueID] [uniqueidentifier] NOT NULL,
    [ProductKeyID] [bigint] NOT NULL,
    [HardwareHash] [nvarchar](512) NOT NULL,
    [OEMOptionalInfo] [xml] NULL,
    [ReasonCode] [nvarchar](2) NULL,
    [ReasonCodeDescription] [nvarchar](160) NULL,
 CONSTRAINT [PK_ComputerBuildReportKey] PRIMARY KEY CLUSTERED 
(
    [CustomerReportUniqueID] ASC,
    [ProductKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryLog]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryLog](
    [CategoryLogID] [int] IDENTITY(1,1) NOT NULL,
    [CategoryID] [int] NOT NULL,
    [LogID] [int] NOT NULL,
 CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED 
(
    [CategoryLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnReportKey]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnReportKey](
    [CustomerReturnUniqueID] [uniqueidentifier] NOT NULL,
    [OEMRMALineNumber] [int] NOT NULL,
    [ReturnTypeID] [nchar](3) NOT NULL,
    [ProductKeyID] [bigint] NOT NULL,
    [MSReturnLineNumber] [int] NULL,
    [LicensablePartNumber] [nvarchar](16) NULL,
    [ReturnReasonCode] [nvarchar](2) NULL,
    [ReturnReasonCodeDescription] [nvarchar](40) NULL,
    [PreProductKeyStateID] [tinyint] NOT NULL,
 CONSTRAINT [PK_ReturnReportKey] PRIMARY KEY CLUSTERED 
(
    [CustomerReturnUniqueID] ASC,
    [ProductKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DuplicatedComputerBuildReport]    Script Date: 03/27/2012 10:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DuplicatedComputerBuildReport](
    [CustomerReportUniqueID] [uniqueidentifier] NOT NULL,
    [IsExported] [bit] NOT NULL,
 CONSTRAINT [PK_DuplicatedComputerBuildReport] PRIMARY KEY CLUSTERED 
(
    [CustomerReportUniqueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DuplicatedComputerBuildReport_IsExported] ON [dbo].[DuplicatedComputerBuildReport] 
(
    [IsExported] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[WriteLog]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[WriteLog]
(
    @eventID int, 
    @priority int, 
    @severity nvarchar(32), 
    @title nvarchar(256), 
    @timestamp datetime,
    @machineName nvarchar(32), 
    @AppDomainName nvarchar(512),
    @ProcessID nvarchar(256),
    @ProcessName nvarchar(512),
    @ThreadName nvarchar(512),
    @Win32ThreadId nvarchar(128),
    @message nvarchar(1500),
    @formattedmessage ntext,
    @LogId int OUTPUT
)
AS 

    INSERT INTO [Log] (
        EventID,
        Priority,
        Severity,
        Title,
        [Timestamp],
        MachineName,
        AppDomainName,
        ProcessID,
        ProcessName,
        ThreadName,
        Win32ThreadId,
        Message,
        FormattedMessage
    )
    VALUES (
        @eventID, 
        @priority, 
        @severity, 
        @title, 
        @timestamp,
        @machineName, 
        @AppDomainName,
        @ProcessID,
        @ProcessName,
        @ThreadName,
        @Win32ThreadId,
        @message,
        @formattedmessage)

    SET @LogId = @@IDENTITY
    RETURN @LogId
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
    [UserID] [int] NOT NULL,
    [RoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
    [UserID] ASC,
    [RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[UserRole] ([UserID], [RoleID]) VALUES (1, 1)
/****** Object:  Table [dbo].[UserHeadQuarter]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserHeadQuarter](
    [UserID] [int] NOT NULL,
    [HeadQuarterID] [int] NOT NULL,
    [IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_UserHeadQuarter] PRIMARY KEY CLUSTERED 
(
    [UserID] ASC,
    [HeadQuarterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[InsertCategoryLog]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsertCategoryLog]
    @CategoryID INT,
    @LogID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CatLogID INT
    SELECT @CatLogID FROM CategoryLog WHERE CategoryID=@CategoryID and LogID = @LogID
    IF @CatLogID IS NULL
    BEGIN
        INSERT INTO CategoryLog (CategoryID, LogID) VALUES(@CategoryID, @LogID)
        RETURN @@IDENTITY
    END
    ELSE RETURN @CatLogID
END
GO
/****** Object:  Table [dbo].[DuplicatedKey]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DuplicatedKey](
    [DuplicatedKeyID] [int] IDENTITY(1,1) NOT NULL,
    [ProductKeyID] [bigint] NOT NULL,
    [ProductKey] [nvarchar](29) NOT NULL,
    [Handled] [bit] NOT NULL,
    [OperationID] [int] NULL,
 CONSTRAINT [PK_DuplicatedKey] PRIMARY KEY CLUSTERED 
(
    [DuplicatedKeyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_DuplicatedKey_Handled] ON [dbo].[DuplicatedKey] 
(
    [Handled] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempKeyId]    Script Date: 10/19/2012 17:02:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TempKeyId](
        [KeyId] [bigint] NOT NULL,
        [KeyState] [tinyint] NULL,
        [KeyType] [int] NULL,
     CONSTRAINT [PK_KeyIds] PRIMARY KEY CLUSTERED 
     (
        [KeyId] ASC
     )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyState]    Script Date: 10/24/2012 10:10:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyState](
	[KeyStateId] [tinyint] NOT NULL,
	[KeyState] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_KeyState] PRIMARY KEY CLUSTERED 
(
	[KeyStateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (1, N'Fulfilled')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (2, N'Consumed')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (3, N'Bound')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (4, N'NotifiedBound')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (5, N'Returned')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (7, N'ReportedBound')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (8, N'ReportedReturn')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (9, N'ActivationEnabled')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (10, N'ActivationDenied')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (11, N'Assigned')
INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (12, N'Retrieved')
INSERT [dbo].[KeyState] ([KeyStateId],[KeyState])  VALUES (13, N'ActivationEnabledPendingUpdate')
GO


/****** Object:  StoredProcedure [dbo].[ClearLogs]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ClearLogs]
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM CategoryLog
    DELETE FROM [Log]
    DELETE FROM Category
END
GO
/****** Object:  StoredProcedure [dbo].[AddCategory]    Script Date: 03/27/2012 10:33:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[AddCategory]
    -- Add the parameters for the function here
    @categoryName nvarchar(64),
    @logID int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CatID INT
    SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @categoryName
    IF @CatID IS NULL
    BEGIN
        INSERT INTO Category (CategoryName) VALUES(@categoryName)
        SELECT @CatID = @@IDENTITY
    END

    EXEC InsertCategoryLog @CatID, @logID 

    RETURN @CatID
END
GO
/****** Object:  Default [DF_User_CreateDate]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreateDate]  DEFAULT (((1900)-(1))-(1)) FOR [CreateDate]
GO
/****** Object:  Default [DF_User_UpdateDate]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_UpdateDate]  DEFAULT (((1900)-(1))-(1)) FOR [UpdateDate]
GO
/****** Object:  ForeignKey [FK_KeySyncNotification_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeySyncNotification]  WITH CHECK ADD  CONSTRAINT [FK_KeySyncNotification_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[KeySyncNotification] CHECK CONSTRAINT [FK_KeySyncNotification_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_KeyOperationHistory_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeyOperationHistory]  WITH CHECK ADD  CONSTRAINT [FK_KeyOperationHistory_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[KeyOperationHistory] CHECK CONSTRAINT [FK_KeyOperationHistory_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_KeyInfoEx_HeadQuarter]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeyInfoEx]  WITH CHECK ADD  CONSTRAINT [FK_KeyInfoEx_HeadQuarter] FOREIGN KEY([HQID])
REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID])
GO
ALTER TABLE [dbo].[KeyInfoEx] CHECK CONSTRAINT [FK_KeyInfoEx_HeadQuarter]
GO
/****** Object:  ForeignKey [FK_KeyInfoEx_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeyInfoEx]  WITH CHECK ADD  CONSTRAINT [FK_KeyInfoEx_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[KeyInfoEx] CHECK CONSTRAINT [FK_KeyInfoEx_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_KeyInfoEx_Subsidiary]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeyInfoEx]  WITH CHECK ADD  CONSTRAINT [FK_KeyInfoEx_Subsidiary] FOREIGN KEY([SSID])
REFERENCES [dbo].[Subsidiary] ([SSID])
GO
ALTER TABLE [dbo].[KeyInfoEx] CHECK CONSTRAINT [FK_KeyInfoEx_Subsidiary]
GO
/****** Object:  ForeignKey [FK_KeyHistory_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[KeyHistory]  WITH CHECK ADD  CONSTRAINT [FK_KeyHistory_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[KeyHistory] CHECK CONSTRAINT [FK_KeyHistory_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_ComputerBuildReportKey_ComputerBuildReport]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[ComputerBuildReportKey]  WITH CHECK ADD  CONSTRAINT [FK_ComputerBuildReportKey_ComputerBuildReport] FOREIGN KEY([CustomerReportUniqueID])
REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID])
GO
ALTER TABLE [dbo].[ComputerBuildReportKey] CHECK CONSTRAINT [FK_ComputerBuildReportKey_ComputerBuildReport]
GO
/****** Object:  ForeignKey [FK_ComputerBuildReportKey_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[ComputerBuildReportKey]  WITH CHECK ADD  CONSTRAINT [FK_ComputerBuildReportKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[ComputerBuildReportKey] CHECK CONSTRAINT [FK_ComputerBuildReportKey_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_CategoryLog_Category]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[CategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[CategoryLog] CHECK CONSTRAINT [FK_CategoryLog_Category]
GO
/****** Object:  ForeignKey [FK_CategoryLog_Log]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[CategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY([LogID])
REFERENCES [dbo].[Log] ([LogID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryLog] CHECK CONSTRAINT [FK_CategoryLog_Log]
GO
/****** Object:  ForeignKey [FK_ReturnReportKey_ProductKeyInfo]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[ReturnReportKey]  WITH CHECK ADD  CONSTRAINT [FK_ReturnReportKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[ReturnReportKey] CHECK CONSTRAINT [FK_ReturnReportKey_ProductKeyInfo]
GO
/****** Object:  ForeignKey [FK_ReturnReportKey_ReturnReport]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[ReturnReportKey]  WITH CHECK ADD  CONSTRAINT [FK_ReturnReportKey_ReturnReport] FOREIGN KEY([CustomerReturnUniqueID])
REFERENCES [dbo].[ReturnReport] ([CustomerReturnUniqueID])
GO
ALTER TABLE [dbo].[ReturnReportKey] CHECK CONSTRAINT [FK_ReturnReportKey_ReturnReport]
GO
/****** Object:  ForeignKey [FK_DuplicatedComputerBuildReport_ComputerBuildReport]    Script Date: 03/27/2012 10:33:33 ******/
ALTER TABLE [dbo].[DuplicatedComputerBuildReport]  WITH CHECK ADD  CONSTRAINT [FK_DuplicatedComputerBuildReport_ComputerBuildReport] FOREIGN KEY([CustomerReportUniqueID])
REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID])
GO
ALTER TABLE [dbo].[DuplicatedComputerBuildReport] CHECK CONSTRAINT [FK_DuplicatedComputerBuildReport_ComputerBuildReport]
GO
/****** Object:  ForeignKey [FK_UserRole_Role]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
/****** Object:  ForeignKey [FK_UserRole_User]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
/****** Object:  ForeignKey [FK_UserHeadQuarter_HeadQuarter]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[UserHeadQuarter]  WITH CHECK ADD  CONSTRAINT [FK_UserHeadQuarter_HeadQuarter] FOREIGN KEY([HeadQuarterID])
REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID])
GO
ALTER TABLE [dbo].[UserHeadQuarter] CHECK CONSTRAINT [FK_UserHeadQuarter_HeadQuarter]
GO
/****** Object:  ForeignKey [FK_UserHeadQuarter_User]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[UserHeadQuarter]  WITH CHECK ADD  CONSTRAINT [FK_UserHeadQuarter_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[UserHeadQuarter] CHECK CONSTRAINT [FK_UserHeadQuarter_User]
GO
/****** Object:  ForeignKey [FK_DuplicatedKey_KeyOperationHistory]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[DuplicatedKey]  WITH CHECK ADD  CONSTRAINT [FK_DuplicatedKey_KeyOperationHistory] FOREIGN KEY([OperationID])
REFERENCES [dbo].[KeyOperationHistory] ([OperationID])
GO
ALTER TABLE [dbo].[DuplicatedKey] CHECK CONSTRAINT [FK_DuplicatedKey_KeyOperationHistory]
GO
/****** Object:  ForeignKey [FK_DuplicatedKey_ProductKeyInfo]    Script Date: 03/27/2012 10:33:35 ******/
ALTER TABLE [dbo].[DuplicatedKey]  WITH CHECK ADD  CONSTRAINT [FK_DuplicatedKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO
ALTER TABLE [dbo].[DuplicatedKey] CHECK CONSTRAINT [FK_DuplicatedKey_ProductKeyInfo]
GO
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

GO

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

GO

ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate] FOREIGN KEY([CustomerUpdateUniqueID])
REFERENCES [dbo].[DataUpdateReport] ([CustomerUpdateUniqueID])
GO

ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_OHRDataUpdate]
GO

ALTER TABLE [dbo].[DataUpdateReportKey]  WITH NOCHECK ADD  CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])
GO

ALTER TABLE [dbo].[DataUpdateReportKey] CHECK CONSTRAINT [FK_OHRDataUpdateKey_ProductKeyInfo]
GO
