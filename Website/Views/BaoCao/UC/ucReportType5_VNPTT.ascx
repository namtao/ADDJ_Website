<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType5_VNPTT.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType5_VNPTT" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%--<script type="text/javascript" src="../baocao.js"></script>--%>
        <table class="tb2">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTitle" runat="server" Text="Báo cáo khiếu nại quá hạn của KTV"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblDoiTacId" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible ="false"></asp:Label>     
                    <asp:Label ID="lblIsFirstLoai" runat="server" Visible ="false"></asp:Label>           
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
            <tr runat="server" id="rowPhongBan">
                <td>
                    Phòng ban
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPhongBan" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                </td>
            </tr>
         <%--   <tr >
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
                <td>Năm</td>
                <td><asp:DropDownList runat="server" ID="ddlYear"/></td>
                <td>
                    Tháng
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Text="Tháng 1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Tháng 2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Tháng 3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Tháng 4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Tháng 5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Tháng 6" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Tháng 7" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Tháng 8" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Tháng 9" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Tháng 10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="Tháng 11" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Tháng 12" Value="12"></asp:ListItem>
                    </asp:DropDownList>
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
                        <asp:LinkButton ID="lbReport" runat="server" CssClass="poplight btn" OnClick="lbReport_Click" >
                            Báo cáo
                        </asp:LinkButton>                                          
                </th>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
