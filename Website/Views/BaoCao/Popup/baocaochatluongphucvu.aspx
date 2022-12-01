<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaochatluongphucvu.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaochatluongphucvu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div runat="server" id="baocao" class="reportFont">
            <div id="reportContainer">
                 <table border="0" width="100%">                    
                    <tr valign="top">
                        <td style="font-weight:bold;font-size:10pt;text-align:center" colspan="3">
                            &nbsp;
                        </td>
                        <td>                    
                        </td>
                        <td style="font-weight:bold;font-size:10pt; text-align:center" colspan="2">
                            <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                            <br />
                            <span style="text-decoration: underline;">
                                Độc lập - Tự do - Hạnh phúc
                            </span>                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5"></td>
                    </tr>            
                </table>     

                <div style="text-align:center">
                    <h1>
                        BÁO CÁO CHẤT LƯỢNG PHỤC VỤ
                    </h1>                            
            
                    <table border="0" width="100%" >
                        <tr>
                            <td colspan="5" style="text-align:center">
                                <asp:Label ID="lblFromDateToDate" runat="server"></asp:Label>
                            </td>                                        
                        </tr>                               
                    </table>                       
                </div>    

                <br />

                <%=sNoiDungBaoCao %>                

            <script src="/JS/jquery.PrintArea.js_4.js" type="text/javascript"></script>

            <script src="/Views/BaoCao/printCore.js" type="text/javascript"></script>

        </div>
    </div>
</asp:Content>
