<?xml version='1.0'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:exsl="http://exslt.org/common"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                xmlns:rx="http://www.renderx.com/XSL/Extensions"
                exclude-result-prefixes="exsl"
                version='1.0'>

<xsl:import href="file:/R:/Shared/flib/docbook/xsl/fo/docbook.xsl"/>
<xsl:import href="titlepage.xsl"/>

<!-- infogrips -->
<xsl:variable name="infogrips.font.plain">Tahoma</xsl:variable>
<xsl:variable name="infogrips.font.serif">CenturySchoolbook</xsl:variable>
<xsl:variable name="infogrips.font.mono">Courier</xsl:variable>
<xsl:variable name="infogrips.addr.street">Technoparkstrasse 1</xsl:variable>
<xsl:variable name="infogrips.addr.city">8005 Z&#252;rich</xsl:variable>
<xsl:variable name="infogrips.addr.tel">044 / 350 10 10</xsl:variable>
<xsl:variable name="infogrips.addr.fax">044 / 350 10 19</xsl:variable>

<!-- general -->
<xsl:variable name="xep.extensions">1</xsl:variable>
<xsl:variable name="section.autolabel">1</xsl:variable>
<xsl:variable name="draft.mode">no</xsl:variable>
<xsl:variable name="title.margin.left">0pt</xsl:variable>
<xsl:attribute-set name="section.level1.properties">
  <xsl:attribute name="break-before">page</xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="appendix.level1.properties">
  <xsl:attribute name="break-before">page</xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="toc.margin.properties">
  <xsl:attribute name="break-after">page</xsl:attribute>
</xsl:attribute-set>

<!-- paper format -->
<xsl:variable name="paper.type">A4</xsl:variable>
<xsl:variable name="page.margin.top">0.6in</xsl:variable>
<xsl:variable name="page.margin.bottom">0.6in</xsl:variable>
<xsl:variable name="page.margin.outer">1.3in</xsl:variable>
<xsl:variable name="page.margin.inner">1in</xsl:variable>
<xsl:variable name="body.margin.top">0.4in</xsl:variable>
<xsl:variable name="body.margin.bottom">0.4in</xsl:variable>
<xsl:variable name="region.after.extent">2.0in</xsl:variable>
<xsl:variable name="region.before.extent">2.0in</xsl:variable> <!-- to be removed -->
<xsl:variable name="double.sided">1</xsl:variable>
<xsl:variable name="footer.rule">1</xsl:variable>

<!-- fonts / colors -->
<xsl:variable name="body.font.family"><xsl:value-of select="$infogrips.font.serif"/></xsl:variable>
<xsl:variable name="body.font.master">10</xsl:variable>
<xsl:variable name="title.font.family">Tahoma</xsl:variable>
<xsl:variable name="title.font.color">#000080</xsl:variable>
<xsl:variable name="href.font.color">#000080</xsl:variable>
<xsl:variable name="article.title.font.size">12pt</xsl:variable>
<xsl:variable name="toc.title.font.size">24pt</xsl:variable>
<xsl:variable name="chapter.title.font.size">24pt</xsl:variable>
<xsl:variable name="section.title.font.size">18pt</xsl:variable>

<!-- titles -->
<xsl:attribute-set name="section.title.properties">
  <xsl:attribute name="font-family">
    <xsl:value-of select="$title.font.family"></xsl:value-of>
  </xsl:attribute>
  <xsl:attribute name="font-weight">bold</xsl:attribute>
  <!-- font size is calculated dynamically by section.heading template -->
  <xsl:attribute name="keep-with-next.within-column">always</xsl:attribute>
  <xsl:attribute name="space-before.minimum">1.8em</xsl:attribute>
  <xsl:attribute name="space-before.optimum">2.0em</xsl:attribute>
  <xsl:attribute name="space-before.maximum">2.2em</xsl:attribute>
  <xsl:attribute name="space-after.minimum">0.8em</xsl:attribute>
  <xsl:attribute name="space-after.optimum">1.0em</xsl:attribute>
  <xsl:attribute name="space-after.maximum">1.2em</xsl:attribute>
  <xsl:attribute name="start-indent">0pt</xsl:attribute>
</xsl:attribute-set>

