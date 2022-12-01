<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNCanDong.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.KNCanDong" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcFillter.ascx" TagName="UcFillter"
    TagPrefix="UcFillter" %>

<%@ Register Src="~/Views/QLKhieuNai/UserControls/PopupChoXuLy.ascx" TagName="PopupChoXuLy"
    TagPrefix="PopupChoXuLy" %>

<script type="text/javascript">
    $(document).ready(function () {

        keyTotal = 17;
        keyGetHTML = 18;
        keyExcel = 5;

        pageSize = $('#DropPageSize').val();
        fnSetSizeDiv();
    });

</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="background: none;"><span style="color: #4D709A; font-size: 15px; font-weight: bold;">Danh sách khiếu nại cần đóng</span></li>
        <li id="btnExportExcelChiTiet" style="float: right;"><a href="javascript:fnExportExcelChiTiet();"><span class="ex_excel">Xuất chi tiết</span></a> </li>
        <li id="btnExportExcel" style="float: right;"><a href="javascript:fnExportExcel();">
            <span class="ex_excel">Xuất Excel</span></a> </li>
        <li id="btnUpdateKNHangLoat" runat="server" style="float: right;">
            <a href="javascript:UpdateKNHangLoat();"><span class="save">Cập nhật KN hàng loạt</span></a>
        </li>
        <li id="btnDongKN" runat="server" style="float: right;">
            <a href="javascript:ShowPoupDongKhieuNai();"><span class="del_file">Đóng khiếu nại</span></a>
        </li>
        <li id="btnChuyenNgangHang" runat="server" style="float: right;">
            <a href="javascript:ShowPoupChuyenNgangHang();"><span class="move_nganghang">Chuyển ngang hàng</span></a>
        </li>
        <li id="btnChuyenPhanHoi" runat="server" style="float: right;">
            <a href="javascript:ShowPoupChuyenPhanHoi();"><span class="move_phanhoi">Chuyển phản hồi</span></a>
        </li>
        <li id="btnChuyenXuLy" runat="server" style="float: right;">
            <a href="javascript:ShowPoupPhongBan();"><span class="move_file">Chuyển xử lý</span></a>
        </li>
        <li id="btnTiepNhan" runat="server" style="float: right;">
            <a href="javascript:ShowPoupTiepNhanKN();"><span class="save">Tiếp nhận</span></a>
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
                                <td>
                                    <p style="border: 1pt solid #CCC; background: #FF0000; width: 22px; height: 13px; float: left;"></p>
                                    <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ xử lý</span></td>
                                <td>
                                    <p style="border: 1pt solid #CCC; background: #FFFF00; width: 22px; height: 13px; float: left;"></p>
                                    <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Đang xử lý</span></td>
                                <td>
                                    <p style="border: 1pt solid #CCC; background: #0095CC; width: 22px; height: 13px; float: left;"></p>
                                    <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">Chờ đóng</span></td>
                                <td>
                                    <p style="border: 1pt solid #CCC; background: #088A08; width: 22px; height: 13px; float: left;"></p>
                                    <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN đã đóng</span></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <p style="border: 1pt solid #CCC; background: #FF8000; width: 22px; height: 13px; float: left;"></p>
                                    <span style="color: #4D709A; font-size: 12px; font-weight: bold; float: left; padding-left: 5px; padding-right: 5px;">KN BP khác chuyển về</span></td>
                                <td colspan="2">
                                    <p style="border: 1pt solid #CCC; background: #999; width: 22px; height: 13px; float: left;"></p>
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
                    <div id="divTotalRecords" style="width: 200px; float: right; margin-top: 5px; text-align: right;">
                    </div>

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
<PopupChoXuLy:PopupChoXuLy runat="server" id="PopupChoXuLy1" />
