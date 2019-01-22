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

if([System.String]::IsNullOrEmpty($TransactionID) -eq $true)
{
    $TransactionID = [System.Guid]::NewGuid().ToString();
}

$LogPath = $RootDir +  "\Log";
if([System.IO.Directory]::Exists($LogPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($LogPath);
	Start-Sleep -Milliseconds 1000;
}


#cd ($RootDir + "\Script");

#.\TurnOnFirewallSettings.ps1;

#.\AddRegistryKey4WorkGroupAccess.ps1;

#.\InvokeVamtAPI.ps1 -TransactionID $TransactionID -RootDir $RootDir;

#.\RemoveRegistryKey4WorkGroupAccess.ps1;

#.\TurnOffFirewallSettings.ps1;


#Turn On Firewall Settings:
#netsh advfirewall firewall set rule group = "Windows Management Instrumentation (WMI)" new enable = yes;
#New-NetFirewallRule -Group "Windows Management Instrumentation (WMI)" -DisplayName "Windows Management Instrumentation (WMI)" -Enabled True; 
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (Async-in)" new remoteip = any enable = yes;
#New-NetFirewallRule -Name "Windows Management Instrumentation (Async-in)" -DisplayName "Windows Management Instrumentation (Async-in)" -RemoteAddress Any -Enabled True;
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (DCOM-in)" new remoteip = any enable = yes;
#New-NetFirewallRule -Name "Windows Management Instrumentation (DCOM-in)" -DisplayName "Windows Management Instrumentation (DCOM-in)" -RemoteAddress Any -Enabled True;
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (WMI-in)" new remoteip = any enable = yes;
#New-NetFirewallRule -Name "Windows Management Instrumentation (WMI-in)" -DisplayName "Windows Management Instrumentation (WMI-in)" -RemoteAddress Any -Enabled True;
#netsh advfirewall firewall set rule group = "File and Printer Sharing" new enable = yes;
#New-NetFirewallRule -Group "File and Printer Sharing" -DisplayName "File and Printer Sharing" -Enabled True;
#netsh advfirewall firewall set rule group = "Remote Administration" new enable = yes;
#New-NetFirewallRule -Group "Remote Administration" -DisplayName "Remote Administration" -Enabled True;

#Add Registry Key for Work Group Access:
#New-ItemProperty -Path Registry::HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system -Name LocalAccountTokenFilterPolicy -PropertyType DWord -Value 1;

#Invoke VAMT API:
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


#Remove Registry Key for Work Group Access:
#Remove-ItemProperty -Path Registry::HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system -Name LocalAccountTokenFilterPolicy;

#Turn Off Firewall Settings:
#netsh advfirewall firewall set rule group = "Windows Management Instrumentation (WMI)" new enable = no;
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (Async-in)" new remoteip = localip enable = no;
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (DCOM-in)" new remoteip = localip enable = no;
#netsh advfirewall firewall set rule name = "Windows Management Instrumentation (WMI-in)" new remoteip = localip enable = no;
#netsh advfirewall firewall set rule group = "File and Printer Sharing" new enable = no;
#netsh advfirewall firewall set rule group = "Remote Administration" new enable = no;

#Set-NetFirewallRule -Group "Windows Management Instrumentation (WMI)" -Enabled False; 
#Set-NetFirewallRule -Name "Windows Management Instrumentation (Async-in)" -RemoteAddress 127.0.0.1 -Enabled False;
#Set-NetFirewallRule -Name "Windows Management Instrumentation (DCOM-in)" -RemoteAddress 127.0.0.1 -Enabled False;
#Set-NetFirewallRule -Name "Windows Management Instrumentation (WMI-in)" -RemoteAddress 127.0.0.1 -Enabled False;
#Set-NetFirewallRule -Group "File and Printer Sharing" -Enabled False;
#Set-NetFirewallRule -Group "Remote Administration" -Enabled False;

#Remove-NetFirewallRule -DisplayName "Windows Management Instrumentation (WMI)";
#Remove-NetFirewallRule -DisplayName "Windows Management Instrumentation (Async-in)";
#Remove-NetFirewallRule -DisplayName "Windows Management Instrumentation (DCOM-in)";
#Remove-NetFirewallRule -DisplayName "Windows Management Instrumentation (WMI-in)";
#Remove-NetFirewallRule -DisplayName "File and Printer Sharing";
#Remove-NetFirewallRule -DisplayName "Remote Administration";

#Ending:
[System.DateTime]$EndTime = [System.DateTime]::Now;
[System.TimeSpan]$TimeSpan = $EndTime.Subtract($StartTime);
$Message = ("Total time spent: {0} seconds ({1} minutes)." -f $TimeSpan.TotalSeconds, $TimeSpan.TotalMinutes);
$Host.UI.RawUI.BackgroundColor = "Yellow";
$Host.UI.RawUI.ForegroundColor = "Green";
Write-Host -Object $Message;