<!-- header / footers -->
<xsl:attribute-set name="footer.content.properties">
  <xsl:attribute name="color">#000000</xsl:attribute>
  <xsl:attribute name="font-weight">normal</xsl:attribute>
  <xsl:attribute name="font-style">normal</xsl:attribute>
</xsl:attribute-set>
<xsl:param name="header.column.widths" select="'1 0 1'"></xsl:param>
<xsl:param name="footer.column.widths" select="'1 0 1'"></xsl:param>

<!-- admonitions -->
<xsl:variable name="shade.verbatim">1</xsl:variable>
<xsl:variable name="admon.textlabel">0</xsl:variable>
<xsl:variable name="admon.graphics">1</xsl:variable>
<xsl:variable name="admon.graphics.path">file:/R:/Shared/flib/docbook/infogrips/common/images/</xsl:variable>

<!-- table of contents -->
<xsl:variable name="toc.section.depth">3</xsl:variable>
<xsl:variable name="generate.toc">
/appendix toc,title
article/appendix  nop
/article  toc,title
book      toc,title
/chapter  toc,title
part      toc,title
/preface  toc,title
qandadiv  toc
qandaset  toc
reference toc,title
/sect1    toc
/sect2    toc
/sect3    toc
/sect4    toc
/sect5    toc
/section  toc
set       toc,title
</xsl:variable>

<!--
 I don't use titlepage.templates.xml + templates.xsl mechanism
 since I find it rather awkward.
 I don't want to have one part of customization layer in titlepage.templates.xml
 and another part implemented as overriding templates. I'd better make 
 all of the customization 'by hand' in my templates even if it's
 considered to be 'not quite clear soultion'.
-->
<!-- ==================================================================== -->
<!-- Code below used to customize TOC appearance.		  	   			  -->
<!-- ==================================================================== -->

<xsl:template name="toc.line">
  <xsl:variable name="id">
    <xsl:call-template name="object.id"/>
  </xsl:variable>

  <xsl:variable name="label">
    <xsl:apply-templates select="." mode="label.markup"/>
  </xsl:variable>

  <fo:block text-align-last="justify"
            end-indent="{$toc.indent.width}pt"
            last-line-end-indent="-{$toc.indent.width}pt">
    <fo:inline keep-with-next.within-line="always">
      <fo:basic-link color="{$href.font.color}" internal-destination="{$id}">
        <xsl:if test="$label != ''">
          <xsl:copy-of select="$label"/>
          <xsl:value-of select="$autotoc.label.separator"/>
        </xsl:if>
        <xsl:apply-templates select="." mode="title.markup"/>
      </fo:basic-link>
    </fo:inline>
    <fo:inline keep-together.within-line="always">
      <xsl:text> </xsl:text>
      <fo:leader leader-pattern="dots"
                 leader-pattern-width="3pt"
                 leader-alignment="reference-area"
                 keep-with-next.within-line="always"/>
      <xsl:text> </xsl:text> 
      <fo:basic-link color="{$href.font.color}" internal-destination="{$id}">
        <fo:page-number-citation ref-id="{$id}"/>
      </fo:basic-link>
    </fo:inline>
  </fo:block>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize formal titles (figure, table...) 		  -->

<xsl:attribute-set name="formal.title.properties">
  <xsl:attribute name="color"><xsl:value-of select="$title.font.color"/></xsl:attribute>
</xsl:attribute-set>

<!-- ==================================================================== -->
<!-- Code below used to customize links appearance			  	 		  -->
<xsl:attribute-set name="xref.properties">
  <xsl:attribute name="color"><xsl:value-of select="$href.font.color"/></xsl:attribute>
  <xsl:attribute name="text-decoration">underline</xsl:attribute>
</xsl:attribute-set>


<!-- ==================================================================== -->
<!-- Code below used to customize verbatim appearance		  	 		  -->
<xsl:attribute-set name="shade.verbatim.style">
  <xsl:attribute name="background-color">#F0F0F0</xsl:attribute>
  <xsl:attribute name="padding">0pt 6pt 0pt 6pt</xsl:attribute>
  <xsl:attribute name="margin">0pt</xsl:attribute>
  <xsl:attribute name="line-stacking-strategy">font-height</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="monospace.verbatim.properties">
  <xsl:attribute name="white-space-treatment">preserve</xsl:attribute>
  <xsl:attribute name="space-before.minimum">0pt</xsl:attribute>
  <xsl:attribute name="space-before.optimum">6pt</xsl:attribute>
  <xsl:attribute name="space-before.maximum">6pt</xsl:attribute>
  <xsl:attribute name="space-after.minimum">0pt</xsl:attribute>
  <xsl:attribute name="space-after.optimum">6pt</xsl:attribute>
  <xsl:attribute name="space-after.maximum">6pt</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="monospace.properties">
  <xsl:attribute name="font-stretch">semi-condensed</xsl:attribute>
