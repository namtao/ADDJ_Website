<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_loaiKhieuNai_VASUpdate_add" Title="Cập nhật loại dịch vụ VAS" CodeBehind="LoaiKhieuNai_VASUpdate_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script type="text/javascript">
        function validForm() {
            //txtName : string
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
    </script>
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnSubmit"  OnClientClick="return validForm();" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Cập nhật
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

                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 25px"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="4" align="left">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Lĩnh vực chung
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlParrent" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlParrent_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Mã dịch vụ
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtMaDichVu" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Tên dịch vụ
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtName" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Mô tả
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtDescription" runat="server" Text="" MaxLength="500" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Thứ tự hiển thị
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtSort" runat="server" Text="0" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Khoá dịch vụ
                        </td>
                        <td style="width: 20%; text-align: left">
                            <asp:CheckBox ID="chkIsDeleted" runat="server" />
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Ngày hết hạn
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">

                                    <asp:TextBox ID="txtNgayHetHan" runat="server"></asp:TextBox>

                                </div>
                            </div>
                        </td>
                        <td style="width: 20%; text-align: left"></td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="height: 25px"></td>
                    </tr>
                </table>

                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>

