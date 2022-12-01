<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNPhanViec.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.KNPhanViec" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ChiTietKN.ascx" TagName="ChiTietKN"
    TagPrefix="ChiTietKN" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ListKNSelect.ascx" TagName="ListKNSelect"
    TagPrefix="ListKNSelect" %>
<script src="/JS/jquery.pagination.js" type="text/javascript"></script>
<script type="text/javascript">
    var pageSize = '';
    $(document).ready(function () {
        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();
        fnSetViewPoupChiTiet();
        fnLoadDropLoaiKhieuNai();

        fnLoadDropdoUuTien();
        fnLocKhieuNai();

        $("#txtNgayQuaHanPhongBan_From").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#txtNgayQuaHanPhongBan_To").datepick({ dateFormat: 'dd/mm/yyyy' });

    });

    function pageselectCallback(page_index) {
        var curentPages = page_index + 1;
        var typeKhieuNai = $("#DropTypeKhieuNai").val();
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
        var _IsPhanHoi = $('#chkIsPhanHoi').attr("checked") ? 1 : 0;
        var txtNgayQuaHanPhongBan_From = $("#txtNgayQuaHanPhongBan_From").val();
        var txtNgayQuaHanPhongBan_To = $("#txtNgayQuaHanPhongBan_To").val();

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=48'
            + '&typeKhieuNai=' + typeKhieuNai
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&isPhanhoi=' + _IsPhanHoi
            + '&ngayQuaHanPhongBanXuLyTu=' + txtNgayQuaHanPhongBan_From
            + '&ngayQuaHanPhongBanXuLyDen=' + txtNgayQuaHanPhongBan_To
            + '&pageSize=' + pageSize
            + '&startPageIndex=' + curentPages, '', function (result) {
                if (result != '') {
                    $('#grid-data').html(result);
                    $('a.normalTip').aToolTip();
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
        var d = $('body').innerWidth() - 56;
        var h = screen.height;
        $("#divScroll").css("width", d);
        $(".divOpacity").css("height", h);

    }
    function fnAddScrollDiv() {
        var h1 = $('#divPoup').innerHeight() - 40;
        var h2 = $('#divContent').innerHeight();
        if (h2 > h1) {
            $("#divContent").css("overflow-y", "scroll");
            $("#divContent").css("height", h1 - 20);
        } else {
            $("#divContent").removeAttr("style");
            $("#divContent").attr("style", "margin: 5px;");
        }


    }
    function fnSetViewPoupChiTiet() {

        var d = screen.width;

        if (d >= 1360) {
            $("#divPoup").css("left", "10%");
            $("#divPoup").css("right", "10%");
        } else {
            if (d >= 1280) {

                $("#divPoup").css("left", "7%");
                $("#divPoup").css("right", "7%");
            } else {
                if (d >= 1024) {
                    $("#divPoup").css("left", "3%");
                    $("#divPoup").css("right", "3%");
                }
            }
        }

    }
    function fnLocKhieuNai() {
        var optInit = getOptionsFromForm();
        var typeKhieuNai = $("#DropTypeKhieuNai").val();
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
        var _IsPhanHoi = $('#chkIsPhanHoi').attr("checked") ? 1 : 0;
        var txtNgayQuaHanPhongBan_From = $("#txtNgayQuaHanPhongBan_From").val();
        var txtNgayQuaHanPhongBan_To = $("#txtNgayQuaHanPhongBan_To").val();

        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=47'
            + '&typeKhieuNai=' + typeKhieuNai
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&isPhanhoi=' + _IsPhanHoi
            + '&ngayQuaHanPhongBanXuLyTu=' + txtNgayQuaHanPhongBan_From
            + '&ngayQuaHanPhongBanXuLyDen=' + txtNgayQuaHanPhongBan_To
            + '&pageSize=' + pageSize
            + '&startPageIndex=1', '',
            function (totalRecords) {
                if (totalRecords != '') {
                    //$("#Pagination").pagination(totalRecords, optInit);
                    if (totalRecords == 0) {
                        $("#Pagination").pagination(0, optInit);
                    }
                    else {
                        $("#Pagination").pagination(totalRecords, optInit);
                    }
                    $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                }

            });
    }
    function fnLoadDropdoUuTien() {
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=28', '', function (result) {
            if (result != '') {
                $('#DropdoUuTien').html(result);
            }

        });
    }
    function fnLoadDropLoaiKhieuNai() {
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=25', '', function (result) {
            if (result != '') {
                $('#DropLoaiKhieuNai').html(result);
            }

        });
    }
    function fnDropKhieuNaiChange() {
        //        var values = $("#DropKhieuNai").val();
        //        if (values == '3') {
        //            $('#btnDongKN').show();
        //            $('#btnChuyenKNHangLoat').show();
        //            $('#btnChuyenKhieuNai').hide();
        //            $('#btnTruyVan').hide();
        //        } else {
        //            $('#btnDongKN').hide();
        //            $('#btnChuyenKNHangLoat').hide();
        //            $('#btnChuyenKhieuNai').show();
        //            $('#btnTruyVan').show();
        //        }

    }
    function fnLoadDropTrangThai() {
        //console.log("abc");
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=352', '', function (result) {
            if (result != '') {
                //console.log(result);
                //result = result + '<option value="3">Đóng</option>';

                $('#DropTrangThai').html(result);

                var TrangThai = Utility.GetUrlParam("TrangThai");
                if (TrangThai != "") {
                    $("#DropTrangThai").val(TrangThai);
                }
            }
        });
    }
    function fnDropLoaiKhieuNaiChange() {
        var loaiKhieuNaiId = document.getElementById('DropLoaiKhieuNai').value;
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=26&loaiKhieuNaiId=' + loaiKhieuNaiId, '', function (result) {
            if (result != '') {
                $('#DropLinhVucChung').html(result);
            }

        });
    }
    function fnDropLinhVucChungChange() {
        var linhVucChungId = document.getElementById('DropLinhVucChung').value;
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=27&linhVucChungId=' + linhVucChungId, '', function (result) {
            if (result != '') {
                $('#DropLinhVucCon').html(result);
            }

        });
    }
    function fnDropPageSizeChange() {
        pageSize = $('#DropPageSize').val();
        fnLocKhieuNai();
    }
    function fnDropTypeKhieuNaiChange() {
        if ($('#DropTypeKhieuNai').val() == "4")
        {
            $('#TrangThaiPhanHoi').show();
        }
        else
        {
            $('#TrangThaiPhanHoi').hide();
        }
        fnLocKhieuNai();
    }
    function fnClearFilter() {
        $("#DropLinhVucChung").val('-1');
        $("#DropLinhVucCon").val('-1');
        $("#DropKhieuNai").val('-1');
        $('#DropLoaiKhieuNai').val('-1');
        $('#DropdoUuTien').val('-1');

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
    function fnGetSetUrl(ctrl) {

        var paramTab = fnGetUrlParameter('tab');
        var catid = fnGetUrlParameter('catid');
        if (paramTab != '') {
            if (catid != '') {
                window.location = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?tab=" + paramTab + "&ctrl=" + ctrl + "&catid=" + catid;
            } else {
                window.location = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?tab=" + paramTab + "&ctrl=" + ctrl;
            }

        } else {
            alert('ko co tab');

        }

    }
    function ShowPoup(id) {
        $('#divPoup').show();
        $('.divOpacity').css('display', 'block');
        var jqxhr = $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=5&id=' + id + '&view=0', '', function (result) {
            if (result != '') {
                //var obj = eval("(" + result + ")");
                $("#info-SoThueBao").html(result.SoThueBao);
                $("#info-MaKhieuNai").html(result.MaKhieuNai);
                $("#info-LoaiKhieuNai").html(result.LoaiKhieuNai);
                $("#info-LinhVucChung").html(result.LinhVucChung);
                $("#info-LinhVucCon").html(result.LinhVucCon);
                $("#info-NguoiXuLy").html(result.NguoiXuLy);
                $("#info-NgayCapNhat").html(result.LDate);
                $("#info-HoTen").html(result.HoTenLienHe);
                $("#info-DoUuTien").html(result.DoUuTien);
                $("#info-ThoiHan").html(result.NgayQuaHan);
                $("#info-DienThoaiLienHe").html(result.SDTLienHe);
                $("#info-NguoiTiepNhan").html(result.NguoiTiepNhan);
                $("#info-NgayTiepNhan").html(result.NgayTiepNhan);
                $("#info-ThoiGianXayRaSuCo").html(result.ThoiGianXayRa);
                $("#info-DiaChi").html(result.DiaChiLienHe);
                $("#info-TrangThai").html(result.TrangThai);
                $("#info-NgayDong").html(result.NgayDongKN);
                $("#info-DiaDiemXayRaSuCo").html(result.DiaDiemXayRa);
                $("#info-GhiChu").html(result.GhiChu);
                $("#info-FileKHGui").html(result.FileDinhKemKH);
                $("#info-FileGQKNGui").html(result.FileDinhKemGQKN);
                $("#info-NoiDungPhanAnh").html(result.NoiDungPA);
                var iframe = '<iframe frameborder="0" src="/Views/QLKhieuNai/LoadControls.aspx?MaKN=' + id + '&Mode=View" width="100%" height="300px"></iframe>';
                $('#tabKNInfo-Content').html(iframe);
            }

        })
        jqxhr.complete(function () {
            fnAddScrollDiv();
        });
    }




    function Update(jobID, content) {

        ClosePoup();
    }
    var listID = '';
    function fnPhanViec() {
        if (listID != '') {
            if (ValidatePhanViec()) {
                var user = $('input[name="SelectPhanViec"]:checked').val();
                $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=45&listID=' + listID + '&user=' + user, '', function (result) {
                    fnLocKhieuNai();
                    MessageAlert.AlertNormal('Phân việc thành công !');
                    ClosePoup();
                });

            }
        }
    }

    function ShowPoupPhanViec() {
        var totalSelect = 0;
        $(".checkbox-item").each(function () {
            if (this.checked) {
                listID += $(this).val() + ",";
                totalSelect++;
            }

        })
        if (listID != '') {
            $('#divPoupPhanViec').show();
            $("#totalSelect").html('(' + totalSelect + ')');
            $('.divOpacity').css('display', 'block');
        } else {
            MessageAlert.AlertNormal('Vui lòng chọn nội dung cần phân việc !');
        }

    }
    function ValidatePhanViec() {
        var ss = $('input[name="SelectPhanViec"]:checked').val();

        var radio = document.getElementsByName('SelectPhanViec');
        var isChecked = false;
        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked) {
                isChecked = true;
                break;
            }
        }
        if (!isChecked) {

            MessageAlert.AlertNormal("Vui lòng chọn tài khoản cần phân");

        }

        return isChecked;

    }

    function fnExportExcel() {
        $('.divOpacity').css('display', 'block');

        var pageSizeExp = $("#DropPageSize").val();
        var typeKhieuNai = $("#DropTypeKhieuNai").val();
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

        $.getJSON('/Views/QLKhieuNai/Handler/ExportExcel.ashx?key=6'
           + '&typeKhieuNai=' + typeKhieuNai
            + '&typeSearch=' + typeSearch
            + '&doUuTien=' + doUuTien
            + '&loaiKhieuNai=' + loaiKhieuNai
            + '&linhVucChung=' + linhVucChung
            + '&linhVucCon=' + linhVucCon
            + '&pageSize=' + pageSizeExp
            + '&startPageIndex=1', '',
            function (result) {
                if (result != '') {
                    window.location.href = "/ExportExcel/Excel" + result;
                    $('.divOpacity').css('display', 'none');
                } else {
                    $('.divOpacity').css('display', 'none');
                    MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại', 'error');
                }

            });
    }

    function ClosePoup() {
        listID = '';
        $('.divOpacity').css('display', 'none');
        $('#divPoup').hide();
        $('#divPoupPhanViec').hide();

    }

