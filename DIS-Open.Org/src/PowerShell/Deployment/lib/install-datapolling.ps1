param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component)

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
  $applicationSource = $root + "pkg\DataPollingService\*";

  $serviceRootDirectory = $applicationRoot + "DataPollingService";
}

if([System.IO.Directory]::Exists($serviceRootDirectory) -eq $false)
{
   New-Item $serviceRootDirectory -ItemType directory -Force;
}

Copy-Item -Path $applicationSource -Destination $serviceRootDirectory -Recurse -Force;

#Set the read-only attribute of the config file to be false explicitly
Set-ItemProperty -Path ($serviceRootDirectory + "\DIS.Services.DataPolling.exe.config") -Name IsReadOnly -Value $false;

[xml]$appConfig = Get-Content($serviceRootDirectory + "\DIS.Services.DataPolling.exe.config");

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='InstallType']").value = $component.AppSetting.InstallationType.ToString();

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudServerAddress']").value = $component.AppSetting.ConfigurationCloudServerAddress;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudAuthHeader']").value = [System.String]::Format("{0}:{1}", $component.AppSetting.ConfigurationCloudUserName, $component.AppSetting.ConfigurationCloudPassword);

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCacheStore']").value = $applicationRoot + $component.AppSetting.CloudConfigurationCacheStore;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCachePolicy']").value = $component.AppSetting.CloudConfigurationCachingPolicy.value__.ToString();

$appConfig.Save($serviceRootDirectory + "\DIS.Services.DataPolling.exe.config");

#$dotnetFrameworkDir = $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory());

#C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ($serviceRootDirectory + "\DIS.Services.DataPolling.exe");

$serviceName = [System.String]::Format("DataPollingService-{0}", $installationType.ToString());

#$serviceDisplayName = [System.String]::Format("DataPollingService - {0}", $installationType.ToString());

$serviceDisplayName = "DataPollingService - ";

if($installationType -eq [DIS.Management.Deployment.Model.InstallationType]::Oem)
{
   $serviceDisplayName = "DataPollingService - OEMCorp";
}
elseif($installationType -eq [DIS.Management.Deployment.Model.InstallationType]::Tpi)
{
   $serviceDisplayName = "DataPollingService - TPICorp";
}
elseif($installationType -eq [DIS.Management.Deployment.Model.InstallationType]::FactoryFloor)
{
   $serviceDisplayName = "DataPollingService - FactoryFloor";
}

$serviceDescription = "Fulfills OA3.0 DPKs from / submits CBRs to Microsoft, and synchronizes DPK status between inventory systems on a schedule";

$serviceBinaryPath = ($serviceRootDirectory + "\DIS.Services.DataPolling.exe");

$result = Install-Service -Name $serviceName -DisplayName $serviceDisplayName -Path $serviceBinaryPath -IsStartingImmediately $false -StartupType DemandStart;

$result;

Set-Service -Name $serviceName -Description $serviceDescription -StartupType Automatic;

#Start-Service -Name $serviceName;