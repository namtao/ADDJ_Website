<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType_DateRange_New.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType_DateRange_New" %>

<style type="text/css">
    .col-left { width: 121px; }
    .lst-check { margin-left: -10px; }
    .lst-check label { display: inline-block; padding-left: 7px; }
    #baocaokhieunai .surrounded.export { margin-left: -10px; }
</style>

<asp:UpdatePanel ID="UdPnlMain" runat="server">
    <ContentTemplate>
        <table class="tb2" style="width: 100%;">
            <tr>
                <th colspan="2">
                    <asp:Label ID="lblTittle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblNguoiXuLy" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <td class="col-left">Tháng</td>
                <td>
                    <asp:DropDownList ID="ddlMonth" Width="150px" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="col-left">Năm</td>
                <td>
                    <asp:DropDownList ID="ddlYear" Width="150px" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="col-left" style="vertical-align: top">Đơn vị báo cáo</td>
                <td>
                    <asp:CheckBoxList CssClass="lst-check" ID="chkDonViBaoCao" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="col-left">&nbsp;</td>
                <td>
                    <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType" runat="server" CssClass="surrounded export" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="html">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>

                <th colspan="2">
                    <asp:LinkButton ID="lbShowReport" runat="server" CssClass="poplight btn" OnClick="lbShowReport_Click">Báo cáo</asp:LinkButton>
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
