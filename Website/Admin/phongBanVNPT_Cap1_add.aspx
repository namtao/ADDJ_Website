<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" CodeBehind="PhongBanVNPT_Cap1_Add.aspx.cs" Inherits="Website.admin.phongBanVNPT_Cap1_add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
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
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Tên phòng ban
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
                                            <asp:TextBox ID="txtDescription" Rows="5" TextMode="MultiLine" runat="server" Text="" MaxLength="500" Width="450px"></asp:TextBox>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
