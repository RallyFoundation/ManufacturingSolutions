param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component)

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
  $applicationSource = $root + "pkg\InternalAPI\*";

  $webSiteRootDirectory = $applicationRoot + $component.AppServerSetting.RootDirectory;
}

if([System.IO.Directory]::Exists($webSiteRootDirectory) -eq $false)
{
   New-Item $webSiteRootDirectory -ItemType directory -Force;
}

Copy-Item -Path $applicationSource -Destination $webSiteRootDirectory -Recurse -Force;

#Set the read-only attribute of the config file to be false explicitly
Set-ItemProperty -Path ($webSiteRootDirectory + "\Web.config") -Name IsReadOnly -Value $false;

[xml]$webConfig = Get-Content($webSiteRootDirectory + "\Web.config");

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='InstallType']").value = $component.AppSetting.InstallationType.ToString();

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudServerAddress']").value = $component.AppSetting.ConfigurationCloudServerAddress;

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudAuthHeader']").value = [System.String]::Format("{0}:{1}", $component.AppSetting.ConfigurationCloudUserName, $component.AppSetting.ConfigurationCloudPassword);

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCacheStore']").value = $applicationRoot + $component.AppSetting.CloudConfigurationCacheStore;

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCachePolicy']").value = $component.AppSetting.CloudConfigurationCachingPolicy.value__.ToString();

$webConfig.Save($webSiteRootDirectory + "\Web.config");

$appPoolName = $component.AppServerSetting.ApplicationPoolName;
$appPoolName = [System.String]::Format("{0}{1}", $installationType.ToString(), $appPoolName);

$appPool = New-WebAppPool -Name $appPoolName -Force;

$appPool.managedRuntimeVersion = "v4.0";
$appPool.enable32BitAppOnWin64 = $true;

$appPoolIdentityType = 3;

if($installationType.value__ -eq 4)
{
   $appPoolIdentityType = 1;
}

$appPool.processModel.identityType = $appPoolIdentityType;

if($appPoolIdentityType -eq 3)
{
  $appPool.processModel.username = $component.AppServerSetting.ApplicationPoolIdentityUserName;
  $appPool.processModel.password = $component.AppServerSetting.ApplicationPoolIdentityPassword;
}

$appPool | Set-Item;

$appPoolIdentityType;

$appPool;

$webSiteName = $component.AppServerSetting.ApplicationName;
$webSiteName = [System.String]::Format("{0}{1}", $installationType.ToString(), $webSiteName);

$webSitePort = $component.AppServerSetting.HTTPPortNumber;

$webSite = Get-Website -Name $webSiteName;

if(($webSite -ne $null) -and (($webSite | Where-Object {$_.Name -eq $webSiteName}) -ne $null))
{
   Remove-Website -Name $webSiteName;
}

#New-WebSite -Name $webSiteName -Port $webSitePort -ApplicationPool $appPoolName -PhysicalPath $webSiteRootDirectory -Ssl -SslFlags 0 -Force;

New-WebSite -Name $webSiteName -Port $webSitePort -ApplicationPool $appPoolName -PhysicalPath $webSiteRootDirectory -Ssl -Force;

Start-WebAppPool -Name $appPoolName;

#Start-WebSite -Name $webSiteName;

Netsh AdvFirewall Firewall Add Rule Name = ("DIS-" + $webSiteName + "-" + $installationType.ToString() + "-" + $webSitePort.ToString() + "-In") Dir=In Action=Allow Protocol=TCP LocalPort = $webSitePort LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = ("DIS-" + $webSiteName + "-" + $installationType.ToString() + "-" + $webSitePort.ToString() + "-Out") Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = $webSitePort RemoteIP=Any