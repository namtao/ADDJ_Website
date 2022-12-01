
var listID = '';
var pageSize = '10';
var backPage = true;

function fnSetSizeDiv() {
    var d = $('body').innerWidth() - 56;
    var h = screen.height;
    $("#divScroll").css("width", d);
    $(".divOpacity").css("height", h);

}

function ShowThongBao(msg) {
    $('#spanThongBao').html(msg);
    $('#spanThongBao').show();
    setTimeout(function () { $('#spanThongBao').hide(500); }, 3000);
}

window.onresize = function (event) {
    fnSetSizeDiv();
}

//Utility Page
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

    if (param == "tab1-KNDaGuiDi") {
        fnGetTotalKhieuNaiDaGuiDi(57, "totalKNDaGuiDi");
    }
    else if (param == "tab2-KNPhanHoi") {
        fnGetTotalKhieuNai(58, "totalKNPhanHoi");
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

    } else {
        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần chuyển !', 'error');
    }
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
        }
    });
}

function ClosePoupChuyenXuLy() {
    $('.divOpacity').css('display', 'none');
    $('#divPoupPhongBan').hide();
}

function ClosePoupChuyenXuLyAuTo() {
    $('.divOpacity').css('display', 'none');
    $('#divPoupChuyenXuLyAuTo').hide();
}

function ShowPopupChuyenXuLyAuto() {
    $('#divPoupChuyenXuLyAuTo').show();
    $('.divOpacity').css('display', 'block');

    // Phi Hoang Hai
    fnLoadTuDongDinhTuyenAndPhongBanCungDoiTac();
}

// Author : Phi Hoang Hai
// Edit : 13/05/2014
// Todo : Chuyển xử lý tự động kiểm tra
//          nếu định tuyến thì gửi vào phòng ban mặc định
//          nếu chuyển phòng ban (cùng đối tác) thì gọi hàm chuyển xử lý
function ChuyenXuLyAuto() {
    listID = "";
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }
    })

    var mode = Utility.GetUrlParam("Mode");
    var note = $('#txtNoiDungXuLyChuyenXuLyAuTo').val();

    if (note == "") {
        MessageAlert.AlertNormal('Vui lòng nhập nội xung xử lý.', 'error', $('#txtNoiDungXuLyChuyenXuLyAuTo').attr('id'));
        return;
    }

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
    else if (phongBanId == -1) // Từ động định tuyến lên VNP
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

//function ChuyenXuLyAuto() {
//    listID = "";
//    $(".checkbox-item").each(function () {
//        if (this.checked) {
//            listID += $(this).val() + ",";
//        }
//    })

//    var mode = Utility.GetUrlParam("Mode");
//    var note = $('#txtNoiDungXuLyChuyenXuLyAuTo').val();

//    if (note == "") {
//        MessageAlert.AlertNormal('Vui lòng nhập nội xung xử lý.', 'error', $('#txtNoiDungXuLyChuyenXuLyAuTo').attr('id'));
//        return;
//    }

//    $.ajax({
//        beforeSend: function () {
//        },
//        type: "POST",
//        dataType: "text",
//        url: "../Ajax/Ajax.ashx",
//        data: { type: "ChuyenListKN", NoiDungXuLy: note, ListId: listID },
//        success: function (text) {
//            if (text != "") {
//                fnLocKhieuNai();
//                MessageAlert.AlertNormal(text, 'error');
//            }
//            else {
//                ClosePoup();
//                fnLocKhieuNai();
//                fnGetTotalRecordTabCurr();
//                MessageAlert.AlertNormal('Chuyển khiếu nại thành công', 'info');
//                $('#txtNoiDungXuLyChuyenXuLyAuTo').val('');
//            }
//        },
//        error: function () {
//        }
//    });

//}

function ShowPoupChuyenXuLy() {
}

function ShowPoupTiepNhanKN() {
    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }
    })
    if (listID != '') {
        $('#divPoupTiepNhanKN').show();
        $('.divOpacity').css('display', 'block');
    } else {

        MessageAlert.AlertNormal('Vui lòng chọn khiếu nại cần tiếp nhận!', 'error');
    }
}

function ShowPoupChuyenPhanHoi() {

    $(".checkbox-item").each(function () {
        if (this.checked) {
            listID += $(this).val() + ",";
        }

    })
    if (listID != '') {
        $('#divPoupChuyenPhanHoi').show();
        $('.divOpacity').css('display', 'block');
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
        $('#divPoupChuyenNgangHang').show();
        $('.divOpacity').css('display', 'block');
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
        $('#divPoupDongKhieuNai').show();
        $('.divOpacity').css('display', 'block');
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
                $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=21&listID=' + listID + '&phongban=' + phongBan + '&Username=' + Username, { data: note },
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

function ChuyenPhanHoi() {

    if (listID != '') {
        var note = $("#txtNoteChuyenPhanHoi").val();
        if (note != "") {
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
        var note = $("#txtNoteChuyenNgangHang").val();
        if (note != "") {
            $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=53&listID=' + listID, { data: note },
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
        if (note != "") {
            $.messager.confirm('Thông báo', "Bạn chắc chắn muốn đóng khiếu nại?", function (result) {
                if (result) {
                    $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=24&listID=' + list, { data: note },
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
            MessageAlert.AlertNormal('Nội dung có dấu (*) là không được để trống', 'error');
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
    $('.divOpacity').css('display', 'none');
    $('#divPoupPhongBan').hide();
    $('#divPoupChuyenPhanHoi').hide();
    $('#divPoupChuyenNgangHang').hide();
    $('#divPoupDongKhieuNai').hide();
    $('#divPoupChuyenXuLyAuTo').hide();
    $('#divPoupTiepNhanKN').hide();
    $('#divPopupConfigColumn').hide();
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

    if (isLoadFlex) {
        $('.flex_KNChoXuLy').flexOptions({ params: param }).flexReload();
    }
    else {
        var urlQuery = '/Views/QLKhieuNai/Handler/HandlerKNGuiDi.ashx?key=' + keyGetHTML;
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
            callFunctionAfterReload: function () {
                $('a.normalTip').aToolTip();
            },
            callFunctionUpdateColumn: function (before, after) {
                UpdateColumn(before, after);
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

    var urlQuery = '/Views/QLKhieuNai/Handler/HandlerCountMyKhieuNai.ashx?key=' + keyTotal
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
        error: function () {
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
            + '&SoThueBao=' + stb
        + '&sortname=' + sortname
            + '&sortorder=' + sortorder;
    window.open(urlQuery);
}

function fnExportExcelChiTiet() {
    $('.divOpacity').css('display', 'block');
    var contentSeach = $("#contentSeach").val();
    var typeSearch = $("#DropKhieuNai").val();
    var pageSizeExp = $("#DropPageSize").val();
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

    var ShowNguoiXuLy = $("#chkShowCaNhan").attr('checked');
    if (ShowNguoiXuLy == undefined || ShowNguoiXuLy == null) {
        ShowNguoiXuLy = 0;
    }
    else if (ShowNguoiXuLy == 'checked') {
        ShowNguoiXuLy = 1;
    }
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
            + '&pageSize=1'
            + '&startPageIndex=1'
        + '&sortname=' + sortname
            + '&sortorder=' + sortorder;
    $.ajax({
        beforeSend: function () {
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
            SoThueBao: stb
        },
        success: function (result) {
            if (result != '') {
                window.location.href = "/ExportExcel/Excel" + result;
                $('.divOpacity').css('display', 'none');
            } else {
                $('.divOpacity').css('display', 'none');
                MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại', 'error');
            }
        },
        error: function () {
        }
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
