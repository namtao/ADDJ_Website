<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaobieudososanhtongsotiengiamtru.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaobieudososanhtongsotiengiamtru" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div runat="server" id="baocao">       
        <asp:Chart ID="chartTongSoTienGiamTru" runat="server" Width="1020" Height="500">
            <Titles> 
                <asp:Title Text="BIỂU ĐỒ TỔNG SỐ TIỀN GIẢM TRỪ" 
                    Font="Tahoma, 12pt, style=Bold"></asp:Title>            
                <asp:Title Font="Tahoma, 9pt, style=Bold"></asp:Title>     
                <asp:Title Font="Tahoma, 9pt, style=Bold"></asp:Title>  
                <asp:Title Font="Tahoma, 9pt, style=Bold"></asp:Title>  
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
