<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<html>
			<head>
				<title>Command Logs</title>
			</head>
			<body>
				<xsl:apply-templates select="//CommandReplay"/>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="CommandReplay">
		<div style="margin-bottom: 1em; font-family: monospace;">

			<!-- Type -->
			<xsl:choose>
				<xsl:when test="Type = 'Error'">
					<div style="color: red;">
						<strong>
							<xsl:value-of select="Type"/>
						</strong>
					</div>
				</xsl:when>
				<xsl:when test="Type = 'Success'">
					<div style="color: green;">
						<strong>
							<xsl:value-of select="Type"/>
						</strong>
					</div>
				</xsl:when>
				<xsl:when test="Type = 'Void'">
					<div style="color: goldenrod;">
						<strong>
							<xsl:value-of select="Type"/>
						</strong>
					</div>
				</xsl:when>
			</xsl:choose>

			<!-- Return -->
			<xsl:if test="Return">
				<div style="color: blue;">
					<strong>Return:</strong>
					<xsl:value-of select="Return"/>
				</div>
			</xsl:if>

			<!-- Timestamp -->
			<xsl:if test="Timestamp">
				<div>
					<strong>Timestamp:</strong>
					<xsl:value-of select="Timestamp"/>
				</div>
			</xsl:if>

			<!-- Function Called -->
			<xsl:if test="FunctionCalled">
				<div>
					<strong>Function:</strong>
					<xsl:value-of select="FunctionCalled"/>
				</div>
			</xsl:if>

			<!-- Message -->
			<xsl:if test="Message">
				<div>
					<strong>Message:</strong>
					<xsl:value-of select="Message"/>
				</div>
			</xsl:if>

			<!-- ThrowError -->
			<xsl:if test="ThrowError">
				<div>
					<strong>ThrowError:</strong>
					<xsl:value-of select="ThrowError"/>
				</div>
			</xsl:if>

			<!-- List Functions -->
			<xsl:if test="ListFunctions">
				<div>
					<strong>Available Functions:</strong>
					<ul>
						<xsl:for-each select="ListFunctions/ValueTupleOfStringString">
							<li>
								<strong>
									<xsl:value-of select="Item1"/>:
								</strong>
								<xsl:value-of select="Item2"/>
							</li>
						</xsl:for-each>
					</ul>
				</div>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>