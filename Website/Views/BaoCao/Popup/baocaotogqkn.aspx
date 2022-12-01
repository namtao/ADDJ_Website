<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotogqkn.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotogqkn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <table border="0">
        <tr>
            <td colspan="3" style="padding-left:100px; text-align:center">
                <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>                
            </td>
        </tr>
    </table>
   
    <div style="text-align:center">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label>
        </h1>

        <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                   
            <asp:label runat="server" id="lblReportMonth"></asp:label>           
        </span>
    </div>    

    <div>        
        <div>
            <table class="tbl_style" border="1">
                <%=sNoiDungBaoCao %>
            </table>
        </div>
    </div>      
</div>

</asp:Content>
