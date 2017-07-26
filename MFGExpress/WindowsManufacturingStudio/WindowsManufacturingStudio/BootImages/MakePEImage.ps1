param([System.String]$Architecture = "x86", [System.String]$ImageType="multicast", [System.String]$OutputDir, [System.String]$PEScriptDir);

$RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($RootDir.EndsWith("\") -eq $true)
{
   $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

if($PEScriptDir.EndsWith("\") -eq $true)
{
   $PEScriptDir = $PEScriptDir.Substring(0, ($PEScriptDir.Length -1));
}

#$WindowsADKDTPath = "C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\";

#$WindowsADKPEPath = "C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\";

#cd $WindowsADKDTPath;

#.\DandISetEnv.bat;

#cd $WindowsADKPEPath;

#.\copype.cmd $Architecture $OutputDir;

cd $OutputDir;

$MountDir =  ".\mount";

Mount-WindowsImage -ImagePath .\media\sources\boot.wim -Path $MountDir -Index 1;

Copy-Item -Path ($PEScriptDir + "\run.ps1") .\mount\windows\system32\run.ps1 -Force;
Copy-Item -Path ($PEScriptDir + "\startnet.cmd") .\mount\windows\system32 -Force;
Copy-Item -Path ($PEScriptDir + "\diskpartcmd.txt") .\mount\windows\system32 -Force;
Copy-Item -Path ($PEScriptDir + "\config.xml") .\mount\windows\system32 -Force;

$PakageDir = "C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\amd64\WinPE_OCs\";

if($Architecture -eq "x86")
{
   $PakageDir = "C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\";
}

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-WMI.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-NetFx.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-Scripting.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-PowerShell.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-DismCmdlets.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-WDS-Tools.cab') -Path $MountDir;

Dismount-WindowsImage -Path $MountDir -Save;

Dismount-WindowsImage -Path $MountDir -Discard;