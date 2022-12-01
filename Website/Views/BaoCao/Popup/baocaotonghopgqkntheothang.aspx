<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/BaoCao/BaoCao.Master" CodeBehind="baocaotonghopgqkntheothang.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopgqkntheothang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="baocao" class="reportFont">
    <div id="reportContainer">
         <table border="0" width="100%">
            <tr valign="top">
                <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="3">
                    &nbsp;
                </td>
                <td>                    
                </td>
                <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="3">
                    <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                    <br />
                    <span style="text-decoration: underline;">
                        Độc lập - Tự do - Hạnh phúc
                    </span>                    
                </td>
            </tr>
            <tr>
                <td colspan="6"></td>
            </tr>            
        </table>     

        <div style="text-align:center">
            <h1>
                TỔNG HỢP BÁO CÁO GIẢI QUYẾT KHIẾU NẠI THÁNG <asp:Literal runat="server" ID="lblMonth"></asp:Literal>  NĂM <asp:Literal runat="server" ID="lblYear"></asp:Literal>
            </h1>                            
            
            <table border="0" width="100%" >
                <tr>
                    <td colspan="5" style="text-align:center">
                        <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>
                    </td>                                        
                </tr>                               
            </table>                       
        </div>    

        <br />

        <table class="tbl_style" border="1" style="border-collapse: collapse;">
           
              <tr>
                  <th rowspan="4">
                    STT
                </th>
                  <th rowspan="4">
                    Tên TTVT
                </th>
                  
            </tr>
             <tr>
                <th colspan="15">Giải quyết khiếu nai</th>
            </tr>
            <tr >
               <th rowspan="2">
                    Mang sang
                </th>
                <th rowspan="2">
                    Tiếp nhận
                </th>
                <th rowspan="2">
                    Giải quyết
                </th>
                <th rowspan="2">
                    Tồn
                </th>
                <th rowspan="2">
                    Giảm cước GQKN 
                      <asp:Literal runat="server" ID="lblThangVaNam"></asp:Literal>
                </th>    
                <th rowspan="2">
                    Lũy kế năm
                    <asp:Literal runat="server" ID="lblNamLuyKe"></asp:Literal>
                </th>         
              <th colspan="4">Nội dung khiếu nại</th>
                <th colspan="4">Nguyên nhân khiếu nại</th>
            </tr>
          
            <tr>
                  <th>
                    Chất lượng DV
                </th>     
                <th>
                    Chất lượng phục vụ
                </th>
                <th>Cước</th>
                <th>Khác</th>
                <th>
                    Lỗi nhân viên
                </th>
                <th>
                    Lỗi hệ thống
                </th>
                <th>CSKH</th>
                <th>Khác</th>
            </tr>
                 <%=sNoiDungBaoCao %>
       
        </table>
            <table border="0" width="100%">
                <tr>
                    <td colspan="12">
                        Ghi Chú:
                        <br />
                        (Cột "Mang sang"):  Tình hình số liệu PAKN tồn trước thời điểm đầu lấy báo cáo tại phòng ban (số liệu tồn quá hạn )
                        <br />
                        (Cột "Tồn"):  Số liệu PAKN tồn trước thời điểm đầu lấy báo cáo tại phòng ban ( số liệu tồn trong hạn)
                        <br />
                        (Cột "Lũy kế năm" ):  Số liệu PAKN tồn lũy kế của phòng ban (số liệu tồn quá hạn + số liệu tồn trong hạn)
                        <br />
                                                                                   
                    </td>
                </tr>
            </table>
        <table border="0" width="100%">
            <tr>
                <td style="border: 0px;">
                    &nbsp;
                </td>
            </tr>
            <tr valign="top">
                <td style="border: 0px; text-align: center; font-weight: bold" colspan="3">                    
                </td>  
                <td style="text-align: center; font-weight: bold; border: 0px" colspan="3">
                    Người báo cáo
                </td>              
            </tr>
            <tr>
                <td colspan="5" style="height:70px">
                    &nbsp;
                </td>
            </tr>
            <tr>                
                <td align="center" colspan="3">
                    <asp:label ID="lblFullName" runat="server"></asp:label>
                </td>
            </tr>
        </table>

    <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

    <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

</div>
</div>
</asp:Content>