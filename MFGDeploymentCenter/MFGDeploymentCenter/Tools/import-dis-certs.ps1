$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

cd $rootDir;

Import-Module .\DIS.Management.Security.dll;
Import-X509Certificate -Path (Get-ChildItem .\CA.cer).FullName -StoreLocation LocalMachine -StoreName Root;
Import-X509Certificate -Path (Get-ChildItem .\IIS.pfx).FullName -StoreLocation LocalMachine -StoreName My -Password 123;
Import-X509Certificate -Path (Get-ChildItem .\CFG.pfx).FullName -StoreLocation LocalMachine -StoreName My -Password 123;
Import-X509Certificate -Path (Get-ChildItem .\KMTforR5.pfx).FullName -StoreLocation LocalMachine -StoreName My -Password 123;