<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListKNCanhBao.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.ListKNCanhBao" %>
<script type="text/javascript">
    var pageSizeCanhBao = 10;
    function pageselectCanhBaoCallback(page_index) {
        var curentPages = page_index + 1;
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=50&pageSize=' + pageSizeCanhBao + '&startPageIndex=' + curentPages, '', function (result) {
            if (result != '') {
                $('#grid-data-canhbao').html(result);
            }
        });

        return false;
    }
    function getOptionsFromFormCanhBao() {
        var opt = { callback: pageselectCanhBaoCallback };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }
    $(document).ready(function () {

        var startPageIndex = 1;
        var optInit = getOptionsFromFormCanhBao();
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=49&pageSize=' + pageSizeCanhBao + '&startPageIndex=' + startPageIndex, '', function (totalRecords) {
            if (totalRecords != '') {
                if (totalRecords == 0) {
                    $("#Pagination-canhbao").pagination(0, optInit);
                }
                else {
                    $("#Pagination-canhbao").pagination(totalRecords, optInit);
                }
                //$("#Pagination-canhbao").pagination(totalRecords, optInit);
                $("#divCanhBaoTotalRecords").html('Khiếu nại cảnh báo:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
            }
            
        });

    });
  
</script>
<div id="divCanhBaoTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;
    font-weight: bold;">
</div>
<table width="100%" class="tbl_style" cellspacing="0" cellpadding="0">
  
    <thead class="grid-data-thead">       
        <th style="text-align: center; width:120px;" class="thead-colunm">
            Mã PA/KN
        </th>
        <th style="text-align: center;" class="thead-colunm">
            Số thuê bao
        </th>
         <th style="text-align: center;" class="thead-colunm">
            Loại khiếu nại
        </th>
         <th style="text-align: center;" class="thead-colunm">
            Người xử lý trước
        </th>
        <th style="text-align: center;" class="thead-colunm">
            Người xử lý
        </th>
    </thead>
    <tbody id="grid-data-canhbao">
    </tbody>
</table>
<div class="div-clear" style="height: 10px;">
</div>
<div id="Pagination-canhbao" class="pagination" style="float: right; margin-right: -5px;">
</div>
<div class="div-clear">
</div>
