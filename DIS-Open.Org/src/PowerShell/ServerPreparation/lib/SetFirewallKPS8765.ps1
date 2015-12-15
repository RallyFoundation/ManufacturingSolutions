##Turning on Firewall Settings to Allow Inbound/Outbound Access to KPS
Netsh AdvFirewall Firewall Add Rule Name = "KPS-8765-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8765 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "KPS-8765-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8765 RemoteIP=Any