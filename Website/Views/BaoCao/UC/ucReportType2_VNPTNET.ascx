<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType2_VNPTNET.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType2_VNPTNET" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="tb2" width="100%">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTittle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblKhuVucXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDonVi" runat="server">Đơn vị</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDonVi" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged">                        
                    </asp:DropDownList>
                </td>
                <td>                   
                </td>
                <td>                                               
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPhongBan" runat="server">Phòng ban/Tổ</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPhongBan" runat="server">                        
                    </asp:DropDownList>
                </td>
                <td>                   
                </td>
                <td>                                               
                </td>
            </tr>
            <tr valign="top">
                <td>Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                    <br />
                    (Giá trị của [đến ngày] nhỏ hơn ngày hiện tại)
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
                    <asp:LinkButton ID="lbReport" runat="server"
                        CssClass="poplight btn" OnClick="lbReport_Click">Báo cáo</asp:LinkButton>                   
                </th>
            </tr>
        </table>

    </ContentTemplate>


</asp:UpdatePanel>