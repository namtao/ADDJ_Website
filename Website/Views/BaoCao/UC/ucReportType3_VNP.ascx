<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType3_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType3_VNP" %>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                <tr>                                    
                <td >
                    Khu vực
                </td>
                <td >                                                                                
                    <asp:DropDownList ID="ddlKhuVuc" runat="server" 
                        DataTextField ="TenDoiTac" DataValueField="Id"                                     
                        onselectedindexchanged="ddlKhuVuc_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList>                   
                </td>        
                <td >
                    Phòng
                </td>
                <td >                                        
                    <asp:DropDownList ID="ddlPhongBan" runat="server" 
                        DataTextField ="Name" DataValueField="Id">
                    </asp:DropDownList>
                </td>                           
            </tr>               
                <tr>
                    <td>
                        Từ ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate1" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                    </td>
                    <td>
                        đến ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate1" runat="server" CssClass="todate"></asp:TextBox>                                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Từ ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate2" runat="server" CssClass="fromdate"></asp:TextBox>                                        
                    </td>
                    <td>
                        đến ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate2" runat="server" CssClass="todate"></asp:TextBox>                                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Thống kê theo loại khiếu nại</asp:ListItem>
                            <asp:ListItem Value="2">Thống kê theo lĩnh vực chung</asp:ListItem>
                            <asp:ListItem Value="3" Selected="True">Thống kê theo lĩnh vực con</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr valign="top">                                    
                    <td >
                        Loại khiếu nại
                    </td>
                    <td colspan="3">  
                        <div style="padding-left:12px"><asp:CheckBox ID="chkCheckAll" CssClass="chkCheckAll_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck toàn bộ các mục" /></div>      
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAllFirstItem" CssClass="chkAllFirstItem_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục Loại khiếu nại" /></div>                    
                    <div style="padding-left:12px"><asp:CheckBox ID="chkAutoCheckChildren" CssClass="chkAutoCheckChildren_TreeViewLoaiKhieuNai" runat="server" Text="Check/Uncheck các mục con" /></div>                                             
                        <asp:TreeView ID="tvLoaiKhieuNai" runat="server" ShowCheckBoxes="All" CssClass="treeViewLoaiKhieuNai">
                        </asp:TreeView>
                    </td>                            
                </tr>                        
                <%--<tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                            <asp:RadioButtonList ID="rblLoaiBaoCao" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                            <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                            <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>--%>
                <tr>
                    <th colspan="4">
                        <asp:LinkButton ID="lbReport" runat="server" 
                            CssClass="poplight btn" OnClick="lbReport_Click">Báo cáo</asp:LinkButton>                                                                      
                    </th>
                </tr>
            </table>

        </ContentTemplate>

    </asp:UpdatePanel>
