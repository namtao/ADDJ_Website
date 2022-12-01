<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_phongBanVNPT_manager" CodeBehind="PhongBanVNPT_Manager.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <style type="text/css">
                #contain {
                    position: static;
                }
            </style>
            <script language="javascript" type="text/javascript">
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
                    <%--<li runat="server" id="li2"><a href="#"><span class="new">
                        <asp:Button ID="Button2" runat="server" CssClass="button_eole" Text="Thêm mới phòng ban cấp 1" OnClick="btThemMoiC1_Click" />&nbsp;</span></a>
                    </li>--%>
                    <li runat="server" id="liUpdate">
                        <asp:LinkButton ID="linkbtnThemMoiC2" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoiC2_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới phòng ban cấp 2
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="li1">
                        <asp:LinkButton ID="linkbtnThemMoiC3" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoiC3_Click">
                             <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới phòng ban cấp 3
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDelete">
                        <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger" runat="server" OnClientClick="javascript:{return confirm('Bạn có muốn xóa phòng ban được chọn?');}">
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
                            <tr style="height: 5px;">
                                <td></td>
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
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:TemplateField HeaderText="Đối tác">
                                                <ItemTemplate>
                                                    <%# BindDoiTac(Eval("DoiTacId")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loại phòng ban">
                                                <ItemTemplate>
                                                    <%# BindLoaiPhongBan(Eval("LoaiPhongBanId")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Tên phòng ban" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Description" HeaderText="Mô tả" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="125px" HeaderText="Chuyển tiếp KN VNP">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="IsChuyenTiepKN" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("IsChuyenVNP")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="125px" HeaderText="Hình thức tiếp nhận">
                                                <ItemTemplate>
                                                    <%# BindHTTN(Eval("DefaultHTTN")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Danh sách người dùng">
                                                <ItemTemplate>
                                                    <%# BindCountNguoiDung(Eval("Id"), Eval("CountUser")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hành động" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# BindHanhDong(Eval("Id"), Eval("Cap")) %>
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
</asp:Content>
