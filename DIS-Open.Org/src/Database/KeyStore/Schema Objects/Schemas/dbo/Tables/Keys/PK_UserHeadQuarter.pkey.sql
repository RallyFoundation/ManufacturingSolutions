﻿ALTER TABLE [dbo].[UserHeadQuarter]
    ADD CONSTRAINT [PK_UserHeadQuarter] PRIMARY KEY CLUSTERED ([UserID] ASC, [HeadQuarterID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

