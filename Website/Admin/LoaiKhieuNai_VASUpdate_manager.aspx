<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_loaiKhieuNai_VASUpdate_manager" Title="Quản lý loại dịch vụ VAS" CodeBehind="LoaiKhieuNai_VASUpdate_manager.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <style type="text/css">
                #contain {
                    position: static;
                }
            </style>
            <script type="javascript">
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
                    <li runat="server" id="liAdd">
                        <asp:LinkButton ID="linkbtnThemMoi" class="btn btn-primary" runat="server" OnClick="linkbtnThemMoi_Click">
                            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liUpdate">
                        <asp:LinkButton ID="linkbtnUpdate" class="btn btn-primary" runat="server" OnClick="linkbtnUpdate_Click">
                            <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDelete">
                        <asp:LinkButton ID="linkbtnDelete" OnClientClick="javascript:{return confirm('Bạn có muốn xóa dịch vụ được chọn?');}"  CssClass="btn btn-danger" runat="server" OnClick="linkbtnDelete_Click">
                            <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa
                        </asp:LinkButton>
                    </li>
                    <li runat="server" id="liDeleteCSDL">
                        <asp:LinkButton ID="linkbtnDeleteCSDL" OnClientClick="javascript:{return confirm('Bạn có muốn xóa dịch vụ được chọn khỏi csdl?');}" CssClass="btn btn-danger" runat="server" OnClick="linkbtnDeleteCSDL_Click">
                            <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span> Xóa CSDL
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
                                                            <strong>Loại dịch vụ:</strong>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <div class="selectstyle">
                                                                <div class="bg">
                                                                    <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="text-align: right;"><strong>Mã dịch vụ</strong> </td>
                                                        <td style="text-align: left">
                                                            <div class="inputstyle">
                                                                <div class="bg">
                                                                    <asp:TextBox ID="txtMaDichVuFilter" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <strong>Tên chứa:</strong>
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
                                                        <td style="text-align: right" width="30%">
                                                            <asp:Button ID="btClearFilter" runat="server" CssClass="button_clear_filter"
                                                                Text="Xóa điều kiện lọc" OnClick="btClearFilter_Click" />
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
                                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed" Width="100%"
                                        DataKeyNames="ID" BackColor="White" BorderColor="#16538C" BorderStyle="Solid"
                                        BorderWidth="1px" CellPadding="3" GridLines="Both" PageSize="50"
                                        AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                            <asp:BoundField DataField="MaDichVu" HeaderText="Mã dịch vụ" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Name" HeaderText="Tên dịch vụ" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CUser" HeaderText="Người tạo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CDate" HeaderText="Ngày tạo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0: dd/MM/yyyy hh:mm}" />
                                            <asp:BoundField DataField="LUser" HeaderText="Người cập nhật" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="LDate" HeaderText="Ngày cập nhật" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0: dd/MM/yyyy hh:mm}" />
                                            <asp:TemplateField HeaderText="Trạng thái đồng bộ">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsUpdate" Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsUpdate")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Khoá dịch vụ">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsDeleted" Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsDeleted")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NgayHetHan" HeaderText="Ngày khoá" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
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
                                <td style="padding-top: 10px">
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
                                                <asp:ListItem Value="30" />
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
