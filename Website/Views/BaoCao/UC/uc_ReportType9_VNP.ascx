<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ReportType9_VNP.ascx.cs" Inherits="Website.Views.BaoCao.UC.uc_ReportType9_VNP" %>

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
                    <td >
                        <asp:Label ID="lblKhuVuc" runat="server">Khu vực</asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlKhuVuc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlKhuVuc_SelectedIndexChanged">                                                       
                        </asp:DropDownList>                                                                                                          
                    </td>                                    
                </tr>
                <tr>
                    <td valign="top" >
                        <asp:Label ID="lblDoiTac" runat="server">Đối tác</asp:Label>
                    </td>
                    <td >
                        <asp:CheckBoxList ID="cblDoiTac" runat="server"></asp:CheckBoxList>                                                                                    
                    </td>  
                </tr>
                <tr>
                    <%--<td>
                        Từ ngày
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>                                       
                    </td>--%>
                    <td>
                        Quá hạn phòng ban đến ngày
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>  
                        <asp:TextBox ID="txtToTime" runat="server" Text="23:59"></asp:TextBox>                                      
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
                            CssClass="poplight btn" onclick="lbReport_Click" >Báo cáo</asp:LinkButton>                                          
                    </th>
                </tr>
            </table>

        </ContentTemplate>
         

    </asp:UpdatePanel>