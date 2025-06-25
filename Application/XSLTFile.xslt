<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" encoding="UTF-8"/>
	
	<!-- Alternative template for root element if it's not LogEntry -->
	<xsl:template match="*[Type or Return or Timestamp or FunctionCalled or Message or ThrowError]">
		
		<!-- Type -->
		<xsl:choose>
			<xsl:when test="Type = 'Error'">
				<xsl:text>\x1b[31m</xsl:text>
				<!-- Red color -->
				<xsl:value-of select="Type"/>
				<xsl:text>\x1b[0m</xsl:text>
				<xsl:text>&#10;</xsl:text>
				<!-- Reset color -->
			</xsl:when>
			<xsl:when test="Type = 'Success'">
				<xsl:text>\x1b[92m</xsl:text>
				<!-- Yellow color -->
				<xsl:value-of select="Type"/>
				<xsl:text >\x1b[0m</xsl:text>
				<xsl:text>&#10;</xsl:text>
				<!-- Reset color -->
			</xsl:when>
			<xsl:when test="Type = 'Void'">
				<xsl:text>\x1b[93m</xsl:text>
				<!-- Yellow color -->
				<xsl:value-of select="Type"/>
				<xsl:text >\x1b[0m</xsl:text>
				<xsl:text>&#10;</xsl:text>
				<!-- Reset color -->
			</xsl:when>
		</xsl:choose>
		
		<!-- Return -->
		<xsl:if test="Return">
			<xsl:text>Return: </xsl:text>
			<xsl:value-of select="Return"/>
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
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>