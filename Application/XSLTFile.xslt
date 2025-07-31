<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="text" encoding="UTF-8"/>

	<xsl:template match="/">
		<xsl:apply-templates select="//CommandReply"/>
	</xsl:template>

	<xsl:template match="CommandReply">
		<!-- Tipo -->
		<xsl:choose>
			<xsl:when test="Type = 'Error'">
				<xsl:text>\x1B[31m[Error]\x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:when test="Type = 'Success'">
				<xsl:text>\x1B[32m[Success]\x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:when test="Type = 'Void'">
				<xsl:text>\x1B[33m[Void]\x1B[0m&#10;</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>\x1B[36m[Unknown Type]\x1B[0m&#10;</xsl:text>
			</xsl:otherwise>
		</xsl:choose>

		<!-- Lista de funciones -->
		<xsl:if test="Return/Entries/FunctionEntry">
			<xsl:text>Available Functions:&#10;</xsl:text>
			<xsl:for-each select="Return/Entries/FunctionEntry">
				<xsl:text>\x1B[34m	</xsl:text>
				<!-- Azul para función -->
				<xsl:value-of select="Function"/>
				<xsl:text>\x1B[0m</xsl:text>
				<xsl:text>	- </xsl:text>
				<xsl:value-of select="Description"/>
				<xsl:text>&#10;</xsl:text>
			</xsl:for-each>
			<xsl:text>Total functions: </xsl:text>
			<xsl:value-of select="count(Return/Entries/FunctionEntry)"/>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>

		<!-- Función invocada -->
		<xsl:if test="FunctionCalled">
			<xsl:text>Function Called: </xsl:text>
			<xsl:value-of select="FunctionCalled"/>
			<xsl:text>&#10;</xsl:text>

		</xsl:if>

		<!-- Mensaje opcional -->
		<xsl:if test="Message">
			<xsl:text>Message:\x1B[35m </xsl:text>
			<xsl:value-of select="Message"/>
			<xsl:text> \x1B[0m</xsl:text>
			<xsl:text>&#10;</xsl:text>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
