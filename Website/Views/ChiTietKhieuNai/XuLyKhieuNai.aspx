<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="XuLyKhieuNai.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.XuLyKhieuNai" %>

<%@ Register TagPrefix="uc" TagName="ChiTietKhieuNai" Src="~/Views/ChiTietKhieuNai/UC/ucChiTietKhieuNai.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ContentPlaceHolderID="HeaderJs">
    <script src="/Content/Chosen/chosen.jquery.js" type="text/javascript"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeaderCss">
    <link href="/Content/Chosen/chosen.css" rel="stylesheet" />
    <style type="text/css">
        #contain { position: static; }
        .chosen-container .chosen-results li.active-result { float: none; display: block; }
        .inputstyle_longlx textarea { padding: 5px; }
        .xlkn_body td { padding-left: 5px; }
    </style>    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script language="javascript" type="text/javascript">
                $(document).ready(function () {
                    fnSetSizeDiv();
                });

                window.onresize = function (event) {
                    fnSetSizeDiv();
                }

                function PrintPhieuKhieuNai() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuKhieuNai.aspx?MaKN=" + MaKN;
                    window.open(url);
                }

                function PrintPhieuXacMinh() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuXacMinh.aspx?MaKN=" + MaKN;
                    window.open(url);
                }

                function PrintPhieuChuyenTiep() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuChuyenTiep.aspx?MaKN=" + MaKN;
                    window.open(url);
                }

                function PrintPhieuTiepNhan() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuTiepNhan.aspx?MaKN=" + MaKN;
                    window.open(url);
                }

                function PrintPhieuTraLoi() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuTraLoi.aspx?MaKN=" + MaKN;
                    window.open(url);
                }
                function ClosePoup() {
                    $('.divOpacity').css('display', 'none');
                    $('#divPoupPhongBan').hide();
                }

                function ShowPoupChuyenXuLy() {
                    // Nếu loại khiếu nại là chất lượng mạng thì kiểm tra phải nhập đầy đủ Lĩnh vực chung, Lĩnh vực con, tỉnh thành, quận huyện, địa điểm xảy ra sự cố
                    var loaiKhieuNaiId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLoaiKhieuNai").val();
                    if (loaiKhieuNaiId == "71") {
                        var linhVucChungId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucChung").val();
                        var linhVucConId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucCon").val();
                        var tinhId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlTinh").val();
                        var quanHuyenId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlHuyen").val();
                        var phuongXaId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlPhuongXa").val();
                        var diaDiemXayRaSuCo = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtDiaDiemSuCo").val();

                        if (linhVucChungId == "0" || linhVucConId == "0" || tinhId == "0" || quanHuyenId == "0" || phuongXaId == "0" || diaDiemXayRaSuCo == "") {
                            MessageAlert.AlertNormal("Khiếu nại chất lượng mạng phải nhập đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Tỉnh/Thành, Quận/Huyện, Phường/Xã và Địa điểm xảy ra sự cố", 'error');
                            return false;
                        }
                    }

                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                        data: {
                            type: "CheckDinhTuyen"
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            // Giá trị trả về của data
                            //      1 : Hiển thị danh sách các phòng ban được phép chuyển xử lý
                            //      2 : Hiển thị màn hình tự động định tuyến tới phòng ban tương ứng
                            //      other : Nội dung lỗi exception
                            if (data == "1") {
                                $('#divPoupPhongBan').show();
                                $('.divOpacity').css('display', 'block');
                                fnLoadPhongBanChuyenXuLy();
                            }
                            else if (data == "2") {
                                ShowPopupChuyenXuLyAuto();
                            }
                            else {
                                MessageAlert.AlertNormal(data, 'error');
                            }
                        }
                    });
                }

                function ClosePoupChuyenPhanHoi() {
                    $('.divOpacity').css('display', 'none');
                    $('#divPoupChuyenPhanHoi').hide();
                }

                function ShowPoupChuyenPhanHoi() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx?KhieuNaiId=" + MaKN,
                        data: {
                            type: "PhongBanNhanPhanHoi"
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            if (data != "1") {
                                $('#slPhongBanNhanPhanHoi').html(data);
                            }
                            $('#divPoupChuyenPhanHoi').show();
                            $('.divOpacity').css('display', 'block');
                        }
                    });
                }


                function ClosePoupChuyenPhanHoiTrungTam() {
                    $('.divOpacity').css('display', 'none');
                    $('#divPoupChuyenPhanHoiTrungTam').hide();
                }

                function ShowPoupChuyenPhanHoiTrungTam() {
                    $('#divPoupChuyenPhanHoiTrungTam').show();
                    $('.divOpacity').css('display', 'block');
                }

                function ClosePoupChuyenNgangHang() {
                    $('.divOpacity').css('display', 'none');
                    $('#divPoupChuyenNgangHang').hide();
                }

                function ShowPoupChuyenNgangHang() {
                    // Nếu loại khiếu nại là chất lượng mạng thì kiểm tra phải nhập đầy đủ Lĩnh vực chung, Lĩnh vực con, tỉnh thành, quận huyện, địa điểm xảy ra sự cố
                    var loaiKhieuNaiId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLoaiKhieuNai").val();
                    if (loaiKhieuNaiId == "71") {
                        var linhVucChungId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucChung").val();
                        var linhVucConId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucCon").val();
                        var tinhId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlTinh").val();
                        var quanHuyenId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlHuyen").val();
                        var phuongXaId = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlPhuongXa").val();
                        var diaDiemXayRaSuCo = $("#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtDiaDiemSuCo").val();

                        if (linhVucChungId == "0" || linhVucConId == "0" || tinhId == "0" || quanHuyenId == "0" || phuongXaId == "0" || diaDiemXayRaSuCo == "") {
                            MessageAlert.AlertNormal("Khiếu nại chất lượng mạng phải nhập đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Tỉnh/Thành, Quận/Huyện, Phường/Xã và Địa điểm xảy ra sự cố", 'error');
                            return false;
                        }
                    }

                    $('#divPoupChuyenNgangHang').show();
                    $('.divOpacity').css('display', 'block');
                }

                function ClosePoupChuyenXuLyAuTo() {
                    $('.divOpacity').css('display', 'none');
                    $('#divPoupChuyenXuLyAuTo').hide();
                }

                function ShowPopupChuyenXuLyAuto() {
                    $('#divPoupChuyenXuLyAuTo').show();
                    $('.divOpacity').css('display', 'block');
                    fnLoadTuDongDinhTuyenAndPhongBanCungDoiTac();

                }

                function ClosePoupDongKN() {
                    $('.divOpacity').css('display', 'none');
                    $('#divDongKN').hide();
                }

                function ClosePoupKhoaPhieu() {
                    $('.divOpacity').css('display', 'none');
                    $('#divKhoaPhieuVNPT').hide();
                }

                function ShowPopupDongKN() {
                    var checkLoaiKhieuNaiIsCuoc = $('#<%=ddlLoaiKhieuNai.ClientID%>').val();
                    if (checkLoaiKhieuNaiIsCuoc == 30 || checkLoaiKhieuNaiIsCuoc == 35) {
                        var checkTinh = $('#<%=ddlTinh.ClientID%>').val();
                        if (checkTinh == 0) {
                            MessageAlert.AlertNormal('Bạn phải chọn tỉnh của khiếu nại.', 'error');
                            return;
                        }
                    }

                    $('#divDongKN').show();
                    $('.divOpacity').css('display', 'block');
                }

                function ShowPopupKhoaPhieuVNPT() {
                    $('#divKhoaPhieuVNPT').show();
                    $('.divOpacity').css('display', 'block');
                }

                function fnDongKhieuNai() {
                    var mode = Utility.GetUrlParam("Mode");
                    if (mode == "Process") {
                        var MaKN = Utility.GetUrlParam("MaKN");

                        var nguyenNhanLoiId = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlNguyenNhanLoi').val();
                        var chiTietLoiId = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlChiTietLoi').val();
                        if (nguyenNhanLoiId <= 0) {
                            MessageAlert.AlertNormal('Bạn phải chọn nguyên nhân lỗi.', 'error');
                            return;
                        }

                        if (chiTietLoiId < 0) {
                            MessageAlert.AlertNormal('Bạn phải chọn chi tiết nguyên nhân lỗi', 'error');
                            return;
                        }

                        var DoHaiLong = $('#slDoHaiLong').val();
                        if (DoHaiLong != '-1') {
                            $.ajax({
                                beforeSend: function () {
                                    $('#divDongKN').hide();
                                },
                                type: "POST",
                                dataType: "text",
                                url: "../Ajax/Ajax.ashx",
                                data: { type: "DongKNHaiLong", MaKN: MaKN, DoHaiLong: DoHaiLong, nguyenNhanLoiId: nguyenNhanLoiId, chiTietLoiId: chiTietLoiId },
                                success: function (text) {
                                    ClosePoupDongKN();
                                    if (text == "") {
                                        var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                        if (strURL == "")
                                            strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công', 'info', strURL);
                                    }
                                    else if (text == "10000") {
                                        var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                        if (strURL == "")
                                            strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.', 'info', strURL);
                                    }
                                    else if (text == "10001") {
                                        var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                        if (strURL == "")
                                            strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Không khóa được phiếu VNPT.', 'warning', strURL);
                                    }
                                    else if (text == "10099") {
                                        var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                        if (strURL == "")
                                            strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống kiểm tra.', 'warning', strURL);
                                    }
                                    else if (text == "0") {
                                        MessageAlert.AlertNormal('Mã khiếu nại không hợp lệ.', 'error');
                                    }
                                    else if (text == "1") {
                                        MessageAlert.AlertNormal('Bạn chưa thêm nội dung xử lý khiếu nại.', 'error');
                                    }
                                    else if (text == "2") {
                                        MessageAlert.AlertNormal('Bạn chưa thêm kết quả giải quyết khiếu nại do đây là khiếu nại giảm trừ cước.', 'error');
                                    }
                                    else {
                                        MessageAlert.AlertNormal(text, 'error');
                                    }
                                },
                                error: function () {
                                    ClosePoupDongKN();
                                }
                            });
                        }
                        else {
                            MessageAlert.AlertNormal('Bạn chưa chọn độ hài lòng của khách hàng.', 'error');
                        }

                    }
                }


                function fnKhoaPhieuVNPT() {
                    var mode = Utility.GetUrlParam("Mode");
                    if (mode == "Process") {
                        var MaKN = Utility.GetUrlParam("MaKN");
                        $.ajax({
                            beforeSend: function () {
                                $('#divKhoaPhieuVNPT').hide();
                            },
                            type: "POST",
                            dataType: "text",
                            url: "../Ajax/Ajax.ashx",
                            data: { type: "KhoaPhieuVNPT", MaKN: MaKN },
                            success: function (text) {
                                ClosePoupKhoaPhieu();
                                if (text == "") {
                                    var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                    if (strURL == "")
                                        strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                    MessageAlert.AlertRedirect('Khóa phiếu về VNPT thành công', 'info', strURL);
                                }
                                else if (text == "10000") {
                                    var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                    if (strURL == "")
                                        strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                    MessageAlert.AlertRedirect('Khóa phiếu về VNPT thành công.', 'info', strURL);
                                }
                                else if (text == "10001") {
                                    var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                    if (strURL == "")
                                        strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                    MessageAlert.AlertRedirect('Không khóa được phiếu VNPT.', 'warning', strURL);
                                }
                                else if (text == "10099") {
                                    var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                                    if (strURL == "")
                                        strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                                    MessageAlert.AlertRedirect('Service khóa phiếu VNPT có lỗi.', 'warning', strURL);
                                }
                                else {
                                    MessageAlert.AlertNormal(text, 'error');
                                }
                            },
                            error: function () {
                                ClosePoupKhoaPhieu();
                            }
                        });
                    }
                }

                function ChuyenXuLyAuto() {
                    var mode = Utility.GetUrlParam("Mode");

                    if (mode == "Process") {
                        var MaKN = Utility.GetUrlParam("MaKN");
                        var strURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');

                        if (strURL == "")
                            strURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                        var phongBanId = $('#ddlTuDongDinhTuyenAndPhongBanCungDoiTac').val();
                        if (phongBanId == 0) // Từ động định tuyến
                        {
                            $.ajax({
                                beforeSend: function () {
                                    $('#divPoupChuyenXuLyAuTo').hide();
                                },
                                type: "POST",
                                dataType: "text",
                                url: "../Ajax/Ajax.ashx",
                                data: { type: "ChuyenKNAuto", MaKN: MaKN, Mode: mode, NoiDungXuLy: '' },
                                success: function (text) {
                                    ClosePoupChuyenXuLyAuTo();
                                    if (text == "") {
                                        MessageAlert.AlertRedirect('Chuyển khiếu nại thành công', 'info', strURL);
                                    }
                                    else if (text == "1000") {

                                        MessageAlert.AlertRedirect('Chuyển khiếu nại về PTDV thành công.', 'info', strURL);
                                    }
                                    else if (text == "10000") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.', 'info', strURL);
                                    }
                                    else if (text == "10001") {
                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Không khóa được phiếu VNPT.', 'warning', strURL);
                                    }
                                    else if (text == "10099") {
                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống kiểm tra.', 'warning', strURL);
                                    }
                                    else {
                                        MessageAlert.AlertNormal(text, 'error');
                                    }
                                },
                                error: function () {
                                    ClosePoupChuyenXuLyAuTo();
                                }
                            });
                        }
                        else if (phongBanId == -1) // Định tuyến lên VNP cho tỉnh
                        {
                            $.ajax({
                                beforeSend: function () {
                                    $('#divPoupChuyenXuLyAuTo').hide();
                                },
                                type: "POST",
                                dataType: "text",
                                url: "../Ajax/Ajax.ashx",
                                data: { type: "ChuyenKNAutoVNP", MaKN: MaKN, Mode: mode, NoiDungXuLy: '' },
                                success: function (text) {
                                    ClosePoupChuyenXuLyAuTo();
                                    if (text == "") {
                                        MessageAlert.AlertRedirect('Chuyển khiếu nại thành công', 'info', strURL);
                                    }
                                    else if (text == "1000") {

                                        MessageAlert.AlertRedirect('Chuyển khiếu nại về PTDV thành công.', 'info', strURL);
                                    }
                                    else if (text == "10000") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.', 'info', strURL);
                                    }
                                    else if (text == "10001") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Không khóa được phiếu VNPT.', 'warning', strURL);
                                    }
                                    else if (text == "10099") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống kiểm tra.', 'warning', strURL);
                                    }
                                    else {
                                        MessageAlert.AlertNormal(text, 'error');
                                    }
                                },
                                error: function () {
                                    ClosePoupChuyenXuLyAuTo();
                                }
                            });
                        }
                        else // Chuyển khiếu nại tới phongBanId
                        {

                            var Username = $('#ddlUserInPhongBan_divPoupChuyenXuLyAuTo').val();
                            var MaKN = Utility.GetUrlParam("MaKN");
                            var Mode = Utility.GetUrlParam("Mode");

                            var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');
                            if (returnURL == "") {
                                returnURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";
                            }
                            $.ajax({
                                type: "GET",
                                beforeSend: function () {
                                    $('#divPoupChuyenXuLyAuTo').hide();
                                },
                                url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                                data: {
                                    type: "ChuyenXuLy",
                                    PhongBanId: phongBanId,
                                    Username: Username,
                                    MaKN: MaKN,
                                    Mode: Mode,
                                    Note: ''
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "text",
                                success: function (text) {
                                    ClosePoup();
                                    if (text == "") {
                                        MessageAlert.AlertRedirect('Chuyển khiếu nại thành công', 'info', returnURL);
                                    }
                                    else if (text == "1000") {

                                        MessageAlert.AlertRedirect('Chuyển khiếu nại về PTDV thành công.', 'info', returnURL);
                                    }
                                    else if (text == "10000") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.', 'info', returnURL);
                                    }
                                    else if (text == "10001") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Không khóa được phiếu VNPT.', 'warning', returnURL);
                                    }
                                    else if (text == "10099") {

                                        MessageAlert.AlertRedirect('Đóng khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống kiểm tra.', 'warning', returnURL);
                                    }
                                    else {
                                        MessageAlert.AlertNormal(text, 'error');
                                    }
                                },
                                error: function () {
                                    ClosePoup();
                                    MessageAlert.AlertNormal('Chức năng chuyển xử lý khiếu nại gặp sự cố. Mời bạn thử lại sau ít phút.', 'error');
                                }
                            });
                        }

                    }
                }


                function fnSetSizeDiv() {
                    var d = $('body').innerWidth() - 56;
                    var h = screen.height;
                    $("#divScroll").css("width", d);
                    $(".divOpacity").css("height", h);

                }

                function fnLoadPhongBanChuyenXuLy() {
                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                        data: { type: "LoadPhongBanChuyenXuLy" },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            $('#ddlPhongBanChuyenXuLy').html(data);
                            // Chưa fix được hiển thị
                            // $("#ddlPhongBanChuyenXuLy").chosen();
                        }
                    });
                }


                function fnLoadTuDongDinhTuyenAndPhongBanCungDoiTac() {
                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                        data: { type: "LoadTuDongDinhTuyenAndPhongBanCungDoiTac" },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            $('#ddlTuDongDinhTuyenAndPhongBanCungDoiTac').html(data);
                        }
                    });

                }

                function fnPhongBanChuyenXuLyChange() {
                    var phongBanId = $('#ddlPhongBanChuyenXuLy').val();
                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                        data: {
                            type: "LoadUserInPhongBanChuyenXuLy",
                            PhongBanId: phongBanId
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            $('#ddlUserInPhongBan').html(data);
                        }
                    });
                }

                // Author : Phi Hoang Hai
                // Created date : 12/05/2014
                // Todo : Lấy danh sách user thuộc phòng ban
                //      ddlPhongBanId : Id của combobox phòng ban
                //      ddlUserInPhongBanId : Id của combobox user
                function fnLoadUserByPhongBanId(ddlPhongBanId, ddlUserInPhongBanId) {
                    var phongBanId = $('#' + ddlPhongBanId).val();
                    $.ajax({
                        type: "GET",
                        url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                        data: {
                            type: "LoadUserInPhongBanChuyenXuLy",
                            PhongBanId: phongBanId
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (data) {
                            $('#' + ddlUserInPhongBanId).html(data);
                        }
                    });
                }

                function QuayVe() {
                    var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');
                    if (returnURL == "") {
                        returnURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";
                    }
                    window.location.href = returnURL;
                }

                function ValidChuyenNgangHang() {

                    if ($('#hidUsername').val() != $('#hidNguoiXuLy').val()) {
                        MessageAlert.AlertNormal('Bạn chưa nhập nội dung xử lý.', 'error', 'ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung');
                        ClosePoupChuyenNgangHang();
                        return false;
                    }

                    return true;
                }

                function ValidChuyenPhanHoi() {

                    if ($('#hidUsername').val() != $('#hidNguoiXuLy').val()) {
                        MessageAlert.AlertNormal('Bạn chưa nhập nội dung xử lý.', 'error', 'ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung');
                        ClosePoupChuyenPhanHoi();
                        return false;
                    }

                    return true;
                }

                function ValidChuyenPhanHoiTrungTam() {

                    if ($('#hidUsername').val() != $('#hidNguoiXuLy').val()) {
                        MessageAlert.AlertNormal('Bạn chưa nhập nội dung xử lý.', 'error', 'ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung');
                        ClosePoupChuyenPhanHoiTrungTam();
                        return false;
                    }

                    return true;
                }

                function ChuyenXuLy() {
                    var phongBanId = $('#ddlPhongBanChuyenXuLy').val();

                    if (phongBanId == 0) {
                        MessageAlert.AlertNormal('Vui lòng chọn phòng ban cần chuyển xử lý.', 'error');

                    }
                    else if ($('#hidUsername').val() != $('#hidNguoiXuLy').val()) {
                        MessageAlert.AlertNormal('Bạn chưa nhập nội dung xử lý.', 'error', 'ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung');
                        ClosePoup();
                    }
                    else {
                        var Username = $('#ddlUserInPhongBan').val();
                        var MaKN = Utility.GetUrlParam("MaKN");
                        var Mode = Utility.GetUrlParam("Mode");
                        var returnURL = decodeURIComponent(Utility.GetUrlParam("ReturnUrl")).replace(/\+/g, '%20');
                        if (returnURL == "") returnURL = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy";

                        $.ajax({
                            type: "GET",
                            beforeSend: function () {
                                $('#divPoupPhongBan').hide();
                                common.loading();
                            },
                            url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
                            data: {
                                type: "ChuyenXuLy",
                                PhongBanId: phongBanId,
                                Username: Username,
                                MaKN: MaKN,
                                Mode: Mode,
                                Note: 'Chuyển xử lý khiếu nại'
                            },
                            contentType: "application/json; charset=utf-8",
                            dataType: "text",
                            success: function (text, status, xhr) {
                                ClosePoup();
                                var msg = JSON.parse(text);
                                if (console && console.log) console.log(msg);
                                if (msg.Code > 0) MessageAlert.AlertRedirect(msg.Message, "info", returnURL);
                                else MessageAlert.AlertRedirect(msg.Message, "error", returnURL);
                                common.unLoading();
                                //console.log(text);
                                //if (text == "") {
                                //    MessageAlert.AlertRedirect('Chuyển khiếu nại thành công', 'info', returnURL);
                                //}
                                //else if (text == "1000") {

                                //    MessageAlert.AlertRedirect('Chuyển khiếu nại về PTDV thành công.', 'info', returnURL);
                                //}
                                //else if (text == "10000") {

                                //    MessageAlert.AlertRedirect('Chuyển khiếu nại thành công.', 'info', returnURL);
                                //}
                                //else if (text == "10001") {

                                //    MessageAlert.AlertRedirect('Chuyển khiếu nại thành công.<br/>Không khóa được phiếu VNPT.', 'warning', returnURL);
                                //}
                                //else if (text == "10099") {

                                //    MessageAlert.AlertRedirect('Chuyển khiếu nại thành công.<br/>Service khóa phiếu VNPT có lỗi, liên hệ quản trị hệ thống kiểm tra.', 'warning', returnURL);
                                //}
                                //else {
                                //    MessageAlert.AlertNormal(text, 'error');
                                //}
                            },
                            error: function (text, status, xhr) {
                                ClosePoup();
                                if (console && console.log) console.log(text);
                                MessageAlert.AlertNormal('Chức năng chuyển xử lý khiếu nại gặp sự cố. Mời bạn thử lại sau ít phút.', 'error');
                            }
                        });
                    }
                }

            </script>
            <input id="hidUsername" runat="server" clientidmode="Static" type="hidden" value="" />
            <div class="nav_btn">
                <ul>
                    <li><a href="javascript:QuayVe();"><span class="back">Quay về</span></a></li>
                    <li runat="server" id="liChuyenXuLy"><a href="javascript:ShowPoupChuyenXuLy();"><span class="move_file">Chuyển xử lý</span></a>
                    </li>
                    <li runat="server" id="liChuyenPhanHoi"><a href="javascript:ShowPoupChuyenPhanHoi();"><span class="move_phanhoi">Chuyển phản hồi</span></a>
                    </li>
                    <li runat="server" id="liChuyenPhanHoiTrungTam"><a href="javascript:ShowPoupChuyenPhanHoiTrungTam();"><span class="move_phanhoi">Chuyển phản hồi trung tâm</span></a>
                    </li>
                    <li runat="server" id="liChuyenNgangHang"><a href="javascript:ShowPoupChuyenNgangHang();"><span class="move_nganghang">Chuyển ngang hàng</span></a>
                    </li>
                    <li runat="server" id="liDongKN"><a href="#" onclick="javascript:ShowPopupDongKN();"><span class="lock">Đóng khiếu nại
                    </span></a>
                    </li>
                    <li runat="server" id="liKhoaPhieu"><a href="#" onclick="javascript:ShowPopupKhoaPhieuVNPT();"><span class="lock">Khóa phiếu về VNPT
                    </span></a>
                    </li>
                    <li><a href="#" onclick="PrintPhieuKhieuNai();"><span class="print">In phiếu KN</span></a></li>
                    <li><a href="#" onclick="PrintPhieuXacMinh();"><span class="print">In phiếu xác minh</span></a></li>
                    <li><a href="#" onclick="PrintPhieuChuyenTiep();"><span class="print">In phiếu chuyển tiếp</span></a></li>
                    <li><a href="#" onclick="PrintPhieuTiepNhan();"><span class="print">In phiếu tiếp nhận</span></a></li>
                    <li><a href="#" onclick="PrintPhieuTraLoi();"><span class="print">In phiếu trả lời</span></a></li>
                </ul>
            </div>

            <div id="divChiTietKN-Title">
                <h1 id="info-MaKhieuNai">Mã khiếu nại:
                    <asp:Literal ID="ltMaKhieuNai" runat="server"></asp:Literal>
                </h1>
            </div>
            <table class="xlkn_body" cellpadding="0" cellspacing="0" style="white-space: nowrap;" width="100%">
                <colgroup>
                    <col width="150px" />
                    <col width="14%" />
                    <col width="130px" />
                    <col width="16%" />
                    <col width="130px" />
                    <col width="130px" />
                    <col width="110px" />
                    <col width="130px" />
                </colgroup>
                <tr style="line-height: 35px; background: #AEAEAE;">
                    <td colspan="4" style="padding-left: 15px;"><span style="font: 14px; font-weight: bold; color: #fff;">Thông tin khiếu nại</span></td>
                    <td colspan="4" style="padding-left: 15px; border-left: 1px solid #ccc;"><span style="font: 14px; font-weight: bold; color: #fff;">Thông tin xử lý</span></td>
                </tr>
                <tr>
                    <td style="position: relative;">Số thuê bao:
                        <div class="selectstyle_longlx" style="position: absolute; right: 0px; top: -2px; width: 100px;">
                            <div class="bg">
                                <asp:DropDownList ID="ddlLoaiThueBao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiThueBao_Changed">
                                    <asp:ListItem Value="0">Trả trước</asp:ListItem>
                                    <asp:ListItem Value="1">Trả sau</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%--<strong>(<asp:Literal ID="ltLoaiThueBao" runat="server"></asp:Literal>)</strong>--%>
                        
                    </td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtSoThueBao" runat="server" Width="80%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>Loại khiếu nại:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiKhieuNai_Changed"></asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Người xử lý:</td>
                    <td>
                        <asp:Literal ID="ltNguoiXuLy" runat="server"></asp:Literal>
                    </td>
                    <td>T/g cập nhật:</td>
                    <td>
                        <asp:Literal ID="ltNgayCapNhat" Text="" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Họ tên liên hệ:</td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtHoTen" AutoPostBack="true" OnTextChanged="txtHoTen_TextChanged" runat="server" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>Lĩnh vực chung:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlLinhVucChung" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChung_Changed" Width="90%">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Độ ưu tiên:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlDoUuTien" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlDoUuTien_Changed"></asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td>Quá hạn PB</td>
                    <td>
                        <asp:Literal ID="ltQuaHanPB_1" Text="" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Điện thoại liên hệ:</td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtDienThoaiLienHe" AutoPostBack="true" OnTextChanged="txtDienThoaiLienHe_TextChanged" runat="server" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>Lĩnh vực con:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlLinhVucCon" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucCon_Changed" Width="90%"></asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Người tiếp nhận:</td>
                    <td>
                        <asp:Literal ID="ltNguoiTiepNhan" runat="server"></asp:Literal></td>
                    <td>T/g tiếp nhận:</td>
                    <td>
                        <asp:Literal ID="ltNgayTiepNhan" Text="" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>T/g xảy ra sự cố:</td>
                    <td>
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtThoiGianSuCo" AutoPostBack="true" OnTextChanged="txtThoiGianSuCo_TextChanged" runat="server" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>Hình thức tiếp nhận:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlHTTiepNhan" runat="server" Width="90%"></asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Trạng thái:</td>
                    <td>
                        <asp:Literal ID="ltTrangThai" runat="server"></asp:Literal>
                    </td>
                    <td>T/g đóng:</td>
                    <td>
                        <asp:Literal ID="ltNgayDong" Text="" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Tỉnh/TP sự cố:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlTinh" OnSelectedIndexChanged="ddlTinh_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="90%"></asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td>Quận/Huyện sự cố:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlHuyen" OnSelectedIndexChanged="ddlHuyen_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="90%">
                                    <asp:ListItem Value="0">--Chọn Quận/Huyện--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Khiếu nại chờ đóng:</td>
                    <td>
                        <asp:CheckBox ID="chkKNChoDong" AutoPostBack="true" OnCheckedChanged="chkKNChoDong_CheckedChanged" runat="server" />
                    </td>
                    <td>Quá hạn TT</td>
                    <td>
                        <asp:Literal ID="ltQuaHanTT" Text="" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Phường/Xã sự cố:</td>
                    <td>
                        <div class="selectstyle_longlx">
                            <div class="bg">
                                <asp:DropDownList ID="ddlPhuongXa" OnSelectedIndexChanged="ddlPhuongXa_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="90%">
                                    <asp:ListItem Value="0">--Chọn Phường/Xã--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td></td>
                    <td></td>
                    <td style="border-left: 1px solid #ccc;">KN hàng loạt:</td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkHangLoat" AutoPostBack="true" OnCheckedChanged="chkHangLoat_Changed" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Địa điểm xảy ra sự cố:</td>
                    <td colspan="3">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtDiaDiemSuCo" AutoPostBack="true" OnTextChanged="txtDiaDiemSuCo_TextChanged" runat="server" Width="96%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td style="border-left: 1px solid #ccc;">Độ hài lòng:</td>
                    <td colspan="3">
                        <asp:Literal ID="ltTenDoHaiLong" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>Địa chỉ liên hệ:</td>
                    <td colspan="3">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtDiaChiLienHe" AutoPostBack="true" OnTextChanged="txtDiaChiLienHe_TextChanged" runat="server" Width="96%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td valign="top" style="border-left: 1px solid #ccc;" rowspan="3">Ghi chú:</td>
                    <td valign="top" colspan="3" rowspan="3">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtGhiChu" runat="server" TextMode="MultiLine" AutoPostBack="true" OnTextChanged="txtGhiChu_TextChanged" Rows="7" Width="96%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>File KH gửi:</td>
                    <td colspan="3" style="padding-top: 5px; padding-bottom: 5px;">
                        <asp:Literal ID="ltFileKH" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td valign="top">Nội dung phản ánh:</td>
                    <td colspan="3">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtNoiDung" AutoPostBack="true" OnTextChanged="txtNoiDung_TextChanged" TextMode="MultiLine" Rows="5" runat="server" Width="96%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">Nội dung hỗ trợ:</td>
                    <td colspan="7">
                        <div class="inputstyle_longlx">
                            <div class="bg">
                                <asp:TextBox ID="txtNoiDungCanHoTro" AutoPostBack="true" OnTextChanged="txtNoiDungCanHoTro_TextChanged" TextMode="MultiLine" Rows="3" runat="server" Width="99%"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <%--<td valign="top" style="border-left: 1px solid #ccc;">Nguyên nhân lỗi</td>
                        <td valign="top" colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlNguyenNhanLoi" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNguyenNhanLoi_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                </td>--%>
                </tr>
                <tr>
                    <td colspan="8" style="padding-left: 0px;">
                        <asp:HiddenField ID="hdKhieuNaiId" runat="server" Value="0" />
                        <uc:chitietkhieunai id="ucChiTietKhieuNai" runat="server" />
                    </td>
                </tr>
            </table>

            <div id="divPoupChuyenPhanHoi" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H6" style="float: left; color: #fff; font-weight: bold;">Chuyển phản hồi khiếu nại
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupChuyenPhanHoi();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div1" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div class="popup_Body">
                            <div class="infoBox">
                                Khiếu nại sẽ được chuyển đến người chuyển khiếu nại cho bạn&nbsp;&nbsp;
                            </div>
                            <br />
                            Chuyển phản hồi tới
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList Width="100%" runat="server" ID="ddlPhongBanPhanHoi">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />

                            <div class="foot_nav_btn">
                                <div class="popup_Buttons">
                                    <a href="#1"><i class="apply">&nbsp;</i><span>
                                        <asp:Button ID="Button1" CssClass="button_eole" OnClientClick="return ValidChuyenPhanHoi();" Text="Đồng ý" runat="server" OnClick="btnOkayChuyenPhanHoi_Click" />
                                    </span></a>
                                    <a href="javascript:ClosePoupChuyenPhanHoi();"><i class="notapply">&nbsp;</i><span><input id="Button2" onclick="javascript: ClosePoupChuyenPhanHoi();" class="button_eole" value="Hủy bỏ" type="button" /></span></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divPoupChuyenNgangHang" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Chuyển ngang hàng khiếu nại
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupChuyenNgangHang();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div4" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div class="popup_Body">
                            <div class="infoBox">
                                Khiếu nại sẽ được chuyển ngang hàng ra phòng ban của bạn&nbsp;&nbsp;
                            </div>
                            <br />
                            Chuyển ngang hàng tới
                            <div class="inputstyle">
                                <div class="bg">
                                    <asp:DropDownList Width="100%" runat="server" ID="ddlUserNgangHang">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />

                            <div class="foot_nav_btn">
                                <div class="popup_Buttons">
                                    <a href="#1"><i class="apply">&nbsp;</i><span><asp:Button ID="btnOkeyNgangHang" OnClientClick="return ValidChuyenNgangHang();" CssClass="button_eole" Text="Đồng ý" runat="server" OnClick="btnOkeyNgangHang_Click" /></span></a>
                                    <a href="javascript:ClosePoupChuyenNgangHang();"><i class="notapply">&nbsp;</i><span><input id="btnCancelNgangHang" onclick="javascript: ClosePoupChuyenNgangHang();" class="button_eole" value="Hủy bỏ" type="button" /></span></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divPoupChuyenPhanHoiTrungTam" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H1" style="float: left; color: #fff; font-weight: bold;">Chuyển phản hồi khiếu nại trung tâm
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupChuyenPhanHoiTrungTam();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div5" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div class="popup_Body">
                            <div class="infoBox">
                                Khiếu nại sẽ được chuyển đến trung tâm chuyển khiếu nại cho trung tâm bạn&nbsp;&nbsp;
                            </div>
                            <br />
                            Chuyển phản hồi tới
                            <div class="selectstyle">
                                <div class="bg">
                                    <asp:DropDownList Width="100%" runat="server" ID="ddlTrungTamPhanHoi">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />

                            <div class="foot_nav_btn">
                                <div class="popup_Buttons">
                                    <a href="#1"><i class="apply">&nbsp;</i><span>
                                        <asp:Button ID="btnOkeyChuyenPhanHoiTrungTam" CssClass="button_eole" OnClientClick="return ValidChuyenPhanHoiTrungTam();" Text="Đồng ý" OnClick="btnOkeyChuyenPhanHoiTrungTam_Click" runat="server" />
                                    </span></a>
                                    <a href="javascript:ClosePoupChuyenPhanHoiTrungTam();"><i class="notapply">&nbsp;</i><span><input id="btnCancelTrungTam" onclick="javascript: ClosePoupChuyenPhanHoiTrungTam();" class="button_eole" value="Hủy bỏ" type="button" /></span></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divPoupPhongBan" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H5" style="float: left; color: #fff; font-weight: bold;">Chọn phòng ban cần chuyển đến
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoup();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div2" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div>
                            Chọn phòng ban
                            <div class="selectstyle">
                                <div class="bg">
                                    <select id="ddlPhongBanChuyenXuLy" class="chosen" onchange="javascript:fnPhongBanChuyenXuLyChange();">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div id="dvUserInPhongBan" runat="server">
                            Chọn user
                            <div class="selectstyle">
                                <div class="bg">
                                    <select id="ddlUserInPhongBan">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 5px;">
                        </div>
                        <ul>
                            <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy</span></a></li>
                            <li style="float: right;"><a href="javascript:ChuyenXuLy();"><span class="apply">Đồng ý </span></a></li>
                        </ul>
                    </div>
                </div>
                <div style="clear: both; height: 1px;">
                </div>
            </div>

            <div id="divPoupChuyenXuLyAuTo" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H3" style="float: left; color: #fff; font-weight: bold;">Chuyển xử lý khiếu nại
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div6" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div class="selectstyle">
                            <div class="bg">
                                <select id="ddlTuDongDinhTuyenAndPhongBanCungDoiTac" onchange="javascript:fnLoadUserByPhongBanId('ddlTuDongDinhTuyenAndPhongBanCungDoiTac','ddlUserInPhongBan_divPoupChuyenXuLyAuTo')">
                                </select>
                            </div>
                        </div>
                        <div id="dvUserInPhongBan_divPoupChuyenXuLyAuTo" runat="server">
                            Chọn user
                            <div class="selectstyle">
                                <div class="bg">
                                    <select id="ddlUserInPhongBan_divPoupChuyenXuLyAuTo">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="popup_Body">
                            <%--<div class="infoBox">
                                Khiếu nại sẽ được tự động định tuyến đến phòng ban xử lý
                            </div>--%>
                            <br />


                            <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 10px;">
                            </div>
                            <ul>
                                <li style="float: right;"><a href="javascript:ClosePoupChuyenXuLyAuTo();"><span class="notapply">Hủy
                                </span></a></li>
                                <li style="float: right;"><a href="javascript:ChuyenXuLyAuto();"><span class="apply">Đồng
                    ý </span></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 1px;">
                </div>
            </div>

            <div id="divDongKN" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H4" style="float: left; color: #fff; font-weight: bold;">Đóng khiếu nại
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupDongKN();">
                        <img src="/Images/x.png" />
                    </a></span>
                </div>
                <div id="div7" style="">
                    <div class="nav_btn" style='background: none;'>
                        <div class="popup_Body">
                            <div class="infoBox">
                                Bạn có muốn đóng khiếu nại ?
                                <br />
                            </div>
                            <br />
                            <span style="font-size: 13px; font-weight: bold;">Bạn vui lòng chọn nguyên nhân lỗi, chi tiết lỗi, độ hài lòng của khách hàng và nhập nội dung xử lý trước khi đóng khiếu nại:  </span>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td>Nguyên nhân lỗi
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlNguyenNhanLoi" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNguyenNhanLoi_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Chi tiết lỗi
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlChiTietLoi" runat="server" Width="350px"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Độ hài lòng
                                            </td>
                                            <td>
                                                <select id="slDoHaiLong" name="slDoHaiLong">
                                                    <option value="-1" selected="selected">Chọn độ hài lòng của khách hàng</option>
                                                    <option value="0">Rất hài lòng</option>
                                                    <option value="1">Hài lòng</option>
                                                    <option value="2">Không hài lòng</option>
                                                    <option value="3">KH phản ứng gay gắt</option>
                                                    <option value="4">Không liên lạc được KH</option>
                                                    <option value="5">Ý kiến khác</option>
                                                </select>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--<span style="font-size: 13px; font-weight: bold;">Bạn vui lòng chọn độ hài lòng của khách hàng trước khi đóng khiếu nại:  <strong style="color: Red">(*)</strong></span>
                            <br />
                            <select id="slDoHaiLong" name="slDoHaiLong">
                                <option value="-1" selected="selected">Chọn độ hài lòng của khách hàng</option>
                                <option value="0">Rất hài lòng</option>
                                <option value="1">Hài lòng</option>
                                <option value="2">Không hài lòng</option>
                                <option value="3">KH phản ứng gay gắt</option>
                                <option value="4">Không liên lạc được KH</option>
                                <option value="5">Ý kiến khác</option>
                            </select>--%>
                            <br />
                            <br />
                            <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 10px;">
                            </div>
                            <ul>
                                <li style="float: right;"><a href="javascript:ClosePoupDongKN();"><span class="notapply">Hủy</span></a></li>
                                <li style="float: right;"><a href="javascript:fnDongKhieuNai();"><span class="apply">Đồng ý </span></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 1px;">
                </div>
            </div>

            <div id="divKhoaPhieuVNPT" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 1000; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
                <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
                    <h3 id="H4" style="float: left; color: #fff; font-weight: bold;">Khóa phiếu về VNPT
                    </h3>
                    <span style="float: right;"><a href="javascript:ClosePoupKhoaPhieu();">
                        <img src="/Images/x.png" alt="Đóng" />
                    </a></span>
                </div>
                <div id="div7">
                    <div class="nav_btn" style='background: none;'>
                        <div class="popup_Body">
                            <div class="infoBox">
                                Bạn có muốn khóa phiếu về VNPT ?
                                <br />
                                <br />
                                <br />
                            </div>
                            <br />
                            <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 10px;">
                            </div>
                            <ul>
                                <li style="float: right;"><a href="javascript:ClosePoupKhoaPhieu();"><span class="notapply">Hủy</span></a></li>
                                <li style="float: right;"><a href="javascript:fnKhoaPhieuVNPT();"><span class="apply">Đồng ý </span></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div style="clear: both; height: 1px;">
                </div>
            </div>

            <div id="divOpacity" class="divOpacity" style="opacity: .4; -moz-opacity: 0.4; filter: alpha(opacity=70); background: #999999; width: 100%; height: 100%; position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