</xsl:attribute-set>


<!-- ==================================================================== -->
<!-- Code below used to customize Chapter/Section/TOC titles.		      -->

<xsl:attribute-set name="section.title.properties">
  <xsl:attribute name="color"><xsl:value-of select="$title.font.color"/></xsl:attribute>
</xsl:attribute-set>
<!--
 I don't use attribute sets for Chapter and TOC since some of parameters
 (font-size) are hard-coded in templates.
-->

<xsl:template match="title" mode="chapter.titlepage.recto.auto.mode">
	<fo:block xmlns:fo="http://www.w3.org/1999/XSL/Format" xsl:use-attribute-sets="chapter.titlepage.recto.style" font-size="{$chapter.title.font.size}" font-weight="bold" color="{$title.font.color}" text-align="left">
		<xsl:call-template name="component.title">
			<xsl:with-param name="node" select="ancestor-or-self::chapter[1]"/>
		</xsl:call-template>
	</fo:block>
</xsl:template>

<xsl:template name="table.of.contents.titlepage.recto">
	<fo:block xmlns:fo="http://www.w3.org/1999/XSL/Format" xsl:use-attribute-sets="table.of.contents.titlepage.recto.style" space-before.minimum="1em" space-before.optimum="1.5em" space-before.maximum="2em" space-after="0.5em" margin-left="{$title.margin.left}" font-size="{$toc.title.font.size}" font-weight="bold" font-family="{$title.font.family}" color="{$title.font.color}">

		<xsl:call-template name="gentext">
			<xsl:with-param name="key" select="'TableofContents'"/>
		</xsl:call-template>
	</fo:block>
</xsl:template>


<!-- ==================================================================== -->
<!-- Code below used to customize book title page appearence. 		  	  -->

<xsl:template name="book.titlepage">
  <fo:block-container display-align="center" height="100%" border="thin silver ridge">
    <fo:block>
    <xsl:call-template name="book.titlepage.recto"/>
    </fo:block>
  </fo:block-container>
</xsl:template>

<xsl:template name="book.titlepage.recto">
  <xsl:choose>
    <xsl:when test="bookinfo/title">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="bookinfo/title"/>
    </xsl:when>
    <xsl:when test="title">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="title"/>
    </xsl:when>
  </xsl:choose>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize Section headings styles (font-size) 	  -->

<xsl:attribute-set name="section.title.level1.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="24"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.title.level2.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="18"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.title.level3.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="15"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.title.level4.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="15"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.title.level5.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="15"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.title.level6.properties">
  <xsl:attribute name="font-size">
    <xsl:value-of select="15"/>
    <xsl:text>pt</xsl:text>
  </xsl:attribute>
</xsl:attribute-set>

<!-- ==================================================================== -->
<!-- Code below used to turn of formating for some inlines inside TOC	  -->
<!-- For now, <classname> is the only inline used.						  -->
<xsl:template match="classname" mode="no.anchor.mode">
  <xsl:apply-templates select="." />
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize running headers						  -->

