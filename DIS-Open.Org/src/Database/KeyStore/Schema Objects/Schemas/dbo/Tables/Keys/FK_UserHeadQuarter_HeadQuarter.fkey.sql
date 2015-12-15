ALTER TABLE [dbo].[UserHeadQuarter]
    ADD CONSTRAINT [FK_UserHeadQuarter_HeadQuarter] FOREIGN KEY ([HeadQuarterID]) REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

