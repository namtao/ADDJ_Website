<%@ Page Language="C#" MasterPageFile="~/admin/adminNotAJAX.master" AutoEventWireup="true"
    Inherits="admin_news_add" Title="Untitled Page" CodeBehind="news_add.aspx.cs" %>

<%@ Register Namespace="CKEditor.NET" Assembly="CKEditor.NET" TagPrefix="ck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

    <script language="javascript">
        function validForm() {
            var name = document.getElementById("ctl00_ContentPlaceHolder_Main_txtName");
            if (name.value.trim() == "") {
                alert('Error');
                name.value = "";
                name.focus();
                return false;
            }
            return true;
        }
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 25px">
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="4" align="left">
                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>   
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">
                Chủ đề 
            </td>
            <td style="width: 20%; text-align: left">
                <asp:DropDownList ID="ddlCategoryNews" runat="server">
                </asp:DropDownList>
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 15%">
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">
                Tiêu đề
            </td>
            <td style="width: 20%; text-align: left">
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Width="450px"></asp:TextBox>
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 15%">
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">
                Mô tả
            </td>
            <td style="width: 20%; text-align: left">
                <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" Width="450px" TextMode="MultiLine"
                    Rows="5"></asp:TextBox>
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 15%">
            </td>
        </tr>        
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">
                Image
            </td>
            <td style="width: 20%; text-align: left">            
                <asp:Image ID="imgOld" runat="server" />
                <br />
                <input runat="server" id="fImage" type="file" />
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 15%">
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td style="width: 15%; text-align: right; padding-right: 10px">
                Nội dung
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 20%; text-align: left">
            </td>
            <td style="width: 15%">
            </td>
        </tr>
        <tr>
            <td colspan="4" style="width: 15%; text-align: right; padding-right: 10px">
                <ck:CKEditorControl runat="server" BasePath="../Content/ckeditor" ID="txtContent"
                    FilebrowserBrowseUrl="../Content/ckfinder/ckfinder.html" Height="400px" Width="96%"></ck:CKEditorControl>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>        
        <tr>
            <td colspan="2">
                <asp:Button ID="btSubmit" CssClass="button" runat="server" Text="Lưu" OnClick="btSubmit_Click" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td style="height: 25px">
            </td>
        </tr>
    </table>
</asp:Content>
