netsh advfirewall firewall set rule group = "Windows Management Instrumentation (WMI)" new enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (Async-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (DCOM-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (WMI-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule group = "File and Printer Sharing" new enable = yes

#netsh firewall set service RemoteAdmin enable

netsh advfirewall firewall set rule group = "Remote Administration" new enable = yes