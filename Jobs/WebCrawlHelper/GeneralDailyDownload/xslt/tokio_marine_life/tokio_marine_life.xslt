<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:customfuns="customfuns"
                >
  <xsl:output method="text"/>


  <xsl:template match="/">
    <xsl:variable name="special" select="'&#34;'"/>
    <xsl:value-of select="concat($special,'Name',$special,',',$special,'Code',$special,',',$special,'Currency',$special,',',$special,'PriceDate',$special,',',$special,'BidPrice',$special,',',$special,'OfferPrice',$special,',',$special,'NAV',$special)"/>

    <xsl:for-each select="//table[@width='483']/tbody/descendant::tr[position()>2]">

      <xsl:text>&#xa;</xsl:text>


      <xsl:variable name="Name" select="concat($special,normalize-space(./td[position()=1]),$special)"/>
      <xsl:variable name="Code" select="concat($special,'N/A',$special)"/>

      <xsl:variable name="PriceDate">
        <xsl:value-of select="concat($special,normalize-space(substring-after(//table[@width='483']/tbody/descendant::tr[position()=1]/descendant::strong,'t')),$special)"/>
      </xsl:variable>

      <xsl:variable name="Currency">
        <xsl:value-of select="concat($special,substring-before(substring-after(//table[@width='483']/tbody/descendant::tr[position()=2]/td[position() = 2],'('),')'),$special)"/>
      </xsl:variable>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,normalize-space(substring-after(./td[position()=3],'$')),$special)"/>
      </xsl:variable>
      <xsl:variable name="BidPrice">
        <xsl:value-of select="concat($special,normalize-space(substring-after(./td[position()=2],'$')),$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV" select="concat($special,./td[position()=4],$special)"/>


      <xsl:value-of select="concat($Name,',',$Code,',',$Currency,',',$PriceDate,',',$BidPrice,',',$OfferPrice,',',$NAV)"/>

    </xsl:for-each>

  </xsl:template>

</xsl:stylesheet>