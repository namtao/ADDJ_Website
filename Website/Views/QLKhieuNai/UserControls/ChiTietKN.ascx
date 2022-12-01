<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChiTietKN.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.ChiTietKN" %>
<style type="text/css">
    #divChiTietKN-Title h1 { font-size: 14px; font-weight: bold; padding-left: 10px; padding-right: 10px; padding-bottom: 5px; border-bottom: 1px solid #CCC; line-height: 33px; margin-bottom: 10px; }
    #divChiTietKN-Info table { border: 1px solid #ccc; background: #F2F2F2; }
    #divChiTietKN-Info table tr { line-height: 30px; }
    #divChiTietKN-Info table tr td { padding-left: 10px; }
    #divChiTietKN-Info .info { font-size: 12px; color: #4D709A; padding-right: 10px; white-space: pre-wrap; line-height: 20px; float: left; }
    #divChiTietKN-Info .info-2 { font-size: 12px; color: #4D709A; padding-right: 10px; white-space: pre-wrap; }
</style>
<script type="text/javascript">

    function fnAddCountCall() {
        //console.log($('#info-MaKhieuNai').html());
        $.messager.confirm('Thông báo', 'Khách hàng gọi lần tiếp theo?', function (resultDialog) {
            if (resultDialog) {
                $.ajax({
                    type: "GET",
                    url: '/Views/ChiTietKhieuNai/Handler/KhieuNai.ashx',
                    data: {
                        type: "AddCountCall",
                        MaKhieuNai: $('#info-MaKhieuNai').html()
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.ErrorId != undefined) {
                            if (data.ErrorId == 0) {
                                $.messager.alert('Thông báo', 'Thêm số lần gọi khách hàng vào khiếu nại thành công.', 'info');
                                $('#spSoLanGoi').html(data.Message);
                            }
                            else
                                $.messager.alert('Thông báo', data.Message, 'error');
                        }
                        else {
                            MessageAlert.AlertNormal('Có lỗi xảy ra.', 'error');
                        }
                    }
                });
            }
        });
    }

    function fnSelectTab(tab) {
        if (tab != '') {
            $("a").removeClass("tabKNInfo-select");
            $('#' + tab).addClass("tabKNInfo-select");
            if (tab == 'tab-info-0') {
                $('#table-thongtinchung').css('display', 'block');
                $('#tabKNInfo-Content').css('display', 'none');
            } else {
                $('#table-thongtinchung').css('display', 'none');

                if (tab == 'tab-info-1') {
                    var iframe = '<iframe frameborder="0" src="/Views/QLKhieuNai/LoadControls.aspx?MaKN=150&Mode=View&ctrl=ucCacBuocXuLy" width="100%">';
                    iframe += '</iframe>';
                    $('#tabKNInfo-Content').html(iframe);
                    $('#tabKNInfo-Content').css('display', 'block');
                } else if (tab == 'tab-info-2') {

                } else if (tab == 'tab-info-3') {

                } else if (tab == 'tab-info-4') {

                } else if (tab == 'tab-info-5') {

                }
            }
        }
    }
</script>

