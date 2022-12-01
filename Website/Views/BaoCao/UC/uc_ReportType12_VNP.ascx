<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType12_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType12_VNP" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="tb2" width="100%">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTittle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblKhuVuc" runat="server">Khu vực/Đối tác</asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlKhuVuc" runat="server">                     
                    </asp:DropDownList>
                </td>
                <td>
                   
                </td>
                <td>
                               
                </td>
            </tr>
            <tr>
                <td>Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Từ nguồn</td>
                 <td colspan="3">
                    <asp:DropDownList ID="ddlNguonKhieuNai" runat="server">                        
                    </asp:DropDownList>
                </td>
            </tr>        
            <tr>
                <td valign="top">
                    Đối tượng
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLoaiKhieuNai_SelectedIndexChanged"></asp:DropDownList>                    
                </td>
            </tr>    
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlLinhVucChung" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLinhVucChung_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlLinhVucCon" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType1" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th colspan="4">
                    <asp:LinkButton ID="lbReport_ReportType1" runat="server"
                        CssClass="poplight btn" OnClick="lbReport_ReportType1_Click">Báo cáo tổng hợp</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lbReport_BaoCaoChiTietChatLuongPhucVu" runat="server"
                        CssClass="poplight btn" OnClick="lbReport_BaoCaoChiTietChatLuongPhucVu_Click">Báo cáo chi tiết CLPV</asp:LinkButton>
                </th>
            </tr>
        </table>

    </ContentTemplate>


</asp:UpdatePanel>