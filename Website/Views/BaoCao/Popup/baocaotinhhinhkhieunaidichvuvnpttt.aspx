<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotinhhinhkhieunaidichvuvnpttt.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotinhhinhkhieunaidichvuvnpttt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div runat="server" id="baocao" class="reportFont">
        <table border="0">
            <tr>
                <td colspan="3" style="padding-left:100px; text-align:center;font-weight:bold">
                    <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblPhongBan" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
   
        <div style="text-align:center">
            <h1>
                BÁO CÁO TÌNH HÌNH KHIẾU NẠI DỊCH VỤ
            </h1>

            <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                                      
                <asp:label runat="server" id="lblReportMonth"></asp:label>           
            </span>
        </div>    

        <div>           
            <div>
                <table class="tbl_style" border="1">
                    <tr>
                        <th rowspan='2'>STT</th>
                        <th rowspan='2'>Số thuê bao</th>
                        <th rowspan='2'>Họ tên khách hàng</th>
                        <th rowspan='2'>Thời gian tiếp nhận</th>
                        <th rowspan='2'>Thời gian báo nhận</th>
                        <th rowspan='2'>Nội dung giải quyết</th>
                        <th rowspan='2'>Thời gian giải quyết</th>
                        <th rowspan='2'>Số hiệu công văn</th>
                        <th colspan='2'>Kết quả</th>
                        <th rowspan='2'>Trạng thái</th>
                    </tr>
                    <tr>
                        <th >Cấp chi tiết</th>
                        <th >Giảm trừ (đồng, chưa VAT)</th>
                    </tr>                    
                    <%=sNoiDungBaoCao %>
                </table>
            </div>
        </div>                
    </div>

</asp:Content>
