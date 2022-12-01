// Biến toàn cục
var listID = '';
var pageSize = '';
var backPage = true;

function fnSetSizeDiv() {
    var d = $('body').innerWidth() - 56;
    var h = screen.height;
    $("#divScroll").css("width", d);

}

function closePopup(id) {
    $("#" + id).hide();
    common.unLoading();
}

function ShowThongBao(msg) {
    $('#spanThongBao').html(msg);
    $('#spanThongBao').show();
    setTimeout(function () { $('#spanThongBao').hide(500); }, 3000);
}

window.onresize = function (event) {
    fnSetSizeDiv();
}

// Utility Page
function addCommas(str) {
    var amount = new String(str);
    amount = amount.split("").reverse();

    var output = "";
    for (var i = 0; i <= amount.length - 1; i++) {
        output = amount[i] + output;
        if ((i + 1) % 3 == 0 && (amount.length - 1) !== i) output = ',' + output;
    }
    return output;
}

function getOptionsFromForm() {
    var opt = { callback: pageselectCallback };
    $("input:text").each(function () {
        opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
    });
    return opt;
}
//End Utility Page


//Popup

function fnGetTotalRecordTabCurr() {
    var param = fnGetUrlParameter('ctrl');

    if (param == "tab0-KNPhanViec") {
        fnGetTotalKhieuNai(57, "totalTongHopChoXuLy");
    }
    else if (param == "tab6-KNTongHopChoXuLy") {
        fnGetTotalKhieuNai(57, "totalTongHopChoXuLy");
    }
    else if (param == "tab1-KNChoXuLy") {
        fnGetTotalKhieuNai(8, "totalChoXuLy");
    }
    else if (param == "tab2-KNChuyenBoPhanKhac") {
        fnGetTotalKhieuNai(9, "totalChuyenBoPhanKhac");
    }
    else if (param == "tab3-KNBoPhanKhacChuyenVe") {
        fnGetTotalKhieuNai(13, "totalBoPhanKhacChuyenVe");
    }
    else if (param == "tab4-KNSapQuaHan") {
        fnGetTotalKhieuNai(16, "totalQuaHan");
    }
    else if (param == "tab5-KNCanDong") {
        fnGetTotalKhieuNai(19, "totalCanDong");
    }
    else if (param == "tab7-KNDaPhanHoi") {
        fnGetTotalKhieuNai(62, "totalDaPhanHoi");
    }
}

function fnDropPageSizeChange() {
    pageSize = $('#DropPageSize').val();
    fnLocKhieuNai();
}


function ShowPoupPhongBan() {
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }
    })
    if (listID != '') {
        // Hiển thị mặt nạ
        common.showBodyMark();
        $.ajax({
            type: "GET",
            url: "/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx",
            data: {
                type: "CheckDinhTuyen"
            },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                if (data == "1") {
                    $('#divPoupPhongBan').show();
                }
                else if (data == "2") {
                    ShowPopupChuyenXuLyAuto();
                }
                else {
                    MessageAlert.AlertNormal(data, 'error');
                }
            }
        });

    } else {
        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần chuyển !', 'error');
    }
}

function ClosePoupChuyenXuLyAuTo() {
    $('#divPoupChuyenXuLyAuTo').hide();
    common.unLoading();
}

function ShowPopupChuyenXuLyAuto() {
    $('#divPoupChuyenXuLyAuTo').show();

    // Phi Hoang Hai
    fnLoadTuDongDinhTuyenAndPhongBanCungDoiTac();
}

// Author : Phi Hoang Hai
// Edit : 13/05/2014
// Todo : Chuyển xử lý tự động kiểm tra
// Nếu định tuyến thì gửi vào phòng ban mặc định
// Nếu chuyển phòng ban (cùng đối tác) thì gọi hàm chuyển xử lý

// Nghiệp vụ chuyển xử lý
function ChuyenXuLyAuto() {
    // Danh sách KN cần chuyển
    listID = "";
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }
    })

    var mode = Utility.GetUrlParam("Mode");

    // Nội dung xử lý
    var note = $('#txtNoiDungXuLyChuyenXuLyAuTo').val();

    if (note == "") { // Kiểm tra nội dung xử lý
        MessageAlert.AlertNormal('Vui lòng nhập nội xung xử lý.', 'error', $('#txtNoiDungXuLyChuyenXuLyAuTo').attr('id'));
        return;
    }

    $('#divPoupChuyenXuLyAuTo').hide(); // Ẩn form
    common.loading();

    var phongBanId = $('#ddlTuDongDinhTuyenAndPhongBanCungDoiTac').val();
    if (phongBanId == 0) // Từ động định tuyến
    {
        $.ajax({
            beforeSend: function () {
            },
            type: "POST",
            dataType: "text",
            url: "../Ajax/Ajax.ashx",
            data: { type: "ChuyenListKN", NoiDungXuLy: note, ListId: listID },
            success: function (text) {
                if (text != "") {
                    fnLocKhieuNai();
                    MessageAlert.AlertNormal(text, 'error');
                }
                else {
                    ClosePoup();
                    fnLocKhieuNai();
                    fnGetTotalRecordTabCurr();
                    MessageAlert.AlertNormal('Chuyển khiếu nại thành công', 'info');
                    $('#txtNoiDungXuLyChuyenXuLyAuTo').val('');
                }
            },
            error: function () {
            }
        });
    }
    else if (phongBanId == -1) // Chuyển lên Vinaphone
    {
        $.ajax({
            beforeSend: function () {
            },
            type: "POST",
            dataType: "text",
            url: "../Ajax/Ajax.ashx",
            data: { type: "ChuyenListKNVNP", NoiDungXuLy: note, ListId: listID },
            success: function (text) {
                if (text != "") {
                    fnLocKhieuNai();
                    MessageAlert.AlertNormal(text, 'error');
                }
                else {
                    ClosePoup();
                    fnLocKhieuNai();
                    fnGetTotalRecordTabCurr();
                    MessageAlert.AlertNormal('Chuyển khiếu nại thành công', 'info');
                    $('#txtNoiDungXuLyChuyenXuLyAuTo').val('');
                }
            },
            error: function () {
            }
        });
    }
    else // Chuyển khiếu nại tới phongBanId
    {
        var Username = $('#ddlUserInPhongBan_divPoupChuyenXuLyAuTo').val();
        if (Username == null || Username == '') {
            Username = '-1';
        }

        if (note != "") {
            $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=21&listID=' + listID + '&phongban=' + phongBanId + '&Username=' + Username, { data: note },
        function (result) {
            if (result == '0') {
                MessageAlert.AlertNormal('Chuyển khiếu nại không thành công ! Vui lòng kiểm tra lại', 'error');
            }
            else if (result == "-2") {
                ClosePoup();
                MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
            }
            else {
                ClosePoup();
                fnLocKhieuNai();
                fnGetTotalRecordTabCurr();
                ShowThongBao('Chuyển phòng ban ' + result + ' khiếu nại thành công !');

                $("#txtNoteChuyenXuLy").val('');
            }
        });
        } else {
            MessageAlert.AlertNormal('Nội dung có dấu (*) là không được để trống', 'error');
        }
    }
}

