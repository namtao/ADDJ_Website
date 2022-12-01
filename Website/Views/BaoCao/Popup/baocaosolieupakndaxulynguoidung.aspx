<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/BaoCao/BaoCao.Master" CodeBehind="baocaosolieupakndaxulynguoidung.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaosolieupakndaxulynguoidung" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <div>       
        <table border="0" width="100%" >
            <tr>
                <td colspan="5" style="text-align:center">
                     <h1>
                        BÁO CÁO SỐ LIỆU
                        <br />
                        PHẢN ÁNH KHIẾU NẠI NGƯỜI DÙNG ĐÃ XỬ LÝ            
                    </h1>
                </td>
            </tr>
            <tr>
                <td colspan="5"></td>
            </tr>
            <tr>
                <td style="text-align:right">
                    Từ ngày:
                </td>
                <td colspan="4" style="text-align:left" ><asp:literal runat="server" id="lbFromDate"></asp:literal></td>
            </tr>
            <tr>
                <td style="text-align:right">
                    Đến ngày:
                </td>
                <td colspan="4" style="text-align:left"><asp:literal runat="server" id="lbToDate"></asp:literal></td>
            </tr>            
        </table>          
    </div>
    
    <br />

    <table class="tbl_style" border="1" style="border-collapse: collapse;">
        <tr>
            <th >STT</th>
            <th >Người xử lý</th>
            <th >Số lượng PAKN đã tiếp nhận</th>
            <th >Số lượng PAKN đã tham gia xử lý</th>
            <th >Số lượng PAKN đã đóng</th>
            <th >Số lượng PAKN đang tồn đọng của user</th>              
        </tr>        
        <%=sNoiDungBaoCao %>
    </table>
</div>

</asp:Content>
