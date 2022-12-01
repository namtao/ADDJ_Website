<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopchatluongmang.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopchatluongmang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="baocao" class="reportFont">
        <div id="reportContainer">
            <table border="0" width="100%">
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblTime" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="3">&nbsp;
                    </td>
                    <td></td>
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="2">
                        <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                        <br />
                        <span style="text-decoration: underline;">Độc lập - Tự do - Hạnh phúc
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
            </table>

            <div style="text-align: center">
                <h1>BÁO CÁO TỔNG HỢP CHẤT LƯỢNG MẠNG        
                </h1>

                <table border="0" width="100%">
                    <tr>
                        <td colspan="5" style="text-align: center">
                            <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>

            <br />

            <table class="tbl_style" border="1" style="border-collapse: collapse;">                
                <%=sNoiDungBaoCao %>
            </table>

            <table border="0" width="100%">
                <tr>
                    <td style="border: 0px;">&nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td style="border: 0px; text-align: center; font-weight: bold" colspan="2"></td>
                    <td style="text-align: center; font-weight: bold; border: 0px" colspan="3">Người báo cáo
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="height: 70px">
                        [2.1], [3.1]: Không xác định<br />
                        [2.2], [3.2]: Sóng kém hoặc chập chờn (cả indoor và outdoor)<br />
                        [2.3], [3.3]: Có sóng nhưng gọi đi hoặc gọi đến không được<br />
                        [2.4], [3.4]: Đang đàm thoại rớt cuộc ( mất tín hiệu, báo gián đoạn..)<br />
                        [2.5], [3.5]: Cuộc gọi nhiễu,nghe xen, tiếng vọng<br />
                        [2.6], [3.6]: Không có sóng Indoor<br />
                        [2.7], [3.7]: Mất sóng hoàn toàn<br /><br />

                        [2.x.1], [3.x.1]: Số lượng tiếp nhận<br />
                        [2.x.2], [3.x.2]: Số lượng tồn<br />
                        [2.x.3], [3.x.3]: Số lượng quá hạn<br />
                    </td>
                </tr>
                <tr>
                    <td style="border: 0px; text-align: center; font-weight: bold" colspan="2"></td>
                    <td style="text-align: center; font-weight: normal; border: 0px" colspan="3">
                         <asp:Label ID="lblFullName" runat="server"></asp:Label>
                    </td>                    
                </tr>
            </table>

            <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

            <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

            <script type="text/javascript">
                $(document).ready(function () {
                    
                });
                function GetListKhieuNaiId(val)
                {
                    $.ajax({
                        type: "POST",
                        url: "danhsachkhieunai.aspx/GetListKhieuNaiId",
                        data:'{lstKhieuNaiId:"'+val+'"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                           var dskhieunai =  window.open(url = '/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage=baocaotonghopchatluongmangactivity&isActivity=1', '_blank', 'width=980, height=550,scrollbars=1,location=0');
                           dskhieunai.onload = function () {
                               //dskhieunai.document.getElementById("noiDungBaoCao").innerHTML = msg.d;
                               dskhieunai.focus();
                           }
                        }
                    })
                }
            </script>
        </div>
    </div>
</asp:Content>
