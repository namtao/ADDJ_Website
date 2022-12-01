<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateSystem.aspx.cs" Inherits="Website.UpdateSystem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đang cập nhật hệ thống ...</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%# string.Format("<link media=\"screen\" rel=\"stylesheet\" href=\"{0}?ver={1}\" />", ResolveClientUrl("~/css/reset.css"), Website.AppCode.Common.Ver) %>
    <%# string.Format("<link media=\"screen\" rel=\"stylesheet\" href=\"{0}?ver={1}\" />", ResolveClientUrl("~/css/style.css"), Website.AppCode.Common.Ver) %>
    <style type="text/css">
        #banner { height: 90px; background: url('../images/bg_banner.jpg') no-repeat 0px 0px #0066b3; position: relative; }
        #banner h1 { color: #fff; font-size: 23px; padding: 30px 0 0 510px; }
        #banner .userarea { position: absolute; right: 10px; top: 10px; color: #fff; }
        #banner .userarea span { font-weight: bold; }
        #banner .userarea a { color: #fff; }
        #footer { width: 100%; position: fixed; bottom: 0; left: 0px; font-weight: bold; }
        #contain { width: 800px; margin: 0px auto; margin-top: 120px; text-align: center; font-weight: bold; color: maroon; font-size: 20px; }
    </style>
</head>
<body>
    <form runat="server">
        <div id="banner">
            <div class="logo">
            </div>
            <div class="company">
                <h1>GIẢI QUYẾT KHIẾU NẠI</h1>
            </div>
        </div>
        <div class="shadow">
        </div>
        <div id="contain">
            <asp:Literal ID="liMessage" Text="Hệ thống đang nâng cấp vui lòng quay lại sau." runat="server"></asp:Literal>
        </div>
        <div id="footer">
            <div class="p8">
                <p>
                    Copyright 2012 by Công ty Dịch vụ Viễn thông VinaPhone
                </p>
                <p>
                    Thiết kế bởi AI Co.,Ltd
                </p>
            </div>
        </div>
    </form>
</body>
</html>

