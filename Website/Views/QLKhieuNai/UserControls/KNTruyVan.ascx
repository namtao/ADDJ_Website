<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNTruyVan.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.KNTruyVan" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ListKNNoSelect.ascx" TagName="ListKNNoSelect" TagPrefix="ListKNNoSelect" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/LichSuTruyVan.ascx" TagName="LichSuTruyVan" TagPrefix="LichSuTruyVan" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ChiTietKN.ascx" TagName="ChiTietKN" TagPrefix="ChiTietKN" %>

<script src="/Js/jquery.pagination.js" type="text/javascript"></script>
<link href="/Css/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/Js/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/Js/plugin/jquery.datepick-vi.js" type="text/javascript"></script>

<style type="text/css">
    .content-truyvan { width: 800px; margin: 0 auto; text-align: left; position: relative; }
    #tagChuThich { position: absolute; bottom: -120px; right: 0px; z-index: 99900; height: 100px; background: #D1E8FA; padding: 10px; border: 1px solid #FFFFFF; border-radius: 3px; display: none; box-shadow: 0 0 2px #AAAAAA; }
    #tagItemSelectMulti { position: absolute; top: 70px; right: 108px; z-index: 9000; background: #D1E8FA; padding: 10px; border: 1px solid #FFFFFF; border-radius: 3px; box-shadow: 0 0 2px #AAAAAA; display: none; width: 250px; height: 50px; }
    #tagItemSelectMulti ul.listitem { padding: 0px; margin: 0px; list-style-type: none; position: absolute; margin-left: -35px; }
    #tagItemSelectMulti ul.listitem li.item { white-space: nowrap; list-style-type: none; line-height: 25px; padding-left: 35px; }
    ul.listitem .clickHinde, ul.listitem .ClickShowHinde { padding-right: 5px; }
    #divPoupTitle { z-index: 200; height: 100px; background: #fff; padding: 10px; border: 1px solid #FFFFFF; border-radius: 3px; display: none; box-shadow: 0 0 2px #AAAAAA; width: auto; margin: 0 auto; position: fixed; top: 30%; left: 30%; right: 30%; display: none; height: 120px; }
    #chuthich { margin-top: 10px; margin-left: 10px; }
    .SelectDropdown-Firefox { left: 255px; position: absolute; top: 19px; }
    .SelectDropdown-Chrome { right: 30px; position: absolute; top: -4px; }
