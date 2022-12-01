<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="Admin_PhongBan_Add" Title="Thêm / Sửa phòng ban" CodeBehind="PhongBan_Add.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function validForm() {
                    var name = $("#<%=txtName.ClientID %>");
                    if (name.val().trim() == "") {
                        name.val("");
                        name.focus();
                        $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                            name.focus();
                        });
                        return false;
                    }
                    return true;
                }

                function QuayVe() {
                    var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl"));
                    if (returnURL == "") {
                        history.back();
                    }
                    else {
                        window.location.href = returnURL;
                    }
                }
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
                    <%-- <li><a href="javascript:QuayVe();"><span class="back">Quay về</span></a></li>
                    --%>
                    <li>
                        <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server" OnClick="linkbtnSubmit_Click">
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

                        <div class="main-msg">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <table border="0" class="tbl_standard tbl_pb_add" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="col1">Loại phòng ban</td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlLoaiPhongBan" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Mã khu vực </td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlKhuVuc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlKhuVuc_Changed">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Mã đối tác </td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlDoiTac" runat="server">
                                                <asp:ListItem Text="Vinaphone" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Tên phòng ban </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="200" Text="" Width="450px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Mô tả </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" Text="" Width="450px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Trạng thái </td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlTrangThai" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Cấp phòng ban </td>
                                <td class="col2">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlCapPhongBan" runat="server">
                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">Thứ tự hiển thị </td>
                                <td class="col2">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtSort" runat="server" MaxLength="500" Text="0" Width="50px"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">&nbsp;</td>
                                <td class="col2">
                                    <asp:CheckBox ID="chkIsDinhTuyenKN" runat="server" Text="Tự động định tuyến khiếu nại" />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">&nbsp;</td>
                                <td class="col2">
                                    <div class="selectstyle selectstyle_longlx">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlHTTiepNhan" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">&nbsp;</td>
                                <td class="col2">
                                    <asp:CheckBox ID="chkIsChuyenTiepKN" runat="server" Text="Tự động chọn Chuyển tiếp khiếu nại" />
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
