<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopkhieunaitoanmang.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopkhieunaitoanmang" %>
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
                <h1>BÁO CÁO TỔNG HỢP KHIẾU NẠI TOÀN MẠNG
                    <asp:Label ID="lblTenDoiTac" runat="server"></asp:Label>
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
                <tr>
                    <th>STT
                    </th>
                    <th>Loại khiếu nại
                    </th>
                    <th>Lĩnh vực chung</th>
                    <th>SL tiếp nhận kỳ trước
                    </th>
                    <th>SL tiếp nhận trong kỳ
                    </th>
                    <th>Tỷ lệ SL tiếp nhận trong kỳ/ SL tiếp nhận kỳ trước (%)
                    </th>
                    <th>SL đã đóng trong kỳ
                    </th>
                    <th>SL đã đóng
                    </th>
                    <th>SL quá hạn toàn trình trong kỳ
                    </th>
                    <th>SL quá hạn toàn trình
                    </th>
                    <th>Tỷ lệ Quá hạn toàn trình trong kỳ/Tiếp nhận (%)
                    </th>
                </tr>
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
                    <td colspan="5" style="height: 70px">&nbsp;
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

        </div>
    </div>
</asp:Content>
