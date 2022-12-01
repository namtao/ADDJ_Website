<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghoptheokhieunai.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghoptheokhieunai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <table border="0">
        <tr>
            <td colspan="3" style="padding-left:100px; text-align:center">
                <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                <br />
                <%--<asp:Label ID="lblPhongBan" runat="server"></asp:Label>--%>
                ĐÀI KHAI THÁC - TỔ XỬ LÝ NGHIỆP VỤ
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
    

   <%-- <table class="tbl_style" border="1" style="border-collapse: collapse;">
        <tr>
            <th rowspan="3">STT</th>
            <th rowspan="3">Số thuê bao</th>
            <th rowspan="3">VNPT TT</th>
            <th colspan="2">Tiếp nhận</th>
            <th rowspan="3">Nội dung KH khiếu nại</th>
            <th rowspan="3">Nội dung VNPT TT yêu cầu hỗ trợ</th>
            <th rowspan="3">Ngày trả lời</th>
            <th colspan="3">Kết quả xử lý</th>
            <th rowspan="3">Nội dung xử lý</th>
            <th rowspan="3">Người xử lý</th>            
            <th rowspan="3">Ghi chú</th>            
        </tr>
        <tr>
            <th rowspan="2">Thời gian</th>
            <th rowspan="2">Hình thức</th>
            <th rowspan="2">Cấp số liệu</th>
            <th colspan="2">Phân tích số liệu</th>
        </tr>
        <tr>
            <th>IR</th>
            <th>DV khác</th>           
        </tr>       
        <%=sNoiDungBaoCao %>
        <tr>
        </tr>
    </table>--%>
</div>
</asp:Content>
