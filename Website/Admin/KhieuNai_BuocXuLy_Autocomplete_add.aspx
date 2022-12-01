<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_khieuNai_BuocXuLy_Autocomplete_add" CodeBehind="KhieuNai_BuocXuLy_Autocomplete_add.aspx.cs" %>

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
                <asp:LinkButton ID="linkbtnSubmit" OnClientClick="return validForm();" 
                    CssClass="btn btn-danger" runat="server" OnClick="linkbtnSubmit_Click">
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Name
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
                        <td></td>
                        <td style="text-align: left">
                       <%--     <asp:Button ID="btSubmit" CssClass="button" runat="server" Text="Luu" OnClick="btSubmit_Click" OnClientClick="return validForm();" />
                       --%> </td>
                        <td></td>
                        <td></td>
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

