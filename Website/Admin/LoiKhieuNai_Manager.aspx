<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" CodeBehind="LoiKhieuNai_Manager.aspx.cs" Inherits="Website.admin.LoiKhieuNai_manager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .tbl_style th {
            line-height: 25px;
            padding-top: 7px;
            padding-bottom: 7px;
        }

        .tblempty {
            border: none;
        }

            .tblempty td {
                margin: 0px;
                padding: 0px;
                border: none;
                vertical-align: top;
            }

            .tblempty td {
                background-color: transparent;
            }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
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

        function NewExpanseOkay() {
            $('#<%=btFilter.ClientID %>').click();
            MessageAlert.AlertNormal('Thêm mới lỗi khiếu nại thành công', 'info');
        }

        function EditOkayScript() {
            $('#<%=btFilter.ClientID %>').click();
            MessageAlert.AlertNormal('Cập nhật lỗi khiếu nại thành công', 'info');
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
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>

                        <asp:LinkButton ID="linkbtnThemMoi" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                        <a href="#"><span class="new">
                            <div class="popup_Buttons" style="display: none">
                                <input id="btnOkay" value="Done" type="button" />
                                <input id="btnCancel" value="Cancel" type="button" />
                            </div>
                            <div id="Panel1" style="display: none;" class="popupConfirmation">
                                <iframe id="frameeditexpanse" frameborder="0" src="LoaiPhongBan_Add.aspx" width="400px" height="280px"
                                    scrolling="no"></iframe>
                            </div>
                        </a>
                    </li>
                    <li>
                        <asp:LinkButton ID="linkbtnUpdateHoatDong" class="btn btn-primary" runat="server" OnClick="linkbtnUpdateHoatDong_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật hoạt động
                        </asp:LinkButton>
                    </li>
                    <%--<li><a href="#"><span class="delete">
                        <asp:Button ID="Button2" runat="server" Text="Xóa" OnClick="btDelete_Click" Width="50px"
                            CssClass="button_eole" OnClientClick="javascript:{return confirm('Bạn có muốn xóa loại phòng ban được chọn?');}" />&nbsp;</span></a>
                    </li>--%>
                </ul>
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="udPanel1" runat="server" class="p8">
        <ContentTemplate>
            <asp:Panel runat="server" DefaultButton="btFilter">

                <!-- begin panel boot -->
                <div class="panel panel-default">
                    <div class="panel-heading"><span style="font-size: 12pt">Lọc tìm kiếm</span></div>
                    <div class="panel-body" style="border: none">
                        <!-- begin body boot -->

                        <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 10px;">
                            <tr>
                                <td style="width: 350px;">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            Ngày áp dụng
                                    <asp:DropDownList ID="ddlPhienBan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPhienBan_SelectedIndexChanged" Width="250"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 200px;">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtNameFilter" placeholder="Từ tìm kiếm" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:Button ID="btFilter" runat="server" Text="Lọc" CssClass="newbtn grad filter" OnClick="btFilter_Click" />
                                    <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" CssClass="newbtn grad rmfilter" Text="Bỏ lọc" />
                                </td>
                            </tr>
                        </table>
            </asp:Panel>
            <div class="msg" style="margin: 10px 0px;">
                <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
            </div>
            <asp:GridView ID="GrvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" DataKeyNames="Id" AllowPaging="True" AllowSorting="True" OnRowDataBound="GrvView_RowDataBound" OnPageIndexChanging="GrvView_PageIndexChanging" Width="100%">
                <AlternatingRowStyle CssClass="rowA" />
                <RowStyle CssClass="rowB" />
                <Columns>
                    <%--<asp:TemplateField HeaderText="Hoạt động">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHoatDong" Checked="<%=Eval('HoatDong') %>" CssClass="case" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tên lỗi">
                        <ItemTemplate>
                            <%# Eval("TenLoi") %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày áp dụng">
                        <ItemTemplate>
                            <%# Eval("TuNgay").ToString().Substring(6,2) + "/" + Eval("TuNgay").ToString().Substring(4,2) + "/" + Eval("TuNgay").ToString().Substring(0,4) + " đến " + Eval("DenNgay").ToString().Substring(6,2) + "/" + Eval("DenNgay").ToString().Substring(4,2) + "/" + Eval("DenNgay").ToString().Substring(0,4) %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Thứ tự" HeaderStyle-Width="115px">
                        <ItemTemplate>
                            <%# Eval("ThuTu") %>
                            <div style="float: right;">
                                <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Up" %>' OnClick="Unnamed_Click" runat="server" Text="Lên"></asp:LinkButton>
                                -
                                    <asp:LinkButton CommandArgument='<%# Eval("Id").ToString() + ";Down" %>' runat="server" Text="Xuống" OnClick="Unnamed_Click"></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Cap" HeaderText="Cấp" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" CssClass="center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Loai" HeaderText="Loại" ItemStyle-CssClass="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="center" HorizontalAlign="Left" Width="100px" />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="HoatDong" HeaderText="Hoạt động" ItemStyle-CssClass="center">
                        <ItemStyle CssClass="center" />
                    </asp:CheckBoxField>
                    <asp:TemplateField HeaderText="Hành động" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <%# HanhDong(Eval("Id")) %>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="center">
                        <HeaderTemplate>
                            Chọn
                             <input type="checkbox" id="selectall" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- end body boot -->
            </div>
                </div>
                <!-- end panel boot -->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btFilter" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btClearFilter" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="ButtonEdit" runat="server" Text="Edit Loại Phòng Ban" Style="display: none" />
    <cc1:ModalPopupExtender BackgroundCssClass="ModalPopupBG"
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

