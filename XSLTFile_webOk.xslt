<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" encoding="UTF-8"/>

	<!-- Root template -->
	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>

	<!-- Template for each log entry -->
	<xsl:template match="LogEntry">
		<!-- Type -->
		<xsl:if test="Type">
			<xsl:text>Type: </xsl:text>
			<xsl:value-of select="Type"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Timestamp -->
		<xsl:if test="Timestamp">
			<xsl:text>Timestamp: </xsl:text>
			<xsl:value-of select="Timestamp"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Function Called -->
		<xsl:if test="FunctionCalled">
			<xsl:text>Function: </xsl:text>
			<xsl:value-of select="FunctionCalled"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Message -->
		<xsl:if test="Message">
			<xsl:text>Message: </xsl:text>
			<xsl:value-of select="Message"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- ThrowError -->
		<xsl:if test="ThrowError">
			<xsl:text>ThrowError: </xsl:text>
			<xsl:value-of select="ThrowError"/>
		</xsl:if>

		<!-- New line after each entry -->
		<xsl:text>&#10;</xsl:text>
	</xsl:template>

	<!-- Alternative template for root element if it's not LogEntry -->
	<xsl:template match="*[Type or Timestamp or FunctionCalled or Message or ThrowError]">
		<!-- Type -->
		<xsl:if test="Type">
			<xsl:text>Type: </xsl:text>
			<xsl:value-of select="Type"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Timestamp -->
		<xsl:if test="Timestamp">
			<xsl:text>Timestamp: </xsl:text>
			<xsl:value-of select="Timestamp"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Function Called -->
		<xsl:if test="FunctionCalled">
			<xsl:text>Function: </xsl:text>
			<xsl:value-of select="FunctionCalled"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Message -->
		<xsl:if test="Message">
			<xsl:text>Message: </xsl:text>
			<xsl:value-of select="Message"/>
			<xsl:text> | </xsl:text>
		</xsl:if>

		<!-- ThrowError -->
		<xsl:if test="ThrowError">
			<xsl:text>ThrowError: </xsl:text>
			<xsl:value-of select="ThrowError"/>
		</xsl:if>

		<!-- New line after each entry -->
		<xsl:text>&#10;</xsl:text>
	</xsl:template>

</xsl:stylesheet>