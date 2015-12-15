/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\ScriptPopulateTable_1.sql
:r .\ScriptPopulateTable_2.sql
:r .\ScriptPopulateTable_3.sql
:r .\ScriptPopulateTable_4.sql
:r .\ScriptPopulateTable_5.sql
