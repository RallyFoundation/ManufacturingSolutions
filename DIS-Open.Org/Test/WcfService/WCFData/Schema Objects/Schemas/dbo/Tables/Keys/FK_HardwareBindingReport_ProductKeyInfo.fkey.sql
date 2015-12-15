ALTER TABLE [dbo].[HardwareBindingReport]
    ADD CONSTRAINT [FK_HardwareBindingReport_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