function ShowPoupChuyenXuLy() {
}

function ShowPoupTiepNhanKN() {
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }
    })
    if (listID != '') {
        common.showBodyMark();
        $('#divPoupTiepNhanKN').show();
    } else {

        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần tiếp nhận!', 'error');
    }
}

// Nghiệp vụ chuyển phản hồi
function ShowPoupChuyenPhanHoi() {
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }

    })
    if (listID != '') {
        $('#divPoupChuyenPhanHoi').show();
        common.showBodyMark();

    } else {

        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần chuyển !', 'error');
    }
}
function ShowPoupChuyenNgangHang() {

    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }

    })
    if (listID != '') {
        common.showBodyMark();
        $('#divPoupChuyenNgangHang').show();

    } else {

        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần chuyển !', 'error');
    }

}
function ShowPoupDongKhieuNai() {

    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }

    })
    if (listID != '') {
        common.showBodyMark();
        $('#divPoupDongKhieuNai').show();
    } else {

        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần đóng !', 'error');
    }

}
function ValidatePhongBan() {
    var ss = $('input[name="SelectPhongBan"]:checked').val();

    var radio = document.getElementsByName('SelectPhongBan');
    var isChecked = false;
    for (var i = 0; i < radio.length; i++) {
        if (radio[i].checked) {
            isChecked = true;
            break;
        }
    }
    if (!isChecked) {

        MessageAlert.AlertNormal('Vui lòng chọn phòng ban', 'error');

    }

    return isChecked;
}

function ChuyenXuLy() {
    if (listID != '') {
        if (ValidatePhongBan()) {
            var phongBan = $('input[name="SelectPhongBan"]:checked').val();
            var note = $("#txtNoteChuyenXuLy").val();
            var Username = $('#ddlUserInPhongBan').val();
            if (Username == null || Username == '') {
                Username = '-1';
            }
            if (note != "") {
                $("#divPoupPhongBan").hide(); // Ẩn cửa sổ
                common.loading(); // Hiển thị "Đang xử lý"

                $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=21&listID=' + listID + '&phongban=' + phongBan + '&Username=' + Username, { data: note },
                function (result) {
                    var objJson = $.parseJSON($.parseJSON(result));
                    if (objJson.Code == '0') {
                        MessageAlert.AlertNormal('Chuyển khiếu nại không thành công ! Vui lòng kiểm tra lại', 'error');
                    }
                    else if (objJson.Code == "-2") {
                        ClosePoup();
                        MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                    }
                    else {
                        ClosePoup();
                        fnLocKhieuNai();
                        fnGetTotalRecordTabCurr();
                        ShowThongBao('Chuyển phòng ban ' + objJson.Message + ' khiếu nại thành công !');
                        $("#txtNoteChuyenXuLy").val('');
                    }
                });

            } else {
                MessageAlert.AlertNormal('Nội dung có dấu (*) là không được để trống', 'error');
            }
        }
    }
}


// Nghiệp vụ chuyển phản hồi
function ChuyenPhanHoi() {
    if (listID != '') {
        var note = $("#txtNoteChuyenPhanHoi").val();
        if (note != "") {

            $('#divPoupChuyenPhanHoi').hide(); // Ẩn form
            common.loading();

            $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=52&listID=' + listID, { data: note },
            function (result) {
                if (result == '0') {
                    MessageAlert.AlertNormal('Chuyển khiếu nại không thành công ! Vui lòng kiểm tra lại', 'error');
                }
                else if (result == "-2") {
                    ClosePoup();
                    MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                }
                else {
                    ClosePoup();
                    fnLocKhieuNai();
                    fnGetTotalRecordTabCurr();
                    ShowThongBao('Chuyển phản hồi ' + result + ' khiếu nại thành công !');

                    $("#txtNoteChuyenPhanHoi").val('');
                }
            });
        } else {
            MessageAlert.AlertNormal('Nội dung có dấu (*) là không được để trống', 'error');
        }
    }
}

