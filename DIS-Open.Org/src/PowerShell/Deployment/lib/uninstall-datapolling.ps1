param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

#Find and kill process:

$serviceRootDirectory = $applicationRoot + "DataPollingService";

$serviceBinaryPath = ($serviceRootDirectory + "\DIS.Services.DataPolling.exe");

$processes = Get-Process -Name "DIS.Services.DataPolling" -ErrorAction SilentlyContinue;

$processes;

if($processes -ne $null)
{
   foreach($process in $processes)
   {
	   $process;

	   #[System.String]$processHome = [System.IO.Path]::GetDirectoryName($process.Path);

	   if($process.Path.ToLower() -eq $serviceBinaryPath.ToLower())
	   {
		  $process.Kill();
	   }
   }
}

#Unregister Windows service:

#C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u ($serviceRootDirectory + "\DIS.Services.DataPolling.exe");

$serviceName = [System.String]::Format("DataPollingService-{0}", $installationType.ToString());

#Stop-Service -Name  $serviceName -Force -ErrorAction SilentlyContinue;

Uninstall-Service -Name $serviceName;

#Delete source files from installation home:

Remove-Item $serviceRootDirectory -Recurse -Force;
