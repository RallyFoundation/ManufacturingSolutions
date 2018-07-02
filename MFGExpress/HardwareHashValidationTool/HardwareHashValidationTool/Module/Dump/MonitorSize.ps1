

$votNames = (
        "HD15",
        "SVIDEO",             
        "COMPOSITE_VIDEO",
        "COMPONENT_VIDEO",
        "DVI",
        "HDMI",
        "LVDS",
        "8",
        "D_JPN",
        "SDI",
        "DISPLAYPORT_EXTERNAL",
        "DISPLAYPORT_EMBEDDED",
        "UDI_EXTERNAL",
        "UDI_EMBEDDED",
        "SDTVDONGLE",
        "MIRACAST"
);

$monitors = Get-WmiObject -Namespace root\wmi -Clas WmiMonitorId

foreach ($mon in $monitors)
{
    $filter = "InstanceName='"+ $mon.InstanceName + "'";
    $filter = $filter.Replace("\", "\\");

    $conn = (Get-WmiObject -Namespace root\wmi -Class WmiMonitorConnectionParams -Filter $filter)

    $edid = (Get-WmiObject -NameSpace root\wmi -Class WmiMonitorDescriptorMethods -Filter $filter).WmiGetMonitorRawEEdidV1Block(0).BlockContent;

    ##
    ## Interpret data
    ## 

    $xcm = $edid[21];
    $ycm = $edid[22];

    ## x:68[7:5] + 66, y:68[4:0]+67

    $xmm = $edid[66] + (($edid[68] -shr 4) -band 0xF) * 256;
    $ymm = $edid[67] + (($edid[68] -shr 0) -band 0xF) * 256;

    $xpx = $edid[56] + (($edid[58] -shr 4) -band 0xF) * 256;
    $ypx = $edid[59] + (($edid[61] -shr 4) -band 0xF) * 256;

    $vot = $conn.VideoOutputTechnology;

    if (($vot -gt 0) -and ($vot -bAnd 0x80000000))
    {
        $loc = "internal";
        $vo2 = $vot -bAnd 0x7FFFFFFF;
    }
    else
    {
        $loc = "external";
        $vo2 = $vot;
    }

    if (($vo2 -ge 0) -and ($vo2 -lt $votNames.Count))
    {
        $votName = $votNames[$vo2];
    }
    else
    {
        $votName = "???";
    }

    Write-Host "=====================================================`r"
    Write-Host "Monitor  : $($mon.InstanceName)`r";
    Write-Host "  Pixels : $xpx x $ypx`r";
    Write-Host "  Size   : $xcm x $ycm cm ($xmm x $ymm mm)`r";
    Write-Host "  Active : $($conn.Active)`r";
    Write-Host "  VOTech : $("{1:D2} - {2}, {3} [0x{0:X}]" -f $vot, $vo2, $votName, $loc)`r";
    Write-Host "`r"

    ##
    ## Do a hex dump first
    ## 

    Write-Host "EDID:  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F`r"

    $off = 0;
    $hex = [System.Bitconverter]::ToString($edid);
    $str = "";

    while ($hex.Length -ne 0)
    {
        if ($hex.Length -gt 48)
        {
            $str = $hex.Substring(0, 48);
            $hex = $hex.Substring(48);
        }
        else
        {
            $str = $hex;
            $hex = "";
        }
        $str = $str.Replace('-', ' ').Trim();

        Write-Host "$("{0:X4}: {1}" -f $off, $str)`r";
        $off = $off + 16;
    }

    Write-Host "`r"
}
