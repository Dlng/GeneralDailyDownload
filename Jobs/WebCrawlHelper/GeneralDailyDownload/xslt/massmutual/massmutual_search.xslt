<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="text"/>
  <xsl:template match="/">

          <xsl:variable name="special" select="'&#34;'"/>
    <xsl:value-of select="concat($special,'Name',$special,',',$special,'Code',$special,',',$special,'Currency',$special,',',$special,'PriceDate',$special,',',$special,'BidPrice',$special,',',$special,'OfferPrice',$special,',',$special,'NAV',$special)"/>

    <xsl:for-each select="//table[@id='reportTable']/descendant::tr[@class='fundSearchResultRow fundSearchResultRow_MMGIU']">
      <xsl:text>&#xa;</xsl:text>

      <xsl:variable name="Name" select="concat($special,./td[@class='fundSearchResultRow_name']/div[@class = 'fundhkfont padding'],$special)"/>
      <xsl:variable name="Code" select="concat($special,./td[position()=2]/div,$special)"/>
      <xsl:variable name="Currency">
         <xsl:value-of select="normalize-space(substring-before(substring-after(//table[@id='reportTable']/descendant::tr[@class = 'reportTitle whitebluelargetabletop']/descendant::span[@class = 'spanCurrency'],'('),')'))"/>
      </xsl:variable>
      <xsl:variable name="PriceDate" select="concat($special,./td[position()=8]/div,$special)"/>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,normalize-space(substring-after(./td[position()=10]/div,'/')),$special)"/>
      </xsl:variable>
      <xsl:variable name="BidPrice">
        <xsl:value-of select="concat($special,normalize-space(substring-before(./td[position()=10]/div,'/')),$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV" select="'N/A'"/>
      <xsl:value-of select="concat($Name,',',$Code,',',$special,$Currency,$special,',',$PriceDate,',',$BidPrice,',',$OfferPrice,',',$NAV)"/>
    </xsl:for-each>

   

  </xsl:template>

</xsl:stylesheet>