<xsl:template name="header.table">

  <xsl:param name="pageclass" select="''"/>
  <xsl:param name="sequence" select="''"/>
  <xsl:param name="gentext-key" select="''"/>

  <!-- default is a single table style for all headers -->
  <!-- Customize it for different page classes or sequence location -->

  <xsl:choose>
    <xsl:when test="$sequence='first'">
        <fo:block border-bottom="thin solid black">
        <fo:table table-layout="fixed" width="100%">
           <fo:table-column column-width="proportional-column-width(1)"/>
           <fo:table-column column-width="proportional-column-width(1)"/>
           <fo:table-column column-width="proportional-column-width(1)"/>
           <fo:table-body>
              <fo:table-row height="14pt">
                 <fo:table-cell>
                    <fo:block text-align="left" font="normal 9pt {$infogrips.font.plain}" >
                       <fo:block>
                          <fo:external-graphic src="url('file:/R:/Shared/flib/docbook/infogrips/common/images/infogrips.png')" content-height="2em"/>
                       </fo:block>
                       <fo:block>
                       </fo:block>
                    </fo:block>
                 </fo:table-cell>
                 <fo:table-cell>
                    <fo:block text-align="center" font="normal 9pt {$infogrips.font.plain}">
                       <fo:block>
                          <xsl:value-of select="$infogrips.addr.street"/>
                       </fo:block>
                       <fo:block>
                          <xsl:value-of select="$infogrips.addr.city"/>
                       </fo:block>
                    </fo:block>
                 </fo:table-cell>
                 <fo:table-cell>
                    <fo:block text-align="right" font="normal 9pt {$infogrips.font.plain}">
                       <fo:block>
                          <xsl:text>Tel.: </xsl:text> <xsl:value-of select="$infogrips.addr.tel"/>
                       </fo:block>
                       <fo:block>
                          <xsl:text>Fax.: </xsl:text> <xsl:value-of select="$infogrips.addr.fax"/>
                       </fo:block>
                    </fo:block>
                 </fo:table-cell>
              </fo:table-row>
           </fo:table-body>
        </fo:table>
        </fo:block>
    </xsl:when>
    <xsl:when test="$sequence='even'">
      <fo:block border-bottom="thin solid black">
      <fo:table table-layout="fixed" width="100%">
           <fo:table-column column-width="proportional-column-width(1)"/>
           <fo:table-body>
              <fo:table-row height="14pt">
                 <fo:table-cell>
                    <fo:block text-align="left" font="normal 9pt {$infogrips.font.plain}">
                       <xsl:value-of select="/article/title"/>
                       <xsl:text>, </xsl:text>
                       <xsl:value-of select="/article/articleinfo/pubdate"/>
                    </fo:block>
                 </fo:table-cell>
              </fo:table-row>
           </fo:table-body>
        </fo:table>
        </fo:block>
    </xsl:when>
    <xsl:when test="$sequence='odd'">
      <fo:block border-bottom="thin solid black">
      <fo:table table-layout="fixed" width="100%">
           <fo:table-column column-width="proportional-column-width(1)"/>
           <fo:table-body>
              <fo:table-row height="14pt">
                 <fo:table-cell>
                    <fo:block text-align="right" font="normal 9pt {$infogrips.font.plain}">
                       <xsl:value-of select="/article/title"/>
                       <xsl:text>, </xsl:text>
                       <xsl:value-of select="/article/articleinfo/pubdate"/>
                    </fo:block>
                 </fo:table-cell>
              </fo:table-row>
           </fo:table-body>
      </fo:table>
      </fo:block>
    </xsl:when>
    <xsl:otherwise>
      <fo:block>
         <xsl:value-of select="$pageclass"/>
         <xsl:text>:</xsl:text>
         <xsl:value-of select="$sequence"/>
         <xsl:text>:</xsl:text>
         <xsl:value-of select="$gentext-key"/>
      </fo:block>
    </xsl:otherwise>
  </xsl:choose>

</xsl:template>

