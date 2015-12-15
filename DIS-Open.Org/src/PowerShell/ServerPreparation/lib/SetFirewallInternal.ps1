##Turning on Firewall Settings to Allow Inbound/Outbound Access to DIS Internal Web Services
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8002-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8002 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8002-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8002 RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8012-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8012 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8012-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8012 RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8022-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8022 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-InternalWebService-8022-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8022 RemoteIP=Any