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

    <xsl:for-each select="//table[@class='tblPrice']/tbody/descendant::tr">
      <xsl:choose>
        <xsl:when test="boolean(number(./td[position()=last()])) = 'true'">
          <xsl:text>&#xa;</xsl:text>
      </xsl:when>
        <xsl:otherwise></xsl:otherwise>
      </xsl:choose>
      
      <!--
-->

      <xsl:variable name="Name" select="concat($special,normalize-space(./td[position()=1]),$special)"/>
      <xsl:variable name="Code" select="concat($special,'N/A',$special)"/>

      <xsl:variable name="PriceDate">
        <xsl:choose>
          <xsl:when test="./td[position()=2] = ' 								 '">
            <xsl:value-of select="concat($special,customfuns:replace(substring-before(substring(normalize-space(./td[position()=1]),7),')'),'-','/'),$special)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($special,customfuns:replace(substring-before(substring(normalize-space(//table[@class='tblPrice']/tbody/descendant::tr[position()=8]/td[position()=1]),7),')'),'-','/'),$special)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="tempCurrency">
      </xsl:variable> 
      
      <xsl:variable name="Currency">
        <xsl:choose>
          <xsl:when test="position()>1 and position() &lt;= 8">
            <xsl:value-of select="concat($special,substring-before(substring-after(normalize-space(//table[@class='tblPrice']/tbody/descendant::tr[position() = 1]/td[position() = 2]),'('),')'),$special)"/>
          </xsl:when>
          <xsl:when test="position() >=9 and position() &lt;= 13">
            <xsl:value-of select="concat($special,substring-before(substring-after(normalize-space(//table[@class='tblPrice']/tbody/descendant::tr[position() = 9]/td[position() = 2]),'('),')'),$special)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($special,substring-before(substring-after(normalize-space(//table[@class='tblPrice']/tbody/descendant::tr[position() = 14]/td[position() = 2]),'('),')'),$special)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="OfferPrice">
        <xsl:value-of select="concat($special,normalize-space(./td[position()=3]),$special)"/>
      </xsl:variable>
      <xsl:variable name="BidPrice">
        <xsl:value-of select="concat($special,normalize-space(./td[position()=2]),$special)"/>
      </xsl:variable>
      <xsl:variable name="NAV" select="concat($special,normalize-space(./td[position()=4]),$special)"/>

      <xsl:choose>
        <xsl:when test="boolean(number(./td[position()=last()])) = 'true'">

          <xsl:value-of select="customfuns:replace(customfuns:replace(concat($Name,',',$Code,',',$Currency,',',$PriceDate,',',$BidPrice,',',$OfferPrice,',',$NAV),'&amp;nbsp;',' '),'amp;','' )"/>
          
          <!--
           <xsl:value-of select="concat($Name,',',$Code,',',$Currency,',',$PriceDate,',',$BidPrice,',',$OfferPrice,',',$NAV)"/>
          <xsl:value-of select="boolean(number(./td[position()=last()]))"/>
         -->
        </xsl:when>
        <xsl:otherwise>
        </xsl:otherwise>
     </xsl:choose>



      <!--
      <xsl:variable name="temp">
        <xsl:value-of select="number(./td[position()=last()])"/>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="$temp != 'NaN'">
          <xsl:value-of select="$temp"/>
        </xsl:when>
      </xsl:choose>
      -->
      

    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>