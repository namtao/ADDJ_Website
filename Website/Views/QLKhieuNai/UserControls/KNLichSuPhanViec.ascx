<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNLichSuPhanViec.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.KNLichSuPhanViec" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<script src="/JS/jquery.pagination.js" type="text/javascript"></script>
<link href="/CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script src="/JS/plugin/jquery.datepick.js" type="text/javascript"></script>
<script src="/JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
<script type="text/javascript">
    var pageSize = '';
    $(document).ready(function () {       
        
        $("#TuNgay").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#DenNgay").datepick({ dateFormat: 'dd/mm/yyyy' });
        var now = new Date();
        var dateFrom = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 15);

        var day_from = dateFrom.getDate();
        var month_from = dateFrom.getMonth() + 1;
        var year_from = dateFrom.getFullYear();
        $("#TuNgay").val(day_from + "/" + month_from + "/" + year_from);

        var curr_date = now.getDate();
        var curr_month = now.getMonth() + 1;
        var curr_year = now.getFullYear();
        $("#DenNgay").val(curr_date + "/" + curr_month + "/" + curr_year);
        pageSize = $('#DropPageSize').val();
        fnLoadDropUserName();
        fnLocKhieuNai();
    });

    function pageselectCallback(page_index) {
        var curentPages = page_index + 1;
        var userName = $("#DropUserName").val();
        if (userName == null) {
            userName = -1;
        }
        var tuNgay = $("#TuNgay").val();
        var denNgay = $("#DenNgay").val();

        var urlQuery = '/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=2'
                + '&userName=' + userName
                + '&tuNgay=' + tuNgay
                + '&denNgay=' + denNgay
                + '&pageSize=' + pageSize
                + '&startPageIndex=' + curentPages

        $.ajax({
            beforeSend: function () {
                Common.Loading();
            },
            type: "GET",
            dataType: "json",
            url: urlQuery,
            success: function (result) {
                if (result != '') {
                    $('#grid-data').html(result);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (console && console.log) console.log(textStatus);
            },
            complete: function (xhr, textStatus) {
                Common.UnLoading();
                //if (console && console.log) console.log("Hoàn thành");                            
            }
        });

        //$.getJSON('/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=2'
        //        + '&userName=' + userName
        //        + '&tuNgay=' + tuNgay
        //        + '&denNgay=' + denNgay
        //        + '&pageSize=' + pageSize
        //        + '&startPageIndex=' + curentPages, '', function (result) {
        //            if (result != '') {
        //                $('#grid-data').html(result);
        //            }
        //        });
        return false;
    }
    function getOptionsFromForm() {
        var opt = { callback: pageselectCallback };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function fnLocKhieuNai() {
        var optInit = getOptionsFromForm();
        var userName = $("#DropUserName").val();
        if (userName == null) {
            userName = -1;
        }
        var tuNgay = $("#TuNgay").val();
        var denNgay = $("#DenNgay").val();

        var urlQuery = '/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=1'
                + '&userName=' + userName
                + '&tuNgay=' + tuNgay
                + '&denNgay=' + denNgay
                + '&pageSize=' + pageSize
                + '&startPageIndex=1';

        $.ajax({
            beforeSend: function () {
                Common.Loading();
            },
            type: "GET",
            dataType: "json",
            url: urlQuery,
            success: function (totalRecords) {
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (console && console.log) console.log(textStatus);
            },
            complete: function (xhr, textStatus) {
                Common.UnLoading();
                //if (console && console.log) console.log("Hoàn thành");                            
            }
        });

        //$.getJSON('/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=1'
        //        + '&userName=' + userName
        //        + '&tuNgay=' + tuNgay
        //        + '&denNgay=' + denNgay
        //        + '&pageSize=' + pageSize
        //        + '&startPageIndex=1', '',
                    
        //        function (totalRecords) {
        //            if (totalRecords != '') {
        //                //$("#Pagination").pagination(totalRecords, optInit);
        //                if (totalRecords == 0) {
        //                    $("#Pagination").pagination(0, optInit);
        //                }
        //                else {
        //                    $("#Pagination").pagination(totalRecords, optInit);
        //                }
        //                $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
        //            }

        //        });
    }

    function fnLoadDropUserName() {        
        $.getJSON('/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=3', '', function (result) {
            if (result != '') {
                $('#DropUserName').html(result);
            }

        });
    }

    function fnDropUserNameChange() {
        fnLocKhieuNai();
    }

    function fnDropTypeKhieuNaiChange() {
        fnLocKhieuNai();
    }
    function fnClearFilter() {
        $("#DropUserName").val('-1');

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

    function fnExportExcel() {
        $('.divOpacity').css('display', 'block');
        var pageSizeExp = $("#DropPageSize").val();
        var userName = $("#DropUserName").val();
        if (userName == null) {
            userName = -1;
        }
        var tuNgay = $("#TuNgay").val();
        var denNgay = $("#DenNgay").val();

        $.getJSON('/Views/QLKhieuNai/Handler/LSPhanViec.ashx?key=4'                
                + '&userName=' + userName
                + '&tuNgay=' + tuNgay
                + '&denNgay=' + denNgay
                + '&pageSize=' + pageSizeExp
                + '&startPageIndex=1', '',
                function (result) {
                    if (result != '') {
                        window.location.href = "/ExportExcel/Excel" + result;
                        $('.divOpacity').css('display', 'none');
                    } else {
                        $('.divOpacity').css('display', 'none');
                        MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại','error');
                    }

                });
    }

      

</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="background: none;"><span style="color: #4D709A; font-size: 15px; font-weight: bold;">
            Lịch sử phân việc của phòng 
            <asp:Label ID="txtPhongBan" runat="server" Text=""></asp:Label>
            </span></li>
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
                                        Lọc tìm kiếm</h3>
                                </td>
                            </tr>
                            <tr style="background: #fffff0">
                                <td>
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left">
                                                    <div class="selectstyle">
                                                        <div class="bg">
                                                            <select id="DropUserName" onchange="javascript:fnDropUserNameChange();" style="width: 200px;">
                                                            </select>
                                                            <input style="width: 200px; height: 25px;" placeholder="Từ ngày..." id="TuNgay">
                                                            <input style="width: 200px; height: 25px;" placeholder="Đến ngày..." id="DenNgay">
                                                            <a style="height: 20px; padding-top: 5px;" onclick="Javascript:fnLocKhieuNai();"
                                                                class="btn_style_button">Lọc</a> <a style="" onclick="Javascript:fnClearFilter();"
                                                                    class="button_clear_filter">Xóa điều kiện lọc</a>
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
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr valign="top">
                <td>
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
                    <table width="100%" class="tbl_style" cellspacing="0" cellpadding="0">
                        <thead class="grid-data-thead">
                            <th class="thead-colunm" style="width: 50px;">
                                STT
                            </th>
                             <th style="text-align: left; padding-left: 10px;width: 150px;" class="thead-colunm">
                                Người tiếp nhận
                            </th>
                            <th style="text-align: center; padding-left: 10px;" class="thead-colunm">
                                Tổng số khiếu nại tiếp nhận
                            </th>
                            <th style="text-align: center; padding-left: 10px;" class="thead-colunm">
                                Tổng số khiếu nại đã xử lý
                            </th>
                            <th class="thead-colunm">
                                Tổng số khiếu nại còn lại
                            </th>
                        </thead>
                        <tbody id="grid-data">
                        </tbody>
                    </table>
                    <div class="div-clear" style="height: 10px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="divOpacity" class="divOpacity" style="opacity: 0.4; background: #000; width: 100%;
    position: fixed; left: 0; top: -80px; display: none; z-index: 100;">
</div>
