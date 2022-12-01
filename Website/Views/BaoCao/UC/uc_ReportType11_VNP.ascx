<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType11_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType11_VNP" %>

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
                    <asp:Label ID="lblKhuVuc" runat="server">Khu vực</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlKhuVuc" runat="server">
                        <asp:ListItem Value="1">VNP</asp:ListItem>
                        <asp:ListItem Value="2">VNP1</asp:ListItem>
                        <asp:ListItem Value="3">VNP2</asp:ListItem>
                        <asp:ListItem Value="5">VNP3</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <%--<asp:Label ID="lblPhongBan_DoiTac_ReportType1" runat="server">Phòng</asp:Label>--%>
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddlPhongBan_ReportType1" runat="server" DataTextField="Name" DataValueField="Id">
                    </asp:DropDownList>     
                                        
                    <asp:DropDownList ID="ddlDoiTac_ReportType1" runat="server" Visible="false"
                            DataTextField ="TenDoiTac" DataValueField="Id">
                    </asp:DropDownList>         --%>                                 
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
                <td>
                    Hiển thị theo
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblDisplayLevelLoaiKhieuNai" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">Loại khiếu nại</asp:ListItem>
                        <asp:ListItem Value="2">Lĩnh vực chung</asp:ListItem>
                        <asp:ListItem Value="3">Lĩnh vực con</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>            
            <tr>
                <td>
                    Loại khiếu nại
                </td>
                <td colspan="3">                   
                    <div style="padding-left:12px"><asp:CheckBox ID="chkCheckAll" CssClass="chkCheckAll_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck toàn bộ các mục" /></div>      
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAllFirstItem" CssClass="chkAllFirstItem_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục Loại khiếu nại" /></div>                    
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAutoCheckChildren" CssClass="chkAutoCheckChildren_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục con" /></div>                    

                    <asp:TreeView ID="tvLoaiKhieuNai" runat="server" ShowCheckBoxes="All" CssClass="treeViewLoaiKhieuNai" ></asp:TreeView>
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
                    <asp:LinkButton ID="lbReport_DanhSachKhieuNai" runat="server"
                        CssClass="poplight btn" OnClick="lbReport_DanhSachKhieuNai_Click">Danh sách khiếu nại</asp:LinkButton>
                </th>
            </tr>
        </table>

    </ContentTemplate>


</asp:UpdatePanel>