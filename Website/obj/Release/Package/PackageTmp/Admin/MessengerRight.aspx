<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true"
    Inherits="admin_MessengerRight"  CodeBehind="MessengerRight.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div class="p10">
        <div class="boxinfor">
            <div class="titlebox">
                <h4>
                    Có lỗi</h4>
            </div>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #ffffff">
                <tr valign="top">
                    <td style="padding-left: 10px; padding-right: 10px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left">
                            <tr>
                                <td style="height: 20px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="Bạn không có quyền thực hiện chức năng này."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px">
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: middle; height: 25px">
                                    <asp:HyperLink ID="Hyperlink2" runat="server" CssClass="ms-toolbar" NavigateUrl="javascript:history.go(-1);">
									<img alt="Trở lại trang cũ" src="Images/back.gif" height="16" width="16" border="0" />																
									                Trở lại trang cũ
                                    </asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 50px">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