</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>

    <ul>
        <li style="background: none;"><span style="color: #4D709A; font-size: 15px; font-weight: bold;">Phân công xử lý khiếu nại</span></li>
        <li id="btnExportExcel" style="float: right;"><a href="javascript:fnExportExcel();"><span class="ex_excel">Xuất Excel</span></a> </li>
        <li id="btnLichSuPhanViec" style="float: right;">
            <a href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab0-KNLichSuPhanViec"><span class="button_eole history">Lịch sử phân việc</span></a>
        </li>
        <li id="btnPhanViec" style="float: right;">
            <a href="javascript:ShowPoupPhanViec();"><span class="phanviec">Phân việc</span></a>
        </li>

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
                                    <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px;">Lọc tìm kiếm</h3>
                                </td>
                            </tr>
                            <tr style="background: #fffff0">
                                <td>
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left; width: 200px;">
                                                    <div class="selectstyle">
                                                        <div class="bg">

                                                            <select id="DropTypeKhieuNai" onchange="javascript:fnDropTypeKhieuNaiChange();" style="width: 200px;">
                                                                <option value="1">Khiếu nại chờ xử lý</option>
                                                                <option value="2">Khiếu nại bộ phận khác chuyển về</option>
                                                                <option value="3">Khiếu nại sắp quá hạn</option>
                                                                <option value="4">Khiếu nại quá hạn</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left;">
                                                    <table class="nobor">
                                                        <tr><td style="width:420px">

                                                            <div class="selectstyle">
                                                        <div class="bg">

                                                            <select id="DropKhieuNai" onchange="javascript:fnDropKhieuNaiChange();" style="width: 200px;">
                                                                <option value="-1">--Tất cả khiếu nại--</option>
                                                                <option value="1">Cá nhân</option>
                                                                <option value="2">Phòng ban</option>
                                                                <option value="3">Khiếu nại hàng loạt</option>
                                                            </select>
                                                            <select id="DropdoUuTien" style="width: 200px;">
                                                            </select>
                                                            
                                                            
                                                        </div>
                                                    </div>
                                                            </td>

                                                            <td style="text-align:left">
                                                                <span id="TrangThaiPhanHoi" style="display: none;">
                                                                Trạng thái phản hồi
                                                            <input type="checkbox" id="chkIsPhanHoi"  name="chkIsPhanHoi" />
                                                                    </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </td>
                                                <td style="text-align: left">
                                                    <a style="height: 20px; padding-top: 5px;" onclick="Javascript:fnLocKhieuNai();"
                                                        class="btn_style_button">Lọc</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <select id="DropLoaiKhieuNai" onchange="javascript:fnDropLoaiKhieuNaiChange();">
                                                            </select>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <select id="DropLinhVucChung" onchange="javascript:fnDropLinhVucChungChange();" style="width: 200px;">
                                                            </select>
                                                            <select id="DropLinhVucCon" style="width: 200px;">
                                                            </select>
                                                            <input id="txtAutocomLoaiKN" placeholder="Nhập lựa chọn..." style="width: 200px;" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left">
                                                    <a style="" onclick="Javascript:fnClearFilter();" class="button_clear_filter">Xóa điều
                                                        kiện lọc</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            Ngày quá hạn PB
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <input id="txtNgayQuaHanPhongBan_From" placeholder="Từ ngày..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                                            <input id="txtNgayQuaHanPhongBan_To" placeholder="Đến ngày..."  style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="text-align: left">
                                                   
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
            <tr valign="top">
                <td>
                    <div id="divNote" style="width: 400px; float: left; margin-top: 5px;">
                        <p style="border: 1pt solid #CCC; background: #FF0000; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span><p style="border: 1pt solid #CCC; background: #FFFF00; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span><p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span><p style="border: 1pt solid #CCC; background: #088A08; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN đã đóng</span></div>
                    <div id="Pagination" class="pagination" style="float: right; margin-right: -5px;">
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

                    <ListKNSelect:ListKNSelect ID="ListKNSelect2" runat="server" />
                    <div class="div-clear" style="height: 10px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="divPoup" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 200; position: fixed; top: 15%; left: 20px; right: 20px; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="divTitle" style="float: left; color: #fff; font-weight: bold;">Thông tin chi tiết khiếu nại - <span id="info-MaKhieuNai"></span>
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="divContent" style="margin: 5px;">
        <ChiTietKN:ChiTietKN ID="ChiTietKN1" runat="server" />
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div id="divPoupPhanViec" style="width: auto; height: auto; background: #fff; margin: 0 auto; z-index: 200; position: fixed; top: 15%; left: 30%; right: 30%; border: 1px solid #4D709A; border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px; height: 25px;">
        <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Chọn tài khoản 
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div1" style="">
        <div class="nav_btn" style='background: none;'>
            <h3>Tổng số khiếu nại đã chọn : <span id="totalSelect" style="color: Red;"></span></h3>
            <div style="margin: 5px; height: 250px; background: none; overflow-x: scroll;">
                <asp:Repeater ID="rptListDataPhanViec" runat="server">
                    <HeaderTemplate>
                        <table cellspacing="1" class="tbl_style">
                            <thead>
                                <tr>
                                    <th class="title" width="30">STT
                                    </th>
                                    <th class="title" align="center">Tài khoản tiếp nhận
                                    </th>
                                    <th class="title" align="center" width="70">Tổng số
                                    </th>
                                    <th class="title" width="50">Chọn
                                    </th>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td style="padding-left: 6px;">
                                    <%# (Container.ItemIndex) + 1%>
                                </td>
                                <td class="nowrap" align="left">
                                    <a target="_blank" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab6-KNTongHopChoXuLy&NXLy=<%# DataBinder.Eval(Container.DataItem, "TenTruyCap")%>&Show=0">
                                        <%# DataBinder.Eval(Container.DataItem, "TenTruyCap")%></a>
                                </td>
                                <td class="nowrap" align="center">
                                    <%# DataBinder.Eval(Container.DataItem, "TongSo")%>
                                </td>
                                <td class="nowrap" align="center">
                                    <input type="radio" id="rad<%# DataBinder.Eval(Container.DataItem, "TenTruyCap")%>" name="SelectPhanViec"
                                        value="<%# DataBinder.Eval(Container.DataItem, "TenTruyCap")%>">
                                </td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <div style="clear: both; height: 1px; border-bottom: 1px solid #CCC; margin-bottom: 5px;">
            </div>
            <ul>
                <li style="float: right;"><a href="javascript:ClosePoup();"><span class="notapply">Hủy
                </span></a></li>
                <li style="float: right;"><a href="javascript:fnPhanViec();"><span class="apply">Đồng ý </span></a></li>
            </ul>
        </div>
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div id="divOpacity" class="divOpacity" style="opacity: .4; -moz-opacity: 0.4; filter: alpha(opacity=70); background: #000; width: 100%; position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
</div>
