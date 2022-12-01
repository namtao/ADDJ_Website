<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLuuVetThayDoi.ascx.cs" Inherits="Website.Views.KhieuNai.UC.ucLuuVetThayDoi" %>

<asp:GridView ID="gvLuuVetThayDoi" runat="server" AutoGenerateColumns="False" CssClass="tbl_style" PageSize="6" CellPadding="5"
    DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvLuuVetThayDoi_RowDataBound"
    OnPageIndexChanging="gvLuuVetThayDoi_PageIndexChanging">
    <RowStyle CssClass="rowB" />
    <AlternatingRowStyle CssClass="rowA" />
    <Columns>
        <asp:BoundField HeaderText="Trường" DataField="TruongThayDoi" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:BoundField DataField="GiaTriCu" HeaderText="Giá trị cũ" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="GiaTriMoi" HeaderText="Giá trị mới" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="CUser" HeaderText="Người thay đổi" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="CDate" HeaderText="Ngày thay đổi" DataFormatString="{0:dd/MM/yyyy HH:mm}"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="ThaoTac" HeaderText="Thao tác" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />
    </Columns>
</asp:GridView>
