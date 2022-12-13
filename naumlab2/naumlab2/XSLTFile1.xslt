<?xml version="1.0" encoding="utf-8"?>
<html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <body style="font-family:Arial;font-size:12pt;background-color:#EEEEEE">
    <xsl:for-each select="root/resourse">
      <div style="background-color:tomato;color:white;padding:4px">
        <span style="font-weight:bold">
          <xsl:value-of select= "@name"/>
        </span>
      </div>
      <div>
        <p>
          <xsl:value-of select="@info"/>
        </p>
        <p>
          <xsl:value-of select="@annotation"/>
        </p>
        <p>
          <xsl:value-of select="@type"/>
        </p>
        <p>
          <xsl:value-of select="@address"/>
        </p>
        <p>
          <xsl:value-of select="@conditions"/>
        </p>
      </div>
      <div style="margin-left:20px;margin-bottom:1em;font-size:10pt">
        <p>
          <xsl:value-of select="author/@name"/>
        </p>
        <p>
          <xsl:value-of select="author/@faculty"/>
        </p>
        <p>
          <xsl:value-of select="author/@cathedra"/>
        </p>
      </div>
    </xsl:for-each>
  </body>
</html>
