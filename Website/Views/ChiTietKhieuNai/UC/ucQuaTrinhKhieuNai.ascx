<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucQuaTrinhKhieuNai.ascx.cs" Inherits="Website.Views.ChiTietKhieuNai.UC.ucQuaTrinhKhieuNai" %>

<asp:GridView ID="gvQuaTrinhKhieuNai" runat="server" AutoGenerateColumns="False" CssClass="tbl_style" PageSize="6" CellPadding="5"
    DataKeyNames="ID" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvQuaTrinhKhieuNai_RowDataBound"
    OnPageIndexChanging="gvQuaTrinhKhieuNai_PageIndexChanging">
    <RowStyle CssClass="rowB" />
    <AlternatingRowStyle CssClass="rowA" />
    <Columns>
        <asp:BoundField DataField="PhongBanXuLyTruocId" HeaderText="Phòng ban xử lý trước" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:BoundField DataField="NguoiXuLyTruoc" HeaderText="Người xử lý trước" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="HanhDong" HeaderText="Hành động" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />   
        <asp:BoundField DataField="PhongBanXuLyId" HeaderText="Phòng ban xử lý" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:BoundField DataField="NgayTiepNhan" HeaderText="Ngày tiếp nhận PB" DataFormatString="{0:dd/MM/yyyy HH:mm}"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="NguoiXuLy" HeaderText="Người xử lý" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />     
        <asp:BoundField DataField="NgayTiepNhan_NguoiXuLy" HeaderText="Ngày tiếp nhận người xử lý" DataFormatString="{0:dd/MM/yyyy HH:mm}"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />        
        <asp:BoundField DataField="LDate" HeaderText="Ngày xử lý" DataFormatString="{0:dd/MM/yyyy HH:mm}"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="NgayQuaHan" HeaderText="Ngày quá hạn phòng ban" DataFormatString="{0:dd/MM/yyyy HH:mm}"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="" HeaderText="Quá hạn phòng ban" ItemStyle-HorizontalAlign="Left"
            HeaderStyle-HorizontalAlign="Center" />          
    </Columns>
</asp:GridView>