<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFillterMyKhieuNai.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.UcFillterMyKhieuNai" %>
<link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
<script type="text/javascript">
    var LoaiKNId_Filter;
    var LinhVucChungId_Filter;
    var LinhVucConId_Filter;
    $(document).ready(function () {

        LoaiKNId_Filter = Utility.GetUrlParam("LoaiKhieuNaiId");
        LinhVucChungId_Filter = Utility.GetUrlParam("LinhVucChungId");
        LinhVucConId_Filter = Utility.GetUrlParam("LinhVucConId");

        $("#ai-images").html('<img src="/images/icons/plus2.gif" /><b>&nbsp;Hiện bộ lọc</b>');
        fnLoadDropLoaiKhieuNai();
        fnLoadDropTrangThai();
        fnLoadDropdoUuTien();
        fnLoadDropPhongBanXuLy();
        AutocompleteNguoiTiepNhan();
        AutocompleteNguoiXuLy();
        AutocompleteNguoiTienXuLy();

        $("#txtNgayTiepNhan_From").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#txtNgayTiepNhan_To").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#txtNgayQuaHan_From").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#txtNgayQuaHan_To").datepick({ dateFormat: 'dd/mm/yyyy' });

        $('input[placeholder]').each(function () {
            var input = $(this);

            $(input).focus(function () {
                if (input.val() == input.attr('placeholder')) {
                    input.val('');
                }
            });

            $(input).blur(function () {
                if (input.val() == '' || input.val() == input.attr('placeholder')) {
                    input.val(input.attr('placeholder'));
                }
            });
        });

        SetParam();
        if ((LoaiKNId_Filter == "" || LoaiKNId_Filter == -1)
            && (LinhVucChungId_Filter == "" || LinhVucChungId_Filter == -1)
            && (LinhVucConId_Filter == "" || LinhVucConId_Filter == -1)) {
            fnLocKhieuNai();
        }

        AutocomLoaiKN();
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=29&linhVucChungId=-1', '', function (result) {
            if (result != '') {
                $('#DropLinhVucCon').html(result);
                //$("#DropLinhVucCon").val(linhVucCon);
            }
        });
    });

    function AutocomLoaiKN() {
        $("#txtAutocomLoaiKN").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=4", {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    return {
                        data: row,
                        value: index.toString(),
                        result: row.Name
                    };
                });
            },
            formatItem: function (item) {
                return formatAutocomLoaiKN(item);
            }
        }).result(function (e, item) {
            $.getJSON('/Views/QLKhieuNai/Handler/Autocom.ashx?key=5&Id=' + item.Id + '&ParentId=' + item.ParentId, '', function (result) {
                if (result != '') {
                    $("#txtAutocomLoaiKN").val(item.Name);
                    var arr = result.split('#');
                    var loaiKN = arr[0];
                    var linhVucChung = arr[1];
                    var linhVucCon = arr[2];

                    $("#DropLinhVucChung").val(-1);
                    $("#DropLinhVucCon").val(-1);
                    if (loaiKN != '0') {
                        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=25', '', function (result) {
                            if (result != '') {
                                $('#DropLoaiKhieuNai').html(result);
                                $("#DropLoaiKhieuNai").val(loaiKN);
                                if ($("#DropLoaiKhieuNai").val() != "-1") {
                                    if (linhVucChung != '0') {
                                        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=26&loaiKhieuNaiId=' + loaiKN, '', function (result) {
                                            if (result != '') {
                                                $('#DropLinhVucChung').html(result);
                                                $("#DropLinhVucChung").val(linhVucChung);
                                                if ($("#DropLinhVucChung").val() != "-1") {
                                                    if (linhVucCon != '0') {
                                                        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=27&linhVucChungId=' + linhVucChung, '', function (result) {
                                                            if (result != '') {
                                                                $('#DropLinhVucCon').html(result);
                                                                $("#DropLinhVucCon").val(linhVucCon);
                                                            }
                                                        });
                                                    }
                                                }
                                                else {
                                                    ShowThongBao('Không có khiếu nại phù hợp với loại khiếu nại này.');
                                                }
                                            }
                                        });
                                    }
                                }
                                else {
                                    ShowThongBao('Không có khiếu nại phù hợp với loại khiếu nại này.');
                                }
                            }

                        });

                    }

                }

            });

        });

        $("#txtAutocomLoaiKN").focus();
    }
    function formatAutocomLoaiKN(item) {
        return "<span class='ac_keyword'>" + item.Name + "</span>";
    }

    function SetParam() {
        var TypeSearch = Utility.GetUrlParam("TypeSearch");
        if (TypeSearch != "") {
            $("#DropKhieuNai").val(TypeSearch);
        }

        var pSize = Utility.GetUrlParam("PSize");
        if (pSize != "") {
            $("#DropPageSize").val(pSize);
            pageSize = pSize;
        }

        var contentSearch = Utility.GetUrlParam("ContentSeach");
        if (contentSearch != "") {
            $("#contentSeach").val(decodeURIComponent(contentSearch));
        }

        var STB = Utility.GetUrlParam("STB");
        if (STB != "-1") {
            $("#txtSoThueBao").val(STB);
        }

        var NTNhan = Utility.GetUrlParam("NTNhan");
        if (NTNhan != "") {
            $("#txtNguoiTiepNhan").val(NTNhan);
        }

        var NXLy = Utility.GetUrlParam("NXLy");
        if (NXLy != "") {
            $("#txtNguoiXuLy").val(NXLy);
        }

        var TNTu = Utility.GetUrlParam("TNTu");
        if (TNTu != "-1" && TNTu.length == 8) {
            $("#txtNgayTiepNhan_From").val(TNTu.substring(6, 8) + '/' + TNTu.substring(4, 6) + '/' + TNTu.substring(0, 4));
        }

        var TNDen = Utility.GetUrlParam("TNDen");
        if (TNDen != "-1" && TNDen.length == 8) {
            $("#txtNgayTiepNhan_To").val(TNDen.substring(6, 8) + '/' + TNDen.substring(4, 6) + '/' + TNDen.substring(0, 4));
        }

        var QHTu = Utility.GetUrlParam("QHTu");
        if (QHTu != "-1" && QHTu.length == 8) {
            $("#txtNgayQuaHan_From").val(QHTu.substring(6, 8) + '/' + QHTu.substring(4, 6) + '/' + QHTu.substring(0, 4));
        }

        var QHDen = Utility.GetUrlParam("QHDen");
        if (QHDen != "-1" && QHDen.length == 8) {
            $("#txtNgayQuaHan_To").val(QHDen.substring(6, 8) + '/' + QHDen.substring(4, 6) + '/' + QHDen.substring(0, 4));
        }

        var Show = Utility.GetUrlParam("Show");
        if (Show != undefined && Show != null && Show != '') {
            if (Show == "1") {
                $("#chkShowCaNhan").attr('checked', 'checked');
            }
            else {
                $("#chkShowCaNhan").removeAttr('checked');
            }
        }
    }

    var state = 1;
    function fnShowLocTinKiem() {
        if (state == 1) {
            $("#trContentLocTimKiem").css("display", "block");
            $("#ai-images").html('<img style ="" src="/images/icons/minus2.gif"><b>&nbsp;Ẩn bộ lọc</b>');
            state = 0;
        } else {
            $("#trContentLocTimKiem").css("display", "none");
            $("#ai-images").html('<img src="/images/icons/plus2.gif"><b>&nbsp;Hiện bộ lọc</b>');

            state = 1;
        }

    }
    function Validate() {
        var value = "1";
        var regexVNP = new RegExp("^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7}))$");
        var txtSoThueBao = $("#txtSoThueBao").val();
        var txtNgayTiepNhan_From = $("#txtNgayTiepNhan_From").val();
        var txtNgayTiepNhan_To = $("#txtNgayTiepNhan_To").val();

        var txtNgayQuaHan_From = $("#txtNgayQuaHan_From").val();
        var txtNgayQuaHan_To = $("#txtNgayQuaHan_To").val();
        if (txtNgayTiepNhan_From != "" && txtNgayTiepNhan_To == "") {
            value = "Vui lòng chọn ngày tiếp nhận khiếu nại (đến ngảy)";
        } else if (txtNgayTiepNhan_From == "" && txtNgayTiepNhan_To != "") {
            value = "Vui lòng chọn ngày tiếp nhận khiếu nại (từ ngảy)";
        } else {
            var batdau = txtNgayTiepNhan_From.split('/');
            var ketthuc = txtNgayTiepNhan_To.split('/');
            var ngaybatdau = batdau[1] + "/" + batdau[0] + "/" + batdau[2];
            var ngayketthuc = ketthuc[1] + "/" + ketthuc[0] + "/" + ketthuc[2];
            if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
                value = "Ngày tiếp nhận:Từ ngày phải nhỏ hơn đến ngày";
            }
        }
        if (txtNgayQuaHan_From != "" && txtNgayQuaHan_To == "") {
            value = "Vui lòng chọn ngày quá hạn (đến ngảy)";
        } else if (txtNgayQuaHan_From == "" && txtNgayQuaHan_To != "") {
            value = "Vui lòng chọn ngày quá hạn (từ ngày)";
        } else {
            var batdau = txtNgayQuaHan_From.split('/');
            var ketthuc = txtNgayQuaHan_To.split('/');
            var ngaybatdau = batdau[1] + "/" + batdau[0] + "/" + batdau[2];
            var ngayketthuc = ketthuc[1] + "/" + ketthuc[0] + "/" + ketthuc[2];
            if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
                value = "Ngày quá hạn:Từ ngày phải nhỏ hơn đến ngày";
            }
        }
        if (txtSoThueBao != "") {
            if (!regexVNP.test("84" + txtSoThueBao)) {
                value = "Không đúng số điện thoại của Vinaphone !";

            }
        }
        return value;
    }
    function fnClearFilter() {

        $("#contentSeach").val('');
        $("#DropLinhVucChung").val('-2');
        $("#DropLinhVucCon").val('-1');
        $("#DropKhieuNai").val('-2');
        $('#DropLoaiKhieuNai').val('-1');
        $('#DropdoUuTien').val('-1');
        $('#DropTrangThai').val('-1');
        $("#txtAutocomLoaiKN").val('');
        $('#DropSelectDate').val('-1');
        $('#DropSelectUserName').val('-1');

        $('#txtSoThueBao').val('');
        $('#txtNgayTiepNhan_From').val('');
        $('#txtNgayTiepNhan_To').val('');
        $('#txtNgayQuaHan_From').val('');
        $('#txtNgayQuaHan_To').val('');
        $('#txtNguoiTiepNhan').val('');
        $('#txtNguoiXuLy').val('');


        fnLocKhieuNai();

    }

    function fnLoadDropPhongBanXuLy() {
        var tabIndex = Utility.GetUrlParam("ctrl");
        if (tabIndex == 'tab2-KNChuyenBoPhanKhac') {
            $("#DropPhongBanXuLy").css('display', 'block');
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=63', '', function (result) {
                if (result != '') {
                    $('#DropPhongBanXuLy').html(result);
                }

            });
        } else {
            $("#DropPhongBanXuLy").css('display', 'none');
        }
    }

    function fnLoadDropdoUuTien() {
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=28', '', function (result) {
            if (result != '') {
                $('#DropdoUuTien').html(result);

                var DoUuTien = Utility.GetUrlParam("DoUuTien");
                if (DoUuTien != "") {
                    $("#DropdoUuTien").val(DoUuTien);
                }
            }

        });
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
    function fnLoadDropLoaiKhieuNai() {
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=25', '', function (result) {
            if (result != '') {
                $('#DropLoaiKhieuNai').html(result);

                if (LoaiKNId_Filter != "") {
                    $("#DropLoaiKhieuNai").val(LoaiKNId_Filter);
                    fnDropLoaiKhieuNaiChange();
                }
            }

        });
    }

    function fnDropKhieuNaiChange() {
        var values = $("#DropKhieuNai").val();
        if (values == '3') {
            $('#btnDongKN').show();
            $('#btnChuyenKNHangLoat').show();
            $('#btnChuyenXuLy').show();
            $('#btnChuyenPhanHoi').hide();
            $('#btnChuyenNgangHang').hide();
            $('#btnTruyVan').hide();
            fnLocKhieuNai();
        } else {
            $('#btnDongKN').show();
            $('#btnChuyenKNHangLoat').hide();
            $('#btnChuyenXuLy').show();
            $('#btnChuyenPhanHoi').show();
            $('#btnChuyenNgangHang').show();
            $('#btnTruyVan').show();
            fnLocKhieuNai();
        }

    }
    function fnDropLoaiKhieuNaiChange() {
        var loaiKhieuNaiId = document.getElementById('DropLoaiKhieuNai').value;
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=26&loaiKhieuNaiId=' + loaiKhieuNaiId, '', function (result) {
            if (result != '') {
                $('#DropLinhVucChung').html(result);
                if (LinhVucChungId_Filter != "") {

                    if (backPage) {
                        $("#DropLinhVucChung").val(LinhVucChungId_Filter);
                    }

                    fnDropLinhVucChungChange();
                }
            }

        });
    }
    function fnDropLinhVucChungChange() {
        $('#DropLinhVucChung').show();
        $('#DropLinhVucCon').show();
        var linhVucChungId = document.getElementById('DropLinhVucChung').value;
        if (linhVucChungId == "-1") {
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=29&linhVucChungId=-1', '', function (result) {
                if (result != '') {
                    $('#DropLinhVucCon').html(result);
                    //$("#DropLinhVucCon").val(linhVucCon);
                }
            });
        }
        else {
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=27&linhVucChungId=' + linhVucChungId, '', function (result) {
                if (result != '') {
                    $('#DropLinhVucCon').html(result);
                    if (backPage && LinhVucConId_Filter != "") {
                        $("#DropLinhVucCon").val(LinhVucConId_Filter);
                        if (LoaiKNId_Filter != "" && LoaiKNId_Filter != -1) {
                            fnLocKhieuNai();
                        }
                    }
                }

            });
        }
    }

    function AutocompleteNguoiTiepNhan() {
        $("#txtNguoiTiepNhan").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    return {
                        data: row,
                        value: index.toString(),
                        result: row.TenTruyCap,
                        id: row.id
                    };
                });
            },
            formatItem: function (item) {
                return format(item);
            }
        }).result(function (e, item) {
            $("#txtNguoiTiepNhan").val(item.TenTruyCap);
        });

        $("#txtNguoiTiepNhan").focus();
    }
    function AutocompleteNguoiXuLy() {
        $("#txtNguoiXuLy").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    return {
                        data: row,
                        value: index.toString(),
                        result: row.TenTruyCap,
                        id: row.id
                    };
                });
            },
            formatItem: function (item) {
                return format(item);
            }
        }).result(function (e, item) {
            $("#txtNguoiXuLy").val(item.TenTruyCap);
        });

        $("#txtNguoiXuLy").focus();
    }

    function AutocompleteNguoiTienXuLy() {
        $("#txtNguoiTienXuLy").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
            dataType: "json",
            width: 300,
            max: 15,
            parse: function (data) {
                return $.map(data, function (row, index) {
                    return {
                        data: row,
                        value: index.toString(),
                        result: row.TenTruyCap,
                        id: row.id
                    };
                });
            },
            formatItem: function (item) {
                return format(item);
            }
        }).result(function (e, item) {
            $("#txtNguoiTienXuLy").val(item.TenTruyCap);
        });

        $("#txtNguoiTienXuLy").focus();
    }

    function format(item) {
        return "<span class='ac_keyword'>" + item.TenTruyCap + "</span>";
    }
