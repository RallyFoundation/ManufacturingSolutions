#
# cleanup.ps1
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


#Remove Registry Key for Work Group Access:
Remove-ItemProperty -Path Registry::HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system -Name LocalAccountTokenFilterPolicy

#Turn Off Firewall Settings:
netsh advfirewall firewall set rule group = "Windows Management Instrumentation (WMI)" new enable = no

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (Async-in)" new remoteip = any enable = no

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (DCOM-in)" new remoteip = any enable = no

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (WMI-in)" new remoteip = any enable = no

netsh advfirewall firewall set rule group = "File and Printer Sharing" new enable = no

netsh advfirewall firewall set rule group = "Remote Administration" new enable = no

#Unjoin Domain:
$ClientHostName = $env:COMPUTERNAME;
$ClientUserName = $env:USERNAME;
$LocalCredential = [System.String]::Format("[0}\{1}", $ClientHostName, $ClientUserName);

[xml]$ConfigXml = Get-Content -Path ($RootDir + "\Config\config.xml") -Encoding UTF8;
$ConfigXml.InnerXml;

[System.String]$DomainName = $ConfigXml.vamtDomainName;
$DomainName = $DomainName.Substring(0, $DomainName.IndexOf("."));
[System.String]$DomainUserName = $ConfigXml.vamtDomainUserName;
[System.String]$DomainPassword =  $ConfigXml.vamtDomainPassword;
$DomainSecuredPassword = ConvertTo-SecureString $DomainPassword -AsPlainText -Force;
$DomainCredential = New-Object System.Management.Automation.PSCredential($DomainUserName, $DomainSecuredPassword);

Remove-Computer -ComputerName $ClientHostName -UnJoinDomainCredential $DomainCredential -PassThru -Verbose -Restart -Force;