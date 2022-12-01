<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDKTGiamTruCuocDVTraSau.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucDKTGiamTruCuocDVTraSau" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="tb2" width="100%">                       
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTittle" runat="server" Text="Báo cáo chi tiết giảm trừ cước dịch vụ trả sau" ></asp:Label>                                        
                </th>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="lblKhuVuc_ReportType1" runat="server">Khu vực</asp:Label>
                </td>
                <td>                                       
                    <asp:DropDownList ID="ddlKhuVuc_ReportType1" runat="server" 
                        DataTextField ="TenDoiTac" DataValueField="Id"
                        AutoPostBack="true" 
                        onselectedindexchanged="ddlKhuVuc_ReportType1_SelectedIndexChanged">                                                                                       
                    </asp:DropDownList>                                                                         
                </td>  
                <td >
                    <asp:Label ID="lblPhongBan_DoiTac_ReportType1" runat="server">Phòng</asp:Label>
                </td>
                <td >
                    <asp:DropDownList ID="ddlPhongBan_ReportType1" runat="server" DataTextField="Name" DataValueField="Id">
                    </asp:DropDownList>     
                                        
                    <asp:DropDownList ID="ddlDoiTac_ReportType1" runat="server" Visible="false"
                            DataTextField ="TenDoiTac" DataValueField="Id">
                    </asp:DropDownList>                                          
                </td>                                    
            </tr>
            <tr>
                <td>
                    Từ ngày
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>                                       
                </td>
                <td>
                    đến ngày
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>                                        
                </td>
            </tr>                        
            <tr>
                <td >
                    Loại khiếu nại
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" DataTextField="Name" 
                        DataValueField="Id" AutoPostBack="true" 
                        onselectedindexchanged="ddlLoaiKhieuNai_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>                            
            </tr>
            <tr>
                <td>
                    Lĩnh vực chung
                </td>
                <td >
                    <asp:DropDownList ID="ddlLinhVucChung" runat="server" DataTextField="Name" 
                        DataValueField="Id" AutoPostBack="true" 
                        onselectedindexchanged="ddlLinhVucChung_SelectedIndexChanged" Width="250px">
                    </asp:DropDownList>
                </td>
                <td>
                    Lĩnh vực con
                </td>
                <td >
                    <asp:DropDownList ID="ddlLinhVucCon" runat="server" DataTextField="Name" DataValueField="Id" Width="250px">
                    </asp:DropDownList>
                </td>
            </tr>                                                           
            <tr>
                <td>
                    &nbsp;
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
                        CssClass="poplight btn" onclick="lbReport_ReportType1_Click" >Báo cáo</asp:LinkButton>                                          
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
