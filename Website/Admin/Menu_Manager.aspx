<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_menu_manager" CodeBehind="Menu_Manager.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script type="text/javascript">
        $(function () {
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
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .td-main {
            padding: 5px 0px;
        }

        .col_center {
            text-align: center;
        }

        .td-main a {
            color: #034AFC;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
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
                    <span class="glyphicon glyphicon-floppy-saved" aria-hidden="true"></span> Cập nhật Display
                </asp:LinkButton>
            </li>
            <li>
                <asp:LinkButton ID="linkbtnDelete" OnClientClick="javascript:{return confirm('Bạn có muốn xóa menu được chọn?');}" CssClass="btn btn-danger" runat="server" OnClick="linkbtnDelete_Click">
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

                <table class="tblstyle tbl_cus" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="padding-top: 10px;">
                            <asp:Literal ID="ltThongBao" runat="server"></asp:Literal></td>
                        <td style="width: 200px">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlParentId" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="width: 200px">
                            <div class="selectstyle" style="margin-right: -5px;">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLevel" runat="server">
                                        <asp:ListItem Text="-- Cấp hiển thị --" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Cấp 1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Cấp 2" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="td-main">
                            <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" Width="100%" DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound"
                                OnPageIndexChanging="grvView_PageIndexChanging">
                                <RowStyle CssClass="rowB" />
                                <AlternatingRowStyle CssClass="rowA" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Tên menu" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Link" HeaderText="Link" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Sắp xếp">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("STT") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <span><%# Eval("STT") %></span>
                                            <div style="float: right;">
                                                <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Up" %>' OnClick="Unnamed_Click" runat="server" Text="Lên"></asp:LinkButton>
                                                -
                                        <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Down" %>' runat="server" Text="Xuống" OnClick="Unnamed_Click"></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hiển thị" ItemStyle-CssClass="col_center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Display" runat="server" Checked='<%# Convert.ToBoolean(Eval("Display")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col_center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kiểu" ItemStyle-CssClass="col_center">
                                        <ItemTemplate>
                                            <%# GetMenuType(Eval("MenuType")) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col_center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chọn">
                                        <HeaderTemplate>
                                            <input type="checkbox" id="selectall" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col_center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="font-weight: bold; color: maroon; font-style: italic">Hiện không có menu con</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>
