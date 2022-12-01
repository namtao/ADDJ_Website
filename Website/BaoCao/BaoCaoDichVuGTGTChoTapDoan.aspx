<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master_Default.master" CodeBehind="BaoCaoDichVuGTGTChoTapDoan.aspx.cs" Inherits="Website.BaoCao.BaoCaoDichVuGTGTChoTapDoan" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/Js/jquery.number.min.js" type="text/javascript"></script>
    <%-- Format Number --%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("span.number").number(true, 0, ",", ".");
            $(".openthis").on({click: function(e){
                // console.log(this);
                var link = $(this).attr("href");
                window.open(link, "Báo cáo dịch vụ GTGT cho tập đoàn", "width=990,height=540,resizable=yes,toolbar=no,menubar=no,location=no,status=no,scrollbars=yes,top=30");
                e.preventDefault();
            }});
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .tbl_head_report.report3 .colleft1 { width: 200px; }
        .tbl_style td span.number { display: block; text-align: right; }
        .rptContent { height: 500px; }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div class="wrapper">
        <table class="tbl_head_report tb2 report3">
            <tr>
                <td class="coltitle">Tháng</td>
                <td class="colleft1">
                    <div class="selectstyle w200">
                        <div class="bg">
                            <asp:DropDownList ID="ddlMonth" Width="150px" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td class="coltitle">Năm</td>
                <td class="colleft1">
                    <div class="selectstyle w200">
                        <div class="bg">
                            <asp:DropDownList ID="ddlYear" runat="server" Width="150px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="coltitle aligntop">Đơn vị báo cáo</td>
                <td colspan="4">
                    <asp:CheckBoxList CssClass="lst-check" ID="chkDonViBaoCao" runat="server">
                    </asp:CheckBoxList>

                </td>
            </tr>
            <tr>
                <td class="coltitle">Loại</td>
                <td colspan="4">
                    <asp:RadioButtonList ID="rpLoaiBaoCao" runat="server" CssClass="rd-selected surrounded export" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="1">Html</asp:ListItem>
                        <asp:ListItem Value="0">Excel</asp:ListItem>
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr>
                <td class="coltitle">&nbsp;</td>
                <td colspan="4">
                    <asp:Button ID="btnExport" runat="server" Text="Lấy báo cáo" CssClass="btn_main save" />

                </td>
            </tr>
        </table>
        <asp:Panel runat="server" CssClass="rptContent" ID="RptContent">
            <asp:Literal runat="server" ID="liReport"></asp:Literal>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            <asp:Literal Id="liJs" runat="server"></asp:Literal>
        });
    </script>
</asp:Content>
