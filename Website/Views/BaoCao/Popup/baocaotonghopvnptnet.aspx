<%@ Page Title="Báo cáo" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaotonghopvnptnet.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaotonghopvnptnet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="baocao" class="reportFont">        
    <div>       
        <table border="0" width="100%" >
            <tr>
                <td colspan="5" style="text-align:center">
                     <h1>
                        <asp:label runat="server" id="lblTitle" text=""></asp:label>
                    </h1>
                </td>
            </tr>
            <tr>
                <td colspan="5"></td>
            </tr>
            
            <tr>
                <td style="text-align:right">
                    Từ ngày:
                </td>
                <td colspan="4" style="text-align:left" ><asp:literal runat="server" id="lbFromDate"></asp:literal></td>
            </tr>
            <tr>
                <td style="text-align:right">
                    Đến ngày:
                </td>
                <td colspan="4" style="text-align:left"><asp:literal runat="server" id="lbToDate"></asp:literal></td>
            </tr>            
        </table>          
    </div>
    
    <br />

    <table class="tbl_style" border="1" style="border-collapse: collapse;">      
        <tr >
            <th>
                Đơn vị/Phòng ban
            </th>
            <th >
                SL tồn kỳ trước
            </th>
            <th >
                SL tiếp nhận
            </th>
            <th>
                SL đã xử lý (tiếp nhận)
            </th>
            <th>
                SL đã xử lý (lũy kế)
            </th>
            <th>
                SL quá hạn đã xử lý
            </th>
            <th >
                SL tồn đọng
            </th>
            <th >
                SL tồn đọng quá hạn
            </th>                  
        </tr>  
        <%=sNoiDungBaoCao %>
    </table>
</div>
</asp:Content>
