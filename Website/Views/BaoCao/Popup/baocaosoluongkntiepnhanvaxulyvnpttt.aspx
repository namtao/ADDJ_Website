<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaosoluongkntiepnhanvaxulyvnpttt.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaosoluongkntiepnhanvaxulyvnpttt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    

    <div runat="server" id="baocao" class="reportFont">
        <table border="0">
            <tr>
                <td colspan="3" style="padding-left:100px; text-align:center;font-weight:bold">
                    <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblPhongBan" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
   
        <div style="text-align:center">
            <h1>
                BÁO CÁO SỐ LƯỢNG KHIẾU NẠI TIẾP NHẬN VÀ XỬ LÝ
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