function ChuyenNgangHang() {
    if (listID != '') {
        var username = $('#ContentPlaceHolder_Main_ctl00_PopupChoXuLy1_ddlUserNgangHang').val();
        var note = $("#txtNoteChuyenNgangHang").val();
        if (note != "") {
            $("#divPoupChuyenNgangHang").hide(); // Ẩn form đi
            common.loading(); // Hiển thị "Đang xử lý"

            $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=53&listID=' + listID + '&userName=' + username, { data: note },
            function (result) {
                if (result == "-1") {
                    ClosePoup();
                    MessageAlert.AlertNormal('Chuyển khiếu nại không thành công ! Vui lòng kiểm tra lại', 'error');
                }
                else if (result == "-2") {
                    ClosePoup();
                    MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                }
                else {
                    ClosePoup();
                    fnLocKhieuNai();
                    fnGetTotalRecordTabCurr();
                    ShowThongBao('Chuyển ngang hàng ' + result + ' khiếu nại thành công !');
                    $("#txtNoteChuyenNgangHang").val('');
                }
            });
        } else {
            MessageAlert.AlertNormal('Nội dung có dấu (*) là không được để trống', 'error');
        }
    }
}

function DongKhieuNai() {
    var list = '';
    $(".checkbox-item").each(function () {
        if (this.checked) {
            list += $(this).val() + ",";
        }
    });
    if (list != '') {
        var note = $("#txtNoteDongKhieuNai").val();
        var DoHaiLong = $("#slDoHaiLong").val();
        var nguyenNhanLoiId = $("#ContentPlaceHolder_Main_ctl00_PopupChoXuLy1_ddlNguyenNhanLoi").val();
        var chiTietLoiId = $("#ContentPlaceHolder_Main_ctl00_PopupChoXuLy1_ddlChiTietLoi").val();
        if (nguyenNhanLoiId == "0") {
            MessageAlert.AlertNormal('Bạn phải chọn nguyên nhân lỗi của khiếu nại.', 'error');
            return;
        }
        if (chiTietLoiId == "-1") {
            MessageAlert.AlertNormal('Bạn phải chọn chi tiết lỗi của khiếu nại.', 'error');
            return;
        }
        if (DoHaiLong == "-1") {
            MessageAlert.AlertNormal('Bạn phải chọn độ hài lòng của khách hàng.', 'error');
            return;
        }
        if (note != "") {
            $.messager.confirm('Thông báo', "Bạn chắc chắn muốn đóng khiếu nại?", function (result) {
                if (result) {
                    $("#divPoupDongKhieuNai").hide(); // Ẩn cái form
                    common.loading(); // Chạy cái "Đang xử lý"

                    // Gửi Ajax đóng KN
                    $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=24&listID=' + list, { data: note, DoHaiLong: DoHaiLong, nguyenNhanLoiId: nguyenNhanLoiId, chiTietLoiId: chiTietLoiId },
                   function (result) {
                       if (result == "-1") {
                           ClosePoup();
                           MessageAlert.AlertNormal('Có lỗi xảy ra khi đóng khiếu nại!', 'error');
                       }
                       else if (result == "-2") {
                           ClosePoup();
                           MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                       }
                       else {
                           ClosePoup();
                           fnLocKhieuNai();
                           fnGetTotalRecordTabCurr();
                           ShowThongBao('Đóng thành công ' + result + ' khiếu nại !');
                           $("#txtNoteDongKhieuNai").val('');
                       }
                   });
                }
            });
        } else {
            MessageAlert.AlertNormal('Bạn phải nhập nội dung xử lý đóng khiếu nại', 'error');
        }

    } else {
        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần đóng !', 'error');
    }
}

function LoadUserInPhongBan(phongBanId) {
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

function fnTiepNhanKN() {
    var list = '';
    $(".checkbox-item").each(function () {
        if (this.checked) {
            list += $(this).val() + ",";
        }
    });
    if (list != '') {
        $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=10001&listID=' + list,
                function (result) {
                    if (result == "-1") {
                        MessageAlert.AlertNormal('Tiếp nhận khiếu nại không thành công!', 'error');
                    }
                    else if (result == "-2") {
                        MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                    }
                    else {
                        ClosePoup();
                        fnLocKhieuNai();
                        fnGetTotalRecordTabCurr();
                        ShowThongBao('Tiếp nhận ' + result + ' khiếu nại thành công !');
                    }
                });
    } else {
        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần xử lý !', 'error');
    }
}

function UpdateKNHangLoat() {
    var list = '';
    $(".checkbox-item").each(function () {
        if (this.checked) {
            list += $(this).val() + ",";
        }
    });
    if (list != '') {
        $.messager.confirm('Thông báo', "Bạn chắc chắn muốn cập nhật khiếu nại thành hàng loạt?", function (result) {
            if (result) {
                $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=59&listID=' + list,
                    function (result) {
                        if (result == "-1") {
                            MessageAlert.AlertNormal('Cập nhật khiếu nại không thành công!', 'error');
                        }
                        else if (result == "-2") {
                            MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này. Liên hệ quản trị hệ thống!', 'error');
                        }
                        else {
                            ClosePoup();
                            fnLocKhieuNai();
                            fnGetTotalRecordTabCurr();
                            //MessageAlert.AlertNormal('Cập nhật khiếu nại thành công !');
                            ShowThongBao('Cập nhật khiếu nại thành công !');
                        }
                    });
            }
        });
    } else {
        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần xử lý !', 'error');
    }
}

function ClosePoup() {
    listID = '';
    $('#divPoupPhongBan').hide();
    $('#divPoupChuyenPhanHoi').hide();
    $('#divPoupChuyenNgangHang').hide();
    $('#divPoupDongKhieuNai').hide();
    $('#divPoupChuyenXuLyAuTo').hide();
    $('#divPoupTiepNhanKN').hide();
    $('#divPopupConfigColumn').hide();
    common.unLoading();
}


//Selected
function SelectedRow() {
    $('.rowA').click(function () {
        $('.rowA').removeClass('rowSelected');
        $(this).addClass('rowSelected');
    });
}




