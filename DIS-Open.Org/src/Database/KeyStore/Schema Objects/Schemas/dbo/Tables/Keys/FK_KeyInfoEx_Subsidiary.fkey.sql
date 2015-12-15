ALTER TABLE [dbo].[KeyInfoEx]  ADD  CONSTRAINT [FK_KeyInfoEx_Subsidiary] FOREIGN KEY([SSID])
REFERENCES [dbo].[Subsidiary] ([SSID])



