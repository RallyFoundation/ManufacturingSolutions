<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <!--xmlns:hhvxslx="HHValidation.XsltExt"-->
    <xsl:output method="html" indent="yes" standalone="yes"/>
    <xsl:param name="transactionId"></xsl:param>
    <xsl:param name="productKeyId"></xsl:param>
    <xsl:param name="mode">online</xsl:param>
    <xsl:param name="detailXslt">xslt-report-detail.xslt</xsl:param>
    <xsl:param name="rootDir"></xsl:param>
  <xsl:template match="/">
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
         Result-
         <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'-'"/>
            <xsl:value-of select="$productKeyId"/>
        </title>
      </head>
      <body>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="'file:///'"/>
            <xsl:value-of select="$rootDir"/>
            <xsl:value-of select="'\Output\'"/>
            <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'_'"/>
            <xsl:value-of select="$productKeyId"/>
            <xsl:value-of select="'_All.zip#save'"/>
          </xsl:attribute>
          Download Result as Zip
        </a>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="'file:///'"/>
            <xsl:value-of select="$rootDir"/>
            <xsl:value-of select="'\Log\'"/>
            <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'.log'"/>
          </xsl:attribute>
          View Log
        </a>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <a target="_self">
          <xsl:attribute name="href">
            <xsl:value-of select="'#'"/>
          </xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:value-of select="'window.external.DoClose()'"/>
          </xsl:attribute>
          Close
        </a>
        <hr/>
        <p style="font-weight:bolder;font-size:x-large;font-family:Arial">Total Result: 
          <xsl:call-template name="TotalResult"/>
        </p>
        <hr/>
        <ul>
          <xsl:call-template name="SystemInfo"></xsl:call-template>
          <xsl:call-template name="HardwareBasedPriceInfo"></xsl:call-template>
          <xsl:call-template name="SmbiosInfo"></xsl:call-template>
        </ul>
      </body>
    </html>
    </xsl:template>

  <xsl:template name="TotalResult">
    <xsl:variable name="totalItemCount" select="count(//Result)"/>
    <xsl:variable name="passedItemCount" select="count(//Result[./text() = 'Passed'])"/>
    <xsl:variable name="failedItemCount" select="count(//Result[./text() = 'Failed'])"/>
    <Span>
      <xsl:if test="$totalItemCount = $passedItemCount">
        <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
        Passed
      </xsl:if>
      <xsl:if test="($totalItemCount &gt; $passedItemCount) and ($failedItemCount &gt; 0)">
        <xsl:attribute name="style">background-color:red</xsl:attribute>
        <xsl:value-of select="$failedItemCount"/>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:if test="$failedItemCount = 1">
          Field
        </xsl:if>
        <xsl:if test="$failedItemCount &gt; 1">
          Fields
        </xsl:if>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        Failed
      </xsl:if>
    </Span>
  </xsl:template>
  
  <xsl:template name="SystemInfo">
    <xsl:variable name="ProductKeyID" select="/TestItems/ProductKeyID"/>
    <xsl:variable name="OSType" select="/TestItems/OSType"/>
    <xsl:variable name="IsPassed" select="($ProductKeyID/Result = 'Passed') and ($OSType/Result = 'Passed')"/>
    <li>
        <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">System Info:
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <xsl:if test="$IsPassed = true()">Passed</xsl:if>
            <xsl:if test="$IsPassed = false()">Failed</xsl:if>
        </p>
        <ul>
            <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>Product Key ID: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;ProductKeyID&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$ProductKeyID/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ProductKeyID/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>OS Type: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <!--window.external.DoTransform('OSType', 'xslt-report-detail.xslt');-->
                      <xsl:value-of select="'window.external.DoTransform(&quot;OSType&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$OSType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$OSType/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>OS Build Number: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;OSType&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$OSType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$OSType/Detail/OSBuild"/>
                  </td>
                </tr>
                <tr>
                  <td>MAC Address: </td>
                  <td>
                  </td>
                </tr>
            </table>   
        </ul>    
    </li>
  </xsl:template>
  
  <xsl:template name="SmbiosInfo">
    <xsl:variable name="ChassisType" select="/TestItems/ChassisType"/>
    <xsl:variable name="SmbiosSystemManufacturer" select="/TestItems/SmbiosSystemManufacturer"/>
    <xsl:variable name="SmbiosSystemFamily" select="/TestItems/SmbiosSystemFamily"/>
    <xsl:variable name="SmbiosSystemProductName" select="/TestItems/SmbiosSystemProductName"/>
    <xsl:variable name="SmbiosBoardProduct" select="/TestItems/SmbiosBoardProduct"/>
    <xsl:variable name="SmbiosSkuNumber" select="/TestItems/SmbiosSkuNumber"/>
    <xsl:variable name="SmbiosSystemSerialNumber" select="/TestItems/SmbiosSystemSerialNumber"/>
    <xsl:variable name="SmbiosUuid" select="/TestItems/SmbiosUuid"/>
    <xsl:variable name="IsPassed" select="($ChassisType/Result = 'Passed') and ($SmbiosSystemManufacturer/Result = 'Passed') and ($SmbiosSystemFamily/Result = 'Passed') and ($SmbiosSystemProductName/Result = 'Passed') and ($SmbiosBoardProduct/Result = 'Passed') and ($SmbiosSkuNumber/Result = 'Passed') and ($SmbiosSystemSerialNumber/Result = 'Passed') and ($SmbiosUuid/Result = 'Passed')"/>
    <li>
        <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">Smbios Fields:
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <xsl:if test="$IsPassed = true()">Passed</xsl:if>
            <xsl:if test="$IsPassed = false()">Failed</xsl:if>
        </p>
        <ul>
            <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>ChassisType: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;ChasisType&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$ChassisType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ChassisType/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>SmbiosSystemManufacturer: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosSystemManufacturer&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosSystemManufacturer/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosSystemManufacturer/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>SmbiosSystemFamily: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosSystemFamily&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosSystemFamily/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosSystemFamily/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>SmbiosSystemProductName: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosSystemProductName&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosSystemProductName/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosSystemProductName/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>SmbiosBoardProduct: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosBoardProduct&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosBoardProduct/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosBoardProduct/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>SmbiosSkuNumber: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosSkuNumber&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosSkuNumber/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosSkuNumber/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>SmbiosSystemSerialNumber: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosSystemSerialNumber&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosSystemSerialNumber/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosSystemSerialNumber/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>SmbiosUuid: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;SmbiosUuid&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$SmbiosUuid/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$SmbiosUuid/Value"/> 
                  </td>
                </tr>
            </table>   
        </ul>    
    </li>
  </xsl:template>
  
  <xsl:template name="HardwareBasedPriceInfo">
    <xsl:variable name="ChassisType" select="/TestItems/ChassisType"/>
    <xsl:variable name="DigitizerSupportID" select="/TestItems/DigitizerSupportID"/>
    <xsl:variable name="ProcessorModel" select="/TestItems/ProcessorModel"/>
    <xsl:variable name="TotalPhysicalRAM" select="/TestItems/TotalPhysicalRAM"/>
    <xsl:variable name="PrimaryDiskType" select="/TestItems/PrimaryDiskType"/>
    <xsl:variable name="PrimaryDiskTotalCapacity" select="/TestItems/PrimaryDiskTotalCapacity"/>
    <xsl:variable name="DisplayResolutionHorizontal" select="/TestItems/DisplayResolutionHorizontal"/>
    <xsl:variable name="DisplayResolutionVertical" select="/TestItems/DisplayResolutionVertical"/>
    <xsl:variable name="DisplaySizePhysicalH" select="/TestItems/DisplaySizePhysicalH"/>
    <xsl:variable name="DisplaySizePhysicalY" select="/TestItems/DisplaySizePhysicalY"/>
    <xsl:variable name="IsPassed" select="($ChassisType/Result = 'Passed') and ($DigitizerSupportID/Result = 'Passed') and ($ProcessorModel/Result = 'Passed') and ($TotalPhysicalRAM/Result = 'Passed') and ($PrimaryDiskType/Result = 'Passed') and ($PrimaryDiskTotalCapacity/Result = 'Passed') and ($DisplayResolutionHorizontal/Result = 'Passed') and ($DisplayResolutionVertical/Result = 'Passed') and ($DisplaySizePhysicalH/Result = 'Passed') and ($DisplaySizePhysicalY/Result = 'Passed')"/>
    <li>
        <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">Hardware-Based Price Fields:
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <xsl:if test="$IsPassed = true()">Passed</xsl:if>
            <xsl:if test="$IsPassed = false()">Failed</xsl:if>
        </p>
        <ul>
            <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>ChassisType: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;ChassisType&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$ChassisType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ChassisType/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>ProcessorModel: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;ProcessorModel&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$ProcessorModel/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ProcessorModel/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>TotalPhysicalRAM: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;TotalPhysicalRAM&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$TotalPhysicalRAM/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$TotalPhysicalRAM/Value"/>
                  </td>
                </tr>
                <tr>
                  <td>PrimaryDiskType: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;PrimaryDiskType&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$PrimaryDiskType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$PrimaryDiskType/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>PrimaryDiskTotalCapacity: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;PrimaryDiskTotalCapacity&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$PrimaryDiskTotalCapacity/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$PrimaryDiskTotalCapacity/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>DisplayResolutionHorizontal: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;DisplayResolutionHorizontal&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$DisplayResolutionHorizontal/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$DisplayResolutionHorizontal/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>DisplayResolutionVertical: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;DisplayResolutionVertical&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$DisplayResolutionVertical/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$DisplayResolutionVertical/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>DisplaySizePhysicalH: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;DisplaySizePhysicalH&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$DisplaySizePhysicalH/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$DisplaySizePhysicalH/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>DisplaySizePhysicalY: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;DisplaySizePhysicalY&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$DisplaySizePhysicalY/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$DisplaySizePhysicalY/Value"/> 
                  </td>
                </tr>
                <tr>
                  <td>DigitizerSupportID: </td>
                  <td>
                    <xsl:attribute name="onclick">
                      <xsl:value-of select="'window.external.DoTransform(&quot;DigitizerSupportID&quot;, &quot;'"/>
                      <xsl:value-of select="$detailXslt"/>
                      <xsl:value-of select="'&quot;);'"/>
                    </xsl:attribute>
                    <xsl:if test="$DigitizerSupportID/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$DigitizerSupportID/Value"/> 
                  </td>
                </tr>
            </table>   
        </ul>    
    </li>
  </xsl:template>
</xsl:stylesheet>
