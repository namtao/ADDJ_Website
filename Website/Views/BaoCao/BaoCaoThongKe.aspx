<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true"
    CodeBehind="BaoCaoThongKe.aspx.cs" Inherits="Website.Views.BaoCao.BaoCaoThongKe" %>

<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/Css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="/Css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <style type="text/css" media="screen">
        .hiddenDiv { display: none; }
        .visibleDiv { display: block; }
        .treeView td { vertical-align: top; }
        .col_right_pagebaocao .wr { padding: 0px 10px; }
        .col_right_pagebaocao .wr > h4 { text-align: center; }
        #baocaokhieunai .tb2 { margin: 10px auto; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="/JS/plugin/date.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="/Views/BaoCao/baocao.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                try {
                    if (console && console.log) console.log("Begin");
                    common.loading();
                }
                catch (e) {
                    if (console && console.log) console.log(e);
                }
            }

            function EndRequestHandler(sender, args) {
                try {
                    if (console && console.log) console.log("End Event");
                    setTimeout(function () {
                        common.unLoading();
                    }, 500);
                    if (args.get_error() == undefined) {
                        var sName = args.get_response().get_webRequest().get_userContext();
                        if (sName == "btnDetails") {
                            //DoSomething();

                        }
                        else {
                            //DoSomethingelse();
                        }
                    }
                }
                catch (e) {
                    if (console && console.log) console.log(e);
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <div id="rightarea">
        <div id="baocaokhieunai">
            <div class="body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblReportType" runat="server" Visible="false"></asp:Label>
                        <div style="overflow: hidden; height: 100%; margin-top: 10px;">
                            <div class="col_left_pagebaocao">
                                <div class="menuhaitac">
                                    <ul>
                                        <li runat="server" id="listReportVNP" visible="false">
                                            <h4>Phòng chăm sóc khách hàng</h4>
                                            <ul>
                                                <%--<li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHop" runat="server" CommandArgument="bc_VNP_BaoCaoTongHop" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp PAKN
                                                    </asp:LinkButton>
                                                </li>--%>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHopGiamTruTheoCP" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTruTheoCP" OnClick="lbReportVNP_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_DoiSoatCSKHPTDV" Visible="false" runat="server" CommandArgument="bc_VNP_DoiSoatCSKHPTDV" OnClick="lbReportVNP_Click">
                                                        Báo cáo doanh thu bù cước dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_DanhSachQuaHan" runat="server" CommandArgument="bc_VNP_DanhSachQuaHan" OnClick="lbReportVNP_Click">
                                                        Lịch sử khiếu nại quá hạn ở phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_KhieuNaiToanMang" runat="server" CommandArgument="bc_VNP_KhieuNaiToanMang" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp khiếu nại toàn mạng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_KhieuNaiToanMangTheoTuan" runat="server" CommandArgument="bc_VNP_KhieuNaiToanMangTheoTuan" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp khiếu nại toàn mạng theo tuần
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_KhieuNaiToanMangTheoThang" runat="server" CommandArgument="bc_VNP_KhieuNaiToanMangTheoThang" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp khiếu nại toàn mạng theo tháng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi" runat="server" CommandArgument="bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi" OnClick="lbReportVNP_Click">
                                                        Thống kê khiếu nại theo nguyên nhân lỗi
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoChatLuongPhucVu" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongPhucVu" OnClick="lbReportVNP_Click">
                                                        Báo cáo chất lượng phục vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoChatLuongMang" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongMang" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp chất lượng mạng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoTonDongQuaHan" runat="server" CommandArgument="bc_VNP_BaoCaoTonDongQuaHan" OnClick="lbReportVNP_Click">
                                                        Báo cáo tồn đọng quá hạn
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoDVGTGTTapDoan" runat="server" CommandArgument="bc_VNP_BaoCaoDVGTGTTapDoan" OnClick="lbReportVNP_Click">
                                                        Báo cáo dịch vụ GTGT (cho tập đoàn)
                                                    </asp:LinkButton>
                                                </li>
                                                <%-- <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoDVGTGTTapDoan_New" runat="server" CommandArgument="bc_VNP_BaoCaoDVGTGTTapDoan_New" OnClick="lbReportVNP_Click">
                                                               Báo cáo dịch vụ GTGT (cho tập đoàn - từ: 1/5/2016)
                                                    </asp:LinkButton>
                                                </li>--%>
                                                <li>
                                                    <h4>Báo cáo giảm trừ</h4>
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHopGiamTruVNPTTT" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTruVNPTTT" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp giảm trừ VNPT TT
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHopGiamTru" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTru" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp giảm trừ toàn mạng
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbReport_VNP_DanhSachGiamTruKhieuNai" runat="server" CommandArgument="bc_VNP_DanhSachGiamTruKhieuNai" OnClick="lbReportVNP_Click">
                                                                Danh sách chi tiết giảm trừ
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHopToGQKN" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopToGQKN" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp của tổ GQKN
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbReport_VNP_BaoCaoGiamTruToGQKN" runat="server" CommandArgument="bc_VNP_BaoCaoGiamTruToGQKN" OnClick="lbReportVNP_Click">
                                                                Báo cáo giảm trừ của tổ GQKN
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>

                                                <%--<li>
                                                    <asp:LinkButton ID="lbReport_VNP_ThongKeSoLuongGQKNTheo1KhoangThoiGian" runat="server" CommandArgument="bc_VNP_ThongKeSoLuongGQKNTheo1KhoangThoiGian" OnClick="lbReportVNP_Click">
                                                        Thống kế số lượng GQKN theo 1 khoảng thời gian
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_ThongKeSoLuongGQKNTheo2KhoangThoiGian" runat="server" CommandArgument="bc_VNP_ThongKeSoLuongGQKNTheo2KhoangThoiGian" OnClick="lbReportVNP_Click">
                                                        Thống kê số lượng GQKN theo 2 khoảng thời gian
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_ThongKeTongSoTienGiamTruTheo1KhoangThoiGian" runat="server" CommandArgument="bc_VNP_ThongKeTongSoTienGiamTruTheo1KhoangThoiGian" OnClick="lbReportVNP_Click">
                                                        Thống kê tổng số tiền giảm trừ theo 1 khoảng thời gian
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_ThongKeTongSoTienGiamTruTheo2KhoangThoiGian" runat="server" CommandArgument="bc_VNP_ThongKeTongSoTienGiamTruTheo2KhoangThoiGian" OnClick="lbReportVNP_Click">
                                                        Thống kê tổng số tiền giảm trừ theo 2 khoảng thời gian
                                                    </asp:LinkButton>
                                                </li>--%>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportGiamTruDVGTGT" visible="false">
                                            <h4>Báo cáo giảm trừ DV GTGT</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton7" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTruDVGTGTDonVi" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp giảm trừ DV GTGT theo đơn vị
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton8" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTruDVGTGTDichVu" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp giảm trừ DV GTGT theo dịch vụ
                                                    </asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoChiTietGiamTruDVGTGT" runat="server" CommandArgument="bc_VNP_BaoCaoChiTietGiamTruDVGTGT" OnClick="lbReportVNP_Click">
                                                                Báo cáo chi tiết giảm trừ DV GTGT
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet" OnClick="lbReportVNP_Click">
                                                                Báo cáo tổng hợp theo tiêu chí thời gian giải quyết
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNP_BaoCaoChiTietTheoChiTieuThoiGianGiaiQuyet" runat="server" CommandArgument="bc_VNP_BaoCaoChiTietTheoChiTieuThoiGianGiaiQuyet" OnClick="lbReportVNP_Click">
                                                                Báo cáo chi tiết theo chỉ tiêu thời gian giải quyết
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportGQKN" visible="false">
                                            <h4>Báo cáo tổ GQKN</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lblReport_GQKN_BaoCaoTongHopLoaiKhieuNai" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopLoaiKhieuNaiToGQKN" OnClick="lblReport_GQKN_BaoCaoTongHopLoaiKhieuNai_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport11" runat="server" CommandArgument="bc11" OnClick="lbReportVNP_Click">
                                                Báo cáo chi tiết giảm trừ cước dịch vụ trả trước
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport81" runat="server" CommandArgument="bc_GQKN_ChiTietGiamTruTraSauGQKN"
                                                        OnClick="lbReportVNP_Click">Báo cáo chi tiết giảm trừ cước dịch vụ trả sau tổ GQKN
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_GQKN_ChiTietGiamTruTraSauVNPTTT" runat="server" CommandArgument="bc_GQKN_ChiTietGiamTruTraSauVNPTTT"
                                                        OnClick="lbReportVNP_Click">Báo cáo chi tiết giảm trừ cước dịch vụ trả sau VNPT TT
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport21" runat="server" CommandArgument="bc21" OnClick="lbReportVNP_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport41" runat="server" CommandArgument="bc41" OnClick="lbReportVNP_Click">Báo cáo chi tiết PPS</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport51" runat="server" CommandArgument="bc51" OnClick="lbReportVNP_Click">Báo cáo chi tiết POST</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport61" runat="server" CommandArgument="bc61" OnClick="lbReportVNP_Click">Báo cáo theo mẫu số 5</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon_GQKN" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_GQKN_BaoCaoKhieuNaiChuyenBoPhanKhac" Visible="true" runat="server" CommandArgument="bc_BaoCaoKhieuNaiChuyenBoPhanKhac" OnClick="lbReportVNP_Click">Báo cáo khiếu nại chuyển bộ phận khác</asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_GQKN_PhongBanCaNhan_TongHopPhongBan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopPhongBan" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của phòng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_GQKN_PhongBanCaNhan_TongHopNguoiDung" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopNguoiDung" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của người dùng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <h4>Báo cáo VNPT TT</h4>
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_GiamTruKhieuNaiDichVu" runat="server" CommandArgument="bc_CSKHKV_GiamTruKhieuNaiDichVu"
                                                                OnClick="lbReportCSKHKhuVuc_Click">Báo cáo giảm trừ do khiếu nại dịch vụ
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_KhieuNaiDichVu" runat="server" CommandArgument="bc_CSKHKV_KhieuNaiDichVu"
                                                                OnClick="lbReportCSKHKhuVuc_Click">Báo cáo tình hình khiếu nại dịch vụ
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_ThucTrangGQKN" runat="server" CommandArgument="bc_CSKHKV_ThucTrangGQKN"
                                                                OnClick="lbReportCSKHKhuVuc_Click">Báo cáo thực trạng GQKN
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP"
                                                                OnClick="lbReportCSKHKhuVuc_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_BaoCaoTongHop" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHop" OnClick="lbReportCSKHKhuVuc_Click">
                                                                Báo cáo tổng hợp PAKN
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lblReport_GQKN_CSKHKV_BaoCaoTongHopGiamTruVNPTTT" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopGiamTruVNPTTT" OnClick="lbReportCSKHKhuVuc_Click">
                                                                Báo cáo tổng hợp giảm trừ VNPT TTP
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportXLNV" visible="false">
                                            <h4>Báo cáo tổ XLNV</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport31" runat="server" CommandArgument="bc31" OnClick="lbReportVNP_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_BaoCaoKhoiLuongCongViec" runat="server" CommandArgument="bc_XLNV_BaoCaoKhoiLuongCongViec" OnClick="lbReportVNP_Click">Báo cáo khối lượng công việc</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon_XLNV" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_BaoCaoTonDongQuaHanCuaDoiTac" runat="server" CommandArgument="bc_XLNV_BaoCaoTonDongQuaHanCuaDoiTac" OnClick="lbReportVNP_Click">Báo cáo tồn đọng và quá hạn của đối tác</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_BaoCaoKhieuNaiChuyenBoPhanKhac" Visible="true" runat="server" CommandArgument="bc_BaoCaoKhieuNaiChuyenBoPhanKhac" OnClick="lbReportVNP_Click">Báo cáo khiếu nại chuyển bộ phận khác</asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_PhongBanCaNhan_TongHopPhongBan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopPhongBan" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của phòng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_PhongBanCaNhan_TongHopNguoiDung" Visible="false" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopNguoiDung" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của người dùng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_ThongKeKhieuNaiTheoNguyenNhanLoi" runat="server" CommandArgument="bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi" OnClick="lbReportVNP_Click">
                                                        Thống kê khiếu nại theo nguyên nhân lỗi
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_XLNV_DanhSachQuaHan" runat="server" CommandArgument="bc_VNP_DanhSachQuaHan" OnClick="lbReportVNP_Click">
                                                        Lịch sử khiếu nại quá hạn ở phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportKS" visible="false">
                                            <h4>Báo cáo tổ KS</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_KS_BaoCaoTongHopKN" runat="server" CommandArgument="bc_KS_BaoCaoTongHopKN" OnClick="lbReportVNP_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon_KS" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_KS_PhongBanCaNhan_TongHopPhongBan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopPhongBan" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của phòng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_KS_PhongBanCaNhan_TongHopNguoiDung" Visible="false" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopNguoiDung" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của người dùng
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportOB" visible="false">
                                            <h4>Báo cáo tổ OB</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_OB_BaoCaoTongHopKN" runat="server" CommandArgument="bc_OB_BaoCaoTongHopKN" OnClick="lbReportVNP_Click">Báo cáo tổng hợp theo loại khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon_OB" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_OB_PhongBanCaNhan_TongHopPhongBan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopPhongBan" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của phòng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_OB_PhongBanCaNhan_TongHopNguoiDung" Visible="false" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopNguoiDung" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của người dùng
                                                    </asp:LinkButton>
                                                </li>

                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportKTV" visible="false">
                                            <h4>Báo cáo tổ trưởng tổ KTV </h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV" runat="server" CommandArgument="bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV" OnClick="lbReportVNP_Click">Báo cáo số lượng KN bị phản hồi về của KTV</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV" runat="server" CommandArgument="bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV" OnClick="lbReportVNP_Click">Báo cáo khiếu nại quá hạn của KTV</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon_KTV" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportTTTC" visible="false">
                                            <h4>Báo cáo Ban KTNV</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_TongHopPAKN" runat="server" CommandArgument="bc_TTTC_TongHopPAKN" OnClick="lbReportTTTC_Click">Báo cáo tổng hợp PAKN TTTC</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_TongHopPAKNTheoPhongBan" runat="server" CommandArgument="bc_TTTC_TongHopPAKNTheoPhongBan" OnClick="lbReportTTTC_Click">Báo cáo tổng hợp PAKN theo phòng ban</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_TongHopPAKNTheoNguoiDung" runat="server" CommandArgument="bc_TTTC_TongHopPAKNTheoNguoiDung" OnClick="lbReportTTTC_Click">Báo cáo tổng hợp PAKN theo người dùng</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_ChiTietPAKNTheoNguoiDung" runat="server" CommandArgument="bc_TTTC_ChiTietPAKNTheoNguoiDung" OnClick="lbReportTTTC_Click" Visible="false">Báo cáo chi tiết PAKN theo người dùng</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_BaoCaoPhoiHopGQKN" runat="server" CommandArgument="bc_TTTC_BaoCaoPhoiHopGQKN" OnClick="lbReportTTTC_Click">Báo cáo phối hợp giải quyết khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_BaoCaoQuaHanPhongBan" runat="server" CommandArgument="bc_TTTC_BaoCaoQuaHanPhongBan" OnClick="lbReportTTTC_Click">Báo cáo tồn đọng và quá hạn phòng ban (Real time)</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_DanhSachQuaHan" runat="server" CommandArgument="bc_TTTC_DanhSachQuaHan" OnClick="lbReportTTTC_Click">
                                                        Lịch sử khiếu nại quá hạn ở phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_BaoCaoTongHopGiamTru" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTru" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp giảm trừ toàn mạng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_VNP_KhieuNaiToanMang" runat="server" CommandArgument="bc_VNP_KhieuNaiToanMang" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp khiếu nại toàn mạng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi" runat="server" CommandArgument="bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi" OnClick="lbReportVNP_Click">
                                                        Thống kê khiếu nại theo nguyên nhân lỗi
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportTTTC_VNP_BaoCaoChatLuongMang" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongMang" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp chất lượng mạng
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportVAS" visible="false">
                                            <h4>Trung tâm phát triển dịch vụ
                                            </h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_SoLieuPAKNDichVuToanMang" runat="server" CommandArgument="bc_VAS_SoLieuPAKNDichVuToanMang"
                                                        OnClick="lbReportVAS_Click">Báo cáo số liệu PAKN dịch vụ toàn mạng</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VAS_TongHopPAKN" runat="server" CommandArgument="bc_VAS_TongHopPAKNVAS" OnClick="lbReportVAS_Click">Báo cáo tổng hợp PAKN trung tâm</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VAS_TongHopPAKNTheoNguoiDung" runat="server" CommandArgument="bc_VAS_TongHopPAKNTheoNguoiDung" OnClick="lbReportVAS_Click">Báo cáo tổng hợp PAKN theo người dùng</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_SoLieuPAKNDaXuLy" Visible="false" runat="server" CommandArgument="bc_VAS_SoLieuPAKNDaXuLy"
                                                        OnClick="lbReportVAS_Click">Báo cáo số liệu PAKN đã xử lý</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TongHopSoLieuPAKNDangTonDong" Visible="false" runat="server" CommandArgument="bc_VAS_TongHopSoLieuPAKNDangTonDong"
                                                        OnClick="lbReportVAS_Click">Báo cáo tổng hợp số liệu PAKN đang tồn đọng</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TongHopSoLieuPAKNDaTiepNhan" Visible="false" runat="server" CommandArgument="bc_VAS_TongHopSoLieuPAKNDaTiepNhan" OnClick="lbReportVAS_Click">Báo cáo tổng hợp số liệu PAKN đã tiếp nhận</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_ChiTietPAKNDaTiepNhan" Visible="false" runat="server" CommandArgument="bc_VAS_ChiTietPAKNDaTiepNhan" OnClick="lbReportVAS_Click">Báo cáo chi tiết PAKN đã tiếp nhận</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportVAS_BaoCaoTongDonNguoiDungPhongBan" Visible="true" runat="server" CommandArgument="bc_Common_BaoCaoTonDongNguoiDungPhongBan" OnClick="lbReport_Common_Click">Báo cáo tồn đọng khiếu nại</asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportVAS_DanhSachQuaHan" runat="server" CommandArgument="bc_VAS_DanhSachQuaHan" OnClick="lbReportVAS_Click">
                                                        Lịch sử khiếu nại quá hạn ở phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReportVAS_BaoCaoTongHopGiamTru" runat="server" CommandArgument="bc_VNP_BaoCaoTongHopGiamTru" OnClick="lbReportVNP_Click">
                                                        Báo cáo tổng hợp giảm trừ toàn mạng
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <%--<li>
                                            <h4>Báo cáo dạng biểu đồ</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReportBieuDo_TheoLinhVucCon" runat="server" CommandArgument="bcBieuDo_TheoLinhVucCon" OnClick="lbReportVNP_Click">Số lượng khiếu nại đã tạo</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>--%>
                                        <li runat="server" id="listReportCSKHKhuVuc" visible="false">
                                            <h4>Phòng CSKH khu vực</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_GiamTruKhieuNaiDichVu" runat="server" CommandArgument="bc_CSKHKV_GiamTruKhieuNaiDichVu"
                                                        OnClick="lbReportCSKHKhuVuc_Click">Báo cáo giảm trừ do khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_KhieuNaiDichVu" runat="server" CommandArgument="bc_CSKHKV_KhieuNaiDichVu"
                                                        OnClick="lbReportCSKHKhuVuc_Click">Báo cáo tình hình khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_ThucTrangGQKN" runat="server" CommandArgument="bc_CSKHKV_ThucTrangGQKN"
                                                        OnClick="lbReportCSKHKhuVuc_Click">Báo cáo thực trạng GQKN
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP"
                                                        OnClick="lbReportCSKHKhuVuc_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_BaoCaoTongHop" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHop" OnClick="lbReportCSKHKhuVuc_Click">
                                                        Báo cáo tổng hợp PAKN
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_BaoCaoTongHopGiamTruVNPTTT" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopGiamTruVNPTTT" OnClick="lbReportCSKHKhuVuc_Click">
                                                        Báo cáo tổng hợp giảm trừ VNPT TTP
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_DanhSachKhieuNaiQuaHan" runat="server" CommandArgument="bc_CSKHKV_DanhSachQuaHan" OnClick="lbReportCSKHKhuVuc_Click">
                                                        Lịch sử khiếu nại quá hạn ở phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_BaoCaoTongHopTaiPhongBan" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopTaiPhongBan" OnClick="lbReportCSKHKhuVuc_Click" Visible="false">
                                                        Báo cáo tổng hợp của phòng CSKH khu vực
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CSKHKV_BaoCaoTongHopNguoiDungTaiPhongBan" runat="server" CommandArgument="bc_CSKHKV_BaoCaoTongHopNguoiDungTaiPhongBan" OnClick="lbReportCSKHKhuVuc_Click" Visible="false">
                                                        Báo cáo tổng hợp của người dùng tại phòng CSKH khu vực
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportVNPTTT" visible="false">
                                            <h4>VNPT tỉnh thành</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_GiamTruKhieuNaiDichVu" runat="server" CommandArgument="bc_VNPTTT_GiamTruKhieuNaiDichVu"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo giảm trừ do khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_KhieuNaiDichVu" runat="server" CommandArgument="bc_VNPTTT_KhieuNaiDichVu"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo tình hình khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_ThucTrangGQKN" runat="server" CommandArgument="bc_VNPTTT_ThucTrangGQKN"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo thực trạng GQKN
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP" runat="server" CommandArgument="bc_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_TongHopPAKNTVTT" runat="server" CommandArgument="bc_VNPTTT_TongHopPAKNTVTT"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo tổng hợp VTT
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_TongHopPAKNPhongBanVTT" runat="server" CommandArgument="bc_VNPTTT_TongHopPAKNPhongBanVTT"
                                                        OnClick="lbReportVNPTTT_Click">Báo cáo tổng hợp phòng ban VTT
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_TongHopPAKNTheoNguoiDung" Visible="false" runat="server" CommandArgument="bc_VNPTTT_TongHopPAKNTheoNguoiDung" OnClick="lbReportVNPTTT_Click">Báo cáo tổng hợp PAKN theo người dùng</asp:LinkButton>
                                                </li>

                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTTT_TongHopPAKNTheoThang" Visible="false" runat="server" CommandArgument="bc_VNPTTT_TongHopPAKNTheoThang" OnClick="lbReportVNPTTT_Click">Báo cáo tổng hợp PAKN theo tháng</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportTruongDaiDien" visible="false">
                                            <h4>Trưởng đại diện</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TDD_GiamTruKhieuNaiDichVu" runat="server" CommandArgument="bc_VNPTTT_GiamTruKhieuNaiDichVu"
                                                        OnClick="lbReportTDD_Click">Báo cáo giảm trừ do khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TDD_KhieuNaiDichVu" runat="server" CommandArgument="bc_VNPTTT_KhieuNaiDichVu"
                                                        OnClick="lbReportTDD_Click">Báo cáo tình hình khiếu nại dịch vụ
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TDD_ThucTrangGQKN" runat="server" CommandArgument="bc_VNPTTT_ThucTrangGQKN"
                                                        OnClick="lbReportTDD_Click">Báo cáo thực trạng GQKN
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_TDD_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP" runat="server" CommandArgument="bc_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP"
                                                        OnClick="lbReportTDD_Click">Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li runat="server" id="listReportDoiTac" visible="false">
                                            <h4>Đối tác</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_DoiTac_BaoCaoSoLuongChuyenXuLyVNP" runat="server" CommandArgument="bc_DoiTac_BaoCaoSoLuongChuyenXuLyVNP"
                                                        OnClick="lbReport_DoiTac_Click">Báo cáo số lượng chuyển xử lý VNP
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_DoiTac_TongHopDonVi" runat="server" CommandArgument="bc_Common_TongHopDonVi"
                                                        OnClick="lbReport_Common_Click">Báo cáo tổng hợp đơn vị
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_DoiTac_TongHopPhongBan" runat="server" CommandArgument="bc_Common_TongHopPhongBan"
                                                        OnClick="lbReport_Common_Click">Báo cáo tổng hợp phòng ban
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_DoiTac_TongHopNguoiDung" Visible="false" runat="server" CommandArgument="bc_Common_TongHopNguoiDung"
                                                        OnClick="lbReport_Common_Click">Báo cáo tổng hợp PAKN theo người dùng</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li runat="server" id="listReportVNPTNET" visible="false">
                                            <h4>VNPT NET</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTNET_BaoCaoChatLuongMang" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongMang" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp chất lượng mạng
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTNET_BaoCaoTongHopVNPTNET" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopVNPTNET" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp VNPT NET
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTNET_BaoCaoTongHopDonVi" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopDonVi" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp các đơn vị
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTNET_BaoCaoTongHopPhongBan" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopPhongBan" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp phòng ban
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_VNPTNET_BaoCaoTongHopNguoiDung" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopNguoiDung" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp người dùng
                                                    </asp:LinkButton>

                                                </li>
                                            </ul>
                                        </li>

                                        <li runat="server" id="listReportHaTangMang" visible="false">
                                            <h4>TT Hạ tầng mạng</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReportHTM_VNP_BaoCaoChatLuongMang" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongMang" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp chất lượng mạng
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopDonVi" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp các đơn vị
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopPhongBan" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp phòng ban
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopNguoiDung" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp người dùng
                                                    </asp:LinkButton>

                                                </li>
                                            </ul>
                                        </li>

                                        <li runat="server" id="listReportDHTT" visible="false">
                                            <h4>TT ĐHTT</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReportDHTT_VNP_BaoCaoChatLuongMang" runat="server" CommandArgument="bc_VNP_BaoCaoChatLuongMang" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp chất lượng mạng
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopDonVi" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp các đơn vị
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopPhongBan" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp phòng ban
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopNguoiDung" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp người dùng
                                                    </asp:LinkButton>

                                                </li>
                                            </ul>
                                        </li>

                                        <li runat="server" id="listReportNETCNTT" visible="false">
                                            <h4>TT CNTT</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton9" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopPhongBan" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp phòng ban
                                                    </asp:LinkButton>

                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton10" runat="server" CommandArgument="bc_VNPTNET_BaoCaoTongHopNguoiDung" OnClick="lbReportVNPTNET_Click">
                                                        Báo cáo tổng hợp người dùng
                                                    </asp:LinkButton>

                                                </li>
                                            </ul>
                                        </li>

                                        <%-- <li runat="server" id="listReportPhongBanCaNhan" visible="true">
                                            <h4>Tổng hợp phòng ban</h4>
                                            <ul>                                              
                                                <li>
                                                    <asp:LinkButton ID="lbReport_PhongBanCaNhan_TongHopPhongBan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopPhongBan" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của phòng
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_PhongBanCaNhan_TongHopCaNhan" runat="server" CommandArgument="bc_Common_PhongBanCaNhan_TongHopNguoiDung" OnClick="lbReport_Common_Click">
                                                        Báo cáo tổng hợp của người dùng
                                                    </asp:LinkButton>
                                                </li>                                          
                                            </ul>
                                        </li>--%>

                                        <li runat="server" id="listReportCaNhan" visible="true">
                                            <h4>Cá nhân</h4>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lbReport_CaNhan_TongHopCaNhan" runat="server" CommandArgument="bc_CaNhan_TongHopCaNhan" OnClick="lbReport_Common_Click">Báo cáo tổng hợp cá nhân
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="col_right_pagebaocao">
                                <div class="wr">
                                    <h4>BÁO CÁO KHIẾU NẠI</h4>
                                    <% if (!IsPostBack)
                                        {%>
                                    <h3 style="margin-top: 20px; text-align: center; color: #2DA0AA; font-size: 14px;">Vui lòng chọn báo cáo</h3>
                                    <% } %>
                                    <asp:Panel ID="pnContainer" runat="server"></asp:Panel>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
