ALTER TABLE [dbo].[ReturnReportKey]
    ADD CONSTRAINT [FK_ReturnReportKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

