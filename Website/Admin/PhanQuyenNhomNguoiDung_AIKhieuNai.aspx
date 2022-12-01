<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_PhanQuyenNhomNguoiDung_AIKhieuNai" CodeBehind="PhanQuyenNhomNguoiDung_AIKhieuNai.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function LoadJS() {
                    // add multiple select / deselect functionality
                    $("#selectallRead").click(function () {
                        $('.chkAllRead').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllRead").click(function () {
                        if ($(".chkAllRead").find("//input[checked='checked']").length == $(".chkAllRead").find("input").length) {
                            $("#selectallRead").attr("checked", "checked");
                        } else {
                            $("#selectallRead").removeAttr("checked");
                        }
                    });
                }
                $(function () {
                    // add multiple select / deselect functionality
                    $("#selectallRead").click(function () {
                        $('.chkAllRead').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllRead").click(function () {
                        if ($(".chkAllRead").find("//input[checked='checked']").length == $(".chkAllRead").find("input").length) {
                            $("#selectallRead").attr("checked", "checked");
                        } else {
                            $("#selectallRead").removeAttr("checked");
                        }
                    });
                });

                function QuayVe() {
                    var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl"));
                    if (returnURL == "") {
                        history.back();
                    }
                    else {
                        window.location.href = returnURL;
                    }
                }
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnPhanQuyen" class="btn btn-primary" runat="server" OnClick="linkbtnPhanQuyen_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Cập nhật
                        </asp:LinkButton>
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

                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 20px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: left; width: 25%">Chọn phòng ban:
                        <asp:DropDownList AutoPostBack="true" ID="ddlUser" runat="server" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                        </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Nhóm quyền mặc định:
                        <asp:DropDownList AutoPostBack="true" ID="ddlPermissionDefault" runat="server" OnSelectedIndexChanged="ddlPermissionDefault_SelectedIndexChanged">
                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: left">
                                    <asp:Literal ID="lbMess" runat="server" Text=""></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="PermissionId"
                                        AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField ItemStyle-Width="60%" DataField="Name" HeaderText="Chức năng" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Read" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Thực hiện
                                        <input type="checkbox" id="selectallRead" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRead" CssClass="chkAllRead" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAllow")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 20px"></td>
                            </tr>
                        </table>

                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
