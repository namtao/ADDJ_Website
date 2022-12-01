<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportType_2DateRange.ascx.cs" Inherits="Website.Views.BaoCao.UC.ucReportType_2DateRange" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="tb2" width="100%">
            <tr>
                <th colspan="4">
                    <asp:Label ID="lblTittle" runat="server"></asp:Label>
                    <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblDoiTacId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPhongBanXuLyId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblIsFirstLoad" runat="server" Visible="false"></asp:Label>
                </th>
            </tr>
            <tr>
                <asp:Panel runat="server" ID="pnRegion">
                    <td>
                        <asp:Label ID="lblKhuVuc_ReportType1" runat="server">Khu vực</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlKhuVuc_ReportType1" runat="server"
                            DataTextField="TenDoiTac" DataValueField="Id"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlKhuVuc_ReportType1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%--<asp:Label ID="lblPhongBan_DoiTac_ReportType1" runat="server">Phòng</asp:Label>--%>
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlPhongBan_ReportType1" runat="server" DataTextField="Name" DataValueField="Id">
                        </asp:DropDownList>     
                                        
                        <asp:DropDownList ID="ddlDoiTac_ReportType1" runat="server" Visible="false"
                                DataTextField ="TenDoiTac" DataValueField="Id">
                        </asp:DropDownList>         --%>                                 
                    </td>
                </asp:Panel>
            </tr>
            <tr>
                <td>Từ ngày (kỳ trước)
                </td>
                <td>
                    <asp:TextBox ID="txtFromDateBefore" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>đến ngày (kỳ trước)
                </td>
                <td>
                    <asp:TextBox ID="txtToDateBefore" runat="server" CssClass="todate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Từ ngày (hiện tại)
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fromdate"></asp:TextBox>
                </td>
                <td>đến ngày (hiện tại)
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="todate"></asp:TextBox>
                </td>
            </tr>
            <tr id="rowNguonKhieuNai" runat="server">
                <td>
                        <asp:Label ID="lblNguonKhieuNai" runat="server">Nguồn khiếu nại</asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlNguonKhieuNai" runat="server">
                        </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td>&nbsp;
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
                        CssClass="poplight btn" OnClick="lbReport_ReportType1_Click">Báo cáo</asp:LinkButton>
                </th>
            </tr>
        </table>

    </ContentTemplate>


</asp:UpdatePanel>