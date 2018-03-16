param([System.String]$RootDir, [System.String]$Architecture = "amd64", [System.String]$NIC, [System.String]$IsShowingBarcode = "false");

#if($RootDir -eq $null)
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

#Content of the file-based oa3tool configuration file
[xml]$OA3ToolConfiguration = [xml]'<?xml version="1.0" encoding="utf-8"?>
                                 <OA3> 
                                  <FileBased> 
                                    <InputKeyXMLFile />
                                    <Parameters/>
                                  </FileBased> 
                                  <OutputData> 
                                    <AssembledBinaryFile /> 
                                    <ReportedXMLFile />
                                  </OutputData> 
                                </OA3>';

[xml]$OemHardwareReportData = [xml]'<OEMOptionalInfo>
										  <Field>
											<Name>ZPC_MODEL_SKU</Name>
											<Value>ABCDEFGHIJKLM1122233344Z</Value>
										  </Field>
										  <Field>
											<Name>ZMANUF_GEO_LOC</Name>
											<Value>1</Value>
										  </Field>
										  <Field>
											<Name>ZPGM_ELIG_VAL</Name>
											<Value>12345678Z,ABC,DEFGH,1212,1212</Value>
										  </Field>
										  <Field>
											<Name>ZOEM_EXT_ID</Name>
											<Value>30000123</Value>
										  </Field>
										  <Field>
											<Name>ZCHANNEL_REL_ID</Name>
											<Value>China</Value>
										  </Field>
										  <Field>
											<Name>ZFRM_FACTOR_ CL1</Name>
											<Value>Tablet</Value>
										  </Field>
										  <Field>
											<Name>ZFRM_FACTOR_ CL2</Name>
											<Value>Standard</Value>
										  </Field>
										  <Field>
											<Name>ZSCREEN_SIZE</Name>
											<Value>10.1</Value>
										  </Field>
										  <Field>
											<Name>ZTOUCH_SCREEN</Name>
											<Value>Touch</Value>
										  </Field>
									   </OEMOptionalInfo>';


$OA3ToolPath = $RootDir + "\OA3Tool9600\amd64\oa3tool.exe";

if($Architecture.ToLower() -eq "x86")
{
   $OA3ToolPath = $RootDir + "\OA3Tool9600\x86\oa3tool.exe";
}

$DPKFilePath = $null;

$LogPath = $RootDir + "\Logs";

$TransactionID = [System.Guid]::NewGuid().ToString();

[System.String]$SerialNumber = "";

[System.String]$Message = "";

