<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuXacMinh.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuXacMinh" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        -->
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 483.5pt; margin: 10px auto;" id="baocao">

            <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0" width="645"
                style='width: 483.5pt; border-collapse: collapse'>
                <tr>
                    <td width="223" valign="top" style='width: 167.4pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b><span
                                lang="EN-GB" style='line-height: 110%'>ĐƠN V&#7882; CH&#7910; QU&#7842;N</span></b>
                        </p>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <span
                                lang="EN-GB" style='line-height: 110%'><u>
                                    <asp:Literal ID="ltDonViTiepNhan" runat="server"></asp:Literal></u> </span><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>
                                        <br>
                                    </span><span lang="EN-GB" style='line-height: 110%'>S&#7889;: </span><span
                                        lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>................<sup>(2)</sup></span>
                        </p>
                    </td>
                    <td width="421" valign="top" style='width: 316.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='margin-left: 3.6pt; text-align: center; line-height: 110%'>
                            <b><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>C&#7896;NG H&Ograve;A X&Atilde; H&#7896;I CH&#7910; NGH&#296;A
  VI&#7878;T NAM<br>
                                Đ&#7897;c <u>l&#7853;p - T&#7921; do - H&#7841;nh</u> phúc</span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td width="223" valign="top" style='width: 167.4pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height: 110%'>
                            <span lang="EN-GB"
                                style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span>
                        </p>
                    </td>
                    <td width="421" valign="top" style='width: 316.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="right" style='margin-right: 7.0pt; text-align: right; line-height: 110%'>
                            <i><span lang="EN-GB" style='line-height: 110%'>............,ngày.<asp:Literal ID="ltNgay" runat="server"></asp:Literal>.tháng.<asp:Literal ID="ltThang" runat="server"></asp:Literal>.năm.<asp:Literal ID="ltNam" runat="server"></asp:Literal>.</span></i>
                        </p>
                    </td>
                </tr>
            </table>
            <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0" width="645px" style='width: 483.5pt; border-collapse: collapse'>
                <tr>
                    <td>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <span
                                lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b><span
                                lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>PHI&#7870;U YÊU C&#7846;U
XÁC MINH KHI&#7870;U N&#7840;I</span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b><span
                                lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                            <b><span
                                lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></b>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 72.0pt; text-align: justify; line-height: 110%'>
                            <span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>Kính
g&#7917;i:.......................................................................................... <sup>(1)</sup></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 72.0pt; text-align: justify; line-height: 110%'>
                            <sup><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></sup>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 72.0pt; text-align: justify; line-height: 110%'>
                            <sup><span lang="EN-GB" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></sup>
                        </p>

                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 20.0pt; text-align: justify; line-height: 110%'>
                            <sup><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>(2)</span></sup><span
                                style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span><span lang="EN-US"
                                    style='font-size: 14.0pt; line-height: 110%'> ........................ nhận được khiếu nại của khách hàng
                                    <asp:Literal ID="ltTenKH" runat="server"></asp:Literal>
                                    <sup>(3)</sup></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='text-align: justify; line-height: 110%'>
                            <sup><span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'></span></sup><span
                                    lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>v&#7873; v&#7845;n
đ&#7873; ........................................................................................................... <sup>(4)</sup></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='text-indent: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Đ&#7875; có đ&#7847;y
đ&#7911; cơ s&#7903; tr&#7843; l&#7901;i khi&#7871;u n&#7841;i khách hàng, yêu
c&#7847;u ……………..<sup>(1) </sup>xác minh và cung c&#7845;p nh&#7919;ng thông
tin v&#7873; các v&#7845;n đ&#7873; sau:</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>1 ................................................................................................................... </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>2 ................................................................................................................... </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>3 ................................................................................................................... </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>M&#7885;i thông tin
ph&#7843;n h&#7891;i xin liên h&#7879;:</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Đơn v&#7883;/ B&#7897;
ph&#7853;n: ........................................................................................... </span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>S&#7889; đi&#7879;n
tho&#7841;i: ..............................      Fax: …………………….......................</span>
                        </p>

                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Email: ............................................................................................................ </span>
                        </p>

                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 36.0pt; line-height: 110%'>
                            <span
                                lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Trân tr&#7885;ng c&#7843;m
ơn.</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0"
                            style='border-collapse: collapse'>
                            <tr>
                                <td width="308" valign="top" style='width: 231.05pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                                    <p class="MsoNormal" align="center" style='margin-left: 18.0pt; text-align: center; line-height: 110%'>
                                        <i><span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></i>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                                        <b><i><span
                                            lang="EN-US" style='font-size: 11.0pt; line-height: 110%'>Nơi nh&#7853;n</span></i></b>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 31.5pt; text-indent: -18.0pt; line-height: 110%'>
                                        <span lang="EN-US" style='font-size: 11.0pt; line-height: 110%; font-family: "Courier New"'>&#821;<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><i><span
                                            lang="EN-US" style='font-size: 11.0pt; line-height: 110%'>Như kính g&#7917;i;</span></i>
                                    </p>
                                    <p class="MsoNormal" style='margin-left: 31.5pt; text-indent: -18.0pt; line-height: 110%'>
                                        <span lang="EN-US" style='font-size: 14.0pt; line-height: 110%; font-family: "Courier New"'>&#821;<span style='font: 7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><i><span
                                            lang="EN-US" style='font-size: 11.0pt; line-height: 110%'>Lưu VT;</span></i>
                                    </p>
                                </td>
                                <td width="308" valign="top" style='width: 231.1pt; padding: 0cm 5.4pt 0cm 5.4pt'>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <b><span
                                            lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>&nbsp;</span></b>
                                    </p>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <b><span
                                            lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>Ngư&#7901;i g&#7917;i
  yêu c&#7847;u</span></b>
                                    </p>
                                    <p class="MsoNormal" align="center" style='text-align: center; line-height: 110%'>
                                        <i><span
                                            lang="EN-US" style='font-size: 14.0pt; line-height: 110%'>(K&yacute;, ghi
  r&otilde; h&#7885; tên)</span></i>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%'><span lang="EN-US">&nbsp;</span></p>
                    </td>

                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%'><span lang="EN-US">&nbsp;</span></p>
                    </td>

                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%'><span lang="EN-US">&nbsp;</span></p>
                    </td>

                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='line-height: 110%'><span lang="EN-US">&nbsp;</span></p>
                    </td>

                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                            <i><span
                                lang="EN-US" style='font-size: 9.0pt; line-height: 110%'>Chú thích:</span></i>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                            <i><span
                                lang="EN-US" style='font-size: 9.0pt; line-height: 110%'>(1): Tên đơn v&#7883;
nh&#7853;n yêu c&#7847;u xác minh.</span></i>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                          <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                        <i><span
                            lang="EN-US" style='font-size: 9.0pt; line-height: 110%'>(2): Tên đơn v&#7883; g&#7917;i
yêu c&#7847;u xác minh.</span></i>
                    </p>
                    </td>
                  
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                            <i><span
                                lang="EN-US" style='font-size: 9.0pt; line-height: 110%'>(3): Tên khách hàng.</span></i>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p class="MsoNormal" style='margin-left: 18.0pt; line-height: 110%'>
                            <i><span
                                lang="EN-US" style='font-size: 9.0pt; line-height: 110%'>(4): V&#7845;n đ&#7873;
khách hàng khi&#7871;u n&#7841;i.</span></i>
                        </p>
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
