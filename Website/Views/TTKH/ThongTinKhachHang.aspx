<%@ Page Title="" Language="C#" MasterPageFile="~/adminNotAJAX.master" AutoEventWireup="true" CodeBehind="ThongTinKhachHang.aspx.cs" Inherits="Website.View.ThongTinKhachHang" %>

<%@ Register TagPrefix="uc" TagName="LichSuKhieuNai" Src="~/Views/TTKH/UC/ucLichSuKhieuNai.ascx" %>
<%@ Register TagPrefix="uc" TagName="LichSuCuocGoi" Src="~/Views/TTKH/UC/ucLichSuCuocGoi.ascx" %>
<%@ Register TagPrefix="uc" TagName="LichSuNapThe" Src="~/Views/TTKH/UC/ucLichSuNapThe.ascx" %>
<%@ Register TagPrefix="asp" Src="~/admin/Control/GridViewPager.ascx" TagName="GridViewPager" %>
<%@ Register TagPrefix="ChiTietKN" Src="~/Views/QLKhieuNai/UserControls/ChiTietKN.ascx" TagName="ChiTietKN" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <link href="../../CSS/flexigrid.pack.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JS/flexigrid.js"></script>
    <link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            <script   type="text/javascript">
                $(document).ready(function () {
                    LoadJS();
                    LoadData();
                });

                window.onresize = function (event) {
                    fnSetSizeDiv();
                }

                function LoadData() {
                    var stb = Utility.GetUrlParam("SoThueBao");
                    if (stb != undefined && stb != "") {
                        $('#<%=txtThueBao.ClientID%>').val(stb);
                        TraCuuThongTin();
                    }
                    LoadLichSuKhieuNai();
                }

                function LoadJS() {
                    fnSetSizeDiv();

                    $('.ajax__tab_outer').click(function () {
                        var tabIndex = $find('<%=TabContainer1.ClientID%>').get_activeTabIndex();
                        ChangeLichSu(tabIndex);
                    });
                }

                var TAB_INDEX = -1;
                function ChangeLichSu(tabIndex) {
                    if (TAB_INDEX != tabIndex) {
                        TAB_INDEX = tabIndex;
                        var tb = $('#<%=txtThueBao.ClientID%>').val();

                        var regex = /^((9[14]([0-9]){7})|(12[34579]([0-9]){7}))$/g;
                        if (regex.test(tb)) {
                            switch (TAB_INDEX) {
                                case 0: //Lịch sử khiếu nại
                                    LichSuKhieuNai(0);
                                    break;
                                case 1: //Lịch sử cuộc gọi
                                    //console.log(2);
                                    break;
                                case 2: //Lịch sử nạp thẻ
                                    //console.log(3);
                                    LichSuNapThe(tb);
                                    break;
                                case 3: //Tra cuu the cao
                                    //console.log(3);
                                    LichSuNapThe(tb);
                                    break;
                                case 4: //Lịch sử thue bao
                                    //console.log(3);
                                    LichSuThueBao(tb);
                                    break;
                                case 5: //Ho tro cat khan cap
                                    //console.log(3);
                                    //LichSuThueBao(tb);
                                    break;
                                case 6: //Thong tin 2 Friend
                                    //console.log(3);
                                    //LichSuThueBao(tb);
                                    break;
                                case 7: //Lịch sử 3G
                                    //console.log(3);
                                    LichSu3G(tb);
                                    break;
                                case 8: //Lịch sử VAS
                                    //console.log(3);
                                    //LichSu3G(tb);
                                    break;
                                case 9: //Tra cuu KM
                                    //console.log(3);
                                    //LichSu3G(tb);
                                    break;
                                case 10: //Tra cuu so phut goi
                                    //console.log(3);
                                    //LichSu3G(tb);
                                    break;
                                case 11: //SMS888
                                    //console.log(3);
                                    TraCuuSMS888(tb);
                                    break;
                                case 12: //Lịch sử bù tiền
                                    //console.log(4);
                                    LichSuBuTen(tb);
                                    break;
                            }
                        }
                    }
                }

                function fnSetSizeDiv() {
                    var d = $('body').innerWidth();
                    var h = screen.height;
                    $("#divScroll").css("width", d - 65);
                }



                function ChangeTypeSearch(e) {
                    if (e == "NgayTiepNhanSort") {
                        $("#txtSearchLSKhieuNai").datepick({ dateFormat: 'dd/mm/yyyy' });
                    }
                    else {
                        $.datepick.destroy("#txtSearchLSKhieuNai");
                    }
                }

                function fnValidForm() {
                }

                function fnExportExcel() {
                    var tb = $('#<%=txtThueBao.ClientID%>').val();
                    window.open("/Views/TTKH/Handler/ExcelLichSuKhieuNai.aspx?SoThueBao=" + tb);
                }

            </script>
            
             <!--Lịch sử kiếu nại-->
            <script type="text/javascript" language="javascript">
                function LichSuKhieuNai(flagAll) {
                    var tb = $('#<%=txtThueBao.ClientID%>').val();
                    $('.flex_LSKhieuNai').flexOptions({ params: [{ name: 'SoThueBao', value: tb }, { name: "ViewAll", value: flagAll }] }).flexReload();
                }

                function LoadLichSuKhieuNai() {
                    var param = [
                    { name: 'SoThueBao', value: $('#<%=txtThueBao.ClientID%>').val() }, { name: 'ViewAll', value: '0' }];

                    $(".flex_LSKhieuNai").flexigrid({
                        url: 'Handler/ThongTinKhachHang.ashx?type=LSKhieuNai',
                        params: param,
                        dataType: 'json',
                        colModel: [
                            { display: 'Mã PA/KN', name: 'MaKhieuNai', width: 110, sortable: true, align: 'center' },
                            { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
                            { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center' },
                            { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },
                            { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
                            { display: 'Giảm trừ', name: 'IsKNGiamTru', width: 80, sortable: false, align: 'center' },
                            { display: 'Ngày TN', name: 'NgayTiepNhanSort', width: 130, sortable: true, align: 'center' },
                            { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
                            { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },
                            { display: 'Ngày đóng KN', name: 'NgayDongKNSort', width: 130, sortable: true, align: 'center' },
                            { display: 'Nội dung GQ', name: 'NoiDungXuLyDongKN', width: 150, sortable: false, align: 'left' },
                            { display: 'Độ ưu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },
                            { display: 'Họ tên liên hệ', name: 'HoTenLienHe', width: 100, sortable: false, align: 'left', hide: true },
                            { display: 'Địa chỉ', name: 'DiaChi_CCBS', width: 150, sortable: false, align: 'left', hide: true },
                            { display: 'Địa chỉ liên hệ', name: 'DiaChiLienHe', width: 150, sortable: false, align: 'left', hide: true },
                            { display: 'SĐT liên hệ', name: 'SDTLienHe', width: 100, sortable: false, align: 'left', hide: true },
                            { display: 'Địa điểm xảy ra', name: 'DiaDiemXayRa', width: 120, sortable: false, align: 'left', hide: true },
                            { display: 'Thời gian xảy ra', name: 'ThoiGianXayRa', width: 120, sortable: false, align: 'left', hide: true },
                            { display: 'Ghi chú', name: 'GhiChu', width: 150, sortable: false, align: 'left', hide: true },
                            { display: 'Mã tỉnh', name: 'MaTinh', width: 100, sortable: false, align: 'left', hide: true },
                            { display: 'Mã quận', name: 'MaQuan', width: 100, sortable: false, align: 'left', hide: true },
                            { display: 'HT tiếp nhận', name: 'HTTiepNhan', width: 100, sortable: true, align: 'center', hide: true },
                            { display: 'Ngày quá hạn', name: 'NgayQuaHanSort', width: 130, sortable: true, align: 'center', hide: true },
                            { display: 'KN hàng loạt', name: 'KNHangLoat', width: 70, sortable: true, align: 'center', hide: true },
                            { display: 'KN lưu sổ', name: 'IsLuuKhieuNai', width: 70, sortable: true, align: 'center', hide: true },
                            { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },
                        ],

                        searchitems: [
                            { display: 'Mã PA/KN', name: 'MaKhieuNai', isdefault: true },
                            { display: 'Trạng thái', name: 'TrangThai' },
                            { display: 'Người tiếp nhận', name: 'NguoiTiepNhan' },
                            { display: 'Ngày tiếp nhận', name: 'NgayTiepNhanSort' },
                            { display: 'Phòng ban xử lý', name: 'PhongBanXuLy' },
                            { display: 'Người xử lý', name: 'NguoiXuLy' },
                            { display: 'Nội dung phản ánh', name: 'NoiDungPA' },
                            { display: 'Độ ưu tiên', name: 'DoUuTien' },
                            { display: 'Loại khiếu nại', name: 'LoaiKhieuNai' },
                            { display: 'Lĩnh vực chung', name: 'LinhVucChung' },
                            { display: 'Lĩnh vực con', name: 'LinhVucCon' }
                        ],
                        searchitemid: 'slLSKhieuNai',
                        searchitemidtextbox: 'txtSearchLSKhieuNai',
                        sortname: "LDate",
                        sortorder: "desc",
                        usepager: true,
                        callFunctionAfterReload: function () {
                            TestNhay();
                        },
                        useRp: true,
                        rp: 10,
                        showTableToggleBtn: true,
                        //width: 3200,
                        height: 180
                    }).flexReload();

                    $('#slLSKhieuNai').change(function () {
                        ChangeTypeSearch($(this).val());
                    });
                }
            </script>

            <!--Lịch sử nạp thẻ-->
            <script type="text/javascript">
                function LichSuNapThe(tb) {
                    $.ajax({
                        beforeSend: function () {
                        },
                        type: "POST",
                        dataType: "text",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "LichSuNapThe", tb: tb },
                        success: function (obj) {
                            //if (obj.ErrorId == 0) {
                            $('#tblLichSuNapThe').html(obj);
                            //}
                            //else {
                            //    MessageAlert.AlertNormal(obj.Message, 'error');
                            //}
                        },
                        error: function (e) {
                            MessageAlert.AlertJSON("-1000");
                        }
                    });
                }
            </script>

            <!--Lịch sử thuee bao-->
            <script type="text/javascript">
                function LichSuThueBao(tb) {
                    $.ajax({
                        beforeSend: function () {
                        },
                        type: "POST",
                        dataType: "text",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "LichSuThueBao", tb: tb },
                        success: function (obj) {
                            //if (obj.ErrorId == 0) {
                            $('#tblLichSuThueBao').html(obj);
                            //}
                            //else {
                            //    MessageAlert.AlertNormal(obj.Message, 'error');
                            //}
                        },
                        error: function (e) {
                            MessageAlert.AlertJSON("-1000");
                        }
                    });
                }
            </script>

            <!--Lịch sử 3G-->
            <script type="text/javascript">
                function LichSu3G(tb) {
                    $.ajax({
                        beforeSend: function () {
                        },
                        type: "POST",
                        dataType: "text",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "LichSu3G", tb: tb },
                        success: function (obj) {
                            //if (obj.ErrorId == 0) {
                            $('#tblLichSu3G').html(obj);
                            //}
                            //else {
                            //    MessageAlert.AlertNormal(obj.Message, 'error');
                            //}
                        },
                        error: function (e) {
                            MessageAlert.AlertJSON("-1000");
                        }
                    });
                }
            </script>

            <!--Tra Cuu SMS888-->
            <script type="text/javascript">
                function TraCuuSMS888(tb) {
                    $.ajax({
                        beforeSend: function () {
                        },
                        type: "POST",
                        dataType: "text",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "TraCuuSMS888", tb: tb },
                        success: function (obj) {
                            //if (obj.ErrorId == 0) {
                            $('#tblTraCuuSMS888').html(obj);
                            //}
                            //else {
                            //    MessageAlert.AlertNormal(obj.Message, 'error');
                            //}
                        },
                        error: function (e) {
                            MessageAlert.AlertJSON("-1000");
                        }
                    });
                }
            </script>

            <!--Tra cứu thông tin-->
            <script type="text/javascript"  >
                var flagTraCuu = false;
                function handleEnter(obj, event) {
                    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                    if (keyCode == 13) {
                        TraCuuThongTin();
                        return false;
                    }
                    else {
                        return true;
                    }
                }

                function TraCuuThongTin() {
                    if (flagTraCuu) return false;

                    flagTraCuu = true;
                    var tb = $('#<%=txtThueBao.ClientID%>').val().trim();

                    LichSuKhieuNai(0);
                    $find('<%=TabContainer1.ClientID %>').set_activeTabIndex(0);

                    var regex = /^((9[14]([0-9]){7})|(12[34579]([0-9]){7})|(88[0123456789]([0-9]){6}))$/g;
                    if (regex.test(tb)) {
                        $.ajax({
                            beforeSend: function () {
                                $('#divLoading').slideDown(200);
                                $('#loadingText').html('Đang lấy dữ liệu thông tin cơ bản của trung tâm tính cước.');
                            },
                            type: "POST",
                            dataType: "json",
                            url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                            data: { type: "GetInfo", tb: tb },
                            success: function (obj) {
                                flagTraCuu = false;
                                //console.log(obj);
                                if (obj.ErrorId == -1) {
                                    //console.log(1);
                                    MessageAlert.AlertNormal(obj.Message, obj.Title, $('#<%=txtThueBao.ClientID%>').attr('id'));
                                    BindDataEmptyToEle();
                                }
                                else {
                                    BindDataToEle(obj);
                                    //Bind dich vu da dung
                                    BindServiceUse(tb);
                                    //$find('<%=TabContainer1.ClientID%>').set_activeTabIndex(0);
                                }
                            },
                            error: function () {
                                flagTraCuu = false;
                                MessageAlert.AlertJSON("-1000");
                            },
                            complete: function () {
                                // Handle the complete event
                                $('#divLoading').slideUp(200);
                            }
                        });
                    }
                    else {
                        flagTraCuu = false;
                        MessageAlert.AlertNormal('Số thuê bao chưa hợp lệ', 'error', $('#<%=txtThueBao.ClientID%>').attr('id'));
                    }
                    return false;
                }

                function BindServiceUse(tb) {
                    $.ajax({
                        beforeSend: function () {
                            $('#divLoading').slideDown(200);
                            $('#loadingText').html('Đang lấy dữ liệu dịch vụ cơ bản của trung tâm tính cước.');
                        },
                        type: "POST",
                        dataType: "json",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "GetServiceDaDung", tb: tb },
                        success: function (obj) {
                            if (obj.ErrorId == 0) {
                                $('#tblDichVuDaDung').html(obj.Content);
                            }
                            else {
                                console.log(2);
                                MessageAlert.AlertNormal(obj.Message, 'error');
                            }
                        },
                        error: function (e) {
                            //console.log(1);
                            MessageAlert.AlertJSON("-1000");
                        },
                        complete: function () {
                            // Handle the complete event
                            $('#divLoading').slideUp(200);
                        }
                    });
                }



                function BindDataEmptyToEle() {

                    $('#<%=txtMSIN.ClientID%>').val("");
                    $('#<%=chkGoiDi.ClientID%>').removeAttr('checked');
                    $('#<%=chkGoiDen.ClientID%>').removeAttr('checked');

                    $('#<%=txtLoaiTB.ClientID%>').val("");
                    $('#<%=txtTinh.ClientID%>').val("");
                    $('#<%=txtNgayKH.ClientID%>').val("");
                    $('#<%=txtMaKH.ClientID%>').val("");
                    $('#<%=txtPIN.ClientID%>').val("");
                    $('#<%=txtPUK.ClientID%>').val("");
                    $('#<%=txtPIN2.ClientID%>').val("");
                    $('#<%=txtPUK2.ClientID%>').val("");
                    $('#<%=txtDoiTuong.ClientID%>').val("");

                    $('#<%=txtMaCQ.ClientID%>').val("");
                    $('#<%=txtNgaySinh.ClientID%>').val("");
                    $('#<%=txtSoGT.ClientID%>').val("");
                    $('#<%=txtNoiCap.ClientID%>').val("");
                    $('#<%=txtTB.ClientID%>').val("");

                    $('#<%=txtDiaChiThanhToan.ClientID%>').val("");
                    $('#<%=txtDiaChiChungTu.ClientID%>').val("");
                    $('#<%=txtDiaChiThuongTru.ClientID%>').val("");

                    $('#<%=txtTKC.ClientID%>').val("");
                    $('#<%=txtHSD.ClientID%>').val("");
                    $('#<%=txtKM.ClientID%>').val("");
                    $('#<%=txtKM1.ClientID%>').val("");
                    $('#<%=txtKM2.ClientID%>').val("");
                    $('#<%=txtData.ClientID%>').val("");
                    $('#<%=TTTKH_MaTinh.ClientID%>').val("");

                }

                function BindDataToEle(obj) {
                    //Neu la TB tra sau
                    if (obj != null) {

                        $('#<%=txtMSIN.ClientID%>').val(obj.SO_MSIN);
                        if (obj.GOI_DI == "A") {
                            $('#<%=chkGoiDi.ClientID%>').attr('checked', 'checked');
                        }
                        else {
                            $('#<%=chkGoiDi.ClientID%>').removeAttr('checked');
                        }
                        if (obj.GOI_DEN == "A") {
                            $('#<%=chkGoiDen.ClientID%>').attr('checked', 'checked');
                        }
                        else {
                            $('#<%=chkGoiDen.ClientID%>').removeAttr('checked');
                        }

                        $('#<%=txtLoaiTB.ClientID%>').val(obj.TEN_LOAI);
                        $('#<%=txtTinh.ClientID%>').val(obj.MA_TINH);
                        $('#<%=txtNgayKH.ClientID%>').val(obj.NGAY_KH);
                        $('#<%=txtMaKH.ClientID%>').val(obj.Ma_KH);
                        $('#<%=txtPIN.ClientID%>').val(obj.SO_PIN);
                        $('#<%=txtPUK.ClientID%>').val(obj.SO_PUK);
                        $('#<%=txtPIN2.ClientID%>').val(obj.PIN2);
                        $('#<%=txtPUK2.ClientID%>').val(obj.PUK2);
                        $('#<%=txtDoiTuong.ClientID%>').val(obj.LOAIKH_ID);

                        if (obj.TEN_LOAI != undefined && (obj.TEN_LOAI == 'EZPOST' || obj.TEN_LOAI == 'Post'
                            || (obj.TEN_LOAI.indexOf("iTouch") != -1) || (obj.TEN_LOAI == "iPlus2"))) {

                            $('#<%=txtMaCQ.ClientID%>').val(obj.MA_CQ);
                            $('#<%=txtNgaySinh.ClientID%>').val(obj.NGAYSINH);
                            $('#<%=txtSoGT.ClientID%>').val(obj.SO_GT);
                            $('#<%=txtNoiCap.ClientID%>').val(obj.NGAYCAP_GT);
                            $('#<%=txtTB.ClientID%>').val(obj.TEN_TB);

                            $('#<%=txtDiaChiThanhToan.ClientID%>').val(obj.DIACHI_TT);
                            $('#<%=txtDiaChiChungTu.ClientID%>').val(obj.DIACHI_CT);
                            $('#<%=txtDiaChiThuongTru.ClientID%>').val(obj.SOTT_NHA);

                            $('#<%=txtTKC.ClientID%>').val("");
                            $('#<%=txtHSD.ClientID%>').val("");
                            $('#<%=txtKM.ClientID%>').val("");
                            $('#<%=txtKM1.ClientID%>').val("");
                            $('#<%=txtKM2.ClientID%>').val("");
                            $('#<%=txtData.ClientID%>').val("");
                            $('#<%=TTTKH_MaTinh.ClientID%>').val(obj.MA_TINH);
                        }
                        else {

                            $('#<%=txtNoiCap.ClientID%>').val(obj.PLACEDATE);
                            $('#<%=txtSoGT.ClientID%>').val(obj.IDNUMBER);
                            $('#<%=txtNgaySinh.ClientID%>').val(obj.BIRTHDAY);
                            $('#<%=txtTB.ClientID%>').val(obj.FULLNAME);
                            $('#<%=txtMaCQ.ClientID%>').val(obj.REGISTERMETHODID);

                            $('#<%=txtDiaChiThuongTru.ClientID%>').val(obj.ADDRESS);
                            $('#<%=txtDiaChiThanhToan.ClientID%>').val("");
                            $('#<%=txtDiaChiChungTu.ClientID%>').val("");

                            $('#<%=txtTKC.ClientID%>').val(obj.Balance);
                            $('#<%=txtHSD.ClientID%>').val(obj.HSD);
                            $('#<%=txtKM.ClientID%>').val(obj.BalanceKM);
                            $('#<%=txtKM1.ClientID%>').val(obj.BalanceKM1);
                            $('#<%=txtKM2.ClientID%>').val(obj.BalanceKM2);
                            $('#<%=txtData.ClientID%>').val(obj.BalanceData);
                        }
                    }
                }
            </script>
            
            <!--Lịch sử bù tiền-->
            <script type="text/javascript" language="javascript">
                function LichSuBuTen(tb) {
                    $.ajax({
                        beforeSend: function () {
                        },
                        type: "POST",
                        dataType: "text",
                        url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                        data: { type: "LichSuBuTien", tb: tb },
                        success: function (obj) {
                            //if (obj.ErrorId == 0) {
                            $('#tblLichSuBuTien').html(obj);
                            //}
                            //else {
                            //    MessageAlert.AlertNormal(obj.Message, 'error');
                            //}
                        },
                        error: function (e) {
                            MessageAlert.AlertJSON("-1000");
                        }
                    });
                }
            </script>

            <asp:HiddenField runat="server" ID="TTTKH_MaTinh"  />
            <div class="p8">
                <div id="divLoading" style="display: none;">
                    <table border="0">
                        <tr>
                            <td style="width: 130px;">
                                <img src="../../images/loading.gif" title="0" /></td>
                            <td style="text-align: left"><span id="loadingText">Đang lấy dữ liệu từ trung tâm tính cước</span></td>
                        </tr>
                    </table>
                </div>
                <table class="nobor">
                    <tr>
                        <td width="66%" valign="top" style="padding-right: 5px;">
                            <div style="line-height: 40px; background: #4D709A;">
                                <span class="topbox" style="color: #fff; padding-left: 10px">THÔNG TIN THUÊ BAO</span>

                            </div>
                            <table class="nobor">
                                <tr>
                                    <td width="110px">Số thuê bao
                                    </td>
                                    <td width="20px">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDauSo" ReadOnly="true" Text="84" Width="15px" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td width="120px">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtThueBao" runat="server" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btTraCuu" runat="server" Text="..." OnClientClick="return TraCuuThongTin();" CssClass="btn_style_button" />
                                    </td>
                                    <td width="100px" align="left" style="padding-left: 5px;">MSIN
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtMSIN" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Loại thuê bao
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtLoaiTB" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkGoiDi" Text=" Gọi đi" Enabled="false" runat="server" />&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkGoiDen" Text=" Gọi đến" Enabled="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tỉnh
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtTinh" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;">Ngày KH
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtNgayKH" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Mã KH
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtMaKH" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;">Mã CQ
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtMaCQ" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tên thuê bao
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtTB" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;" class='nowarp'>Ngày sinh
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtNgaySinh" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Số GT
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtSoGT" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;">Ngày cấp
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtNoiCap" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Số PIN/PUK
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtPIN" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtPUK" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td align="left" style="padding-left: 5px;">Số PIN2/PUK2
                                    </td>
                                    <td>
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtPIN2" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtPUK2" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='nowarp'>Đối tượng
                                    </td>
                                    <td colspan="6">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDoiTuong" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='nowarp'>Địa chỉ chứng từ
                                    </td>
                                    <td colspan="6">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDiaChiChungTu" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Địa chỉ thanh toán
                                    </td>
                                    <td colspan="6">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDiaChiThanhToan" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>ĐC thường trú
                                    </td>
                                    <td colspan="6">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDiaChiThuongTru" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tài khoản chính
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtTKC" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;" class='nowarp'>Hạn sử dụng
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtHSD" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tài khoản KM
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtKM" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;" class='nowarp'>Tài khoản KM1
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtKM1" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tài khoản KM2
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtKM2" CssClass="mw" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;" class='nowarp'>Data
                                    </td>
                                    <td colspan="2">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtData" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="2"></td>
                                    <td></td>
                                    <td align="left" style="padding-left: 5px;" class='nowarp'></td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <span id="spThemMoiKN" runat="server">
                                            <input type="button" id="btThemMoiKN" onclick="javascript: fnAddNewKN();" value="Thêm khiếu nại" class="btn_style_button" />
                                        </span>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td width="34%" valign="top" style="padding-left: 5px; border-left: 1px solid #ccc;">
                            <div style='height: 600px; overflow: scroll'>
                                <table class="tbl_style" id="tblDichVuDaDung" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                    <tr>
                                        <th align="center" scope="col">Mã dịch vụ</th>
                                        <th align="center" scope="col">Tên dịch vụ</th>
                                    </tr>
                                </table>
                                <%--<asp:GridView ID="grvViewDichVuDaDung" ShowHeaderWhenEmpty="true" runat="server"
                                    AutoGenerateColumns="False" CssClass="tbl_style">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                        <asp:BoundField DataField="MA_DV" HeaderText="Mã dịch vụ" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TEN_DV" HeaderText="Tên dịch vụ" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>--%>
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="nobor">
                    <tr>
                        <td colspan="2" style="">
                            <cc2:tabcontainer id="TabContainer1" runat="server" cssclass="Tab"
                                height="315px">
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LSKhieuNai">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        khiếu nại
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <div id="divNote" style="width: 100%; float: left; margin-top: 5px;">
                                                            <p style="border: 1pt solid #CCC; background: red; width: 22px; height: 13px; float: left;">
                                                            </p>
                                                            <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span>
                                                            <p style="border: 1pt solid #CCC; background: yellow; width: 22px; height: 13px; float: left;">
                                                            </p>
                                                            <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span>
                                                            <p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;">
                                                            </p>
                                                            <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span>

                                                            <p style="border: 1pt solid #CCC; background: #FF8000; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN BP khác chuyển về</span>

                                                            <p style="border: 1pt solid #CCC; background: #999; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN quá hạn</span>

                                                            <p style="border: 1pt solid #CCC; background: green; width: 22px; height: 13px; float: left;">
                                                            </p>
                                                            <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đã đóng</span>
                                                            <br />
                                                        </div>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <div style="text-align: right; padding-right: 5px; background: none !important;">
                                                            <input id="btViewVNPT" type="button" value="Xem KN từ HNI" onclick="LichSuKhieuNai(2);" class="btn_style_button" />
                                                            <input id="btViewGanDay" type="button" value="Xem những KN gần nhât" onclick="LichSuKhieuNai(0);" class="btn_style_button" />
                                                            <input id="btViewAll" type="button" value="Xem toàn bộ" onclick="LichSuKhieuNai(1);" class="btn_style_button" />
                                                            <input id="Button1" type="button" value="Xuất Excel" onclick="fnExportExcel();" class="btn_style_button" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="divScroll" style="height: 270px; width: 100%;">
                                                <table class="flex_LSKhieuNai" style="display: none"></table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>

                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LSCuocGoi">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        cuộc gọi
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grvViewLichSuCuocGoi" runat="server" AutoGenerateColumns="true"
                                                        CssClass="tbl_style" OnRowDataBound="grvViewLichSuCuocGoi_RowDataBound">
                                                        <RowStyle CssClass="rowB" />
                                                        <AlternatingRowStyle CssClass="rowA" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="STT" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LSNapThe">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        nạp thẻ
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblLichSuNapThe" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                    <tr>
                                                        <th align="center" scope="col">STT</th>
                                                        <th align="center" scope="col">Mệnh giá</th>
                                                        <th align="center" scope="col">Ngày nạp</th>
                                                        <th align="center" scope="col">Phương thức</th>
                                                        <th align="center" scope="col">TKC Trước nạp</th>
                                                        <th align="center" scope="col">TKC Sau nạp</th>
                                                        <th align="center" scope="col">TKKM Trước nạp</th>
                                                        <th align="center" scope="col">TKKM Sau nạp</th>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="KTSeri">
                                    <HeaderTemplate>
                                        Tra cứu<br />
                                        thẻ cào
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblTraCuuTheCao" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LSThueBao">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        thuê bao
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblLichSuThueBao" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                    <tr>
                                                        <th align="center" scope="col">STT</th>
                                                        <th scope="col">NGAY_THANG</th>
                                                        <th scope="col">MA_DV</th>
                                                        <th scope="col">THAO_TAC</th>
                                                        <th scope="col">GHI_CHU</th>
                                                        <th scope="col">NGUOI_DUNG</th>
                                                        <th scope="col">SO_MSIN_CU</th>
                                                        <th scope="col">SO_MSIN_MOI</th>
                                                        <th scope="col">MA_TINH_CU</th>
                                                        <th scope="col">MA_TINH_MOI</th>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="HoTroCatKC">
                                    <HeaderTemplate>
                                        Hỗ trợ cắt<br />
                                        khẩn cấp
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TT2Friend">
                                    <HeaderTemplate>
                                        Thông tin<br />
                                        2Friends
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LS3G">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        3G
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblLichSu3G" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                    <tr>
                                                        <th align="center" scope="col">STT</th>
                                                        <th scope="col">MA_DV</th>
                                                        <th scope="col">TEN_GOI</th>
                                                        <th scope="col">NGAY_BAT_DAU</th>
                                                        <th scope="col">NGAY_KET_THUC</th>
                                                        <th scope="col">ACTIVE</th>
                                                        <th scope="col">GIA_HAN</th>
                                                        <th scope="col">LOAI_TB</th>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LSVAS">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        VAS
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TraCuuKM">
                                    <HeaderTemplate>
                                        Tra cứu<br />
                                        khuyến mại
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TraCuuSoPhutGoi">
                                    <HeaderTemplate>
                                        Tra cứu số<br />
                                        phút đã gọi
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="TraCuuSMS888">
                                    <HeaderTemplate>
                                        Tra cứu<br />
                                        SMS 888
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblTraCuuSMS888" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                    <tr>
                                                        <th align="center" scope="col">STT</th>
                                                        <th scope="col">DIA_CHI_NHAN</th>
                                                        <th scope="col">DIA_CHI_GUI</th>
                                                        <th scope="col">NOI_DUNG</th>
                                                        <th scope="col">THOI_GIAN</th>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                            
                                <cc2:TabPanel runat="server" HeaderText="TabPanel1" ID="LichSuBuTen">
                                    <HeaderTemplate>
                                        Lịch sử<br />
                                        bù tiền
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="p8">
                                            <div style='height: 300px; overflow: scroll'>
                                                <table class="tbl_style" id="tblLichSuBuTien" cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">
                                                    <tr>
                                                        <th align="center" scope="col">STT</th>
                                                        <th scope="col">Mã KN</th>
                                                        <th scope="col">Người TH</th>
                                                        <th scope="col">Số tiền</th>
                                                        <th scope="col">Dịch vụ</th>
                                                        <th scope="col">Thời gian</th>
                                                        <th scope="col">Số công văn</th>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                            </cc2:tabcontainer>
                        </td>
                    </tr>
                </table>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
