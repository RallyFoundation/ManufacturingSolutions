ALTER TABLE [dbo].[ComputerBuildReportKey]
    ADD CONSTRAINT [FK_ComputerBuildReportKey_ComputerBuildReport] FOREIGN KEY ([CustomerReportUniqueID]) REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

