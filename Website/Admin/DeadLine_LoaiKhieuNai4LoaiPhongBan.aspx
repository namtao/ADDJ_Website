<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeadLine_LoaiKhieuNai4LoaiPhongBan.aspx.cs"
    Inherits="Website.admin.DeadLine_LoaiKhieuNai4LoaiPhongBan" %>

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
    <style type="text/css">
        #contain {
            position: static;
        }
    </style>
    <script type="text/javascript">
        var EditMode = '<%=strEditMode %>';

        function okay() {
            //var UIMODE = $('#hdnWindowUIMODE').value;
            if (EditMode == "EDIT")
                window.parent.document.getElementById('btnOkayEdit').click();
            else {
                window.parent.document.getElementById('btnOkay').click();
            }
        }

        function cancel() {
            if (EditMode == "EDIT")
                window.parent.document.getElementById('btnCancelEdit').click();
            else
                window.parent.document.getElementById('btnCancel').click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" value="" runat="server" id="hdnWindowUIMODE" />
        <div class="popup_Container">
            <div class="popup_Titlebar" id="PopupHeader">
                <div class="TitlebarLeft">
                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                </div>
                <div class="TitlebarRight" onclick="cancel();">
                </div>
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
                        <td style="width: 15%; text-align: right; padding-right: 10px">Loại phòng ban
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiPhongBanSelect" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Loại khiếu nại
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiKhieuNaiSelect" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Thời gian cảnh báo
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianCanhBao" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: right; padding-right: 10px">Thời gian ước tính
                        </td>
                        <td style="width: 20%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianUocTinh" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px"></td>
                    </tr>
                </table>
                <div class="popup_Buttons">
                    <asp:Button ID="btnOkay" CssClass="btn_style_button" Text="Chọn" runat="server" OnClick="btSubmit_Click" />
                    <input id="btnCancel" class="btn_style_button" value="Hủy bỏ" type="button" onclick="cancel();" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
