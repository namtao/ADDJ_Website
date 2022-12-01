<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="admin_thongBao_add" Title="Chỉnh sửa thông tin thông báo" CodeBehind="ThongBao_Add.aspx.cs" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="Server">

    <script type="text/javascript">
        function validForm() {
            //txtTieuDe : nvarchar
            var name = $("#<%=txtTieuDe.ClientID %>");
            if (name.val().trim() == "") {
                name.val("");
                name.focus();
                $.messager.alert('Thông báo', 'Không để trống trường này', 'error', function () {
                    name.focus();
                });
                return false;
            }

            //txtNoiDung : nvarchar
            var name = $("#<%=txtNoiDung.ClientID %>");
        CKEDITOR.instances["FCKeditor1"];

        var oEditor = FCKeditorAPI.GetInstance('<%=txtNoiDung.ClientID %>');
        var oDOM = oEditor.EditorDocument;
        var strFCKEditorText = oDOM.body.innerText;
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
                <asp:LinkButton ID="linkbtnSubmit" class="btn btn-primary" runat="server" OnClick="linkbtnSubmit_Click">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span> Thêm mới
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Tiêu đề
                        </td>
                        <td colspan="2" style="width: 20%; text-align: left">
                            <asp:TextBox ID="txtTieuDe" runat="server" Text="" MaxLength="255" Width="450px"></asp:TextBox>
                            <asp:CheckBox ID="chkNew" Text="Đánh dấu tin nổi bật" runat="server" />
                        </td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Nội dung
                        </td>
                        <td colspan="2" style="width: 20%; text-align: left">
                            <CKEditor:CKEditorControl ID="txtNoiDung" BasePath="/Content/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        </td>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Display
                        </td>
                        <td style="width: 20%; text-align: left">
                            <asp:CheckBox ID="chkDisplay" runat="server" Checked="false" Text="Display" />
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
                            <%--    <a href="#1"><i class="save">&nbsp;</i><span><asp:Button ID="btSubmit" CssClass="button_eole"
                                    runat="server" Text="Cập nhật" OnClick="btSubmit_Click" /></span>

                                </a>--%>
                                <a href="javascript:history.back()"><i class="cancel">&nbsp;</i><span>Hủy bỏ</span></a>
                            </div>
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

                <!-- end body boot -->
            </div>
        </div>
        <!-- end panel boot -->
    </div>
</asp:Content>

