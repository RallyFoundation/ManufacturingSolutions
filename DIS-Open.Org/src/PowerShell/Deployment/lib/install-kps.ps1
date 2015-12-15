param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component)

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
  $applicationSource = $root + "pkg\KeyProviderService\*";

  $serviceRootDirectory = $applicationRoot + "KeyProviderService";
}

if([System.IO.Directory]::Exists($serviceRootDirectory) -eq $false)
{
   New-Item $serviceRootDirectory -ItemType directory -Force;
}

Copy-Item -Path $applicationSource -Destination $serviceRootDirectory -Recurse -Force;

#Set the read-only attribute of the config file to be false explicitly
Set-ItemProperty -Path ($serviceRootDirectory + "\DIS.Services.KeyProviderService.exe.config") -Name IsReadOnly -Value $false;

[xml]$appConfig = Get-Content($serviceRootDirectory + "\DIS.Services.KeyProviderService.exe.config");

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='InstallType']").value = $component.AppSetting.InstallationType.ToString();

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudServerAddress']").value = $component.AppSetting.ConfigurationCloudServerAddress;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudAuthHeader']").value = [System.String]::Format("{0}:{1}", $component.AppSetting.ConfigurationCloudUserName, $component.AppSetting.ConfigurationCloudPassword);

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCacheStore']").value = $applicationRoot + $component.AppSetting.CloudConfigurationCacheStore;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCachePolicy']").value = $component.AppSetting.CloudConfigurationCachingPolicy.value__.ToString();

$appConfig.Save($serviceRootDirectory + "\DIS.Services.KeyProviderService.exe.config");

#$dotnetFrameworkDir = $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory());

#C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ($serviceRootDirectory + "\DIS.Services.KeyProviderService.exe");

$serviceName = [System.String]::Format("KeyProviderService-{0}", $installationType.ToString());

#$serviceDisplayName = [System.String]::Format("Key Provider Service (KPS) - {0}", $installationType.ToString());

$serviceDisplayName = "KeyProviderService - Default";

$serviceDescription = "Interacts with Microsoft OA3.0 Tool, provisions DPK binaries to and recieves bindings from production line.";

$serviceBinaryPath = ($serviceRootDirectory + "\DIS.Services.KeyProviderService.exe");

$result = Install-Service -Name $serviceName -DisplayName $serviceDisplayName -Path $serviceBinaryPath -IsStartingImmediately $false -StartupType DemandStart;

$result;

Netsh AdvFirewall Firewall Add Rule Name = ("DIS-KPS-" + $installationType.ToString() + "-" + $component.AppSetting.ServicePortNumber.ToString() + "-In") Dir=In Action=Allow Protocol=TCP LocalPort = $component.AppSetting.ServicePortNumber LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = ("DIS-KPS-" + $installationType.ToString() + "-" + $component.AppSetting.ServicePortNumber.ToString() + "-Out") Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = $component.AppSetting.ServicePortNumber RemoteIP=Any


Set-Service -Name $serviceName -Description $serviceDescription -StartupType Automatic;
#Start-Service -Name $serviceName;