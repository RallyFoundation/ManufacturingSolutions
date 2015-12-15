<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  
  <xsl:output method="xml" indent="yes" />

  <xsl:param name="mode" select="0"/>

  <xsl:template match="/">
    <xsl:if test="$mode=0">
      <ArrayOfExportKeyInfo>
        <xsl:call-template name="TemplateExportKeyInfoes"/>
      </ArrayOfExportKeyInfo>
    </xsl:if>
    
    <xsl:if test="$mode=1">
      <ExportKeyList>
        <UserName>
          <xsl:value-of select="ExportKeyList/UserName"/>
        </UserName>
        <AccessKey>
          <xsl:value-of select="ExportKeyList/AccessKey"/>
        </AccessKey>
        <CreatedDate>
          <xsl:value-of select="ExportKeyList/CreatedDate"/>
        </CreatedDate>
        <Keys>
          <xsl:call-template name="TemplateExportKeyInfoes"/>
        </Keys>
      </ExportKeyList>
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="TemplateExportKeyInfoes">
    <xsl:for-each select="//ExportKeyInfo">
      <ExportKeyInfo>
        <xsl:copy-of select="./*[name() != 'SerialNumber']"/>
      </ExportKeyInfo>
    </xsl:for-each>
  </xsl:template>
  
</xsl:stylesheet>
