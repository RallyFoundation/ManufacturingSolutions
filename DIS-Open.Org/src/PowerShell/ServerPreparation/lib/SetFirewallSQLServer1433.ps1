﻿##Turning on Firewall Settings to Allow Inbound/Outbound Access to SQL Server
Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-In" Dir=In Action=Allow Protocol=TCP LocalPort = 1433 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "SQLServer-1433-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 1433 RemoteIP=Any