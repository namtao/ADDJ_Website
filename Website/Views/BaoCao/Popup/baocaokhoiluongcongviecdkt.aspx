<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaokhoiluongcongviecdkt.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaokhoiluongcongviecdkt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div runat="server" id="baocao" class="reportFont">
    <table border="0" width="100%">
        <tr>
            <td colspan="6" style="padding-left:100px; text-align:left">
                <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                <br />
                <%--<asp:Label ID="lblPhongBan" runat="server"></asp:Label>--%>
                ĐÀI KHAI THÁC - TỔ XỬ LÝ NGHIỆP VỤ
            </td>
        </tr>
    </table>   
    <div style="text-align:center">
        <table border="0" width="100%">
            <tr>
                <td colspan="6" style="text-align: center;">
                    <h3>
                        BÁO CÁO KHỐI LƯỢNG CÔNG VIỆC BỘ PHẬN HTXL
                    </h3>

                    <span >                                   
                        Từ ngày <asp:label runat="server" id="lblTuNgay"></asp:label>
                        đến ngày <asp:label runat="server" id="lblDenNgay"></asp:label>
                    </span>
                </td>
            </tr>
        </table>
        
    </div>

    <table>
        <tr style="font-weight:bold">
            <td width="25px">
                1.
            </td>
            <td width="150px">
                Nhân lực
            </td>
            <td colspan="4">
                &nbsp;
            </td>            
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                Tổng số nhân viên tổ XLNV
            </td>
            <td colspan="3" style="text-align:left">                
                <asp:Label ID="lblTongSoNhanVien" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                Tổng số lượt nhân viên trực
            </td>
            <td colspan="3" style="text-align:left">
            
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                Tổng số PAKN đã xử lý
            </td>
            <td colspan="3" style="text-align:left">
                <asp:Label ID="lblTongSoPAKNXuLy" runat="server"></asp:Label>
            </td>
        </tr>
        <tr style="font-weight:bold">
            <td>
                2.
            </td>
            <td colspan="5">
                Khối lượng công việc
            </td>
        </tr>
    </table>

    <table class="tbl_style" border="1">
        <tr>
            <th>STT</th>
            <th>Họ tên</th>
            <th>Tổng số PAKN đã giải quyết</th>
            <th>Tổng thời gian làm việc (h)</th>
            <th>Khối lượng công việc trung bình</th>
            <th>Ghi chú</th>
        </tr>
        <tr>
            <td width="50px">&nbsp;</td>
            <td>&nbsp;</td>
            <td style="text-align:center;color:red" width="200px">1</td>
            <td style="text-align:center;color:red" width="200px">2</td>
            <td style="text-align:center;color:red" width="200px">3</td>
            <td>&nbsp;</td>
        </tr>        
        <%=sNoiDungBaoCao %>
    </table>

    <br />
    <br />
    <table border="0" width="100%">
        <tr>
            <td colspan="4">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td colspan="2" style="text-align:center;padding-right:70px">
                 <asp:Label ID="lblWhereWhen" runat="server"></asp:Label>
                    <br />
                    Người làm báo cáo
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lblWho" runat="server"></asp:Label>    
            </td>
        </tr>
    </table>   
</div>

</asp:Content>
