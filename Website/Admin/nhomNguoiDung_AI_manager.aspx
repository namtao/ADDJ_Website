<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_nhomNguoiDung_AI_manager" CodeBehind="NhomNguoiDung_AI_Manager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li runat="server" id="liUpdate">
                        <asp:LinkButton ID="linkbtnThemMoi" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDelete">
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
                            <tr valign="top">
                                <td>
                                    <table cellpadding="1" cellspacing="1" border="0" class="text">
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td style="height: 5px; text-align: left">
                                    <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr valign="top">
                                <td style="text-align: center">
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound"
                                        OnPageIndexChanging="grvView_PageIndexChanging">
                                      
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("Active")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CountNguoiSuDung" HeaderText="Số lượng người dùng" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Hành động" HeaderStyle-Width="320px">
                                                <ItemTemplate>
                                                    <%# BindHanhDong(Eval("Id")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div style="position: absolute; visibility: visible; border: none; z-index: 10000; top: 0px; left: 0px; width: 100%; height: 100%; background: #999; opacity: 0.4;">
                <div style="position: relative; min-height: 300px">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loader.gif" CssClass="loading_updatepanel" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
