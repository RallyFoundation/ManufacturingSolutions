CREATE TABLE [dbo].[Table_5] (
    [Column01] INT          IDENTITY (1, 1) NOT NULL,
    [Column02] VARCHAR (50) NOT NULL,
    [Column03] VARCHAR (50) NULL,
    [Column04] VARCHAR (50) NULL,
    [Column05] DATETIME     NULL,
    CONSTRAINT [PK_Table_5] PRIMARY KEY CLUSTERED ([Column01] ASC),
    CONSTRAINT [FK_Table_5_Table_4] FOREIGN KEY ([Column04]) REFERENCES [dbo].[Table_4] ([Column04])
);

