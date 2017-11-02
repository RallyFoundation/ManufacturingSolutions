<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:hhvxslx="HHValidation.XsltExt">
    <xsl:output method="html" indent="yes" standalone="yes"/>
    <xsl:param name="transactionId"></xsl:param>
    <xsl:param name="productKeyId"></xsl:param>
  <xsl:param name="mode">online</xsl:param>
  <xsl:template match="/">
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
         Result-
         <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'-'"/>
            <xsl:value-of select="$productKeyId"/>
        </title>
        <!--<script type="text/javascript">
          function SetWindowSize()
          {
              window.resizeTo((screen.width - 6), (screen.height - 20));
          }
        </script>-->
      </head>
      <body>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <!--<a>
          <xsl:attribute name="href">
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
            <xsl:value-of select="'../Log/'"/>
            <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'.log'"/>
          </xsl:attribute>
          View Log
        </a>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <a target="_self">
          <xsl:attribute name="href">
            <xsl:value-of select="'#close'"/>
          </xsl:attribute>
          Close
        </a>-->
        <hr/>
        <p style="font-weight:bolder;font-size:x-large;font-family:Arial">Total Result: 
          <xsl:call-template name="TotalResult"/>
        </p>
        <hr/>
        <ul>
          <li>
            <!--<p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">System Info:</p>-->
            <xsl:call-template name="SystemInfo"/>
            <!--<ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">Product Key ID: </p>
                <xsl:apply-templates select="TestItems/ProductKeyID">
                </xsl:apply-templates>
              </li>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">OS Type: </p>
                <xsl:apply-templates select="TestItems/OSType">
                </xsl:apply-templates>
              </li>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">Mac Address: </p>
                <xsl:apply-templates select="TestItems/NIC">
                </xsl:apply-templates>
              </li>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">OA3Tool: </p>
                <xsl:apply-templates select="TestItems/OA3Tool">
                </xsl:apply-templates>
              </li>
            </ul>-->    
          </li>
          <li>
            <!--<p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">Hardware-Based Price Fields:</p>-->
            <xsl:call-template name="HardwareBasedPriceInfo"/>
            <!--<ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ChassisType: </p>
                <xsl:apply-templates select="TestItems/ChassisType">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ProcessorModel: </p>
                <xsl:apply-templates select="TestItems/ProcessorModel">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">TotalPhysicalRAM: </p>
                <xsl:apply-templates select="TestItems/TotalPhysicalRAM">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">PrimaryDiskType: </p>
                <xsl:apply-templates select="TestItems/PrimaryDiskType">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">PrimaryDiskTotalCapacity: </p>
                <xsl:apply-templates select="TestItems/PrimaryDiskTotalCapacity">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplayResolutionHorizontal: </p>
                <xsl:apply-templates select="TestItems/DisplayResolutionHorizontal">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplayResolutionVertical: </p>
                <xsl:apply-templates select="TestItems/DisplayResolutionVertical">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplaySizePhysicalH: </p>
                <xsl:apply-templates select="TestItems/DisplaySizePhysicalH">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplaySizePhysicalY: </p>
                <xsl:apply-templates select="TestItems/DisplaySizePhysicalY">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
                <li>
                  <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DigitizerSupportID: </p>
                  <xsl:apply-templates select="TestItems/DigitizerSupportID">
                  </xsl:apply-templates>
                </li>
              </ul>-->
          </li>
          <li>
            <!--<p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">Smbios Fields:</p>-->
            <xsl:call-template name="SmbiosInfo"/>
            <!--<ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ChassisType: </p>
                <xsl:apply-templates select="TestItems/ChassisType">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemManufacturer: </p>
                <xsl:apply-templates select="TestItems/SmbiosSystemManufacturer">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemFamily: </p>
                <xsl:apply-templates select="TestItems/SmbiosSystemFamily">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemProductName: </p>
                <xsl:apply-templates select="TestItems/SmbiosSystemProductName">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosBoardProduct: </p>
                <xsl:apply-templates select="TestItems/SmbiosBoardProduct">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSkuNumber: </p>
                <xsl:apply-templates select="TestItems/SmbiosSkuNumber">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemSerialNumber: </p>
                <xsl:apply-templates select="TestItems/SmbiosSystemSerialNumber">
                </xsl:apply-templates>
              </li>
            </ul>
            <ul>
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosUuid: </p>
                <xsl:apply-templates select="TestItems/SmbiosUuid">
                </xsl:apply-templates>
              </li>
            </ul>-->
          </li>
        </ul>
      </body>
    </html>
    </xsl:template>

  <!--<xsl:template name="TotalResult">
    <xsl:variable name="totalItemCount" select="count(//Result)"/>
    <xsl:variable name="passedItemCount" select="count(//Result[./text() = 'Passed'])"/>
    <Span>
      <xsl:if test="$totalItemCount = $passedItemCount">
        <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
        Passed
      </xsl:if>
      <xsl:if test="$totalItemCount &gt; $passedItemCount">
        <xsl:attribute name="style">background-color:red</xsl:attribute>
        Failed
      </xsl:if>
    </Span>
  </xsl:template>-->

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
    <xsl:variable name="MacAddress" select="/TestItems/NIC"/>
    <xsl:variable name="OA3Tool" select="/TestItems/OA3Tool"/>
    <xsl:variable name="IsPassed" select="($ProductKeyID/Result = 'Passed') and ($OSType/Result = 'Passed') and ($MacAddress/Result = 'Passed') and ($OA3Tool/Result = 'Passed')"/>
      <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">
        System Info:
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:if test="$IsPassed = true()">Passed</xsl:if>
        <xsl:if test="$IsPassed = false()">Failed</xsl:if>
      </p>
      <ul>
        <xsl:if test="$ProductKeyID != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">Product Key ID: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Expected Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ProductKeyID/Expected)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ProductKeyID/Value)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$ProductKeyID/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$ProductKeyID/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$ProductKeyID/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:value-of select="$ProductKeyID/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
        <xsl:if test="$OSType != ''">
         <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">OS Type: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Expected Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($OSType/Expected)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($OSType/Value)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$OSType/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$OSType/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$OSType/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  OS Build Number:
                  <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                  <xsl:value-of select="$OSType/Detail/OSBuild"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
        <xsl:if test="$MacAddress != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">Mac Address: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($MacAddress/Min)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($MacAddress/Value)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$MacAddress/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$MacAddress/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$MacAddress/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:for-each select="$MacAddress/Detail/child::*[starts-with(name(), 'PhysicalMedium_')]">
                    <span>
                      <xsl:value-of select="name()"/>
                      <xsl:value-of select="':'"/>
                      <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                      <xsl:value-of select="text()"/>
                      <br/>
                    </span>
                  </xsl:for-each>
                </td>
              </tr>
            </table>
          </li> 
        </xsl:if>
        <xsl:if test="$OA3Tool != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">OA3Tool: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Expected Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($OA3Tool/Expected)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($OA3Tool/Value)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$OA3Tool/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$OA3Tool/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$OA3Tool/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <span>
                    Tool Version:
                    <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                    <xsl:value-of select="$OA3Tool/Detail/ToolVersion"/>
                    <br/>
                    Tool Build:
                    <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                    <xsl:value-of select="$OA3Tool/Detail/ToolBuild"/>
                  </span>      
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
      </ul>  
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
      <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">
        Smbios Fields:
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:if test="$IsPassed = true()">Passed</xsl:if>
        <xsl:if test="$IsPassed = false()">Failed</xsl:if>
      </p>
    <ul>
      <xsl:if test="$ChassisType != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ChassisType: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Expected Value: </td>
              <td>
                <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Expected)" disable-output-escaping="yes"/>
              </td>
            </tr>
            <tr>
              <td>Unexpected Value: </td>
              <td>
                <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Unexpected)" disable-output-escaping="yes"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Value)" disable-output-escaping="yes"/>
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$ChassisType/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$ChassisType/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$ChassisType/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$ChassisType/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosSystemManufacturer != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemManufacturer: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemManufacturer/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemManufacturer/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosSystemManufacturer/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosSystemManufacturer/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosSystemManufacturer/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosSystemManufacturer/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosSystemManufacturer/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosSystemManufacturer/Detail"/>
              </td>
            </tr>
          </table>
        </li> 
      </xsl:if>
      <xsl:if test="$SmbiosSystemFamily != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemFamily: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemFamily/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemFamily/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosSystemFamily/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosSystemFamily/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosSystemFamily/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosSystemFamily/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosSystemFamily/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosSystemFamily/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosSystemProductName != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemProductName: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemProductName/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemProductName/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosSystemProductName/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosSystemProductName/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosSystemProductName/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosSystemProductName/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosSystemProductName/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosSystemProductName/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosBoardProduct != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosBoardProduct: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosBoardProduct/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosBoardProduct/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosBoardProduct/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosBoardProduct/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosBoardProduct/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosBoardProduct/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosBoardProduct/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosBoardProduct/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosSkuNumber != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSkuNumber: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSkuNumber/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSkuNumber/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosSkuNumber/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosSkuNumber/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosSkuNumber/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosSkuNumber/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosSkuNumber/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosSkuNumber/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosSystemSerialNumber != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosSystemSerialNumber: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemSerialNumber/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosSystemSerialNumber/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosSystemSerialNumber/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosSystemSerialNumber/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosSystemSerialNumber/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosSystemSerialNumber/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosSystemSerialNumber/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosSystemSerialNumber/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
      <xsl:if test="$SmbiosUuid != ''">
        <li>
          <p style="font-weight:bolder;text-decoration:underline;font-style:italic">SmbiosUuid: </p>
          <table xmlns="http://www.w3.org/1999/xhtml">
            <tr>
              <td>Min Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosUuid/Min"/>
              </td>
            </tr>
            <tr>
              <td>Max Value (Length): </td>
              <td>
                <xsl:value-of select="$SmbiosUuid/Max"/>
              </td>
            </tr>
            <tr>
              <td>Test Value: </td>
              <td style="background-color:yellow">
                <xsl:value-of select="$SmbiosUuid/Value"/>
                (Length: <xsl:value-of select="string-length($SmbiosUuid/Value)"/> characters)
              </td>
            </tr>
            <tr>
              <td>Result: </td>
              <td>
                <xsl:if test="$SmbiosUuid/Result = 'Passed'">
                  <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                </xsl:if>
                <xsl:if test="$SmbiosUuid/Result = 'Failed'">
                  <xsl:attribute name="style">background-color:red</xsl:attribute>
                </xsl:if>
                <xsl:value-of select="$SmbiosUuid/Result"/>
              </td>
            </tr>
            <tr>
              <td>Detail: </td>
              <td>
                <xsl:value-of select="$SmbiosUuid/Detail"/>
              </td>
            </tr>
          </table>
        </li>
      </xsl:if>
    </ul>
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
    <xsl:variable name="IsPassed" select="($ChassisType/Result = 'Passed' or $ChassisType = '') and ($DigitizerSupportID/Result = 'Passed' or $DigitizerSupportID = '') and ($ProcessorModel/Result = 'Passed' or $ProcessorModel = '') and ($TotalPhysicalRAM/Result = 'Passed' or $TotalPhysicalRAM = '') and ($PrimaryDiskType/Result = 'Passed') and ($PrimaryDiskTotalCapacity/Result = 'Passed' or $PrimaryDiskTotalCapacity = '') and ($DisplayResolutionHorizontal/Result = 'Passed' or $DisplayResolutionHorizontal = '') and ($DisplayResolutionVertical/Result = 'Passed' or $DisplayResolutionVertical = '') and ($DisplaySizePhysicalH/Result = 'Passed' or $DisplaySizePhysicalH = '') and ($DisplaySizePhysicalY/Result = 'Passed' or $DisplaySizePhysicalY = '')"/>
      <p style="font-weight:bolder;font-size:x-large;text-decoration:underline;font-family:Arial">
        Hardware-Based Price Fields:
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <xsl:if test="$IsPassed = true()">Passed</xsl:if>
        <xsl:if test="$IsPassed = false()">Failed</xsl:if>
      </p>
      <ul>
          <xsl:if test="$ChassisType != ''">
            <li>
              <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ChassisType: </p>
                <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>Expected Value: </td>
                  <td>
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Expected)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Unexpected Value: </td>
                  <td>
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Unexpected)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Test Value: </td>
                  <td style="background-color:yellow">
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ChassisType/Value)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Result: </td>
                  <td>
                    <xsl:if test="$ChassisType/Result = 'Passed'">
                      <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                    </xsl:if>
                    <xsl:if test="$ChassisType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ChassisType/Result"/>
                  </td>
                </tr>
                <tr>
                  <td>Detail: </td>
                  <td>
                    <xsl:value-of select="$ChassisType/Detail"/>
                  </td>
                </tr>
              </table>
            </li>     
          </xsl:if>
          <xsl:if test="$ProcessorModel != ''">
            <li>
              <p style="font-weight:bolder;text-decoration:underline;font-style:italic">ProcessorModel: </p>
              <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>Expected Value: </td>
                  <td>
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ProcessorModel/Expected)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Test Value: </td>
                  <td style="background-color:yellow">
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($ProcessorModel/Value)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Result: </td>
                  <td>
                    <xsl:if test="$ProcessorModel/Result = 'Passed'">
                      <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                    </xsl:if>
                    <xsl:if test="$ProcessorModel/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$ProcessorModel/Result"/>
                  </td>
                </tr>
                <tr>
                  <td>Detail: </td>
                  <td>
                    <xsl:value-of select="$ProcessorModel/Detail"/>
                  </td>
                </tr>
              </table>
            </li>
            </xsl:if>
          <xsl:if test="$TotalPhysicalRAM != ''">
              <li>
                <p style="font-weight:bolder;text-decoration:underline;font-style:italic">TotalPhysicalRAM: </p>
                <table xmlns="http://www.w3.org/1999/xhtml">
                  <tr>
                    <td>Min Value (Length): </td>
                    <td>
                      <xsl:value-of select="$TotalPhysicalRAM/Min"/>
                    </td>
                  </tr>
                  <tr>
                    <td>Max Value (Length): </td>
                    <td>
                      <xsl:value-of select="$TotalPhysicalRAM/Max"/>
                    </td>
                  </tr>
                  <tr>
                    <td>Test Value: </td>
                    <td style="background-color:yellow">
                      <xsl:value-of select="$TotalPhysicalRAM/Value"/>
                      (Length: <xsl:value-of select="string-length($TotalPhysicalRAM/Value)"/> characters)
                    </td>
                  </tr>
                  <tr>
                    <td>Result: </td>
                    <td>
                      <xsl:if test="$TotalPhysicalRAM/Result = 'Passed'">
                        <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                      </xsl:if>
                      <xsl:if test="$TotalPhysicalRAM/Result = 'Failed'">
                        <xsl:attribute name="style">background-color:red</xsl:attribute>
                      </xsl:if>
                      <xsl:value-of select="$TotalPhysicalRAM/Result"/>
                    </td>
                  </tr>
                  <tr>
                    <td>Detail: </td>
                    <td>
                      <xsl:if test="$mode = 'online' and $TotalPhysicalRAM/Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
                        <a target="_blank">
                          <xsl:attribute name="href">
                            <xsl:value-of select="$transactionId"/>
                            <xsl:value-of select="'_'"/>
                            <xsl:value-of select="$productKeyId"/>
                            <xsl:value-of select="'_Trace.xml'"/>
                          </xsl:attribute>
                          View OA3Tool Log Trace
                        </a>
                        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                        <a target="_blank">
                          <xsl:attribute name="href">
                            <xsl:value-of select="$transactionId"/>
                            <xsl:value-of select="'_'"/>
                            <xsl:value-of select="$productKeyId"/>
                            <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
                          </xsl:attribute>
                          View SMBIOS Dump
                        </a>
                        <br/>
                      </xsl:if> 
                      <xsl:value-of select="$TotalPhysicalRAM/Detail"/>
                    </td>
                  </tr>
                </table>
              </li>   
            </xsl:if>
          <xsl:if test="$PrimaryDiskType != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">PrimaryDiskType: </p>
              <table xmlns="http://www.w3.org/1999/xhtml">
                <tr>
                  <td>Expected Value: </td>
                  <td>
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($PrimaryDiskType/Expected)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Unexpected Value: </td>
                  <td>
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($PrimaryDiskType/Unexpected)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Test Value: </td>
                  <td style="background-color:yellow">
                    <xsl:value-of select="hhvxslx:GetHtmlSpacedString($PrimaryDiskType/Value)" disable-output-escaping="yes"/>
                  </td>
                </tr>
                <tr>
                  <td>Result: </td>
                  <td>
                    <xsl:if test="$PrimaryDiskType/Result = 'Passed'">
                      <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                    </xsl:if>
                    <xsl:if test="$PrimaryDiskType/Result = 'Failed'">
                      <xsl:attribute name="style">background-color:red</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="$PrimaryDiskType/Result"/>
                  </td>
                </tr>
                <tr>
                  <td>Detail: </td>
                  <td>
                    <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
                      <a target="_blank">
                        <xsl:attribute name="href">
                          <xsl:value-of select="$transactionId"/>
                          <xsl:value-of select="'_'"/>
                          <xsl:value-of select="$productKeyId"/>
                          <xsl:value-of select="'_Trace.xml'"/>
                        </xsl:attribute>
                        View OA3Tool Log Trace
                      </a>
                      <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                      <a target="_blank">
                        <xsl:attribute name="href">
                          <xsl:value-of select="$transactionId"/>
                          <xsl:value-of select="'_'"/>
                          <xsl:value-of select="$productKeyId"/>
                          <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
                        </xsl:attribute>
                        View SMBIOS Dump
                      </a>
                      <br/>
                    </xsl:if>
                    <xsl:value-of select="$PrimaryDiskType/Detail"/>
                  </td>
                </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$PrimaryDiskTotalCapacity != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">PrimaryDiskTotalCapacity: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value: </td>
                <td>
                  <xsl:value-of select="$PrimaryDiskTotalCapacity/Min"/>
                </td>
              </tr>
              <tr>
                <td>Max Value: </td>
                <td>
                  <xsl:value-of select="$PrimaryDiskTotalCapacity/Max"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="$PrimaryDiskTotalCapacity/Value"/>
                  (Length: <xsl:value-of select="string-length($PrimaryDiskTotalCapacity/Value)"/> characters)
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$PrimaryDiskTotalCapacity/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$PrimaryDiskTotalCapacity/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$PrimaryDiskTotalCapacity/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_Trace.xml'"/>
                      </xsl:attribute>
                      View OA3Tool Log Trace
                    </a>
                    <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
                      </xsl:attribute>
                      View SMBIOS Dump
                    </a>
                    <br/>
                  </xsl:if>
                  <xsl:value-of select="$PrimaryDiskTotalCapacity/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$DisplayResolutionHorizontal != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplayResolutionHorizontal: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionHorizontal/Min"/>
                </td>
              </tr>
              <tr>
                <td>Max Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionHorizontal/Max"/>
                </td>
              </tr>
              <tr>
                <td>Chassis Type: </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionHorizontal/ChassisType"/>
                  <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                  (<xsl:value-of select="hhvxslx:GetEnclosureType($DisplayResolutionHorizontal/ChassisType)"/>)
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="$DisplayResolutionHorizontal/Value"/>
                  (Length: <xsl:value-of select="string-length($DisplayResolutionHorizontal/Value)"/> characters)
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$DisplayResolutionHorizontal/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$DisplayResolutionHorizontal/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$DisplayResolutionHorizontal/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionHorizontal/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$DisplayResolutionVertical != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplayResolutionVertical: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionVertical/Min"/>
                </td>
              </tr>
              <tr>
                <td>Max Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionVertical/Max"/>
                </td>
              </tr>
              <tr>
                <td>Chassis Type: </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionVertical/ChassisType"/>
                  <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                  (<xsl:value-of select="hhvxslx:GetEnclosureType($DisplayResolutionVertical/ChassisType)"/>)
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="$DisplayResolutionVertical/Value"/>
                  (Length: <xsl:value-of select="string-length($DisplayResolutionVertical/Value)"/> characters)
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$DisplayResolutionVertical/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$DisplayResolutionVertical/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$DisplayResolutionVertical/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:value-of select="$DisplayResolutionVertical/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$DisplaySizePhysicalH != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplaySizePhysicalH: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalH/Min"/>
                </td>
              </tr>
              <tr>
                <td>Max Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalH/Max"/>
                </td>
              </tr>
              <tr>
                <td>Chassis Type: </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalH/ChassisType"/>
                  <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                  (<xsl:value-of select="hhvxslx:GetEnclosureType($DisplaySizePhysicalH/ChassisType)"/>)
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="$DisplaySizePhysicalH/Value"/>
                  (Length: <xsl:value-of select="string-length($DisplaySizePhysicalH/Value)"/> characters)
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$DisplaySizePhysicalH/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$DisplaySizePhysicalH/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$DisplaySizePhysicalH/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_Trace.xml'"/>
                      </xsl:attribute>
                      View OA3Tool Log Trace
                    </a>
                    <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_MonitorSize_Dump.txt'"/>
                      </xsl:attribute>
                      View Monitor Size Dump
                    </a>
                    <br/>
                  </xsl:if>
                  <xsl:value-of select="$DisplaySizePhysicalH/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$DisplaySizePhysicalY != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DisplaySizePhysicalY: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Min Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalY/Min"/>
                </td>
              </tr>
              <tr>
                <td>Max Value (Length): </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalY/Max"/>
                </td>
              </tr>
              <tr>
                <td>Chassis Type: </td>
                <td>
                  <xsl:value-of select="$DisplaySizePhysicalY/ChassisType"/>
                  <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                  (<xsl:value-of select="hhvxslx:GetEnclosureType($DisplaySizePhysicalY/ChassisType)"/>)
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="$DisplaySizePhysicalY/Value"/>
                  (Length: <xsl:value-of select="string-length($DisplaySizePhysicalY/Value)"/> characters)
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$DisplaySizePhysicalY/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$DisplaySizePhysicalY/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$DisplaySizePhysicalY/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_Trace.xml'"/>
                      </xsl:attribute>
                      View OA3Tool Log Trace
                    </a>
                    <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
                    <a target="_blank">
                      <xsl:attribute name="href">
                        <xsl:value-of select="$transactionId"/>
                        <xsl:value-of select="'_'"/>
                        <xsl:value-of select="$productKeyId"/>
                        <xsl:value-of select="'_MonitorSize_Dump.txt'"/>
                      </xsl:attribute>
                      View Monitor Size Dump
                    </a>
                    <br/>
                  </xsl:if>
                  <xsl:value-of select="$DisplaySizePhysicalY/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>
          <xsl:if test="$DigitizerSupportID != ''">
          <li>
            <p style="font-weight:bolder;text-decoration:underline;font-style:italic">DigitizerSupportID: </p>
            <table xmlns="http://www.w3.org/1999/xhtml">
              <tr>
                <td>Expected Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($DigitizerSupportID/Expected)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Unexpected Value: </td>
                <td>
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($DigitizerSupportID/Unexpected)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Test Value: </td>
                <td style="background-color:yellow">
                  <xsl:value-of select="hhvxslx:GetHtmlSpacedString($DigitizerSupportID/Value)" disable-output-escaping="yes"/>
                </td>
              </tr>
              <tr>
                <td>Result: </td>
                <td>
                  <xsl:if test="$DigitizerSupportID/Result = 'Passed'">
                    <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
                  </xsl:if>
                  <xsl:if test="$DigitizerSupportID/Result = 'Failed'">
                    <xsl:attribute name="style">background-color:red</xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="$DigitizerSupportID/Result"/>
                </td>
              </tr>
              <tr>
                <td>Detail: </td>
                <td>
                  <xsl:value-of select="$DigitizerSupportID/Detail"/>
                </td>
              </tr>
            </table>
          </li>
        </xsl:if>    
      </ul>
  </xsl:template>

  <xsl:template name="TemplateTableExpected">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="$itemNode/Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="TemplateTableExpectedUnexpected">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Unexpected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Unexpected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="$itemNode/Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="TemplateTableMinCount">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Min)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:for-each select="$itemNode/Detail/child::*[starts-with(name(), 'PhysicalMedium_')]">
            <span>
              <xsl:value-of select="name()"/>
              <xsl:value-of select="':'"/>
              <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
              <xsl:value-of select="text()"/>
              <br/>
            </span>
          </xsl:for-each>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="TemplateTableMinMax">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Min)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Max Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Max)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString($itemNode/Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:for-each select="$itemNode/Detail/child::*[starts-with(name(), 'PhysicalMedium_')]">
            <span>
              <xsl:value-of select="name()"/>
              <xsl:value-of select="':'"/>
              <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
              <xsl:value-of select="text()"/>
              <br/>
            </span>
          </xsl:for-each>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="TemplateTableMinMaxLength">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="$itemNode/Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="$itemNode/Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="$itemNode/Value"/>
          (Length: <xsl:value-of select="string-length($itemNode/Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="$itemNode/Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="TemplateTableMinMaxLengthChassis">
    <xsl:param name="itemNode"/>
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="$itemNode/Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="$itemNode/Max"/>
        </td>
      </tr>
      <tr>
        <td>Chassis Type: </td>
        <td>
          <xsl:value-of select="$itemNode/ChassisType"/>
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          (<xsl:value-of select="hhvxslx:GetEnclosureType($itemNode/ChassisType)"/>)
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="$itemNode/Value"/>
          (Length: <xsl:value-of select="string-length($itemNode/Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="$itemNode/Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="$itemNode/Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$itemNode/Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="$itemNode/Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/ProductKeyID">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/OSType">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          OS Build Number:
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          <xsl:value-of select="./Detail/OSBuild"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/NIC">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Min)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:for-each select="./Detail/child::*[starts-with(name(), 'PhysicalMedium_')]">
            <span>
              <xsl:value-of select="name()"/>
              <xsl:value-of select="':'"/>
              <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
              <xsl:value-of select="text()"/>
              <br/>
            </span>
          </xsl:for-each>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/OA3Tool">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <span>
            Tool Version:
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <xsl:value-of select="./Detail/ToolVersion"/>
            <br/>
            Tool Build:
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <xsl:value-of select="./Detail/ToolBuild"/>
          </span>      
        </td>
      </tr>
    </table>
  </xsl:template>
  
  <xsl:template match="TestItems/ChassisType">
     <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Unexpected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Unexpected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosSystemManufacturer">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosSystemFamily">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosSystemProductName">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosBoardProduct">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosSkuNumber">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosSystemSerialNumber">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/SmbiosUuid">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/DigitizerSupportID">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Unexpected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Unexpected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/ProcessorModel">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>
  
  <xsl:template match="TestItems/TotalPhysicalRAM">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_Trace.xml'"/>
              </xsl:attribute>
              View OA3Tool Log Trace
            </a>
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
              </xsl:attribute>
              View SMBIOS Dump
            </a>
            <br/>
          </xsl:if>
          
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/PrimaryDiskType">
     <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Expected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Expected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Unexpected Value: </td>
        <td>
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Unexpected)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="hhvxslx:GetHtmlSpacedString(./Value)" disable-output-escaping="yes"/>
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_Trace.xml'"/>
              </xsl:attribute>
              View OA3Tool Log Trace
            </a>
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
              </xsl:attribute>
              View SMBIOS Dump
            </a>
            <br/>
          </xsl:if>
          
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/PrimaryDiskTotalCapacity">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value: </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value: </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_Trace.xml'"/>
              </xsl:attribute>
              View OA3Tool Log Trace
            </a>
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_SMBIOS_Dump.txt'"/>
              </xsl:attribute>
              View SMBIOS Dump
            </a>
            <br/>
          </xsl:if>
          
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/DisplayResolutionHorizontal">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Chassis Type: </td>
        <td>
          <xsl:value-of select="./ChassisType"/>
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          (<xsl:value-of select="hhvxslx:GetEnclosureType(./ChassisType)"/>)
      </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/DisplayResolutionVertical">
   <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
     <tr>
       <td>Max Value (Length): </td>
       <td>
         <xsl:value-of select="./Max"/>
       </td>
     </tr>
      <tr>
        <td>Chassis Type: </td>
        <td>
          <xsl:value-of select="./ChassisType"/>
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          (<xsl:value-of select="hhvxslx:GetEnclosureType(./ChassisType)"/>)
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/DisplaySizePhysicalH">
     <table xmlns="http://www.w3.org/1999/xhtml">
       <tr>
         <td>Min Value (Length): </td>
         <td>
           <xsl:value-of select="./Min"/>
         </td>
       </tr>
       <tr>
         <td>Max Value (Length): </td>
         <td>
           <xsl:value-of select="./Max"/>
         </td>
       </tr>
      <tr>
        <td>Chassis Type: </td>
        <td>
          <xsl:value-of select="./ChassisType"/>
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          (<xsl:value-of select="hhvxslx:GetEnclosureType(./ChassisType)"/>)
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_Trace.xml'"/>
              </xsl:attribute>
              View OA3Tool Log Trace
            </a>
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_MonitorSize_Dump.txt'"/>
              </xsl:attribute>
              View Monitor Size Dump
            </a>
            <br/>
          </xsl:if>
          
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="TestItems/DisplaySizePhysicalY">
    <table xmlns="http://www.w3.org/1999/xhtml">
      <tr>
        <td>Min Value (Length): </td>
        <td>
          <xsl:value-of select="./Min"/>
        </td>
      </tr>
      <tr>
        <td>Max Value (Length): </td>
        <td>
          <xsl:value-of select="./Max"/>
        </td>
      </tr>
      <tr>
        <td>Chassis Type: </td>
        <td>
          <xsl:value-of select="./ChassisType"/>
          <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
          (<xsl:value-of select="hhvxslx:GetEnclosureType(./ChassisType)"/>)
        </td>
      </tr>
      <tr>
        <td>Test Value: </td>
        <td style="background-color:yellow">
          <xsl:value-of select="./Value"/>
          (Length: <xsl:value-of select="string-length(./Value)"/> characters)
        </td>
      </tr>
      <tr>
        <td>Result: </td>
        <td>
          <xsl:if test="./Result = 'Passed'">
            <xsl:attribute name="style">background-color:forestgreen</xsl:attribute>
          </xsl:if>
          <xsl:if test="./Result = 'Failed'">
            <xsl:attribute name="style">background-color:red</xsl:attribute>
          </xsl:if>
          <xsl:value-of select="./Result"/>
        </td>
      </tr>
      <tr>
        <td>Detail: </td>
        <td>
          <xsl:if test="$mode = 'online' and ./Result = 'Failed' and $transactionId != '' and $productKeyId != ''">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_Trace.xml'"/>
              </xsl:attribute>
              View OA3Tool Log Trace
            </a>
            <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$transactionId"/>
                <xsl:value-of select="'_'"/>
                <xsl:value-of select="$productKeyId"/>
                <xsl:value-of select="'_MonitorSize_Dump.txt'"/>
              </xsl:attribute>
              View Monitor Size Dump
            </a>
            <br/>
          </xsl:if>
          
          <xsl:value-of select="./Detail"/>
        </td>
      </tr>
    </table>
  </xsl:template>
  
</xsl:stylesheet>
