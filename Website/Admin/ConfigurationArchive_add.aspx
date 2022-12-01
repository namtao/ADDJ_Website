<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="ConfigurationArchive_add" CodeBehind="ConfigurationArchive_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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

                    //txtServerName : nvarchar
                    var name = $("#<%=txtServerName.ClientID %>");
                    if (name.val().trim() == "") {
                        name.val("");
                        name.focus();
                        $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                            name.focus();
                        });
                        return false;
                    }

                    //txtDatabaseName : nvarchar
                    var name = $("#<%=txtDatabaseName.ClientID %>");
                if (name.val().trim() == "") {
                    name.val("");
                    name.focus();
                    $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                        name.focus();
                    });
                    return false;
                }

                //txtUsername : nvarchar
                var name = $("#<%=txtUsername.ClientID %>");
                if (name.val().trim() == "") {
                    name.val("");
                    name.focus();
                    $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                        name.focus();
                    });
                    return false;
                }

                //txtPassword : nvarchar
                var name = $("#<%=txtPassword.ClientID %>");
                if (name.val().trim() == "") {
                    name.val("");
                    name.focus();
                    $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                        name.focus();
                    });
                    return false;
                }

                //txtPathFileSystem : nvarchar
                var name = $("#<%=txtPathFileSystem.ClientID %>");
                if (name.val().trim() == "") {
                    name.val("");
                    name.focus();
                    $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                        name.focus();
                    });
                    return false;
                }

                //txtPathUrlFile : nvarchar
                var name = $("#<%=txtPathUrlFile.ClientID %>");
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
                        <asp:LinkButton ID="linkSubmit"  CssClass="btn btn-primary"  OnClientClick="return validForm();" runat="server" OnClick="linkSubmit_Click">
                             <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
                        </asp:LinkButton>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Năm lưu trữ
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="selectstyle">
                                        <div class="bg">
                                            <asp:DropDownList ID="ddlNam" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 20%; text-align: left"></td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td style="height: 5px"></td>
                            </tr>
                            <tr>
                                <td style="width: 15%; text-align: right; padding-right: 10px">Tên Archive
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
                                <td style="width: 15%; text-align: right; padding-right: 10px; vertical-align: top;">Mô tả
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDescription" runat="server" Text="" TextMode="MultiLine" Rows="2" MaxLength="500" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Tên server
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtServerName" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Tên database
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDatabaseName" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Tên truy cập
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtUsername" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Mật khẩu
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPassword" runat="server" Text="" MaxLength="200" Width="450px"></asp:TextBox>
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
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td style="width: 15%; text-align: right; padding-right: 10px">Đường dẫn lưu trữ file
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPathFileSystem" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Đường dẫn download file
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtPathUrlFile" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
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
                                <td></td>
                                <td style="text-align: left">
                                    <div class="foot_nav_btn">
                                        <a href="#1"><i class="save">&nbsp;</i><span><asp:Button ID="btSubmit" CssClass="button_eole"
                                            runat="server" Text="Cập nhật" OnClick="btSubmit_Click" OnClientClick="return validForm();" /></span></a>
                                        <a href="javascript:history.back()"><i class="cancel">&nbsp;</i><span>Hủy bỏ</span></a>
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="height: 25px"></td>
                            </tr>
                        </table>
                        <!-- end body boot -->
                    </div>
                </div>
                <!-- end panel boot -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
