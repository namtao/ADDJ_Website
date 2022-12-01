<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaodoisoatcskhptdv.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaodoisoatcskhptdv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div runat="server" id="baocao" class="reportFont">
    <div style="text-align:center">        
        <table border="0" width="100%" >
                <tr>
                    <td colspan="6" style="text-align:center;font-weight:bold">
                        CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                        <br />
                            Độc lập - Tự do - Hạnh phúc
                        <br />
                            ---------o0o----------                            
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:center">
                        Hà Nội, <asp:Label ID="lblCurDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align:center;font-weight:bold">
                        BIÊN BẢN DOANH THU BÙ CƯỚC DỊCH VỤ 
                        <br />
                        TỪ HỆ THỐNG CSKH- GiẢI QUYẾT KHIẾU NẠI   
                    </td>                                
                </tr>          
                <tr>
                    <td colspan="6" style="text-align:center;"">
                        <asp:Label ID="lblLocation" runat="server"></asp:Label>
                    </td>
                </tr>      
                <tr>
                    <td colspan="6" style="text-align:center">
                        <asp:Label ID="lblReportDate" runat="server"></asp:Label>
                    </td>                    
                </tr>
            </table>
            
    </div>
    
    <br />

    <table class="tbl_style_2" border="1" style="border-collapse: collapse;">
        <tr>
            <th>STT</th>
            <th >Ngày</th>
            <th >Doanh thu CSKH gửi PTDV</th>
            <th >Doanh Thu PTDV nhận được</th>
            <th >Chênh lệch</th>
            <th >% Lệch</th>               
        </tr>
        <tr>
            <th ></th>
            <th >1</th>
            <th >2</th>
            <th >3</th>
            <th >4 = 2 - 3</th>
            <th >5 = 4 / max[2,3]</th>
        </tr>         
        <%=sNoiDungBaoCao %>
    </table>

    <br />
    <table width="100%">
        <tr>
            <td colspan="3" style="text-align:center;font-weight:bold">
                Đại diện TT.PTDV
            </td>
            <td colspan="3" style="text-align:center;font-weight:bold">
                Đại diện CSKH
            </td>
        </tr>
    </table>
    </div>

</asp:Content>
