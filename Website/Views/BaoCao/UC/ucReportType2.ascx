<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType2.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType2" %>
<style type="text/css">
    #baocaokhieunai td { vertical-align: top; }
</style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!-- pnReportType2 -->
        <table class="tb2" width="100%">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoai" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>Khu vực
                </td>
                <td>
                    <asp:DropDownList ID="ddlKhuVuc_ReportType2" runat="server"
                        DataTextField="TenDoiTac" DataValueField="Id"
                        OnSelectedIndexChanged="ddlKhuVuc_ReportType2_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>

                    <asp:Label ID="lblKhuVucId_ReportType2" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblTenKhuVuc_ReportType2" runat="server" Visible="false"></asp:Label>
                </td>
                <td>Phòng
                </td>
                <td>
                    <asp:DropDownList ID="ddlPhongBan_ReportType2" runat="server"
                        DataTextField="Name" DataValueField="Id">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="todate"></asp:TextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:Label ID="lblDoiTac" runat="server" Text="Đối tác"></asp:Label>
                </td>
                <td>
                    <asp:CheckBoxList ID="cblDoiTac" runat="server" DataTextField="TenDoiTac" DataValueField="Id">
                    </asp:CheckBoxList>
                </td>
                <td>Loại khiếu nại
                </td>
                <td>
                    <div style="padding-left: 12px">
                        <asp:CheckBox ID="chkCheckAll" CssClass="chkCheckAll_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck toàn bộ các mục" />
                    </div>
                    <div style="padding-left: 12px">
                        <asp:CheckBox ID="chkAllFirstItem" CssClass="chkAllFirstItem_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục Loại khiếu nại" />
                    </div>
                    <div style="padding-left: 12px">
                        <asp:CheckBox ID="chkAutoCheckChildren" CssClass="chkAutoCheckChildren_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục con" />
                    </div>
                    <asp:TreeView ID="tvLoaiKhieuNai_ReportType2" runat="server" CssClass="treeViewLoaiKhieuNai"
                        ShowCheckBoxes="All" NodeWrap="True" Width="300px">
                    </asp:TreeView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
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
                    <asp:LinkButton ID="btnReport_ReportType2" runat="server"
                        CssClass="poplight btn" OnClick="btnReport_ReportType2_Click">Báo cáo</asp:LinkButton>
                </th>
            </tr>
        </table>

    </ContentTemplate>
</asp:UpdatePanel>


