ALTER TABLE [dbo].[HardwareBindingReport]
    ADD CONSTRAINT [FK_HardwareBindingReport_ComputerBuildReport] FOREIGN KEY ([CustomerReportUniqueID]) REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

