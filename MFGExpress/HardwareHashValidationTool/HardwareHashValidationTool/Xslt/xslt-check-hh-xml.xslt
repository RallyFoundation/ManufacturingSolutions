<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:hhvxslx="HHValidation.XsltExt">
    <xsl:output method="xml" indent="yes" encoding="utf-8" standalone="yes"/>
    <xsl:param name="transactionId"></xsl:param>
    <xsl:param name="productKeyId"></xsl:param>
    <xsl:param name="osType"></xsl:param>
    <xsl:param name="processorModel"></xsl:param>

  <xsl:template match="/">
      <TestItems>
        <SmbiosSystemManufacturer>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosSystemManufacturer']">
            <!--<xsl:with-param name="max" select="32"/>-->
          </xsl:apply-templates>
        </SmbiosSystemManufacturer>
        <SmbiosSystemFamily>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosSystemFamily']">
            <!--<xsl:with-param name="max" select="64"/>-->
          </xsl:apply-templates>
        </SmbiosSystemFamily>
        <SmbiosSystemProductName>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosSystemProductName']">
            <!--<xsl:with-param name="max" select="64"/>-->
          </xsl:apply-templates>
        </SmbiosSystemProductName>
        <SmbiosBoardProduct>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosBoardProduct']">
            <!--<xsl:with-param name="max" select="32"/>-->
          </xsl:apply-templates>
        </SmbiosBoardProduct>
        <SmbiosSkuNumber>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosSkuNumber']">
            <!--<xsl:with-param name="max" select="32"/>-->
          </xsl:apply-templates>
        </SmbiosSkuNumber>
        <SmbiosSystemSerialNumber>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosSystemSerialNumber']">
            <!--<xsl:with-param name="max" select="128"/>-->
          </xsl:apply-templates>
        </SmbiosSystemSerialNumber>
        <SmbiosUuid>
          <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='SmbiosUuid']">
            <!--<xsl:with-param name="max" select="128"/>-->
          </xsl:apply-templates>
        </SmbiosUuid>
          <ProductKeyID>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='ProductKeyID']">
              <xsl:with-param name="productKeyId" select="$productKeyId"/>
            </xsl:apply-templates>
          </ProductKeyID>
          <OSType>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='OSType']">
              <xsl:with-param name="osType" select="$osType"/>
            </xsl:apply-templates>
          </OSType>
         <ChassisType>
           <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='ChassisType']">
            </xsl:apply-templates>
         </ChassisType>
          <DigitizerSupportID>
             <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='DigitizerSupportID']">
              </xsl:apply-templates>
          </DigitizerSupportID>
          <ProcessorModel>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='ProcessorModel']">
              <xsl:with-param name="processorModel" select="$processorModel"/>
            </xsl:apply-templates>
          </ProcessorModel>
          <TotalPhysicalRAM>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='TotalPhysicalRAM']">
              <!--<xsl:with-param name="min">0</xsl:with-param>
              <xsl:with-param name="max">4</xsl:with-param>-->
            </xsl:apply-templates>
          </TotalPhysicalRAM>
          <PrimaryDiskType>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='PrimaryDiskType']">
            </xsl:apply-templates>
          </PrimaryDiskType>
          <PrimaryDiskTotalCapacity>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='PrimaryDiskTotalCapacity']">
              <!--<xsl:with-param name="min">1</xsl:with-param>
              <xsl:with-param name="max">65536</xsl:with-param>-->
            </xsl:apply-templates>
          </PrimaryDiskTotalCapacity>
          <DisplayResolutionHorizontal>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='DisplayResolutionHorizontal']">
              <xsl:with-param name="currentChassisType">
                <xsl:value-of select="HardwareReport/HardwareInventory/p[@n='ChassisType']/@v"/>
              </xsl:with-param>
              <xsl:with-param name="specialChassisType">0x03</xsl:with-param>
            </xsl:apply-templates>
          </DisplayResolutionHorizontal>
          <DisplayResolutionVertical>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='DisplayResolutionVertical']">
              <xsl:with-param name="currentChassisType">
                <xsl:value-of select="HardwareReport/HardwareInventory/p[@n='ChassisType']/@v"/>
              </xsl:with-param>
              <xsl:with-param name="specialChassisType">0x03</xsl:with-param>
            </xsl:apply-templates>
          </DisplayResolutionVertical>
          <DisplaySizePhysicalH>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='DisplaySizePhysicalH']">
              <xsl:with-param name="currentChassisType">
                <xsl:value-of select="HardwareReport/HardwareInventory/p[@n='ChassisType']/@v"/>
              </xsl:with-param>
              <xsl:with-param name="specialChassisType">0x03</xsl:with-param>
            </xsl:apply-templates>
          </DisplaySizePhysicalH>
          <DisplaySizePhysicalY>
            <xsl:apply-templates select="HardwareReport/HardwareInventory/p[@n='DisplaySizePhysicalY ']">
              <xsl:with-param name="currentChassisType">
                <xsl:value-of select="HardwareReport/HardwareInventory/p[@n='ChassisType']/@v"/>
              </xsl:with-param>
              <xsl:with-param name="specialChassisType">0x03</xsl:with-param>
            </xsl:apply-templates>
          </DisplaySizePhysicalY>    
        </TestItems>
    </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='ProductKeyID']">
    <xsl:param name="productKeyId"></xsl:param>
    <Expected>
      <xsl:value-of select="$productKeyId"/>
    </Expected>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="./@v = $productKeyId">Passed</xsl:if>
      <xsl:if test="./@v != $productKeyId">Failed</xsl:if>
    </Result>
    <Detail> 
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='OSType']">
    <xsl:param name="osType"></xsl:param>
    <Expected>
      <xsl:value-of select="$osType"/>
    </Expected>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="./@v = $osType">Passed</xsl:if>
      <xsl:if test="./@v != $osType">Failed</xsl:if>
    </Result>
    <Detail>
      <OSBuild>
        <xsl:value-of select="/HardwareReport/HardwareInventory/p[@n='OsBuild']/@v"/>     
      </OSBuild>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='ProcessorModel']">
    <xsl:param name="processorModel"></xsl:param>
    <Expected>
      <xsl:value-of select="$processorModel"/>
    </Expected>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="./@v = $processorModel">Passed</xsl:if>
      <xsl:if test="./@v != $processorModel">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosSystemManufacturer']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">32</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosSystemFamily']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">64</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosSystemProductName']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">64</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosBoardProduct']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">32</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosSkuNumber']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">32</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosSystemSerialNumber']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">128</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>
  
  <xsl:template match="HardwareReport/HardwareInventory/p[@n='SmbiosUuid']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">128</xsl:param>
    <Min>
      <xsl:value-of select="$min"/>
    </Min>
    <Max>
      <xsl:value-of select="$max"/>
    </Max>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>
  
  <xsl:template match="HardwareReport/HardwareInventory/p[@n='TotalPhysicalRAM']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">4</xsl:param>
    <Min><xsl:value-of select="$min"/></Min> 
    <Max><xsl:value-of select="$max"/></Max>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <xsl:if test="string-length(./@v) &gt; $min and string-length(./@v) &lt;= $max">Passed</xsl:if>
      <xsl:if test="string-length(./@v) &lt;= $min or string-length(./@v) &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='PrimaryDiskType']">
    <xsl:param name="unexpected">Not Applicable,Not Found,Unspecified,ATA,Unknown,SATA,Error,No Data,</xsl:param>
    <xsl:param name="expected">Virtual,NVMe,RAID,Other,SD,SSD,HDD</xsl:param>
    <Unexpected><xsl:value-of select="$unexpected"/></Unexpected>
    <Expected><xsl:value-of select="$expected"/></Expected>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $unexpected, ',') = false() and hhvxslx:IsValueInStringArray(./@v, $expected, ',') = true()">Passed</xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $unexpected, ',') = true() or hhvxslx:IsValueInStringArray(./@v, $expected, ',') = false()">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='PrimaryDiskTotalCapacity']">
    <xsl:param name="min">1</xsl:param>
    <xsl:param name="max">65536</xsl:param>
    <Min><xsl:value-of select="$min"/></Min>
    <Max><xsl:value-of select="$max"/></Max>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <xsl:if test="./@v &gt;= $min and ./@v &lt;= $max">Passed</xsl:if>
      <xsl:if test="./@v &lt; $min or ./@v &gt; $max">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='DisplayResolutionHorizontal']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">4</xsl:param>
    <xsl:param name="currentChassisType"/>
    <xsl:param name="specialChassisType">0x03</xsl:param>
    <xsl:param name="commonChassisTypes">0x0A,0x0D,0x1E,0x1F,0x20</xsl:param>
    <Min><xsl:value-of select="$min"/></Min>
    <Max><xsl:value-of select="$max"/></Max>
    <ChassisType><xsl:value-of select="$currentChassisType"/></ChassisType>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <!--<xsl:if test="(./@v &gt;= $min) or (./@v &lt;= $min and $currentChassisType = $specialChassisType)">Passed</xsl:if>
      <xsl:if test="(./@v &lt; $min) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>-->   
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = true()">
        <xsl:if test="string-length(./@v) &gt; $min and string-length(./@v) &lt;= $max">Passed</xsl:if>
        <xsl:if test="string-length(./@v) &lt;= $min or string-length(./@v) &gt; $max">Failed</xsl:if>
      </xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = false()">
        <xsl:if test="(string-length(./@v) &gt;= $min  and string-length(./@v) &lt;= $max) or ((string-length(./@v) &lt; $min or string-length(./@v) &gt; $max)  and $currentChassisType = $specialChassisType)">Passed</xsl:if>
        <xsl:if test="(string-length(./@v) &lt; $min or string-length(./@v) &gt; $max) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>
      </xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='DisplayResolutionVertical']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">4</xsl:param>
    <xsl:param name="currentChassisType"/>
    <xsl:param name="specialChassisType">0x03</xsl:param>
    <xsl:param name="commonChassisTypes">0x0A,0x0D,0x1E,0x1F,0x20</xsl:param>
    <Min><xsl:value-of select="$min"/></Min>
    <Max><xsl:value-of select="$max"/></Max>
    <ChassisType><xsl:value-of select="$currentChassisType"/></ChassisType>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <!--<xsl:if test="(./@v &gt;= $min) or (./@v &lt;= $min and $currentChassisType = $specialChassisType)">Passed</xsl:if>
      <xsl:if test="(./@v &lt; $min) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>-->
     <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = true()">
        <xsl:if test="string-length(./@v) &gt; $min and string-length(./@v) &lt;= $max">Passed</xsl:if>
        <xsl:if test="string-length(./@v) &lt;= $min or string-length(./@v) &gt; $max">Failed</xsl:if>
      </xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = false()">
        <xsl:if test="(string-length(./@v) &gt;= $min  and string-length(./@v) &lt;= $max) or ((string-length(./@v) &lt; $min or string-length(./@v) &gt; $max)  and $currentChassisType = $specialChassisType)">Passed</xsl:if>
        <xsl:if test="(string-length(./@v) &lt; $min or string-length(./@v) &gt; $max) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>
      </xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='DisplaySizePhysicalH']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">4</xsl:param>
    <xsl:param name="currentChassisType"/>
    <xsl:param name="specialChassisType">0x03</xsl:param>
    <xsl:param name="commonChassisTypes">0x0A,0x0D,0x1E,0x1F,0x20</xsl:param>
    <Min><xsl:value-of select="$min"/></Min>
    <Max><xsl:value-of select="$max"/></Max>
    <ChassisType><xsl:value-of select="$currentChassisType"/></ChassisType>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <!--<xsl:if test="(./@v &gt;= $min) or (./@v &lt;= $min and $currentChassisType = $specialChassisType)">Passed</xsl:if>
      <xsl:if test="(./@v &lt; $min) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>-->
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = true()">
        <xsl:if test="string-length(./@v) &gt; $min and string-length(./@v) &lt;= $max">Passed</xsl:if>
        <xsl:if test="string-length(./@v) &lt;= $min or string-length(./@v) &gt; $max">Failed</xsl:if>
      </xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = false()">
        <xsl:if test="(string-length(./@v) &gt;= $min  and string-length(./@v) &lt;= $max) or ((string-length(./@v) &lt; $min or string-length(./@v) &gt; $max)  and $currentChassisType = $specialChassisType)">Passed</xsl:if>
        <xsl:if test="(string-length(./@v) &lt; $min or string-length(./@v) &gt; $max) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>
      </xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>

  <xsl:template match="HardwareReport/HardwareInventory/p[@n='DisplaySizePhysicalY ']">
    <xsl:param name="min">0</xsl:param>
    <xsl:param name="max">4</xsl:param>
    <xsl:param name="currentChassisType"/>
    <xsl:param name="specialChassisType">0x03</xsl:param>
    <xsl:param name="commonChassisTypes">0x0A,0x0D,0x1E,0x1F,0x20</xsl:param>
    <Min><xsl:value-of select="$min"/></Min>
    <Max><xsl:value-of select="$max"/></Max>
    <ChassisType><xsl:value-of select="$currentChassisType"/></ChassisType>
    <Value><xsl:value-of select="./@v"/></Value>
    <Result>
      <!--<xsl:if test="(./@v &gt;= $min) or (./@v &lt;= $min and $currentChassisType = $specialChassisType)">Passed</xsl:if>
      <xsl:if test="(./@v &lt; $min) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>-->
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = true()">
        <xsl:if test="string-length(./@v) &gt; $min and string-length(./@v) &lt;= $max">Passed</xsl:if>
        <xsl:if test="string-length(./@v) &lt;= $min or string-length(./@v) &gt; $max">Failed</xsl:if>
      </xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray($currentChassisType, $commonChassisTypes, ',') = false()">
        <xsl:if test="(string-length(./@v) &gt;= $min  and string-length(./@v) &lt;= $max) or ((string-length(./@v) &lt; $min or string-length(./@v) &gt; $max)  and $currentChassisType = $specialChassisType)">Passed</xsl:if>
        <xsl:if test="(string-length(./@v) &lt; $min or string-length(./@v) &gt; $max) and ($currentChassisType != $specialChassisType)">Failed</xsl:if>
      </xsl:if>
    </Result>
    <Detail>
    </Detail>
  </xsl:template>
  
 <xsl:template match="HardwareReport/HardwareInventory/p[@n='ChassisType']">
    <xsl:param name="chassisTypeValueScope">0x03,0x0A,0x0D,0x1E,0x1F,0x20,0x24,0x23,0x21,0x22</xsl:param>
    <Expected>
      <xsl:value-of select="$chassisTypeValueScope"/>
    </Expected>
    <Value>
      <xsl:value-of select="./@v"/>
    </Value>
    <Result>
      <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $chassisTypeValueScope, ',') = true()">Passed</xsl:if>
      <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $chassisTypeValueScope, ',') = false()">Failed</xsl:if>
    </Result>
    <Detail>
    </Detail>
 </xsl:template>
  
 <xsl:template match="HardwareReport/HardwareInventory/p[@n='DigitizerSupportID']">
    <xsl:param name="unexpected">Not Applicable,Not Found,Error,No Data,</xsl:param>
    <xsl:param name="expected">IntegratedTouch, IntegratedPen, MultiInput|IntegratedTouch, IntegratedPen, ExternalPen,|ExternalPen, Ready|IntegratedTouch, Ready|IntegratedTouch, IntegratedPen, MultiInput, R|Pen|IntegratedTouch, MultiInput, Ready|Touch_Pen|Touch, Pen|Touch|None</xsl:param>
      <Unexpected><xsl:value-of select="$unexpected"/></Unexpected>
      <Expected><xsl:value-of select="$expected"/></Expected>
      <Value><xsl:value-of select="./@v"/></Value>
      <Result>
        <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $unexpected, ',') = false() and hhvxslx:IsValueInStringArray(./@v, $expected, '|') = true()">Passed</xsl:if>
        <xsl:if test="hhvxslx:IsValueInStringArray(./@v, $unexpected, ',') = true() or hhvxslx:IsValueInStringArray(./@v, $expected, '|') = false()">Failed</xsl:if>
      </Result>
       <Detail>
       </Detail>
  </xsl:template>
  
</xsl:stylesheet>
