<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="BC_DanhMucBaoCao.aspx.cs" Inherits="Website.HeThongHoTro.Report.BC_DanhMucBaoCao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <div style="padding: 10px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-3">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Báo cáo chi tiết</div>
                        <div class="panel-body">
                            <div class="list-group">
                                <a href="#" class="list-group-item" onclick="window.open('BC_ChiTietTheoTrangThai.aspx')">BC Chi tiết theo trạng thái</a>
                                <a href="#" class="list-group-item" onclick="window.open('BC_ChiTietTheoNguoiDung.aspx')">BC Chi tiết theo người dùng</a>
                                <a href="#" class="list-group-item" onclick="window.open('BC_ChiTietTheoPhongBan.aspx')">BC Chi tiết theo phòng ban</a>
                                <a href="#" class="list-group-item" onclick="window.open('BC_ChiTietTheoHeThong.aspx')">BC Chi tiết theo hệ thống</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Báo cáo cá nhân</div>
                        <div class="panel-body">
                            <div class="list-group">
                                <a href="#" class="list-group-item" onclick="window.open('BC_ChiTietCaNhan.aspx')">BC cá nhân</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Báo cáo tổng hợp</div>
                        <div class="panel-body">
                            <div class="list-group">
                                <a href="#" class="list-group-item" onclick="window.open('bc_TongHopSoLieuTheoNguoiDung.aspx')">Báo cáo theo người dùng</a>
                                <a href="#" class="list-group-item" onclick="window.open('bc_TongHopSoLieuTheoHeThong.aspx')">Báo cáo theo hệ thống</a>
                                <a href="#" class="list-group-item" onclick="window.open('bc_TongHopSoLieuTheoThoiGianNhap.aspx')">Báo cáo theo thời gian nhập</a>
                                <a href="#" class="list-group-item">Báo cáo 01</a>
                            </div>
                        </div>
                        <%-- <div class="panel-footer"></div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
