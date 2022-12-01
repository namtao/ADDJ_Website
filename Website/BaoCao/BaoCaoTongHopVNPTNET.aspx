<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master_Default.master" CodeBehind="BaoCaoTongHopVNPTNET.aspx.cs" Inherits="Website.BaoCao.BaoCaoTongHopVNPTNET" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .rptContent { margin-top: 10px; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <link href="/Css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/Js/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/Js/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="/Js/jquery.number.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".datetime").datepick({ dateFormat: 'dd/mm/yyyy' });
            $("#<%= GrvView.ClientID%> td a").on({
                click: function (e) {
                    try {
                        var top = 50;
                        var left = 50;
                        var link = $(this).attr("href");
                        window.open(link, "Danh sách chi tiết khiếu nại", "width=990,height=540,resizable=yes,toolbar=no,menubar=no,location=no,status=no,scrollbars=yes,top=" + top + ",left=" + left + "").focus();
                    }
                    catch (e) {
                        alert(e.message);
                    }
                    e.preventDefault();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <div class="wrapper">
        <table class="tbl_head_report tb2">
            <tr>
                <td class="coltitle">Đơn vị</td>
                <td class="colleft1">
                    <div class="selectstyle w200">
                        <div class="bg">
                            <asp:DropDownList runat="server" ID="ddlDoiTac">
                                <asp:ListItem Text="NET" Value="10194"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td class="coltitle">Định dạng</td>
                <td>
                    <asp:RadioButtonList CssClass="rd-selected" ID="rdlSelectedExport" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Selected="True" Value="1">Html</asp:ListItem>
                        <asp:ListItem Value="0">Excel</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="coltitle">Phòng ban / tổ</td>
                <td class="colleft1">
                    <div class="selectstyle w200">
                        <div class="bg">
                            <asp:DropDownList runat="server" ID="ddlPhongBan"></asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td class="coltitle">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="coltitle">Từ ngày</td>
                <td class="colleft1">
                    <div class="inputstyle w200">
                        <div class="bg">
                            <asp:TextBox ID="txtFromDate" CssClass="datetime" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </td>
                <td class="coltitle">Đến ngày</td>
                <td>
                    <div class="inputstyle w200">
                        <div class="bg">
                            <asp:TextBox ID="txtToDate" CssClass="datetime" runat="server"></asp:TextBox>
                        </div>
                    </div>

                </td>
            </tr>
            <tr>
                <td class="coltitle">&nbsp;</td>
                <td class="colleft1 export">
                    <asp:Button ID="btnExport" CssClass="btn_main savel" runat="server" Text="Lấy báo cáo" />
                </td>
                <td class="coltitle">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <asp:Panel runat="server" CssClass="rptContent" ID="RptContent">
            <asp:Literal runat="server" ID="liReport"></asp:Literal>
            <asp:GridView runat="server" AutoGenerateColumns="false" Width="100%" CssClass="tbl_style customized tblcus" ID="GrvView">
                <Columns>
                    <asp:TemplateField HeaderText="Tên đối tác">
                        <ItemTemplate>
                            <%# Eval("TenDoiTac") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL tồn kỳ trước">
                        <ItemTemplate>
                            <%# Eval("SLTonDongKyTruoc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL tiếp nhận">
                        <ItemTemplate>
                            <%# Eval("SLTiepNhan") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL đã xử lý (tiếp nhận)">
                        <ItemTemplate>
                            <%# Eval("SLDaXuLyTiepNhan") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL đã xử lý (lũy kế)">
                        <ItemTemplate>
                            <%# Eval("SLDaXuLyLuyKe") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL quá hạn đã xử lý">
                        <ItemTemplate>
                            <%# Eval("SLQuaHanDaXuLy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL tồn đọng">
                        <ItemTemplate>
                            <%# Eval("SLTonDong") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SL tồn đọng quá hạn">
                        <ItemTemplate>
                            <%# Eval("SLQuaHanTonDong") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    Hiện chưa có dữ liệu
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>

    </div>
</asp:Content>
