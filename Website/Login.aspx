<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Website.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng nhập hệ thống</title>
    <script src="/JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/JS/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/JS/Utility.js" type="text/javascript"></script>
    <link href="/Content/easyui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(function () {
                $.get('<%=sso_service %>/SSOService.svc/user/RequestToken?callback=?', {},
                    function (ssodata) {
                        if (ssodata.Status == 'SUCCESS') {
                            $.ajax({
                                type: "GET",
                                url: "/Views/Global/Login.ashx",
                                data: { type: "login", token: ssodata.Token },
                                contentType: "application/json; charset=utf-8",
                                dataType: "text",
                                async: false,
                                success: function (data) {
                                    if (data == 1) {
                                        var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl"));
                                        if (returnURL == "") returnURL = '<%=sso_url %>';
                                        window.location = returnURL;
                                    }
                                    else if (data == 2) {
                                        var url = '<%= sso_login %>';
                                        MessageAlert.AlertRedirect('Tài khoản của bạn đã bị khóa tại phân hệ GQKN<br/>Liên hệ người quản trị.', 'error', url);
                                    }
                                    else {
                                        window.location = '<%=sso_login %>';
                                    }

                                }
                            });
                    } else {
                            // $(".main").html("<%= Request.Url.Port %>");
                            window.location.href = '<%= sso_login %>/Login.aspx<%= UrlReturn %>';
                        }
                    }, 'jsonp');
            });
        });
    </script>
</head>
<body style="position: relative">
    <form runat="server">
        <div class="main">
            <asp:Literal runat="server" ID="liMessage"></asp:Literal>
        </div>
    </form>
</body>
</html>
