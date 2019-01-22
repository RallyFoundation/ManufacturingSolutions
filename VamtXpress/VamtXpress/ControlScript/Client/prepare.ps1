#
# prepare.ps1
#
New-ItemProperty -Path Registry::HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system -Name LocalAccountTokenFilterPolicy -PropertyType DWord -Value 1

netsh advfirewall firewall set rule group = "Windows Management Instrumentation (WMI)" new enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (Async-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (DCOM-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule name = "Windows Management Instrumentation (WMI-in)" new remoteip = any enable = yes

netsh advfirewall firewall set rule group = "File and Printer Sharing" new enable = yes

netsh advfirewall firewall set rule group = "Remote Administration" new enable = yes