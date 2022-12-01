<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_groupadmin_add" CodeBehind="Groupadmin_Add.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <script type="text/javascript">
        function validForm() {
            var name = document.getElementById("<%=txtName.ClientID %>");
            if (name.value.trim() == "") {
                alert('Tên nhóm không được trống');
                name.value = "";
                name.focus();
                return false;
            }
            return true;
        }
    </script>
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span> Quay về</a></li>
            <li>                
                <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server" OnClick="linkbtnSubmit_Click">
                    <span class="glyphicon glyphicon-floppy-saved" aria-hidden="true"></span> Cập nhật
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
                        <td style="width: 30%"></td>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Tên nhóm
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="180px"></asp:TextBox>
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
                        <td style="width: 30%"></td>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Mô tả
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Height="30" runat="server" MaxLength="50"
                                        Width="180px"></asp:TextBox>
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
                        <td style="width: 30%"></td>
                        <td style="width: 15%; text-align: right"></td>
                        <td style="width: 20%; text-align: left;">
                            <asp:CheckBox ID="chkStatus" runat="server" Text="Active" Checked="true" />
                        </td>
                        <td style="width: 20%"></td>
                        <td style="width: 15%"></td>
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
</asp:Content>
