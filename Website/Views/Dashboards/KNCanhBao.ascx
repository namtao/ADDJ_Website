<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNCanhBao.ascx.cs" Inherits="Website.Views.Dashboards.KNCanhBao" %>
<script type="text/javascript">
    function getOptionsFromForm_KNCanhBao() {
        var opt = { callback: pageselectCallback_KNCanhBao };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function pageselectCallback_KNCanhBao(page_index) {
        var curentPages = page_index + 1;
        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=6&pageSize=' + pageSize + '&startPageIndex=' + curentPages, '', function (result) {
            if (result != '') {
                $('#grid-data-KNCanhBao').html(result);

                TestNhay();
            }
        });
        return false;
    }
    function fnLocKhieuNai_KNCanhBao() {
        var optInit = getOptionsFromForm_KNCanhBao();
        var startPageIndex = 1;
        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=5&pageSize=' + pageSize + '&startPageIndex=' + startPageIndex, '', function (totalRecords) {
            if (totalRecords != '') {
                if (totalRecords == 0) {
                    $("#Pagination-KNCanhBao").pagination(0, optInit);
                }
                else {
                    $("#Pagination-KNCanhBao").pagination(totalRecords, optInit);
                }
                $("#divTotalRecords-KNCanhBao").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
            }

        });
    }
</script>
<table class="tbl_style_dashboard" cellspacing="0" cellpadding="0" style="width: 100%;">
    <tr>
        <th style="text-align: center; width: 120px;" class="thead-colunm">Mã PA/KN
        </th>
        <th style="text-align: center;" class="thead-colunm">Số thuê bao
        </th>
        <th style="text-align: center;" class="thead-colunm">Loại khiếu nại
        </th>
        <th style="text-align: center;" class="thead-colunm">Người xử lý trước
        </th>
        <th style="text-align: center;" class="thead-colunm">Người xử lý
        </th>
    </tr>
    <tbody id="grid-data-KNCanhBao">
    </tbody>
</table>
<table class="nobor" style="width: 100%">
    <tr>
        <td>
            <div id="divTotalRecords-KNCanhBao" style="width: 150px; float: left; margin-top: 5px; text-align: left;">
            </div>
        </td>
        <td style="text-align: right">
            <div id="Pagination-KNCanhBao" class="pagination" style="float: right; margin-right: 5px;">
            </div>
        </td>
    </tr>
</table>
<style type="text/css">
    .class-add-ContentBlock { position: relative; }
    div.ContentBlock-hide { background: none repeat scroll 0 0 #F9FDFC; border-bottom-left-radius: 3px; border-bottom-right-radius: 3px; box-shadow: 0 0 2px #AAAAAA; margin-right: 1px; margin-top: 15px; padding: 5px; position: absolute; right: 0; top: 2px; width: 300px; height: 200px; z-index: 99900; }
</style>
