
param([System.String]$unattend = $null, [System.String]$root = $null);

#if($root -eq $null)
#{
#   $rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;
#}
#else
#{
#   $rootDir = $root;
#}

$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($rootDir.EndsWith("\") -eq $false)
{
   $rootDir = $rootDir + "\";
}

if($unattend -eq  $null)
{
   Write-Host -Object "You must specify an unattend file!";
   exit;
}

if($unattend.IndexOf("\") -le 0)
{
   $unattend = $rootDir + $unattend;
}

if($unattend.StartsWith(".\") -eq $true)
{
   $unattend = $rootDir + $unattend.Substring(2);
}

if([System.IO.File]::Exists($unattend) -eq  $false)
{
   Write-Host -Object ([System.String]::Format("The path for the unattend file that you specified ('{0}')could not be found! Please verify that it's a valid absolute path.", $unattend));
   exit;
}

cd $rootDir;

$modulePath = $rootDir + "lib";

Import-Module WebAdministration;

Import-Module ("$modulePath\DIS.Management.Deployment.dll");

Import-Module ("$modulePath\DIS.Management.Storage.dll");

Import-Module ("$modulePath\DIS.Management.Service.dll");

if($unattend -ne $null)
{
   [xml]$unattendXml = (Get-Content -Path $unattend -Encoding UTF8);

   [DIS.Management.Deployment.Model.Installation]$installInfo = Get-InstallationInfo -UnattendXml $unattendXml.InnerXml;

   if($installInfo -ne $null)
   {
      [System.String]$applicationRoot = $installInfo.ApplicationRoot;

      [DIS.Management.Deployment.Model.InstallationType]$installationType = $installInfo.InstallationType;

      [DIS.Management.Deployment.Model.InstallationMode]$installationMode = $installInfo.InstallationMode;

      if($installationType -ne [DIS.Management.Deployment.Model.InstallationType]::Cloud)
	  {
		  Write-Host -Object "Verifying DIS Configuration Cloud Caching Policy settings...";

		  [DIS.Management.Deployment.Model.Component[]]$conflictingComponents = $null;
		  [DISConfigurationCloud.Client.CachingPolicy]$cachingPolicy = [DISConfigurationCloud.Client.CachingPolicy]::RemoteOnly;
	  
		  [System.Boolean]$areCloudCachingPoliciesConsistent = $installInfo.ValidateComponentCloudCachingPolicy([ref] $conflictingComponents, [ref] $cachingPolicy);

		  if($areCloudCachingPoliciesConsistent -eq $false)
		  {
			  Write-Host -Object "The following components have conflicting DIS Configuration Cloud Caching Policy settings, please double check your unattend file:";

			  foreach($comp in $conflictingComponents)
			  {
				  Write-Host -Object $comp.Name;
			  }

			  #[System.String]$continueFlag = Read-Host -Prompt "Do you still want to continue the installation? (Y: Yes (NOT recommanded); N: No (Default))";

			  #if($continueFlag.ToLower() -ne "y")
			  #{
			  #	  exit;
			  #}

              Read-Host -Prompt "Press any key to terminate the installation...";

			  exit;
		  }
		  else
		  {
			  Write-Host -Object "OK.";
		  }
	  
		  if($cachingPolicy -ne [DISConfigurationCloud.Client.CachingPolicy]::LocalOnly)
		  {
			  Write-Host -Object "Verifying DIS Configuration Cloud connection settings...";

			  [DIS.Management.Deployment.Model.Component[]]$problemComponents = $null;

			  [System.Boolean]$areCloudConnectionsOK = $installInfo.ValidateComponentCloudConnection([ref] $problemComponents);

			  if($areCloudConnectionsOK -eq $false)
			  {
					Write-Host -Object "The following components are having problems in DIS Configuration Cloud connection settings, please double check your unattend file:";

					foreach($comp in $problemComponents)
					{
						Write-Host -Object $comp.Name;
					}

					#[System.String]$continueFlag = Read-Host -Prompt "Do you still want to continue the installation? (Y: Yes (NOT recommanded); N: No (Default))";

					#if($continueFlag.ToLower() -ne "y")
					#{
					#	exit;
					#}

                    Read-Host -Prompt "Press any key to terminate the installation...";

			        exit;
			  }
			  else
			  {
				  Write-Host -Object "OK.";
			  }
		  }
	  }

      [DIS.Management.Deployment.Model.Component]$cloudComponent = $installInfo.GetComponent("DISConfigurationCloud");

      [DIS.Management.Deployment.Model.Component]$kmtComponent = $installInfo.GetComponent("KMT");

      [DIS.Management.Deployment.Model.Component]$internalAPIComponent = $installInfo.GetComponent("InternalAPI");

      [DIS.Management.Deployment.Model.Component]$externalAPIComponent = $installInfo.GetComponent("ExternalAPI");

      [DIS.Management.Deployment.Model.Component]$dataPollingComponent = $installInfo.GetComponent("DataPollingService");

      [DIS.Management.Deployment.Model.Component]$kpsComponent = $installInfo.GetComponent("KeyProviderService");

      [DIS.Management.Deployment.Model.Component]$databaseComponent = $installInfo.GetComponent("Database");

      if($dataPollingComponent -ne $null)
	  {
	      $serviceName = [System.String]::Format("DataPollingService-{0}", $installationType.ToString());

	      $serviceInfo = Get-Service -Name $serviceName -ErrorAction Ignore;

          if($serviceInfo -ne $null)
		  {
	          Write-Host -Object "The Data Polling Service component already exists on this machine. Please make sure it has been uninstalled before proceeding with the installation.";
	          exit;
	      }
	  }

      if($kpsComponent -ne $null)
	  {
	      $serviceName = [System.String]::Format("KeyProviderService-{0}", $installationType.ToString());

	      $serviceInfo = Get-Service -Name $serviceName -ErrorAction Ignore;

          if($serviceInfo -ne $null)
		  {
	          Write-Host -Object "The Key Provider Service component already exists on this machine. Please make sure it has been uninstalled before proceeding with the installation.";
	          exit;
	      }
	  }

      $appRoot = $applicationRoot;

      if($applicationRoot.EndsWith("\") -eq  $false)
	  {
	      $appRoot = $applicationRoot + "\";
	  }
	  
	  $appRoot = $appRoot + (Convert-ToConfigurationType -Value $installationType).ToString() + "\";
       
      if($cloudComponent -ne $null)
      {
		  .\lib\install-cloud.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $cloudComponent;
      }

      if($kmtComponent -ne  $null)
	  {
	     .\lib\install-kmt.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $kmtComponent;
	  }

      if($dataPollingComponent -ne  $null)
	  {
	     .\lib\install-datapolling.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $dataPollingComponent;
	  }

      if($kpsComponent -ne  $null)
	  {
	     .\lib\install-kps.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $kpsComponent;
	  }

	  if($internalAPIComponent -ne  $null)
	  {
	     .\lib\install-internalapi.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $internalAPIComponent;
	  }

      if($externalAPIComponent -ne  $null)
	  {
	     .\lib\install-externalapi.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $externalAPIComponent;
	  }

      if($installationType -ne [DIS.Management.Deployment.Model.InstallationType]::Cloud)
	  {
         .\lib\install-configtool.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $null;
      
		  if(($cachingPolicy -eq [DISConfigurationCloud.Client.CachingPolicy]::LocalOnly) -and ($databaseComponent -ne  $null) -and ($databaseComponent.DBServerSetting -ne $null) -and ($databaseComponent.DBServerSetting.IsIncluded -eq $true))
		  {
			  $connectionString = Get-DBConnectionString -DBServerName $databaseComponent.DBServerSetting.ServerAddress -DBUserName $databaseComponent.DBServerSetting.ServerLoginName -DBPassword $databaseComponent.DBServerSetting.ServerPassword -DBName "master";
	      
			  $isDBConnectionValid = Test-DBConnection -DBConnectionString $connectionString;

			  if($isDBConnectionValid -eq $true)
			  {
				 $connectionString = Get-DBConnectionString -DBServerName $databaseComponent.DBServerSetting.ServerAddress -DBUserName $databaseComponent.DBServerSetting.ServerLoginName -DBPassword $databaseComponent.DBServerSetting.ServerPassword -DBName $databaseComponent.DBServerSetting.DatabaseName;

                 $isDBConnectionValid = Test-DBConnection -DBConnectionString $connectionString;

     #            $isDBInExistence = $false;

     #            $dbNameList = Get-DatabaseList -DBConnectionString $connectionString;

     #            [System.String]$dbName = "";
				 #[System.String]$databaseName = $databaseComponent.DBServerSetting.DatabaseName.ToString();

     #            foreach($dbName in $dbNameList)
     #            {
     #               if($dbName.ToLower() -eq $databaseName.ToLower())
     #               {
     #                    $isDBInExistence = $true;
     #                    break;
     #               }
     #            }

                 if($isDBConnectionValid -eq $false) #if($isDBInExistence -eq $false) #
				 {
	                 Write-Host -Object ([System.String]::Format("Database '{0}' does not exist, a new one will be created...", $databaseComponent.DBServerSetting.DatabaseName));
	                 
					 $result = New-Database -DBServerName $databaseComponent.DBServerSetting.ServerAddress -DBUserName $databaseComponent.DBServerSetting.ServerLoginName -DBPassword $databaseComponent.DBServerSetting.ServerPassword -DBName $databaseComponent.DBServerSetting.DatabaseName -DBCreationScriptPath ("$modulePath\KeyStore.publish.sql");

					 $result;
				 }
	             else
	             {
	                 Write-Host -Object ([System.String]::Format("Database '{0}' is in existence, and will be used to configure the default buinsess in the local cache directly...", $databaseComponent.DBServerSetting.DatabaseName));
	             }

				 New-DISConfigurationCloudCache -Default $true -DBConnectionString $connectionString -ConfigurationType (Convert-ToConfigurationType -Value $installationType) -CacheFilePath ($appRoot + "Cloud-Configs.xml");
			  }
			  else
			  {
				 New-DISConfigurationCloudCache -Empty $true -CacheFilePath ($appRoot + "Cloud-Configs.xml");
			  }
		  }
		  else
		  {
			 New-DISConfigurationCloudCache -Empty $true -CacheFilePath ($appRoot + "Cloud-Configs.xml");
		  }
	  }
	  
	  $installationSourcePath = $appRoot + "Installation\source\";

      $installationUnattendPath = $appRoot + "Installation\unattend\";

      if([System.IO.Directory]::Exists($installationSourcePath) -eq $false)
      {
		 New-Item $installationSourcePath -ItemType directory -Force;
	  }

      if([System.IO.Directory]::Exists($installationUnattendPath) -eq $false)
      {
		 New-Item $installationUnattendPath -ItemType directory -Force;
	  }

      Copy-Item -Path ($rootDir + "*") -Destination $installationSourcePath -Recurse -Force;

      Copy-Item -Path $unattend -Destination $installationUnattendPath -Force;

      if([System.IO.File]::Exists(($rootDir + "version.xml")) -eq $true)
	  {
		  Copy-Item -Path ($rootDir + "version.xml") -Destination ($appRoot + "Installation\") -Force;
	  }

      if($internalAPIComponent -ne  $null)
	  {
	     Write-Host("Starting Internal API...");

         Start-WebSite -Name ([System.String]::Format("{0}{1}", $installationType.ToString(), $internalAPIComponent.AppServerSetting.ApplicationName));

         Write-Host("Done.");
	  }

      if($externalAPIComponent -ne  $null)
	  {
	     Write-Host("Starting External API...");

         Start-WebSite -Name ([System.String]::Format("{0}{1}", $installationType.ToString(), $externalAPIComponent.AppServerSetting.ApplicationName));

         Write-Host("Done.");
	  }

      if($kpsComponent -ne  $null)
	  {
	      Write-Host("Starting Key Provider Service...");

          Start-Service -Name ([System.String]::Format("KeyProviderService-{0}", $installationType.ToString()));

          Write-Host("Done.");
	  }

      if($dataPollingComponent -ne $null)
	  {
	      Write-Host("Starting Data Polling Service...");

          Start-Service -Name ([System.String]::Format("DataPollingService-{0}", $installationType.ToString()));

          Write-Host("Done.");
	  }
   }
}