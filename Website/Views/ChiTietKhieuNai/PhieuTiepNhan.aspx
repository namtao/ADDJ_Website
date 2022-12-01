<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuTiepNhan.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuTiepNhan" %>

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

          
              <table class="MsoNormalTable" border="0" cellspacing="0" cellpadding="0" width="645"
                style='width: 483.5pt; border-collapse: collapse'>
                  <tr>
                      <td>
                          <p class=MsoNormal align=center style='text-align:center;line-height:110%'><span
lang=EN-GB style='font-size:14.0pt;line-height:110%'>&nbsp;</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
lang=EN-GB style='font-size:14.0pt;line-height:110%'>PHI&#7870;U TI&#7870;P
NH&#7852;N KHI&#7870;U N&#7840;I</span></b></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
lang=EN-GB style='font-size:14.0pt;line-height:110%'>&nbsp;</span></b></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:72.0pt;text-align:justify;line-height:
110%'><span lang=EN-GB style='font-size:14.0pt;line-height:110%'>Kính g&#7917;i:.................................................................... <sup>(1)</sup></span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:72.0pt;text-align:justify;line-height:
110%'><sup><span lang=EN-GB style='font-size:14.0pt;line-height:110%'>&nbsp;</span></sup></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>Ngày…..     tháng…..     năm…..
, <sup>(2)</sup></span><span style='font-size:14.0pt;line-height:110%'>&nbsp;</span><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>……………………………………….</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>nh&#7853;n
đư&#7907;c khi&#7871;u n&#7841;i c&#7911;a....<asp:Literal ID="ltTenKH" runat="server"></asp:Literal>............................... <sup>(1)</sup></span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                        <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>v&#7873;
v&#7845;n đ&#7873; ........................................................................................... <sup>(3)
</sup>và </span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>yêu c&#7847;u .................................................................................................. <sup>(4)</sup></span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>          Chúng
tôi đang xác minh và s&#7869; có k&#7871;t lu&#7853;n tr&#7843; l&#7901;i
Qu&yacute; khách hàng trong th&#7901;i gian s&#7899;m nh&#7845;t.</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>          N&#7871;u
có thông tin liên quan đ&#7871;n khi&#7871;u n&#7841;i, đ&#7873; ngh&#7883;
jhách hàng liên h&#7879; v&#7899;i chúng tôi theo s&#7889; đi&#7879;n
tho&#7841;i …………..ho&#7863;c email………………………</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>          Chúng
tôi r&#7845;t mong nh&#7853;n đư&#7907;c s&#7921; h&#7895; tr&#7907; và thông
c&#7843;m c&#7911;a Qu&yacute; khách hàng.</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>          Trân tr&#7885;ng
kính chào.</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><sup><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>&nbsp;</span></sup></p>

                      </td>
                  </tr>
                  <tr>
                      <td>
                          <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
 style='border-collapse:collapse'>
 <tr>
  <td width=308 valign=top style='width:231.05pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal align=center style='margin-left:18.0pt;text-align:center;
  line-height:110%'><i><span lang=EN-US style='font-size:14.0pt;line-height:
  110%'>&nbsp;</span></i></p>
  </td>
  <td width=308 valign=top style='width:231.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>Đ&#7840;I DI&#7878;N ĐƠN
  V&#7882;</span></b></p>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>(K&yacute; tên, đóng
  d&#7845;u)</span></p>
  </td>
 </tr>
</table>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-US>&nbsp;</span></p>
                      </td>
                  </tr>
                   <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-US>&nbsp;</span></p>
                      </td>
                  </tr>
                   <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-US>&nbsp;</span></p>
                      </td>
                  </tr>
                   <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-US>&nbsp;</span></p>
                      </td>
                  </tr>
                   <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-US>&nbsp;</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>Chú thích:</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(1): Tên khách hàng
khi&#7871;u n&#7841;i, n&#7871;u là t&#7893; ch&#7913;c th&igrave; ghi
đ&#7847;y đ&#7911; tên c&#7911;a t&#7893; ch&#7913;c, n&#7871;u là cá nhân
th&igrave; ph&#7843;i ghi ÔNG/BÀ trư&#7899;c tên khách hàng.</span></i></p>

                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(2): Tên đơn v&#7883;
g&#7917;i phi&#7871;u ti&#7871;p nh&#7853;n khi&#7871;u n&#7841;i</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(3): Trích n&#7897;i dung
mà khách hàng khi&#7871;u n&#7841;i</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(4): Yêu c&#7847;u
gi&#7843;i quy&#7871;t c&#7911;a khách hàng</span></i></p>
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
