<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:customfuns="customfuns"
>
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:value-of select ="'Fund Code'"/>
    <xsl:value-of select ="','"/>
    <xsl:value-of select ="'Fund Name'"/>
    <xsl:value-of select ="','"/>
    <xsl:value-of select ="'As of'"/>
    <xsl:value-of select ="','"/>
    <xsl:value-of select ="'Sell (USD)'"/>
    <xsl:value-of select ="','"/>
    <xsl:value-of select ="'Buy (USD)'"/>
    <xsl:text>&#xa;</xsl:text>

    <xsl:for-each select="//table[@id='fundListTable']/tbody/tr[position()>1 and position() != last()]/td">

      <xsl:text>&#34;</xsl:text>
      <xsl:value-of select ="normalize-space(.)"/>
      <xsl:text>&#34;</xsl:text>


      <xsl:choose>
        <xsl:when test="@class='right buy'">
          <xsl:text>&#xa;</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="','"/>
        </xsl:otherwise>
      </xsl:choose>


    </xsl:for-each>

    <xsl:for-each select="//table[@class='lines'][2]/tbody/tr[position()>1]/td">

      <xsl:text>&#34;</xsl:text>
      <xsl:value-of select ="normalize-space(.)"/>
      <xsl:text>&#34;</xsl:text>



      <xsl:choose>
        <xsl:when test="@class='right buy'">
          <xsl:text>&#xa;</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="','"/>
        </xsl:otherwise>
      </xsl:choose>


    </xsl:for-each>

  </xsl:template>
</xsl:stylesheet>