<xsl:template name="header.content">
  <xsl:param name="pageclass" select="''"/>
  <xsl:param name="sequence" select="''"/>
  <xsl:param name="gentext-key" select="''"/>
  <xsl:param name="position" select="''"/>

  <!-- <fo:block>
    <xsl:value-of select="$pageclass"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$sequence"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$postion"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$gentext-key"/>
  </fo:block> -->

  <xsl:variable name="header.align">
    <xsl:choose>
    	<xsl:when test="$double.sided != 0 and $sequence='odd'">left</xsl:when>
		<xsl:when test="$double.sided != 0 and $sequence='even'">right</xsl:when>
		<xsl:when test="$double.sided = 0 and ($sequence='first' or $sequence='even' or $sequence='odd')">center</xsl:when>
		<xsl:otherwise>nop</xsl:otherwise>
	</xsl:choose>
  </xsl:variable>

  <fo:block font="normal 9pt {$infogrips.font.plain}">

    <!-- sequence can be odd, even, first, blank -->
    <!-- position can be left, center, right -->
    <xsl:choose>
      <xsl:when test="$sequence = 'blank'">
        <!-- nothing -->
      </xsl:when>

      <xsl:when test="$header.align!='nop'">
        <xsl:if test="$pageclass = 'body'">
        	<fo:block text-align="{$header.align}" border-bottom="medium solid black">
            <xsl:value-of select="/article/title"/>
            <fo:inline>, </fo:inline>
            <xsl:value-of select="/article/articleinfo/pubdate"/>
			</fo:block>
        </xsl:if>
      </xsl:when>
      
      <xsl:when test="$draft.mode!=0">
        <!-- Same for odd, even, empty, and blank sequences -->
        <xsl:call-template name="draft.text"/>
      </xsl:when>
      
      <xsl:when test="$sequence = 'first' and $position='right'">
        <xsl:value-of select="/article/articleinfo/pubdate"/>
      </xsl:when>

      <xsl:when test="$sequence = 'blank'">
        <!-- nothing for blank pages -->
      </xsl:when>
    </xsl:choose>
  </fo:block>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize running footers						  -->

<xsl:template name="page.number">
	<fo:inline font="normal 9pt {$infogrips.font.plain}">Seite <fo:page-number/>
	</fo:inline>
</xsl:template>

<xsl:template name="footer.content">
  <xsl:param name="pageclass" select="''"/>
  <xsl:param name="sequence" select="''"/>
  <xsl:param name="position" select="''"/>
  <xsl:param name="gentext-key" select="''"/>

  <!--
  <fo:block>
    <xsl:value-of select="$pageclass"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$sequence"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$position"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$gentext-key"/>
  </fo:block>
  -->

  <fo:block font="normal 9pt {$infogrips.font.plain}">
    <!-- pageclass can be front, body, back -->
    <!-- sequence can be odd, even, first, blank -->
    <!-- position can be left, center, right -->
    <xsl:choose>
      <xsl:when test="$pageclass = 'titlepage'">
        <!-- nop; no footer on title pages -->      </xsl:when>

      <xsl:when test="$double.sided != 0 and $sequence = 'even'
                      and $position='left'">
        <xsl:call-template name="page.number"/>
      </xsl:when>

      <xsl:when test="$double.sided != 0 and $sequence = 'even'
                      and $position='right'">
            <fo:inline>Copyright &#169; </fo:inline><xsl:value-of select="/article/articleinfo/copyright"/>
      </xsl:when>

      <xsl:when test="$double.sided != 0 and $sequence = 'odd'
                      and $position='left'">
            <fo:inline>Copyright © </fo:inline><xsl:value-of select="/article/articleinfo/copyright"/>
      </xsl:when>

      <xsl:when test="$sequence = 'first' and $position='right'">
        <xsl:value-of select="/article/articleinfo/pubdate"/>
      </xsl:when>

      <xsl:when test="$double.sided != 0 and ($sequence = 'odd')
                      and $position='right'">
        <xsl:call-template name="page.number"/>
      </xsl:when>

      <xsl:when test="$double.sided != 0 and ($sequence = 'odd' or $sequence = 'first')
                      and $position='left'">
            <fo:inline>Copyright © </fo:inline><xsl:value-of select="/article/articleinfo/copyright"/>
      </xsl:when>

      <xsl:when test="$sequence='blank'">
        <xsl:choose>
          <xsl:when test="$double.sided != 0 and $position = 'left'">
            <xsl:call-template name="page.number"/>
          </xsl:when>
          <xsl:when test="$double.sided = 0 and $position = 'center'">
            <xsl:call-template name="page.number"/>
          </xsl:when>
          <xsl:otherwise>
            <!-- nop -->
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <!-- nop -->
      </xsl:otherwise>
    </xsl:choose>
  </fo:block>
</xsl:template>

<!-- Just to change footer separator... -->

<xsl:template name="foot.sep.rule">
  <xsl:if test="$footer.rule != 0">
    <xsl:attribute name="border-top-width">thin</xsl:attribute>
    <xsl:attribute name="border-top-style">dotted</xsl:attribute>
    <xsl:attribute name="border-top-color">black</xsl:attribute>
  </xsl:if>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to add 'terminator' block in the and of article flow -->
<!-- and to move 'Abstract' before TOC 									  -->

