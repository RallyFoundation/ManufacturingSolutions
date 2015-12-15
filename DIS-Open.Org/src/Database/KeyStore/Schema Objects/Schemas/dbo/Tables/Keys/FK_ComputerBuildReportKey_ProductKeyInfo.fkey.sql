ALTER TABLE [dbo].[ComputerBuildReportKey]
    ADD CONSTRAINT [FK_ComputerBuildReportKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

