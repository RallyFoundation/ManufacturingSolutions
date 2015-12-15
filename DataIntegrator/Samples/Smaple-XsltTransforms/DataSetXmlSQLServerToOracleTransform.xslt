<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes" encoding="utf-8"/>

    <xsl:template match="@* | node()">
        <!--<xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>-->
      <NewDataSet>
        <xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:msprop="urn:schemas-microsoft-com:xml-msprop">
          <xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
            <xs:complexType>
              <xs:choice minOccurs="0" maxOccurs="unbounded">
                <xs:element name="Table" msprop:BaseTable.0="PRODUCTKEYS">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="PRODUCTKEYID" msprop:BaseColumn="PRODUCTKEYID" msprop:OraDbType="107" type="xs:decimal" minOccurs="0" />
                      <xs:element name="PRODUCTKEY" msprop:BaseColumn="PRODUCTKEY" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="PRODUCTKEYSTATEID" msprop:BaseColumn="PRODUCTKEYSTATEID" msprop:OraDbType="107" type="xs:decimal" minOccurs="0" />
                      <xs:element name="HARDWAREHASH" msprop:BaseColumn="HARDWAREHASH" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="OEMPARTNUMBER" msprop:BaseColumn="OEMPARTNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="ENDITEMPARTNUMBER" msprop:BaseColumn="ENDITEMPARTNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="ORDERID" msprop:BaseColumn="ORDERID" msprop:OraDbType="107" type="xs:decimal" minOccurs="0" />
                      <xs:element name="OEMADDITIONALINFO" msprop:BaseColumn="OEMADDITIONALINFO" msprop:OraDbType="105" type="xs:string" minOccurs="0" />
                      <xs:element name="BUSINESSNAME" msprop:BaseColumn="BUSINESSNAME" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="ORDERUNIQUEID" msprop:BaseColumn="ORDERUNIQUEID" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="SOLDTOCUSTOMERID" msprop:BaseColumn="SOLDTOCUSTOMERID" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="REFERENCENUMBER" msprop:BaseColumn="REFERENCENUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="OEMPONUMBER" msprop:BaseColumn="OEMPONUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="OEMLINEITEMNUMBER" msprop:BaseColumn="OEMLINEITEMNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="MSORDERNUMBER" msprop:BaseColumn="MSORDERNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="MSLINENUMBER" msprop:BaseColumn="MSLINENUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="CONTRACTNUMBER" msprop:BaseColumn="CONTRACTNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="ORDERTYPEID" msprop:BaseColumn="ORDERTYPEID" msprop:OraDbType="107" type="xs:decimal" minOccurs="0" />
                      <xs:element name="LICENSABLEPARTNUMBER" msprop:BaseColumn="LICENSABLEPARTNUMBER" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="QUANTITY" msprop:BaseColumn="QUANTITY" msprop:OraDbType="107" type="xs:decimal" minOccurs="0" />
                      <xs:element name="PRODUCTKEYSTATE" msprop:BaseColumn="PRODUCTKEYSTATE" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="SKUID" msprop:BaseColumn="SKUID" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="RETURNREASONCODE" msprop:BaseColumn="RETURNREASONCODE" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="ACTIONCODE" msprop:BaseColumn="ACTIONCODE" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="CREATEDBY" msprop:BaseColumn="CREATEDBY" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="CREATEDDATE" msprop:BaseColumn="CREATEDDATE" msprop:OraDbType="106" type="xs:dateTime" minOccurs="0" />
                      <xs:element name="MODIFIEDBY" msprop:BaseColumn="MODIFIEDBY" msprop:OraDbType="126" type="xs:string" minOccurs="0" />
                      <xs:element name="MODIFIEDDATE" msprop:BaseColumn="MODIFIEDDATE" msprop:OraDbType="106" type="xs:dateTime" minOccurs="0" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        <xsl:call-template name="TemplateTables"/>
      </NewDataSet>
    </xsl:template>

  <xsl:template name="TemplateTables">
    <xsl:for-each select="//Table">
      <Table>
        <PRODUCTKEYID>
          <xsl:value-of select="./ProductKeyID"/>
        </PRODUCTKEYID>
        <PRODUCTKEY>
          <xsl:value-of select="./ProductKey"/>
        </PRODUCTKEY>
        <PRODUCTKEYSTATEID>
          <xsl:value-of select="./ProductKeyStateID"/>
        </PRODUCTKEYSTATEID>
        <HARDWAREHASH>
          <xsl:value-of select="./HardwareID"/>
        </HARDWAREHASH>
        <OEMPARTNUMBER>
          <xsl:value-of select="./OEMPartNumber"/>
        </OEMPARTNUMBER>
        <PRODUCTKEYSTATE>
          <xsl:value-of select="./ProductKeyState"/>
        </PRODUCTKEYSTATE>
        <ACTIONCODE>
          <xsl:value-of select="./ActionCode"/>
        </ACTIONCODE>
        <CREATEDBY>
          <xsl:value-of select="./CreatedBy"/>
        </CREATEDBY>
        <CREATEDDATE>
          <xsl:value-of select="./CreatedDate"/>
        </CREATEDDATE>
        <MODIFIEDBY>
          <xsl:value-of select="./ModifiedBy"/>
        </MODIFIEDBY>
        <MODIFIEDDATE>
          <xsl:value-of select="./ModifiedDate"/>
        </MODIFIEDDATE>
      </Table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
