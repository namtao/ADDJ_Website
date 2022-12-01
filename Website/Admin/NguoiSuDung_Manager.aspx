<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_nguoiSuDung_manager" CodeBehind="NguoiSuDung_Manager.aspx.cs" %>

<%@ Register Assembly="VnptNet.Control" Namespace="VnptNet.Control" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <style type="text/css">
                #contain {
                    position: static;
                }
            </style>
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
                $(document).ready(function () {
                    LoadJS();
                });
            </script>
            <script language="javascript" type="text/javascript">
                function ClearUI() {
                    $find("modalAddPhongBan").hide();
                }

                function ClearUI_Remove() {
                    $find("modalRemovePhongBan").hide();
                }

                function cancel() {
                    $get('btnCancel').click();
                }

                function cancel_Remove() {
                    $get('btnCancelRemove').click();
                }

            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li>
                        <a href="javascript:history.back()" class="btn btn-primary">
                            <span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về
                        </a>
                    </li>
                    <li runat="server" id="liUpdate">
                        <asp:LinkButton ID="linkbtnTrangThai" class="btn btn-primary" runat="server" OnClick="linkbtnTrangThai_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật hoạt động
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liUpdateToDoiTac">
                        <asp:LinkButton ID="linkbtnUpdateToDoiTac"  class="btn btn-primary" runat="server" OnClick="linkbtnUpdateToDoiTac_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật đối tác
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liUpdateToKTV">
                        <asp:LinkButton ID="linkbtnUpdateToKTV" class="btn btn-primary" runat="server" OnClick="linkbtnUpdateToKTV_Click">
                             <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật khai thác viên
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDelete"><a href="#">
                        <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger" OnClientClick="javascript:{return confirm('Bạn có muốn xóa người sử dụng được chọn?');}" runat="server" OnClick="linkbtnDelete_Click">
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
                                <td>
                                    <table border="0">
                                        <tr>
                                            <td style="text-align: left">
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlDonVi" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged" Width="150px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="text-align: left">
                                                <div class="inputstyle">
                                                    <div class="bg">
                                                        <asp:TextBox ID="txtKeyword" runat="server" placeholder="Tìm kiếm từ khóa" Style="width: 150px;"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlFilter" runat="server" Width="170px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="text-align: right">Phòng ban </td>
                                            <td style="text-align: left">
                                                <div class="selectstyle">
                                                    <div class="bg">
                                                        <asp:DropDownList ID="ddlPhongBan" runat="server" Width="200px">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Button ID="btFilter" runat="server" CssClass="newbtn grad filter" OnClick="btFilter_Click" Text="Lọc" />
                                                <asp:Button ID="btClearFilter" runat="server" CssClass="newbtn grad rmfilter" OnClick="btClearFilter_Click" Text="Bỏ lọc" />
                                            </td>
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
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td align="right" style="width: 50%; text-align: right">
                                                <asp:Button ID="btRemoveUserToPhongBan" CssClass="btn_style_button" runat="server" Text="Xóa người dùng khỏi phòng ban"
                                                    Width="220px" />
                                                <cc2:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="ModalPopupBG"
                                                    runat="server" CancelControlID="btnCancelRemove" TargetControlID="btRemoveUserToPhongBan"
                                                    PopupControlID="pnSelectRemovePhongBan" Drag="true" PopupDragHandleControlID="PopupHeader"
                                                    BehaviorID="modalRemovePhongBan">
                                                </cc2:ModalPopupExtender>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btAddUserToPhongBan" CssClass="btn_style_button" runat="server" Text="Thêm người dùng vào phòng ban"
                                            Width="220px" />
                                                <cc2:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG"
                                                    runat="server" CancelControlID="btnCancel" TargetControlID="btAddUserToPhongBan"
                                                    PopupControlID="pnSelectPhongBan" Drag="true" PopupDragHandleControlID="PopupHeader"
                                                    BehaviorID="modalAddPhongBan">
                                                </cc2:ModalPopupExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr valign="top">
                                <td style="text-align: center">
                                    <cc1:RepeaterPaging ID="rptView" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-bordered table-hover table-striped table-condensed" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                                <tbody>
                                                    <tr class="th" align="center" style="color: White; background-color: #2360A4; font-weight: bold;">
                                                        <th align="center" scope="col">STT
                                                        </th>
                                                        <th align="center" scope="col">Tên truy cập
                                                        </th>
                                                        <th align="center" scope="col">Tên đầy đủ
                                                        </th>
                                                        <th align="center" scope="col">Nhóm
                                                        </th>
                                                        <th align="center" scope="col">Tên đối tác
                                                        </th>
                                                        <th align="center" scope="col">Di động
                                                        </th>
                                                        <th align="center" scope="col">Hoạt động
                                                        </th>
                                                        <th align="center" scope="col">Chi tiết đăng nhập
                                                        </th>
                                                        <th align="center" scope="col">Hành động
                                                        </th>
                                                        <th align="center" scope="col">Phòng ban
                                                        </th>
                                                        <th scope="col">Chọn
                                                    <input type="checkbox" id="selectall">
                                                        </th>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="row_odd">
                                                <asp:HiddenField ID="hdId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                <asp:HiddenField ID="hdTrangThai" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "TrangThai") %>' />
                                                <td align="center" style="width: 5%;">
                                                    <%# (Container.ItemIndex + 1) + ((pageIndex-1)*pageSize) %>
                                                </td>
                                                <td align="left">
                                                    <a href='nguoiSuDung_add.aspx?ID=<%#DataBinder.Eval(Container.DataItem, "Id")%>'>
                                                        <%#DataBinder.Eval(Container.DataItem, "TenTruyCap")%></a>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "TenDayDu")%>
                                                </td>
                                                <td align="center">
                                                    <%# BindNhomNguoiDung(DataBinder.Eval(Container.DataItem, "NhomNguoiDung"))%>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "TenDoiTac")%>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "DiDong")%>
                                                </td>
                                                <td align="center">
                                                    <%--<%# BindTrangThai(DataBinder.Eval(Container.DataItem, "TrangThai"))%>--%>
                                                    <asp:CheckBox ID="chkUpdateTrangThai" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "TrangThai")) %>' />
                                                </td>
                                                <td align="left">Count:
                                            <%#DataBinder.Eval(Container.DataItem, "LoginCount")%><br />
                                                    Last:
                                            <%#DataBinder.Eval(Container.DataItem, "LastLogin")%>
                                                </td>
                                                <td style="text-align: left">
                                                    <%# BindHanhDong(DataBinder.Eval(Container.DataItem, "Id"))%>                                            
                                                </td>
                                                <td style="text-align: left">
                                                    <%# DataBinder.Eval(Container.DataItem, "PhongBan_Name") %>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="row_even">
                                                <asp:HiddenField ID="hdId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                <asp:HiddenField ID="hdTrangThai" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "TrangThai") %>' />
                                                <td align="center" style="width: 5%;">
                                                    <%# (Container.ItemIndex + 1) + ((pageIndex-1)*pageSize) %>
                                                </td>
                                                <td align="left">
                                                    <a href='nguoiSuDung_add.aspx?ID=<%#DataBinder.Eval(Container.DataItem, "Id")%>'>
                                                        <%#DataBinder.Eval(Container.DataItem, "TenTruyCap")%></a>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "TenDayDu")%>
                                                </td>
                                                <td align="center">
                                                    <%# BindNhomNguoiDung(DataBinder.Eval(Container.DataItem, "NhomNguoiDung"))%>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "TenDoiTac")%>
                                                </td>
                                                <td align="left">
                                                    <%#DataBinder.Eval(Container.DataItem, "DiDong")%>
                                                </td>
                                                <td align="center">
                                                    <%--<%# BindTrangThai(DataBinder.Eval(Container.DataItem, "TrangThai"))%>--%>
                                                    <asp:CheckBox ID="chkUpdateTrangThai" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "TrangThai")) %>' />
                                                </td>
                                                <td align="left">Count:
                                            <%#DataBinder.Eval(Container.DataItem, "LoginCount")%><br />
                                                    Last:
                                            <%#DataBinder.Eval(Container.DataItem, "LastLogin")%>
                                                </td>
                                                <td style="text-align: left">
                                                    <%# BindHanhDong(DataBinder.Eval(Container.DataItem, "Id"))%>                                            
                                                </td>
                                                <td style="text-align: left">
                                                    <%# DataBinder.Eval(Container.DataItem, "PhongBan_Name") %>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbSelectAll" CssClass="case" runat="server" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </cc1:RepeaterPaging>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <!--Start Phân trang AJAX-->
                                    <div class="DDPager">
                                        <span style="float: left">
                                            <asp:ImageButton AlternateText="Trang đầu tiên" ToolTip="Trang đầu tiên" ID="ImageButtonFirst"
                                                runat="server" ImageUrl="~/admin/Images/PgFirst.gif" Width="8" Height="9" OnClick="ImageButtonFirst_Click" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang trước" ToolTip="Trang trước" ID="ImageButtonPrev"
                                        runat="server" ImageUrl="~/admin/Images/PgPrev.gif" Width="5" Height="9" OnClick="ImageButtonPrev_Click" />
                                            &nbsp;
                                    <asp:Label ID="LabelPage" runat="server" Text="Trang " AssociatedControlID="TextBoxPage" />
                                            <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" OnTextChanged="TextBoxPage_TextChanged"
                                                Width="20px" CssClass="DDControl" />
                                            /
                                    <asp:Label ID="LabelNumberOfPages" runat="server" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang tiếp theo" ToolTip="Trang tiếp theo" ID="ImageButtonNext"
                                        runat="server" ImageUrl="~/admin/Images/PgNext.gif" Width="5" Height="9" OnClick="ImageButtonNext_Click" />
                                            &nbsp;
                                    <asp:ImageButton AlternateText="Trang cuối cùng" ToolTip="Trang cuối cùng" ID="ImageButtonLast"
                                        runat="server" ImageUrl="~/admin/Images/PgLast.gif" Width="8" Height="9" OnClick="ImageButtonLast_Click" />
                                        </span><span style="float: right">
                                            <asp:Label ID="LabelRows" runat="server" Text="Kết quả trên 1 trang:" AssociatedControlID="DropDownListPageSize" />
                                            <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="DDControl"
                                                OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
                                                <asp:ListItem Value="10" />
                                                <asp:ListItem Value="15" />
                                                <asp:ListItem Value="20" />
                                                <asp:ListItem Value="30" />
                                                <asp:ListItem Value="40" />
                                                <asp:ListItem Value="50" />
                                                <asp:ListItem Value="100" />
                                            </asp:DropDownList>
                                            /&nbsp Tổng số:
                                    <asp:Literal ID="ltTongSoBanGhi" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <!--End Phân trang AJAX-->
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 30px"></td>
                            </tr>
                        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
            <!--Start Panel Popup-->
            <div id="pnSelectPhongBan" style="display: none;" class="popupConfirmation">
                <div class="popup_Container">
                    <div class="popup_Titlebar" id="PopupHeader">
                        <div class="TitlebarLeft">
                            Chọn phòng ban thêm người dùng
                        </div>
                        <div class="TitlebarRight" onclick="cancel();">
                        </div>
                    </div>
                    <div class="popup_Body">
                        Chọn phòng ban
                        <asp:DropDownList ID="ddlPhongBanSelect" Width="200px" runat="server">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="popup_Buttons">
                        <asp:Button ID="btnOkay" Text="Chọn" runat="server" OnClick="btnOkay_Click" />
                        <input id="btnCancel" value="Hủy" type="button" />
                    </div>
                </div>
            </div>
            <div id="pnSelectRemovePhongBan" style="display: none; width: 300px;" class="popupConfirmation">
                <div class="popup_Container">
                    <div class="popup_Titlebar" id="Div2">
                        <div class="TitlebarLeft">
                            Chọn phòng ban xóa người dùng
                        </div>
                        <div class="TitlebarRight" onclick="cancel_Remove();">
                        </div>
                    </div>
                    <div class="popup_Body">
                        Chọn phòng ban
                        <asp:DropDownList ID="ddlPhongBanSelectRemove" runat="server">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="popup_Buttons">
                        <asp:Button ID="btnOkayRemove" Text="Chọn" runat="server" OnClick="btnOkayRemove_Click" />
                        <input id="btnCancelRemove" value="Hủy" type="button" />
                    </div>
                </div>
            </div>
            <!--End Panel Popup-->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btFilter" EventName="Click" />
        </Triggers>
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
