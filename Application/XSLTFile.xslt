<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="text" encoding="UTF-8"/>

	<xsl:template match="/">
		<xsl:apply-templates select="//CommandReply"/>
	</xsl:template>

	<xsl:template match="CommandReply">
		<!-- Type -->
		<xsl:choose>
			<xsl:when test="Type = 'Error'">
				<xsl:text>\x1B[31m Error \x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:when test="Type = 'Success'">
				<xsl:text>\x1B[32m Success \x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:when test="Type = 'Void'">
				<xsl:text>\x1B[33m Void \x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>\x1B[36m[Unknown Type]\x1B[0m&#10;</xsl:text>
			</xsl:otherwise>
		</xsl:choose>

		<!-- Return for list -->
		<xsl:if test="Return/Entries/FunctionEntry">
			<xsl:text>Available:&#10;</xsl:text>
			<xsl:text>&#10;</xsl:text>
			<xsl:for-each select="Return/Entries/FunctionEntry">
				<xsl:text>\x1B[34m  </xsl:text>
				<!-- Function name -->
				<xsl:value-of select="Function"/>

				<!-- Parameters inline -->
				<xsl:if test="Parameters/string">
					<xsl:text>\x1B[90m</xsl:text>
					<xsl:text>	(</xsl:text>
					<xsl:for-each select="Parameters/string">
						<xsl:value-of select="."/>
						<xsl:if test="position() != last()">
							<xsl:text>,</xsl:text>
						</xsl:if>
					</xsl:for-each>
					<xsl:text>)</xsl:text>
				</xsl:if>

				<xsl:text>\x1B[0m</xsl:text>
				<xsl:text>	</xsl:text>
				<xsl:value-of select="Description"/>
				<xsl:text>&#10;</xsl:text>
			</xsl:for-each>
			<xsl:text>&#10;</xsl:text>

			<xsl:text>Total: </xsl:text>
			<xsl:value-of select="count(Return/Entries/FunctionEntry)"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
		
		<!-- Return -->
		<xsl:if test="Return and not(Return/Entries/FunctionEntry)">
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
			<xsl:text>Function Called: </xsl:text>
			<xsl:value-of select="FunctionCalled"/>
			<xsl:text>&#10;</xsl:text>

		</xsl:if>

		<!-- Message -->
		<xsl:if test="Message">
			<xsl:text>Message: </xsl:text>
			<xsl:value-of select="Message"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
