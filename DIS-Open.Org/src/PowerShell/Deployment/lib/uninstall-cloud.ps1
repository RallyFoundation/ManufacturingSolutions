param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

#Delete web site:
$webSiteName = $component.AppServerSetting.ApplicationName;

$webSite = Get-Website -Name $webSiteName;

if(($webSite -ne $null) -and (($webSite | Where-Object {$_.Name -eq $webSiteName}) -ne $null))
{
   Stop-WebSite -Name $webSiteName;
   Remove-Website -Name $webSiteName;
}

#Delete application pool:

$appPoolName = $component.AppServerSetting.ApplicationPoolName;

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

#Delete DB:
#if(($component -ne  $null) -and ($component.DBServerSetting -ne $null) -and ($component.DBServerSetting.IsIncluded -eq $true))
#{ 
#	#Delete data from the 3 tables in ASP.NET membership DB:

#	[System.String]$membershipConnectionString = Get-DBConnectionString -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName "aspnetdb";

#	Clear-DISConfigurationCloudMembershipDB -DBConnectionString $membershipConnectionString;

#	#Unregister ASP.NET membership:

#	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regsql.exe -S $component.DBServerSetting.ServerAddress -U $component.DBServerSetting.ServerLoginName -P $component.DBServerSetting.ServerPassword -R all -Q;

#	#Delete ASP.NET membership DB:

#    [System.String]$connectionString = Get-DBConnectionString -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName "master";

#    Remove-Database -DBConnectionString $connectionString -DBName "aspnetdb";

#	#Delete DIS Configuration Cloud DB:

#    Remove-Database -DBConnectionString $connectionString -DBName $component.DBServerSetting.DatabaseName;
#}

#Delete firewall rules:

$webSitePort = $component.AppServerSetting.HTTPPortNumber;

Netsh AdvFirewall Firewall Delete Rule Name = ("DIS-ConfigurationCloud-" + $webSitePort.ToString() + "-In");
Netsh AdvFirewall Firewall Delete Rule Name = ("DIS-ConfigurationCloud-" + $webSitePort.ToString() + "-Out");

#Delete desktop shortcuts:
$desktop = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop);

if([System.IO.File]::Exists("$desktop\DIS Configuration Cloud.lnk") -eq $true)
{
	Remove-Item ("$desktop\DIS Configuration Cloud.lnk") -Force;
}

#Delete programs menu shortcuts:
$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution";

if([System.IO.File]::Exists("$programsFolderPath\DIS Configuration Cloud.lnk") -eq $true)
{
	Remove-Item ("$programsFolderPath\DIS Configuration Cloud.lnk") -Force;
}