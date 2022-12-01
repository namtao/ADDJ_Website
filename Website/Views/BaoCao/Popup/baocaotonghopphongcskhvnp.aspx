<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopphongcskhvnp.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopphongcskhvnp" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .reportFont { margin: 10px 0px; }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <asp:Button runat="server" ID="btnExportExcel" Text="Xuất Excel" />
    <div runat="server" id="baocao" class="reportFont">
        <div id="reportContainer">
            <table border="0" style="width: 100%">
                <tr>
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="15">
                        <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                        <br />
                        <span style="text-decoration: underline;">Độc lập - Tự do - Hạnh phúc
                        </span>

                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="15">&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="15">
                        <h1>BÁO CÁO TỔNG HỢP SỐ LIỆU PAKN</h1>
                        <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td colspan="15">&nbsp;</td>
                </tr>
            </table>
            <asp:Literal runat="server" ID="liReport"></asp:Literal>
            <table border="0" width="100%" style="margin-top: 20px">
                <tr>
                    <td colspan="15"></td>
                </tr>
                <tr valign="top">
                    <td style="text-align: center; font-weight: bold; border: 0px" colspan="15">Người báo cáo
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblFullName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="15">

                        <b><i>Giải thích</i></b>
                        <br />
                        [1] : Số lượng khiếu nại tồn tính đến trước ngày thực hiện báo cáo
            <br />
                        [2] : Số lượng khiếu nại tiếp nhận (khiếu nại từ nơi khác chuyển đến + tạo mới) ngoại trừ các khiếu nại tồn đọng từ kỳ trước
            <br />
                        [2.1] : Số lượng khiếu nại do người dùng tạo ra
            <br />
                        [3] = [3.1] + [3.2] + [3.3] : Tổng số khiếu nại đã được xử lý (chuyển ngang hàng/chuyển xử lý/chuyển phản hồi/đóng khiếu nại)            
            <br />
                        [3.1] : Số lượng khiếu nại đã được chuyển xử lý
            <br />
                        [3.2] : Số lượng khiếu nại đã được chuyển chuyển phản hồi
            <br />
                        [3.3] : Số lượng khiếu nại người dùng đã đóng
            <br />
                        [3.4] : Số lượng khiếu nại đã được xử lý nhưng quá hạn phòng ban quy định
            <br />
                        [4] : Số lượng khiếu nại tồn tính tới thời điểm cuối cùng thực hiện báo cáo
            <br />
                        [4.1] : Số lượng khiếu nại tồn quá hạn tính tới thời điểm cuối cùng thực hiện báo cáo
                       
                    </td>
                </tr>
            </table>
            <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>
            <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>
        </div>
    </div>
    <asp:Button runat="server" ID="btnExportExcel2" Text="Xuất Excel" />
</asp:Content>
