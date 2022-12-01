<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaosoluongkhieunaitondongvaquahancuadoitac.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaosoluongkhieunaitondongvaquahancuadoitac" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="baocao" class="reportFont">
        <div style="text-align:center">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="BÁO CÁO SỐ LƯỢNG KHIẾU NẠI QUÁ HẠN VÀ TỒN ĐỌNG"></asp:Label>
            </h1>           
        </div>    

        <div>                       
            <table class="tbl_style" border="1">
                <%=sNoiDungBaoCao %>
            </table>
        </div>
    </div>
    
</asp:Content>
