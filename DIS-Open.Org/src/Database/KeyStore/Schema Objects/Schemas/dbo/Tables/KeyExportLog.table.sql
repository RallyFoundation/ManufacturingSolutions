CREATE TABLE [dbo].[KeyExportLog] (
    [ExportLogID]  INT            IDENTITY (1, 1) NOT NULL,
    [ExportTo]     NVARCHAR (20)  NOT NULL,
    [ExportType]   NVARCHAR (20)  NOT NULL,
    [KeyCount]     INT            NOT NULL,
    [FileName]     NVARCHAR (300) NOT NULL,
    [IsEncrypted]  BIT            NOT NULL,
    [FileContent]  XML            NOT NULL,
	[CreateBy]     NVARCHAR (50)  NOT NULL,
	[CreateDate]   DATETIME       NOT NULL,  
);

