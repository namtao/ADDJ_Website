<%@ Page Title="Báo cáo" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="emptypagevnpttt.aspx.cs" Inherits="Website.Views.BaoCao.Popup.emptypagevnpttt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="baocao" class="reportFont">
        <div style="text-align:center">
            <table border="0" width="100%">
                <tr valign="top">
                    <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="4">
                        <asp:Label ID="lblDonVi" runat="server"></asp:Label>
                    </td>
                    <td>
                    
                    </td>
                    <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="10">
                        <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                        <br />
                        <span style="text-decoration: underline;">
                            Độc lập - Tự do - Hạnh phúc
                        </span>                    
                    </td>
                </tr>            
            </table>       

            <p style="text-align:center">
                <h1>
                    <asp:Label ID="lblReportTitle" runat="server"></asp:Label>                       
                </h1>   
            </p>
             
             <p style="text-align:center">            
                <asp:Label ID="lblReportMonth" runat="server" Font-Italic="true"></asp:Label>                                   
            </p>



            <table class="tbl_style" border="1" style="border-collapse: collapse;">            
                <%=sNoiDungBaoCao %>
            </table>

        
        </div>
    </div>
     
</asp:Content>
