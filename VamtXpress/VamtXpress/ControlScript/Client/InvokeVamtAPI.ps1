#
# InvokeVamtAPI.ps1
#

param([System.String]$TransactionID, [System.String]$RootDir)


if([System.String]::IsNullOrEmpty($RootDir) -eq $true)
{
   $RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

   if($RootDir.EndsWith("\") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
   }

   if($RootDir.ToLower().EndsWith("\script") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.ToLower().LastIndexOf("\script")));
   }
}

if($RootDir.EndsWith("\") -eq $true)
{
  $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

$ErrorActionPreference = "Stop";

[System.DateTime]$StartTime = [System.DateTime]::Now;

$ClientHostName = $env:COMPUTERNAME;
$ClientUserName = $env:USERNAME;

[xml]$ConfigXml = Get-Content -Path ($RootDir + "\Config\config.xml") -Encoding UTF8;

$ConfigXml.InnerXml;

[System.String]$VamtApiServicePoint = $ConfigXml.configurationItems.vamtApiServicePoint;

if($VamtApiServicePoint.EndsWith("/") -eq $false)
{
   $VamtApiServicePoint += "/";
}

$VamtServerUserName= $ConfigXml.configurationItems.vamtServerUserName;
$VamtServerPassword = $ConfigXml.configurationItems.vamtServerPassword;

if([System.String]::IsNullOrEmpty($ConfigXml.configurationItems.vamtClientUserName) -eq $false)
{
	$ClientUserName = $ConfigXml.configurationItems.vamtClientUserName;
}

$ClientPassword = $ConfigXml.configurationItems.vamtClientPassword;

[int]$ClientIdentifierType = $ConfigXml.configurationItems.clientIdentifierType;

$NICName = $ConfigXml.configurationItems.nicName;

$TransactionID = [System.Guid]::NewGuid().ToString();

$ClientID = "";

if($ClientIdentifierType -eq 0)
{
	$ComputerBIOS = Get-CimInstance CIM_BIOSElement;
	$SerialNumber = $ComputerBIOS.SerialNumber;
	$ClientID = $SerialNumber;
}

if($ClientIdentifierType -eq 1)
{
	$MacObject = Get-WmiObject Win32_NetworkAdapter | Where-Object { $_.MacAddress -and $_.Name -eq $NICName} | Select-Object Name, MacAddress; 
	$MacAddress = $MacObject.MacAddress;
	$ClientID = $MacAddress;
}

if([System.String]::IsNullOrEmpty($ClientID))
{
	$ClientID = $ConfigXml.configurationItems.clientIdentifierValue;
}

$ClientID;

[System.String]$UrlRegister = "vamt/register/";

$Body = ConvertFrom-Json -InputObject "{`"Key`":`"`", `"Value`":`"`", `"Data`":`"`", `"TransID`":`"`", `"Time`":`"`", `"Host`":`"`", `"User`":`"`", `"Password`":`"`"}";
$Body.TransID = $TransactionID;
$Body.Key = $ClientID;
$Body.Host = $ClientHostName;
$Body.User = $ClientUserName;
$Body.Password = $ClientPassword;

$Body.Value = [System.String]::Format("VAMTRegister-{0}", [System.Guid]::NewGuid());
$Body.Time = [System.DateTime]::Now;
$BodyJson = ConvertTo-Json -InputObject $Body;
Invoke-RestMethod -Method Post -Uri ($VamtApiServicePoint + $UrlRegister) -Body $BodyJson -ContentType "application/json";


[System.DateTime]$EndTime = [System.DateTime]::Now;
[System.TimeSpan]$TimeSpan = $EndTime.Subtract($StartTime);
$Message = ("Total time spent: {0} seconds ({1} minutes)." -f $TimeSpan.TotalSeconds, $TimeSpan.TotalMinutes);
$Host.UI.RawUI.BackgroundColor = "Yellow";
$Host.UI.RawUI.ForegroundColor = "Green";
Write-Host -Object $Message;