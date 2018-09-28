param([System.String]$TransactionID, [System.String]$RootDir, [System.String]$ServiceUrl, [System.Boolean]$ByPassUI = 0, [System.Boolean]$StayInHost = 0, [ref]$OutResult);

$ErrorActionPreference = "Stop";

$PowerShellVersionInfo = $PSVersionTable;

$PowerShellVersionInfo;

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

$LogPath = $RootDir +  "\Log";
if([System.IO.Directory]::Exists($LogPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($LogPath);
	Start-Sleep -Milliseconds 1000;
}

$OutputPath = $RootDir +  "\Output";
if([System.IO.Directory]::Exists($OutputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($OutputPath);
    Start-Sleep -Milliseconds 1000;
}

$InputPath = $RootDir +  "\Input";
if([System.IO.Directory]::Exists($InputPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($InputPath);
	Start-Sleep -Milliseconds 1000;
}

$MatrixPath = $RootDir +  "\Matrix";
if([System.IO.Directory]::Exists($MatrixPath) -eq $false)
{
    [System.IO.Directory]::CreateDirectory($MatrixPath);
	Start-Sleep -Milliseconds 1000;
}

$MatrixUIPath = ($RootDir + "\Module\UI\Views\Matrix.html");
$WebViewPlusPath = ($RootDir + "\Module\UI\WebViewPlus.exe");

Start-Process -FilePath $WebViewPlusPath -ArgumentList @($MatrixUIPath) -Wait -NoNewWindow;

$Host.SetShouldExit(1);