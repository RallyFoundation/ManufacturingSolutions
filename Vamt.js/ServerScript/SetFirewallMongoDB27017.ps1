﻿Netsh AdvFirewall Firewall Add Rule Name = "MongoDB-27017-In" Dir=In Action=Allow Protocol=TCP LocalPort = 27017 LocalIP=Any RemotePort=Any RemoteIP=Any
Netsh AdvFirewall Firewall Add Rule Name = "MongoDB-27017-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 27017 RemoteIP=Any