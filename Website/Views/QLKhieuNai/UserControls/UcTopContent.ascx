<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTopContent.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.UcTopContent" %>
<div id="dvTop">
    <ul class="dvqlkhieunai-top">
        <li class="liItem-right"><a id="tab7" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab7-KNDaPhanHoi" title="Khiếu nại đã phản hồi">KN đã phản hồi <span id="totalDaPhanHoi"></span></a></li>
        <li class="liItem"><a id="tab5" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab5-KNCanDong" title="Khiếu nại đã quá hạn phòng ban xử lý">KN quá hạn <span id="totalCanDong"></span></a></li>
        <li class="liItem"><a id="tab4" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab4-KNSapQuaHan" title="Khiếu nại sắp quá hạn phòng ban xử lý">KN sắp quá hạn <span id="totalQuaHan"></span></a></li>
        <li class="liItem"><a id="tab3" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab3-KNBoPhanKhacChuyenVe" title="Khiếu nại bộ phận khác chuyển về phòng ban">KN BP khác chuyển về<span id="totalBoPhanKhacChuyenVe"></span></a></li>
        <li class="liItem"><a id="tab2" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab2-KNChuyenBoPhanKhac" title="Khiếu nại đã chuyển cho các bộ phận khác">KN chuyển BP khác <span id="totalChuyenBoPhanKhac"></span></a></li>
        <li class="liItem"><a id="tab1" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab1-KNChoXuLy" title="Khiếu nại đang chờ xử lý">KN chờ xử lý <span id="totalChoXuLy"></span></a></li>
        <li class="liItem"><a id="tab6" class="" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab6-KNTongHopChoXuLy" title="Tổng hợp tất cả khiếu nại chờ xử lý">Tất cả KN <span id="totalTongHopChoXuLy"></span></a></li>
        <li class="liItem"><a id="tab0" href="/Views/QLKhieuNai/QuanLyKhieuNai.aspx?ctrl=tab0-NhomKNChoXL">Trang chủ <span id="totalThongKe"></span></a></li>
    </ul>
</div>
<script type="text/javascript">
    var pageSize = 10;
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
    function fnTab2Click(url) {
        var param = fnGetUrlParameter('catid');
        if (param != '') {
            window.location.href = url + '&catid=' + param;
        }
    }

    function fnGetTotalTab() {
        $.ajax({
            type: "GET",
            url: "/Views/QLKhieuNai/Handler/Handler.ashx?key=10000",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                if (data != "-1") {
                    var arrData = data.split('|');
                    if (arrData.length == 8) {
                        $('#totalTongHopChoXuLy').html("(" + arrData[0].substring(1) + ")");
                        $('#totalChoXuLy').html("(" + arrData[1] + ")");
                        $('#totalChuyenBoPhanKhac').html("(" + arrData[2] + ")");
                        $('#totalBoPhanKhacChuyenVe').html("(" + arrData[3] + ")");
                        $('#totalQuaHan').html("(" + arrData[4] + ")");
                        $('#totalCanDong').html("(" + arrData[5] + ")");
                        $('#totalDaPhanHoi').html("(" + arrData[6] + ")");
                    }
                }
            }
        });
    }

    function fnGetTotalKhieuNai(key, tag) {
        //$.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=' + key + '&IsCache=' + cache, '', function (totalRecords) {
        //    if (totalRecords != '') {
        //        $('#' + tag).html("(" + totalRecords + ")");
        //    }
        //});
        $.ajax({
            type: "GET",
            url: "/Views/QLKhieuNai/Handler/Handler.ashx?key=" + key,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                if (data != "-1") {
                    var arrData = data.split('|');
                    if (arrData.length == 8) {
                        $('#totalTongHopChoXuLy').html("(" + arrData[0].substring(1) + ")");
                        $('#totalChoXuLy').html("(" + arrData[1] + ")");
                        $('#totalChuyenBoPhanKhac').html("(" + arrData[2] + ")");
                        $('#totalBoPhanKhacChuyenVe').html("(" + arrData[3] + ")");
                        $('#totalQuaHan').html("(" + arrData[4] + ")");
                        $('#totalCanDong').html("(" + arrData[5] + ")");
                        $('#totalDaPhanHoi').html("(" + arrData[6] + ")");
                    }
                }
            }
        });
    }

    $(document).ready(function () {
        var param = fnGetUrlParameter('ctrl');
        if (param != '') {
            var arr = param.split("-");
            $("a").removeClass("liItem-select");
            $('#' + arr[0]).addClass("liItem-select");
        } else {
            $("a").removeClass("liItem-select");
            $('#tab0').addClass("liItem-select");
        }

        var typeSearch = fnGetUrlParameter('TypeSearch');
        if (typeSearch == "-1") {
            $("#tab0").attr("href", $("#tab0").attr("href") + "&TypeSearch=-1");
            $("#tab1").attr("href", $("#tab1").attr("href") + "&TypeSearch=-1");
            $("#tab2").attr("href", $("#tab2").attr("href") + "&TypeSearch=-1");
            $("#tab3").attr("href", $("#tab3").attr("href") + "&TypeSearch=-1");
            $("#tab4").attr("href", $("#tab4").attr("href") + "&TypeSearch=-1");
            $("#tab5").attr("href", $("#tab5").attr("href") + "&TypeSearch=-1");
            $("#tab6").attr("href", $("#tab6").attr("href") + "&TypeSearch=-1");
            $("#tab7").attr("href", $("#tab7").attr("href") + "&TypeSearch=-1");
        }

        if (param == "" || param == "tab0-NhomKNChoXL") {
            fnGetTotalTab();
        }
        else if (param == "tab0-KNPhanViec") {
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

    });

</script>