<div id="divChiTietKN-Info" style="">
    <div id="table-thongtinchung">
        <div style="clear: both; height: 1px">
        </div>
        <table cellpadding="0" cellspacing="0" width="100%" style="white-space: nowrap; font-size: 12px; margin-top: -2px;">
            <tr style="line-height: 35px; background: #AEAEAE;">
                <td colspan="4" style="width: 60%;">
                    <span style="font: 14px; font-weight: bold; color: #fff;">Thông tin khiếu nại</span>
                    <span style="font-weight: bold; color: red; font-size: 20px;">
                        <!--padding-left:270px-->
                        Số lần khách hàng gọi: <span id="spSoLanGoi">1</span>
                        <input type="button" id="btnAddCountCall" onclick="fnAddCountCall();" value="Thêm số lần gọi" style="height: 30px;" class="btn_style_button" />
                    </span>
                </td>
                <td colspan="4" style="border-left: 1px solid #ccc; width: 40%;">
                    <span style="font: 14px; font-weight: bold; color: #fff;">Thông tin xử lý</span>
                </td>
            </tr>
            <tr>
                <td>Số thuê bao:
                </td>
                <td>
                    <span class="info" id="info-SoThueBao"></span>
                </td>
                <td>Loại khiếu nại:
                </td>
                <td>
                    <span class="info" id="info-LoaiKhieuNai"></span>
                </td>
                <td style="border-left: 1px solid #ccc;">Người xử lý:
                </td>
                <td>
                    <span class="info" id="info-NguoiXuLy"></span>
                </td>
                <td>T/g cập nhật:
                </td>
                <td>
                    <span class="info" id="info-NgayCapNhat"></span>
                </td>
            </tr>
            <tr>
                <td>Họ tên:
                </td>
                <td>
                    <span class="info" id="info-HoTen"></span>
                </td>
                <td>Lĩnh vực chung:
                </td>
                <td style="line-height: 15px;">
                    <span class="info-2" id="info-LinhVucChung"></span>
                </td>
                <td style="border-left: 1px solid #ccc;">Độ ưu tiên:
                </td>
                <td>
                    <span class="info" id="info-DoUuTien"></span>
                </td>
                <td>Quá hạn PB:
                </td>
                <td>
                    <%--<span class="info" id="info-ThoiHan"></span>--%>
                    <span class="info" id="info-QuaHanPB"></span>
                </td>
            </tr>
            <tr>
                <td>Điện thoại liên hệ:
                </td>
                <td>
                    <span class="info" id="info-DienThoaiLienHe"></span>
                </td>
                <td>Lĩnh vực con:
                </td>
                <td style="line-height: 15px;">
                    <span class="info-2" id="info-LinhVucCon"></span>
                </td>
                <td style="border-left: 1px solid #ccc;">Người tiếp nhận:
                </td>
                <td>
                    <span class="info" id="info-NguoiTiepNhan"></span>
                </td>
                <td>T/g tiếp nhận:
                </td>
                <td>
                    <span class="info" id="info-NgayTiepNhan"></span>
                </td>
            </tr>
            <tr>
                <td>T/g xảy ra sự cố:
                </td>
                <td>
                    <span class="info" id="info-ThoiGianXayRaSuCo"></span>
                </td>
                <td>Hình thức tiếp nhận
                </td>
                <td>
                    <span class="info" id="info-HTTiepNhan"></span>
                </td>
                <td style="border-left: 1px solid #ccc;">Trạng thái:
                </td>
                <td>
                    <span class="info" id="info-TrangThai"></span>
                </td>
                <td>T/g đóng:
                </td>
                <td>
                    <span class="info" id="info-NgayDong"></span>
                </td>
            </tr>
            <tr>
                <td>Tỉnh/Thành xảy ra sự cố
                </td>
                <td>
                    <span class="info" id="info-TinhThanhXayRaSuCo"></span>
                </td>
                <td>Quận/Huyện xảy ra sự cố
                </td>
                <td>
                    <span class="info" id="info-QuanHuyenXayRaSuCo"></span>
                </td>
                <td style="border-left: 1px solid #ccc;">&nbsp;
                </td>
                <td>&nbsp;
                </td>
                <td>Quá hạn TT:
                </td>
                <td>
                    <span class="info" id="info-QuaHanTT"></span>
                </td>
            </tr>
            <tr>
                <td>Phường/Xã xảy ra sự cố
                </td>
                <td>
                    <span class="info" id="info-PhuongXaXayRaSuCo"></span>
                </td>
                <td></td>
                <td></td>
                <td style="border-left: 1px solid #ccc;">Ghi chú:
                </td>
                <td colspan="3">
                    <span class="info" id="info-GhiChu"></span>
                </td>
            </tr>
            <tr>
                <td>Địa điểm xảy ra sự cố:
                </td>
                <td colspan="3">
                    <span class="info" id="info-DiaDiemXayRaSuCo"></span>
                </td>

                <td style="border-left: 1px solid #ccc;" valign="top">File GQKN gửi:
                </td>
                <td colspan="3" valign="top" style="line-height: 15px;">
                    <span class="info" id="info-FileGQKNGui"></span>
                </td>
            </tr>
            <tr>
                <td valign="top">Địa chỉ:
                </td>
                <td colspan="3" style="white-space: pre-wrap; line-height: 15px;">
                    <span class="info" id="info-DiaChi"></span>
                </td>
                <td>Độ hài lòng:
                </td>
                <td colspan="3">
                    <span class="info" id="info-TenDoHaiLong"></span>
                </td>
            </tr>
            <tr>
                <td valign="top">File KH gửi:
                </td>
                <td colspan="3" valign="top">
                    <span class="info" id="info-FileKHGui"></span>
                </td>

            </tr>
            <tr>
                <td>Nguyên nhân lỗi
                </td>
                <td colspan="3">
                    <span class="info" id="info-NguyenNhanLoi"></span>
                </td>
                <td>Chi tiết lỗi
                </td>
                <td colspan="3">
                    <span class="info" id="info-ChiTietLoi"></span>
                </td>
            </tr>
            <tr>
                <td valign="top">Nội dung phản ánh:
                </td>
                <td colspan="7" valign="top">
                    <div id="info-NoiDungPhanAnh" style="margin: 0 5px 5px 5px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">Nội dung đóng khiếu nại:
                </td>
                <td colspan="7" valign="top">
                    <div id="info-NoiDungXuLyDongKN" style="margin: 0 5px 5px 5px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="8" valign="top" style="padding-left: 0px;">
                    <div id="tabKNInfo-Content">
                        <%--<iframe frameborder="0" src="<%= ResolveClientUrl("~/") %>Views/QLKhieuNai/LoadControls.aspx?MaKN=150&Mode=View"
                            width="100%" height="250px"></iframe>--%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
