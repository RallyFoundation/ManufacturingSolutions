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

if($unattend -ne  $null)
{
   [xml]$unattendXml = (Get-Content -Path $unattend -Encoding UTF8);

   [DIS.Management.Deployment.Model.Installation]$installInfo = Get-InstallationInfo -UnattendXml $unattendXml.InnerXml;

   if($installInfo -ne $null)
   {
      [System.String]$applicationRoot = $installInfo.ApplicationRoot;

      $installationType = $installInfo.InstallationType;

      $installationMode = $installInfo.InstallationMode;

      [DIS.Management.Deployment.Model.Component]$cloudComponent = $installInfo.GetComponent("DISConfigurationCloud");

      [DIS.Management.Deployment.Model.Component]$kmtComponent = $installInfo.GetComponent("KMT");

      [DIS.Management.Deployment.Model.Component]$internalAPIComponent = $installInfo.GetComponent("InternalAPI");

      [DIS.Management.Deployment.Model.Component]$externalAPIComponent = $installInfo.GetComponent("ExternalAPI");

      [DIS.Management.Deployment.Model.Component]$dataPollingComponent = $installInfo.GetComponent("DataPollingService");

      [DIS.Management.Deployment.Model.Component]$kpsComponent = $installInfo.GetComponent("KeyProviderService");

      [DIS.Management.Deployment.Model.Component]$databaseComponent = $installInfo.GetComponent("Database");

      $appRoot = $applicationRoot;

      if($applicationRoot.EndsWith("\") -eq  $false)
	  {
	      $appRoot = $applicationRoot + "\";
	  }
	  
	  $appRoot = $appRoot + (Convert-ToConfigurationType -Value $installationType).ToString() + "\";

      if([System.IO.Directory]::Exists(($appRoot + "Installation")) -eq $false)
	  {
	      Write-Output "Cloud not find a corresponding installation, the uninstallation will not continue.";
	      Read-Host "Press any key to exit...";
	      exit;
	  }
       
      if($cloudComponent -ne $null)
      {
		  .\lib\uninstall-cloud.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $cloudComponent;
      }

      if($kmtComponent -ne  $null)
	  {
	     .\lib\uninstall-kmt.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $kmtComponent;
	  }

      if($dataPollingComponent -ne  $null)
	  {
	     .\lib\uninstall-datapolling.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $dataPollingComponent;
	  }

      if($kpsComponent -ne  $null)
	  {
	     .\lib\uninstall-kps.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $kpsComponent;
	  }

	  if($internalAPIComponent -ne  $null)
	  {
	     .\lib\uninstall-internalapi.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $internalAPIComponent;
	  }

      if($externalAPIComponent -ne  $null)
	  {
	     .\lib\uninstall-externalapi.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $externalAPIComponent;
	  }
	  
	  if($installationType -ne [DIS.Management.Deployment.Model.InstallationType]::Cloud)
	  {
         .\lib\uninstall-configtool.ps1 -root $rootDir -applicationRoot $appRoot -installationType $installationType -installationMode $installationMode -component $null;
      }

	  #if(($installationType -eq [DIS.Management.Deployment.Model.InstallationType]::Cloud) -and ($databaseComponent -ne  $null) -and ($databaseComponent.DBServerSetting -ne $null) -and ($databaseComponent.DBServerSetting.IsIncluded -eq $true))
	  #{
	  #    $connectionString = Get-DBConnectionString -DBServerName $databaseComponent.DBServerSetting.ServerAddress -DBUserName $databaseComponent.DBServerSetting.ServerLoginName -DBPassword $databaseComponent.DBServerSetting.ServerPassword -DBName "master";
	      
	  #    $isDBConnectionValid = Test-DBConnection -DBConnectionString $connectionString;

   #       if($isDBConnectionValid -eq $true)
		 # {
   #          $result = Remove-Database -DBConnectionString $connectionString -DBName $databaseComponent.DBServerSetting.DatabaseName;

   #          $result;
	  #    }
	  #}

      #Backup local cloud meta file (Cloud-Configs.xml) to the /AppData/Local store
	  $localCloudMetaPath = ($appRoot + "Cloud-Configs.xml");
	  
	  if([System.IO.File]::Exists($localCloudMetaPath) -eq $true)
	  {
	      $localCloudMetaBackupLocation = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::LocalApplicationData);
      
		  $localCloudMetaBackupLocation = $localCloudMetaBackupLocation + "\DIS Solution\" + $installationType.ToString() + "\" + ([System.DateTime]::Now).ToString("MM-dd-yyyy-hh-mm-ss");

		  if([System.IO.Directory]::Exists($localCloudMetaBackupLocation) -eq  $false)
		  {
			 [System.IO.Directory]::CreateDirectory($localCloudMetaBackupLocation);
		  }

		  Copy-Item -Path $localCloudMetaPath -Destination $localCloudMetaBackupLocation -Force;
	  }

      Remove-Item -Path $appRoot -Recurse -Force;

      $programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);

      if($installationType -ne [DIS.Management.Deployment.Model.InstallationType]::Cloud)
	  {
	     $programsFolderPath = $programsFolderPath + "\DIS Solution\" + $installationType.ToString();
	     Remove-Item -Path $programsFolderPath -Recurse -Force;
	     
	     [System.String]$isRestartingComputer = Read-Host -Prompt "Some changes won't take effect until you restart the computer. Restart now? Y: Yes (Default); N: No.";
	
		 if($isRestartingComputer.ToUpper() -ne "N")
		 {
			Restart-Computer -ComputerName localhost;
		 } 
	  }
   }
}