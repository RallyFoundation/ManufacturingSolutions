ALTER TABLE [dbo].[KeyInfoEx]  ADD  CONSTRAINT [FK_KeyInfoEx_ProductKeyInfo] FOREIGN KEY([ProductKeyID])
REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID])


