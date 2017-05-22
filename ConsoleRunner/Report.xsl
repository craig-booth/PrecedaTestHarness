<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
    <style>
      .report-heading {
      font-family: verdana;
      font-size: 24px;
      text-align: center;
      padding: 20;
      }

      .summary {
      font-family: verdana;
      font-size: 18px;
      background: gray;
      color: white;
      width: 100%;
      align: center;
      padding: 10;
      }

      .group-heading {
      height: 20;
      width: 100%;
      font-family: verdana;
      font-size: 15px;
      background: gray;
      color: white;
      padding: 2;
      margin: 2;
      }

      .passed-test {
      font-family: verdana;
      font-size: 15px;
      color: black;
      }

      .failed-test {
      font-family: verdana;
      font-size: 15px;
      color: red;
      }
    </style>
  <body>
    <div class="report-heading">Test Report (<xsl:value-of select="testrun/summary/start"/>)</div>
	<xsl:apply-templates select="testrun"/>
  </body>
  </html>
</xsl:template>

<xsl:template match="testrun">
	<xsl:apply-templates select="summary"/>
	<br/>
	<xsl:apply-templates select="results"/>
</xsl:template>

<xsl:template match="summary">
    <table class="summary">
	  <tr>
      <td width="120">Passed</td>
      <td align="right"><xsl:value-of select="passed"/></td>
    </tr>
	  <tr>
      <td width="120">Failed</td>
      <td align="right"><xsl:value-of select="failed"/></td>
    </tr>
	  <tr>
      <td width="120">Not Run</td>
      <td align="right"><xsl:value-of select="notrun"/></td>
    </tr>
	  <tr>
      <td width="120">Total</td>
      <td align="right"><xsl:value-of select="total"/></td>
    </tr>
	</table>
</xsl:template>

<xsl:template match="results">
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="testgroup">
  <div class="group-heading"><xsl:value-of select="name"/> - <xsl:value-of select="description"/> (Passed: <xsl:value-of select="passed"/> Failed: <xsl:value-of select="failed"/> Total: <xsl:value-of select="total"/>)</div>
	<div style="margin-left: 10"><xsl:apply-templates select="results"/></div>
</xsl:template>

<xsl:template match="unittest">
  <div class="group-heading"><xsl:value-of select="name"/> - <xsl:value-of select="description"/> (Passed: <xsl:value-of select="passed"/> Failed: <xsl:value-of select="failed"/> Total: <xsl:value-of select="total"/>)</div>
	<div style="margin-left: 10"><xsl:apply-templates select="results"/></div>
</xsl:template>

<xsl:template match="testcase">
  <table width="100%">
    <tr>
      <xsl:choose>
        <xsl:when test="result = 'Passed'">
          <xsl:attribute name="class">passed-test</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="class">failed-test</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
		  <td width="200"><xsl:value-of select="name"/></td>
		  <td width="*"><xsl:value-of select="description"/></td>
		  <td width="100"><xsl:value-of select="result"/></td>
	  </tr>
  </table>
</xsl:template>

</xsl:stylesheet>