#Retrieves MAC Info:
try
{
  $Message = [System.String]::Format("Retrieving MAC Info..., {0}", [System.DateTime]::Now);
  $Message;
  $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
   
  $NICName = $NIC; 

  $MacObject = Get-WmiObject Win32_NetworkAdapter | Where-Object { $_.MacAddress -and $_.Name -eq $NICName} | Select-Object Name, MacAddress; 

  "MAC Info: " | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
  $MacObject | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

  $SerialNumber = $MacObject.MacAddress;
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred retrieving MAC info!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

if([System.String]::IsNullOrEmpty($SerialNumber))
{
   $Host.UI.RawUI.BackgroundColor = "Red";
   $Host.UI.RawUI.ForegroundColor = "Yellow";
   Write-Host -Object "Failed to bind a value to serial number! ";
   Read-Host -Prompt "Press any key to exit...";
   exit;
}

"Transaction ID: " | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$TransactionID | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

"Serial Number: " | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
$SerialNumber | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

#Invokes OA3Tool.exe /Validate to test if there is already a DPK injected
try
{
    $Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
       
    $Message = & ($OA3ToolPath) @("/Validate");
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    if($Message.Contains("The operation completed successfully.") -eq $true)
    {
       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "The board has already got a DPK injected, and the ACPI MSDM table is NOT empty!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
    }
    else
    {
       Write-Host -Object "OK.";
    }
}
catch [System.Exception]
{
    $Message = $Error[0].Exception;
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    $Host.UI.RawUI.BackgroundColor = "Red";
    $Host.UI.RawUI.ForegroundColor = "Yellow";
    Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
    Read-Host -Prompt "Press any key to exit...";
    exit;
}

try
{
   [System.String[]]$DPKInputFiles = [System.IO.Directory]::GetFiles(($RootDir + "\DPKInputs\"), "*.xml", [System.IO.SearchOption]::AllDirectories);

   if(($DPKInputFiles -ne $null) -and ($DPKInputFiles.Length -gt 0))
   {
      $DPKFilePath = $DPKInputFiles[0];

      $Message = [System.String]::Format("{0} matche(s) found, and '{1}' has been picked up. {2}", $DPKInputFiles.Length, $DPKFilePath, [System.DateTime]::Now);

      $Message;

      $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
   }

   $DPKFilePath;
   
}
catch [System.Exception]
{
  $Message = $Error[0].Exception;
  $Message;
  $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
  
  $Host.UI.RawUI.BackgroundColor = "Red";
  $Host.UI.RawUI.ForegroundColor = "Yellow";
  Write-Host -Object "Errors occurred!";
  Read-Host -Prompt "Press any key to exit...";
  exit;
}

##Sets oa3tool configurations, runs oa3tool and injects DPK with the DPK input file found
if(($DPKFilePath -ne $null) -and ($DPKFilePath -ne ""))
{
    [System.String]::Format("Begin processing '{0}', {1}", $DPKFilePath, [System.DateTime]::Now) | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

    #Gets the file name of the DPK file from the file's full path
    [System.String]$DPKFileName = $DPKFilePath.Substring($DPKFilePath.LastIndexOf("\") + 1);

    $DPKFileName;

    $OA3OutputXmlFilePath = ($RootDir + "\DPKOutputs\" + $DPKFileName);

    $OA3OutputBinFilePath = ($OA3OutputXmlFilePath + ".bin");

    #Sets the input file in the oa3tool configuration
    $OA3ToolConfiguration.OA3.FileBased.InputKeyXMLFile = $DPKFilePath;

    #Sets the ouput .bin file in the oa3tool configuration
    $OA3ToolConfiguration.OA3.OutputData.AssembledBinaryFile = $OA3OutputBinFilePath;

    #Sets the ouput .xml report file in the oa3tool configuration
    $OA3ToolConfiguration.OA3.OutputData.ReportedXMLFile = $OA3OutputXmlFilePath;
    
    [System.String]$OA3ToolConfigurationFilePath = ($OA3OutputXmlFilePath + ".cfg");

    #Saves the content of oa3tool configuration to the specified file location
    $OA3ToolConfiguration.Save($OA3ToolConfigurationFilePath);

    #Invokes OA3Tool.exe /Assemble to generate .bin file and output DPK info xml file
    try
    {
       $Message = [System.String]::Format("Assembling DPK..., {0}", [System.DateTime]::Now);
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
       
       #& ($RootDir + "\OA3Tool9600\amd64\oa3tool.exe") @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) | Out-File -FilePath ($RootDir + "\production-log.log") -Append;

       #Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\oa3tool-assemble-" + $TransactionID + ".log");
       Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Assemble",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath)) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-assemble.log");
    }
    catch [System.Exception]
    {
       $Message = $Error[0].Exception;
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "Errors occurred!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
    }

    ##Runs DPK Injection Tool Here
    try
    {
       $Message = [System.String]::Format("Injecting DPK..., {0}", [System.DateTime]::Now);
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
    }
    catch [System.Exception]
    {
       $Message = $Error[0].Exception;
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "Errors occurred!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
    }

    #Invokes OA3Tool.exe /Validate to test if the DPK injection is successful
    try
    {
        $Message = [System.String]::Format("Validating ACPI MSDM table..., {0}", [System.DateTime]::Now);
        $Message;
        $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
       
        $Message = & ($OA3ToolPath) @("/Validate");
        $Message;
        $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

        if($Message.Contains("The operation completed successfully.") -eq $false)
        {
           $Host.UI.RawUI.BackgroundColor = "Red";
           $Host.UI.RawUI.ForegroundColor = "Yellow";
           Write-Host -Object "Could NOT find a DPK injected, ACPI MSDM table is empty!";
           Read-Host -Prompt "Press any key to exit...";
           exit;
        }
        else
        {
           Write-Host -Object "OK.";
        }
    }
    catch [System.Exception]
    {
        $Message = $Error[0].Exception;
        $Message;
        $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

        $Host.UI.RawUI.BackgroundColor = "Red";
        $Host.UI.RawUI.ForegroundColor = "Yellow";
        Write-Host -Object "Error(s) occurred during ACPI MSDM table validation!";
        Read-Host -Prompt "Press any key to exit...";
        exit;
    }

    #Invokes OA3Tool.exe /Report to generate output DPK info xml file
    try
    {
        $Message = [System.String]::Format("Reporting DPK..., {0}", [System.DateTime]::Now);
        $Message;
        $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

        #& ($RootDir + "\OA3Tool9600\amd64\oa3tool.exe") @("/Report",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) | Out-File -FilePath ($RootDir + "\production-log.log") -Append;

        #Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\oa3tool-report-" + $TransactionID + ".log");
        Start-Process -FilePath $OA3ToolPath -ArgumentList @("/Report",  ("/ConfigFile=" + $OA3ToolConfigurationFilePath), ("/LogTrace=" + $OA3OutputXmlFilePath + ".log.xml")) -Wait -NoNewWindow -RedirectStandardOutput ($LogPath + "\" + $TransactionID + "-oa3tool-report.log");
    }
    catch [System.Exception]
    {
       $Message = $Error[0].Exception;
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

       $Host.UI.RawUI.BackgroundColor = "Red";
       $Host.UI.RawUI.ForegroundColor = "Yellow";
       Write-Host -Object "Errors occurred!";
       Read-Host -Prompt "Press any key to exit...";
       exit;
    }
    
    #Appends OHR data to DPK info xml file generated
    try
    {
       [xml]$OA3OutputXml = Get-Content -Path $OA3OutputXmlFilePath -Encoding UTF8;
    
       if($OA3OutputXml.Key.OEMOptionalInfo -ne $null)
       {
         $OA3OutputXml.key.RemoveChild($OA3OutputXml.Key.OEMOptionalInfo);
       }

       $OHRNodes = $OA3OutputXml.ImportNode($OemHardwareReportData.OEMOptionalInfo, $true);
  
       #$OA3OutputXml.Key.AppendChild($OHRNodes);

       #$OA3OutputXml.Key.InsertAfter($OHRNodes, $OA3OutputXml.Key.HardwareHash);
       $OA3OutputXml.Key.InsertAfter($OHRNodes, $OA3OutputXml.Key.SelectSingleNode("HardwareHash"));

       $OA3OutputXml.Save($OA3OutputXmlFilePath);
    }
    catch [System.Exception]
    {
       $Message = $Error[0].Exception;
       $Message;
       $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
    }

    #Copies the input DPK xml file to the directory that archives all of the DPK xml files, and renames it to be ending with ".input"
    Copy-Item -Path $DPKFilePath -Destination ($OA3OutputXmlFilePath + ".input") -Force;
    
    $NewDPKFileName = $DPKFilePath + ".bak";

    #Renames the current DPK input xml in the folder that contains the DPK xml files exported from DIS to be ended with ".bak"
    Rename-Item $DPKFilePath -NewName $NewDPKFileName;

    $Message = [System.String]::Format("Finish processing '{0}', {1}", $DPKFilePath, [System.DateTime]::Now);
    $Message;
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
}
else
{
    $Message = [System.String]::Format("No matching input found, {0}", [System.DateTime]::Now);
    $Message; 
    $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
}


##Creates DPKID-SN pair, and saves the pair to the xml file of DPKID-SN.xml in the root directory
#if($MatchCount -eq 2)
if(([System.String]::IsNullOrEmpty($SerialNumber) -eq $false) -and ($MatchCount -eq 2))
{
   try
   {
      Import-Module ($RootDir + "\PowerShellModules\PowerShellOA3DPKSNBinder.dll");

      [xml]$ProductKeyInfo = [xml](Get-Content -Path $OA3OutputXmlFilePath); 

      $ProductKeyID = $ProductKeyInfo.Key.ProductKeyID;

      if($ProductKeyID -ne $null)
      {
         $PairID = Add-DPKIDSNBinding -ProductKeyID $ProductKeyID -SerialNumber $SerialNumber -TransactionID $TransactionID -PersistencyMode FileSystemXML -FilePath ($RootDir + "\DPKID-SN.xml");

         $Message = [System.String]::Format("Pair created, Product Key ID: {0}, Serial Number: {1}, Pair ID: {2}, {3}", $ProductKeyID, $SerialNumber, $PairID, [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

         #$Message = [System.String]::Format("Shutting down..., {0}", [System.DateTime]::Now);
         #$Message;
         #$Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

         #Adding serial number to OA3Tool output XML:
         if($ProductKeyInfo.Key.SerialNumber -ne $null)
         {
            $ProductKeyInfo.Key.SerialNumber = $SerialNumber;
         }
         else
         {
            [xml]$SerialNumberXml = [System.String]::Format("<SerialNumber>{0}</SerialNumber>", $SerialNumber);

            $SerialNumberNode = $ProductKeyInfo.ImportNode($SerialNumberXml.FirstChild, $true);

            $ProductKeyInfo.Key.AppendChild($SerialNumberNode);

            $ProductKeyInfo.Key.SerialNumber;
         }

         $ProductKeyInfo.Save($OA3OutputXmlFilePath);

         $IsShowingDPKIDQRCode = (($IsShowingBarcode.ToLower() -eq "true") -or ($IsShowingBarcode -eq "1"));

         if($IsShowingDPKIDQRCode -eq $true)
         {
            Show-SNBarcode -BarcodeValue $ProductKeyID -BarcodeType QR_CODE -ImageWidth 300 -ImageHeight 300 -IsShowingBarcodeText $true;
         }

         $Host.UI.RawUI.BackgroundColor = "Green";
         $Host.UI.RawUI.ForegroundColor = "Black";
         $Message = "OA3.0 process completed successfully!";
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;

         #& ("wpeutil") @("shutdown");

         [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms");

         $DialogResult = [System.Windows.Forms.MessageBox]::Show("OA3.0 process completed successfully! Shutdown unit now?" , "Success" , 4);

         if($DialogResult -eq "YES")
         {
            Stop-Computer -ComputerName "localhost";
         }
      }
      else
      {
         $Host.UI.RawUI.BackgroundColor = "Red";
         $Host.UI.RawUI.ForegroundColor = "Yellow";
         $Message = [System.String]::Format("Could not find product key ID, {0}", [System.DateTime]::Now);
         $Message;
         $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
      }
   }
   catch [System.Exception]
   {
      $Message = $Error[0].Exception;
      $Message;
      $Message | Out-File -FilePath ($LogPath + "\production-log.log") -Append;
   }
} 