param([System.String]$root = $null);

#if($root -eq $null)
#{
#   $rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;
#}
#else
#{
#   $rootDir = $root;
#}

$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($rootDir.EndsWith("\") -eq $false)
{
   $rootDir = $rootDir + "\";
}

cd $rootDir;

Import-Module ServerManager;

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
  .\lib\ServerConfig-NET351-2K8.ps1;
  .\lib\ServerConfig-WebServerRole-2K8.ps1;

  if($dotnetHome.IndexOf("\Microsoft.NET\Framework64\v4.") -lt 0)
  {
	 Write-Host(".NET Framework 4.0 64-bit is not installed, will install now...");
     #Start-Process -FilePath ($rootDir + "pkg\dotNetFx40_Full_x86_x64.exe") -ArgumentList @("/q",  "/norestart") -Wait;
     Start-Process -FilePath ($rootDir + "pkg\dotNetFx40_Full_x86_x64.exe") -ArgumentList @("/passive",  "/norestart", "/msicl ACCEPTLICENSE=1") -Wait;
  }
  else 
  {
      Write-Host("Registering ASP.NET to .NET Framework 4.0 Runtime (x64)...");
	  C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe /i
	  Write-Host("Done.");

      Write-Host("Registering ASP.NET to .NET Framework 4.0 Runtime (x86)...");
	  C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe /i
	  Write-Host("Done.");
  }
}

if(($osVersion.Minor -eq 2) -or ($osVersion.Minor -eq 3))
{
  .\lib\ServerConfig-NET351-2012.ps1;
  .\lib\ServerConfig-WebServerRole-2012.ps1;
}

Write-Host("Installing Microsoft® SQL Server® 2012 SP1 Native Client...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\sqlncli.msi"), "/qr", "IACCEPTSQLNCLILICENSETERMS=YES") -Wait;

Write-Host("Installing Microsoft® SQL Server® 2012 SP1 Command Line Utilities...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\SqlCmdLnUtils.msi"), "/qr") -Wait;

Write-Host("Installing Microsoft® System CLR Types for Microsoft® SQL Server® 2012 SP1 (64-bit)...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\SQLSysClrTypes.msi"), "/qr") -Wait;

Write-Host("Installing Microsoft® System CLR Types for Microsoft® SQL Server® 2012 SP1 (32-bit)...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\SQLSysClrTypes_x86.msi"), "/qr") -Wait;

Write-Host("Installing Microsoft® SQL Server® 2012 SP1 Shared Management Objects (64-bit)...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\SharedManagementObjects.msi"), "/qr") -Wait;

Write-Host("Installing Microsoft® SQL Server® 2012 SP1 Shared Management Objects (32-bit)...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\SharedManagementObjects_x86.msi"), "/qr") -Wait;

Write-Host("Installing Microsoft® Windows PowerShell Extensions for Microsoft® SQL Server® 2012 SP1 ...");

Start-Process -FilePath msiexec.exe -ArgumentList @(("/i " + $rootDir + "pkg\PowerShellTools.msi"), "/qr") -Wait;

Write-Host("Installing Visual C++ Redistributable Packages for Visual Studio 2013...");

#Start-Process -FilePath ($rootDir + "pkg\vcredist_x86.exe") -ArgumentList @("/q",  "/norestart") -Wait;

Start-Process -FilePath ($rootDir + "pkg\vcredist_x86.exe") -ArgumentList @("/passive",  "/norestart") -Wait;

Write-Host("Installing Entity Framework 4.1...");

Start-Process -FilePath ($rootDir + "pkg\EntityFramework41.exe") -ArgumentList @("/passive",  "/norestart", "/msicl ACCEPTLICENSE=1") -Wait;

.\lib\AddDISOSUser.ps1;
.\lib\SetFirewallExternal.ps1;
.\lib\SetFirewallInternal.ps1;
.\lib\SetFirewallKPS8765.ps1;
.\lib\SetFirewallSQLServer1433.ps1;
.\lib\SetFirewallFPS.ps1;