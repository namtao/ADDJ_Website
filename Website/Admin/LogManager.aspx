<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    CodeBehind="LogManager.aspx.cs" Inherits="Website.admin.LogManager" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/Css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .col1 {
            width: 120px;
            text-align: right;
            padding-right: 10px;
        }

        .col2 .inputstyle input[type=text] {
            width: 110px;
            margin-left: 2px;
        }

        .col2 {
            width: 230px;
        }

        .selectstyle select {
            width: 98%;
        }

        .msg {
            padding: 10px 5px;
        }

        .tbl_style th {
            padding-left: 5px;
            padding-right: 5px;
        }

        .txtcenter {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/Js/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/Js/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script type="text/javascript">
        function LoadJS() {
            $("#<%=txtFromDate.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
            $("#<%=txtToDate.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
        }

        $(document).ready(function () {
            LoadJS();
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                try {
                    $(".over-screen").removeClass("off").addClass("on");
                }
                catch (e) {
                    if (console && console.log) console.log(e);
                }
            }

            function EndRequestHandler(sender, args) {
                try {
                    LoadJS();
                    setTimeout(function () {
                        $(".over-screen").removeClass("on").addClass("off");
                    }, 500);
                    if (args.get_error() == undefined) {
                        var sName = args.get_response().get_webRequest().get_userContext();
                        if (sName == "btnDetails") {
                            // DoSomething();

                        }
                        else {
                            // DoSomethingelse();
                        }
                    }
                }
                catch (e) {
                    if (console && console.log) console.log(e);
                }
            }
        });
    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li>
                        <a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a>
                    </li>
                </ul>
            </div>
            <!-- end panel nav boot -->
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->

                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="col1">Từ ngày </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="col1">Đến ngày </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="col1">Đối tượng</td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlObjType" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </td>
                                <td class="col1">Hành động </td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlAction" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="col1">Mã đối tượng </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtMaDoiTuong" runat="server" Width="250px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td class="col1">Chi tiết lỗi </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtChiTietLoi" runat="server" Width="250px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btPhanQuyen" runat="server" CssClass="newbtn grad filter" OnClick="btSearch_Click" Text="Lọc" />
                                    <asp:Button ID="btClearFilter" runat="server" CssClass="newbtn grad rmfilter" OnClick="btClearFilter_Click" Text="Bỏ lọc" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="5" class="msg">
                                    <div style="float: left;">
                                        <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                                    </div>
                                    <div style="float: right">
                                        <%= ConfigurationManager.AppSettings["ServerIP"] %>
                                    </div>
                                    <div class="vide"></div>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="grvView" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" DataKeyNames="Id" OnPageIndexChanging="grvView_PageIndexChanging" OnRowDataBound="grvView_RowDataBound">
                            <RowStyle CssClass="rowB" />
                            <AlternatingRowStyle CssClass="rowA" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="STT" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:TemplateField>
                                <asp:BoundField DataField="ObjId" HeaderStyle-HorizontalAlign="Center" HeaderText="Mã Id" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ObjName" HeaderStyle-HorizontalAlign="Center" HeaderText="Tên đối tượng" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Hành động">
                                    <ItemTemplate>
                                        <%# GetActionType(Eval("Action"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Note" HeaderStyle-HorizontalAlign="Center" HeaderText="Nội dung" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Ip" HeaderStyle-HorizontalAlign="Center" HeaderText="IP" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Username" HeaderStyle-HorizontalAlign="Center" HeaderText="Người dùng" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Thời gian">
                                    <ItemTemplate>
                                        <%# GetDateCreate(Eval("DateCreate"), Eval("TimeCreate"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="margin-top: 10px;" class="DDPager">
                            <div style="float: left">
                                <asp:ImageButton AlternateText="Trang đầu tiên" ToolTip="Trang đầu tiên" ID="ImageButtonFirst" runat="server" ImageUrl="~/admin/Images/PgFirst.gif" Width="8" Height="9" OnClick="ImageButtonFirst_Click" />
                                &nbsp;
                        <asp:ImageButton AlternateText="Trang trước" ToolTip="Trang trước" ID="ImageButtonPrev" runat="server" ImageUrl="~/admin/Images/PgPrev.gif" Width="5" Height="9" OnClick="ImageButtonPrev_Click" />
                                &nbsp;
                        <asp:Label ID="LabelPage" runat="server" Text="Trang " AssociatedControlID="TextBoxPage" />
                                <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" OnTextChanged="TextBoxPage_TextChanged" Width="20px" CssClass="DDControl txtcenter" />
                                /
                        <asp:Label ID="LabelNumberOfPages" runat="server" />
                                &nbsp;
                        <asp:ImageButton AlternateText="Trang tiếp theo" ToolTip="Trang tiếp theo" ID="ImageButtonNext" runat="server" ImageUrl="~/admin/Images/PgNext.gif" Width="5" Height="9" OnClick="ImageButtonNext_Click" />
                                &nbsp;
                        <asp:ImageButton AlternateText="Trang cuối cùng" ToolTip="Trang cuối cùng" ID="ImageButtonLast" runat="server" ImageUrl="~/admin/Images/PgLast.gif" Width="8" Height="9" OnClick="ImageButtonLast_Click" />
                            </div>
                            <div style="float: right">
                                <asp:Label ID="LabelRows" runat="server" Text="Kết quả trên 1 trang:" AssociatedControlID="DropDownListPageSize" />
                                <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="DDControl" OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
                                    <asp:ListItem Value="15" />
                                    <asp:ListItem Value="20" />
                                    <asp:ListItem Value="30" />
                                    <asp:ListItem Value="40" />
                                    <asp:ListItem Value="50" />
                                    <asp:ListItem Value="100" />
                                </asp:DropDownList>
                                /&nbsp Tổng số:
                        <asp:Literal ID="ltTongSoBanGhi" runat="server"></asp:Literal>
                            </div>
                            <div class="vide"></div>
                        </div>

                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="over-screen off">
        <div class="mask"></div>
        <div class="loading">
            <div class="loadAjax">
                <img runat="server" src="~/images/loading.gif" alt="Đang tải ..." />
            </div>
        </div>
    </div>
</asp:Content>
