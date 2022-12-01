<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNChuyenBoPhanKhac.ascx.cs"
    Inherits="Website.Views.QLKhieuNai.UserControls.KNChuyenBoPhanKhac" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcFillter.ascx" TagName="UcFillter"
    TagPrefix="UcFillter" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/PopupChoXuLy.ascx" TagName="PopupChoXuLy"
    TagPrefix="PopupChoXuLy" %>


<script type="text/javascript">
    $(document).ready(function () {

        keyTotal = 6;
        keyGetHTML = 7;
        keyExcel = 2;

        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();

        $('#chkShowCaNhan').removeAttr('checked');
    });
</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>

    <ul>
        <li style="background: none;"><span id="titleChuyenBoPhanKhac" style="color: #4D709A; font-size: 15px; font-weight: bold;">Danh sách khiếu nại chuyển bộ phận khác</span></li>
        <li id="btnExportExcelChiTiet" style="float: right;"><a href="javascript:fnExportExcelChiTiet();"><span class="ex_excel">Xuất chi tiết</span></a> </li>
        <li id="btnExportExcel" style="float: right;"><a href="javascript:fnExportExcel();"><span class="ex_excel">Xuất Excel</span></a> </li>
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
                    <UcFillter:UcFillter ID="UcFillter1" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr valign="top">
                <td>
                    <div id="divNote" style="width: 400px; float: left; margin-top: 5px;">
                        <table border="0" cellspacing="2" cellpading="2">
                            <tr>
                                <td><p style="border: 1pt solid #CCC; background: #FF0000; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span></td>
                                <td><p style="border: 1pt solid #CCC; background: #FFFF00; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span></td>
                                <td><p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span></td>
                                <td><p style="border: 1pt solid #CCC; background: #088A08; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN đã đóng</span></td>
                            </tr>
                            <tr>
                                <td colspan="2"><p style="border: 1pt solid #CCC; background: #FF8000; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN BP khác chuyển về</span></td>
                                <td colspan="2"><p style="border: 1pt solid #CCC; background: #999; width: 22px; height: 13px; float: left;"></p>
                        <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN quá hạn</span></td>
                            </tr>
                        </table>
                        
                    </div>
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
                                    <option value="500">500</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div id="divTotalRecords" style="width: 150px; float: right; margin-top: 5px; text-align: right;">
                    </div>
                    <%--
                    <div id="divScroll" style='overflow-x: scroll; height: auto;'>
                        <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 2150px;'>

                            <thead class="grid-data-thead">                               
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">STT
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Trạng thái
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Mã PA/KN
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Độ ưu tiên
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;">Số thuê bao
                                </th>
                                <th class="thead-colunm" style="padding-left: 5px;">Nội dung
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Loại khiếu nại
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực chung
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực con
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người tiếp nhận
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người tiền xử lý
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 150px;">Phòng ban xử lý
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người xử lý
                                </th>
                                <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Ngày tiếp nhận
                                </th>
                                <th class="thead-colunm-end" style="color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;">Ngày quá hạn PB
                                </th>
                                <th class="thead-colunm-end" style="color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;">Ngày quá hạn TT
                                </th>
                                
                            </thead>
                            <tbody id="grid-data">
                            </tbody>
                        </table>
                    </div>--%>
                    <div class="div-clear" style="height: 10px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <table class="nobor">
        <tr>
            <td>
                <div id="divScroll" style="height: 370px; width: 100%;">
                    <table class="flex_KNChoXuLy" style="display: none"></table>
                </div>
            </td>
        </tr>
    </table>
</div>
<PopupChoXuLy:PopupChoXuLy runat="server" ID="PopupChoXuLy1" />
