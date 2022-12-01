<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNSoTheoDoi.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.KNSoTheoDoi" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ListKNSoTheoDoi.ascx" TagName="ListKNSoTheoDoi"
    TagPrefix="ListKNSoTheoDoi" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/SoTheoDoiEdit.ascx" TagName="SoTheoDoiEdit"
    TagPrefix="SoTheoDoiEdit" %>
<script src="/JS/jquery.pagination.js" type="text/javascript"></script>
<link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
<script type="text/javascript">

    var pageSize = '';
    //    window.onresize = function (event) {
    //        fnSetSizeDiv();
    //    }
    $(document).ready(function () {
        $("#<%=txtNgayTiepNhan_tu.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#<%=txtNgayTiepNhan_den.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#NgayTiepNhan").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#NgayTraLoiKN").datepick({ dateFormat: 'dd/mm/yyyy' });
        var now = new Date();
        var dateFrom = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 15);

        var day_from = dateFrom.getDate();
        var month_from = dateFrom.getMonth() + 1;
        var year_from = dateFrom.getFullYear();
        $("#<%=txtNgayTiepNhan_tu.ClientID %>").val(day_from + "/" + month_from + "/" + year_from);

        var curr_date = now.getDate();
        var curr_month = now.getMonth() + 1;
        var curr_year = now.getFullYear();
        $("#<%=txtNgayTiepNhan_den.ClientID %>").val(curr_date + "/" + curr_month + "/" + curr_year);
        $("#NgayTiepNhan").val(curr_date + "/" + curr_month + "/" + curr_year);
        $("#NgayTraLoiKN").val(curr_date + "/" + curr_month + "/" + curr_year);
        AutocompleteNguoiTiepNhan();
        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();
        //        window.onresize = function (event) {
        //            fnSetSizeDiv();
        //        }
        fnLocKhieuNai();

    });

    function pageselectCallback(page_index) {
        var curentPages = page_index + 1;
        var select = $("#DropSelect").val();
        if (select == null) {
            select = -1;
        }
        var typeSearch = $("#DropKhieuNai").val();
        if (typeSearch == null) {
            typeSearch = -1;
        }
        var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
        if (SoThueBao == null || SoThueBao == '') {
            SoThueBao = -1;
        }
        var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();
        var ThoiGianTiepNhanTu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
        var ThoiGianTiepNhanDen = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=32'
                + '&select=' + select
                + '&typeSearch=' + typeSearch
                + '&SoThueBao=' + SoThueBao
                + '&NguoiTiepNhan=' + NguoiTiepNhan
                + '&ThoiGianTiepNhanTu=' + ThoiGianTiepNhanTu
                + '&ThoiGianTiepNhanDen=' + ThoiGianTiepNhanDen
                + '&pageSize=' + pageSize
                + '&startPageIndex=' + curentPages, '', function (result) {
                    if (result != '') {
                        $('#grid-data').html(result);
                    }
                });
        return false;
    }
    function getOptionsFromForm() {
        var opt = { callback: pageselectCallback };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function fnSetSizeDiv() {
        var d = screen.width - 56;
        var h = screen.height;
        $("#divScroll").css("width", d);
        $(".divOpacity").css("height", h);
    }

    function CheckDateFromTo(startDt, endDt) {
        if (startDt != '' && endDt != '') {
            var i = startDt.split('/');
            var j = endDt.split('/');
            var ngaybatdau = i[1] + "/" + i[0] + "/" + i[2];
            var ngayketthuc = j[1] + "/" + j[0] + "/" + j[2];
            if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
                MessageAlert.AlertNormal("Không được nhỏ hơn từ ngày");
                return false;
            }
        } else {
            if (startDt == '' && endDt != '') {
                MessageAlert.AlertNormal("Vui lòng chọn ngày bắt đầu");
                return false;
            } else if (startDt != '' && endDt == '') {
                MessageAlert.AlertNormal("Vui lòng chọn ngày kết thúc");
                return false;
            }
        }
        return true;
    }
    function fnLocKhieuNai() {

        var ThoiGianTiepNhanTu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
        var ThoiGianTiepNhanDen = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();
        if (CheckDateFromTo(ThoiGianTiepNhanTu, ThoiGianTiepNhanDen)) {
            var optInit = getOptionsFromForm();
            var select = $("#DropSelect").val();
            if (select == null) {
                select = -1;
            }
            var typeSearch = $("#DropKhieuNai").val();
            if (typeSearch == null) {
                typeSearch = -1;
            }
            var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
            if (SoThueBao == null || SoThueBao == '') {
                SoThueBao = -1;
            }
            var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();

            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=31'
                + '&select=' + select
                + '&typeSearch=' + typeSearch
                + '&SoThueBao=' + SoThueBao
                + '&NguoiTiepNhan=' + NguoiTiepNhan
                + '&ThoiGianTiepNhanTu=' + ThoiGianTiepNhanTu
                + '&ThoiGianTiepNhanDen=' + ThoiGianTiepNhanDen

                + '&pageSize=' + pageSize
                + '&startPageIndex=1', '',
                function (totalRecords) {
                    if (totalRecords != '') {
                        ////GetTitle(totalRecords);
                        if (totalRecords == 0) {
                            $("#Pagination").pagination(0, optInit);
                        }
                        else {
                            $("#Pagination").pagination(totalRecords, optInit);
                        }
                        //$("#Pagination").pagination(totalRecords, optInit);
                        $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                    }

                });
        }
    }

    function fnLoadDropLoaiKhieuNai() {
        //$('#DropLinhVucChung').hide();
        //$('#DropLinhVucCon').hide();
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=37', '', function (result) {
            if (result != '') {
                $('#DropLoaiKhieuNai').html(result);
            }

        });
    }

    function fnLoadDropLoaiKhieuNai(LoaiKhieuNaiId, LinhVucChungId, LinhVucConId) {

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=37', '', function (result) {
            if (result != '') {
                $('#DropLoaiKhieuNai').html(result);
                $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=26&loaiKhieuNaiId=' + LoaiKhieuNaiId, '', function (result) {
                    if (result != '') {
                        $('#DropLinhVucChung').html(result);
                        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=27&linhVucChungId=' + LinhVucChungId, '', function (result) {
                            if (result != '') {
                                $('#DropLinhVucCon').html(result);
                                $('#DropLoaiKhieuNai').val(LoaiKhieuNaiId);
                                $('#DropLinhVucChung').val(LinhVucChungId);
                                $('#DropLinhVucCon').val(LinhVucConId);

                            }

                        });
                    }

                });
                //fnLoadLinhVucChung(LoaiKhieuNaiId);
                //fnLoadLinhVucCon(LinhVucChungId);
            }

        });
    }
    function fnDropSelectChange() {
        fnLocKhieuNai();
    }
    function fnDropLoaiKhieuNaiChange() {
        var loaiKhieuNaiId = document.getElementById('DropLoaiKhieuNai').value;
        //$('#DropLinhVucChung').show();
        //$('#DropLinhVucCon').hide();
        fnLoadLinhVucChung(loaiKhieuNaiId);
    }
    function fnLoadLinhVucChung(loaiKhieuNaiId) {
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=262&loaiKhieuNaiId=' + loaiKhieuNaiId, '', function (result) {
            if (result != '') {
                $('#DropLinhVucChung').html(result);
            }

        });
    }
    function fnLoadLinhVucCon(linhVucChungId) {

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=272&linhVucChungId=' + linhVucChungId, '', function (result) {
            if (result != '') {
                $('#DropLinhVucCon').html(result);
            }

        });
    }
    function fnDropLinhVucChungChange() {
        //$('#DropLinhVucChung').show();
        //$('#DropLinhVucCon').show();
        var linhVucChungId = document.getElementById('DropLinhVucChung').value;
        fnLoadLinhVucCon(linhVucChungId);
    }

    function fnDropPageSizeChange() {
        pageSize = $('#DropPageSize').val();
        fnLocKhieuNai();
    }
    function fnClearFilter() {

        $("#<%= txtSoThueBao.ClientID %>").val('');
        $("#<%=txtNguoiTiepNhan.ClientID %>").val('');
        $('#DropSelect').val('-1');
        $('#DropKhieuNai').val('-1');
        var now = new Date();
        var dateFrom = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 15);

        var day_from = dateFrom.getDate();
        var month_from = dateFrom.getMonth() + 1;
        var year_from = dateFrom.getFullYear();
        $("#<%=txtNgayTiepNhan_tu.ClientID %>").val(day_from + "/" + month_from + "/" + year_from);

        var curr_date = now.getDate();
        var curr_month = now.getMonth() + 1;
        var curr_year = now.getFullYear();
        $("#<%=txtNgayTiepNhan_den.ClientID %>").val(curr_date + "/" + curr_month + "/" + curr_year);
        fnLocKhieuNai();

    }
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
    function ShowPoup(id) {

        $('#divPoup').show();
        $('.divOpacity').css('display', 'block');
        $('#createFileUpload').html('');
        if (id != 0) {
            $('#divTitle').html('Chỉnh sửa nội dung sổ theo dõi');
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=5&id=' + id + '&view=1', '', function (result) {
                if (result != '') {
                    //var obj = eval("(" + result + ")");
                    var i = result.NgayTiepNhan.split(' ')[0].split('/');
                    var j = result.NgayTraLoiKN.split(' ')[0].split('/');
                    var ngayTiepNhan = i[0] + "/" + i[1] + "/" + i[2];
                    var ngayTraLoi = j[0] + "/" + j[1] + "/" + j[2];
                    fnLoadDropLoaiKhieuNai(result.LoaiKhieuNaiId, result.LinhVucChungId, result.LinhVucConId);
                    $("#HoTenLienHe").val(result.HoTenLienHe);
                    $("#DiaChiLienHe").val(result.DiaChiLienHe);
                    $("#NoiDungPA").val(result.NoiDungPA);
                    $("#NoiDungXuLy").val(result.NoiDungXuLy);
                    $("#NgayTiepNhan").val(ngayTiepNhan);
                    $("#NgayTraLoiKN").val(ngayTraLoi);
                    $("#KetQuaXuLy").val(result.KetQuaXuLy);
                    $("#GhiChu").val(result.GhiChu);
                    $("#SoThueBao").val(result.SoThueBao);
                    $("#divListFile").html(result.FileDinhKemKH);

                    $('#DropLinhVucChung').show(result.SoThueBao);
                    $('#DropLinhVucCon').show();

                }

            });
        } else {
            fnLoadDropLoaiKhieuNai();
            $("#HoTenLienHe").val("");
            $("#DiaChiLienHe").val("");
            $("#NoiDungPA").val("");
            $("#NoiDungXuLy").val("");
            //$("#NgayTiepNhan").val("");
            //$("#NgayTraLoiKN").val("");
            $("#KetQuaXuLy").val("");
            $("#GhiChu").val("");
            $("#SoThueBao").val("");
            $("#divListFile").html("");
            //$('#DropLinhVucChung').hide();
            //$('#DropLinhVucCon').hide();
            $('#divTitle').html('Thêm mới nội dung sổ theo dõi');
            $('#message-error').html('');
        }
    }
    function ClearContent() {
        fnLoadDropLoaiKhieuNai();
        $("#HoTenLienHe").val("");
        $("#DiaChiLienHe").val("");
        $("#NoiDungPA").val("");
        $("#NoiDungXuLy").val("");
        //$("#NgayTiepNhan").val("");
        //$("#NgayTraLoiKN").val("");
        $("#KetQuaXuLy").val("");
        $("#GhiChu").val("");
        $("#SoThueBao").val("");
        $("#divListFile").html("");
        //$('#DropLinhVucChung').hide();
        //$('#DropLinhVucCon').hide();
        $('#message-error').html('');
    }
    var maKhieuNai = 0;
    function TaoMoiKhieuNai(id) {
        if (id != 0) {
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=41', '', function (result) {
                if (result != '') {
                    if (result == '0') {
                        MessageAlert.AlertNormal('Bạn không đủ quyền hạn thực hiện chức năng cập nhật!');
                    } else {
                        maKhieuNai = id;
                        ShowPoup(id);
                    }
                }

            });

        } else {
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=40', '', function (result) {
                if (result != '') {
                    if (result == '0') {
                        MessageAlert.AlertNormal('Bạn không đủ quyền hạn thực hiện chức năng thêm mới!');
                    } else {
                        maKhieuNai = id;
                        ShowPoup(id);
                    }
                }

            });
        }

    }

    function AutocompleteNguoiTiepNhan() {
        $("#<%=txtNguoiTiepNhan.ClientID %>").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    return {
                        data: row,
                        value: index.toString(),
                        result: row.TenTruyCap
                    };
                });
            },
            formatItem: function (item) {
                return format(item);
            }
        }).result(function (e, item) {
            $("#<%=txtNguoiTiepNhan.ClientID %>").val(item.TenTruyCap);
        });

        $("#<%=txtNguoiTiepNhan.ClientID %>").focus();
    }
    function format(item) {
        return "<span class='ac_keyword'>" + item.TenTruyCap + "</span>";
    }
    function fnCheckInput() {

        var sHoTenLienHe = $("#HoTenLienHe").val();
        var sDiaChiLienHe = $("#DiaChiLienHe").val();
        var sNoiDungPA = $("#NoiDungPA").val();

        var sNgayTiepNhan = $("#NgayTiepNhan").val();
        var sNgayTraLoiKN = $("#NgayTraLoiKN").val();
        var i = sNgayTiepNhan.split('/');
        var j = sNgayTraLoiKN.split('/');
        var ngaybatdau = i[1] + "/" + i[0] + "/" + i[2];
        var ngayketthuc = j[1] + "/" + j[0] + "/" + j[2];
        var regexVNP = new RegExp("^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7}))$");
        var sDauSo = $("#DauSo").val();
        var sSoThueBao = $("#SoThueBao").val();
        var loaiKhieuNai = $("#DropLoaiKhieuNai").val();
        var linhVucChung = $("#DropLinhVucChung").val();

        if (sSoThueBao == '') {
            $('#message-error').html('Lỗi nhập liệu-Số thuê bao. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;
        } else if (!regexVNP.test(sDauSo + sSoThueBao)) {
            $('#message-error').html('Không đúng số điện thoại của Vinaphone !');
            $('#message-error').css("display", "block");
            return false;

        } else if (sHoTenLienHe == '') {
            $('#message-error').html('Lỗi nhập liệu-Tên khách hàng. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;

        } else if (sDiaChiLienHe == '') {
            $('#message-error').html('Lỗi nhập liệu-Địa chỉ liên hệ. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;

        } else if (loaiKhieuNai == null || loaiKhieuNai == '-1') {
            $('#message-error').html('Lỗi nhập liệu-Chọn loại khiếu nại. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;
        } else if (sNoiDungPA == '') {
            $('#message-error').html('Lỗi nhập liệu-Nội dung khiếu nại. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;

        } else if (sNgayTiepNhan == '') {
            $('#message-error').html('Lỗi nhập liệu-Ngày tiếp nhận. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;

        } else if (sNgayTraLoiKN == '') {
            $('#message-error').html('Lỗi nhập liệu-Ngày trả lời. Lưu ý: Các trường (*) là bắt buộc nhập !');
            $('#message-error').css("display", "block");
            return false;
        } else if (loaiKhieuNai == null || loaiKhieuNai == '-1') {
            return false;
        } else if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
            $('#message-error').css("display", "block");
            $('#message-error').html("Ngày tiếp nhận phải nhỏ hơn hay bằng ngày trả lời khiếu nại. ");
            return false;
        }
        return true;
    }
    function fnUpdateSoTheoDoi() {

        var sHoTenLienHe = $("#HoTenLienHe").val();
        var sDiaChiLienHe = $("#DiaChiLienHe").val();
        var sNoiDungPA = $("#NoiDungPA").val();
        var sNoiDungXuLy = $("#NoiDungXuLy").val();
        var sNgayTiepNhan = $("#NgayTiepNhan").val();
        var sNgayTraLoiKN = $("#NgayTraLoiKN").val();
        var sKetQuaXuLy = $("#KetQuaXuLy").val();
        var sGhiChu = $("#GhiChu").val();
        var sSoThueBao = $("#SoThueBao").val();

        var loaiKhieuNai = $("#DropLoaiKhieuNai").val() + "#" + $("#DropLoaiKhieuNai :selected").text();

        var linhVucChung = $("#DropLinhVucChung").val();
        if (linhVucChung == null || linhVucChung == '-1') {
            linhVucChung = -1;
        } else {
            linhVucChung = $("#DropLinhVucChung").val() + "#" + $("#DropLinhVucChung :selected").text();
        }
        var linhVucCon = $("#DropLinhVucCon").val();
        if (linhVucCon == null || linhVucCon == '-1') {
            linhVucCon = -1;
        } else {
            linhVucCon = $("#DropLinhVucCon").val() + "#" + $("#DropLinhVucCon :selected").text();
        }

        if (fnCheckInput()) {
            $('#message-error').css("display", "none");
            $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=36&maKhieuNai=' + maKhieuNai,
                {
                    SoThueBao: sSoThueBao,
                    HoTenLienHe: sHoTenLienHe,
                    DiaChiLienHe: sDiaChiLienHe,
                    NoiDungPA: sNoiDungPA,
                    NoiDungXuLy: sNoiDungXuLy,
                    NgayTiepNhan: sNgayTiepNhan,
                    NgayTraLoiKN: sNgayTraLoiKN,
                    KetQuaXuLy: sKetQuaXuLy,
                    GhiChu: sGhiChu,
                    LoaiKhieuNai: loaiKhieuNai,
                    LinhVucChung: linhVucChung,
                    LinhVucCon: linhVucCon

                },
                function (result) {
                    fnLocKhieuNai();
                    ClearContent();
                    if (result == '0') {
                        MessageAlert.AlertNormal('Lưu không thành công ! Vui lòng kiểm tra lại');
                    } else if (result == '-1') {
                        MessageAlert.AlertNormal('Lỗi hệ thống!');
                    }
                    else if (result == '-2') {
                        MessageAlert.AlertNormal('Bạn không đủ quyền hạn thực hiện chức năng này!');
                    }
                    else {
                        if (maKhieuNai == 0) {
                            SetValues(result);

                        } else {
                            SetValues(maKhieuNai);
                        }
                    }

                });
        }

    }
    function TraCuuThongTin() {
        var sSoThueBao = $("#SoThueBao").val();
        if (sSoThueBao == '') {
            $('#message-error').html('Vui lòng nhập số thuê bao !');
            $('#message-error').css("display", "block");
        } else {
            $('#message-error').html('');
            $('#message-error').css("display", "none");
            var sDauSo = $("#DauSo").val();
            var sSoThueBao = $("#SoThueBao").val();
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=38&dauSo=' + sDauSo + '&soThueBao=' + sSoThueBao, '', function (result) {
                if (result != '') {
                    var mySplitResult = result.split("#");
                    if (mySplitResult.length > 0) {
                        $("#HoTenLienHe").val(mySplitResult[0]);
                        $("#DiaChiLienHe").val(mySplitResult[1]);
                    }
                }

            });
        }
    }
    function fnDropKhieuNaiChange() {
        fnLocKhieuNai();
    }
    function fnExportExcel() {
        $('.divOpacity').css('display', 'block');
        var pageSizeExp = $("#DropPageSize").val();
        var select = $("#DropSelect").val();
        if (select == null) {
            select = -1;
        }

        var selectKhieuNai = $("#DropKhieuNai").val();
        if (selectKhieuNai == null) {
            selectKhieuNai = -1;
        }

        var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
        if (SoThueBao == null || SoThueBao == '') {
            SoThueBao = -1;
        }
        var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();
        var ThoiGianTiepNhanTu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
        var ThoiGianTiepNhanDen = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();
        var PhongBanXuLy = -1;
        $.getJSON('/Views/QLKhieuNai/Handler/ExportExcel.ashx?key=7'
                + '&select=' + select
                + '&typeSearch=' + selectKhieuNai
                + '&SoThueBao=' + SoThueBao
                + '&NguoiTiepNhan=' + NguoiTiepNhan
                + '&PhongBanXuLy=' + PhongBanXuLy
                + '&ThoiGianTiepNhanTu=' + ThoiGianTiepNhanTu
                + '&ThoiGianTiepNhanDen=' + ThoiGianTiepNhanDen
                + '&pageSize=' + pageSizeExp
                + '&startPageIndex=' + 1, '',
                function (result) {
                    if (result != '') {
                        if (result == '-1') {
                            $('.divOpacity').css('display', 'none');
                            MessageAlert.AlertNormal('Dữ liệu quá lớn (< Max 60,000) ! Vui lòng kiểm tra lại');
                        } else {
                            window.location.href = "/ExportExcel/Excel" + result;
                            $('.divOpacity').css('display', 'none');
                        }
                    } else {
                        $('.divOpacity').css('display', 'none');
                        MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại', 'error');
                    }

                });
    }

    function ClosePoup() {
        ClearContent();
        $('.divOpacity').css('display', 'none');
        $('#divPoup').hide();

    }
</script>
<%--<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />--%>
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="float: left;"><a href="javascript:history.back()">
            <input type="button" class="button_eole back" value="Quay về" /></a></li>
        <li id="btnDongKN"><a href="javascript:TaoMoiKhieuNai(0);"><span class="new" title="Thêm PAKN vào sổ theo dõi">
            Thêm PAKN</span></a> </li>
        <li id="btnExportExcel" style="float: right;"><a href="javascript:fnExportExcel();">
            <span class="ex_excel">Xuất Excel</span></a> </li>
    </ul>
    <div class="div-clear">
    </div>
</div>
<div class="p8">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr valign="top">
                <td style="height: 5px">
                </td>
            </tr>
            <tr>
                <td>
                    <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                        <tbody>
                            <tr>
                                <td bgcolor="#f0f0f0" style="text-align: left">
                                    <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;">
                                        Lọc sổ theo dõi</h3>
                                </td>
                            </tr>
                            <tr style="background: #fffff0">
                                <td>
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <select id="DropSelect" onchange="javascript:fnDropSelectChange();" style="width: 130px;">
                                                                <option value="-1">--Tất cả--</option>
                                                                <option value="0">Khiếu nại chuyển xử lý</option>
                                                                <option value="1">Khiếu nại lưu sổ theo dõi</option>
                                                            </select>
                                                            <select id="DropKhieuNai" onchange="javascript:fnDropKhieuNaiChange();" style="width: 100px;">
                                                                <%=strFilterKhieuNai %>
                                                            </select>
                                                            <asp:TextBox ID="txtSoThueBao" Width="150px" CssClass="typeNumber" MaxLength="11"
                                                                placeHolder="Số thuê bao..." runat="server"></asp:TextBox>
                                                            <asp:TextBox ID="txtNguoiTiepNhan" Width="150px" MaxLength="50" placeHolder="Người tiếp nhận..."
                                                                runat="server"></asp:TextBox>
                                                            <asp:TextBox ID="txtNgayTiepNhan_tu" runat="server" placeHolder="Ngày tiếp nhận từ..."
                                                                CssClass="typeDate" Text="" Width="80" MaxLength="10"></asp:TextBox>
                                                            <asp:TextBox ID="txtNgayTiepNhan_den" placeHolder="Ngày tiếp nhận đến..." runat="server"
                                                                Text="" Width="80" MaxLength="10"></asp:TextBox>
                                                            <a style="height: 20px; padding-top: 5px;position:absolute;margin-left:5px;margin-top:7px;" onclick="Javascript:fnLocKhieuNai();"
                                                                class="btn_style_button">Lọc</a>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left; width: 50px;">
                                                </td>
                                                <td width="15%">
                                                    <a style="float: right;" onclick="Javascript:fnClearFilter();" class="button_clear_filter">
                                                        Xóa điều kiện lọc</a>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <%--<div id="divNote" style="width: 350px; float: left; margin-top: 5px;">
                        <p style="border: 1pt solid #CCC; background:#ccc; width: 22px; height: 13px;float:left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;">Chờ xử lý</span>    
                        
                          <p style="border: 1pt solid #CCC; background:#ccc; width: 22px; height: 13px;float:left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;">Đang xử lý</span>   
                         
                          <p style="border: 1pt solid #CCC; background:#ccc; width: 22px; height: 13px;float:left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;">Đóng khiếu nại</span>    
                                            
                    </div>--%>
                    <div id="Pagination" class="pagination" style="float: right; margin-right: 0px;">
                    </div>
                    <div id="PageSize" class="pagination" style="float: right;">
                        <div class="selectstyle">
                            <div class="bg" style="margin: -7px; margin-right: 10px; margin-left: 10px;">
                                <select id="DropPageSize" onchange="javascript:fnDropPageSizeChange();" style="width: 60px;">
                                    <option value="10" selected="selected">10</option>
                                    <option value="20">20</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div id="divTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;">
                    </div>
                    <ListKNSoTheoDoi:ListKNSoTheoDoi ID="ListKNSoTheoDoi2" runat="server" />
                    <div class="div-clear" style="height: 10px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="divPoup" style="height: auto; background: #fff; margin: 0 auto; width: 900px;
    left: 5%; right: 5%; z-index: 200; position: fixed; top: 1%; border: 1px solid #4D709A;
    border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px;
        height: 25px;">
        <h3 id="divTitle" style="float: left; color: #fff; font-weight: bold;">
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="divContent" style="margin: 5px;">
        <SoTheoDoiEdit:SoTheoDoiEdit ID="SoTheoDoiEdit1" runat="server" />
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div id="divOpacity" class="divOpacity" style="opacity: 0.4; background: #000; width: 100%;
    position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
</div>
