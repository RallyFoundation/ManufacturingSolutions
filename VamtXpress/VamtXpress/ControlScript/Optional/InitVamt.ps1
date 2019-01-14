#Remember to check or replace the following path corrosponding to your VAMT installation
cd 'C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\VAMT3'

Import-Module .\VAMT.psd1

Initialize-VamtData -Confirm $false