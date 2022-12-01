<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_loaiPhongBan_manager" CodeBehind="LoaiPhongBan_manager.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script type="text/javascript">
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

        function ShowEditModal(LoaiPhongBanId) {
            var frame = $get('IframeEdit');
            frame.src = "/admin/LoaiPhongBan_Add.aspx?UIMODE=EDIT&ID=" + LoaiPhongBanId;
            $find('EditModalPopup').show();
        }

        function ShowCopyModal(LoaiPhongBanId) {
            var frame = $get('IframeEdit');
            frame.src = "/admin/LoaiPhongBan_Add.aspx?UIMODE=COPY&ID=" + LoaiPhongBanId;
            $find('EditModalPopup').show();
        }

        function NewExpanseOkay() {
            $('#<%=btFilter.ClientID %>').click();
            MessageAlert.AlertNormal('Thêm mới loại phòng ban thành công', 'info');
        }

        function EditOkayScript() {
            $('#<%=btFilter.ClientID %>').click();
            MessageAlert.AlertNormal('Cập nhật loại phòng ban thành công', 'info');
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
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <li><a href="#"><span class="new">
                        <asp:LinkButton ID="btAdd" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                        <cc1:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG"
                            runat="server" CancelControlID="btnCancel" OkControlID="btnOkay" TargetControlID="btAdd"
                            PopupControlID="Panel1" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="NewExpanseOkay();">
                        </cc1:ModalPopupExtender>
                        <div class="popup_Buttons" style="display: none">
                            <input id="btnOkay" value="Done" type="button" />
                            <input id="btnCancel" value="Cancel" type="button" />
                        </div>
                        <div id="Panel1" style="display: none;" class="popupConfirmation">
                            <iframe id="frameeditexpanse" frameborder="0" src="LoaiPhongBan_Add.aspx" width="400px" height="280px"
                                scrolling="no"></iframe>
                        </div>
                    </a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger" OnClientClick="javascript:{return confirm('Bạn có muốn xóa loại phòng ban được chọn?');}" runat="server" OnClick="linkbtnDelete_Click">
                            <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa
                        </asp:LinkButton>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                                        <tr>
                                            <td bgcolor="#f0f0f0" style="text-align: left">
                                                <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;">Lọc tìm kiếm</h3>
                                            </td>
                                        </tr>
                                        <tr style="background: #fffff0">
                                            <td>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <strong>Tên loại phòng ban:</strong>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <div class="inputstyle">
                                                                <div class="bg">
                                                                    <asp:TextBox ID="txtNameFilter" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:Button ID="btFilter" runat="server" Text="Lọc" CssClass="btn_style_button" OnClick="btFilter_Click" />
                                                        </td>
                                                        <td style="text-align: right" width="50%">
                                                            <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" CssClass="button_clear_filter"
                                                                Text="Xóa điều kiện lọc" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="grvView_RowDataBound"
                                        OnPageIndexChanging="grvView_PageIndexChanging">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Tên loại phòng" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Description" HeaderText="Mô tả" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ThoiGianXuLyMacDinh" HeaderText="Thời gian xử lý mặc định" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="LoaiDuLieu" HeaderText="Loại dữ liệu" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Phòng ban">
                                                <ItemTemplate>
                                                    <%# BindCountPhongBan(Eval("Id"), Eval("CountPhongBan")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hành động">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="false" CommandName=""
                                                        Text="Sửa"></asp:LinkButton>
                                                    |
                                            <asp:LinkButton ID="lnkCopy" runat="server" CausesValidation="false" CommandName=""
                                                Text="Sao chép"></asp:LinkButton>
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
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btClearFilter" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
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
    <asp:Button ID="ButtonEdit" runat="server" Text="Edit Loại Phòng Ban" Style="display: none" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="ModalPopupBG"
        runat="server" CancelControlID="btnCancelEdit" OkControlID="btnOkayEdit" TargetControlID="ButtonEdit"
        PopupControlID="DivEditWindow" OnOkScript="EditOkayScript();"
        BehaviorID="EditModalPopup">
    </cc1:ModalPopupExtender>
    <div class="popup_Buttons" style="display: none">
        <input id="btnOkayEdit" value="Done" type="button" />
        <input id="btnCancelEdit" value="Cancel" type="button" />
    </div>
    <div id="DivEditWindow" style="display: none;" class="popupConfirmation">
        <iframe id="IframeEdit" src="" frameborder="0" width="400px" height="280px" scrolling="no"></iframe>
    </div>
</asp:Content>
