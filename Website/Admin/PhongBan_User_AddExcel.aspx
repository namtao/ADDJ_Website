<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="phongBan_User_AddExcel" CodeBehind="PhongBan_User_AddExcel.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script type="text/javascript">
        $(function () {

            // add multiple select / deselect functionality
            $("#selectallRead").click(function () {
                $('.chkAllRead').find("input").attr('checked', this.checked);
            });

            // if all checkbox are selected, check the selectall checkbox
            // and viceversa
            $(".chkAllRead").click(function () {

                if ($(".chkAllRead").find("input").length == $(".chkAllRead").find("input").length) {
                    $("#selectallRead").attr("checked", "checked");
                } else {
                    $("#selectallRead").removeAttr("checked");
                }

            });
        });
    </script>
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnCapNhat" class="btn btn-primary" runat="server" OnClick="linkbtnCapNhat_Click">
                      <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> Cập nhật
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
                        <td>
                            <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                                <tr>
                                    <td bgcolor="#f0f0f0" style="text-align: left">
                                        <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;">Chọn file danh sách và phòng ban</h3>
                                    </td>
                                </tr>
                                <tr style="background: #fffff0">
                                    <td>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td style="text-align: left; width: 150px;">
                                                    <strong>File danh sách:</strong>
                                                </td>
                                                <td style="text-align: left">
                                                    <input runat="server" id="fUpload" type="file" />
                                                    <asp:Label ForeColor="Red" Font-Bold="true" ID="Label1" runat="server" Text="  (*.csv)  "></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 150px;">Chọn phòng ban
                                                </td>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <asp:DropDownList ID="ddlPhongBan" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;"></td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btLayDanhSach" CssClass="btn_style_button" runat="server" Text="Lấy danh sách người dùng" OnClick="btLayDanhSach_Click" />
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
                        <td style="height: 5px; text-align: left; color: red">
                            <asp:Literal ID="lbMessage" runat="server"></asp:Literal>
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

        <table width="100%" cellpadding="0" cellspacing="0" border="0" id="tableResult" runat="server">
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td style="height: 5px; text-align: left; color: red">
                                <asp:Literal ID="ltThongBao" runat="server"></asp:Literal></td>
                            <td style="text-align: right">
                                <asp:Button ID="btHuy" Visible="false" CssClass="btn_style_button" runat="server" Text="Hủy" OnClick="btHuy_Click" />

                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr valign="top">
                <td style="text-align: center">
                    <asp:GridView ID="grvView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-striped table-condensed"
                        AllowPaging="false" AllowSorting="True" OnRowDataBound="grvView_RowDataBound">
                        <RowStyle CssClass="rowB" />
                        <AlternatingRowStyle CssClass="rowA" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Username" HeaderText="Tên truy cập"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField ItemStyle-Width="200px" DataField="Fullname" HeaderText="Tên đầy đủ"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField ItemStyle-Width="120px" DataField="Phone" HeaderText="Điện thoại"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="PhongBanName" HeaderText="Phòng ban"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Chọn" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    Chọn
                                            <input type="checkbox" id="selectallRead" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRead" CssClass="chkAllRead" runat="server" Checked="false" />
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
    </div>
</asp:Content>
