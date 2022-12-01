<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuChuyenTiep.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuChuyenTiep" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        * {
            margin: 0px;
            padding: 0px;
        }

        body {
            font-size: 15px;
            font-family: "Times New Roman", Times, serif;
        }

        .print {
            text-decoration: none;
            text-transform: uppercase;
            padding: 5px 15px;
            border: solid 1px #056fc4;
            color: #fff;
            background: url(/images/button.png) repeat-x 0 0;
        }
    </style>
    <style>
        <!--
        /* Font Definitions */
        @font-face {
            font-family: Wingdings;
            panose-1: 5 0 0 0 0 0 0 0 0 0;
        }

        @font-face {
            font-family: "Cambria Math";
            panose-1: 2 4 5 3 5 4 6 3 2 4;
        }
        /* Style Definitions */
        p.MsoNormal, li.MsoNormal, div.MsoNormal {
            margin: 0cm;
            margin-bottom: .0001pt;
            font-size: 12.0pt;
            font-family: "Times New Roman","serif";
        }

        p.MsoFooter, li.MsoFooter, div.MsoFooter {
            margin: 0cm;
            margin-bottom: .0001pt;
            font-size: 12.0pt;
            font-family: "Times New Roman","serif";
        }

        p {
            margin-right: 0cm;
            margin-left: 0cm;
            font-size: 12.0pt;
            font-family: "Times New Roman","serif";
        }
        /* Page Definitions */
        @page WordSection1 {
            size: 21.0cm 842.0pt;
            margin: 2.0cm 42.55pt 2.0cm 3.0cm;
        }

        div.WordSection1 {
            page: WordSection1;
        }
        /* List Definitions */
        ol {
            margin-bottom: 0cm;
        }

        ul {
            margin-bottom: 0cm;
        }

        table.MsoNormalTable {
            mso-style-name: "Table Normal";
            mso-tstyle-rowband-size: 0;
            mso-tstyle-colband-size: 0;
            mso-style-noshow: yes;
            mso-style-unhide: no;
            mso-style-parent: "";
            mso-padding-alt: 0cm 5.4pt 0cm 5.4pt;
            mso-para-margin: 0cm;
            mso-para-margin-bottom: .0001pt;
            mso-pagination: widow-orphan;
            font-size: 10.0pt;
            font-family: "Times New Roman","serif";
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 483.5pt; margin: 10px auto;" id="baocao">
            <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0" width="645"
                style='width: 483.5pt; border-collapse: collapse; mso-padding-alt: 0cm 0cm 0cm 0cm;'>
                <tr style='mso-yfti-irow: 0; mso-yfti-firstrow: yes'>
                    <td width="223" valign="top" style='width: 167.4pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b
                                style='mso-bidi-font-weight: normal'><span lang="EN-GB" style='mso-bidi-font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>ĐƠN V&#7882; CH&#7910;
  QU&#7842;N<o:p></o:p></span></b>
                        </p>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <span
                                lang="EN-GB" style='mso-bidi-font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'> <u><asp:Literal ID="ltDonViTiepNhan" runat="server"></asp:Literal></u> </span><span
                                                                                                                                                                                                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><br/>
                                </span><span lang="EN-GB" style='mso-bidi-font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>S&#7889;: </span><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>................<sup>(2)</sup></span><sup><span
                                    lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><o:p></o:p></span></sup>
                        </p>
                    </td>
                    <td width="421" valign="top" style='width: 316.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='margin-left: 3.6pt; text-align: center; line-height: 110%'>
                            <b><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>C&#7896;NG H&Ograve;A X&Atilde; H&#7896;I CH&#7910; NGH&#296;A
                               VI&#7878;T NAM<br />
                                Đ&#7897;c <u>l&#7853;p - T&#7921; do - H&#7841;nh</u> phúc</span></b><span
                                    lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><o:p></o:p></span>
                        </p>
                    </td>
                </tr>
                <tr style='mso-yfti-irow: 1; mso-yfti-lastrow: yes'>
                    <td width="223" valign="top" style='width: 167.4pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height: 110%'>
                            <span lang="EN-GB"
                                style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>&nbsp;</span><span
                                    lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><o:p></o:p></span>
                        </p>
                    </td>
                    <td width="421" valign="top" style='width: 316.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="right" style='margin-right: 7.0pt; text-align: right; line-height: 110%'>
                            <i><span lang="EN-GB" style='mso-bidi-font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>............<span class="GramE">,ngày</span>.<asp:Literal ID="ltNgay" runat="server"></asp:Literal>.tháng.<asp:Literal ID="ltThang" runat="server"></asp:Literal>.năm.<asp:Literal ID="ltNam" runat="server"></asp:Literal>.</span></i><span
                                lang="EN-US" style='mso-bidi-font-size: 14.0pt; line-height: 110%'><o:p></o:p></span>
                        </p>
                    </td>
                </tr>
            </table>
            <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0" style='width: 483.5pt; border-collapse: collapse; mso-padding-alt: 0cm 0cm 0cm 0cm;'>
                <tr>
                    <td>
                        <p class="MsoNormal" style='text-align: center; line-height: 110%'>
                            <span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>
                                <o:p>&nbsp;</o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='text-align: center; line-height: 110%'>
                            <b
                                style='mso-bidi-font-weight: normal'><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>PHI&#7870;U CHUY&#7874;N TI&#7870;P KHI&#7870;U
N&#7840;I<o:p></o:p></span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b
                                style='mso-bidi-font-weight: normal'><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>
                                    <o:p>&nbsp;</o:p>
                                </span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 72.0pt; text-align: justify; line-height: 110%; tab-stops: right dotted 396.0pt'>
                            <span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>Kính g&#7917;i:<span
                                style='mso-tab-count: 1 dotted'>.................................................................... </span><sup>(1)<o:p></o:p></sup></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 72.0pt; text-align: justify; line-height: 110%; tab-stops: right dotted 396.0pt'>
                            <sup><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: EN-GB'>
                                <o:p>&nbsp;</o:p>
                            </span></sup>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 20.0pt; text-align: justify; line-height: 110%; tab-stops: right dotted 144.0pt 216.0pt blank 468.0pt'>
                            <sup><span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>(2)</span></sup><span
                                    style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: VI'>&nbsp;</span><span
                                        lang="EN-US" style='font-size: 14.0pt; line-height: 110%'> <span style='mso-tab-count: 2 dotted'>....................... </span><span class="GramE">nh&#7853;n</span> được khiếu nại của khách hàng <asp:Literal ID="ltTenKH" runat="server"></asp:Literal><span
                                            style='mso-tab-count: 1'> &nbsp;</span><sup>(3)<o:p></o:p></sup></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='text-align: justify; line-height: 110%; tab-stops: right dotted 369.0pt'>
                            <sup><span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><span
                                    style='mso-spacerun: yes'></span></span></sup><span class="GramE"><span
                                        lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>v&#7873;</span></span><span
                                            lang="EN-US" style='font-size: 14.0pt; line-height: 110%'> v&#7845;n đ&#7873; <span
                                                style='mso-tab-count: 1 dotted'>........................................................................................................... </span><sup>(4)</sup>.<o:p></o:p></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%; tab-stops: right dotted 468.0pt'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>L&yacute; do không
gi&#7843;i quy&#7871;t đư&#7907;c: <span style='mso-tab-count: 1 dotted'>.................................................................................. </span>
                                <o:p></o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%; tab-stops: right dotted 468.0pt'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><span style='mso-tab-count: 1 dotted'>............................................................................................................................... </span>
                                <o:p></o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%'>
                            <span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>H&#7891; sơ kèm <span class="GramE">theo</span>
                                g&#7891;m:<o:p></o:p></span>
                        </p>

                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%; tab-stops: right dotted 468.0pt'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>1 <span style='mso-tab-count: 1 dotted'>................................................................................................................... </span>
                                <o:p></o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%; tab-stops: right dotted 468.0pt'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>2 <span style='mso-tab-count: 1 dotted'>................................................................................................................... </span>
                                <o:p></o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%; tab-stops: right dotted 468.0pt'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>3 <span style='mso-tab-count: 1 dotted'>................................................................................................................... </span>
                                <o:p></o:p>
                            </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%; tab-stops: dotted 396.0pt right 468.0pt'>
                            <span
                                class="GramE"><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Chúng
tôi xin chuy&#7875;n ti&#7871;p h&#7891; sơ, đ&#7873; ngh&#7883; <sup>(1) </sup><span
    style='mso-tab-count: 1 dotted'>.................................... </span>xem
xét gi&#7843;i quy&#7871;t và thông báo k&#7871;t qu&#7843; tr&#7843; l&#7901;i
khi&#7871;u n&#7841;i t&#7899;i khách hàng.</span></span><span lang="EN-US"
    style='font-size: 14.0pt; line-height: 110%'><o:p></o:p></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style="margin-left: 36.0pt; ">
                            <span lang="EN-US" style='font-size: 14.0pt; mso-bidi-font-size: 12.0pt'><span style='mso-tab-count: 1'></span>Trân tr&#7885;ng cám ơn<span
                                class="GramE">./</span>.<o:p></o:p></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0"
                            style='border-collapse: collapse; mso-padding-alt: 0cm 0cm 0cm 0cm'>
                            <tr style='mso-yfti-irow: 0; mso-yfti-firstrow: yes; mso-yfti-lastrow: yes'>
                                <td width="308" valign="top" style='width: 231.05pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                                    <p class="MsoNormal" style='line-height: 110%'>
                                        <i style='mso-bidi-font-style: normal'><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>
                                            <o:p>&nbsp;</o:p>
                                        </span></i>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                                        <b
                                            style='mso-bidi-font-weight: normal'><i style='mso-bidi-font-style: normal'><span
                                                lang="EN-US" style='font-size: 11.0pt; mso-bidi-font-size: 14.0pt; line-height: 110%'>Nơi nh&#7853;n<o:p></o:p></span></i></b>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 31.5pt; text-indent: -18.0pt; line-height: 110%; mso-list: l0 level1 lfo4'>
                                        <![if !supportLists]><span lang="EN-US"
                                            style='font-size: 11.0pt; mso-bidi-font-size: 14.0pt; line-height: 110%; font-family: "Courier New"; mso-fareast-font-family: "Courier New"'><span
                                                style='mso-list: Ignore'>&#821;<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp;
                                                </span></span></span><![endif]><i style='mso-bidi-font-style: normal'><span
                                                    lang="EN-US" style='font-size: 11.0pt; mso-bidi-font-size: 14.0pt; line-height: 110%'>Như kính g&#7917;i;<o:p></o:p></span></i>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 31.5pt; text-indent: -18.0pt; line-height: 110%; mso-list: l0 level1 lfo4'>
                                        <![if !supportLists]><span lang="EN-US"
                                            style='font-size: 14.0pt; line-height: 110%; font-family: "Courier New"; mso-fareast-font-family: "Courier New"'><span style='mso-list: Ignore'>&#821;<span
                                                style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span></span><![endif]><i
                                                    style='mso-bidi-font-style: normal'><span lang="EN-US" style='font-size: 11.0pt; mso-bidi-font-size: 14.0pt; line-height: 110%'>Lưu VT;</span></i><i
                                                        style='mso-bidi-font-style: normal'><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><o:p></o:p></span></i>
                                    </p>
                                </td>
                                <td valign="top" style='width: 231.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <b style='mso-bidi-font-weight: normal'><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>
                                                <o:p>&nbsp;</o:p>
                                            </span></b>
                                    </p>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <b
                                            style='mso-bidi-font-weight: normal'><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Ngư&#7901;i g&#7917;i yêu c&#7847;u<o:p></o:p></span></b>
                                    </p>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <i
                                            style='mso-bidi-font-style: normal'><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>(K&yacute;, ghi r&otilde; h&#7885; tên)</span></i><i
                                                style='mso-bidi-font-style: normal'><span style='font-size: 14.0pt; line-height: 110%; mso-ansi-language: VI'><o:p></o:p></span></i>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class=MsoNormal style='line-height:110%'><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <p class=MsoNormal style='line-height:110%'><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <p class=MsoNormal style='line-height:110%'><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <p class=MsoNormal style='line-height:110%'><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left:18.0pt;line-height:110%'><i
style='mso-bidi-font-style:normal'><span lang=EN-US style='font-size:9.0pt;
mso-bidi-font-size:12.0pt;line-height:110%'>Chú thích:<o:p></o:p></span></i></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left:18.0pt;line-height:110%'><i
style='mso-bidi-font-style:normal'><span lang=EN-US style='font-size:9.0pt;
mso-bidi-font-size:12.0pt;line-height:110%'>(1): Tên đơn v&#7883; nh&#7853;n
yêu c&#7847;u chuy&#7875;n ti&#7871;p.<o:p></o:p></span></i></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left:18.0pt;line-height:110%'><i
style='mso-bidi-font-style:normal'><span lang=EN-US style='font-size:9.0pt;
mso-bidi-font-size:12.0pt;line-height:110%'>(2): Tên đơn v&#7883; g&#7917;i yêu
c&#7847;u chuy&#7875;n ti&#7871;p.<o:p></o:p></span></i></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i
style='mso-bidi-font-style:normal'><span lang=EN-US style='font-size:9.0pt;
mso-bidi-font-size:12.0pt;line-height:110%'>(3): Tên khách hàng.<o:p></o:p></span></i></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i
style='mso-bidi-font-style:normal'><span lang=EN-US style='font-size:9.0pt;
mso-bidi-font-size:12.0pt;line-height:110%'>(4): V&#7845;n đ&#7873; khách hàng
khi&#7871;u n&#7841;i.<o:p></o:p></span></i></p>
                    </td>
                </tr>
            </table>
        </div>














        <center>
        <a href="#" class="print" rel="baocao">Print</a>
        </center>
        <script src="/JS/jquery-1.7.2.min.js" type="text/javascript"></script>

        <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>
        <script src="printCore.js"></script>
        <br />
        <br />
    </form>
</body>
</html>
