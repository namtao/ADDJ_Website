<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaochitietpps.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaochitietpps" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <div style="text-align:center">
        <h1>
            SỔ QUẢN LÝ GIẢI QUYẾT KHIẾU NẠI CƯỚC DỊCH VỤ  
            <br />
                <%--TUẦN 30-2013 - Từ ngày 18/7 đến ngày 24/7/2013--%>
            <asp:Label ID="lblReportMonth" runat="server" ></asp:Label>
        </h1>

        <table border="0" width="100%" >
                <tr>
                    <td colspan="5" style="text-align:right">
                    Khu vực:
                    </td>
                    <td colspan="14" style="text-align:left"><asp:literal runat="server" id="lbKhuVuc"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="5" style="text-align:right">
                    Đơn vị:
                    </td>
                    <td colspan="14" style="text-align:left"><asp:literal runat="server" id="lbDonVi"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="5" style="text-align:right">
                    Từ ngày - đến ngày:
                    </td>
                    <td colspan="14" style="text-align:left">
                        <asp:label runat="server" id="lblTuNgay"></asp:label> - <asp:label runat="server" id="lblDenNgay"></asp:label>
                    </td>
                </tr>
            </table>

            <br />
            <table border="0" width="100%" >          
                <tr>                    
                    <td align="right" colspan="3">   
                         Loại khiếu nại:                 
                    </td>
                    <td align="left" colspan="16">
                        <asp:literal runat="server" id="lbLoaiKhieuNai"></asp:literal>
                    </td>                    
                </tr>
                <tr>                    
                    <td align="right" colspan="3">Lĩnh vực chung:</td>
                    <td align="left" colspan="16"><asp:literal runat="server" id="lbLinhVucChung"></asp:literal></td>                    
                </tr>
                <tr>                    
                    <td align="right" colspan="3">Lĩnh vực con:</td>
                    <td align="left" colspan="16"><asp:literal runat="server" id="lbLinhVucCon"></asp:literal></td>                    
                </tr>
            </table>
    </div>
    
    <br />

    <table class="tbl_style_2" border="1" style="border-collapse: collapse;">
        <tr>
            <th rowspan="3">STT</th>
            <th rowspan="3">Số thuê bao</th>
            <th rowspan="3">Tỉnh</th>
            <th rowspan="3">Ngày tiếp nhận</th>
            <th rowspan="3" style="width:200px">Nội dung khiếu nại</th>
            <th rowspan="3">Loại khiếu nại</th>
            <th rowspan="3">Lĩnh vực chung</th>
            <th rowspan="3">Tên dịch vụ</th>
            <th rowspan="3">Mã dịch vụ</th>
            <th rowspan="3">Ngày xử lý</th>
            <th rowspan="3">SHCV</th>
            <th colspan="5" style="width:300px">Kết quả xử lý</th>
            <th rowspan="3" style="width:200px">Nội dung xử lý</th>
            <th rowspan="3" style="width:200px">Kết quả</th>
            <th rowspan="3">Ghi chú</th>
            <th rowspan="3">Người xử lý</th>
            <th rowspan="3">Trạng thái</th>
        </tr>
        <tr>
            <th rowspan="2">Cấp chi tiết</th>
            <th colspan="4">Giảm trừ<br />(Số tiền : đồng)</th>
        </tr>
        <tr>
            <th >TKC</th>
            <th >TKKM</th>
            <th >Data</th>
            <th >Khác</th>
        </tr>
        <%=sNoiDungBaoCao %>
        </table>
    </div>
</asp:Content>
