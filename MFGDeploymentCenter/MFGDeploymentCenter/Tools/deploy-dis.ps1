$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

if($rootDir.EndsWith("\") -eq $false)
{
   $rootDir = $rootDir + "\";
}

cd $rootDir;

#Prepare DB
.\preparation\db\run.ps1;

cd $rootDir;

#Restart SQL
..\Tools\restart-sql-service.ps1;

cd $rootDir;

#Prepare Server
.\preparation\run.ps1;

cd $rootDir;

#Import Cert
.\installation\lib\import-dis-certs.ps1;

cd $rootDir;

#Install Cloud
[System.String]$unattend = $rootDir;
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend + "\Tools\unattend-cloud.xml";
.\installation\install.ps1 -unattend $unattend;

cd $rootDir;

#Install CKI
$unattend = $rootDir;
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend + "\Tools\unattend-oem.xml";
.\installation\install.ps1 -unattend $unattend;

cd $rootDir;

#Install TPI
$unattend = $rootDir;
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend + "\Tools\unattend-tpi.xml";
.\installation\install.ps1 -unattend $unattend;

cd $rootDir;

#Install FFKI
$unattend = $rootDir;
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend.Substring(0, $unattend.LastIndexOf("\"));
$unattend = $unattend + "\Tools\unattend-ff.xml";
.\installation\install.ps1 -unattend $unattend;




