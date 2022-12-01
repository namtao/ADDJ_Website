<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_congTyDoiTac_add" CodeBehind="CongTyDoiTac_Add.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .colName {
            width: 150px;
        }

        .width300 {
            display: inline-block;
            width: 300px;
        }

        .main-mesage {
            padding-bottom: 5px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
            <li>
                <asp:LinkButton ID="linkbtnSubmit" OnClientClick="return validForm();" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
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
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="colName">&nbsp;</td>
                        <td class="main-mesage">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="colName">Tên
                    <asp:RequiredFieldValidator ValidationGroup="A1" Text="(*)" CssClass="aws-red" Display="Static" ControlToValidate="txtTen" runat="server" ErrorMessage="Tên công ty"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="inputstyle width300">
                                <div class="bg">
                                    <asp:TextBox ID="txtTen" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="colName">DiaChi
                        </td>
                        <td>
                            <div class="inputstyle width300">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaChi" runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td class="colName">Điện thoại, fax
                        </td>
                        <td>
                            <div class="inputstyle width300">
                                <div class="bg">
                                    <asp:TextBox ID="txtDienThoai_Fax" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="colName">Website
                        </td>
                        <td>
                            <div class="inputstyle width300">
                                <div class="bg">
                                    <asp:TextBox ID="txtWebsite" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="colName width300">Hỗ trợ khách hàng
                        </td>
                        <td>
                            <div class="inputstyle width300">
                                <div class="bg">
                                    <asp:TextBox ID="txtHoTroKhachHang" runat="server"></asp:TextBox>
                                </div>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td class="colName">Sử dụng</td>
                        <td>
                            <asp:CheckBox ID="chkTrangThai" CssClass="cbx-main" runat="server" />
                        </td>
                    </tr>
                  <%--  <tr>
                        <td class="colName">
                            <asp:ValidationSummary runat="server" ValidationGroup="A1" ShowMessageBox="true" ShowSummary="false" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnUpdate" ValidationGroup="A1" CssClass="btn_main save" Text="Cập nhật" OnClick="btSubmit_Click" />
                            <asp:Button runat="server" ID="btnCancel" OnClientClick="document.location.href='CongTyDoiTac_Manager.aspx'; return false;" CssClass="btn_main cancel" Text="Hủy bỏ" />
                        </td>
                    </tr>--%>
                </table>
                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>


