﻿ALTER TABLE [dbo].[UserHeadQuarter]
    ADD CONSTRAINT [FK_UserHeadQuarter_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

