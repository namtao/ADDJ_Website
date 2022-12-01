<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType15_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType15_VNP" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="tb2" width="100%">                       
                <tr>
                    <th colspan="4">
                        <asp:Label ID="lblTittle" runat="server" ></asp:Label>      
                        <asp:Label ID="lblReportType" runat="server" Visible ="false" ></asp:Label>     
                        <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblIsFirstLoai" runat="server" Visible ="false"  ></asp:Label>                                                             
                    </th>
                </tr>    
                <tr>
                    <td>
                        Đơn vị
                    </td>
                    <td colspan="3">
                        <asp:TreeView onclick="postbackOnCheck()" ID="TreeView1" runat="server" ShowLines="True"  ShowCheckBoxes="All" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged">
                        </asp:TreeView>
                    </td>                    
                </tr>              
                <tr>
                    <td>
                        Từ ngày</td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                    </td>
                    <td>
                        đến ngày</td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                    </td>
                </tr>                                                                                           
                <tr>
                    <td>
                        Từ mức tiền:</td>
                    <td>                               
                        <asp:TextBox ID="txtMucTien1" runat="server"  ></asp:TextBox>
                    </td>
                    <td>đến mức tiền</td>
                    <td>
                        <asp:TextBox ID="txtMucTien2" runat="server" ></asp:TextBox>
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
                            <asp:LinkButton ID="lbReport" runat="server" 
                            CssClass="poplight btn" OnClick="lbReport_Click">Báo cáo</asp:LinkButton>                                          
                    </th>
                </tr>
            </table> 
        </ContentTemplate>
    </asp:UpdatePanel>