<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaobieudosoluongkhieunaitiepnhan.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaobieudosoluongkhieunaitiepnhan" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao">
    <asp:Chart ID="chartSoLuongKhieuNaiTiepNhan" runat="server" Width="1020" Height="500">
        <Titles> 
            <asp:Title Text="BIỂU ĐỒ SỐ LƯỢNG KHIẾU NẠI ĐÃ TẠO" 
                Font="Tahoma, 12pt, style=Bold"></asp:Title> 
        </Titles>         
        <Series>            
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
</div>
    
</asp:Content>
