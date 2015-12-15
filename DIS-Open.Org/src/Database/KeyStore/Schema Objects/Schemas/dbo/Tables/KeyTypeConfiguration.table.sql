CREATE TABLE [dbo].[KeyTypeConfiguration]
(
	[KeyTypeConfigurationId] INT IDENTITY (1, 1) NOT NULL,
	[HeadQuarterId]        INT Null,
    [LicensablePartNumber] NVARCHAR (16)  NOT NULL,
    [Maximum]              INT            NULL,
    [Minimum]              INT            NULL,
    [KeyType]              INT            NULL
)
