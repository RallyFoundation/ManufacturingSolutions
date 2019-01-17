##Turning on Firewall Settings to Allow Inbound/Outbound Access to OData (8096)
Netsh AdvFirewall Firewall Add Rule Name = "OData-8096-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8096 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "OData-8096-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8096 RemoteIP=Any