</script>
<table style="border: 1px solid #d2d2d2; border-collapse: collapse; width: 100%">
    <tbody>
        <tr>
            <td bgcolor="#f0f0f0" style="text-align: left">
                <table class="nobor" cellspacing="0" cellpadding="0" style="width: 100%">
                    <tr>
                        <td style="width: 30%">
                            <h3 style="color: #3c78b5; line-height: 30px; padding-left: 15px; float: left;">Lọc tìm kiếm</h3>
                        </td>
                        <td style="width: 40%; text-align: center">
                            <span id="spanThongBao" style="color: red; font-weight: bold; display: none; line-height: 30px; padding-left: 15px;">Thêm mới thành công.</span>
                        </td>
                        <td style="width: 30%">
                            <span style="color: #3c78b5; line-height: 30px; padding-right: 5px; float: right;"><a
                    id="ai-images" href="javascript:fnShowLocTinKiem();"></a></span>
                        </td>
                    </tr>
                </table>
                
                
                
                
            </td>
        </tr>
        <tr id="trContentLocTimKiem" style="background: #fffff0; display: none;">
            <td>
                <table width="100%" border="0">
                    <tbody>
                        <tr>
                            <td style="text-align: left; width: 190px;">
                                <div class="inputstyle">
                                    <div class="bg">
                                        <input id="contentSeach" placeholder="Nhập nội dung phản ảnh..." style="width: 190px; height: 25px;" />
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left; width: 600px;">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <select id="DropKhieuNai" onchange="javascript:fnDropKhieuNaiChange();" style="width: 190px;">
                                            <%=strFilterKhieuNai %>
                                        </select>
                                        <select id="DropdoUuTien" style="width: 190px;">
                                        </select>
                                        <select id="DropTrangThai" style="width: 190px;">
                                        </select>
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left"></td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <input id="txtAutocomLoaiKN" placeholder="Nhập lựa chọn..." value="Nhập lựa chọn..." style="width: 190px; height: 25px;" />

                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <select id="DropLoaiKhieuNai" onchange="javascript:fnDropLoaiKhieuNaiChange();" style="width: 190px;">
                                        </select>
                                        <select id="DropLinhVucChung" onchange="javascript:fnDropLinhVucChungChange();" style="width: 190px;">
                                            <option value="-1">--Lĩnh vực chung--</option>
                                            <option value="0">--Trống--</option>
                                        </select>
                                        <select id="DropLinhVucCon" style="width: 190px;">
                                            <option value="-1">--Lĩnh vực con--</option>
                                            <option value="0">--Trống--</option>
                                        </select>

                                    </div>
                                </div>
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
                                        <input id="txtSoThueBao" class="typeNumber" placeholder="Số thuê bao..." value="Số thuê bao..." style="width: 190px; height: 25px;" />
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <input id="txtNguoiTiepNhan" placeholder="Người tiếp nhận..." value="Người tiếp nhận..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                        <input id="txtNguoiTienXuLy" placeholder="Người tiền xử lý..." value="Người tiền xử lý..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                        <input id="txtNguoiXuLy" placeholder="Người xử lý..." value="Người xử lý..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left">
                                <a style="" onclick="Javascript:fnClearFilter();" class="button_clear_filter">Xóa điều
                                    kiện lọc</a>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Ngày tiếp nhận
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <input id="txtNgayTiepNhan_From" placeholder="Từ ngày..." value="Từ ngày..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                        <input id="txtNgayTiepNhan_To" placeholder="Đến ngày..." value="Đến ngày..." style="width: 188px; height: 25px; float: left; margin-right: 3px;" />
                                        <select id="DropPhongBanXuLy" style="width: 190px; display: none; margin-top: 1px; float: left; margin-right: 3px;">
                                            <option value="-1">--Phòng ban xử lý--</option>
                                        </select>
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: left; vertical-align: top" rowspan="2">
                                <div id="divShowCaNhan">
                                    <input id="chkShowCaNhan" type="checkbox" value="1" title="Ẩn hiện khiếu nại đã có người xử lý trong phòng ban" />
                                    <b>&nbsp;&nbsp;&nbsp;Ẩn KN đã có người xử lý trong phòng ban</b>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Ngày quá hạn
                            </td>
                            <td style="text-align: left">
                                <div class="selectstyle">
                                    <div class="bg">
                                        <input id="txtNgayQuaHan_From" placeholder="Từ ngày..." value="Từ ngày..." style="width: 188px; height: 25px;" />
                                        <input id="txtNgayQuaHan_To" placeholder="Đến ngày..." value="Đến ngày..." style="width: 188px; height: 25px;" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
