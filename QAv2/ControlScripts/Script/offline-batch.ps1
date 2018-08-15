param([System.String]$RootDir, [System.String]$ReportFileDir, [System.String]$BatchID)

if([System.String]::IsNullOrEmpty($ReportFileDir))
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "Report file directory should NOT be empty!";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

if([System.IO.Directory]::Exists($ReportFileDir) -eq $false)
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "Report file directory does NOT exist!";
   exit;

   if($StayInHost -eq $false)
   {
	  $Host.SetShouldExit(1);
   }
}

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

if([System.String]::IsNullOrEmpty($BatchID) -eq $true)
{
    $BatchID = [System.Guid]::NewGuid().ToString();
}

$Result = "";

[System.String]$TransactionID = "";

[System.String[]]$ReportFilePaths = [System.IO.Directory]::GetFiles($ReportFileDir, "*.xml");

$ReportFilePaths;

if(($ReportFilePaths -ne $null) -and ($ReportFilePaths.Length -gt 0))
{
	cd $RootDir;

	foreach($ReportFilePath in $ReportFilePaths)
	{
	   $TransactionID = [System.String]::Format("{0}_{1}", $BatchID, [System.GUID]::NewGuid());

	   $TransactionID;

	   $ReportFilePath;

	   .\Script\validate-offlineV2.ps1 -ReportFilePath $ReportFilePath -TransactionID $TransactionID -RootDir $RootDir -ByPassUI $true -StayInHost $true -OutResult ([ref]$Result);

	   $Result;
	}
}