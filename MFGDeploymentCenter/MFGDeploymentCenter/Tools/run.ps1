$rootDir = Split-Path -parent $MyInvocation.MyCommand.Definition;

cd $rootDir;

#.\run.cmd

Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-In" Dir=In Action=Allow Protocol=TCP LocalPort = 1433 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 1433 RemoteIP=Any
Netsh AdvFirewall Firewall Set Rule Group = "File and Printer Sharing" New Enable = Yes
sqlcmd -S .\sqlexpress -i .\SetMESDB.sql -o .\SQLOutput_SetMESDB.txt