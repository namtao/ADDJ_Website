<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaochitietpost.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaochitietpost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <div style="text-align:center">
        <h1>
            <%--BÁO CÁO GQKN POST TUẦN 30  TỪ 17/07 ĐẾN 24/07/2013--%>
            <asp:Label ID="lblReportMonth" runat="server"></asp:Label>
        </h1>

        <table border="0" width="100%" >
                <tr>
                    <td colspan="6" style="text-align:right">
                    Khu vực:
                    </td>
                    <td colspan="9" style="text-align:left"><asp:literal runat="server" id="lbKhuVuc"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:right">
                    Đơn vị:
                    </td>
                    <td colspan="9" style="text-align:left"><asp:literal runat="server" id="lbDonVi"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:right">
                    Từ ngày - đến ngày:
                    </td>
                    <td colspan="9" style="text-align:left">
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
                    <td align="left" colspan="12">
                        <asp:literal runat="server" id="lbLoaiKhieuNai"></asp:literal>
                    </td>                    
                </tr>
                <tr>                    
                    <td align="right" colspan="3">Lĩnh vực chung:</td>
                    <td align="left" colspan="12"><asp:literal runat="server" id="lbLinhVucChung"></asp:literal></td>                    
                </tr>
                <tr>                   
                    <td align="right" colspan="3">Lĩnh vực con:</td>
                    <td align="left" colspan="12"><asp:literal runat="server" id="lbLinhVucCon"></asp:literal></td>                    
                </tr>
            </table>
    </div>
    
    <br />

    <table class="tbl_style_2" border="1" style="border-collapse: collapse;">
        <tr>
            <th rowspan="3" style="width:20px;">STT</th>
            <th rowspan="3" style="width:70px;">Số thuê bao</th>
            <th rowspan="3" style="width:70px;">VNPT TT</th>
            <th colspan="2" style="width:100px;">Tiếp nhận</th>
            <th rowspan="3" style="width:200px;">Nội dung KH khiếu nại</th>
            <th rowspan="3" style="width:200px;">Nội dung yêu cầu hỗ trợ</th>
            <th rowspan="3" style="width:70px;">Ngày trả lời</th>
            <th colspan="3" style="width:150px;">Kết quả xử lý</th>
            <th rowspan="3" style="width:200px;">Nội dung xử lý</th>
            <th rowspan="3" style="width:70px;">Người xử lý</th>   
            <th rowspan="3" style="width:50px;">Trạng thái</th>           
            <th rowspan="3" style="width:150px;">Ghi chú</th>            
        </tr>
        <tr>
            <th rowspan="2" style="width:70px;">Thời gian</th>
            <th rowspan="2" style="width:70px;">Hình thức</th>
            <th rowspan="2" style="width:50px;">Cấp số liệu</th>
            <th colspan="2"  style="width:100px;">Phân tích số liệu</th>
        </tr>
        <tr>
            <th  style="width:50px;">IR</th>
            <th  style="width:50px;">DV khác</th>           
        </tr>       
        <%=sNoiDungBaoCao %>
        </table>
    </div>
</asp:Content>
