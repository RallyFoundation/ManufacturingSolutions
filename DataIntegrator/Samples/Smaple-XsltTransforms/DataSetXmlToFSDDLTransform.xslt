<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:di="DI.DefaultEx"
>

  <xsl:param name="OutputEncodingName" />
  
    <xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="no" />

  <!--<xsl:output method="xml" indent="yes" omit-xml-declaration="no" />-->

    <xsl:template match="@* | node()">
        <!--<xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>-->
      <Directory xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Name="ProductKeys" Type="ProductKey" Title="Product Keys">
        <Description>Product Keys</Description>
        <Directories>
          <Directory>
            <xsl:attribute name="Name">
              <xsl:value-of select="di:GetCurrentDateTimeStringByPattern('yyyy-MM-dd')"/>
            </xsl:attribute>
            <xsl:attribute name="Title">
              <xsl:value-of select="''"/>
            </xsl:attribute>
            <Description>
              <xsl:value-of select="''"/>
            </Description>
            <Files>
              <File>
                <xsl:attribute name="Name">
                  <xsl:value-of select="translate(translate(di:GetCurrentDateTimeStringByPattern('yyyy-MM-dd-hh-mm-ss-ffffzzz'), '+', 'A'), ':', '-')"/>
                  <xsl:value-of select="'.xml'"/>
                </xsl:attribute>
                <Content>
                  <xsl:value-of select="di:GetXmlNodeBytesValueBase64String(/, $OutputEncodingName)"/>
                </Content>
              </File>
            </Files>
          </Directory>
        </Directories>
      </Directory>
    </xsl:template>

  <!--<xsl:template name="TemplateGetFileContentBase64">
    --><!--<xsl:param name="FileContent"/>
    <xsl:copy-of select="di:GetStringBytesValueBase64String(/)"/>--><!--
    <xsl:variable name="nodes">
      <xsl:copy-of select="/"/>
    </xsl:variable>
   --><!--<xsl:copy-of select="di:GetXmlNodeBytesValueBase64String(msxsl:node-set($nodes))"/>--><!--
  --><!--<xsl:copy-of select="di:GetStringBytesValueBase64String(msxsl:node-set($nodes))"/>--><!--
    <xsl:value-of select="di:GetXmlNodeBytesValueBase64String(/)"/>
  </xsl:template>-->
</xsl:stylesheet>
