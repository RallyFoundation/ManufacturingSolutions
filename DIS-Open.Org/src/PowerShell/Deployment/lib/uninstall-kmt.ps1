param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

#Find and kill process:

$serviceRootDirectory = $applicationRoot + "KeyManagementTool";

$processes = Get-Process -Name "DIS.Presentation.KMT" -ErrorAction SilentlyContinue;

$processes;

if($processes -ne $null)
{
   foreach($process in $processes)
   {
	   $process;

	   [System.String]$processHome = [System.IO.Path]::GetDirectoryName($process.Path);

	   if($processHome.ToLower() -eq $serviceRootDirectory.ToLower())
	   {
		  $process.Kill();
	   }
   }
}

#Delete source files from installation home:

Remove-Item $serviceRootDirectory -Recurse -Force;

#Delete desktop shortcuts:

$desktop = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop);

$desktopKMTShortcutPath = [System.String]::Format("{0}\KMT - {1}.lnk", $desktop, $installationType.ToString());

#$desktopCloudShortcutPath = [System.String]::Format("{0}\DIS Configuration Cloud - {1}.lnk", $desktop, $installationType.ToString());

if([System.IO.File]::Exists($desktopKMTShortcutPath) -eq  $true)
{
	Remove-Item $desktopKMTShortcutPath -Force;
}

#Remove-Item $desktopCloudShortcutPath -Force;

#Delete programs menu shortcuts:

$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution\" + $installationType.ToString();

$programsFolderKMTShortcutPath = [System.String]::Format("{0}\KMT - {1}.lnk", $programsFolderPath, $installationType.ToString());

#$programsFolderCloudShortcutPath = [System.String]::Format("{0}\DIS Configuration Cloud - {1}.lnk", $programsFolderPath, $installationType.ToString());

if([System.IO.File]::Exists($programsFolderKMTShortcutPath) -eq  $true)
{
	Remove-Item $programsFolderKMTShortcutPath -Force;
}

#Remove-Item $programsFolderCloudShortcutPath -Force;