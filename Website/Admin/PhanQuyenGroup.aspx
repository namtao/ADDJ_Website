<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_PhanQuyenGroup" CodeBehind="PhanQuyenGroup.aspx.cs" %>

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
                    // add multiple select / deselect functionality
                    $("#selectallEdit").click(function () {
                        $('.chkAllEdit').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllEdit").click(function () {
                        if ($(".chkAllEdit").find("//input[checked='checked']").length == $(".chkAllEdit").find("input").length) {
                            $("#selectallEdit").attr("checked", "checked");
                        } else {
                            $("#selectallEdit").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallDelete").click(function () {
                        $('.chkAllDelete').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllDelete").click(function () {
                        if ($(".chkAllDelete").find("//input[checked='checked']").length == $(".chkAllDelete").find("input").length) {
                            $("#selectallDelete").attr("checked", "checked");
                        } else {
                            $("#selectallDelete").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallOther1").click(function () {
                        $('.chkAllOther1').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllOther1").click(function () {
                        if ($(".chkAllOther1").find("//input[checked='checked']").length == $(".chkAllOther1").find("input").length) {
                            $("#selectallOther1").attr("checked", "checked");
                        } else {
                            $("#selectallOther1").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallOther2").click(function () {
                        $('.chkAllOther2').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllOther2").click(function () {
                        if ($(".chkAllOther2").find("//input[checked='checked']").length == $(".chkAllOther2").find("input").length) {
                            $("#selectallOther2").attr("checked", "checked");
                        } else {
                            $("#selectallOther2").removeAttr("checked");
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
                    // add multiple select / deselect functionality
                    $("#selectallEdit").click(function () {
                        $('.chkAllEdit').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllEdit").click(function () {
                        if ($(".chkAllEdit").find("//input[checked='checked']").length == $(".chkAllEdit").find("input").length) {
                            $("#selectallEdit").attr("checked", "checked");
                        } else {
                            $("#selectallEdit").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallDelete").click(function () {
                        $('.chkAllDelete').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllDelete").click(function () {
                        if ($(".chkAllDelete").find("//input[checked='checked']").length == $(".chkAllDelete").find("input").length) {
                            $("#selectallDelete").attr("checked", "checked");
                        } else {
                            $("#selectallDelete").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallOther1").click(function () {
                        $('.chkAllOther1').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllOther1").click(function () {
                        if ($(".chkAllOther1").find("//input[checked='checked']").length == $(".chkAllOther1").find("input").length) {
                            $("#selectallOther1").attr("checked", "checked");
                        } else {
                            $("#selectallOther1").removeAttr("checked");
                        }
                    });
                    // add multiple select / deselect functionality
                    $("#selectallOther2").click(function () {
                        $('.chkAllOther2').find("input").attr('checked', this.checked);
                    });
                    // if all checkbox are selected, check the selectall checkbox
                    // and viceversa
                    $(".chkAllOther2").click(function () {
                        if ($(".chkAllOther2").find("//input[checked='checked']").length == $(".chkAllOther2").find("input").length) {
                            $("#selectallOther2").attr("checked", "checked");
                        } else {
                            $("#selectallOther2").removeAttr("checked");
                        }
                    });
                });
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnPhanQuyen"  class="btn btn-primary" runat="server" OnClick="linkbtnPhanQuyen_Click">
                             <span class="glyphicon glyphicon-floppy-saved" aria-hidden="true"></span> Cập nhật
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
                                <td style="text-align: left; width: 25%">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            Chọn nhóm:
                        <asp:DropDownList AutoPostBack="true" ID="ddlUser" runat="server" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                        </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="text-align: right; width: 25%">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            Chọn menu:
                        <asp:DropDownList AutoPostBack="true" ID="ddlMenu" runat="server" OnSelectedIndexChanged="ddlMenu_SelectedIndexChanged">
                        </asp:DropDownList>
                                        </div>
                                    </div>
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
                                        DataKeyNames="ID" AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField ItemStyle-Width="40%" DataField="Name" HeaderText="Tên menu" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Read">
                                                <HeaderTemplate>
                                                    Xem
                                        <input type="checkbox" id="selectallRead" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRead" CssClass="chkAllRead" runat="server" Checked='<%# Convert.ToBoolean(Eval("UserRead")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Edit">
                                                <HeaderTemplate>
                                                    Sửa
                                        <input type="checkbox" id="selectallEdit" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEdit" CssClass="chkAllEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("UserEdit")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Delete">
                                                <HeaderTemplate>
                                                    Xóa
                                        <input type="checkbox" id="selectallDelete" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDelete" CssClass="chkAllDelete" runat="server" Checked='<%# Convert.ToBoolean(Eval("UserDelete")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Delete">
                                                <HeaderTemplate>
                                                    Chức năng khác 1
                                        <input type="checkbox" id="selectallOther1" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkOther1" CssClass="chkAllOther1" runat="server" Checked='<%# Convert.ToBoolean(Eval("Other1")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Delete">
                                                <HeaderTemplate>
                                                    Chức năng khác 1
                                        <input type="checkbox" id="selectallOther2" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkOther2" CssClass="chkAllOther2" runat="server" Checked='<%# Convert.ToBoolean(Eval("Other2")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
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
