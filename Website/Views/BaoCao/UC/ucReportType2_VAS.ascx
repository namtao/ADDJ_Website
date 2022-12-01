<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType2_VAS.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType2_VAS" %>

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
                        Khu vực
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlKhuVuc" runat="server">
                            <asp:ListItem Value="1" Text="-- Tất cả --"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Khu vực 1"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Khu vực 2"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Khu vực 3"></asp:ListItem>
                        </asp:DropDownList>                 
                    </td>
                    <td>
                        
                    </td>
                    <td>
                                                            
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
                <tr runat="server" visible="false">
                    <td >
                        Loại khiếu nại
                    </td>
                    <td colspan="3">                               
                        <asp:TreeView ID="tvLoaiKhieuNai" runat="server" ShowCheckBoxes="All">
                        </asp:TreeView>
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
                            <asp:LinkButton ID="lbShowReport" runat="server" 
                            CssClass="poplight btn" onclick="lbShowReport_Click">Báo cáo</asp:LinkButton>                                          
                    </th>
                </tr>
            </table>

         </ContentTemplate>
     </asp:UpdatePanel> 