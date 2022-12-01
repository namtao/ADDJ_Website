<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_LoaiPhongBan2PhongBan" CodeBehind="LoaiPhongBan2PhongBan.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
            <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
            <script type="text/javascript">

                $(document).ready(function () {
                    LoadJS();
                });

                function LoadJS() {
                    $("#selectall").click(function () {
                        $('.case').find("input").attr('checked', this.checked);
                    });
                    $(".case").click(function () {
                        if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                            $("#selectall").attr("checked", "checked");
                        } else {
                            $("#selectall").removeAttr("checked");
                        }
                    });
                }

            </script>
            <script type="text/javascript">
                function validForm() {
                    return true;
                }
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                </ul>
            </div>
            <div class="p8">
                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->

                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 25px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px"></td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: center; padding-right: 10px">Danh sách phòng ban được nhận khiếu nại của của loại khiếu nại
                            <br />
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlLoaiKhieuNai" AutoPostBack="true" OnSelectedIndexChanged="ddlPhongBan_Changed"
                                                runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 70%; text-align: center">Thêm phòng ban chuyển xử lý
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Button ID="btLeaver" runat="server" CssClass="btn_style_button" Text="Xóa phòng ban khỏi loại phòng ban >>"
                                        OnClick="btLeaver_Click" />
                                </td>
                                <td style="width: 70%; text-align: center">
                                    <asp:Button ID="btAddPhongBan2LoaiPhongBan" runat="server" CssClass="btn_style_button" Text="<< Thêm phòng ban vào loại phòng ban"
                                        OnClick="btAddPhong_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px"></td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: left; padding-right: 10px">
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound"
                                        OnPageIndexChanging="grvView_PageIndexChanging">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Phòng ban" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="15%">
                                                <HeaderTemplate>
                                                    Chọn
                                            <input type="checkbox" id="selectall" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td style="width: 70%; text-align: center; vertical-align: top">
                                    <table border="0" width="100%">
                                        <tr>
                                            <td style="width: 30%;">Tên phòng ban hoặc mô tả
                                            </td>
                                            <td>
                                                <div class="inputstyle">
                                                    <div class="bg">
                                                        <asp:TextBox ID="txtTenDangNhap" runat="server" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width: 50px;"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="inputstyle">
                                                    <div class="bg">
                                                        <asp:TextBox ID="txtListTenDangNhap" Width="100%" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td style="height: 25px"></td>
                            </tr>
                        </table>

                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
            <script type="text/javascript">
                function AutocompleteTenDangNhap() {
                    $("#<%=txtTenDangNhap.ClientID %>").autocomplete("/admin/Ajax/AutocompleteProcess.ashx?type=AutoCompletePhongBan", {
                        dataType: "json",
                        width: 300,
                        max: 15,
                        parse: function (data) {
                            return $.map(data, function (row, index) {
                                return {
                                    data: row,
                                    value: index.toString(),
                                    result: row.Name
                                };
                            });
                        },
                        formatItem: function (item) {
                            return format(item);
                        }
                    }).result(function (e, item) {
                        $("#<%=txtTenDangNhap.ClientID %>").val(item.Name);
                        });

                    $("#<%=txtTenDangNhap.ClientID %>").keydown(function (e) {
                        if (e.keyCode == '13') {
                            document.getElementById('<%=btAddPhongBan2LoaiPhongBan.ClientID%>').click();
                        }
                    });

                    $("#<%=txtTenDangNhap.ClientID %>").focus();
                }

                AutocompleteTenDangNhap();

                function format(item) {
                    return "<span class='ac_keyword'>" + item.Name + "</span>";
                }

            </script>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
