<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType_DateRange.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType_DateRange" %>
<style type="text/css">
    #baocaokhieunai .tb2 { width: 100%; }
    .selectinput.cus select { padding: 2px; border: 1px solid #ccc; width: 210px !important; }
    .fromdate.cus, .todate.cus { padding: 4px; width: 200px !important; }
    #baocaokhieunai .tb2 td { padding: 3px 5px; }
</style>
<asp:UpdatePanel ID="UpdatePnl" runat="server">
    <ContentTemplate>
        <table class="tb2" cellpadding="0" border="0" cellspacing="0" width="100%">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTittle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblNguoiXuLy" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate cus"></asp:TextBox>
                </td>
                <td>Đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate cus"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Tỉnh</td>
                <td>
                    <div class="selectinput cus">
                        <div class="bg">
                            <asp:DropDownList runat="server" ID="ddlTinh">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td>Huyện</td>
                <td>
                    <div class="selectinput cus">
                        <div class="bg">
                            <asp:DropDownList runat="server" ID="ddlHuyen">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="rowNguonKhieuNai" runat="server">
                <td>Từ nguồn</td>
                <td colspan="3">
                    <div class="selectinput cus">
                        <div class="bg">
                            <asp:DropDownList ID="ddlNguonKhieuNai" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th colspan="4">
                    <asp:LinkButton ID="lbShowReport" runat="server" CssClass="poplight btn" OnClick="lbShowReport_Click">Báo cáo</asp:LinkButton>
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
