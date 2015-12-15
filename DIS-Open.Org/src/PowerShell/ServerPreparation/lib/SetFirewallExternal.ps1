##Turning on Firewall Settings to Allow Inbound/Outbound Access to DIS External Web Services
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8001-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8001 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8001-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8001 RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8011-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8011 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8011-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8011 RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8021-In" Dir=In Action=Allow Protocol=TCP LocalPort = 8021 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "DIS-ExternalWebService-8021-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 8021 RemoteIP=Any