<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType10_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType10_VNP" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="tb2" width="100%">                       
                <tr>
                    <th colspan="4">
                        <asp:Label ID="lblTitle" runat="server" ></asp:Label>      
                        <asp:Label ID="lblReportType" runat="server" Visible ="false" ></asp:Label>     
                        <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"  ></asp:Label>     
                        <asp:Label ID="lblIsFirstLoad" runat="server" Visible ="false"  ></asp:Label>                                                             
                    </th>
                </tr>               
                <tr>
                    <td valign="top" >
                        <asp:Label ID="lblPhongBan" runat="server">Phòng ban tiếp nhận</asp:Label>
                    </td>
                    <td>
                        <div ><asp:CheckBox ID="chkCheckAll"  runat="server" Text="Check/Uncheck toàn bộ các mục" AutoPostBack="True" OnCheckedChanged="chkCheckAll_CheckedChanged" Checked="true" /></div>      
                        <asp:CheckBoxList ID="cblPhongBan" runat="server"></asp:CheckBoxList>                                                                                    
                    </td>  
                </tr>                                                                                        
               <%-- <tr>
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
                            CssClass="poplight btn" onclick="lbReport_Click" >Báo cáo</asp:LinkButton>                                          
                    </th>
                </tr>
            </table>

        </ContentTemplate>
         
    </asp:UpdatePanel>
