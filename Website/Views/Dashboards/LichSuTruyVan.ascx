<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LichSuTruyVan.ascx.cs"
    Inherits="Website.Views.Dashboards.LichSuTruyVan" %>
<script type="text/javascript">
    $(document).ready(function () {
        fnLoadDanhSachSuTruyVan();
    });
    function AppendData(dataJson, Id) {
        $.post('/Views/QLKhieuNai/Handler/HandlerTruyVan.ashx?key=2' + '&startPageIndex=1&pageSize=' + pageSize, { data: dataJson },
        function (totalRecords) {
            if (totalRecords != '') {
                $("#total_" + Id).html(addCommas(totalRecords));
            }

        });
    }
    function pageselectCallbackLichSuTruyVan(page_index) {
        var curentPages = page_index + 1;
        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=7'
                + '&pageSize=' + pageSize
                + '&startPageIndex=' + curentPages, '', function (result) {
                    if (result != '') {
                        var strData = '';
                        var dataJson = eval(result);
                        for (var i = 0, len = dataJson.length; i < len; ++i) {
                            var dataTruyVan = dataJson[i].Data;
                            var Id = dataJson[i].Id;
                            var Name = dataJson[i].Name;
                            if (i % 2 == 0) {
                                strData += "<tr id =\"row-" + Id + "\" class=\"rowA\">";
                            }
                            else {
                                strData += "<tr id =\"row-" + Id + "\" class=\"rowB\">";
                            }

                            strData += "        <td class =\"nowrap\" align=\"left\"><a href=\"/Views/QLKhieuNai/TruyVan.aspx?id=" + Id + "\" title=\"Chọn tiêu trí truy vấn\">" + Name + "</a></td>";

                            strData += "        <td id =\"total_" + Id + "\" align=\"center\">" + AppendData(dataTruyVan, Id) + "</td>";

                            strData += " </tr>";

                        }
                        $('#grid-data-SuTruyVan').html(strData);
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
        $.getJSON('/Views/Dashboards/Handler/Handler.ashx?key=8'
                + '&pageSize=' + pageSize
                + '&startPageIndex=1', '',
                function (totalRecords) {
                    if (totalRecords != '') {
                        if (totalRecords == 0) {
                            $("#PaginationLichSu").pagination(0, optInit);
                        }
                        else {
                            $("#PaginationLichSu").pagination(totalRecords, optInit);
                        }
                        $("#divTotalRecordsLichSu").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                    }

                });
    }
</script>
<table class="tbl_style_dashboard" cellspacing="0" cellpadding="0" style="width: 100%;">
    <tr>
        <th class="thead-colunm" style="padding-left: 5px;">Tiêu đề
        </th>
        <th class="thead-colunm" style="text-align: center; width: 150px;">Tổng số
        </th>
    </tr>
    <tbody id="grid-data-SuTruyVan">
    </tbody>
</table>
<table class="nobor" style="width: 100%">
    <tr>
        <td>
            <div id="divTotalRecordsLichSu" style="width: 150px; float: left; margin-top: 5px; text-align: left;">
            </div>
        </td>
        <td style="text-align: right">
            <div id="PaginationLichSu" class="pagination" style="float: right; margin-right: 5px;">
            </div>
        </td>
    </tr>
</table>

