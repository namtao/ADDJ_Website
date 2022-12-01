<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoaiPhongBan_Add.aspx.cs"
    Inherits="Website.admin.LoaiPhongBan_Add" %>

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

        function okay() {
            if (EditMode == "EDIT")
                window.parent.document.getElementById('btnOkayEdit').click();
            else if (EditMode == "COPY")
                window.parent.document.getElementById('btnOkayEdit').click();
            else {
                window.parent.document.getElementById('btnOkay').click();
            }
        }

        function cancel() {
            if (EditMode == "EDIT")
                window.parent.document.getElementById('btnCancelEdit').click();
            else if (EditMode == "COPY")
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


            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">
                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                </h3>
                <span style="float: right;"><a href="javascript:cancel();">
                    <img src="/Images/x.png">
                </a></span>
            </div>

            <div class="popup_Body">
                <table border="0" cellpadding="0" cellspacing="0" height="150px" width="100%">
                    <tr>
                        <td style="height: 5px"></td>
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
                        <td colspan="2">
                            <div class="infoBox" id="divInfoBox" runat="server" visible="false">
                                Dữ liệu về thời gian xử lý của loại PB và quyền KN của loại PB được chọn sẽ được sao chép sang tên PB mới.
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: right; padding-right: 10px">Tên loại phòng ban
                        </td>
                        <td style="width: 70%; text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:TextBox ID="txtName"  runat="server" Text="" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Mô tả
                        </td>
                        <td style="text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Text="" MaxLength="500"></asp:TextBox>
                                    </div>
                                </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Thời gian xử lý
                        </td>
                        <td style="text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                            <asp:TextBox ID="txtThoiGianXuLyMacDinh" runat="server" Rows="3" Text="" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; padding-right: 10px">Kiểu dữ liệu
                        </td>
                        <td style="text-align: left">
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiDuLieu" runat="server">
                                        <asp:ListItem Value="0" Text="Chọn giá trị"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Dữ liệu ngày giờ"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="% Thời gian gian xử lý"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            
                        </td>
                    </tr>
                </table>
            </div>
            <div class="foot_nav_btn">
                <div class="popup_Buttons">
                    <a href="#1"><i class="apply">&nbsp;</i><span>
                        <asp:Button ID="btnOkay" CssClass="button_eole" Text="Chọn" runat="server" OnClick="btSubmit_Click" />                        
                    </span></a>
                    <a href="#1"><i class="apply">&nbsp;</i><span><asp:Button ID="btnResetTime" runat="server" Text="Reset thời gian xử lý" CssClass="button_eole" OnClientClick="return confirm('Bạn có chắc chắn muốn reset lại thời gian xử lý mặc định cho các phòng ban không ?');" OnClick="btnResetTime_Click"/></span></a>
                    <a href="#1"><i class="notapply">&nbsp;</i><span><input id="btnCancel" class="button_eole" value="Hủy bỏ" type="button" onclick="cancel();" /></span></a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
