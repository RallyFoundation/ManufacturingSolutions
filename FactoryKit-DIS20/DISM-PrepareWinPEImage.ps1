param([System.String]$WorkingDir, [System.String]$Architecture = "amd64");

if([System.String]::IsNullOrEmpty($WorkingDir) -eq $true)
{
   Write-Host -Object "Working directory should NOT be empty!";
   Read-Host -Prompt "Press any key to exit...";
   exit;
}

if([System.IO.Directory]::Exists($WorkingDir) -eq $false)
{
   Write-Host -Object "Working directory does NOT exist!";
   Read-Host -Prompt "Press any key to exit...";
   exit;
}

$MoudulePath = "C:\Program Files (x86)\Windows Kits\8.1\Assessment and Deployment Kit\Deployment Tools\amd64\DISM";

$ImagePath = ($WorkingDir + "WindowsPE\amd64\media\sources\boot.wim");

$MountDir =  ($WorkingDir + "WindowsPE\amd64\mount");

$PakageDir = "C:\Program Files (x86)\Windows Kits\8.1\Assessment and Deployment Kit\Windows Preinstallation Environment\amd64\WinPE_OCs\";

if($Architecture.ToLower() -eq "x86")
{
   $MoudulePath = "C:\Program Files (x86)\Windows Kits\8.1\Assessment and Deployment Kit\Deployment Tools\x86\DISM";
   $ImagePath = ($WorkingDir + "WindowsPE\x86\media\sources\boot.wim");
   $MountDir =  ($WorkingDir + "WindowsPE\x86\mount");
   $PakageDir = "C:\Program Files (x86)\Windows Kits\8.1\Assessment and Deployment Kit\Windows Preinstallation Environment\x86\WinPE_OCs\";
}

Import-Module $MoudulePath;

Mount-WindowsImage -ImagePath $ImagePath -Path $MountDir -Index 1;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-WMI.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-NetFx.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-Scripting.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-PowerShell.cab') -Path $MountDir;

Add-WindowsPackage -PackagePath ($PakageDir + 'WinPE-DismCmdlets.cab') -Path $MountDir;

Add-Content -Path ($MountDir + "\Windows\System32\startnet.cmd") -Value "PowerShell -ExecutionPolicy ByPass" -Force;

Dismount-WindowsImage -Path $MountDir -Save;

#Dismount-WindowsImage -Path $MountDir -Discard;