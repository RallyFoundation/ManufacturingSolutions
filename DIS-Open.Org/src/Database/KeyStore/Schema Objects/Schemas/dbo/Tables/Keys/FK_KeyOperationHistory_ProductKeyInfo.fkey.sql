ALTER TABLE [dbo].[KeyOperationHistory]
    ADD CONSTRAINT [FK_KeyOperationHistory_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