</style>
<script type="text/javascript">
    var pageSize = '';
    $(document).ready(function () {
        $('#form1').click(function (e) {
            var tagId = e.target.id;
            if (tagId.indexOf('checkbox') == -1) {
                if ($('#tagItemSelectMulti').css('display') == 'block') {
                    $("#tagItemSelectMulti").css("display", "none");
                }
            }
        });
        $('#ul-SelectMulti').click(function (e) {
            var names = [];
            var pheptoan = $("#DropPhepToan").val();
            if (pheptoan == 'IN' || pheptoan == 'NOT IN') {
                $('#ul-SelectMulti input:checked').each(function () {
                    if (!this.disabled) {
                        names.push(this.value + '-' + this.name);
                    }
                });
                $("#giatri").val(names);
            } else {
                if (pheptoan == '=') {
                    var giatri = $("#giatri").val();
                    if (giatri.length > 0) {
                        var temp = 0;
                        $('#ul-SelectMulti input:checked').each(function () {
                            temp++;
                            if (!this.disabled) {
                                if (giatri != this.value + '-' + this.name) {
                                    this.checked = false;
                                }

                            }
                        });
                        if (temp == 0) {
                            $("#giatri").val('');
                        } else {
                            MessageAlert.AlertNormal('Phép toán (=) chỉ được phép nhập 1 giá trị !', 'error');
                        }
                    } else {
                        $('#ul-SelectMulti input:checked').each(function () {
                            if (!this.disabled) {
                                names.push(this.value + '-' + this.name);
                                var $input = $(this);
                                var tinhthanhid = $input.attr("tinhthanhid");
                                $("#hidtinhthanhid").val(tinhthanhid);
                                var loaikhieunai0id = $input.attr("loaikhieunai0id");
                                $("#hidloaikhieunai0id").val(loaikhieunai0id);
                                var loaikhieunai1id = $input.attr("loaikhieunai1id");
                                $("#hidloaikhieunai1id").val(loaikhieunai1id);
                                var loaikhieunai2id = $input.attr("loaikhieunai2id");
                                $("#hidloaikhieunai2id").val(loaikhieunai2id);
                            }
                        });
                        $("#giatri").val(names);
                    }
                }
            }


        });
        fnLoadTreeCheckBox();
        pageSize = $('#DropPageSize').val();
        fnLoadDropParam();
        fnSetSizeDiv();

        var id = fnGetUrlParameter('id');
        if (id != '') {
            SelectRow(id);
        }

        pageselectCallback(0);

    });
    function SetStyleBrowserName() {
        var Browser = navigator.userAgent;
        if (Browser.indexOf('MSIE') >= 0) {

        } else if (Browser.indexOf('Firefox') >= 0) {
            $("#SelectDropdown").addClass("SelectDropdown-Firefox");
        } else if (Browser.indexOf('Chrome') >= 0) {
            $("#SelectDropdown").addClass("SelectDropdown-Chrome");
        } else if (Browser.indexOf('Safari') >= 0) {

        } else if (Browser.indexOf('Opera') >= 0) {

        } else {
            Browser = 'UNKNOWN';
        }

        return Browser;

    }

    function ClickShowHinde(tag) {
        if ($("#" + tag).css('display') == 'block') {
            var html = "<img id = \"img-checkbox" + tag + "\" src=\"/images/icons/Next.png\" />";
            $("#a-checkbox" + tag).html(html);
            $("#" + tag).css("display", "none");

        } else {
            var html = "<img id = \"img-checkbox" + tag + "\" src=\"/images/icons/Down.png\" />";
            $("#a-checkbox" + tag).html(html);
            $("#" + tag).css("display", "block");

        }
    }
    function ViewChuthich() {
        $("#tagChuThich").css("display", "block");
    }
    function fnAddScrollDiv() {
        var h1 = $('#divPoupChiTiet').innerHeight() - 40;
        var h2 = $('#divContent').innerHeight();
        if (h2 > h1) {
            $("#divContent").css("overflow-y", "scroll");
            $("#divContent").css("height", h1 - 20);
        } else {
            $("#divContent").removeAttr("style");
            $("#divContent").attr("style", "margin: 5px;");
        }


    }

    function ClosePoup() {
        $("#tagChuThich").css("display", "none");
    }
    function ClosePoupLichSu() {

        $('.divOpacity').css('display', 'none');
        $('#divPoup').hide();

    }
    function ShowPoupLichSu() {
        $('#divPoup').show();
        $('.divOpacity').css('display', 'block');
    }
    function ClosePoupTitle() {
        $("#NameLichSuTruyVan").val('');
        $("#divPoupTitle").css("display", "none");
        $('.divOpacity').css('display', 'none');
    }
    function ShowPoupTitle() {
        if (dataTruyVan.items.length > 0) {
            $('#divPoupTitle').show();
            $('.divOpacity').css('display', 'block');
        } else {
            MessageAlert.AlertNormal("Chưa có tiêu trí tìm kiếm");
        }
    }
    var data = [
                { "Name": "Id", "Type": "int", "Title": "Mã phản ánh" },
                { "Name": "NoiDungPA", "Type": "text", "Title": "Nội dung phản ánh" },
                { "Name": "PhongBanTiepNhanId", "Type": "int", "Title": "Phòng ban tiếp nhận" },
                { "Name": "PhongBanXuLyId", "Type": "int", "Title": "Phòng ban xử lý" },
                // { "Name": "KhuVucId", "Type": "int", "Title": "Khu vực tiếp nhận" },
                { "Name": "DoiTacId", "Type": "int", "Title": "Đối tác tiếp nhận" },
                { "Name": "DoiTacXuLyId", "Type": "int", "Title": "Đối tác xử lý" },
                //    { "Name": "KhuVucXuLyId", "Type": "int", "Title": "Khu vực xử lý" },
                { "Name": "LoaiKhieuNaiId", "Type": "int", "Title": "Loại khiếu nại" },
                { "Name": "LinhVucChungId", "Type": "int", "Title": "Lĩnh vực chung" },
                { "Name": "LinhVucConId", "Type": "int", "Title": "Lĩnh vực con" },
                { "Name": "DoUuTien", "Type": "int", "Title": "Độ ưu tiên" },
                { "Name": "TrangThai", "Type": "int", "Title": "Trạng thái" },
                { "Name": "HTTiepNhan", "Type": "int", "Title": "Hình thức tiếp nhận" },
                { "Name": "SoThueBao", "Type": "int", "Title": "Số thuê bao" },
                { "Name": "MaTinhId", "Type": "int", "Title": "Tỉnh thành" },
                { "Name": "MaQuanId", "Type": "int", "TinhThanhId": "TinhThanhId", "Title": "Quận huyện" },

                { "Name": "NguoiTiepNhan", "Type": "string", "Title": "Người tiếp nhận" },
                { "Name": "NguoiXuLy", "Type": "string", "Title": "Người xử lý" },
                //  { "Name": "HTTiepNhan", "Type": "int", "Title": "Ten chu thich" },
                { "Name": "NgayTiepNhan", "Type": "date", "Title": "Ngày tiếp nhận" },
                // { "Name": "NgayTiepNhanSort", "Type": "int", "Title": "Ten chu thich" },

                { "Name": "NgayQuaHan", "Type": "date", "Title": "Ngày quá hạn" },
                // { "Name": "NgayQuaHanSort", "Type": "int", "Title": "Ten chu thich" },
                { "Name": "NgayCanhBao", "Type": "date", "Title": "Ngày cảnh báo" },
                // { "Name": "NgayCanhBaoSort", "Type": "int", "Title": "Ten chu thich" },
                // { "Name": "NgayChuyenPhongBan", "Type": "date", "Title": "Ten chu thich" },
                // { "Name": "NgayCanhBaoPhongBanXuLy", "Type": "date", "Title": "Ten chu thich" },
                { "Name": "NgayQuaHanPhongBanXuLy", "Type": "date", "Title": "Ngày quá hạn phòng ban xử lý" },
                { "Name": "NgayTraLoiKN", "Type": "date", "Title": "Ngày trả lời khiếu nại" },
                // { "Name": "NgayTraLoiKNSort", "Type": "int", "Title": "Ten chu thich" },
                { "Name": "NgayDongKN", "Type": "date", "Title": "Ngày đóng khiếu nại" },
                { "Name": "GhiChu", "Type": "string", "Title": "Ghi chú" },
                { "Name": "SDTLienHe", "Type": "string", "Title": "Điện thoại liên hệ" },
                { "Name": "HoTenLienHe", "Type": "string", "Title": "Họ tên liên hệ" },
                { "Name": "DoHaiLong", "Type": "int", "Title": "Độ hài lòng" },


    ];

    function fnLoadTreeCheckBox() {
        $.extend($.expr[':'], {
            unchecked: function (obj) {
                return ((obj.type == 'checkbox' || obj.type == 'radio') && !$(obj).is(':checked'));
            }
        });

        $("#tagItemSelectMulti input:checkbox").live('change', function () {
            $(this).next('ul').find('input:checkbox').prop('checked', $(this).prop("checked"));

            for (var i = $('#tagItemSelectMulti').find('ul').length - 1; i >= 0; i--) {
                $('#tagItemSelectMulti').find('ul:eq(' + i + ')').prev('input:checkbox').prop('checked', function () {
                    return $(this).next('ul').find('input:unchecked').length === 0 ? true : false;
                });
            }
        });

    }

    function fnSetSizeDiv() {
        var d = $('body').innerWidth() - 56;
        var h = screen.height;
        $("#divScroll").css("width", d);
        //        $(".divOpacity").css("height", h);
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
    function fnGetUrlParameter(sParam) {
        var values = '';
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                values = sParameterName[1];
            }
        }

        return values;
    }
    function fnLoadDropParam() {
        var html = '';
        html += '<option value="-1">--Tham số tìm kiếm--</option>';
        $.each(data, function (i, item) {
            html += '<option value="' + item.Type + "#" + item.Name + "#" + item.TinhThanhId + '">' + item.Title + '</option>';
        });
        $("#DropParam").html(html);


    }

    function fnDropParamChange() {
        $('#tagAdd').html('');
        var arr = $("#DropParam").val().split("#");
        var type = arr[0];
        fnLoadDropPhepToan(type);
    }
    function fnDropPhepToanChange() {

        var arr = $("#DropParam").val().split("#");
        var type = arr[0];
        var column = arr[1];
        var pheptoan = $("#DropPhepToan").val();
        fnLoadValuePhepToan(pheptoan, type, column);
    }
    function fnLoadValuePhepToan(pheptoan, type, column) {
        $('#tagAdd').html('');
        var tagInput = '';
        if (type != '-1' && pheptoan != '-1') {
            if (pheptoan == '=' || pheptoan == '>=' || pheptoan == '<=' || pheptoan == 'IN' || pheptoan == 'NOT IN'
        || pheptoan == 'LIKE' || pheptoan == 'AND' || pheptoan == 'OR') {
                if (type == 'int' || type == 'string' || type == 'text') {
                    $("#tagAdd").append(LoadContentValue(type));
                    tagInput = 'giatri';
                    SetStyleBrowserName();
                } else if (type == 'date') {
                    $("#tagAdd").append(LoadContentValueDate());
                    $("#giatriDate").datepick({ dateFormat: 'dd/mm/yyyy' });

                }
            } else {
                if (pheptoan == 'BETWEEN') {
                    if (type == 'int' || type == 'string') {
                        $("#tagAdd").append(LoadContentValueFromTo(type));
                    } else if (type == 'date') {
                        $("#tagAdd").append(LoadContentValueDateFromTo());
                        $("#giatriTuNgay").datepick({ dateFormat: 'dd/mm/yyyy' });
                        $("#giatriDenNgay").datepick({ dateFormat: 'dd/mm/yyyy' });
                    }

                }
            }
        }
        ChuThichDieuKien(pheptoan);
        if (pheptoan != 'LIKE') {
            Autocomplete(column, tagInput);
        }

    }
    function XoaDieuKienTruyVan() {
        fnLoadDropParam();
        $("#DropPhepToan").html('');
        $("#tagAdd").html('');
        dataTruyVan = { items: [] };
        fnBindDataTruyVan();
        $("#rowListData").css("display", "none");
    }
    function ChuThichDieuKien(pheptoan) {

        if (pheptoan == '=' || pheptoan == '>=' || pheptoan == '<=' || pheptoan == 'AND') {
            $('#tagAdd').append('<span id="chuthich" style=""><a href="javascript:ViewChuthich();"><img style="" src="/Images/17_Question-16.png" /></a></span>');
            $('#tagContentChuThich').html("Giá trị nhập = 1 chuỗi cố định không có ký tự đặc biệt");
        } else if (pheptoan == 'IN' || pheptoan == 'NOT IN' || pheptoan == 'OR') {
            $('#tagAdd').append('<span id="chuthich" style=""><a href="javascript:ViewChuthich();"><img src="/Images/17_Question-16.png" /></a></span>');
            $('#tagContentChuThich').html("Giá trị nhập = 1 chuỗi cố định,<br />nếu tìm kiếm trong danh sách giá trị thì các gia trị cách nhau bởi dấu (,) <br /> vi du( 1,2,3...) hoặc ( doitac1,ktv3)");
        } else if (pheptoan == 'BETWEEN') {
            $('#tagAdd').append('<span style=""><a href="javascript:ViewChuthich();"><img src="/Images/17_Question-16.png" /></a></span>');
            $('#tagContentChuThich').html("Giá trị nhập = 1 chuỗi cố định không có ký tự đặc biệt<br/> Ví dụ: (1 To 3) hoặc (12/09/2013 TO 15/09/2013)");
        } else if (pheptoan == 'LIKE') {
            $('#tagAdd').append('<span style=""><a href="javascript:ViewChuthich();"><img src="/Images/17_Question-16.png" /></a></span>');
            $('#tagContentChuThich').html("Giá trị nhập = 1 chuỗi không có ký tự đặc biệt<br/> Ví dụ: gprs,max100...");
        }

    }
    function fnLoadDropPhepToan(type) {
        var html = '<option value="-1">--Phép toán--</option>';
        if (type != '-1') {
            if (type == 'int') {
                html += '<option value="=">[ = ]</option>';
                html += '<option value="IN">[ IN ]</option>';
                html += '<option value="NOT IN">[ NOT IN ]</option>';
            } else if (type == 'string') {
                html += '<option value="=">[ = ]</option>';
                html += '<option value="AND">[ AND ]</option>';
                html += '<option value="OR">[ OR ]</option>';
                html += '<option value="IN">[ IN ]</option>';
                html += '<option value="NOT IN">[ NOT IN ]</option>';

            } else if (type == 'text') {
                html += '<option value="LIKE">[ LIKE ]</option>';
            } else if (type == 'date') {
                html += '<option value="=">[ = ]</option>';
                html += '<option value=">=">[ >= ]</option>';
                html += '<option value="<=">[ <= ]</option>';
                html += '<option value="BETWEEN">[ BETWEEN ]</option>';
            }
        }

        $("#DropPhepToan").html(html);
    }
    function fnSetSizeDiv() {
        var d = $('body').innerWidth() - 30;
        var h = screen.height;
        $("#divScroll").css("width", d);
        $(".divOpacity").css("height", h);

    }
    function LoadContentValue(type) {

        var html = '';
        if (type == 'string') {
            html = '<input id="giatri" type="text" placeholder="Nhập giá trị..." style="width: 270px !important;height: 26px !important;" />';
        } else if (type == 'text') {
            html = '<input id="giatri" type="text" placeholder="Nhập giá trị..." style="width: 270px !important;height: 26px !important;" />';
        } else if (type == 'int') {
            html = '<BR /><input class = "droplist" id="giatri" type="text" placeholder="Nhập giá trị..." style="width: 690px !important;height: 26px !important;" />';
            var arr = $("#DropParam").val().split("#");
            var tenTruong = arr[1];
            if (tenTruong != 'SoThueBao') {
                html += '<span id="SelectDropdown"><a href="javascript:DropdownSelectMulti();"><img src="/Images/icons/SelectDropdown.png" /></a></span>';
            }

        }
        return html;
    }
    function LoadContentValueDate() {
        var html = '<input id ="giatriDate" type ="text" placeHolder="Nhập giá trị..." style="width: 150px !important;height: 26px !important;" />';
        return html;
    }
    function LoadContentValueFromTo(type) {
        var htmlFrom = '';
        var htmlTo = '';
        if (type == 'string') {
            htmlFrom = '<input id ="giatriTu" type="text" placeholder="Nhập giá trị từ..." style="width: 130px !important;height: 26px !important;" />';
            htmlTo = '<input id ="giatriDen" type="text" placeholder="Nhập giá trị đến..." style="width: 130px !important;height: 26px !important;" />';
        } else {
            htmlFrom = '<input id ="giatriTu" class ="typeNumber" type="text" placeholder="Nhập giá trị từ..." style="width: 130px !important;height: 26px !important;" />';
            htmlTo = '<input id ="giatriDen" class ="typeNumber" type="text" placeholder="Nhập giá trị đến..." style="width: 130px !important;height: 26px !important;" />';
        }
        return htmlFrom + htmlTo;
    }
    function LoadContentValueDateFromTo() {
        var htmlFrom = '';
        var htmlTo = '';
        htmlFrom = '<input id ="giatriTuNgay" type ="text" placeHolder="Từ ngày..." style="width: 130px !important;height: 26px !important;" />';
        htmlTo = '<input id ="giatriDenNgay" type ="text" placeHolder="Đến ngày..." style="width: 130px !important;height: 26px !important;" />';
        return htmlFrom + htmlTo;
    }
    var dataTruyVan = { items: [] };
    function CreateTruyVan() {
        var tieuDe = $("#DropParam option:selected").text();
        //var kieuDuLieu = $("#DropParam").val();
        var arr = $("#DropParam").val().split("#");
        var kieuDuLieu = arr[0];
        var tenTruong = arr[1];
        var tinhThanhId = $("#hidtinhthanhid").val();
        var loaikhieunai0id = $("#hidloaikhieunai0id").val();
        var loaikhieunai1id = $("#hidloaikhieunai1id").val();
        var loaikhieunai2id = $("#hidloaikhieunai2id").val();
        var pheptoan = $("#DropPhepToan").val();
        var giaTri = '';
        if (kieuDuLieu != '-1' && pheptoan != '-1') {
            if (pheptoan == '=' || pheptoan == '>=' || pheptoan == '<=' || pheptoan == 'IN' || pheptoan == 'NOT IN'
        || pheptoan == 'LIKE' || pheptoan == 'AND' || pheptoan == 'OR') {
                if (kieuDuLieu == 'int' || kieuDuLieu == 'string' || kieuDuLieu == 'text') {
                    giaTri = $("#giatri").val();

                } else if (kieuDuLieu == 'date') {
                    giaTri = $("#giatriDate").val();
                }
            } else {
                if (pheptoan == 'BETWEEN') {
                    if (kieuDuLieu == 'int' || kieuDuLieu == 'string') {
                        if ($("#giatriTu").val() != '' && $("#giatriDen").val() !== '') {
                            if ($("#giatriDen").val() > $("#giatriTu").val()) {
                                giaTri = $("#giatriTu").val() + "#" + $("#giatriDen").val();
                            } else {
                                MessageAlert.AlertNormal("Giá trị Từ phải < giá trị đến!");
                            }
                        } else {
                            MessageAlert.AlertNormal("Không để trống 2 giá trị này!");
                        }
                    } else if (kieuDuLieu == 'date') {
                        if ($("#giatriTuNgay").val() != '' && $("#giatriDenNgay").val() !== '') {
                            var startDt = $("#giatriTuNgay").val();
                            var endDt = $("#giatriDenNgay").val();
                            var i = startDt.split('/');
                            var j = endDt.split('/');
                            var ngaybatdau = i[1] + "/" + i[0] + "/" + i[2];
                            var ngayketthuc = j[1] + "/" + j[0] + "/" + j[2];
                            if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
                                MessageAlert.AlertNormal("Giá trị từ ngày không được > giá trị đến ngày!");
                            } else {
                                giaTri = $("#giatriTuNgay").val() + "#" + $("#giatriDenNgay").val();
                            }
                        } else {
                            MessageAlert.AlertNormal("Không để trống 2 ngày này!");
                        }
                    }

                }
            }
        }
        if (kieuDuLieu == '-1') {
            MessageAlert.AlertNormal("Vui lòng chọn tham số tìm kiếm");
        } else if (pheptoan == null || pheptoan == '-1') {
            MessageAlert.AlertNormal("Vui lòng chọn phép toán");
        } else if (giaTri != null && giaTri != '') {
            if (fnCheckInputData(tenTruong, kieuDuLieu, pheptoan)) {
                if (kieuDuLieu == 'text') {
                    kieuDuLieu = 'string';
                }
                dataTruyVan.items.push({ TenTruong: tenTruong, TieuDe: tieuDe, KieuDuLieu: kieuDuLieu, PhepToan: pheptoan, GiaTri: giaTri, TinhThanhId: tinhThanhId, Loaikhieunai0id: loaikhieunai0id, Loaikhieunai1id: loaikhieunai1id, Loaikhieunai2id: loaikhieunai2id });
                fnBindDataTruyVan();
            } else {
                MessageAlert.AlertNormal("điều kiện đã có trong danh sách!Vui lòng chọn điều kiện khác");
            }
        }
    }
    function fnCheckInputData(tenTruong, kieuDuLieu, pheptoan) {
        var values = true;
        $.each(dataTruyVan.items, function (i, item) {
            if (item.TenTruong == tenTruong && item.PhepToan == pheptoan && item.KieuDuLieu == kieuDuLieu) {
                values = false;
            }
        });
        return values;
    }
    function fnBindDataTruyVan() {
        var html = '';
        $.each(dataTruyVan.items, function (i, item) {
            if (i % 2 == 0) {
                html += '<tr class ="rowA">';
            } else {
                html += '<tr class ="rowB">';
            }
            html += '        <td align ="left">' + item.TieuDe + '</td>';
            html += '        <td align ="center">' + item.KieuDuLieu + '</td>';
            html += '        <td align ="center">' + item.PhepToan + '</td>';
            html += '        <td align ="left">' + item.GiaTri + '</td>';
            html += '        <td align ="center"><a class="mybtn" href="javascript:RemoveTruyVan(' + i + ');"><span class="del_file">Xóa</span></a></td>';
            html += '    </tr>';
        });

        $("#grid-TruyVan").html(html);
    }
    function RemoveTruyVan(value) {
        var IdTinh = dataTruyVan.items[value]["TinhThanhId"];
        var GiaTri = dataTruyVan.items[value]["GiaTri"];
        var Loaikhieunai0id = dataTruyVan.items[value]["Loaikhieunai0id"];
        var Loaikhieunai1id = dataTruyVan.items[value]["Loaikhieunai1id"];
        var Loaikhieunai2id = dataTruyVan.items[value]["Loaikhieunai2id"];
        var res = GiaTri.split("-");
        if (IdTinh == res[value]) {
            $("#hidtinhthanhid").val("");
        }
        if (Loaikhieunai0id != "") {
            $("#hidloaikhieunai0id").val("");
        }
        if (Loaikhieunai1id != "") {
            $("#hidloaikhieunai1id").val("");
        }
        if (Loaikhieunai2id != "") {
            $("#hidloaikhieunai2id").val("");
        }
        if (dataTruyVan.items.length > 1) {
            if (value == 0) {
                dataTruyVan.items.splice(value, 1);
            } else {
                dataTruyVan.items.splice(value, value);
            }
        } else {
            dataTruyVan = { items: [] };
        }

        fnBindDataTruyVan();
    }
    function fnGetDataJson() {
        var dataJson = '{"object_list":[';
        $.each(dataTruyVan.items, function (i, item) {
            dataJson += '{ "TenTruong":' + '"' + item.TenTruong + '"' + ',' + '"TieuDe":' + '"' + item.TieuDe + '"' + ',' + '"KieuDuLieu":' + '"' + item.KieuDuLieu + '"' + ',' + '"PhepToan":' + '"' + item.PhepToan + '"' + ',' + '"GiaTri":' + '"' + item.GiaTri + '"' + '}' + ',';
        });
        dataJson += ']}'
        return dataJson.replace(",]}", "]}");
    }
    //function pageselectCallback(page_index) {
    //    var curentPages = page_index + 1;
    //    if (dataTruyVan.items.length > 0) {
    //        var dataJson = fnGetDataJson();
    //        $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=1' + '&startPageIndex=' + curentPages + '&pageSize=' + pageSize, { data: dataJson },
    //        function (result) {
    //            $('#grid-data').html(result);
    //            $('a.normalTip').aToolTip();
    //        });

    //    }
    //    return false;
    //}
    function getOptionsFromForm() {
        var opt = { callback: pageselectCallback };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function fnDropPageSizeChange() {
        pageSize = $('#DropPageSize').val();
        fnTruyVan();
    }

    //Process DropdownSelectMulti
    function LoadContentSelectMulti(TenTruong, capDulieu) {
        $('#ul-SelectMulti').html("");
        $("#tagItemSelectMulti").css("display", "block");
        $("#img-loader").css("display", "block");
        var tinhthanhid = $("#hidtinhthanhid").val();
        var loaikhieunai0id = $("#hidloaikhieunai0id").val();
        var loaikhieunai1id = $("#hidloaikhieunai1id").val();
        var loaikhieunai2id = $("#hidloaikhieunai2id").val();
        var jqxhr = $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=10&tenTruong=' + TenTruong + '&cap=' + capDulieu + '&tinhthanhid=' + tinhthanhid + '&loaikhieunai0id=' + loaikhieunai0id + '&loaikhieunai1id=' + loaikhieunai1id + '&loaikhieunai2id=' + loaikhieunai2id, { data: "" },
            function (result) {
                $("#img-loader").css("display", "none");
                $('#ul-SelectMulti').html(result);

            });
        jqxhr.complete(function () {
            var Height = $('#ul-SelectMulti').innerHeight();
            var Width = $('#ul-SelectMulti').innerWidth();
            if (Height > 280) {
                $("#tagItemSelectMulti").css("overflow-x", "scroll");
                $("#tagItemSelectMulti").css("height", 280);
                $("#tagItemSelectMulti").css("width", Width);
            } else {
                // $("#tagItemSelectMulti").removeAttribute('overflow-x');
                $("#tagItemSelectMulti").removeAttr('overflow-x');
                $("#tagItemSelectMulti").css("height", Height + 10);
                $("#tagItemSelectMulti").css("width", Width);
            }
        });
    }

    function DropdownSelectMulti() {
        var DropParam = $('#DropParam').val();
        var validate = true;
        var capDulieu = 0;
        var arr = DropParam.split('#');
        var KieuDuLieu = arr[0];
        var TenTruong = arr[1];
        if (KieuDuLieu == 'int') {
            if (dataTruyVan.items.length > 0) {
                $.each(dataTruyVan.items, function (i, item) {
                    if (TenTruong == item.TenTruong) {
                        validate = false;
                        MessageAlert.AlertNormal("Điều kiện này đã có trong danh sách điều kiện tìm kiếm!");
                    }
                });
            }
            if (validate) {
                if (TenTruong == 'LoaiKhieuNaiId' || TenTruong == 'MaTinhId') {
                    capDulieu = 0;
                } else if (TenTruong == 'LinhVucChungId' || TenTruong == 'MaQuanId') {
                    capDulieu = 1;
                } else if (TenTruong == "LinhVucConId") {
                    capDulieu = 2;
                }
                LoadContentSelectMulti(TenTruong, capDulieu);
            }
        }
    }
    //End Process DropdownSelectMulti

    function Autocomplete(column, tagInput) {
        var values = '';
        $("#" + tagInput).autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=2&column=" + column, {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    if (column == 'NguoiTiepNhan' || column == 'NguoiXuLy') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.TenTruyCap
                        };
                    } else if (column == 'LoaiKhieuNaiId' || column == 'LinhVucChungId' || column == 'LinhVucConId') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.Name
                        };
                    } else if (column == 'PhongBanTiepNhanId' || column == 'PhongBanXuLyId') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.Name
                        };
                    }
                    else if (column == 'MaTinhId' || column == 'MaQuanId') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.Name
                        };
                    }
                    else if (column == 'DoiTacId' || column == 'DoiTacXuLyId') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.TenDoiTac
                        };
                    }
                    else if (column == 'DoUuTien' || column == 'TrangThai' || column == 'DoHaiLong') {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.Name
                        };
                    }
                });
            },
            formatItem: function (item) {
                return format(item, column);
            }
        }).result(function (e, item) {
            var pheptoan = $("#DropPhepToan").val();
            if (values != '' && pheptoan == '=') {
                $("#" + tagInput).val(values.substring(0, values.length - 1));
                MessageAlert.AlertNormal('Phép toán (=) chỉ được phép nhập 1 giá trị !', 'error');
            } else {
                if (column == 'NguoiTiepNhan' || column == 'NguoiXuLy') {
                    values += item.TenTruyCap + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                } else if (column == 'LoaiKhieuNaiId' || column == 'LinhVucChungId' || column == 'LinhVucConId') {
                    values += item.Id + "-" + item.Name + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                } else if (column == 'PhongBanTiepNhanId' || column == 'PhongBanXuLyId') {
                    values += item.Id + "-" + item.Name + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                } else if (column == 'MaTinhId' || column == 'MaQuanId') {
                    values += item.Id + "-" + item.Name + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                }
                else if (column == 'DoUuTien' || column == 'TrangThai' || column == 'DoHaiLong') {
                    values += item.Value + "-" + item.Name + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                } else if (column == 'DoiTacId' || column == 'DoiTacXuLyId') {
                    values += item.Value + "-" + item.TenDoiTac + ",";
                    $("#" + tagInput).val(values.substring(0, values.length - 1));
                }
            }

        });
        $("#" + tagInput).focus();
    }
    function format(item, column) {
        if (column == 'NguoiTiepNhan' || column == 'NguoiXuLy') {
            return "<span class='ac_keyword'>" + item.TenTruyCap + "</span>";
        } else if (column == 'LoaiKhieuNaiId' || column == 'LinhVucChungId' || column == 'LinhVucConId') {
            return "<span class='ac_keyword'>" + item.Name + "</span>";
        } else if (column == 'PhongBanTiepNhanId' || column == 'PhongBanXuLyId') {
            return "<span class='ac_keyword'>" + item.Name + "</span>";
        } else if (column == 'MaTinhId' || column == 'MaQuanId') {
            return "<span class='ac_keyword'>" + item.Name + "</span>";
        } else if (column == 'DoUuTien' || column == 'TrangThai' || column == 'DoHaiLong') {
            return "<span class='ac_keyword'>" + item.Name + "</span>";
        } else if (column == 'DoiTacId' || column == 'DoiTacXuLyId') {
            return "<span class='ac_keyword'>" + item.TenDoiTac + "</span>";
        }
    }
    function fnLuuTruyVan() {
        if (dataTruyVan.items.length > 0) {
            var dataJson = fnGetDataJson();
            var NameLichSuTruyVan = $('#NameLichSuTruyVan').val();
            $.getJSON('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=7' + '&Name=' + NameLichSuTruyVan, '', function (result) {
                if (result == '1') {
                    MessageAlert.AlertNormal("Trùng thông tin!");
                }
                else {
                    $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=5' + '&Name=' + NameLichSuTruyVan, { data: dataJson },
                    function (result) {
                        ClosePoupTitle();
                        if (result == '0') {
                            MessageAlert.AlertNormal("Thêm mới không thành công!");
                        } else if (result == '-1') {
                            MessageAlert.AlertNormal("Lỗi thêm mới!");
                        } else {
                            fnLoadDanhSachSuTruyVan();
                            MessageAlert.AlertNormal("Thêm mới thành công!");

                        }
                    });
                }
            });

        }
    }

    function fnExportExcel() {
        var total = -1;
        try {
            var text = $("#divTotalRecords span").text();
            text = text.replace("(", "").replace(")", "").replace(",", "");
            if ($.isNumeric(text)) total = Number(text)
        }
        catch (e) {
            if (console && console.log) console.log(e);
        }
        var tmpZip = 0;
        if (total == -1 || total > 15000) {
            var isZip = window.confirm("Nếu số lượng bản ghi quá lớn, sẽ trả về theo từng trang, bạn có muốn nén lại thành 1 file?");
            tmpZip = (isZip == true) ? 1 : 0;
        }
        $(".over-screen").removeClass("off").addClass("on");
        if (dataTruyVan.items.length > 0) {
            $("#rowListData").css("display", "block");
            var dataJson = fnGetDataJson();
            $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?Key=9' + '&StartPageIndex=1&PageSize=' + 10000 + "&isZip=" + tmpZip, { data: dataJson },
                function (result) {
                    try {
                        var retObj = $.parseJSON(result);

                        // Thành công
                        if (retObj.Code == 1) {
                            $(".over-screen").removeClass("on").addClass("off");
                            // document.location.href = retObj.Data;
                            // File nén
                            if (tmpZip == 1) document.location.href = retObj.Data;
                            else {
                                // Không nén, nhiều hơn 1 file
                                if (total > 15000) {
                                    var count = retObj.DataEx;
                                    if (console && console.log) console.log("Số lượng file cần tải về: " + count);
                                    for (var i = 0; i < retObj.Data.length; i++) {
                                        window.open(retObj.Data[i]);
                                    }
                                }
                                else // Không nén, 1 file
                                {
                                    document.location.href = retObj.Data
                                }
                            }
                        }
                        else {
                            $(".over-screen").removeClass("on").addClass("off");
                            alert(retObj.Message);
                            if (console && console.log) console.log(retObj);
                        }
                    }
                    catch (e) {
                        $(".over-screen").removeClass("on").addClass("off");
                        alert("Có lỗi xảy ra, vui lòng thử lại!");
                        if (console && console.log) console.log(e);
                    }
                });
        } else {
            $(".over-screen").removeClass("on").addClass("off");
            $("#rowListData").css("display", "none");
            MessageAlert.AlertNormal("Danh sách không có dữ liệu");
        }
    }
