<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="DanhSachKhieuNai.aspx.cs" Inherits="Website.Views.BaoCao.Popup.danhsachkhieunai" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblNoiDungBaoCao" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblTenFile" runat="server" Visible="false"></asp:Label>

    <div runat="server" id="baocao" class="reportFont">
        <table border="0" width="100%" align="center">
            <tr>
                <td style="text-align: center; font-weight: bold" colspan="12">
                    <asp:Literal runat="server" ID="ltTitle"></asp:Literal></td>
            </tr>
            <tr>
                <td style="text-align: center; font-weight: bold" colspan="12">
                    <asp:Literal runat="server" ID="lbPhongBan_NguoiDung"></asp:Literal></td>
            </tr>
            <tr>
                <td style="text-align: center; font-weight: bold" colspan="12">Từ ngày - đến ngày:
                    <asp:Label runat="server" ID="lblTuNgay"></asp:Label>
                    -
                    <asp:Label runat="server" ID="lblDenNgay"></asp:Label>
                </td>
            </tr>
        </table>

        <br />

        <div>
            <asp:Button ID="btnExportExcelTop" runat="server" Text="Xuất excel" OnClick="btnExportExcel_Click" />
        </div>

        <br />

        <table class="tbl_style" border="1" style="border-collapse: collapse;" id="noiDungBaoCao">
            <%=sNoiDungBaoCao %>
        </table>

        <br />
        <div>
            <asp:Button ID="btnExportExcel" runat="server" Text="Xuất excel" OnClick="btnExportExcel_Click" />
        </div>

    </div>

</asp:Content>
