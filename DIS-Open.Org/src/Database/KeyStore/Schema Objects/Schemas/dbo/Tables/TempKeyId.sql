CREATE TABLE [dbo].[TempKeyId]
(
	[KeyId] [bigint] NOT NULL,
    [KeyState] [tinyint] NULL,
    [KeyType] [int] NULL,
	 CONSTRAINT [PK_KeyIds] PRIMARY KEY CLUSTERED 
     (
        [KeyId] ASC
     ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)ON [PRIMARY];
