<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:customfuns="customfuns"
>
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:variable name="special" select="'&#34;'"/>
    <xsl:value-of select="concat($special,'Name',$special,',',$special,'Code',$special,',',$special,'Currency',$special,',',$special,'PriceDate',$special,',',$special,'BidPrice',$special,',',$special,'OfferPrice',$special,',',$special,'NAV',$special)"/>

    <xsl:for-each select="//table[@id='viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:othersInternal']/descendant::tr[position()>1]">
      <xsl:text>&#xa;</xsl:text>

      <xsl:variable name="Name" select="concat($special,./td[@class = 'otherLPsecond'],$special)"/>
      <xsl:variable name="Code" select="concat($special,./td[@class = 'otherLPfirst'],$special)"/>
      <xsl:variable name="Currency" select="'USD'"/>
      <xsl:variable name="PriceDate" select="concat($special,./td[@class = 'third'],$special)"/>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="'N/A'"/>
      </xsl:variable>
      <xsl:variable name="NAV">
        <xsl:value-of select="concat($special,./td[@class = 'otherLPfourth']/descendant::span[position()=3],$special)"/>
      </xsl:variable>

      <xsl:value-of select="concat($Name,',',$Code,',',$special,$Currency,$special,',',$PriceDate,',',$special,'N/A',$special,',',$OfferPrice,',',$NAV)"/>
    </xsl:for-each>

  </xsl:template>
</xsl:stylesheet>