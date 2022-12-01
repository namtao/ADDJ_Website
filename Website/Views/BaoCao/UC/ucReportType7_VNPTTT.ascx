<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType7_VNPTTT.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType7_VNPTTT" %>


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
                        VNPT
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPhongBan" runat="server" DataTextField="Name" DataValueField="Id">
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