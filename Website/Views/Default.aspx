<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/Master_Default.master" Inherits="Website.Views.Default" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <style type="text/css">
        .colNamee { width: 165px; }
        .tblme td { padding: 5px; }
        .p5 { padding: 5px; }
        .colName { width: 200px; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <table class="tblme" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="colName">Khiếu Nại Id (ex: 2459214)
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtKhieuNaiId" CssClass="aws" ErrorMessage="Vui lòng nhập mã khiếu nại" ValidationGroup="A1">(*)</asp:RequiredFieldValidator>
            </td>
            <td>
                <div class="inputstyle">
                    <div class="bg">
                        <asp:TextBox runat="server" ID="txtKhieuNaiId" Width="150px"></asp:TextBox>
                    </div>
                </div>

            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="colName">Phòng ban xử lý Id (ex: 942)
                <div style="margin-top: 7px;">
                    Có thể để trắng
                </div>
            </td>
            <td>
                <div class="inputstyle">
                    <div class="bg">
                        <asp:TextBox runat="server" ID="txtPhongBanId" Width="150px" ></asp:TextBox>
                    </div>
                </div>

            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="colName">&nbsp;</td>
            <td>
                <asp:Button ID="btnAction" runat="server" Text="Xử lý" Width="85px" ValidationGroup="A1" />&nbsp;
                    <asp:Button ID="btnExportExcel" runat="server" Text="Xuất excel" Width="85px" ValidationGroup="A1" />
                <a href="/">Trang chủ</a>
            </td>
            <td>
                <asp:ValidationSummary runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="A1" />
            </td>
        </tr>
    </table>
    <div class="p5">
        <asp:GridView runat="server" CssClass="tbl_style tblcus tblme" ID="GrvView" Width="100%" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" />
                <asp:BoundField DataField="KhieuNaiId" HeaderText="KhieuNaiId" />
                <asp:BoundField DataField="TenPhongBan" HeaderText="Tên phòng Ban" />
                <asp:BoundField DataField="NguoiXuLy" HeaderText="Người XL" />
                <asp:BoundField DataField="NgayTiepNhan" HeaderText="Tiếp nhận phòng ban" />
                <asp:BoundField DataField="NgayQuaHan" HeaderText="Quá hạn phòng ban" />
                <asp:BoundField DataField="NgayTiepNhanNguoiXuLy" HeaderText="Ngày tiếp nhận NXL" />
                <asp:BoundField DataField="LDate" HeaderText="Ngày chuyển xử lý" />
                <asp:BoundField DataField="OffSetPBXL" HeaderText="Thời gian PBXL" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="OffSetNXL" HeaderText="Thời gian NXL" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="IsQuaHan" HeaderText="Trạng thái" />
            </Columns>
            <EmptyDataTemplate>
                Hiện chưa có dữ liệu
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
