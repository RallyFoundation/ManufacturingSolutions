echo Turning on SQL Server firewall settings... 
Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-In" Dir=In Action=Allow Protocol=TCP LocalPort = 1433 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 1433 RemoteIP=Any
echo Done.
echo Turning on File and Priner Sharing firewall settings...
Netsh AdvFirewall Firewall Set Rule Group = "File and Printer Sharing" New Enable = Yes
echo Done.
echo Turning on SQL Server remote connection settings and configuring SQL Server mixed authentication mode...
sqlcmd -S . -i .\SetDISDB.sql -o .\SQLOutput_SetDISDB.txt
echo Done.