<xsl:template match="article">
  <xsl:variable name="id">
    <xsl:call-template name="object.id"/>
  </xsl:variable>

  <xsl:variable name="master-reference">
    <xsl:call-template name="select.pagemaster"/>
  </xsl:variable>

  <fo:page-sequence id="{$id}"
                    hyphenate="{$hyphenate}"
                    master-reference="{$master-reference}">
    <xsl:attribute name="language">
      <xsl:call-template name="l10n.language"/>
    </xsl:attribute>
    <xsl:attribute name="format">
      <xsl:call-template name="page.number.format"/>
    </xsl:attribute>
    <xsl:choose>
      <xsl:when test="not(preceding::chapter
                          or preceding::preface
                          or preceding::appendix
                          or preceding::article
                          or preceding::dedication
                          or parent::part
                          or parent::reference)">
        <!-- if there is a preceding component or we're in a part, the -->
        <!-- page numbering will already be adjusted -->
        <xsl:attribute name="initial-page-number">1</xsl:attribute>
      </xsl:when>
      <xsl:when test="$double.sided != 0">
        <xsl:attribute name="initial-page-number">auto-odd</xsl:attribute>
      </xsl:when>
    </xsl:choose>

    <xsl:apply-templates select="." mode="running.head.mode">
      <xsl:with-param name="master-reference" select="$master-reference"/>
    </xsl:apply-templates>

    <xsl:apply-templates select="." mode="running.foot.mode">
      <xsl:with-param name="master-reference" select="$master-reference"/>
    </xsl:apply-templates>

    <fo:flow flow-name="xsl-region-body">
      <xsl:call-template name="article.titlepage"/>

	  <xsl:apply-templates select="abstract"/>
	
      <xsl:variable name="toc.params">
        <xsl:call-template name="find.path.params">
          <xsl:with-param name="table" select="normalize-space($generate.toc)"/>
        </xsl:call-template>
      </xsl:variable>

      <xsl:if test="contains($toc.params, 'toc')">
        <xsl:call-template name="component.toc"/>
        <xsl:call-template name="component.toc.separator"/>
      </xsl:if>
      <xsl:apply-templates select="*[not(self::abstract)]"/>
      <xsl:if test="not(following-sibling::article)"><fo:block id="terminator"/></xsl:if>
    </fo:flow>
  </fo:page-sequence>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize footnotes					 		 	  -->
<!-- We do not want special treatment for footnotes in tables			  -->

<xsl:template match="footnote" name="process.footnote" mode="table.footnote.mode"/>

<xsl:template match="footnote">
      <fo:footnote>
        <fo:inline>
          <xsl:call-template name="format.footnote.mark">
            <xsl:with-param name="mark">
              <xsl:apply-templates select="." mode="footnote.number"/>
            </xsl:with-param>
          </xsl:call-template>
        </fo:inline>
        <fo:footnote-body font-family="{$body.font.family}"
                          font-size="{$footnote.font.size}"
                          font-weight="normal"
                          font-style="normal"
                          text-align="left"
                          margin-left="0pc">
          <xsl:apply-templates mode="format.footnote.body" select="."/>
        </fo:footnote-body>
      </fo:footnote>
</xsl:template>

<xsl:template name="format.footnote.mark">
  <xsl:param name="mark" select="'?'"/>
  <fo:inline baseline-shift="super" font-size="75%">
    <xsl:copy-of select="$mark"/>
  </fo:inline>
</xsl:template>

<xsl:template match="footnote/para[1]
                     |footnote/simpara[1]
                     |footnote/formalpara[1]"
              priority="2">
  <xsl:apply-templates/>
</xsl:template>
<xsl:template match="footnote"
              mode="format.footnote.body">

  <!-- this only works if the first thing in a footnote is a para, -->
  <!-- which is ok, because it usually is. -->

      <fo:list-block provisional-distance-between-starts="15pt"
                     provisional-label-separation="0pt">
        <fo:list-item>
          <fo:list-item-label end-indent="label-end()">
            <fo:block text-align="start">
			    <xsl:call-template name="format.footnote.mark">
      				<xsl:with-param name="mark">
        				<xsl:apply-templates select="." mode="footnote.number"/>
      				</xsl:with-param>
				</xsl:call-template>
            </fo:block>
          </fo:list-item-label>
          <fo:list-item-body start-indent="body-start()">
            <xsl:apply-templates select="*" mode="format.footnote.body"/>
          </fo:list-item-body>
        </fo:list-item>
      </fo:list-block>
