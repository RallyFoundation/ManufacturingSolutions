param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

#Find and kill process:

$serviceRootDirectory = $applicationRoot + "Configuration";

$processes = Get-Process -Name "ConfigurationProtectedTool" -ErrorAction SilentlyContinue;

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

#Delete programs menu shortcuts:

$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution\" + $installationType.ToString() + "\LocalConfigurationTools\";

Remove-Item $programsFolderPath -Recurse -Force;

