param([System.String]$root = $null, [System.String]$applicationRoot = $null, $installationType, $installationMode, [DIS.Management.Deployment.Model.Component]$component)

$applicationSource = ".\pkg\*";

if($root -ne $null)
{
   $applicationSource = $root + "pkg\Configuration\*";

   $serviceRootDirectory = $applicationRoot + "Configuration";
}

if([System.IO.Directory]::Exists($serviceRootDirectory) -eq $false)
{
   New-Item $serviceRootDirectory -ItemType directory -Force;
}

Copy-Item -Path $applicationSource -Destination $serviceRootDirectory -Recurse -Force;

#Set the read-only attribute of the config file to be false explicitly
Set-ItemProperty -Path ($serviceRootDirectory + "\ConfigurationProtectedTool.exe.config") -Name IsReadOnly -Value $false;

[xml]$appConfig = Get-Content($serviceRootDirectory + "\ConfigurationProtectedTool.exe.config");

$appConfig.configuration.appSettings.SelectSingleNode("//add[@key='InstallType']").value = $installationType.ToString();

$appConfig.Save($serviceRootDirectory + "\ConfigurationProtectedTool.exe.config");

$shell = New-Object -ComObject WScript.Shell;
$programsFolderPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::CommonPrograms);
$programsFolderPath = $programsFolderPath + "\DIS Solution\" + $installationType.ToString() + "\LocalConfigurationTools\";

if([System.IO.Directory]::Exists($programsFolderPath) -eq $false)
{
   New-Item $programsFolderPath -ItemType directory -Force;
}

$shortcut = $shell.CreateShortcut("$programsFolderPath\ConfigurationProtectedTool.lnk");
$shortcut.TargetPath = [System.String]::Format("{0}\ConfigurationProtectedTool.exe", $serviceRootDirectory);
$shortcut.IconLocation = [System.String]::Format("{0}\ConfigurationProtectedTool.exe, 0", $serviceRootDirectory);
$shortcut.Description = "Configuration Protected Tool - " + $installationType.ToString();
$shortcut.Save();
