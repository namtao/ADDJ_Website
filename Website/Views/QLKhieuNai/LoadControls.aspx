<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadControls.aspx.cs" Inherits="Website.Views.QLKhieuNai.LoadControls" %>
<%@ Register TagPrefix="uc" TagName="ChiTietKhieuNai" Src="~/Views/ChiTietKhieuNai/UC/ucChiTietKhieuNai.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link media="screen" rel="stylesheet" href="/css/reset.css" />
    <link href="/CSS/RegionToTruong.css" rel="stylesheet" type="text/css" />
    <link media="screen" rel="stylesheet" href="/css/style.css" />
    <link media="screen" rel="stylesheet" href="/css/ddsmoothmenu.css" />  
    <link href="/Content/easyui.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="false">
    </asp:ScriptManager>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height:500px">
        <uc:ChiTietKhieuNai ID="ucChiTietKhieuNai" runat="server" />
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
