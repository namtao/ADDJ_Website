<%@ Page Language="C#" MasterPageFile="~/adminNotAJAX.master" AutoEventWireup="true"
    Inherits="admin_phongBanVNPT_add" Title="Untitled Page" CodeBehind="phongBanVNPT_add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script language="javascript">
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
            <div class="nav_btn">
                <ul>
                    <li><a href="javascript:QuayVe();"><span class="back">Quay về</span></a></li>
                    <li><a href="#"><span class="save">
                        <asp:Button ID="Button1" runat="server" CssClass="button_eole" Text="Cập nhật" OnClick="btSubmit_Click"
                            OnClientClick="return validForm();" />&nbsp;</span></a> </li>
                </ul>
            </div>
            <div class="p8">
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Loại phòng ban
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiPhongBan" runat="server">
                                    </asp:DropDownList>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Mã khu vực
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlKhuVuc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlKhuVuc_Changed">
                                    </asp:DropDownList>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Mã đối tác
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlDoiTac" runat="server">
                                        <asp:ListItem Text="Vinaphone" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">
                            Gửi khiếu nại đến
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlPhongBanDinhTuyen" runat="server">
                                    </asp:DropDownList>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Hình thức tiếp nhận mặc định khi tạo khiếu nại</td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlHTTiepNhan" runat="server">
                                    </asp:DropDownList>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px"></td>
                        <td style="width: 20%; text-align: left">
                            <asp:CheckBox ID="chkIsChuyenTiepKN" Text="Tự động chọn Chuyển tiếp khiếu nại" runat="server" />
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
            </div>
            <script></script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