</xsl:template>

<!-- Two templates below redefines para appearance inside footnotes and format all other blocks as by default -->
<xsl:template match="*" mode="format.footnote.body" priority="-10">
  <xsl:apply-templates select="."/>
</xsl:template>

<xsl:template match="para" mode="format.footnote.body">
  <fo:block padding-top="2pt">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>


<xsl:template match="footnote" mode="footnote.number">
  <xsl:choose>
    <xsl:when test="ancestor::tgroup">
      <xsl:variable name="tfnum">
        <xsl:number level="any" format="1"/> <!-- from="table|informaltable" removed -->
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="string-length($table.footnote.number.symbols) &gt;= $tfnum">
          <xsl:value-of select="substring($table.footnote.number.symbols, $tfnum, 1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:number level="any"
                      format="{$table.footnote.number.format}"/> <!-- from="tgroup" removed -->
        </xsl:otherwise>
      </xsl:choose>
    </xsl:when>
    <xsl:otherwise>
      <xsl:variable name="pfoot" select="preceding::footnote"/>
      <xsl:variable name="ptfoot" select="preceding::tgroup//footnote"/>
      <xsl:variable name="fnum" select="count($pfoot) - count($ptfoot) + 1"/>

      <xsl:choose>
        <xsl:when test="string-length($footnote.number.symbols) &gt;= $fnum">
          <xsl:value-of select="substring($footnote.number.symbols, $fnum, 1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:number value="$fnum" format="{$footnote.number.format}"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>


<!-- ==================================================================== -->
<!-- Code below used to customize table headers appearance				  -->
<xsl:template match="thead">
  <xsl:variable name="tgroup" select="parent::*"/>

  <fo:table-header background-color="silver">
    <xsl:apply-templates select="row[1]">
      <xsl:with-param name="spans">
        <xsl:call-template name="blank.spans">
          <xsl:with-param name="cols" select="../@cols"/>
        </xsl:call-template>
      </xsl:with-param>
    </xsl:apply-templates>
  </fo:table-header>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize meta-information						  -->

<xsl:template match="author" mode="meta-info">
  <xsl:variable name="author">
      <xsl:call-template name="person.name">
         <xsl:with-param name="node" select="."/>
      </xsl:call-template>
  </xsl:variable>
  <rx:meta-field name="author" value="{normalize-space($author)}"/>
</xsl:template>

<xsl:template match="authorgroup" mode="meta-info">
  <xsl:variable name="authors">
    <xsl:for-each select="author">
      <xsl:call-template name="person.name">
         <xsl:with-param name="node" select="."/>
      </xsl:call-template>
      <xsl:if test="following-sibling::author">
        <xsl:text>, </xsl:text>
      </xsl:if>
    </xsl:for-each>
  </xsl:variable>
  <rx:meta-field name="author" value="{normalize-space($authors)}"/>
</xsl:template>

<xsl:template name="xep-document-information">
  <rx:meta-info>
  	<xsl:choose>
  		<xsl:when test="//authorgroup">
			<xsl:apply-templates select="//authorgroup" mode="meta-info"/>
		</xsl:when>
		<xsl:when test="//author[1]">
			<xsl:apply-templates select="//author" mode="meta-info"/>
		</xsl:when>
	</xsl:choose>

    <xsl:variable name="title">
      <xsl:apply-templates select="/*[1]" mode="label.markup"/>
      <xsl:apply-templates select="/*[1]" mode="title.markup"/>
    </xsl:variable>

    <xsl:element name="rx:meta-field">
      <xsl:attribute name="name">title</xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="$title"/>
      </xsl:attribute>
    </xsl:element>

    <xsl:if test="//keyword">
      <xsl:element name="rx:meta-field">
        <xsl:attribute name="name">keywords</xsl:attribute>
        <xsl:attribute name="value">
          <xsl:for-each select="//keyword">
            <xsl:value-of select="."/>
            <xsl:if test="position() != last()">
              <xsl:text>, </xsl:text>
            </xsl:if>
          </xsl:for-each>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>

    <xsl:if test="//subjectterm">
      <xsl:element name="rx:meta-field">
        <xsl:attribute name="name">subject</xsl:attribute>
        <xsl:attribute name="value">
          <xsl:for-each select="//subjectterm">
            <xsl:value-of select="."/>
            <xsl:if test="position() != last()">
              <xsl:text>, </xsl:text>
            </xsl:if>
          </xsl:for-each>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </rx:meta-info>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to customize informalexample						  -->

