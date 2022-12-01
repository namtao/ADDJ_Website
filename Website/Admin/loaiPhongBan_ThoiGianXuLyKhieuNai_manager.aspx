<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_loaiPhongBan_ThoiGianXuLyKhieuNai_manager"
    CodeBehind="LoaiPhongBan_ThoiGianXuLyKhieuNai_manager.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnUpdate" CssClass="btn btn-primary" runat="server" OnClick="linkbtnUpdate_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Cập nhật
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="linkbtnMultiUpdate" CssClass="btn btn-primary" runat="server" OnClick="linkbtnMultiUpdate_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Cập nhật nhiều loại phòng ban
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
                                                            <strong>Loại khiếu nại:</strong>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <div class="selectstyle">
                                                                <div class="bg">
                                                                    <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <strong>Loại phòng ban:</strong>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <div class="inputstyle">
                                                                <div class="bg">
                                                                    <asp:DropDownList ID="ddlLoaiPhongBan" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblLoaiPhongBan" runat="server" Height="150"></asp:CheckBoxList>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:Button ID="btFilter" runat="server" Text="Lọc" CssClass="btn_style_button" OnClick="btFilter_Click" />
                                                        </td>
                                                        <td style="text-align: right" width="30%">
                                                            <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" CssClass="button_clear_filter"
                                                                Text="Xóa điều kiện lọc" />
                                                        </td>
                                                    </tr>
                                                </table>
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
                            <tr valign="top">
                                <td style="text-align: center">

                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                                        DataKeyNames="ID" AllowPaging="false" OnRowDataBound="grvView_RowDataBound">
                                        <RowStyle CssClass="rowB" />
                                        <AlternatingRowStyle CssClass="rowA" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="LoaiKhieuNai_Name" HeaderText="Loại khiếu nại" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Thời gian cảnh báo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCanhBao" ToolTip='<%# Eval("ThoiGianCanhBao") %>' runat="server" Text='<%# Eval("ThoiGianCanhBao") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Thời gian cho phòng ban">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPhongBan" ToolTip='<%# Eval("ThoiGianUocTinh") %>' runat="server" Text='<%# Eval("ThoiGianUocTinh") %>'>></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LoaiKhieuNai_ThoiGianUocTinh" HeaderText="Thời gian cho toàn KN" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td>
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
                                                <asp:ListItem Value="30" />
                                                <asp:ListItem Value="50" />
                                                <asp:ListItem Value="100" />
                                            </asp:DropDownList>
                                            /&nbsp Tổng số:
                                    <asp:Literal ID="ltTongSoBanGhi" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </td>
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
