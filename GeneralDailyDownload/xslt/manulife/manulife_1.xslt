<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:customfuns="customfuns"
>
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:variable name="special" select="'&#34;'"/>
    <xsl:value-of select="concat($special,'Name',$special,',',$special,'Code',$special,',',$special,'Currency',$special,',',$special,'PriceDate',$special,',',$special,'BidPrice',$special,',',$special,'OfferPrice',$special,',',$special,'NAV',$special)"/>
    
    <xsl:for-each select="//table[@id='viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:datatable1']/descendant::tr[position()>1]">
      <xsl:text>&#xa;</xsl:text>
   
      <xsl:variable name="Name" select="concat($special,./td[@class = 'second'],$special)"/>
      <xsl:variable name="Code" select="concat($special,./td[@class = 'first'],$special)"/>
      <xsl:variable name="Currency" select="'USD'"/>
      <xsl:variable name="PriceDate" select="concat($special,./td[@class = 'third'],$special)"/>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,./td[@class = 'otherLPfifthLarge']/descendant::span[position()=2],$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV">
        <xsl:value-of select="concat($special,./td[@class = 'fourth']/descendant::span[position()=3],$special)"/>
      </xsl:variable>
      <xsl:value-of select="concat($Name,',',$Code,',',$special,$Currency,$special,',',$PriceDate,',',$special,'N/A',$special,',',$OfferPrice,',',$NAV)"/>
    </xsl:for-each>

    <xsl:for-each select="//table[@id='viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:othersBond']/descendant::tr[position()>1]">
      <xsl:text>&#xa;</xsl:text>

      <xsl:variable name="Name" select="concat($special,./td[@class = 'second'],$special)"/>
      <xsl:variable name="Code" select="concat($special,./td[@class = 'first'],$special)"/>
      <xsl:variable name="Currency" select="'USD'"/>
      <xsl:variable name="PriceDate" select="concat($special,./td[@class = 'third'],$special)"/>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,./td[@class = 'otherLPfifthLarge']/descendant::span[position()=2],$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV">
        <xsl:value-of select="concat($special,./td[@class = 'fourth']/descendant::span[position()=3],$special)"/>
      </xsl:variable>
      <xsl:value-of select="concat($Name,',',$Code,',',$special,$Currency,$special,',',$PriceDate,',',$special,'N/A',$special,',',$OfferPrice,',',$NAV)"/>

    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>

<!--
    <xsl:text>Name</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>Code</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>Currency</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>PriceDate</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>BidPrice</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>OfferPrice</xsl:text>
    <xsl:value-of select ="','"/>
    <xsl:text>NAV</xsl:text>
    <xsl:text>&#xa;</xsl:text>

    <xsl:for-each select="//table[@id='viewns_Z7_01A41A42IGEOE0A0S3EDN81020_:mainContent:datatable1']/tr[position()>3]">

      <xsl:value-of select="normalize-space(./td[@class = 'second'])"/>
      <xsl:text>,</xsl:text>
      <xsl:value-of select ="normalize-space(./td[@class = 'first'])"/>
      <xsl:text>,</xsl:text>
      <xsl:text>USD</xsl:text>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="normalize-space(./td[@class = 'third'])"/>
      <xsl:text>,</xsl:text>
      <xsl:text>N/A</xsl:text>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="normalize-space(./td[@class = 'otherLPfifthLarge'])"/>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="normalize-space(./td[@class = 'fourth'])"/>
      <xsl:text>&#xa;</xsl:text>


    </xsl:for-each>

-->