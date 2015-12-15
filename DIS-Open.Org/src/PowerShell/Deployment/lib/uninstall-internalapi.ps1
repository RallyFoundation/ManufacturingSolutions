param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

#Delete web site:
$webSiteName = $component.AppServerSetting.ApplicationName;
$webSiteName = [System.String]::Format("{0}{1}", $installationType.ToString(), $webSiteName);

$webSite = Get-Website -Name $webSiteName;

if(($webSite -ne $null) -and (($webSite | Where-Object {$_.Name -eq $webSiteName}) -ne $null))
{
   Stop-WebSite -Name $webSiteName;
   Remove-Website -Name $webSiteName;
}

#Delete application pool:
$appPoolName = $component.AppServerSetting.ApplicationPoolName;
$appPoolName = [System.String]::Format("{0}{1}", $installationType.ToString(), $appPoolName);

$appPool = Get-Item ("IIS:\AppPools\" + $appPoolName);

if($appPool -ne $null)
{
   Stop-WebAppPool -Name $appPoolName;
   Remove-WebAppPool -Name $appPoolName;
}

#Delete home directory and source files:
$webSiteRootDirectory = $applicationRoot + $component.AppServerSetting.RootDirectory;

if([System.IO.Directory]::Exists($webSiteRootDirectory) -eq $true)
{
   Remove-Item $webSiteRootDirectory -Force -Recurse;
}

#Delete firewall rules:
$webSitePort = $component.AppServerSetting.HTTPPortNumber;

Netsh AdvFirewall Firewall Delete Rule Name = ("DIS-" + $webSiteName + "-" + $installationType.ToString() + "-" + $webSitePort.ToString() + "-In");
Netsh AdvFirewall Firewall Delete Rule Name = ("DIS-" + $webSiteName + "-" + $installationType.ToString() + "-" + $webSitePort.ToString() + "-Out");