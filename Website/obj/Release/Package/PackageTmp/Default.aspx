<%@ Page Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" Inherits="Default"
    Title="Home - Hệ thống hỗ trợ kỹ thuật tập trung" CodeBehind="Default.aspx.cs" %>

<%@ Register Src="HeThongHoTro/Dashboards/DanhSachHoTroXuLyHT.ascx" TagName="DanhSachHoTroXuLyHT" TagPrefix="uc1" %>
<%@ Register Src="HeThongHoTro/Dashboards/DanhSachYeuCauHT.ascx" TagName="DanhSachYeuCauHT" TagPrefix="uc2" %>
<%@ Register Src="ADDJ_TH/views/WebUserControl1_q10.ascx" TagPrefix="uc1" TagName="WebUserControl1" %>



<asp:Content ContentPlaceHolderID="HeaderCss" runat="server">
 
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Main" runat="Server">
    <div style="padding: 10px">
       <uc1:WebUserControl1 runat="server" id="dgdsg" />
    </div>
</asp:Content>