//Lọc
var isLoadFlex = false;
var keyTotal = 0;
var keyGetHTML = 0;
var keyExcel = 0;
var pageCurrentSelect = 0;

function pageselectCallback(page_index) {
    var curentPages = page_index + 1;
    pageCurrentSelect = curentPages;
    var contentSeach = $("#contentSeach").val();
    var typeSearch = $("#DropKhieuNai").val();
    var doUuTien = $("#DropdoUuTien").val();
    if (doUuTien == null) {
        doUuTien = -1;
    }
    var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
    if (loaiKhieuNai == null) {
        loaiKhieuNai = -1;
    }
    var linhVucChung = $("#DropLinhVucChung").val();
    if (linhVucChung == null) {
        linhVucChung = -1;
    }
    var linhVucCon = $("#DropLinhVucCon").val();
    if (linhVucCon == null) {
        linhVucCon = -1;
    }
    var trangThai = $("#DropTrangThai").val();
    if (trangThai == null) {
        trangThai = -1;
    }
    var phongBanXuLy = $("#DropPhongBanXuLy").val();
    if (phongBanXuLy == null) {
        phongBanXuLy = -1;
    }
    //LONGLX
    var stb = $("#txtSoThueBao").val();
    if (stb == undefined || stb == null) {
        stb = "";
    }
    var strNguoiTiepNhan = $("#txtNguoiTiepNhan").val();
    if (strNguoiTiepNhan == undefined || strNguoiTiepNhan == null) {
        strNguoiTiepNhan = "";
    }

    var strNguoiTienXuLy = $("#txtNguoiTienXuLy").val();
    if (strNguoiTienXuLy == undefined || strNguoiTienXuLy == null) {
        strNguoiTienXuLy = "";
    }

    var strNguoiXuLy = $("#txtNguoiXuLy").val();
    if (strNguoiXuLy == undefined || strNguoiXuLy == null) {
        strNguoiXuLy = "";
    }


    var NgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
    if (NgayTiepNhan_From == undefined || NgayTiepNhan_From == null) {
        NgayTiepNhan_From = "";
    }
    var NgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();
    if (NgayTiepNhan_To == undefined || NgayTiepNhan_To == null) {
        NgayTiepNhan_To = "";
    }

    var NgayQuaHan_From = $("#txtNgayQuaHan_From").val();
    if (NgayQuaHan_From == undefined || NgayQuaHan_From == null) {
        NgayQuaHan_From = "";
    }
    var NgayQuaHan_To = $("#txtNgayQuaHan_To").val();
    if (NgayQuaHan_To == undefined || NgayQuaHan_To == null) {
        NgayQuaHan_To = "";
    }

    var NgayQuaHanPB_From = $("#txtNgayQuaHanPB_From").val();
    if (NgayQuaHanPB_From == undefined || NgayQuaHanPB_From == null) {
        NgayQuaHanPB_From = "";
    }
    var NgayQuaHanPB_To = $("#txtNgayQuaHanPB_To").val();
    if (NgayQuaHanPB_To == undefined || NgayQuaHanPB_To == null) {
        NgayQuaHanPB_To = "";
    }

    var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
        ShowNguoiXuLy = 0;
    }
    else if (ShowNguoiXuLy == 'checked') {
        ShowNguoiXuLy = 1;
    }

    //Param de truyen vao ajax. O day la ashx
    var param = [{ name: 'typeSearch', value: typeSearch },
                    { name: 'doUuTien', value: doUuTien },
                    { name: 'trangThai', value: trangThai },
                    { name: 'loaiKhieuNai', value: loaiKhieuNai },
                    { name: 'linhVucChung', value: linhVucChung },
                    { name: 'linhVucCon', value: linhVucCon },
                    { name: 'phongBanXuLy', value: phongBanXuLy },
                    { name: 'ShowNguoiXuLy', value: ShowNguoiXuLy },
                    { name: 'contentSeach', value: contentSeach },
                    { name: 'NguoiTiepNhan', value: strNguoiTiepNhan },
                    { name: 'NguoiXuLy', value: strNguoiXuLy },
                    { name: 'NguoiTienXuLy', value: strNguoiTienXuLy },
                    { name: 'NgayTiepNhan_From', value: NgayTiepNhan_From },
                    { name: 'NgayTiepNhan_To', value: NgayTiepNhan_To },
                    { name: 'NgayQuaHan_From', value: NgayQuaHan_From },
                    { name: 'NgayQuaHan_To', value: NgayQuaHan_To },
                    { name: 'NgayQuaHanPB_From', value: NgayQuaHanPB_From },
                    { name: 'NgayQuaHanPB_To', value: NgayQuaHanPB_To },
                    { name: 'SoThueBao', value: stb },
                    { name: 'pageSize', value: pageSize },
                    { name: 'startPageIndex', value: curentPages }];
    //ColModel
    /*
    { display: '<input id='selectall' onclick='javascript: SelectAllCheckboxes();' type='checkbox' />', name: 'CheckAll', width: 40, sortable: false, align: 'center' },
    { display: 'STT', name: 'STT', width: 40, sortable: false, align: 'center' },
    { display: 'Trạng thái', name: 'TrangThai', width: 70, sortable: true, align: 'center' },
    { display: 'Mã PA/KN', name: 'Id', width: 110, sortable: true, align: 'center' },
    { display: 'Ðộ uu tiên', name: 'DoUuTien', width: 70, sortable: true, align: 'center' },
    { display: 'Số thuê bao', name: 'SoThueBao', width: 110, sortable: true, align: 'center' },
    { display: 'Nội dung PA', name: 'NoiDungPA', width: 200, sortable: false, align: 'left' },
    { display: 'Loại khiếu nại', name: 'LoaiKhieuNai', width: 150, sortable: false, align: 'left' },
    { display: 'Lĩnh vực chung', name: 'LinhVucChung', width: 200, sortable: false, align: 'left' },
    { display: 'Lĩnh vực con', name: 'LinhVucCon', width: 200, sortable: false, align: 'left' },

    { display: 'Phòng ban TN', name: 'PhongBanTiepNhan', width: 120, sortable: false, align: 'center', hide: true },
    { display: 'Người TN', name: 'NguoiTiepNhan', width: 120, sortable: false, align: 'center' },

    { display: 'Người tiền XL', name: 'NguoiXuLyTruoc', width: 120, sortable: false, align: 'center' },
    { display: 'Người được phản hồi', name: 'NguoiDuocPhanHoi', width: 120, sortable: false, align: 'center', hide: true },
    { display: 'Phòng ban XL', name: 'PhongBanXuLy', width: 120, sortable: false, align: 'center', hide: true },

    { display: 'Người XL', name: 'NguoiXuLy', width: 120, sortable: false, align: 'center' },

    { display: 'Phân việc', name: 'IsPhanViec', width: 80, sortable: false, align: 'center' },
    { display: 'Ngày TN', name: 'NgayTiepNhanSort', width: 130, sortable: true, align: 'center' },
    { display: 'Ngày quá hạn PB', name: 'NgayQuaHanPhongBanXuLySort', width: 130, sortable: true, align: 'center' },
    { display: 'Ngày quá hạn TT', name: 'NgayQuaHanSort', width: 130, sortable: true, align: 'center' },                

    { display: 'Ngày cập nhật', name: 'LDate', width: 130, sortable: true, align: 'center' },
    */
    //Common.Loading();
    if (isLoadFlex) {
        $('.flex_KNChoXuLy').flexOptions({ params: param }).flexReload();
    }
    else {
        var urlQuery = '/Views/QLKhieuNai/Handler/HandlerKNChoXuLy.ashx?key=' + keyGetHTML;
        $(".flex_KNChoXuLy").flexigrid({
            url: urlQuery,
            params: param,
            dataType: 'json',
            colModel: strConfigColumn,
            useUpdateCol: true,
            sortname: "LDate",
            sortorder: "desc",
            useStatusBar: true,
            pagestat: "",
            rp: 500,
            beforeSend: function () {
                Common.Loading();
            },
            callFunctionAfterReload: function () {
                $('a.normalTip').aToolTip();
                TestNhay();
            },
            callFunctionUpdateColumn: function (before, after) {
                UpdateColumn(before, after);
            },
            complete: function (xhr, textStatus) {
                Common.UnLoading();
            },
            height: 316
        });

        isLoadFlex = true;
    }
    return false;
}

