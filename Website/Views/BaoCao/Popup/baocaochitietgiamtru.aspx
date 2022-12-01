<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="baocaochitietgiamtru.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaochitietgiamtru" %>

<link href="/CSS/BaoCao.css" rel="stylesheet" type="text/css" />
<link href="/CSS/style.css" rel="stylesheet" type="text/css" />

<div runat="server" id="baocao" class="reportFont">
    <div id="reportContainer">
         <table border="0" width="100%">
            <tr valign="top">
                <td style="font-weight:bold;font-size:10pt;text-align:center;" colspan="6">
                   <asp:Label ID="lblKhuVucHeader" runat="server"></asp:Label>
                </td>
                <td>
                    
                </td>
                <td style="font-weight:bold;font-size:10pt;text-align:center;" colspan="5">
                    <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                    <br />
                    <span style="text-decoration: underline;">
                        Độc lập - Tự do - Hạnh phúc
                    </span>                    
                </td>
            </tr>
            <tr>
                <td colspan="12"></td>
            </tr>
            <tr>               
                <td style="font-style:italic;padding-right:100px" colspan="12" align="right" >
                    <asp:Label ID="lblWhereWhen" runat="server"></asp:Label>
                </td>
            </tr>
        </table>     

        <div style="text-align:center">
            <h1>
                BÁO CÁO CHI TIẾT GIẢM TRỪ TRẢ TRƯỚC
            </h1>                            
            
            <table border="0" width="100%" >
                <tr>
                    <td colspan="6" style="text-align:right">
                    Khu vực:
                    </td>
                    <td colspan="6" style="text-align:left"><asp:literal runat="server" id="lbKhuVuc"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:right">
                    Đơn vị:
                    </td>
                    <td colspan="6" style="text-align:left"><asp:literal runat="server" id="lbDonVi"></asp:literal></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:right">
                    Từ ngày - đến ngày:
                    </td>
                    <td colspan="6" style="text-align:left">
                        <asp:label runat="server" id="lblTuNgay"></asp:label> - <asp:label runat="server" id="lblDenNgay"></asp:label>
                    </td>
                </tr>
            </table>

            <br />
            <table border="0" width="100%" >          
                <tr>                   
                    <td align="right" colspan="4">   
                         Loại khiếu nại:                 
                    </td>
                    <td align="left" colspan="8">
                        <asp:literal runat="server" id="lbLoaiKhieuNai"></asp:literal>
                    </td>                   
                </tr>
                <tr>                    
                    <td align="right" colspan="4">Lĩnh vực chung:</td>
                    <td align="left" colspan="8"><asp:literal runat="server" id="lbLinhVucChung"></asp:literal></td>                  
                </tr>
                <tr>                    
                    <td align="right" colspan="4">Lĩnh vực con:</td>
                    <td align="left" colspan="8"><asp:literal runat="server" id="lbLinhVucCon"></asp:literal></td>                  
                </tr>
            </table>

             <br />

        <table border="0" width="100%" align="center">
            <tr>
                <td style="text-align:right;width:40%" valign="top" colspan="6">
                    Kính gửi :
                </td>
                <td style="text-align:left" colspan="6">
                    - Trung tâm phát triển dịch vụ
                    <br />
                    - Phòng chăm sóc khách hàng
                </td>
            </tr>                
        </table>    
            
        </div>    

        <br />

        <table class="tbl_style" border="1" style="border-collapse: collapse;">
            <tr >
                <th>
                    STT
                </th>
                <th >
                    Số thuê bao
                </th>
                <th>
                    Nội dung khiếu nại
                </th>
                <th >
                    Tên dịch vụ
                </th>
                <th >
                    Ngày xử lý
                </th>
                <th >
                    SHCV
                </th>
                <th >
                    Nội dung xử lý
                </th>
                <th >
                    Tài khoản chính
                </th>
                <th>
                    Tài khoản KM
                </th>
                <th>
                    Tài khoản KM 1
                </th>
                <th>
                    Tài khoản KM 2
                </th>
                <th>
                    Tài khoản Data
                </th>                
                <th>
                    Tài khoản khác
                </th>         
            </tr>
            <%=sNoiDungBaoCao %>
        </table>

        <table border="0" width="100%">
            <tr>
                <td style="border: 0px;">
                    &nbsp;
                </td>
            </tr>
            <tr valign="top">
                <td colspan="6" style="border: 0px; text-align: center; font-weight: bold">
                    NGƯỜI LẬP BẢNG
                </td>
                <td colspan="6" style="text-align: center; font-weight: bold; border: 0px">
                    TL. GIÁM ĐỐC
                    <br />
                    TRƯỞNG ĐÀI
                </td>
            </tr>            
            <tr>
                <td colspan="12" style="height:70px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:label ID="lblFullName" runat="server"></asp:label>
                </td>
                <td align="center" colspan="6">
                
                </td>
            </tr>
        </table>
        <%--<div class="bcdv">
        <div class="top" style="text-align:center">
            <h2>
                BÁO CÁO THEO DỊCH VỤ</h2>
            <p>
                (Báo cáo Cross-Sell)</p>
            <p>
                Từ ngày <asp:label runat="server" ID="lblTuNgay"></asp:label> đến ngày <asp:label runat="server" ID="lblDenNgay"></asp:label></p>
        </div>
        <table class="myTbl">
            <tr>
                <th>
                    STT
                </th>
                <th>
                    Tên đơn vị
                </th>
                <th>
                    User name
                </th>
                <th>
                    Số thuê bao
                </th>
                <th>
                    Ngày thực hiện
                </th>
                <th>
                    Nội dung tư vấn
                </th>
                <th>
                    Kết quả
                </th>
                <th>
                    Độ hài lòng
                </th>
            </tr>
            <%=sThongBao %>
            <tr>               
            </tr>
            </table>
        <table class="signature">
            <tr>
                <td>
                    <strong>Người lập báo cáo</strong>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <p>
                        Ngày .... tháng .... năm 20....</p>
                    <p>
                        <strong>Lãnh đạo đơn vị</strong></p>
                </td>
            </tr>
        </table>
    </div>--%>
    </div>
    <%--<a href="#" class="print" rel="reportContainer">Print</a>--%>

    <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

    <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

</div>
