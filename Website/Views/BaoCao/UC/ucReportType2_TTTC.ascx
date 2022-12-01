<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType2_TTTC.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType2_TTTC" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="tb2" width="100%">                       
            <tr>
                <th colspan="4">                                
                    <asp:Label ID="lblTittle" runat="server" ></asp:Label>      
                    <asp:Label ID="lblReportType" runat="server" Visible ="false" ></asp:Label>     
                    <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"  ></asp:Label>     
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"  ></asp:Label>     
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible ="false"  ></asp:Label>                           
                </th>
            </tr>                                
            <tr>
                <td>
                    Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate_ReportType3" runat="server" CssClass="fromdate"></asp:TextBox>
                    <%--<input type="text" runat="server" value="" id="txtFromDate_BaoCaoTongHopTheoKhieuNai" />--%>
                </td>
                <td>
                    đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate_ReportType3" runat="server" CssClass="todate"></asp:TextBox>
                    <%--<input type="text" runat="server" value="" id="txtToDate_BaoCaoTongHopTheoKhieuNai" />--%>
                </td>
            </tr>
            <tr valign="top">
                <td >
                    Phòng
                </td>
                <td >                                        
                    <asp:DropDownList ID="ddlPhongBan_ReportType3" runat="server" 
                        DataTextField ="Name" DataValueField="Id" AutoPostBack="true" 
                        onselectedindexchanged="ddlPhongBan_ReportType3_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>   
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Người dùng"></asp:Label>
                </td>
                <td>                                        
                    <asp:CheckBoxList ID="cblNguoiDung" runat="server" DataTextField="TenDayDu" DataValueField="TenTruyCap">
                    </asp:CheckBoxList>
                </td>
                <%--<td >
                    Loại khiếu nại
                </td>
                <td>                               
                    <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All">
                    </asp:TreeView>
                </td>        --%>                    
            </tr>                        
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3">
                        <asp:RadioButtonList ID="rblLoaiBaoCao_ReportType3" runat="server" CssClass="surrounded" RepeatDirection="Horizontal">
                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                        <asp:ListItem Value="excel">EXCEL</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th colspan="4">
                    <asp:LinkButton ID="btnReport_ReportType3" runat="server" 
                        CssClass="poplight btn" 
                        onclick="btnReport_ReportType3_Click">Báo cáo</asp:LinkButton>                                
                    <%--<a id="btnReport_TongHopTheoKhieuNai" class="poplight btn" href="#"><span>Báo cáo</span></a>--%>
                        <%--<a href="#1" class="btn"><span>Hủy</span></a>--%>
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
    