function UpdateColumn(before, after) {
    var param = fnGetUrlParameter('ctrl');

    $.ajax({
        type: "GET",
        url: "/Views/QLKhieuNai/XMLFiles/HandlerUpdateXML.ashx",
        data: {
            ctrl: param,
            before: before,
            after: after
        },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (data) {
        }
    });
}


function fnLocKhieuNai() {

    var optInit = getOptionsFromForm();
    var contentSeach = $("#contentSeach").val();
    var typeSearch = $("#DropKhieuNai").val();
    var doUuTien = $("#DropdoUuTien").val();
    if (doUuTien == null) {
        doUuTien = -1;
    }
    var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
    if (loaiKhieuNai == null) {
        loaiKhieuNai = -1;
    }
    var linhVucChung = $("#DropLinhVucChung").val();
    if (linhVucChung == null) {
        linhVucChung = -1;
    }
    var linhVucCon = $("#DropLinhVucCon").val();
    if (linhVucCon == null) {
        linhVucCon = -1;
    }
    var trangThai = $("#DropTrangThai").val();
    if (trangThai == null) {
        trangThai = -1;
    }

    var pIndex = 1;
    var pIndexTemp = Utility.GetUrlParam("PIndex");
    if (pIndexTemp != "") {
        pIndex = pIndexTemp;
    }
    var phongBanXuLy = $("#DropPhongBanXuLy").val();
    if (phongBanXuLy == null) {
        phongBanXuLy = -1;
    }
    //LONGLX
    var stb = $("#txtSoThueBao").val();
    if (stb == undefined || stb == null) {
        stb = "";
    }

    var strNguoiTiepNhan = $("#txtNguoiTiepNhan").val();
    if (strNguoiTiepNhan == undefined || strNguoiTiepNhan == null) {
        strNguoiTiepNhan = "";
    }

    var strNguoiTienXuLy = $("#txtNguoiTienXuLy").val();
    if (strNguoiTienXuLy == undefined || strNguoiTienXuLy == null) {
        strNguoiTienXuLy = "";
    }

    var strNguoiXuLy = $("#txtNguoiXuLy").val();
    if (strNguoiXuLy == undefined || strNguoiXuLy == null) {
        strNguoiXuLy = "";
    }

    var NgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
    if (NgayTiepNhan_From == undefined || NgayTiepNhan_From == null) {
        NgayTiepNhan_From = "";
    }
    var NgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();
    if (NgayTiepNhan_To == undefined || NgayTiepNhan_To == null) {
        NgayTiepNhan_To = "";
    }
    var NgayQuaHan_From = $("#txtNgayQuaHan_From").val();
    if (NgayQuaHan_From == undefined || NgayQuaHan_From == null) {
        NgayQuaHan_From = "";
    }
    var NgayQuaHan_To = $("#txtNgayQuaHan_To").val();
    if (NgayQuaHan_To == undefined || NgayQuaHan_To == null) {
        NgayQuaHan_To = "";
    }

    var NgayQuaHanPB_From = $("#txtNgayQuaHanPB_From").val();
    if (NgayQuaHanPB_From == undefined || NgayQuaHanPB_From == null) {
        NgayQuaHanPB_From = "";
    }
    var NgayQuaHanPB_To = $("#txtNgayQuaHanPB_To").val();
    if (NgayQuaHanPB_To == undefined || NgayQuaHanPB_To == null) {
        NgayQuaHanPB_To = "";
    }

    var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
        ShowNguoiXuLy = 0;
    }
    else if (ShowNguoiXuLy == 'checked') {
        ShowNguoiXuLy = 1;
    }
    //END LONGLX
    var curentPages = pageCurrentSelect - 1;
    if (pageCurrentSelect == 0)
        curentPages = 0;
    if (backPage) {
        var page = Utility.GetUrlParam("PIndex");
        if (page != "") {
            curentPages = page - 1;
        }
        backPage = false;
    }

    var urlQuery = '/Views/QLKhieuNai/Handler/HandlerCountKNChoXuLy.ashx?key=' + keyTotal
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&trangThai=' + trangThai
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&phongBanXuLy=' + phongBanXuLy
            + '&ShowNguoiXuLy=' + ShowNguoiXuLy
            + '&pageSize=' + pageSize
            + '&startPageIndex=' + curentPages;
    $.ajax({
        beforeSend: function () {
            common.loading();
        },
        type: "GET",
        dataType: "text",
        url: urlQuery,
        data: {
            contentSeach: contentSeach,
            NguoiTiepNhan: strNguoiTiepNhan,
            NguoiXuLy: strNguoiXuLy,
            NguoiTienXuLy: strNguoiTienXuLy,
            NgayTiepNhan_From: NgayTiepNhan_From,
            NgayTiepNhan_To: NgayTiepNhan_To,
            NgayQuaHan_From: NgayQuaHan_From,
            NgayQuaHan_To: NgayQuaHan_To,
            NgayQuaHanPB_From: NgayQuaHanPB_From,
            NgayQuaHanPB_To: NgayQuaHanPB_To,
            SoThueBao: stb
        },
        success: function (totalRecords) {
            if (totalRecords != '') {
                if (totalRecords == 0) {
                    $("#Pagination").pagination(0, optInit);
                    $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(0)</span>");
                }
                else {
                    if ((pageSize * (curentPages)) >= totalRecords)
                        curentPages = 0;
                    $("#Pagination").pagination(totalRecords, optInit, curentPages);
                    $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                }
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (console && console.log) console.log(textStatus);
        },
        complete: function (xhr, textStatus) {
            common.unLoading();
            //if (console && console.log) console.log("Hoàn thành");                            
        }
    });

}

