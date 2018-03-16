param([System.String]$RootDir, [System.String]$Architecture = "x86");

if([System.String]::IsNullOrEmpty($RootDir) -eq $true)
{
   $RootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

   if($RootDir.EndsWith("\") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
   }

   if($RootDir.ToLower().EndsWith("\scripts") -eq $true)
   {
      $RootDir = $RootDir.Substring(0, ($RootDir.ToLower().LastIndexOf("\scripts")));
   }
}

if($RootDir.EndsWith("\") -eq $true)
{
  $RootDir = $RootDir.Substring(0, ($RootDir.Length -1));
}

##Runs DPK Injection Tool to erase the injected DPK
try
{
    $Message = [System.String]::Format("Erase DPK..., {0}", [System.DateTime]::Now);
    $Message;
    
    Start-Process -FilePath ($RootDir + "\Module\InjectionTool\AMI\AFUWin.exe")  -NoNewWindow -ArgumentList @("/OAD") -Wait;

    $Host.UI.RawUI.BackgroundColor = "Green";
    $Host.UI.RawUI.ForegroundColor = "Red";
    $Message = "DPK erased successfully!";
    $Message;

    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms");

    $DialogResult = [System.Windows.Forms.MessageBox]::Show("DPK erased successfully! Restart unit now?" , "Success" , 4);

    if($DialogResult -eq "YES")
    {
        Restart-Computer;
    }
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    
    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Errors occurred!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}