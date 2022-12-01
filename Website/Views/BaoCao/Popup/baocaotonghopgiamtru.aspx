<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopgiamtru.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopgiamtru" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <div style="text-align:center">
        <table border="0" width="100%">
            <tr valign="top">
                <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="2">
                    <asp:Label ID="lblKhuVucHeader" runat="server"></asp:Label>
                </td>
                <td>
                    
                </td>
                <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="3">
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
            <tr>                              
                <td colspan="6" style="font-style:italic; text-align:right; padding-right:100px">
                    <asp:Label ID="lblWhereWhen" runat="server"></asp:Label>
                </td>
            </tr>
        </table>       

        <h1>
            BÁO CÁO TỔNG HỢP GIẢM TRỪ CƯỚC DV GTGT THEO CP
            <br />            
            <asp:Label ID="lblReportMonth" runat="server"></asp:Label>
        </h1>        

        <table border="0" width="100%" >
            <tr>
                <td style="text-align:right;width:40%" colspan="2">
                Khu vực:
                </td>
                <td style="text-align:left" colspan="3"><asp:literal runat="server" id="lbKhuVuc"></asp:literal></td>
            </tr>
            <tr>
                <td style="text-align:right" colspan="2">
                Đơn vị:
                </td>
                <td style="text-align:left" colspan="3"><asp:literal runat="server" id="lbDonVi"></asp:literal></td>
            </tr>
            <tr>
                <td style="text-align:right" colspan="2">
                Từ ngày - đến ngày:
                </td>
                <td style="text-align:left" colspan="3">
                    <asp:label runat="server" id="lblTuNgay"></asp:label> - <asp:label runat="server" id="lblDenNgay"></asp:label>
                </td>
            </tr>
        </table>

        <br />
        <table border="0" width="100%" >          
            <tr>               
                <td align="right" style="width:20%" colspan="2">   
                        Loại khiếu nại:                 
                </td>
                <td align="left" colspan="3">
                    <asp:literal runat="server" id="lbLoaiKhieuNai"></asp:literal>
                </td>                
            </tr>
            <tr>                
                <td align="right" colspan="2">Lĩnh vực chung:</td>
                <td align="left" colspan="3"><asp:literal runat="server" id="lbLinhVucChung"></asp:literal></td>                
            </tr>
            <tr>               
                <td align="right" colspan="2">Lĩnh vực con:</td>
                <td align="left" colspan="3"><asp:literal runat="server" id="lbLinhVucCon"></asp:literal></td>                
            </tr>                
        </table>   
            
        <br />

        <table border="0" width="100%" align="center">
            <tr>
                <td style="text-align:right;width:40%" valign="top" colspan="2">
                    Kính gửi :
                </td>
                <td style="text-align:left" colspan="3">
                    - Trung tâm phát triển dịch vụ
                    <br />
                    - Phòng chăm sóc khách hàng
                </td>
            </tr>                
        </table>    
    </div>
    
    <table class="tbl_style" border="1" style="border-collapse: collapse;">
        <tr>
            <th>TT</th>
            <th >Đối tác CP</th>
            <th >Số lượng giảm trừ (đ/v: thuê bao)</th>
            <th >Tài khoản chính (đ/v: đồng)</th>           
            <th >Ghi chú</th>            
        </tr>        
        <%=sNoiDungBaoCao %>
        </table>
    </div>
</asp:Content>
