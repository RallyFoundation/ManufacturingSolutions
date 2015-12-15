param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component)

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
  $applicationSource = $root + "pkg\KeyManagementTool\*";

  $serviceRootDirectory = $applicationRoot + "KeyManagementTool";
}

if([System.IO.Directory]::Exists($serviceRootDirectory) -eq $false)
{
   New-Item $serviceRootDirectory -ItemType directory -Force;
}

Copy-Item -Path $applicationSource -Destination $serviceRootDirectory -Recurse -Force;

#Set the read-only attribute of the config file to be false explicitly
Set-ItemProperty -Path ($serviceRootDirectory + "\DIS.Presentation.KMT.exe.config") -Name IsReadOnly -Value $false;

[xml]$appConfig = Get-Content($serviceRootDirectory + "\DIS.Presentation.KMT.exe.config");

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='InstallType']").value = $component.AppSetting.InstallationType.ToString();

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudServerAddress']").value = $component.AppSetting.ConfigurationCloudServerAddress;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='ConfigurationCloudAuthHeader']").value = [System.String]::Format("{0}:{1}", $component.AppSetting.ConfigurationCloudUserName, $component.AppSetting.ConfigurationCloudPassword);

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCacheStore']").value = $applicationRoot + $component.AppSetting.CloudConfigurationCacheStore;

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCachePolicy']").value = $component.AppSetting.CloudConfigurationCachingPolicy.value__.ToString();

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='CloudConfigCacheToolPath']").value = $applicationRoot + "Configuration\ConfigurationProtectedTool.exe";

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='IsTpiInCentralizedMode']").value = $component.AppSetting.IsTPIInCentralizeMode.ToString();

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='IsTpiUsingCarbonCopy']").value = $component.AppSetting.IsTPIUsingCarbonCopy.ToString();

$appConfig.Save($serviceRootDirectory + "\DIS.Presentation.KMT.exe.config");

$shell = New-Object -ComObject WScript.Shell;
$desktop = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop);

$desktopKMTShortcutPath = [System.String]::Format("{0}\KMT - {1}.lnk", $desktop, $installationType.ToString());

$shortcut = $shell.CreateShortcut($desktopKMTShortcutPath);
$shortcut.TargetPath = [System.String]::Format("{0}\DIS.Presentation.KMT.exe", $serviceRootDirectory);
$shortcut.IconLocation = [System.String]::Format("{0}\DIS.Presentation.KMT.exe, 0", $serviceRootDirectory);
$shortcut.Description = "Key Management Tool - " + $installationType.ToString();
$shortcut.Save();

#$desktopCloudShortcutPath = [System.String]::Format("{0}\DIS Configuration Cloud - {1}.lnk", $desktop, $installationType.ToString());

#$shortcut = $shell.CreateShortcut($desktopCloudShortcutPath);
#$shortcut.TargetPath = $component.AppSetting.ConfigurationCloudServerAddress;
#$shortcut.IconLocation = [System.String]::Format("{0}\DISConfigurationCloud.Client.dll, 0", $serviceRootDirectory);
#$shortcut.Description = "DIS Configuration Cloud - " + $installationType.ToString();
#$shortcut.Save();

$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution\" + $installationType.ToString();

if([System.IO.Directory]::Exists($programsFolderPath) -eq $false)
{
   New-Item $programsFolderPath -ItemType directory -Force;
}

#$programsFolderCloudShortcutPath = [System.String]::Format("{0}\DIS Configuration Cloud - {1}.lnk", $programsFolderPath, $installationType.ToString());

#$shortcut = $shell.CreateShortcut($programsFolderCloudShortcutPath);
#$shortcut.TargetPath = $component.AppSetting.ConfigurationCloudServerAddress;
#$shortcut.IconLocation = [System.String]::Format("{0}\DISConfigurationCloud.Client.dll, 0", $serviceRootDirectory);
#$shortcut.Description = "DIS Configuration Cloud - "  + $installationType.ToString();
#$shortcut.Save();

$programsFolderKMTShortcutPath = [System.String]::Format("{0}\KMT - {1}.lnk", $programsFolderPath, $installationType.ToString());

$shortcut = $shell.CreateShortcut($programsFolderKMTShortcutPath);
$shortcut.TargetPath = [System.String]::Format("{0}\DIS.Presentation.KMT.exe", $serviceRootDirectory);
$shortcut.IconLocation = [System.String]::Format("{0}\DIS.Presentation.KMT.exe, 0", $serviceRootDirectory);
$shortcut.Description = "Key Management Tool - " + $installationType.ToString();
$shortcut.Save();

