<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNChoXuLy.ascx.cs" Inherits="Website.Views.Dashboards.KNChoXuLy" %>
<script type="text/javascript">
    function getOptionsFromForm_KNChoXuLy() {
        var opt = { callback: pageselectCallback_KNChoXuLy };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function pageselectCallback_KNChoXuLy(page_index) {
        var curentPages = page_index + 1;
        var contentSeach = '';
        var typeSearch = 1;
        var doUuTien = -1;
        var loaiKhieuNai = -1;
        var linhVucChung = -1;
        var linhVucCon = -1;
        var trangThai = -1;
        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=4'
                + '&contentSeach=' + contentSeach
                + '&typeSearch=' + typeSearch
                + '&doUuTien=' + doUuTien
                + '&trangThai=' + trangThai
                + '&loaiKhieuNai=' + loaiKhieuNai
                + '&linhVucChung=' + linhVucChung
                + '&linhVucCon=' + linhVucCon
                + '&pageSize=' + pageSize
                + '&startPageIndex=' + curentPages, '', function (result) {
                    if (result != '') {
                        $('#grid-data-KNChoXuLy').html(result);

                        TestNhay();

                        $('.aui-dropdown-trigger').click(
                            function () {
                                $("#rightMenu").stop().slideToggle(500);
                            }
                        );
                    }
                });
        return false;
    }
    function fnLocKhieuNaiDangXuly() {
        var optInit = getOptionsFromForm_KNChoXuLy();
        var contentSeach = '';
        var typeSearch = 1;
        var doUuTien = -1;
        var loaiKhieuNai = -1;
        var linhVucChung = -1;
        var linhVucCon = -1;
        var trangThai = -1;

        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=3'
                + '&contentSeach=' + contentSeach
                + '&typeSearch=' + typeSearch
                + '&doUuTien=' + doUuTien
                + '&trangThai=' + trangThai
                + '&loaiKhieuNai=' + loaiKhieuNai
                + '&linhVucChung=' + linhVucChung
                + '&linhVucCon=' + linhVucCon
                + '&pageSize=' + pageSize
                + '&startPageIndex=1', '',
                function (totalRecords) {
                    if (totalRecords != '') {
                        if (totalRecords == 0) {
                            $("#Pagination-KNChoXuLy").pagination(0, optInit);
                        }
                        else {
                            $("#Pagination-KNChoXuLy").pagination(totalRecords, optInit);
                        }
                        $("#divTotalRecords-KNChoXuLy").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                    }

                });
    }
    var trangthai = 1;
    function ShowContent(id) {
        if (trangthai == 1) {
            var html = '<div class ="ContentBlock-hide"></div>';
            $("#divContentBlock_" + id).html(html);
            trangthai = 0;
        } else {
            $(".ContentBlock-hide").css("display", "none");
            $("#divContentBlock_" + id).html('');
            trangthai = 1;
        }
    }
</script>
<table class="tbl_style_dashboard" cellspacing="0" cellpadding="0" style="width: 100%;">
    <tr>
        <th class="thead-colunm" style="padding-left: 5px;">Mã PA/KN
        </th>
        <th class="thead-colunm" style="text-align: center;">Độ ưu tiên
        </th>
        <th class="thead-colunm" style="text-align: center;">Số thuê bao
        </th>
        <th class="thead-colunm" style="text-align: center;">Ngày quá hạn
        </th>
        <th class="thead-colunm" style="text-align: center;"></th>
    </tr>
    <tbody id="grid-data-KNChoXuLy">
    </tbody>
</table>
<table class="nobor" style="width: 100%">
    <tr>
        <td>
            <div id="divTotalRecords-KNChoXuLy" style="width: 150px; float: left; margin-top: 5px; text-align: left;"></div>
        </td>
        <td style="text-align: right">
            <div id="Pagination-KNChoXuLy" class="pagination" style="float: right; margin-right: 5px;"></div>
        </td>
    </tr>
</table>
