<%@ Page Title="Báo cáo" Language="C#" MasterPageFile="~/Views/BaoCao/BaoCao.Master" AutoEventWireup="true" CodeBehind="baocaodvgtgtchotapdoan.aspx.cs" Inherits="Website.Views.BaoCao.Popup.baocaodvgtgtchotapdoan" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblNoiDungBaoCao" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblTenFile" runat="server" Visible="false"></asp:Label>

    <div runat="server" id="baocao" class="reportFont">
        <div>
            <table border="0" width="100%">
                <tr valign="top">
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="4">
                        <asp:Label ID="lblDonVi" runat="server"></asp:Label>
                    </td>
                    <td></td>
                    <td style="font-weight: bold; font-size: 10pt; text-align: center" colspan="10">
                        <span>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span>
                        <br />
                        <span style="text-decoration: underline;">Độc lập - Tự do - Hạnh phúc
                        </span>
                    </td>
                </tr>
            </table>

            <h1 style="text-align: center">
                <asp:Label ID="lblReportTitle" runat="server"></asp:Label>
            </h1>

            <p style="text-align: center">
                <asp:Label ID="lblReportMonth" runat="server" Font-Italic="true"></asp:Label>
            </p>

            <br />

            <div style="text-align: left">
                <asp:Button ID="btnExportExcelTop" runat="server" Text="Xuất excel" OnClick="btnExportExcel_Click" />
            </div>

            <br />

            <table class="tbl_style" border="1" style="border-collapse: collapse;">
                <%= sNoiDungBaoCao %>
            </table>

            <br />
            <div style="text-align: left">
                <asp:Button ID="btnExportExcel" runat="server" Text="Xuất excel" OnClick="btnExportExcel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
