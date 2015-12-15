ALTER TABLE [dbo].[KeyHistory]
    ADD CONSTRAINT [FK_KeyHistory_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

