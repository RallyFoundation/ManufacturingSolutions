﻿ALTER TABLE [dbo].[KeyHistory]
    ADD CONSTRAINT [PK_KeyHistory] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC, [ProductKeyStateID] ASC, [StateChangeDate] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

