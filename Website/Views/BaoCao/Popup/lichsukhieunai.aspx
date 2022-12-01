<%@ Page Title="Lịch sử khiếu nại" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="lichsukhieunai.aspx.cs" Inherits="Website.Views.BaoCao.Popup.lichsukhieunai" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div runat="server" id="baocao" class="reportFont">
        <div id="reportContainer">              

            <div style="text-align:center">
                <h1>
                    LỊCH SỬ KHIẾU NẠI
                </h1>                            
            
                <table border="0" width="100%" >
                    <tr>
                        <td colspan="10" style="text-align:center">
                            <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>
                        </td>                                        
                    </tr>      
                    <tr>
                        <td colspan="10">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="10" style="text-align:center;font-weight:bold;">
                            <asp:Label ID="lblKhieuNai" runat="server"></asp:Label>
                        </td>                                        
                    </tr>                           
                </table>                       
            </div>    

            <br />

            <table class="tbl_style" border="1" style="border-collapse: collapse;">   
                <tr>
                    <th>STT</th>
                    <th>Phòng ban xử lý trước</th>
                    <th>Người xử lý trước</th>
                    <th>Phòng ban xử lý</th>
                    <th>Người xử lý</th>
                    <th>Hành động</th>
                    <th>Ngày tiếp nhận</th>
                    <th>Ngày quá hạn</th>
                    <th>Ngày xử lý</th>
                    <th>Quá hạn</th>                    
                </tr>                
                <%=sNoiDungBaoCao %>
            </table>                                    

        <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

        <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

        </div>
    </div>

</asp:Content>
