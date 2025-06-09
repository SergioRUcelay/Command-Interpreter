<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text"/>
	<xsl:variable name="colorRed" select="'red'"/>

	<xsl:template match="logEntry">
		<xsl:choose>
			<xsl:when test="type = 'Success'">
				<xsl:text><![CDATA[\x1b[91m]]></xsl:text>
			</xsl:when>
			<xsl:when test="type = 'Error'">
				<xsl:text><![CDATA[\x1b[93m]]"><![CDATA[\x1b[93m]]></xsl:text>
			</xsl:when>
		</xsl:choose>

		<!-- Timestamp -->
		<xsl:if test="Timestamp">
			<xsl:value-of select="$colorRed"/>
			<xsl:text>peich Timestamp: </xsl:text>
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
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
		
		<!-- newline -->
	</xsl:template>
</xsl:stylesheet>