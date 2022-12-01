<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType3_VNPTTT.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType3_VNPTTT" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%--<script type="text/javascript" src="../baocao.js"></script>--%>
        <table class="tb2">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTitle" runat="server" Text="Báo cáo khiếu nại quá hạn của KTV"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblIsFirstLoai" runat="server" Visible ="false"></asp:Label>           
                </th>    
            </tr>
            <tr runat="server" id="rowVNPTTT">
                <td>
                    Viễn thông
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlVNPTTT" runat="server" DataTextField="TenDoiTac" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddlVNPTTT_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="rowPhongBan">
                <td>
                    Phòng ban
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPhongBan" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Lấy báo cáo của
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblLayDuLieuTheo1HoacNhieuPhongBan" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Theo phòng ban được chọn</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">Theo phòng ban được chọn và các phòng ban trực thuộc</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>
                    Đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                </td>
            </tr>
            <tr id="rowChkDongKhieuNai" runat="server">
                <td>
                    Thống kê theo
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblThongKeTheoThoiGian" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">Theo ngày đóng khiếu nại</asp:ListItem>
                        <asp:ListItem Value="2">Theo ngày xử lý khiếu nại (Đã đóng và đang xử lý)</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3">                               
                    <asp:RadioButtonList ID="rblLoaiBaoCao" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th colspan="4">
                        <asp:LinkButton ID="lbReport" runat="server" CssClass="poplight btn" OnClick="lbReport_Click">
                            Báo cáo
                        </asp:LinkButton>                                          
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
