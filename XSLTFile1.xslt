<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text"/>

	<xsl:template match="logEntry">
		<xsl:choose>
			<xsl:when test="type = 'Success'">
				<xsl:text><![CDATA[\x1b[91m]]></xsl:text>
			</xsl:when>
			<xsl:when test="type = 'Error'">
				<xsl:text><![CDATA[\x1b[93m]]></xsl:text>
			</xsl:when>
		</xsl:choose>

		<xsl:value-of select="concat(timestamp, ': ', message)"/>
		<xsl:text><![CDATA[\x1b[0m]]></xsl:text>
		<xsl:text>&#10;</xsl:text>
		<!-- newline -->
	</xsl:template>
</xsl:stylesheet>