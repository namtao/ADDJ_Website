<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopsolieupakndatiepnhanttptdv.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopsolieupakndatiepnhanttptdv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <div>        
        <table border="0" width="100%" >
            <tr>
                <td colspan="6" style="text-align:center">
                    <h1>
                        BÁO CÁO TỔNG HỢP SỐ LIỆU
                        <br />
                        PHẢN ÁNH KHIẾU NẠI ĐÃ TIẾP NHẬN        
                    </h1>
                </td>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    Từ ngày:
                </td>
                <td colspan="4" style="text-align:left" ><asp:literal runat="server" id="lbFromDate"></asp:literal></td>
            </tr>          
            <tr>
                <td colspan="2" style="text-align:right">
                    Đến ngày:
                </td>
                <td colspan="4" style="text-align:left" ><asp:literal runat="server" id="lbToDate"></asp:literal></td>
            </tr>             
        </table>          
    </div>
    
    <br />

    <table class="tbl_style" border="1" style="border-collapse: collapse;">
        <tr>
            <th>STT</th>
            <th>Loại khiếu nại</th>
            <th>Lĩnh vực chung</th>
            <th>Lĩnh vực con</th>
            <th>Số lượng PAKN tiếp nhận</th>  
            <th>Số lượng PAKN đã xử lý</th>
        </tr>        
        <%=sNoiDungBaoCao %>
    </table>
</div>

</asp:Content>
