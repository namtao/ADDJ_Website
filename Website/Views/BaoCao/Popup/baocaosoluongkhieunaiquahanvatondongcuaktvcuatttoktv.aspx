<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaosoluongkhieunaiquahanvatondongcuaktvcuatttoktv.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaosoluongkhieunaiquahanvatondongcuaktvcuatttoktv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div runat="server" id="baocao" class="reportFont">
        <table border="0">
            <tr>
                <td colspan="3" style="padding-left:100px; text-align:center">
                  <%--  <asp:Label ID="lblKhuVuc" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblPhongBan" runat="server"></asp:Label>--%>
                    TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV I 
                    <br />
                    ĐÀI KHAI THÁC - TỔ KHAI THÁC
                </td>
            </tr>
        </table>
   
        <div style="text-align:center">
            <h1>
                BÁO CÁO SỐ LƯỢNG KHIẾU NẠI QUÁ HẠN VÀ TỒN ĐỌNG
            </h1>

            <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                   
                <asp:label runat="server" id="lblReportMonth" Visible="false"></asp:label>         
                Từ ngày 01/12/2013 đến ngày 31/12/2013     
            </span>
        </div>    

        <div>           
            <div>
                <%--<table class="tbl_style" border="1">
                    <%=sNoiDungBaoCao %>
                </table>--%>

                <table class="tbl_style" border="1">
                    <tr>
                        <th>STT</th>
                        <th>Họ tên</th>
                        <th>Số lượng KN tồn đọng</th>
                        <th>Số lượng KN quá hạn</th>
                    </tr>
                    <tr>
                        <td align="center">1</td>
                        <td>Lê Minh Phương</td>
                        <td align="center">5</td>
                        <td align="center">1</td>
                    </tr>
                    <tr>
                        <td align="center">2</td>
                        <td>Nguyễn Phương Nga</td>
                        <td align="center">8</td>
                        <td align="center">3</td>
                    </tr>
                    <tr>
                        <td align="center">3</td>
                        <td>Ngô Bích Diệp</td>
                        <td align="center">10</td>
                        <td align="center">2</td>
                    </tr>
                    <tr>
                        <td align="center">4</td>
                        <td>Vũ Minh Huyền</td>
                        <td align="center">15</td>
                        <td align="center">6</td>
                    </tr>
                    <tr>
                        <td align="center">5</td>
                        <td>Nguyễn Thanh Thủy</td>
                        <td align="center">20</td>
                        <td align="center">10</td>
                    </tr>
                </table>
            </div>
        </div>                
    </div>
</asp:Content>
