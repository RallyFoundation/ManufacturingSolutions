USE [aspnetdb]
GO

DECLARE @RC int
DECLARE @ApplicationName nvarchar(256)
DECLARE @UserName nvarchar(256)
DECLARE @Password nvarchar(128)
DECLARE @PasswordSalt nvarchar(128)
DECLARE @Email nvarchar(256)
DECLARE @PasswordQuestion nvarchar(256)
DECLARE @PasswordAnswer nvarchar(128)
DECLARE @IsApproved bit
DECLARE @CurrentTimeUtc datetime
DECLARE @CreateDate datetime
DECLARE @UniqueEmail int
DECLARE @PasswordFormat int
DECLARE @UserId uniqueidentifier

-- TODO: Set parameter values here.
SET @ApplicationName='DISConfigurationCloud'; 
SET @UserName='DIS';
SET @Password='l7oqz9/3Z4DTP0R52UeU5569XKI=';
SET @PasswordSalt='uo3SR95UqgOnTQSLuFDfWg==';
SET @PasswordFormat=1;
SET @PasswordQuestion='Where was you born?';
SET @PasswordAnswer='Microsoft';
SET @Email='DIS@Microsoft.com';
SET @IsApproved=1;
SET @CurrentTimeUtc=GETDATE();
SET @CreateDate=GETDATE();

EXECUTE @RC = [dbo].[aspnet_Membership_CreateUser] 
   @ApplicationName
  ,@UserName
  ,@Password
  ,@PasswordSalt
  ,@Email
  ,@PasswordQuestion
  ,@PasswordAnswer
  ,@IsApproved
  ,@CurrentTimeUtc
  ,@CreateDate
  ,@UniqueEmail
  ,@PasswordFormat
  ,@UserId OUTPUT
GO


