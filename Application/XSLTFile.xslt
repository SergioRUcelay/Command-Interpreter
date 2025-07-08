<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" encoding="UTF-8"/>
	
	<!-- Alternative template for root element if it's not LogEntry -->
	<xsl:template match="*[Type or Return or Function or Description or Timestamp or FunctionCalled or Message or ThrowError]">
		
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
				<!-- Blue color -->
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
		
		<!-- Function and Description-->
		<xsl:if test="Function">
			<xsl:text>\x1b[92m</xsl:text>
			<xsl:value-of select="Function"/>
			<xsl:text>: </xsl:text>
			<xsl:value-of select="Description"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
		
		<!-- Return -->
		<xsl:if test="Return">
			<xsl:text>Return: </xsl:text>
			<xsl:value-of select="Return"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Timestamp -->
		<xsl:if test="Timestamp">
			<xsl:text>Timestamp: </xsl:text>
			<xsl:value-of select="concat(substring(Timestamp, 6, 2), '/', substring(Timestamp, 9, 2), '/', substring(Timestamp, 1, 4), ' ',
					substring(Timestamp, 12, 2), 'h:', substring(Timestamp, 15, 2), 'm:',substring(Timestamp, 18, 2),'s')"/>
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