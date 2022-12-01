<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuKhieuNai.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuKhieuNai" %>

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
lang=EN-GB style='font-size:14.0pt;line-height:110%'>PHI&#7870;U KHI&#7870;U
N&#7840;I</span></b></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;text-indent:
-18.0pt;line-height:110%'><span style='font-size:14.0pt;line-height:110%'>1.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>B&#7897; ph&#7853;n
ti&#7871;p nh&#7853;n khi&#7871;u n&#7841;i: …………………………………………………...</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;text-indent:
-18.0pt;line-height:110%'><span style='font-size:14.0pt;line-height:110%'>2.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
style='font-size:14.0pt;line-height:110%'>H&#7885; tên ngư&#7901;i khi&#7871;u
n&#7841;i<sup>(3)</sup>:…<span
                        style='font-size: 14.0pt; line-height: 110%'><asp:Literal ID="ltNguoiKN" runat="server"></asp:Literal></span>
                              …...……………………..</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;line-height:
110%'><span style='font-size:14.0pt;line-height:110%'>CMND s&#7889;………….
C&#7845;p ngày……/……/……. Nơi c&#7845;p…………………...…</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;text-indent:
-18.0pt;line-height:110%'><span style='font-size:14.0pt;line-height:110%'>3.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
style='font-size:14.0pt;line-height:110%'>Đ&#7883;a ch&#7881;/s&#7889;
đi&#7879;n tho&#7841;i liên h&#7879;:…………………………………………………....</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;text-indent:
-18.0pt;line-height:110%'><span style='font-size:14.0pt;line-height:110%'>4.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
style='font-size:14.0pt;line-height:110%'>Loại dịch vụ khiếu nại: …………….Giờ nhận khiếu nại: ………giờ…….......</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-align:justify;text-indent:
-18.0pt;line-height:110%'><span style='font-size:14.0pt;line-height:110%'>5.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>N&#7897;i dung khi&#7871;u
n&#7841;i</span></p>
                      </td>
                  </tr>
                     <tr>
                         <td>
                                  <p class="MsoNormal" style='margin-left: 36.0pt; text-align: justify; line-height: 110%'>
                <span lang="EN-US" style='font-size: 14.0pt; line-height: 110%'><span>
                    <asp:Literal ID="ltNoiDung" runat="server"></asp:Literal>
                </span></span>
            </p>
                         </td>
                    

                    </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-indent:-18.0pt;line-height:
110%'><span style='font-size:14.0pt;line-height:110%'>6.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span lang=EN-US style='font-size:14.0pt;line-height:110%'>H&igrave;nh
th&#7913;c ti&#7871;p nh&#7853;n khi&#7871;u n&#7841;i:</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          
                <table style="margin-left:36.0pt; width: 100%;">
                    <tr>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Điện thoại <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span></td>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Văn bản<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span></td>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Email <span>&nbsp;&nbsp;&nbsp;&nbsp; </span></span></td>

                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                        <td style="border: 1pt solid #333" width="20px"></td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Hình thức khác</span></td>
                    </tr>
                    <tr>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Trực tiếp<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span></td>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td><span style="font-size: 14pt; line-height: 110%;">Website<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span></td>
                        <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                        <td colspan="4"><span style="font-size: 14pt; line-height: 110%;">Phương tiện thông tin đại chúng</span></td>
                    </tr>
                </table>

                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-indent:-18.0pt;line-height:
110%'><span style='font-size:14.0pt;line-height:110%'>7.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span style='font-size:14.0pt;line-height:110%'>Các gi&#7845;y
t&#7901;, ch&#7913;ng t&#7915; liên quan:</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          
                    <table style=" width:100%;margin-left: 36pt; ">
                        <tr>
                            <td style="border: 1pt solid #333; width: 20px;">&nbsp;</td>
                            <td><span lang="EN-US" style="font-size: 14pt; line-height: 110%;">Bản in chi tiết cước tháng…<span> </span></span></td>
                            <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                            <td><span lang="EN-US" style="font-size: 14pt; line-height: 110%;">Hóa đơn thu tiền cướctháng……</span></td>
                        </tr>
                        <tr>
                            <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                            <td><span lang="EN-US" style="font-size: 14pt; line-height: 110%;">Đơn từ, công văn khiếu nại </span></td>
                            <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                            <td><span lang="EN-US" style="font-size: 14pt; line-height: 110%;">Nội dung Email/Website khiếu nại(bản in)</span></td>
                        </tr>
                        <tr>
                            <td style="border: 1pt solid #333" width="20px">&nbsp;</td>
                            <td colspan="3"><span lang="EN-US" style="font-size: 14pt; line-height: 110%;">Tài liệu khác (kèm theo):</span></td>
                        </tr>
                    </table>
                 
                
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;text-indent:-18.0pt;line-height:
110%'><span style='font-size:14.0pt;line-height:110%'>8.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span lang=EN-US style='font-size:14.0pt;line-height:110%'>Gi&#7843;i
thích c&#7911;a nhân viên ti&#7871;p nh&#7853;n (n&#7871;u có):</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>...................................................................................................................... </span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:36.0pt;line-height:110%'><span
lang=EN-US style='font-size:14.0pt;line-height:110%'>...................................................................................................................... </span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span lang=EN-GB style='font-size:
14.0pt;line-height:110%'>                             </span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'><span style='font-size:14.0pt;
line-height:110%'>&nbsp;</span></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
 style='border-collapse:collapse'>
 <tr>
  <td width=308 valign=top style='width:231.05pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal align=center style='margin-left:18.0pt;text-align:center;
  line-height:110%'><b><span style='font-size:14.0pt;line-height:110%'>Ngư&#7901;i
  ti&#7871;p nh&#7853;n khi&#7871;u n&#7841;i</span></b></p>
  <p class=MsoNormal align=center style='margin-left:18.0pt;text-align:center;
  line-height:110%'><i><span style='line-height:110%'>(K&yacute; và ghi
  r&otilde; h&#7885; tên)</span></i></p>
  </td>
  <td width=308 valign=top style='width:231.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><b><span
  style='font-size:14.0pt;line-height:110%'>Ngư&#7901;i khi&#7871;u n&#7841;i</span></b></p>
  <p class=MsoNormal align=center style='text-align:center;line-height:110%'><i><span
  style='line-height:110%'>(K&yacute; và ghi r&otilde; h&#7885; tên)</span></i></p>
  </td>
 </tr>
</table>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'>&nbsp;</p>
                      </td>
                  </tr>
                      <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'>&nbsp;</p>
                      </td>
                  </tr>
                      <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'>&nbsp;</p>
                      </td>
                  </tr>
                      <tr>
                      <td>
                          <p class=MsoNormal style='line-height:110%'>&nbsp;</p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;text-align:justify;text-justify:
distribute-all-lines;line-height:110%'><i>Ghi chú: Trư&#7901;ng h&#7907;p
khi&#7871;u n&#7841;i tr&#7921;c ti&#7871;p t&#7841;i Giao d&#7883;ch, nhân
viên ti&#7871;p nh&#7853;n Photocopy giao l&#7841;i cho khách hàng 01 b&#7843;n
thay th&#7871; cho phi&#7871;u ti&#7871;p nh&#7853;n khi&#7871;u n&#7841;i</i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i>&nbsp;</i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i>&nbsp;</i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
style='font-size:9.0pt;line-height:110%'>Chú thích:</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
style='font-size:9.0pt;line-height:110%'>(1): tên đơn v&#7883; ch&#7911;
tr&igrave; ho&#7863;c b&#7897; ph&#7853;n gi&#7843;i quy&#7871;t khi&#7871;u
n&#7841;i</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
style='font-size:9.0pt;line-height:110%'>(2): M&atilde; s&#7889; phi&#7871;u
cung c&#7845;p cho khách hàng đ&#7875; theo d&otilde;i gi&#7843;i quy&#7871;t
khi&#7871;u n&#7841;i trên m&#7841;ng</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
style='font-size:9.0pt;line-height:110%'>(3): Ngư&#7901;i khi&#7871;u n&#7841;i
có th&#7875; là ngư&#7901;i đ&#7841;i di&#7879;n h&#7907;p pháp cho khách hàng</span></i></p>
                      </td>
                  </tr>
                  <tr>
                      <td>
                          <p class=MsoNormal style='margin-left:18.0pt;line-height:110%'><i><span
style='font-size:9.0pt;line-height:110%'>(4): Đ&#7883;a ch&#7881; c&#7911;a
B&#7897; ph&#7853;n gi&#7843;i quy&#7871;t khi&#7871;u n&#7841;i bao g&#7891;m
c&#7843; thông tin đ&#7883;a ch&#7881;, đi&#7879;n tho&#7841;i, Email..liên
h&#7879;</span></i></p>
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