</script>
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="float: right;"><a href="javascript:history.back()">
            <input type="button" class="button_eole back" value="Quay về"></a></li>
        <li style="background: none;"><span id="titleChoXuLy" style="color: #4D709A; font-size: 15px; font-weight: bold;"></span></li>
        <li style="float: right;"><a href="javascript:fnExportExcel();"><span class="ex_excel">Xuất Excel</span></a> </li>
        <li style="float: right;"><a href="javascript:ShowPoupLichSu();">
            <input type="button" class="button_eole history" value="Lịch sử truy vấn"></a></li>
    </ul>
    <div class="div-clear">
    </div>
</div>
<div class="p8">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr valign="top">
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td>
                    <table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
                        <tbody>
                            <tr>
                                <td bgcolor="#f0f0f0" style="text-align: left">
                                    <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;"></h3>
                                </td>
                            </tr>
                            <tr style="background: #fffff0">
                                <td>
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <div id="divTruyVan">
                                                        <div class="content-truyvan">
                                                            <div id="tagItemSelectMulti">
                                                                <img id="img-loader" style="" src="/Images/loader.gif" />
                                                                <ul id="ul-SelectMulti" class="listitem">
                                                                </ul>
                                                            </div>
                                                            <span class="selectstyle">
                                                                <select id="DropParam" onchange="javascript:fnDropParamChange();" style="width: 250px;">
                                                                </select>
                                                            </span><span class="selectstyle">
                                                                <select id="DropPhepToan" onchange="javascript:fnDropPhepToanChange();" style="width: 150px;">
                                                                </select>
                                                            </span><span id="tagAdd" class="inputstyle" style="position: relative;"></span><span
                                                                id="tagChuThich"><span style="float: right;"><a href="javascript:ClosePoup();">
                                                                    <img src="/Images/x.png" /></a> </span>
                                                                <br />
                                                                <span id="tagContentChuThich"></span></span><a href="javascript:CreateTruyVan();"
                                                                    style="float: right; height: 20px; margin-top: 10px; padding-left: 10px; padding-right: 10px; width: 20px;">
                                                                    <img src="/images/icons/plus2.gif" />
                                                                </a><a href="javascript:XoaDieuKienTruyVan();" style="float: right; height: 20px; margin-top: 10px; padding-left: 10px; padding-right: 10px; width: 20px;">
                                                                    <img src="/images/icons/sync-16.png" />
                                                                </a>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <div id="div1" style="margin-bottom: 10px;">
                                                        <div class="content-truyvan">
                                                            <table class="tbl_style" cellspacing="0" cellpadding="0" style="width: 100%;">
                                                                <tr style="line-height: 25px;">
                                                                    <th align="center" style="background: #F0F0F0 !important; border: 1pt solid #B2D4E6 !important; color: #333"
                                                                        class="thead-colunm">Tên trường
                                                                    </th>
                                                                    <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important; border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;"
                                                                        class="thead-colunm">Kiểu dữ liệu
                                                                    </th>
                                                                    <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important; border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;"
                                                                        class="thead-colunm">Phép toán
                                                                    </th>
                                                                    <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important; border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;"
                                                                        class="thead-colunm">Giá trị
                                                                    </th>
                                                                    <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important; border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important; width: 100px"
                                                                        class="thead-colunm">Thao tác
                                                                    </th>
                                                                </tr>
                                                                <tbody id="grid-TruyVan">
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <a style="height: 20px; padding-top: 5px;" onclick="Javascript:fnTruyVan();" class="btn_style_button">Truy vấn</a> <a style="height: 20px; padding-top: 5px;" onclick="Javascript:ShowPoupTitle();"
                                                        class="btn_style_button">Lưu truy vấn</a>
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
                <td style="height: 10px"></td>
            </tr>
            <tr id="rowListData" style="display: none;" valign="top">
                <td>
                    <div id="divNote" style="width: 500px; float: left; margin-top: 5px;">
                        <p style="border: 1pt solid #CCC; background: #FF0000; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span>
                        <p style="border: 1pt solid #CCC; background: #FFFF00; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span>
                        <p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span>
                        <p style="border: 1pt solid #CCC; background: #088A08; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN đã đóng</span>
                        <p style="border: 1pt solid #CCC; background: #999; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN quá hạn</span>
                    </div>
                    <div id="Pagination" class="pagination" style="float: right; margin-right: -3px;">
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
                    <div id="divTotalRecords" style="width: 180px; float: right; margin-top: 5px; text-align: right;">
                    </div>


                </td>
            </tr>
        </tbody>
    </table>
    <%--phuongtt--%>
    <table class="nobor">
        <tr>
            <td>
                <!--Code html chi co 3 dong nay thoi flex_KNChoXuLy-->
                <div id="divScroll" style="height: 370px; width: 100%;">
                    <table class="flex_KNChoXuLy" style="display: none"></table>
                </div>
            </td>
        </tr>
    </table>
    <%--phuongtt--%>
