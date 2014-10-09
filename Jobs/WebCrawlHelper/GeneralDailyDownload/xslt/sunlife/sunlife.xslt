<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:customfuns="customfuns"
>
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:variable name="special" select="'&#34;'"/>

    <xsl:choose>
      <xsl:when test="count(//table[@class='data-sheet'][1]/descendant::tr[position()>1][1]/descendant::td) = 6">
        <xsl:value-of select="concat($special,'Valuation Date (MM/DD/YYYY)',$special,',',$special,'Investment-linked Fund Name',$special,',',$special,'Fund Code',$special,',',$special,'Currency',$special,',',$special,'Bid Price',$special,',',$special,'Offer Price',$special)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat($special,'Valuation Date (MM/DD/YYYY)',$special,',',$special,'Investment-linked Fund Name',$special,',',$special,'Fund Code',$special,',',$special,'Currency',$special,',',$special,'Offer/Bid Price',$special)"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:for-each select="//table[@class='data-sheet']/descendant::tr[position()>1]">
      <xsl:text>&#xa;</xsl:text>

      <xsl:variable name="Valuation-Date" select="concat($special,normalize-space(substring-after(//span[@id = 'lbl_val'],'Valuation Date (MM/DD/YYYY) ')),$special)"/>
      <xsl:variable name="Fund-Name" select="concat($special,normalize-space(./td[@class = 'first']),$special)"/>
      <xsl:variable name="Fund-Code" select="concat($special,normalize-space(./td[position()=2]),$special)"/>
      <xsl:variable name="Currency"  select="concat($special,substring-before(normalize-space(./td[position()=3]),' '),$special)"/>
      <xsl:choose>
        <xsl:when test="count(./descendant::td) = 6">
          <xsl:variable name="OfferPrice">
            <xsl:value-of select="concat($special,normalize-space(substring(./td[position()=4],4)),$special)"/>
          </xsl:variable>
          <xsl:variable name="BidPrice">
            <xsl:value-of select="concat($special,normalize-space(substring(./td[position()=3],4)),$special)"/>
          </xsl:variable>
          <xsl:value-of select="concat($Valuation-Date ,',',$Fund-Name,',',$Fund-Code,',',$Currency,',',$BidPrice,',',$OfferPrice)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:variable name="OfferPrice">
            <xsl:value-of select="concat($special,normalize-space(substring(./td[position()=3],4)),$special)"/>
          </xsl:variable>
          <xsl:value-of select="concat($Valuation-Date ,',',$Fund-Name,',',$Fund-Code,',',$Currency,',',$OfferPrice)"/>
        </xsl:otherwise>
      </xsl:choose>

    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>