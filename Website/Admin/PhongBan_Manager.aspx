<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="Admin_PhongBan_Manager" Title="Quản lý phòng ban" CodeBehind="PhongBan_Manager.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        #contain {
            position: static;
        }

        .tmpsearch input {
            padding-left: 5px;
            padding-right: 5px;
        }

        .tbl_style.tblcus a {
            text-decoration: none;
        }

        .tbl_style.tblcus th {
            line-height: 150%;
            padding: 5px;
        }

        .button_clear_filter {
            background-position: 0 5px;
            text-decoration: none;
            color: blue;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="updatePnl" runat="server">
        <ContentTemplate>
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
            <script type="text/javascript">
                function ClearUI() {
                    $find("modalAddPhongBan").hide();
                }

                function cancel() {
                    $get('btnCancel').click();
                }
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li runat="server" id="liUpdate">
                        <asp:LinkButton ID="linkbtnThemMoi" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDelete">
                        <asp:LinkButton ID="linkbtnDelete" CssClass="btn btn-danger" OnClientClick="javascript:{return confirm('Bạn có muốn xóa loại khiếu nại được chọn?');}" runat="server" OnClick="linkbtnDelete_Click">
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

                        <asp:Panel runat="server" DefaultButton="btFilter">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="text-align: left; width: 200px;">
                                        <div class="selectstyle">
                                            <div class="bg">
                                                <asp:DropDownList ID="ddlDonVi" runat="server" Width="180px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="text-align: left; width: 200px;">
                                        <div class="selectstyle">
                                            <div class="bg">
                                                <asp:DropDownList ID="ddlDonVi2" runat="server" DataValueField="Id" DataTextField="MaDoiTac" Width="180px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="text-align: right; width: 200px;">
                                        <div class="selectstyle">
                                            <div class="bg">
                                                <asp:DropDownList ID="ddlLoaiPhongBan" runat="server" Width="180px">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="text-align: left; width: 160px;">
                                        <div class="inputstyle">
                                            <div class="bg tmpsearch">
                                                <asp:TextBox ID="txtKeyword" placeholder="Từ khóa" runat="server" Style="width: 150px;"> </asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="text-align: left; width: 180px;">
                                        <asp:Button ID="btFilter" runat="server" CssClass="newbtn grad filter" OnClick="btFilter_Click" Text="Lọc" />&nbsp;&nbsp;
                                <asp:Button ID="btnRemoveFilter" runat="server" CssClass="newbtn grad rmfilter" OnClick="btClearFilter_Click" Text="Bỏ lọc" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table cellpadding="0" cellspacing="0" style="width: 100%; margin: 10px 0px;">
                            <tr>
                                <td align="center" style="width: 50%; text-align: left">
                                    <asp:Literal ID="ltThongBao" runat="server"></asp:Literal>
                                </td>
                                <td align="right" style="width: 50%; text-align: right">
                                    <asp:Button ID="btAddPhongBanToPhongBan" runat="server" CssClass="btn_style_button" Text="Thêm phòng ban chuyển xử lý" />
                                    <cc2:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="ModalPopupBG"
                                        runat="server" CancelControlID="btnCancel" TargetControlID="btAddPhongBanToPhongBan"
                                        PopupControlID="pnSelectPhongBan" Drag="true" PopupDragHandleControlID="PopupHeader"
                                        BehaviorID="modalAddPhongBan">
                                    </cc2:ModalPopupExtender>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="GrvDatas" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                            DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="GrvDatas_RowDataBound"
                            OnPageIndexChanging="GrvDatas_PageIndexChanging">
                            <RowStyle CssClass="rowB" />
                            <AlternatingRowStyle CssClass="rowA" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Tên phòng ban" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Description" HeaderText="Mô tả" NullDisplayText="-" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Đối tác">
                                    <ItemTemplate>
                                        <%# BindDoiTac(Eval("DoiTacId")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="125px" HeaderText="Định tuyến KN">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsStatus" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("IsDinhTuyenKN")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="125px" HeaderText="Chuyển tiếp KN">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsChuyenTiepKN" runat="server" Enabled="false" Checked='<%# Convert.ToBoolean(Eval("IsChuyenTiepKN")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="125px" HeaderText="Hình thức tiếp nhận">
                                    <ItemTemplate>
                                        <%# BindHTTN(Eval("DefaultHTTN")) %>
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DS người dùng">
                                    <ItemTemplate>
                                        <%# BindCountNguoiDung(Eval("Id"), Eval("CountUser")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trạng thái">
                                    <ItemTemplate>
                                        <%# new Website.AppCode.PhongBanHelper().GetNameFromId( Eval("Status"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hành động" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# HanhDong(Eval("Id")) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
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

                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
            </div>
            <div id="pnSelectPhongBan" style="display: none;" class="popupConfirmation">
                <div class="popup_Container">
                    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                        <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Chọn phòng ban
                        </h3>
                        <span style="float: right;"><a href="javascript:cancel();">
                            <img alt="" src="/Images/x.png">
                        </a></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                    <div class="popup_Body">
                        <div class="infoBox">
                            Phòng ban được chọn ở đây khi chuyển khiếu nại
                            <br />
                            sẽ chuyển đến các phòng ban được chọn bên dưới.
                        </div>
                        <br />
                        Chọn phòng ban
                        <div class="selectstyle">
                            <div class="bg">
                                <asp:DropDownList ID="ddlPhongBanSelect" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                    </div>
                    <div class="foot_nav_btn">
                        <div class="popup_Buttons">
                            <a href="#1"><i class="apply">&nbsp;</i><span>
                                <asp:Button ID="btnOkay" Text="Chọn" CssClass="button_eole" runat="server" OnClick="btnOkay_Click" />
                            </span></a>
                            <a href="#1"><i class="notapply">&nbsp;</i><span><input id="btnCancel" class="button_eole" value="Hủy bỏ" type="button" /></span></a>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
