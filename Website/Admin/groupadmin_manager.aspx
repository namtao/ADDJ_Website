<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_groupadmin_manager" CodeBehind="Groupadmin_Manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">
        $(function () {
            // add multiple select / deselect functionality
            $("#selectall").click(function () {
                $('.case').find("input").attr('checked', this.checked);
            });
            // if all checkbox are selected, check the selectall checkbox
            // and viceversa
            $(".case").click(function () {
                if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                    $("#selectall").attr("checked", "checked");
                } else {
                    $("#selectall").removeAttr("checked");
                }
            });
        });
    </script>
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnThemMoi" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                </asp:LinkButton>
            </li>
            <li>
                <asp:LinkButton ID="linkbtnUpdate" class="btn btn-primary" runat="server" OnClick="linkbtnUpdate_Click">
                    <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật
                </asp:LinkButton>
            </li>
            <li>
                <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger" OnClientClick="javascript:{return confirm('Bạn có muốn xóa nhóm người dùng được chọn?');}" runat="server" OnClick="linkbtnDelete_Click">
                    <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa
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
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr valign="top">
                        <td style="text-align: center">
                            <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound"
                                OnPageIndexChanging="grvView_PageIndexChanging">
                                <RowStyle CssClass="rowB" />
                                <AlternatingRowStyle CssClass="rowA" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Tên nhóm" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField ItemStyle-Width="125px" HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IsStatus" runat="server" Checked='<%# Convert.ToBoolean(Eval("Status")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="125px" HeaderText="Hành động">
                                        <ItemTemplate>
                                            <%# HanhDong(Eval("Id")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="125px">
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
</asp:Content>
