<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LichSuTruyVan.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.LichSuTruyVan" %>
<script language="javascript">
    var pageSizeLichSuTruyVan = '10';
    $(document).ready(function () {
        fnLoadDanhSachSuTruyVan();
    });
    function pageselectCallbackLichSuTruyVan(page_index) {       
        var curentPages = page_index + 1;
        $.ajax({
            type: "GET",
            url: "/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=3&pageSize=" + pageSizeLichSuTruyVan + "&startPageIndex=" + curentPages,            
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (result) {
                if (result != '') {
                    $('#grid-data-SuTruyVan').html(result);
                }
            }
        });
        return false;
    }
    function getOptionsFromFormLichSuTruyVan() {
        var opt = { callback: pageselectCallbackLichSuTruyVan };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }
    function fnLoadDanhSachSuTruyVan() {
        var optInit = getOptionsFromFormLichSuTruyVan();
        $.getJSON('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=4'
                + '&pageSize=' + pageSizeLichSuTruyVan
                + '&startPageIndex=1', '',
                function (totalRecords) {
                    if (totalRecords != '') {                        
                        if (totalRecords == 0) {
                            $("#PaginationLichSu").pagination(0, optInit);
                        }
                        else {
                            $("#PaginationLichSu").pagination(totalRecords, optInit);
                        }
                        //$("#PaginationLichSu").pagination(totalRecords, optInit);
                        $("#divTotalRecordsLichSu").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                    }

                });
    }
    function fnDeleteLichSuTruyVan(id) {
        $.getJSON('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=6' + '&id=' + id, '', function (result) {
            if (result == '0') {
                MessageAlert.AlertNormal("Xóa không thành công!");
            } else if (result == '-1') {
                MessageAlert.AlertNormal("Lỗi Xóa du liệu!");
            } else {
                fnLoadDanhSachSuTruyVan();
            }
        });
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
    function SelectRow(id) {
        $.getJSON('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=8' + '&id=' + id, '', function (result) {
            if (result != '') {

                var dataJson = jQuery.parseJSON(result);

                dataTruyVan = { items: [] };

                $.each(dataJson.object_list, function (i, item) {
                    dataTruyVan.items.push(
                        { TenTruong: item.TenTruong,TieuDe:item.TieuDe, KieuDuLieu: item.KieuDuLieu, PhepToan: item.PhepToan, GiaTri: item.GiaTri }
                       );
                });
                
                fnBindDataTruyVan();
                fnTruyVan();
                ClosePoupLichSu();
            } else {
                MessageAlert.AlertNormal("Không có thông tin!");
            }
        });
    }
</script>
<div id="PaginationLichSu" class="pagination" style="float: right; margin-right: 0px;">
</div>
<div id="divTotalRecordsLichSu" style="width: 180px; float: right; margin-top: 5px;
    text-align: right;">
</div>
<div style="clear: both; height: 5px;">
</div>
<div id="divDataLichSu">
    <table class="tbl_style" cellspacing="0" cellpadding="0" style="width: 100%;">
        <tr style="line-height: 25px;">
            <th align="center" style="background: #F0F0F0 !important; border: 1pt solid #B2D4E6 !important;
                width: 50px; color: #333" class="thead-colunm">
                STT
            </th>
            <th align="left" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important;
                border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;"
                class="thead-colunm">
                Tiêu đề
            </th>
            <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important;
                border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;
                width: 100px;" class="thead-colunm">
                Người tạo
            </th>
            <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important;
                border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;
                width: 150px;" class="thead-colunm">
                Ngày tạo
            </th>
            <th align="center" style="background: #F0F0F0 !important; color: #333; border-right: 1pt solid #B2D4E6 !important;
                border-top: 1pt solid #B2D4E6 !important; border-bottom: 1pt solid #B2D4E6 !important;
                width: 100px" class="thead-colunm">
                Thao tác
            </th>
        </tr>
        <tbody id="grid-data-SuTruyVan">
        </tbody>
    </table>
</div>
