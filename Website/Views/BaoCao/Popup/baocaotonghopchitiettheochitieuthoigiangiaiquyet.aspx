<%@ Page MasterPageFile="~/Views/BaoCao/BaoCao.Master" CodeBehind="baocaotonghopchitiettheochitieuthoigiangiaiquyet.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopchitiettheochitieuthoigiangiaiquyet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
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

    <%--    <br />
        [1]: Số lượng KN tiếp nhận
        <br />
        [2]: Số lượng KN giải quyết trong kỳ tiếp nhận
        <br />
        [3]: Tổng thời gian giải quyết (phút)
        <br />
        [4]: Tổng số KN được giảm trừ
        <br />
        [5]: Tổng số tiền được giảm trừ
        <br />
        [6]: Tổng số KN được giải quyết/tổng số KN tiếp nhận (%)
        <br />
        [7]: Tổng số KN được giảm trừ/tổng số KN được giải quyết (%)
        <br />
        [8]: So sánh thời gian giải quyết/thời hạn theo quy định (%)--%>
        <div>
            <asp:Button ID="btnExportExcel" runat="server" Text="Xuất excel" OnClick="btnExportExcel_Click" />
        </div>
    </div>
</asp:Content>