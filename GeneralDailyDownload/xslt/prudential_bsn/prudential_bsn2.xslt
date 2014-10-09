<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:customfuns="customfuns"
                >
  <xsl:output method="text"/>
  <msxsl:script language="C#" implements-prefix="customfuns">
    <msxsl:using namespace="System"/>
    <msxsl:using namespace="System.IO"/>
    <msxsl:using namespace="System.Web"/>
    <msxsl:using namespace="System.Net"/>
    <msxsl:using namespace="System.Xml.XPath"/>
    <msxsl:using namespace="System.Xml"/>
    <msxsl:using namespace="System.Text"/>
    <msxsl:using namespace="System.Collections.Generic"/>
    <msxsl:using namespace="System.Xml.Xsl"/>
    <msxsl:using namespace="System.Globalization"/>
    <msxsl:assembly name="System"/>
    <![CDATA[ 
    public string replace(string text, string replace, string by)
    {
      return text.Replace(replace,by);
    }
    public string Trim(string text)
    {
      return text.Trim();
    }
    ]]>
  </msxsl:script>

  <xsl:template match="/">
    <xsl:variable name="special" select="'&#34;'"/>
    <xsl:value-of select="concat($special,'Name',$special,',',$special,'Code',$special,',',$special,'Currency',$special,',',$special,'PriceDate',$special,',',$special,'BidPrice',$special,',',$special,'OfferPrice',$special,',',$special,'NAV',$special)"/>

    <xsl:for-each select="//price">

      <xsl:text>&#xa;</xsl:text>

      <xsl:variable name="Name" select="concat($special,@desc,$special)"/>
      <xsl:variable name="Code" select="concat($special,'N/A',$special)"/>

      <xsl:variable name="PriceDate">
        <xsl:value-of select="concat($special,customfuns:replace(//fundprices2/@date,' ','/'),$special)"/>
      </xsl:variable>

      <xsl:variable name="Currency">
        <xsl:value-of select="concat($special,'RM',$special)"/>
      </xsl:variable>
      <xsl:variable name="BidPrice">
        <xsl:value-of select="concat($special,'N/A',$special)"/>
      </xsl:variable>
      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,'N/A',$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV" select="concat($special,substring-after(@price,'M'),$special)"/>

      <xsl:value-of select="concat($Name,',',$Code,',',$Currency,',',$PriceDate,',',$BidPrice,',',$OfferPrice,',',$NAV)"/>








    </xsl:for-each>
    <xsl:value-of select="//price"/>
  </xsl:template>

</xsl:stylesheet>