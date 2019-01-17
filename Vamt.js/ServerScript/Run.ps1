param([System.String]$TransactionID, [System.String]$RootDir)

if([System.String]::IsNullOrEmpty($RootDir) -eq $true)
{
   $RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

   if($RootDir.EndsWith("\") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
   }

   if($RootDir.ToLower().EndsWith("\script") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.ToLower().LastIndexOf("\script")));
   }
}

if($RootDir.EndsWith("\") -eq $true)
{
  $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

if([System.String]::IsNullOrEmpty($TransactionID) -eq $true)
{
    $TransactionID = [System.Guid]::NewGuid().ToString();
}

$LogPath = $RootDir +  "\Log";
if([System.IO.Directory]::Exists($LogPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($LogPath);
	Start-Sleep -Milliseconds 1000;
}

Start-Process "node" -ArgumentList @(($RootDir + "\Bin\Vamt.js\vamt.js"));
#Start-Process "node" -ArgumentList @(($RootDir + "\Bin\General\wds-api.js"));
#Start-Process "node" -ArgumentList @(($RootDir + "\Bin\OData\server-odata.js"));
#Start-Process "node" -ArgumentList @(($RootDir + "\Bin\LogMon\log-monitor.js"));

#cd ($RootDir + "\Bin\Ftp\");
#Start-Process ($RootDir + "\Lib\python\python.exe") -ArgumentList @(".\PythonFTP.py");

#cd ($RootDir + "\Lib\nginx\");
#Start-Process ($RootDir + "\Lib\nginx\nginx.exe");