﻿ALTER TABLE [dbo].[TestParameter]
    ADD CONSTRAINT [FK_TestParameter_Test] FOREIGN KEY ([TestID]) REFERENCES [dbo].[Test] ([TestID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

