<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityPhongBan.ascx.cs" Inherits="Website.Views.Dashboards.ActivityPhongBan" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="font-weight: bold; font-size: 14pt; padding-bottom: 5px;">
            <asp:Literal ID="ltPhongBan" runat="server"></asp:Literal>
        </div>
        <hr />
        <asp:Literal ID="ltContent" runat="server"></asp:Literal>
        <br />
        <asp:Button ID="btShowMore" CssClass="button" runat="server" Text="Nhiều hơn" OnClick="btShowMore_Click" />
    </ContentTemplate>
</asp:UpdatePanel>
