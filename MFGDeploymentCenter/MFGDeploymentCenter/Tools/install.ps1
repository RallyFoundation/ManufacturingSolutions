param([System.String]$unattend = $null, [System.String]$root = $null);


$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($rootDir.EndsWith("\") -eq $false)
{
   $rootDir = $rootDir + "\";
}

cd $rootDir;

$osVersion = [System.Environment]::OSVersion.Version;

$is64BitSystem = [System.Environment]::Is64BitOperatingSystem;

$dotnetHome = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory();

if($is64BitSystem -eq  $false)
{
  Write-Host("The OS is not a 64-bit edition, please use 64-bit based system for the installation!");

  Exit;
}

if($osVersion.Major -lt 6)
{
   Write-Host("Windows Server 2008 (R2) / Windows Server 2012 (R) is required  for the installation!");
   Exit;
}

if($osVersion.Minor -eq 1)
{
    .\install-w2k8.ps1 -unattend $unattend;
}

if(($osVersion.Minor -eq 2) -or ($osVersion.Minor -eq 3))
{
  .\install-w2012.ps1 -unattend $unattend;
}