##Turning on Firewall Settings to Allow Inbound/Outbound Access to Redis (6379)
Netsh AdvFirewall Firewall Add Rule Name = "Redis-6379-In" Dir=In Action=Allow Protocol=TCP LocalPort = 6379 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "Redis-6379-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 6379 RemoteIP=Any