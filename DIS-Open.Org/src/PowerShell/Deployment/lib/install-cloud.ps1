param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component);

[System.String]$membershipConnectionString = Get-DBConnectionString -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName "aspnetdb";

#if(($component -ne  $null) -and ($component.DBServerSetting -ne $null) -and ($component.DBServerSetting.IsIncluded -eq $true))
if(($component -ne  $null) -and ($component.DBServerSetting -ne $null))
{ 
    try
	{
	    [System.String]$testingConnectionString = Get-DBConnectionString -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName "master";

        $isDBConnectionAvailable = Test-DBConnection -DBConnectionString $testingConnectionString;

        if($isDBConnectionAvailable -eq  $false)
		{
	        Write-Host -Object ([System.String]::Format("The database connection info specified is not valid, and the connection test failed{DB Server : {0}; Username: {1}; Password: {2}}. Please check your DB connection settings.", $component.DBServerSetting.ServerAddress, $component.DBServerSetting.ServerLoginName, $component.DBServerSetting.ServerPassword));

            exit;
	    }

        $dbNames = Get-DatabaseList -DBConnectionString $testingConnectionString;

        $isAspNetMembershipDBinExsitence = $false;
		$isDBinExsitence = $false;
	    $isReusingExistingDB = "1";

        if($dbNames -ne $null)
		{
	        [System.String]$dbName = $null;

	        foreach($dbName in $dbNames)
	        {
	            if($dbName.ToLower() -eq  "aspnetdb")
	            { 
	                Write-Host -Object "ASP.NET membership database already exists, will reuse the current one.";

	                $isAspNetMembershipDBinExsitence = $true;
	            } 

                if($dbName.ToLower() -eq  $component.DBServerSetting.DatabaseName.ToLower())
				{
	                #Write-Host -Object ([System.String]::Format("The database you specified ({0}) already exists! The installation will be terminated. Please check the database settings in the unattend file.", $dbName));
	                #exit;
	                
	                Write-Host -Object ([System.String]::Format("The database you specified ({0}) already exists!", $dbName));

                    $isReusingExistingDB = Read-Host -Prompt "Reuse the current one (1, Default), or replace it with a new one (0)?";

                    if($isReusingExistingDB -ne "0")
					{
	                   $isDBinExsitence = $true;
	                }
	                else
	                {
	                    try
	                    {
	                        Write-Host -Object "Removing existing database...";
	                        
	                        Remove-Database -DBConnectionString $testingConnectionString -DBName $dbName;

                            $isDBinExsitence = $false;
	                    }
	                    catch [System.Exception]
						{
							$Message = $Error[0].Exception;
							$Message;

							Write-Host -Object "Error(s) occurred removing database, installation will be terminated!";

							exit;
						}
	                }

                    break;
	            }
	        }
	    }
	    
	    if($isDBinExsitence -eq $false)
	    {
	        Write-Host -Object "Creating database...";

			New-Database -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName $component.DBServerSetting.DatabaseName; 
			New-Database -DBServerName $component.DBServerSetting.ServerAddress -DBUserName $component.DBServerSetting.ServerLoginName -DBPassword $component.DBServerSetting.ServerPassword -DBName $component.DBServerSetting.DatabaseName -DBCreationScriptPath ($root + "lib\DataModel.edmx.sql");
	    }

	    if($isAspNetMembershipDBinExsitence -eq $false)
	    {
			C:\Windows\Microsoft.NET\Framework64\v4.0.30319\aspnet_regsql.exe -S $component.DBServerSetting.ServerAddress -U $component.DBServerSetting.ServerLoginName -P $component.DBServerSetting.ServerPassword -A all;
	    }
	    else
	    {
	        Clear-DISConfigurationCloudMembershipDB -DBConnectionString $membershipConnectionString -ApplicationName "DISConfigurationCloud";
	    }
	    
		Add-DISConfigurationCloudDefaultAccount -DBConnectionString $membershipConnectionString;
	}
	catch [System.Exception]
	{
	    $Message = $Error[0].Exception;
        $Message;

        Write-Host -Object "Error(s) occurred creating database, installation will be terminated!";

        exit;
	}
}

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
  $applicationSource = $root + "pkg\DISConfigurationCloud\*";

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

$webConfig.configuration.appSettings.SelectSingleNode("//add[@key='DatabaseBackupLocation']").value = $component.AppSetting.DatabaseBackupLocation;