function fnExportExcel() {
    var contentSeach = $("#contentSeach").val();
    var typeSearch = $("#DropKhieuNai").val();
    var pageSizeExp = $("#DropPageSize").val();
    var doUuTien = $("#DropdoUuTien").val();
    var sortname = $('#sortname').val();
    var sortorder = $('#sortorder').val();
    //alert( sortorder);
    if (doUuTien == null) {
        doUuTien = -1;
    }
    var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
    if (loaiKhieuNai == null) {
        loaiKhieuNai = -1;
    }
    var linhVucChung = $("#DropLinhVucChung").val();
    if (linhVucChung == null) {
        linhVucChung = -1;
    }
    var linhVucCon = $("#DropLinhVucCon").val();
    if (linhVucCon == null) {
        linhVucCon = -1;
    }
    var trangThai = $("#DropTrangThai").val();
    if (trangThai == null) {
        trangThai = -1;
    }
    var phongBanXuLy = $("#DropPhongBanXuLy").val();
    if (phongBanXuLy == null) {
        phongBanXuLy = -1;
    }

    //LONGLX
    var stb = $("#txtSoThueBao").val();
    if (stb == undefined || stb == null) {
        stb = "";
    }

    var strNguoiTiepNhan = $("#txtNguoiTiepNhan").val();
    if (strNguoiTiepNhan == undefined || strNguoiTiepNhan == null) {
        strNguoiTiepNhan = "";
    }

    var strNguoiTienXuLy = $("#txtNguoiTienXuLy").val();
    if (strNguoiTienXuLy == undefined || strNguoiTienXuLy == null) {
        strNguoiTienXuLy = "";
    }

    var strNguoiXuLy = $("#txtNguoiXuLy").val();
    if (strNguoiXuLy == undefined || strNguoiXuLy == null) {
        strNguoiXuLy = "";
    }

    var NgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
    if (NgayTiepNhan_From == undefined || NgayTiepNhan_From == null) {
        NgayTiepNhan_From = "";
    }
    var NgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();
    if (NgayTiepNhan_To == undefined || NgayTiepNhan_To == null) {
        NgayTiepNhan_To = "";
    }
    var NgayQuaHan_From = $("#txtNgayQuaHan_From").val();
    if (NgayQuaHan_From == undefined || NgayQuaHan_From == null) {
        NgayQuaHan_From = "";
    }
    var NgayQuaHan_To = $("#txtNgayQuaHan_To").val();
    if (NgayQuaHan_To == undefined || NgayQuaHan_To == null) {
        NgayQuaHan_To = "";
    }

    var NgayQuaHanPB_From = $("#txtNgayQuaHanPB_From").val();
    if (NgayQuaHanPB_From == undefined || NgayQuaHanPB_From == null) {
        NgayQuaHanPB_From = "";
    }
    var NgayQuaHanPB_To = $("#txtNgayQuaHanPB_To").val();
    if (NgayQuaHanPB_To == undefined || NgayQuaHanPB_To == null) {
        NgayQuaHanPB_To = "";
    }

    var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
        ShowNguoiXuLy = 0;
    }
    else if (ShowNguoiXuLy == 'checked') {
        ShowNguoiXuLy = 1;
    }
    //END LONGLX

    var urlQuery = '/Views/QLKhieuNai/Excel.aspx?key=' + keyExcel
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&trangThai=' + trangThai
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&phongBanXuLy=' + phongBanXuLy
            + '&ShowNguoiXuLy=' + ShowNguoiXuLy
            + '&contentSeach=' + contentSeach
            + '&NguoiXuLy=' + strNguoiXuLy
            + '&NguoiTiepNhan=' + strNguoiTiepNhan
            + '&NguoiTienXuLy=' + strNguoiTienXuLy
            + '&NgayTiepNhan_From=' + NgayTiepNhan_From
            + '&NgayTiepNhan_To=' + NgayTiepNhan_To
            + '&NgayQuaHan_From=' + NgayQuaHan_From
            + '&NgayQuaHan_To=' + NgayQuaHan_To
            + '&NgayQuaHanPB_From=' + NgayQuaHanPB_From
            + '&NgayQuaHanPB_To=' + NgayQuaHanPB_To
            + '&SoThueBao=' + stb
            + '&sortname=' + sortname
            + '&sortorder=' + sortorder;
    window.open(urlQuery);
}

