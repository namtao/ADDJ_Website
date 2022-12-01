<%@ Page MasterPageFile="~/Views/BaoCao/BaoCao.Master" Language="C#" AutoEventWireup="true" CodeBehind="baocaotonghoptheokhieunai_gqkn.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghoptheokhieunai_gqkn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <table border="0">
        <tr>
            <td colspan="3" style="padding-left:100px; text-align:center">
                <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                <br />
           
                ĐÀI KHAI THÁC - TỔ GIẢI QUYẾT KHIẾU NẠI
            </td>
        </tr>
    </table>   
    <div style="text-align:center">
        <h1>
            BÁO CÁO THEO LOẠI KHIẾU NẠI
        </h1>

        <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                   
            Từ ngày <asp:label runat="server" id="lblTuNgay"></asp:label>
            đến ngày <asp:label runat="server" id="lblDenNgay"></asp:label>
        </span>
    </div>

    <table class="tbl_style" border="1">
        <%=sNoiDungBaoCao %>
    </table>

    <br />
    <br />

    <div style="float:right; width:300px; text-align:right;padding-right:70px">
        <div style="text-align:center">
            <asp:Label ID="lblWhereWhen" runat="server"></asp:Label>
            <br />
            Người làm báo cáo
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblWho" runat="server"></asp:Label>       
        </div>        
    </div>
    

  
</div>
</asp:Content>
