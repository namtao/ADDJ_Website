<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Website.WebForm1" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.State" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v21.2.Web, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <% if(DesignMode){ %>
    <script src="ASPxScriptIntelliSense.js" type="text/javascript"></script>
<% } %>
    <script>
        
        
        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        }
    </script>
    <form id="form1" runat="server">
   <div>

       <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" EnableTheming="True" Text="ASPxButton" Theme="Aqua">
           <ClientSideEvents Click="function(s, e) {
	lblHienThi.SetValue(txtHoTen.GetValue());
}" />
       </dx:ASPxButton>
       <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="lblHienThi" Text="ASPxLabel" Theme="Aqua">
       </dx:ASPxLabel>
       <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="txtHoTen" Theme="Aqua" Width="170px">
       </dx:ASPxTextBox>
       <dx:ASPxButton ID="ASPxButton2" runat="server" EnableTheming="True" Text="Hiện Popup" Theme="Aqua">
           <ClientSideEvents Click="function(s, e) {
	popupDangNhapHeThong.Show();
}" />
       </dx:ASPxButton>
       <dx:ASPxButton ID="ASPxButton4" runat="server" EnableTheming="True" Text="Hiển thị tại tọa độ..." Theme="Aqua">
           <ClientSideEvents Click="function(s, e) {
	popupDangNhapHeThong.ShowAtPos(100,100);
}" />
       </dx:ASPxButton>
       <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Đóng popup" Theme="Aqua">
           <ClientSideEvents Click="function(s, e) {
	popupDangNhapHeThong.Hide();
}" />
       </dx:ASPxButton>
       <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="popupDangNhapHeThong" HeaderText="Đăng nhập hệ thống" Height="220px" Theme="Office2010Silver" Width="297px" AllowDragging="True" EnableTheming="True" Modal="True">
           <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Tên đăng nhập" Theme="iOS">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ClientInstanceName="txtTenDangNhap" Width="170px">
                </dx:ASPxTextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Mật khẩu" Theme="iOS">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ClientInstanceName="txtMatKhau" Width="170px">
                </dx:ASPxTextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Đăng nhập" Theme="iOS">
                </dx:ASPxButton>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
               </dx:PopupControlContentControl>
</ContentCollection>
       </dx:ASPxPopupControl>

   </div>
    </form>
</body>
</html>
