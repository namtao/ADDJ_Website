<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotheoloaikhieunai.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotheoloaikhieunai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="baocao" class="reportFont">
    <table border="0">
        <tr>
            <td colspan="3" style="padding-left:100px; text-align:center">
                <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>                
            </td>
        </tr>
    </table>
   
    <div style="text-align:center">
        <h1>
            BÁO CÁO THEO MẪU SỐ 5
        </h1>

        <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                   
            <asp:label runat="server" id="lblReportMonth"></asp:label>           
        </span>
    </div>    

    <div>
        <span style="font-weight:bold">1. Nội dung công việc</span>

        <div>
            <table class="tbl_style" border="1">
                <%=sNoiDungCongViec %>
            </table>
        </div>
    </div>

    <br />

    <div>
        <span style="font-weight:bold">2. Phân loại khiếu nại PPS đã giải quyết trong tuần</span>

        <div>
            <table class="tbl_style" border="1">
                <tr>
                    <td colspan ="37">
                        <i>
                            <b>Ghi chú đơn vị tính </b></br>
                            - Cấp chi tiết/Tổng số/Số lượng : Thuê bao <br />
                            - Số tiền : đồng
                        </i>
                        
                    </td>
                </tr>
                <%=sNoiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan %>
            </table>
        </div>
    </div>

    <br />

    <div>
        <span style="font-weight:bold">3. Số liệu hỗ trợ VNPT TT</span>

        <div>
            <table class="tbl_style" border="1">
                <%=sNoiDungSoLieuHoTroVNPTTT %>
            </table>
        </div>
    </div>

    <br />

    <div>
        <span style="font-weight:bold">4. Khó khăn</span>
    </div>

    <br />

    <div>
        <span style="font-weight:bold">5. Đề xuất</span>
    </div>

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
</div>
</asp:Content>
