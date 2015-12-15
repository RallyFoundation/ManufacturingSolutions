ALTER TABLE [dbo].[DuplicatedKey]
    ADD CONSTRAINT [FK_DuplicatedKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

