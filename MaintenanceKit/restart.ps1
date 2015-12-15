#Stopping DPS
Write-Host -Object "Stopping Data Polling Service...";

$dpsProcesses = Get-Process -Name "DIS.Services.DataPolling" -ErrorAction SilentlyContinue;

$dpsProcesses;

if($dpsProcesses-ne $null)
{
   foreach($process in $dpsProcesses)
   {
	   $process.Kill();
   }
}

Write-Host -Object "Done.";


#Stopping KPS
Write-Host -Object "Stopping Key Provider Service...";

$kpsProcesses = Get-Process -Name "DIS.Services.KeyProviderService" -ErrorAction SilentlyContinue;

$kpsProcesses;

if($kpsProcesses -ne $null)
{
   foreach($process in $kpsProcesses)
   {
	   $process.Kill();
   }
}

Write-Host -Object "Done.";


#Stopping WWW
Write-Host -Object "Stopping WWW Service...";

Stop-Service -Name W3SVC -Force -ErrorAction SilentlyContinue;

Write-Host -Object "Done.";


#Starting WWW
Write-Host -Object "Starting WWW Service...";

Start-Service -Name W3SVC -ErrorAction SilentlyContinue;

Write-Host -Object "Done.";

#Starting DPS
Write-Host -Object "Starting Data Polling Service...";

$services = Get-Service -Name DataPollingService-*;

foreach($service in $services)
{
   $service.Start();
}

Write-Host -Object "Done.";


#Starting KPS
Write-Host -Object "Starting Key Provider Service...";

$services = Get-Service -Name KeyProviderService-*;

foreach($service in $services)
{
   $service.Start();
}

Write-Host -Object "Done.";