</div>
<div id="divPoup" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 200; position: fixed; top: 15%; left: 15%; right: 15%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="divTitle" style="float: left; color: #fff; font-weight: bold;">Lịch sử truy vấn
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoupLichSu();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="divContent" style="margin: 5px;">
        <LichSuTruyVan:LichSuTruyVan ID="LichSuTruyVan1" runat="server" />
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div id="divPoupTitle" style="">
    <h3 id="H1" style="float: left; color: #4f4f4f; font-weight: bold;">Nhập tiêu đề
    </h3>
    <span style="float: right;"><a href="javascript:ClosePoupTitle();">
        <img src="/Images/x.png" />
    </a></span>
    <div style="clear: both; height: 5px;">
    </div>
    <div class="inputstyle" style="text-align: center;">
        <input type="text" style="width: 95% !important; height: 26px !important;" placeholder="Nhập tiêu đề..."
            class="typeNumber ac_input" id="NameLichSuTruyVan" />
    </div>
    <div style="clear: both; height: 5px;">
    </div>
    <div class="nav_btn" style='background: none;'>
        <ul>
            <li style="float: right;"><a href="javascript:ClosePoupTitle();"><span class="notapply">Hủy </span></a></li>
            <li style="float: right;"><a href="javascript:fnLuuTruyVan();"><span class="apply">Cập
                nhật </span></a></li>
        </ul>
    </div>
</div>
<div id="divOpacity" class="divOpacity" style="opacity: 0.4; background: #000; width: 100%; position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
</div>
