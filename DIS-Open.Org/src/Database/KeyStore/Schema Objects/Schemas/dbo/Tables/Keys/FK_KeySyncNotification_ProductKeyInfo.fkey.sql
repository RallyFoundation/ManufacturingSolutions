ALTER TABLE [dbo].[KeySyncNotification]
    ADD CONSTRAINT [FK_KeySyncNotification_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

