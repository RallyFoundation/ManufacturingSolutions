ALTER TABLE [dbo].[ReturnReportKey]
    ADD CONSTRAINT [FK_ReturnReportKey_ReturnReport] FOREIGN KEY ([CustomerReturnUniqueID]) REFERENCES [dbo].[ReturnReport] ([CustomerReturnUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