[System.String]$connectionString = "metadata=res://*/DataModel.csdl|res://*/DataModel.ssdl|res://*/DataModel.msl;provider=System.Data.SqlClient;provider connection string='data source={0};initial catalog={1};persist security info=True;user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework'";

$connectionString = [System.String]::Format($connectionString, $component.DBServerSetting.ServerAddress, $component.DBServerSetting.DatabaseName, $component.DBServerSetting.ServerLoginName, $component.DBServerSetting.ServerPassword);

$webConfig.configuration.connectionStrings.SelectSingleNode("//add[@name='DataModelContainer']").connectionString = $connectionString;

$webConfig.configuration.connectionStrings.SelectSingleNode("//add[@name='ApplicationServices']").connectionString = $membershipConnectionString;

$webConfig.configuration.'system.web'.membership.providers.SelectSingleNode("//add[@name='AspNetSqlMembershipProvider']").applicationName = "DISConfigurationCloud";

$webConfig.configuration.'system.web'.profile.providers.SelectSingleNode("//add[@name='AspNetSqlProfileProvider']").applicationName = "DISConfigurationCloud";

$webConfig.configuration.'system.web'.roleManager.providers.SelectSingleNode("//add[@name='AspNetSqlRoleProvider']").applicationName = "DISConfigurationCloud";

$webConfig.configuration.'system.web'.roleManager.providers.SelectSingleNode("//add[@name='AspNetWindowsTokenRoleProvider']").applicationName = "DISConfigurationCloud";

$webConfig.configuration.'system.diagnostics'.sources.SelectSingleNode("//source[@name='DISConfigurationCloudTraceSource']").listeners.SelectSingleNode("//add[@name='messages']").initializeData = "messages-dis-cloud-trace.svclog";

$webConfig.Save($webSiteRootDirectory + "\Web.config");

$appPoolName = $component.AppServerSetting.ApplicationPoolName;

$appPool = New-WebAppPool -Name $appPoolName -Force;

$appPool.managedRuntimeVersion = "v4.0";
$appPool.enable32BitAppOnWin64 = $true;

$appPoolIdentityType = $component.AppServerSetting.ApplicationPoolIdentityType.value__;

$appPool.processModel.identityType = $appPoolIdentityType;

$appPool | Set-Item;

$appPoolIdentityType;

$appPool;

$webSiteName = $component.AppServerSetting.ApplicationName;

$webSitePort = $component.AppServerSetting.HTTPPortNumber;

$webSite = Get-Website -Name $webSiteName;

if(($webSite -ne $null) -and (($webSite | Where-Object {$_.Name -eq $webSiteName}) -ne $null))
{
   Remove-Website -Name $webSiteName;
}

New-WebSite -Name $webSiteName -Port $webSitePort -ApplicationPool $appPoolName -PhysicalPath $webSiteRootDirectory -Force;

Start-WebAppPool -Name $appPoolName;

Start-WebSite -Name $webSiteName;

Netsh AdvFirewall Firewall Add Rule Name = ("DIS-ConfigurationCloud-" + $webSitePort.ToString() + "-In") Dir=In Action=Allow Protocol=TCP LocalPort = $webSitePort LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = ("DIS-ConfigurationCloud-" + $webSitePort.ToString() + "-Out") Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = $webSitePort RemoteIP=Any

$shell = New-Object -ComObject WScript.Shell;
$desktop = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop);
$shortcut = $shell.CreateShortcut("$desktop\DIS Configuration Cloud.lnk");
$shortcut.TargetPath = [System.String]::Format("http://localhost:{0}", $webSitePort); 
$shortcut.IconLocation = [System.String]::Format("{0}\bin\DISConfigurationCloud.dll, 0", $webSiteRootDirectory);
$shortcut.Description = "DIS Configuration Cloud";
$shortcut.Save();

$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution";

if([System.IO.Directory]::Exists($programsFolderPath) -eq $false)
{
   New-Item $programsFolderPath -ItemType directory -Force;
}

$shortcut = $shell.CreateShortcut("$programsFolderPath\DIS Configuration Cloud.lnk");
$shortcut.TargetPath = [System.String]::Format("http://localhost:{0}", $webSitePort); 
$shortcut.IconLocation = [System.String]::Format("{0}\bin\DISConfigurationCloud.dll, 0", $webSiteRootDirectory);
$shortcut.Description = "DIS Configuration Cloud";
$shortcut.Save();