<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_nhomNguoiDung_AI_add" CodeBehind="NhomNguoiDung_AI_Add.aspx.cs" %>

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
                    return true;
                }
            </script>
            <!-- begin panel nav boot -->
            <div class="nav_btn_bootstrap">
                <ul>
                    <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
                    <li>
                        <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" OnClientClick="return validForm();" runat="server" OnClick="linkbtnSubmit_Click">
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Name
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtName" runat="server" Text="" MaxLength="50" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Description
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <div class="inputstyle">
                                        <div class="bg">
                                            <asp:TextBox ID="txtDescription" runat="server" Rows="3" TextMode="MultiLine" Text="" MaxLength="500" Width="450px"></asp:TextBox>
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
                                <td style="width: 15%; text-align: right; padding-right: 10px">Active
                                </td>
                                <td style="width: 20%; text-align: left">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" Text="Hoạt động" />
                                </td>
                                <td style="width: 20%; text-align: left"></td>
                                <td style="width: 15%"></td>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0"
        DynamicLayout="true">
        <ProgressTemplate>
            <div style="position: absolute; visibility: visible; border: none; z-index: 10000; top: 0px; left: 0px; width: 100%; height: 100%; background: #999; opacity: 0.4;">
                <div style="position: relative; min-height: 300px">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loader.gif" CssClass="loading_updatepanel" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
