echo Turning off SQL Server firewall settings... 
Netsh AdvFirewall Firewall Delete Rule Name = "SQLServer-1433-In"
Netsh AdvFirewall Firewall Delete Rule Name = "SQLServer-1433-Out"
echo Done.
echo Turning off File and Priner Sharing firewall settings...
Netsh AdvFirewall Firewall Set Rule Group = "File and Printer Sharing" New Enable = No
echo Done.
echo Turning off SQL Server remote connection settings and configuring SQL Server authentication mode...
sqlcmd -S . -i .\ResetDISDB.sql -o .\SQLOutput_SetDISDB.txt
echo Done.

