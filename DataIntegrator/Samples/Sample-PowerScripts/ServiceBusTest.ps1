Import-Module "C:\Users\v-rawang\Documents\Visual Studio 2012\Projects\DataIntegrator\ServiceBusPowerShell\bin\Debug\ServiceBusPowerShell.dll"

Init-Adapters -MountPoint "D:\Adapters" -SearchPattern "*.xml"

$adapters = Show-Adapters

$adapters

$adapter =  Get-Adapter -AdapterName "D:\Adapters\AdpterTest - Copy (2) - Copy.xml"

$adapter

$adapter.Destination.Address

$adapter = Set-Adapter -AdapterName $adapters[2].ToString();

$adapter

$adapter.Transformer.Address