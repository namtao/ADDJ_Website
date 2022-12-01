<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuTraLoi.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuTraLoi" %>

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
lang=EN-GB style='font-size:14.0pt;line-height:110%'>&nbsp;</span></b></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:72.0pt;text-align:justify;line-height:
110%'><span lang=EN-GB style='font-size:14.0pt;line-height:110%'>Kính
g&#7917;i:.........<span lang=EN-US style='font-size:14.0pt;line-height:110%'><asp:Literal ID="ltTenKH" runat="server"></asp:Literal></span>................................................... <sup>(1)</sup></span></p>
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
                     <p class=MsoNormal style='text-align:justify;text-indent:36.0pt;line-height:
110%'><sup><span lang=EN-US style='font-size:14.0pt;line-height:110%'>&nbsp;</span></sup></p>
                 </td>
             </tr>
             <tr>
       <td>
           <p class=MsoNormal style='text-align:justify;text-indent:36.0pt;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>............................ <sup>(2)</sup></span><span
style='font-size:14.0pt;line-height:110%'>&nbsp;</span><span style='font-size:
14.0pt;line-height:110%'> <span lang=EN-US>trân tr&#7885;ng kính chào
qu&yacute; khách hàng và chân thành c&#7843;m ơn Qu&yacute; khách hàng
đ&atilde; l&#7921;a ch&#7885;n và s&#7917; d&#7909;ng d&#7883;ch v&#7909; do
chúng tôi cung c&#7845;p trong th&#7901;i gian qua.</span></span></p>
       </td>
   </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='text-align:justify;text-indent:36.0pt'><span
lang=EN-US style='font-size:14.0pt'>Ngày <asp:Literal runat="server" ID="LBL_NGAYTIEPNHAN"></asp:Literal>&nbsp;/&nbsp;<asp:Literal runat="server" ID="LBL_THANGTIEPNHAN"></asp:Literal>&nbsp;/&nbsp;<asp:Literal runat="server" ID="LBL_NAMTIEPNHAN"></asp:Literal>, chúng tôi
đ&atilde; nh&#7853;n đư&#7907;c khi&#7871;u n&#7841;i c&#7911;a Qu&yacute;
khách hàng </span><sup><span lang=EN-US style='font-size:14.0pt'> </span></sup><span
lang=EN-US style='font-size:14.0pt'>v&#7873; vi&#7879;c <sup>(3)</sup>....................................................................................................................  </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='text-align:justify'><span lang=EN-US
style='font-size:14.0pt'>          Qua th&#7901;i gian xem xét và ph&#7889;i
h&#7907;p v&#7899;i các đơn v&#7883; có liên quan xác đ&#7883;nh nguyên nhân,
chúng tôi xin đư&#7907;c tr&#7843; l&#7901;i khi&#7871;u n&#7841;i c&#7911;a
khách hàng như sau:</span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:54.0pt;text-indent:-18.0pt'><span
lang=EN-US style='font-size:14.0pt'>1.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span lang=EN-US style='font-size:14.0pt'>Nguyên nhân phát sinh
khi&#7871;u n&#7841;i:</span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>.................................................................................................................. </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>.................................................................................................................. </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:54.0pt;text-indent:-18.0pt;line-height:
110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%'>2.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>K&#7871;t lu&#7853;n:</span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>.................................................................................................................. </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>.................................................................................................................. </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='text-indent:36.0pt'><span lang=EN-US
style='font-size:14.0pt'>&nbsp;</span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='text-align:justify;text-indent:36.0pt'><span
lang=EN-US style='font-size:14.0pt'>Chúng tôi xin ghi nh&#7853;n các &yacute;
ki&#7871;n và trân tr&#7885;ng cám ơn Qu&yacute; khách đ&atilde; đóng góp xây
d&#7921;ng đ&#7875; chúng tôi không ng&#7915;ng nâng cao ch&#7845;t lư&#7907;ng
d&#7883;ch v&#7909;, ph&#7909;c v&#7909; khách hàng ngày m&#7897;t t&#7889;t
hơn.</span></p>

                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='text-align:justify;text-indent:36.0pt'><span
lang=EN-US style='font-size:14.0pt'>Chúng tôi r&#7845;t mong ti&#7871;p
t&#7909;c nh&#7853;n đư&#7907;c s&#7921; quan tâm, úng h&#7897; c&#7911;a
Qu&yacute; khách và hân h&#7841;nh đư&#7907;c ti&#7871;p t&#7909;c ph&#7909;c
v&#7909; Qu&yacute; khách trong th&#7901;i gian t&#7899;i. M&#7885;i thông tin
xin liên h&#7879; <sup>(4)</sup>        </span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal><span lang=EN-US style='font-size:14.0pt'>          Trân
tr&#7885;ng kính chào./.</span></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
 style='border-collapse:collapse'>
 <tr>
  <td width=308 valign=top style='width:231.05pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal style='line-height:110%'><i><span lang=EN-US
  style='font-size:14.0pt;line-height:110%'>&nbsp;</span></i></p>
  <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><b><i><span
  lang=EN-US style='font-size:11.0pt;line-height:110%'>Nơi nh&#7853;n</span></i></b></p>
  <p class=MsoNormal style='margin-left:31.5pt;text-indent:-18.0pt;line-height:
  110%'><span lang=EN-US style='font-size:11.0pt;line-height:110%;font-family:
  "Courier New"'>&#821;<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><i><span
  lang=EN-US style='font-size:11.0pt;line-height:110%'>Như kính g&#7917;i;</span></i></p>
  <p class=MsoNormal style='margin-left:31.5pt;text-indent:-18.0pt;line-height:
  110%'><span lang=EN-US style='font-size:14.0pt;line-height:110%;font-family:
  "Courier New"'>&#821;<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><i><span
  lang=EN-US style='font-size:11.0pt;line-height:110%'>Lưu VT;</span></i></p>
  </td>
  <td width=308 valign=top style='width:231.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>&nbsp;</span></b></p>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>Đ&#7840;I DI&#7878;N</span></b></p>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>ĐƠN V&#7882; GI&#7842;I
  QUY&#7870;T KHI&#7870;U N&#7840;I</span></b></p>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><i><span
  lang=EN-US style='font-size:14.0pt;line-height:110%'>(K&yacute; ,đóng
  d&#7845;u và ghi r&otilde; h&#7885; tên)</span></i></p>
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
                     <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>Chú thích:</span></i></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(1): Tên khách hàng.</span></i></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(2): Tên đơn v&#7883; gi&#7843;i
quy&#7871;t khi&#7871;u n&#7841;i.</span></i></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(3): V&#7845;n đ&#7873;
khách hàng khi&#7871;u n&#7841;i.</span></i></p>
                 </td>
             </tr>
             <tr>
                 <td>
                     <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
lang=EN-US style='font-size:9.0pt;line-height:110%'>(4): Thông tin c&#7911;a
b&#7897; ph&#7853;n gi&#7843;i quy&#7871;t khi&#7871;u n&#7841;i</span></i></p>
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
