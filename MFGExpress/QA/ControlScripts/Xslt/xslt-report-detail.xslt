<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:hhvxslx="HHValidation.XsltExt">
    <xsl:output method="html" indent="yes" standalone="yes"/>
    <xsl:param name="transactionId"></xsl:param>
    <xsl:param name="productKeyId"></xsl:param>
    <xsl:param name="mode">online</xsl:param>
    <xsl:param name="anchor"></xsl:param>
    <xsl:param name="summaryXslt">xslt-report-summary.xslt</xsl:param>
  <xsl:template match="/">
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
         Result-
         <xsl:value-of select="$transactionId"/>
            <xsl:value-of select="'-'"/>
            <xsl:value-of select="$productKeyId"/>
            <xsl:value-of select="'-'"/>
            <xsl:value-of select="$anchor"/>
        </title>
      </head>
      <body>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
        <a target="_self">
          <!--<xsl:attribute name="href">
            <xsl:value-of select="'#close'"/>
          </xsl:attribute>-->
          <xsl:attribute name="href">
            <xsl:value-of select="'#'"/>
          </xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:value-of select="'window.external.DoTransform(&quot;&quot;, &quot;'"/>
            <xsl:value-of select="$summaryXslt"/>
            <xsl:value-of select="'&quot;);'"/>
          </xsl:attribute>
          Back
        </a>
        <hr/>
        <p style="font-weight:bolder;font-size:x-large;font-family:Arial">
          <xsl:value-of select="$anchor"/>
        </p>       
        <hr/>
         <xsl:choose>
          <xsl:when test="$anchor = 'ProductKeyID'">
            <xsl:apply-templates select="TestItems/ProductKeyID"></xsl:apply-templates>
          </xsl:when>
          <xsl:when test="$anchor = 'OSType'">
            <xsl:apply-templates select="TestItems/OSType"></xsl:apply-templates>
          </xsl:when>
           <xsl:when test="$anchor = 'SmbiosSystemManufacturer'">
             <xsl:apply-templates select="TestItems/SmbiosSystemManufacturer"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosSystemFamily'">
             <xsl:apply-templates select="TestItems/SmbiosSystemFamily"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosSystemProductName'">
             <xsl:apply-templates select="TestItems/SmbiosSystemProductName"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosBoardProduct'">
             <xsl:apply-templates select="TestItems/SmbiosBoardProduct"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosSkuNumber'">
             <xsl:apply-templates select="TestItems/SmbiosSkuNumber"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosSystemSerialNumber'">
             <xsl:apply-templates select="TestItems/SmbiosSystemSerialNumber"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'SmbiosUuid'">
             <xsl:apply-templates select="TestItems/SmbiosUuid"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DigitizerSupportID'">
             <xsl:apply-templates select="TestItems/DigitizerSupportID"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'ProcessorModel'">
             <xsl:apply-templates select="TestItems/ProcessorModel"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'ChassisType'">
             <xsl:apply-templates select="TestItems/ChassisType"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'TotalPhysicalRAM'">
             <xsl:apply-templates select="TestItems/TotalPhysicalRAM"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'PrimaryDiskType'">
             <xsl:apply-templates select="TestItems/PrimaryDiskType"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'PrimaryDiskTotalCapacity'">
             <xsl:apply-templates select="TestItems/PrimaryDiskTotalCapacity"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DisplayResolutionHorizontal'">
             <xsl:apply-templates select="TestItems/DisplayResolutionHorizontal"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DisplayResolutionHorizontal'">
             <xsl:apply-templates select="TestItems/DisplayResolutionHorizontal"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DisplayResolutionVertical'">
             <xsl:apply-templates select="TestItems/DisplayResolutionVertical"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DisplaySizePhysicalH'">
             <xsl:apply-templates select="TestItems/DisplaySizePhysicalH"></xsl:apply-templates>
           </xsl:when>
           <xsl:when test="$anchor = 'DisplaySizePhysicalY'">
             <xsl:apply-templates select="TestItems/DisplaySizePhysicalY"></xsl:apply-templates>
           </xsl:when>
        </xsl:choose>
      </body>
    </html>
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
