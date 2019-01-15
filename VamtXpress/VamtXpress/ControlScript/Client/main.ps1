Set-ExecutionPolicy -ExecutionPolicy Bypass -Force;

.\TurnOnFirewallSettings.ps1;

.\AddRegistryKey4WorkGroupAccess.ps1;

.\InvokeVamtAPI.ps1;

.\RemoveRegistryKey4WorkGroupAccess.ps1;

.\TurnOffFirewallSettings.ps1;

