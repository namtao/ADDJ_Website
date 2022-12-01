<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuXacMinh2.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.PhieuXacMinh2" %>

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
            font-size: 14pt;
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
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 800px; margin: 10px auto;" id="baocao">
            <table border="0" style="width: 100%">
                <tr>
                    <td style="font-size: 12pt; font-weight: bold; text-align: center; width: 50%">ĐƠN VỊ CHỦ QUẢN</td>
                    <td style="font-weight: bold; text-align: center">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</td>
                </tr>
                <tr>
                    <td style="font-size: 12pt; text-align: center; width: 50%;">Đơn vị chủ trì GQ khiếu nại
                        <center>
                        <hr style="text-align: center; border:0px; width: 25%; border-top-width: 1px; border-style: solid;" />
                            </center>
                    </td>
                    <td style="font-weight: bold; text-align: center">Độc lập - Tự do - Hạnh phúc
                        <center>
                        <hr style="text-align: center; width: 25%; border-top-width: 1px; border-style: solid;" />
                            </center>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 12pt; text-align: center; width: 50%">Số:.............(2)</td>
                    <td style="font-weight: bold; text-align: center"></td>
                </tr>
                <tr>
                    <td style="font-size: 12pt; text-align: center; width: 50%"></td>
                    <td style="font-style: italic; text-align: right">............,ngày......tháng......năm........</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold; text-align: center" colspan="2">PHIẾU YÊU CẦU XÁC MINH KHIẾU NẠI
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>               
                <tr>
                    <td style="padding-left: 100pt" colspan="2">Kính gửi:............................................(1)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>               
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">(2).....................................nhận được khiếu nại của khách hàng................(3)
                        <br />
                        về vấn đề Nhà số 3 - đường Trần Hưng Nhượng - Yên Bình - Hưng Phúc - Thành Phố Vinh - Nghệ An* 2 năm* KH p/a sóng indoor(trong nhà ở tầng 1) kém, chập chờn, thường xuyên bị ngắt cuộc, có khi mất hẳn sóng. Trên máy có 1 -> 2 vạch sóng. Di chuyển ra ngoài trời hoặc lên tầng 2 tình trạng có tốt hơn... Khách hàng đang dùng máy nokia không có máy khác để thay sang. KH đang đứng ở vị trí pa gọi lên TĐ tín hiệu bình thường. Các thuê bao cùng KV bị hiện tượng như vậy: 915358785; 916338554 ...* LH: anh Nghĩa 917864999*Cell id: tb post
                        
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">Để có đầy đủ cơ sở trả lời khiếu nại khách hàng, yêu cầu................(1)xác minh và cung cấp những thông tin về các vấn đề sau:.............................................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">1 .................................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">2 .................................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">3 .................................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">Mọi thông tin phản hồi xin liên hệ:
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">Đơn vị/ Bộ phận:.....................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt; width: 50%" >Số điện thoại:.............................

                    </td>
                    <td>Fax:......................</td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">Email:.....................
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 50pt;" colspan="2">Trân trọng cảm ơn.
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-size: 11pt; text-indent:30pt; font-weight: bold; font-style: italic">Nơi nhận
                        <br />
                        <span style="font-size: 11pt; padding-left:20pt; font-weight: normal;  font-style: italic">- Như kính gửi;
                            </span>
                        <br /><span style="font-size: 11pt; padding-left:20pt; font-weight: normal;  font-style: italic">- Lưu VT;
                            </span>
                    </td>
                    <td style="font-weight: bold; text-align: center; ">
                        Người gửi yêu cầu
                        <br />
                        <span style="font-style: italic; font-weight: normal; text-align: center">
                            (Ký, ghi rõ họ tên)
                        </span>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="font-size: 9pt; padding-left:20pt; font-style: italic">
                        Chú thích:
                        <br />
                        (1): Tên đơn vị nhận yêu cầu xác minh.
                        <br />
                        (2): Tên đơn vị nhận yêu cầu xác minh.
                        <br />
                        (3): Tên đơn vị nhận yêu cầu xác minh.
                        <br />
                        (4): Tên đơn vị nhận yêu cầu xác minh.
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
