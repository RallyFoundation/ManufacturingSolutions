ALTER TABLE [dbo].[DuplicatedKey]
    ADD CONSTRAINT [FK_DuplicatedKey_KeyOperationHistory] FOREIGN KEY ([OperationID]) REFERENCES [dbo].[KeyOperationHistory] ([OperationID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

