ALTER TABLE [dbo].[ReturnReportKey]
    ADD CONSTRAINT [FK_ReturnReportKey_ReturnReport] FOREIGN KEY ([ReturnUniqueID]) REFERENCES [dbo].[ReturnReport] ([ReturnUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