<xsl:template match="informalexample">
  <fo:block border="0.5pt gray solid"
  			margin="0pt"
            padding="0pt 12pt 0pt 12pt"
            space-before="6pt"
            space-after="6pt">
  	<xsl:call-template name="informal.object"/>
  </fo:block>
</xsl:template>

<!-- ==================================================================== -->
<!-- Code below used to remove side effect (font-stretch) from callouts	  -->

<xsl:template match="co">
  <fo:inline id="{@id}" font-stretch="normal">
    <xsl:apply-templates select="." mode="callout-bug"/>
  </fo:inline>
</xsl:template>


<!-- =============================================================== -->
<!-- Code below used to customize definition list 					 -->

<xsl:template match="variablelist">
  <fo:block space-before.optimum="6pt"
            space-after.optimum="6pt">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<xsl:template match="varlistentry">
  <fo:block space-before.optimum="6pt"
            keep-together.within-column="always"
            keep-with-next.within-column="always"
            font-weight="bold"
            line-height="1.1">
    <xsl:for-each select="term">
      <fo:block><xsl:apply-templates/></fo:block>
    </xsl:for-each>
  </fo:block>

  <xsl:apply-templates select="listitem"/>
</xsl:template>

<xsl:template match="varlistentry/listitem">
  <fo:block space-before.optimum="3pt"
            margin-left="0.5in">
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>

<!-- =============================================================== -->
<!-- Code below used to customize <note> and <important> elements    -->

<xsl:template match="note|important">
  <fo:table space-before.optimum="3pt">
    <fo:table-column column-width="30pt"/>
    <fo:table-body>
      <fo:table-row>
        <fo:table-cell>
          <xsl:choose>
            <xsl:when test="name()='note'">
              <fo:block font="24pt ZapfDingbats">&#x261E;</fo:block>
            </xsl:when>
            <xsl:otherwise>
	          <fo:block font-family="Helvetica"
    	      		    font-size="24pt"
        	  		    font-weight="bold"
					    text-align="center">!</fo:block>
            </xsl:otherwise>
          </xsl:choose>
        </fo:table-cell>
        <fo:table-cell padding-top="6pt">
          <fo:block font-size="90%"><xsl:apply-templates/></fo:block>
        </fo:table-cell>
      </fo:table-row>
    </fo:table-body>
  </fo:table>
</xsl:template>

<!-- =============================================================== -->
<!-- Code below used to customize <application> appearance	     -->
<xsl:template match="application">
  <xsl:call-template name="inline.italicseq"/>
</xsl:template>

<!-- =============================================================== -->
<!-- Code below used to customize <property> appearance	    	     -->
<xsl:template match="property">
  <xsl:call-template name="inline.monoseq"/>
</xsl:template>

<!-- =============================================================== -->
<!-- Code below used to customize <literallayout> appearance -->

<xsl:template match="literallayout">

  <xsl:param name="suppress-numbers" select="'0'"/>
  <xsl:variable name="id"><xsl:call-template name="object.id"/></xsl:variable>

  <xsl:variable name="content">
    <xsl:choose>
      <xsl:when test="$suppress-numbers = '0'
                      and @linenumbering = 'numbered'
                      and $use.extensions != '0'
                      and $linenumbering.extension != '0'">
        <xsl:call-template name="number.rtf.lines">
          <xsl:with-param name="rtf">
            <xsl:apply-templates/>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <fo:block id="{$id}"
     white-space-collapse='false'
     white-space-treatment='preserve'
     linefeed-treatment="preserve"
     xsl:use-attribute-sets="monospace.verbatim.properties">
     <xsl:copy-of select="$content"/>
  </fo:block>

</xsl:template>

<xsl:template match="command">
  <xsl:call-template name="inline.monoseq"/>
</xsl:template>

</xsl:stylesheet>
