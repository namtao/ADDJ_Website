<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopgqknvnpttt.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopgqknvnpttt" %>
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
                BÁO CÁO THỰC TRẠNG GQKN 
            </h1>

            <span style="border: 0px; text-align: center; font-style: italic; font-size: 12px;">                                                      
                <asp:label runat="server" id="lblReportMonth"></asp:label>           
            </span>
        </div>    

        <div>           
            <div>
                <table class="tbl_style" border="1">
                    <tr>
                        <th rowspan="2">STT</th>
                        <th rowspan="2">Loại khiếu nại</th>
                        <th>Lũy kế KN đã GQ đến đầu tuần <%=sDauTuan %></th>
                        <th>Lũy kế KN tồn đọng đầu tuần<%=sDauTuan %></th>
                        <th>Số lượng tiếp nhận trong tuần</th>
                        <th>Số lượng đã giải quyết trong tuần</th>
                        <th>Số lượng tồn đọng trong tuần </th>
                        <th>Lũy kế KN đã GQ đến cuối tuần<%=sCuoiTuan %></th>
                        <th>Lũy kế KN tồn đọng do quá hạn cuối tuần <%=sCuoiTuan %></th>
                    </tr>        
                    <tr>
                        <th>(1)</th>
                        <th>(2)</th>
                        <th>(3)</th>
                        <th>(4)</th>
                        <th>(5) = (3) - (4)</th>
                        <th>(6) = (1) + (4)</th>
                        <th>(7) = (2) + (5)</th>
                    </tr>            
                    <%=sNoiDungBaoCao %>
                </table>
            </div>
        </div>                
    </div>

</asp:Content>
