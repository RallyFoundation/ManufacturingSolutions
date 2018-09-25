$smb = get-wmiobject MSSmBios_RawSMBiosTables -namespace root\wmi

$majv = $smb.SmbiosMajorVersion;
$minv = $smb.SmbiosMinorVersion;
$size = $smb.Size;

Write-Host "SMBIOS dump for '$env:ComputerName'`r";
Write-Host "Version: $majv.$minv`r";
Write-Host "Size   : $size bytes`r";
Write-Host "`r";

$bin = $smb.SMBiosData;

Write-Host "OFFSET   0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F   ASCII`r";

$hex = "";
$asc = "";
$off = 0;

$cnt = 0;

foreach ($byte in $bin) {

    $hex += "{0:x2} " -f $byte ;

    if ($byte -ge 0x20 -and $byte -le 0x7E) {
        $asc += [char] $byte;
    } else {
        $asc += ".";
    }

    if (++$cnt -eq 16) {
        Write-Host "$("{0:x4}`t{1,-49}  {2}" -f $off, $hex, $asc)`r";
        $off = $off + $cnt;
        $cnt = 0;
        $hex = "";
        $asc = "";
    } elseif ($cnt -eq 8) {
        $hex += " ";
    }
}

if ($cnt -ne 0) {
    Write-Host "$("{0:x4}`t{1,-49}  {2}" -f $off, $hex, $asc)`r";
}


# Size, SMBiosData, SmbiosMajorVersion, SmbiosMinorVersion