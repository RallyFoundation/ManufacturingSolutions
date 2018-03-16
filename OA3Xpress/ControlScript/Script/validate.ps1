param([System.String]$RootDir, [System.String]$Architecture = "x86");

$ErrorActionPreference = "Stop";

#Checking PowerShell version and CLR (.NET Framework) version:
$PowerShellVersionInfo = $PSVersionTable;

$PowerShellVersionInfo;

if($OutResult -ne $null)
{
   $OutResult.Value = "Unknown";
}

if($PowerShellVersionInfo.PSVersion.Major -lt 5)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "PowerShell 5.1 is required!";
   Read-Host -Prompt "PowerShell 5.1 is required!`nPress any key to exit...";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

if($PowerShellVersionInfo.CLRVersion.Major -lt 4)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object ".NET Framework 4.52 is required!";
   Read-Host -Prompt ".NET Framework 4.52 is required!`nPress any key to exit...";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

#if($RootDir -eq $null)
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

#$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

#if([System.String]::IsNullOrEmpty($RootDir) -eq $true)
#{
#   $RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

#   if($RootDir.EndsWith("\") -eq $true)
#   {
#      $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
#   }

#   if($RootDir.ToLower().EndsWith("\scripts") -eq $true)
#   {
#      $RootDir = $RootDir.Substring(0, ($RootDir.ToLower().LastIndexOf("\scripts")));
#   }
#}

#if($RootDir.EndsWith("\") -eq $true)
#{
#  $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
#}

#$OA3ToolPath = $RootDir + "\OA3Tool\x86\oa3tool.exe";

#if($Architecture.ToLower() -eq "amd64")
#{
#   $OA3ToolPath = $RootDir + "\OA3Tool\amd64\oa3tool.exe";
#}

$OSInfo = Get-CimInstance -ClassName Win32_OperatingSystem;

$OSInfo;

$OSInfoJson = $OSInfo | ConvertTo-Json;

$OSInfoJson;

[System.String]$OSArchitecture = $OSInfo.CimInstanceProperties.Item("OSArchitecture").Value;

$OSArchitecture;

$OA3ToolPath = $RootDir + "\OA3Tool";

if(($OSArchitecture -eq "64-bit") -or ($OSArchitecture.Contains("64")))
{
	$OA3ToolPath += "\amd64\oa3tool.exe"; 
}
else
{
	$OA3ToolPath += "\x86\oa3tool.exe"; 
}

#Invokes OA3Tool.exe /Validate to test if there is already a DPK injected
try
{
    $Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
    $Message;
       
    $Message = & ($OA3ToolPath) @("/Validate");
    $Message;

    if($Message.Contains("The operation completed successfully.") -eq $false)
    {

       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "The board has NOT got a DPK injected, and the ACPI MSDM table is empty!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
    }
    else
    {
       Write-Host -Object "OK.";
    }
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}


#Invokes OA3Tool.exe /CheckEdition /Online to test if the DPK injected matches the edition of OS
try
{
    $Message = [System.String]::Format("Matching DPK with OS edition..., {0}", [System.DateTime]::Now);
    $Message;
       
    $Message = & ($OA3ToolPath) @("/CheckEdition", "/Online");
    $Message;

    Write-Host -Object "OK.";
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred during DPK / OS edition verification!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

#Invokes slmgr.vbs /dlv
try
{
    $Message = [System.String]::Format("Invoking slmgr.vbs /dlv..., {0}", [System.DateTime]::Now);
    $Message;
       
    $Message = & ("slmgr.vbs") @("/dlv");
    $Message;

    Write-Host -Object "OK.";
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred during slmgr.vbs invocation!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}