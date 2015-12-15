ALTER TABLE [dbo].[DuplicatedComputerBuildReport]
    ADD CONSTRAINT [FK_DuplicatedComputerBuildReport_ComputerBuildReport] FOREIGN KEY ([CustomerReportUniqueID]) REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