function fnExportExcelChiTiet() {
    var contentSeach = $("#contentSeach").val();
    var typeSearch = $("#DropKhieuNai").val();
    var doUuTien = $("#DropdoUuTien").val();
    var sortname = $('#sortname').val();
    var sortorder = $('#sortorder').val();
    if (doUuTien == null) {
        doUuTien = -1;
    }
    var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
    if (loaiKhieuNai == null) {
        loaiKhieuNai = -1;
    }
    var linhVucChung = $("#DropLinhVucChung").val();
    if (linhVucChung == null) {
        linhVucChung = -1;
    }
    var linhVucCon = $("#DropLinhVucCon").val();
    if (linhVucCon == null) {
        linhVucCon = -1;
    }
    var trangThai = $("#DropTrangThai").val();
    if (trangThai == null) {
        trangThai = -1;
    }

    var pIndex = 1;
    var pIndexTemp = Utility.GetUrlParam("PIndex");
    if (pIndexTemp != "") {
        pIndex = pIndexTemp;
    }
    var phongBanXuLy = $("#DropPhongBanXuLy").val();
    if (phongBanXuLy == null) {
        phongBanXuLy = -1;
    }
    //LONGLX
    var stb = $("#txtSoThueBao").val();
    if (stb == undefined || stb == null) {
        stb = "";
    }

    var strNguoiTiepNhan = $("#txtNguoiTiepNhan").val();
    if (strNguoiTiepNhan == undefined || strNguoiTiepNhan == null) {
        strNguoiTiepNhan = "";
    }

    var strNguoiTienXuLy = $("#txtNguoiTienXuLy").val();
    if (strNguoiTienXuLy == undefined || strNguoiTienXuLy == null) {
        strNguoiTienXuLy = "";
    }

    var strNguoiXuLy = $("#txtNguoiXuLy").val();
    if (strNguoiXuLy == undefined || strNguoiXuLy == null) {
        strNguoiXuLy = "";
    }

    var NgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
    if (NgayTiepNhan_From == undefined || NgayTiepNhan_From == null) {
        NgayTiepNhan_From = "";
    }
    var NgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();
    if (NgayTiepNhan_To == undefined || NgayTiepNhan_To == null) {
        NgayTiepNhan_To = "";
    }
    var NgayQuaHan_From = $("#txtNgayQuaHan_From").val();
    if (NgayQuaHan_From == undefined || NgayQuaHan_From == null) {
        NgayQuaHan_From = "";
    }
    var NgayQuaHan_To = $("#txtNgayQuaHan_To").val();
    if (NgayQuaHan_To == undefined || NgayQuaHan_To == null) {
        NgayQuaHan_To = "";
    }

    var NgayQuaHanPB_From = $("#txtNgayQuaHanPB_From").val();
    if (NgayQuaHanPB_From == undefined || NgayQuaHanPB_From == null) {
        NgayQuaHanPB_From = "";
    }
    var NgayQuaHanPB_To = $("#txtNgayQuaHanPB_To").val();
    if (NgayQuaHanPB_To == undefined || NgayQuaHanPB_To == null) {
        NgayQuaHanPB_To = "";
    }

    var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
        ShowNguoiXuLy = 0;
    }
    else if (ShowNguoiXuLy == 'checked') {
        ShowNguoiXuLy = 1;
    }

    //Param de truyen vao ajax. O day la ashx
    //var param = [{ name: 'typeSearch', value: typeSearch },
    //                { name: 'doUuTien', value: doUuTien },
    //                { name: 'trangThai', value: trangThai },
    //                { name: 'loaiKhieuNai', value: loaiKhieuNai },
    //                { name: 'linhVucChung', value: linhVucChung },
    //                { name: 'linhVucCon', value: linhVucCon },
    //                { name: 'phongBanXuLy', value: phongBanXuLy },
    //                { name: 'ShowNguoiXuLy', value: ShowNguoiXuLy },
    //                { name: 'contentSeach', value: contentSeach },
    //                { name: 'NguoiTiepNhan', value: strNguoiTiepNhan },
    //                { name: 'NguoiXuLy', value: strNguoiXuLy },
    //                { name: 'NguoiTienXuLy', value: strNguoiTienXuLy },
    //                { name: 'NgayTiepNhan_From', value: NgayTiepNhan_From },
    //                { name: 'NgayTiepNhan_To', value: NgayTiepNhan_To },
    //                { name: 'NgayQuaHan_From', value: NgayQuaHan_From },
    //                { name: 'NgayQuaHan_To', value: NgayQuaHan_To },
    //                { name: 'SoThueBao', value: stb },
    //                { name: 'pageSize', value: pageSize },
    //                { name: 'startPageIndex', value: 1 }];
    // var contentSeach = $("#contentSeach").val();
    // var typeSearch = $("#DropKhieuNai").val();
    // var pageSizeExp = $("#DropPageSize").val();
    // var doUuTien = $("#DropdoUuTien").val();
    //if (doUuTien == null) {
    //     doUuTien = -1;
    // }
    //var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
    //if (loaiKhieuNai == null) {
    //    loaiKhieuNai = -1;
    //}
    //var linhVucChung = $("#DropLinhVucChung").val();
    //if (linhVucChung == null) {
    //    linhVucChung = -1;
    //}
    //var linhVucCon = $("#DropLinhVucCon").val();
    //if (linhVucCon == null) {
    //    linhVucCon = -1;
    //}
    //var trangThai = $("#DropTrangThai").val();
    //if (trangThai == null) {
    //    trangThai = -1;
    //}
    //var phongBanXuLy = $("#DropPhongBanXuLy").val();
    //if (phongBanXuLy == null) {
    //    phongBanXuLy = -1;
    //}

    //LONGLX
    //var stb = $("#txtSoThueBao").val();
    //if (stb == undefined || stb == null) {
    //    stb = "";
    //}
    //var strNguoiTiepNhan = $("#txtNguoiTiepNhan").val();
    //if (strNguoiTiepNhan == undefined || strNguoiTiepNhan == null) {
    //    strNguoiTiepNhan = "";
    //}
    //var strNguoiTienXuLy = $("#txtNguoiTienXuLy").val();
    //if (strNguoiTienXuLy == undefined || strNguoiTienXuLy == null) {
    //    strNguoiTienXuLy = "";
    //}

    //var strNguoiXuLy = $("#txtNguoiXuLy").val();
    //if (strNguoiXuLy == undefined || strNguoiXuLy == null) {
    //    strNguoiXuLy = "";
    //}

    //var NgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
    //if (NgayTiepNhan_From == undefined || NgayTiepNhan_From == null) {
    //    NgayTiepNhan_From = "";
    //}
    //var NgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();
    //if (NgayTiepNhan_To == undefined || NgayTiepNhan_To == null) {
    //    NgayTiepNhan_To = "";
    //}
    //var NgayQuaHan_From = $("#txtNgayQuaHan_From").val();
    //if (NgayQuaHan_From == undefined || NgayQuaHan_From == null) {
    //    NgayQuaHan_From = "";
    //}
    //var NgayQuaHan_To = $("#txtNgayQuaHan_To").val();
    //if (NgayQuaHan_To == undefined || NgayQuaHan_To == null) {
    //    NgayQuaHan_To = "";
    //}

    //var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    //if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
    //    ShowNguoiXuLy = 0;
    //}
    //else if (ShowNguoiXuLy == 'checked') {
    //    ShowNguoiXuLy = 1;
    //}
    //END LONGLX

    var urlQuery = '/Views/QLKhieuNai/Handler/ExportExcelChiTietKN.ashx?key=' + keyExcel
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&trangThai=' + trangThai
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&phongBanXuLy=' + phongBanXuLy
            + '&ShowNguoiXuLy=' + ShowNguoiXuLy
            //+ '&contentSeach=' + contentSeach
            //+ '&NguoiTiepNhan=' + strNguoiTiepNhan
            //+ '&NguoiXuLy=' + strNguoiXuLy
            //+ '&NguoiTienXuLy=' + strNguoiTienXuLy
            //+ '&NgayTiepNhan_From=' + NgayTiepNhan_From
            //+ '&NgayTiepNhan_To=' + NgayTiepNhan_To
            //+ '&NgayQuaHan_From=' + NgayQuaHan_From
            //+ '&NgayQuaHan_To=' + NgayQuaHan_To
          //  + '&SoThueBao=' + stb

            + '&pageSize=' + pageSize
            + '&startPageIndex=1'
        + '&sortname=' + sortname
            + '&sortorder=' + sortorder;
    $.ajax({
        beforeSend: function () {
            common.loading();
        },
        type: "GET",
        dataType: "JSON",
        url: urlQuery,

        data: {
            contentSeach: contentSeach,
            NguoiXuLy: strNguoiXuLy,
            NguoiTiepNhan: strNguoiTiepNhan,
            NguoiTienXuLy: strNguoiTienXuLy,
            NgayTiepNhan_From: NgayTiepNhan_From,
            NgayTiepNhan_To: NgayTiepNhan_To,
            NgayQuaHan_From: NgayQuaHan_From,
            NgayQuaHan_To: NgayQuaHan_To,
            NgayQuaHanPB_From: NgayQuaHanPB_From,
            NgayQuaHanPB_To: NgayQuaHanPB_To,
            SoThueBao: stb
        },
        success: function (result) {
            if (result != '') {
                common.unLoading();
                window.location.href = "/ExportExcel/Excel" + result;
            } else {
                MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại', 'error');
            }
        },
        error: function () {
        },
        complete: function () { common.unLoading(); }
    });

}

var check = 0;
function SelectAllCheckboxes() {
    if (check == 0)
        check = 1;
    else
        check = 0;
    if (check == 1) {
        $('.checkbox-item').each(function () {
            if (!$(this).attr('disabled')) {
                $(this).attr('checked', 'checked');
            }
        });
    }
    else {
        $('.checkbox-item').each(function () {
            if (!$(this).attr('disabled')) {
                $(this).removeAttr('checked', 'checked');
            }
        });
    }
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

// Author : Phi Hoang Hai
// Created date : 13/05/2014
// Todo : Load giá trị định tuyến và các phòng ban cùng đối tác
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