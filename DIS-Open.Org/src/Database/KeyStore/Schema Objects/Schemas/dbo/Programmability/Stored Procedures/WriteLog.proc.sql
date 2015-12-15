CREATE PROCEDURE [dbo].[WriteLog]
(
	@eventID int, 
	@priority int, 
	@severity nvarchar(32), 
	@title nvarchar(256), 
	@timestamp datetime,
	@machineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@message nvarchar(1500),
	@formattedmessage ntext,
	@LogId int OUTPUT
)
AS 

	INSERT INTO [Log] (
		EventID,
		Priority,
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		Message,
		FormattedMessage
	)
	VALUES (
		@eventID, 
		@priority, 
		@severity, 
		@title, 
		@timestamp,
		@machineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@message,
		@formattedmessage)

	SET @LogId = @@IDENTITY
	RETURN @LogId