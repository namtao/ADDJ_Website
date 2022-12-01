<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DichVuCP_Add.aspx.cs"
    Inherits="Website.admin.DichVuCP_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <link href="/CSS/reset.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/style.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="Scripts/DichVuCP_Add.js" type="text/javascript"></script>
    <script type="text/javascript">
        var EditMode = '<%=strEditMode %>';
        function validForm() {
            //txtName : string
            var name = $("#<%=txtMaDichVu.ClientID %>");
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
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" value="" runat="server" id="hdnWindowUIMODE" />
        <div class="popup_Container">
            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">
                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                </h3>
                <span style="float: right;"><a href="#" onclick="DichVuCP.Edit().cancel()">
                    <img src="/Images/x.png">
                </a></span>
            </div>
            <div class="popup_Body">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: right; padding-right: 10px">ID dịch vụ
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtIdDichVu" Width="80px" runat="server" Text="" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Mã dịch vụ
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtMaDichVu" runat="server" Text="" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Ngày bắt đầu
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox runat="server" ID="txtNgayBatDau" CssClass="date-input"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Ngày kết thúc
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox runat="server" ID="txtNgayKetThuc" CssClass="date-input"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Deactive
                        </td>
                        <td style="text-align: left">
                            <asp:CheckBox runat="server" ID="chkDeactive"></asp:CheckBox>
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Ghi chú
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox runat="server" ID="txtGhiChu" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="foot_nav_btn">
                <div class="popup_Buttons">
                    <a href="#1"><i class="apply">&nbsp;</i><span>
                        <asp:Button ID="btnOkay" CssClass="button_eole" Text="Chọn" runat="server" OnClick="btSubmit_Click" OnClientClick="DichVuCP.Edit().okay()" />
                    </span></a>

                    <a href="#1"><i class="notapply">&nbsp;</i><span><input id="btnCancel" class="button_eole" value="Hủy bỏ" type="button" onclick="DichVuCP.Edit().cancel();" /></span></a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
