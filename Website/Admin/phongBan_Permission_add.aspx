﻿<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_phongBan_Permission_add" CodeBehind="PhongBan_Permission_Add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

    <script type="text/javascript">
        function validForm() {
            return true;
        }
    </script>

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
            <td style="width: 15%; text-align: right; padding-right: 10px">Chọn PermissionSchemeId
            </td>
            <td style="width: 20%; text-align: left">
                <asp:DropDownList ID="ddlPermissionSchemeId" runat="server">
                </asp:DropDownList>
            </td>
            <td style="width: 20%; text-align: left"></td>
            <td style="width: 15%"></td>
        </tr>
        <tr>
            <td style="height: 10px"></td>
        </tr>
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">Chọn PhongBanId
            </td>
            <td style="width: 20%; text-align: left">
                <asp:DropDownList ID="ddlPhongBanId" runat="server">
                </asp:DropDownList>
            </td>
            <td style="width: 20%; text-align: left"></td>
            <td style="width: 15%"></td>
        </tr>
        <tr>
            <td style="height: 10px"></td>
        </tr>
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">IsAllow
            </td>
            <td style="width: 20%; text-align: left">
                <asp:CheckBox ID="chkIsAllow" runat="server" Checked="false" Text="IsAllow" />
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
                <asp:Button ID="btSubmit" CssClass="button" runat="server" Text="Luu" OnClick="btSubmit_Click" OnClientClick="return validForm();" />
            </td>
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
</asp:Content>
