<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="NhapDuLieuSoHoa.aspx.cs" Inherits="Website.ADDJ_TH.views.NhapDuLieuSoHoa" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <div style="padding:20px">
        <div style="padding-bottom:10px">
            <dx:ASPxLabel ID="btnThongTin" ClientInstanceName="btnThongTin" runat="server" Theme="MaterialCompact" Text="Thực hiện nén và tải file hồ sơ Số hóa cần nhập, sau đó thực hiện"></dx:ASPxLabel>
            <dx:ASPxLabel ID="ASPxLabel1" ClientInstanceName="cacFileDuocPhep" runat="server" Theme="MaterialCompact" Text="Các tập tin được phép tải lên: ZIP, XLS, XLSX"></dx:ASPxLabel>
       
            </div>
            <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" Theme="MaterialCompact">
                <settings rootfolder="~/ADDJ_TH/XULY" thumbnailfolder="~/Thumb/" />
                <SettingsAdaptivity Enabled="true" />
                <SettingsEditing AllowCopy="True" AllowCreate="True" AllowDelete="True" AllowDownload="True" AllowMove="True" AllowRename="True" />
                <SettingsUpload>
                    <AdvancedModeSettings EnableMultiSelect="True">
                    </AdvancedModeSettings>
                </SettingsUpload>
            </dx:ASPxFileManager>
        <hr />
        <div style="text-align:right">
            <dx:ASPxButton ID="btnXuLySoHoa" runat="server" Theme="MaterialCompact" Text="Xử lý số hóa" ClientInstanceName="btnXuLySoHoa" OnClick="btnXuLySoHoa_Click">
                <Image IconID="arrows_next_svg_16x16">
                </Image>
            </dx:ASPxButton>
            <telerik:RadCheckBox ID="chkThemHHS" runat="server" Text="Thêm H, HS ở đầu">
            </telerik:RadCheckBox>
        </div>
     </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
