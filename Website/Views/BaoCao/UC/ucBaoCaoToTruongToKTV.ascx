<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBaoCaoToTruongToKTV.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucBaoCaoToTruongToKTV" %>

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
