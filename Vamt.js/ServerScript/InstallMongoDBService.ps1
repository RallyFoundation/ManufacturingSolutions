mkdir D:\MongoDB\Data\db\;
mkdir D:\MongoDB\Data\log\;
copy .\mongod.cfg 'C:\Program Files\MongoDB\Server\3.4';
& 'C:\Program Files\MongoDB\Server\3.4\bin\mongod.exe' --install --config 'C:\Program Files\MongoDB\Server\3.4\mongod.cfg';

##Turning on Firewall Settings to Allow Inbound/Outbound Access to MongoDB 27017
Netsh AdvFirewall Firewall Add Rule Name = "MongoDB-27017-In" Dir=In Action=Allow Protocol=TCP LocalPort = 27017 LocalIP=Any RemotePort=Any RemoteIP=Any;
Netsh AdvFirewall Firewall Add Rule Name = "MongoDB-27017-Out" Dir=Out Action=Allow Protocol=TCP LocalPort=Any LocalIP=Any RemotePort = 27017 RemoteIP=Any;

Start-Service -Name MongoDB;