param($ClientHostName, $ClientUserName, $ClientPassword, $VamtPSModulePath="C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\VAMT3\VAMT.psd1")

#Remember to check or replace the following path corrosponding to your VAMT installation 
#cd "C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\VAMT3"

#cd $VamtPSModulePath;

#Import-Module .\VAMT.psd1

Import-Module -Name $VamtPSModulePath;

$ProductInfo = Find-VamtManagedMachine -QueryType Manual -QueryValue $ClientHostName -Username $ClientUserName -Password $ClientPassword;
$ProductInfo = Update-VamtProduct -Products $ProductInfo -Username $ClientUserName -Password $ClientPassword;
$ProductInfo = Get-VamtConfirmationId -Products $ProductInfo;
$ProductInfo = Install-VamtConfirmationId -Products $ProductInfo -Username $ClientUserName -Password $ClientPassword;

$ProductInfo;