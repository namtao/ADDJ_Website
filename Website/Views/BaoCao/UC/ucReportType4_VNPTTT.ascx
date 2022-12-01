<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType4_VNPTTT.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType4_VNPTTT" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
       
 <!-- pnReportType2 -->   
        <table class="tb2" width="100%">                       
            <tr>
                <th colspan="4">                                
                    <asp:Label ID="lblTitle" runat="server" ></asp:Label>     
                    <asp:Label ID="lblReportType" runat="server" Visible ="false" ></asp:Label>     
                    <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"  ></asp:Label>     
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"  ></asp:Label>     
                    <asp:Label ID="lblIsFirstLoai" runat="server" Visible ="false"  ></asp:Label>                            
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
            <tr>
                <td>
                    Phòng ban
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPhongBan" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                </td>
            </tr>
            <%--<tr>
                <td>
                    Lấy báo cáo của
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblLayDuLieuTheo1HoacNhieuPhongBan" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Theo phòng ban được chọn</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">Theo phòng ban được chọn và các phòng ban trực thuộc</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>--%>
            <tr>
                <td>
                    Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                </td>
                <td>
                    đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate_BaoCaoTongHopTheoKhieuNai" runat="server" CssClass="todate"></asp:TextBox>                                       
                </td>
            </tr>
            <tr>                                    
                <td valign="top">
                    Loại khiếu nại
                </td>
                <td colspan="3">         
                    <div style="padding-left:12px"><asp:CheckBox ID="chkCheckAll" CssClass="chkCheckAll_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck toàn bộ các mục" /></div>      
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAllFirstItem" CssClass="chkAllFirstItem_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục Loại khiếu nại" /></div>                    
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAutoCheckChildren" CssClass="chkAutoCheckChildren_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục con" /></div>                    
                    <asp:TreeView ID="tvLoaiKhieuNai_ReportType2" runat="server" CssClass="treeViewLoaiKhieuNai" 
                        ShowCheckBoxes="All" NodeWrap="True" Width="300px">
                    </asp:TreeView>
                </td>                     
            </tr>       
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3" >
                        <asp:RadioButtonList ID="rblLoaiBaoCao" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>     
            <tr>
                <th colspan="4">
                    <asp:LinkButton ID="btnReport_ReportType2" runat="server" 
                        CssClass="poplight btn" onclick="btnReport_ReportType2_Click">Báo cáo</asp:LinkButton>                                                                        
                </th>
            </tr>
        </table>   

    </ContentTemplate>
</asp:UpdatePanel>