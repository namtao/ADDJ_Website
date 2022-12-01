<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaochitietgiamtrutrasau.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaochitietgiamtrutrasau" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <div id="reportContainer">
         <table border="0" width="100%">
            <tr valign="top" runat="server" visible="false" id="rowToGQKN">
                <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="6">
                    <asp:Label ID="lblKhuVucHeader" runat="server"></asp:Label>
                </td>
                <td>                    
                </td>
                <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="6">
                    <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                    <br />
                    <span style="text-decoration: underline;">
                        Độc lập - Tự do - Hạnh phúc
                    </span>                    
                </td>
            </tr>
            <tr valign="top" runat="server" visible="false" id="rowVNPTTT">
                <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="6">                    
                    <span style="text-decoration: underline; text-transform:uppercase">
                        <asp:Label ID="lblVNPTTT" runat="server"></asp:Label>
                    </span>                   
                </td>
                <td>                    
                </td>
                <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="6">
                    <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                    <br />
                    <span style="text-decoration: underline;">
                        Độc lập - Tự do - Hạnh phúc
                    </span>                    
                </td>
            </tr>
            <tr>
                <td colspan="13"></td>
            </tr>
            <tr>               
                <td style="font-style:italic;padding-right:100px;text-align:right" colspan="13">
                    <asp:Label ID="lblWhereWhen" runat="server"></asp:Label>
                </td>
            </tr>
        </table>     

        <div style="text-align:center">
            <h1>
                BÁO CÁO CHI TIẾT GIẢM TRỪ TRẢ SAU
            </h1>                            
            
            <table border="0" width="100%" >
               <%-- <tr>
                    <td colspan="7" style="text-align:right">
                    Tỉnh thành:
                    </td>
                    <td colspan="6" style="text-align:left"><asp:literal runat="server" id="lbVNPTTT"></asp:literal></td>
                </tr>   --%>            
                <tr>
                    <td colspan="7" style="text-align:right">
                    Từ ngày - đến ngày:
                    </td>
                    <td colspan="6" style="text-align:left">
                        <asp:label runat="server" id="lblTuNgay"></asp:label> - <asp:label runat="server" id="lblDenNgay"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td colspan="13"></td>
                </tr>
                <tr>                    
                    <td align="right" colspan="4">   
                         Loại khiếu nại:                 
                    </td>
                    <td align="left" colspan="9">
                        <asp:literal runat="server" id="lbLoaiKhieuNai"></asp:literal>
                    </td>                   
                </tr>
                <tr>                    
                    <td align="right" colspan="4">Lĩnh vực chung:</td>
                    <td align="left" colspan="9"><asp:literal runat="server" id="lbLinhVucChung"></asp:literal></td>                    
                </tr>
                <tr>                    
                    <td align="right" colspan="4">Lĩnh vực con:</td>
                    <td align="left" colspan="9"><asp:literal runat="server" id="lbLinhVucCon"></asp:literal></td>                    
                </tr>
                <tr>
                    <td colspan="13"></td>
                </tr>
                <tr valign="top">
                    <td style="text-align:right;width:40%" colspan="7" >
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
                <th>
                    Cước GPRS
                </th>
                <th>
                    Cước CP
                </th>
                <th>
                    Cước Thoại
                </th>                
                <th>
                    Cước SMS
                </th>   
                <th>
                    Cước IR
                </th>                
                <th>
                    Cước Khác
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
                <td style="border: 0px; text-align: center; font-weight: bold" colspan="7">
                    NGƯỜI LẬP BẢNG
                </td>  
                <td style="text-align: center; font-weight: bold; border: 0px" colspan="7">
                    TL. GIÁM ĐỐC
                    <br />
                    TRƯỞNG ĐÀI
                </td>              
            </tr>
            <tr>
                <td colspan="14" style="height:70px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="7">
                    <asp:label ID="lblFullName" runat="server"></asp:label>
                </td>
                <td align="center" colspan="7">
                
                </td>
            </tr>
        </table>

    <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

    <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

</div>
</div>

</asp:Content>
