<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPhongBan.aspx.cs"
    Inherits="Website.admin.SelectPhongBan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <style type="text/css">
        
        
        .popup_Container
        {
            background-color: #f0f0f0;
            border: 2px solid #000000;
            padding: 0px 0px 0px 0px;
        }
        
        .popup_Titlebar
        {
            background: url(Images/titlebar_bg.jpg);
            height: 29px;
        }
        
        .popup_Body
        {
            padding: 15px 15px 15px 15px;
            font-family: Arial;
            font-weight: bold;
            font-size: 12px;
            color: #000000;
            line-height: 15pt;
            clear: both;
            padding: 20px;
        }
        
        
        .TitlebarLeft
        {
            float: left;
            padding-left: 5px;
            padding-top: 5px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            font-size: 12px;
            color: #FFFFFF;
        }
        
        .TitlebarRight
        {
            background: url(Images/cross_icon_normal.png);
            background-position: right;
            background-repeat: no-repeat;
            height: 15px;
            width: 16px;
            float: right;
            cursor: pointer;
            margin-right: 5px;
            margin-top: 5px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function getbacktostepone() {
            window.location = "SelectPhongBan.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 100);
        }
        function onError() {
            setTimeout(getbacktostepone, 100);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            document.getElementById('btnCancel').click();
        }
    </script>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    </form>
</body>
</html>
