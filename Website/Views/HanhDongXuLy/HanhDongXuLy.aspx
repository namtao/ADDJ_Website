<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true"
    CodeBehind="HanhDongXuLy.aspx.cs" Inherits="Website.Views.HanhDongXuLy.HanhDongXuLy" %>

<%@ Register Src="~/Views/QLKhieuNai/UserControls/ChiTietKN.ascx" TagName="ChiTietKN"
    TagPrefix="ChiTietKN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/atooltip.css" rel="stylesheet" type="text/css" />
    <script src="../../JS/plugin/jquery.datepick.js" type="text/javascript"></script>
    <script src="../../JS/plugin/jquery.datepick-vi.js" type="text/javascript"></script>
    <script src="/JS/jquery.pagination.js" type="text/javascript"></script>
    <script src="/JS/jquery.atooltip.js" type="text/javascript"></script>
    <%--Su dung Autocomplate--%>
    <link href="/CSS/autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/JS/jquery.autocomplete.js" type="text/javascript"></script>
    <%--Su dung Autocomplate--%>
    <style type="text/css">
        #grid-data td.nowrap { white-space: nowrap; }

        .autocomplete-suggestions { border: 1px solid #999; background: #FFF; cursor: default; overflow: auto; -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); }

        .autocomplete-suggestion { padding: 2px 5px; white-space: nowrap; overflow: hidden; }

        .autocomplete-selected { background: #F0F0F0; }

        .autocomplete-suggestions strong { font-weight: normal; color: #3399FF; }
    </style>
    <script type="text/javascript">
        var pageSize = '';
        var PhongBanId = "<%=strPhongBanId%>";
        var Username = "<%=strUsername%>";
        $(document).ready(function () {
            pageSize = $('#DropPageSize').val();
            fnSetSizeDiv();

            fnLoadDropPhongBanXuLy();


            //fnLocKhieuNai();

            AutocompleteNguoiTiepNhan();
            AutocompletemaKhieuNai();
            AutocompleteNguoiXuLy();
            $("#lbtDongTruyVan").hide();

            $("#txtThoiGianCapNhat_From").datepick({ dateFormat: 'dd/mm/yyyy' });
            $("#txtThoiGianCapNhat_To").datepick({ dateFormat: 'dd/mm/yyyy' });
        });


        function fnLoadDropPhongBanXuLy() {
            $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=63', '', function (result) {
                if (result != '') {
                    $('#DropPhongBanXuLy').html(result);

                    fnDropddlNoiDungXuLy();
                }
            });
        }

        function pageselectCallback(page_index) {
            var curentPages = page_index + 1;
            var typeSearch = $("#ddlNoiDungXuLy").val();
            var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
            if (SoThueBao == null) {
                SoThueBao == ''
            }
            var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();
            if (NguoiTiepNhan == null) {
                NguoiTiepNhan = '';
            }
            var NguoiXuLy = $("#<%=txtNguoiXuLy.ClientID %>").val();
            if (NguoiXuLy == null) {
                NguoiXuLy = '';
            }
            var maKhieuNai = $("#<%=txtMaKhieuNai.ClientID %>").val();
            if (maKhieuNai == null) {
                maKhieuNai == ''
            }

            var PhongBanXuLy = $("#DropPhongBanXuLy").val();

            var loaiKhieuNai = $("#ddlLoaiKhieuNai").val();
            if (loaiKhieuNai == null || loaiKhieuNai == '0') {
                loaiKhieuNai = -1;
            }
            var linhVucChung = $("#ddlLinhVucChung").val();
            if (linhVucChung == null || linhVucChung == '0') {
                linhVucChung = -1;
            }
            var linhVucCon = $("#ddlLinhVucCon").val();
            if (linhVucCon == null || linhVucCon == '0') {
                linhVucCon = -1;
            }
            var trangThai = $("#ddlTrangThai").val();
            var NgayTiepNhan_tu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
            if (NgayTiepNhan_tu == null || NgayTiepNhan_tu == '') {
                NgayTiepNhan_tu = -1;
            }
            var NgayTiepNhan_den = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();
            if (NgayTiepNhan_den == null || NgayTiepNhan_den == '') {
                NgayTiepNhan_den = -1;
            }

            var ThoiGianCapNhat_tu = $("#<%=txtThoiGianCapNhat_From.ClientID %>").val();
            if (ThoiGianCapNhat_tu == null || ThoiGianCapNhat_tu == '') {
                ThoiGianCapNhat_tu = -1;
            }
            var ThoiGianCapNhat_den = $("#<%=txtThoiGianCapNhat_To.ClientID %>").val();
            if (ThoiGianCapNhat_den == null || ThoiGianCapNhat_den == '') {
                ThoiGianCapNhat_den = -1;
            }

            var urlQuery = '/Views/HanhDongXuLy/Ajax/HanhDongXuLy_Ajax.ashx?key=11'
                + '&typeSearch=' + typeSearch
                + '&PhongBanXuLy=' + PhongBanXuLy
                + '&trangThai=' + trangThai
                + '&loaiKhieuNai=' + loaiKhieuNai
                + '&linhVucChung=' + linhVucChung
                + '&linhVucCon=' + linhVucCon
                + '&pageSize=' + pageSize
                + '&startPageIndex=' + curentPages

            $.ajax({
                beforeSend: function () {
                },
                type: "GET",
                dataType: "JSON",
                url: urlQuery,
                data: {
                    maKhieuNai: maKhieuNai,
                    NguoiXuLy: NguoiXuLy,
                    NguoiTiepNhan: NguoiTiepNhan,
                    NgayTiepNhan_tu: NgayTiepNhan_tu,
                    NgayTiepNhan_den: NgayTiepNhan_den,
                    ThoiGianCapNhat_tu: ThoiGianCapNhat_tu,
                    ThoiGianCapNhat_den: ThoiGianCapNhat_den,
                    SoThueBao: SoThueBao
                },
                success: function (result) {
                    if (result != '') {
                        $('#grid-data').html(result);
                        $('a.normalTip').aToolTip();
                    }
                },
                error: function () {
                }
            });

            return false;
        }
        function getOptionsFromForm() {
            fnSetSizeDiv();
            var opt = { callback: pageselectCallback };
            $("input:text").each(function () {
                opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
            });
            return opt;
        }
        function fnDropPageSizeChange() {
            pageSize = $('#DropPageSize').val();
            fnLocKhieuNai();
        }
        function fnSetSizeDiv() {
            var d = $('body').innerWidth() - 56;
            var h = screen.height;
            $("#divScroll").css("width", d);
            $(".divOpacity").css("height", h);
        }

        function fnLocKhieuNai() {
            fnSetSizeDiv();
            var optInit = getOptionsFromForm();
            var typeSearch = $("#ddlNoiDungXuLy").val();

            var maKhieuNai = $("#<%=txtMaKhieuNai.ClientID %>").val();
            if (maKhieuNai == null || maKhieuNai == '') {
                maKhieuNai == ''
            }
            var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
            if (SoThueBao == null) {
                SoThueBao == ''
            }
            var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();
            if (NguoiTiepNhan == null) {
                NguoiTiepNhan == ''
            }
            var NguoiXuLy = $("#<%=txtNguoiXuLy.ClientID %>").val();
            if (NguoiXuLy == null) {
                NguoiXuLy = '';
            }

            var PhongBanXuLy = $("#DropPhongBanXuLy").val();

            var loaiKhieuNai = $("#ddlLoaiKhieuNai").val();
            if (loaiKhieuNai == null || loaiKhieuNai == '0') {
                loaiKhieuNai = -1;
            }
            var linhVucChung = $("#ddlLinhVucChung").val();
            if (linhVucChung == null || linhVucChung == '0') {
                linhVucChung = -1;
            }
            var linhVucCon = $("#ddlLinhVucCon").val();
            if (linhVucCon == null || linhVucCon == '0') {
                linhVucCon = -1;
            }
            var trangThai = $("#ddlTrangThai").val();
            var NgayTiepNhan_tu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
            if (NgayTiepNhan_tu == null || NgayTiepNhan_tu == '') {
                NgayTiepNhan_tu = -1;
            }
            var NgayTiepNhan_den = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();
            if (NgayTiepNhan_den == null || NgayTiepNhan_den == '') {
                NgayTiepNhan_den = -1;
            }

            var ThoiGianCapNhat_tu = $("#<%=txtThoiGianCapNhat_From.ClientID %>").val();
            if (ThoiGianCapNhat_tu == null || ThoiGianCapNhat_tu == '') {
                ThoiGianCapNhat_tu = -1;
            }
            var ThoiGianCapNhat_den = $("#<%=txtThoiGianCapNhat_To.ClientID %>").val();
            if (ThoiGianCapNhat_den == null || ThoiGianCapNhat_den == '') {
                ThoiGianCapNhat_den = -1;
            }

            var urlQuery = '/Views/HanhDongXuLy/Ajax/HanhDongXuLy_Ajax.ashx?key=12'
                + '&typeSearch=' + typeSearch
                + '&trangThai=' + trangThai
                + '&PhongBanXuLy=' + PhongBanXuLy
                + '&loaiKhieuNai=' + loaiKhieuNai
                + '&linhVucChung=' + linhVucChung
                + '&linhVucCon=' + linhVucCon
                + '&pageSize=' + pageSize
                + '&startPageIndex=1'

            $.ajax({
                beforeSend: function () {
                },
                type: "GET",
                dataType: "JSON",
                url: urlQuery,
                data: {
                    maKhieuNai: maKhieuNai,
                    NguoiXuLy: NguoiXuLy,
                    NguoiTiepNhan: NguoiTiepNhan,
                    NgayTiepNhan_tu: NgayTiepNhan_tu,
                    NgayTiepNhan_den: NgayTiepNhan_den,
                    ThoiGianCapNhat_tu: ThoiGianCapNhat_tu,
                    ThoiGianCapNhat_den: ThoiGianCapNhat_den,
                    SoThueBao: SoThueBao
                },
                success: function (totalRecords) {
                    if (totalRecords != '') {
                        if (totalRecords == 0) {
                            $("#Pagination").pagination(0, optInit);
                        }
                        else {
                            $("#Pagination").pagination(totalRecords, optInit);
                        }
                        $("#divTotalRecords").html('Tổng số bản ghi:' + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
                    }
                },
                error: function () {
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
        function fnDropLoaiKhieuNaiChange() {
            var loaiKhieuNaiId = document.getElementById('ddlLoaiKhieuNai').value;
            $.getJSON('/Views/HanhDongXuLy/Ajax/HanhDongXuLy_Ajax.ashx?key=09&loaiKhieuNaiId=' + loaiKhieuNaiId, '', function (result) {
                if (result != '') {
                    $('#ddlLinhVucChung').html(result);
                }

            });
        }
        function fnDropLinhVucChungChange() {
            var linhVucChungId = document.getElementById('ddlLinhVucChung').value;
            $.getJSON('/Views/HanhDongXuLy/Ajax/HanhDongXuLy_Ajax.ashx?key=10&linhVucChungId=' + linhVucChungId, '', function (result) {
                if (result != '') {
                    $('#ddlLinhVucCon').html(result);
                }

            });
        }


        function fnDropddlNoiDungXuLy() {
            var typeSearch = $("#ddlNoiDungXuLy").val();

            if (typeSearch == 0) {
                $("#<%=txtNguoiXuLy.ClientID %>").val("");
                $("#DropPhongBanXuLy").val(-1);
            }
            else if (typeSearch == 1) {
                $("#<%=txtNguoiXuLy.ClientID %>").val(Username);
                $("#DropPhongBanXuLy").val(PhongBanId);
            }
            else if (typeSearch == 2) {
                $("#<%=txtNguoiXuLy.ClientID %>").val("");
                    $("#DropPhongBanXuLy").val(PhongBanId);
                }
        fnLocKhieuNai();
    }
    function ClosePoup() {

        $('.divOpacity').css('display', 'none');
        $('#divPoup').hide();

    }

    function fnExportExcel() {
        common.loading();
        var typeSearch = $("#ddlNoiDungXuLy").val();
        var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val();
        if (SoThueBao == null) {
            SoThueBao == ''
        }
        var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val();
        if (NguoiTiepNhan == null) {
            NguoiTiepNhan == ''
        }
        var NguoiXuLy = $("#<%=txtNguoiXuLy.ClientID %>").val();
        if (NguoiXuLy == null) {
            NguoiXuLy = '';
        }
        var maKhieuNai = $("#<%=txtMaKhieuNai.ClientID %>").val();
            if (maKhieuNai == null || maKhieuNai == '') {
                maKhieuNai == ''
            }

            var PhongBanXuLy = $("#DropPhongBanXuLy").val();

            var loaiKhieuNai = $("#ddlLoaiKhieuNai").val();
            if (loaiKhieuNai == null || loaiKhieuNai == '0') {
                loaiKhieuNai = -1;
            }
            var linhVucChung = $("#ddlLinhVucChung").val();
            if (linhVucChung == null || linhVucChung == '0') {
                linhVucChung = -1;
            }
            var linhVucCon = $("#ddlLinhVucCon").val();
            if (linhVucCon == null || linhVucCon == '0') {
                linhVucCon = -1;
            }
            var trangThai = $("#ddlTrangThai").val();
            var NgayTiepNhan_tu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val();
            if (NgayTiepNhan_tu == null || NgayTiepNhan_tu == '') {
                NgayTiepNhan_tu = -1;
            }
            var NgayTiepNhan_den = $("#<%=txtNgayTiepNhan_den.ClientID %>").val();
            if (NgayTiepNhan_den == null || NgayTiepNhan_den == '') {
                NgayTiepNhan_den = -1;
            }

            var ThoiGianCapNhat_tu = $("#<%=txtThoiGianCapNhat_From.ClientID %>").val();
            if (ThoiGianCapNhat_tu == null || ThoiGianCapNhat_tu == '') {
                ThoiGianCapNhat_tu = -1;
            }
            var ThoiGianCapNhat_den = $("#<%=txtThoiGianCapNhat_To.ClientID %>").val();
            if (ThoiGianCapNhat_den == null || ThoiGianCapNhat_den == '') {
                ThoiGianCapNhat_den = -1;
            }

            var urlQuery = '/Views/HanhDongXuLy/Ajax/HanhDongXuLy_Ajax.ashx?key=13'
                + '&typeSearch=' + typeSearch
                + '&trangThai=' + trangThai
                + '&PhongBanXuLy=' + PhongBanXuLy
                + '&loaiKhieuNai=' + loaiKhieuNai
                + '&linhVucChung=' + linhVucChung
                + '&linhVucCon=' + linhVucCon
                + '&pageSize=' + pageSize
                + '&startPageIndex=1'

            $.ajax({
                beforeSend: function () {
                },
                type: "GET",
                dataType: "JSON",
                url: urlQuery,
                data: {
                    maKhieuNai: maKhieuNai,
                    NguoiXuLy: NguoiXuLy,
                    NguoiTiepNhan: NguoiTiepNhan,
                    NgayTiepNhan_tu: NgayTiepNhan_tu,
                    NgayTiepNhan_den: NgayTiepNhan_den,
                    ThoiGianCapNhat_tu: ThoiGianCapNhat_tu,
                    ThoiGianCapNhat_den: ThoiGianCapNhat_den,
                    SoThueBao: SoThueBao
                },
                success: function (result) {
                    if (result != '') {
                        window.location.href = "/ExportExcel/Excel" + result;
                        common.unLoading();
                    } else {
                        common.unLoading();s
                        MessageAlert.AlertNormal('Quá trình xuất file bị lỗi ! Vui lòng kiểm tra lại', 'error');
                    }
                },
                error: function () {
                }
            });
        }
    </script>
    <div class="p8">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 500px">
                    <div class="selectstyle_longlx" style="width: 200px">
                        <div class="bg">
                            <asp:DropDownList ID="ddlNoiDungXuLy" onchange="javascript:fnDropddlNoiDungXuLy();"
                                runat="server" ClientIDMode="Static">
                                <asp:ListItem Value="0" Text="Tất cả hành động"></asp:ListItem>
                                <asp:ListItem Value="1" Selected="True" Text="Nội dung xử lý (Cá nhân)"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Nội dung xử lý (Cấp phòng ban)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td align="right">
                    <input type="button" id="lbtTruyVan" class="btn_style_button" value="Truy vấn">
                    <input type="button" id="lbtDongTruyVan" class="btn_style_button" onclick="javascript: hide_truyvan();"
                        value="Đóng truy vấn">
                    <input type="button" id="Button1" class="btn_style_button" onclick="javascript: fnExportExcel();"
                        value="Xuất Excel">
                </td>
            </tr>
        </table>
        <br />
        <div id="PTruyVanNangCao" style="display: none">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3">
                        <h4>Truy vấn thông tin</h4>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 200px">Mã khiếu nại
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtMaKhieuNai" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">Số thuê bao
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtSoThueBao" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                        <span id="validateSoDienThoai" style="display: block; color: Red"></span>
                    </td>
                    <td align="left">
                        <input type="button" id="lbtTimKien" style="margin-top: 15px;" class="btn_style_button"
                            value="Tìm kiếm">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 200px">Người tiếp nhận
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtNguoiTiepNhan" runat="server" Text="" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">Phòng ban xử lý
                        <div class="selectstyle_longlx" style="width: 200px">
                            <div class="bg">
                                <select id="DropPhongBanXuLy">
                                    <option value="-1">--Phòng ban xử lý--</option>
                                </select>
                            </div>
                        </div>
                    </td>
                    <td align="left">Người xử lý
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtNguoiXuLy" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 200px">Loại khiếu nại
                                <div class="selectstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" ClientIDMode="Static" onchange="javascript:fnDropLoaiKhieuNaiChange();">
                                            <asp:ListItem>--Chọn Loại khiếu nại--</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">Lĩnh vực chung
                                <div class="selectstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlLinhVucChung" runat="server" ClientIDMode="Static" onchange="javascript:fnDropLinhVucChungChange();">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">Lĩnh vực con
                                <div class="selectstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlLinhVucCon" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 200px">Trạng thái
                                <div class="selectstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:DropDownList ID="ddlTrangThai" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="-1">--Trạng thái--</asp:ListItem>
                                            <asp:ListItem Value="0">Chờ xử lý</asp:ListItem>
                                            <asp:ListItem Value="1">Đang xử lý</asp:ListItem>
                                            <asp:ListItem Value="2">Chờ đóng</asp:ListItem>
                                            <asp:ListItem Value="3">Đóng</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">T/g tiếp nhận từ ngày
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtNgayTiepNhan_tu" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">T/g tiếp nhận đến ngày
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtNgayTiepNhan_den" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                        <span id="ngayden" style="display: block; color: Red"></span>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="left" style="width: 200px">T/g cập nhật từ ngày
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtThoiGianCapNhat_From" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                    </td>
                    <td align="left" style="width: 200px">T/g cập nhật đến ngày
                                <div class="inputstyle_longlx" style="width: 200px">
                                    <div class="bg">
                                        <asp:TextBox ID="txtThoiGianCapNhat_To" runat="server" Text="" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                        <span id="Span1" style="display: block; color: Red"></span>
                    </td>
                </tr>
            </table>
        </div>
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tbody>
                <tr>
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
                        <div id="Pagination" class="pagination" style="float: right; margin-right: -3px;">
                        </div>
                        <div id="PageSize" class="pagination" style="float: right;">
                            <div class="selectstyle_longlx">
                                <div class="bg" style="margin: -7px; margin-right: 10px; margin-left: 0px;">
                                    <select id="DropPageSize" onchange="javascript:fnDropPageSizeChange();" style="width: 60px;">
                                        <option value="10" selected="selected">10</option>
                                        <option value="20">20</option>
                                        <option value="50">50</option>
                                        <option value="100">100</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div id="divTotalRecords" style="width: 150px; float: right;">
                        </div>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <div id="divScroll" style='overflow-x: scroll; height: auto;'>
                            <table class="tbl_style" cellspacing="0" cellpadding="0" style='width: 2150px;'>
                                <thead class="grid-data-thead">
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">STT
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Trạng thái
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Mã PA/KN
                                    </th>

                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Số thuê bao
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Hành động
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Người tác động
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người tiếp nhận
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Người xử lý
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Loại khiếu nại
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực chung
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Lĩnh vực con
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px;">Khách hàng
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Địa chỉ liên hệ
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Địa điểm sự cố
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Thời gian sự cố
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Tỉnh\Thành phố
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Quận\Huyện
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Thời gian cập nhật
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Auto
                                    </th>
                                    <th class="thead-colunm" style="white-space: nowrap; padding-left: 5px; padding-right: 5px; text-align: left;">Nội dung
                                    </th>
                                </thead>
                                <tbody id="grid-data">
                                </tbody>
                            </table>
                        </div>
                        <div class="div-clear" style="height: 10px;">
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        function handleEnter(obj, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode == 13) {
                document.getElementById(obj).click();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">

        $("#<%=txtNgayTiepNhan_tu.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });
        $("#<%=txtNgayTiepNhan_den.ClientID %>").datepick({ dateFormat: 'dd/mm/yyyy' });

        $("#lbtTimKien").click(function () {
            $("#lbtTruyVan").hide();
            $("#lbtDongTruyVan").show();
            $("#PTruyVanNangCao").css("display", "block")
            var startDt = document.getElementById("<%=txtNgayTiepNhan_tu.ClientID %>").value;
            var endDt = document.getElementById("<%=txtNgayTiepNhan_den.ClientID %>").value;
            var i = startDt.split('/');
            var j = endDt.split('/');
            var ngaybatdau = i[1] + "/" + i[0] + "/" + i[2];
            var ngayketthuc = j[1] + "/" + j[0] + "/" + j[2];
            var d = new Date();
            var curr_date = d.getDate();
            var curr_month = d.getMonth() + 1; //Months are zero based
            var curr_year = d.getFullYear();
            var ngayhientai = curr_month + "/" + curr_date + "/" + curr_year;

            if ($('#<%=txtSoThueBao.ClientID %>').val() != "") {
                var value = $('#<%=txtSoThueBao.ClientID %>').val().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                var intRegex = /^\d+$/;
                if (!intRegex.test(value)) {
                    $("#validateSoDienThoai").html("Chỉ nhập số.");
                    return false;
                }
                else {
                    $("#validateSoDienThoai").html("");
                }
            } else {
                $("#validateSoDienThoai").html("");
            }
            if ((new Date(ngaybatdau).getTime() > new Date(ngayketthuc).getTime())) {
                $("#ngayden").html("Không được nhỏ hơn từ ngày");
                return false;
            }
            else {
                $("#ngayden").html("");
            }

            fnLocKhieuNai();
        });

        $("#lbtTruyVan").click(function () {
            $("#lbtTruyVan").hide();
            $("#lbtDongTruyVan").show();
            $("#PTruyVanNangCao").css("display", "block")
            fnSetSizeDiv();
        });

        function hide_truyvan() {
            $("#lbtTruyVan").show();
            $("#lbtDongTruyVan").hide();
            $("#PTruyVanNangCao").css("display", "none")
            var SoThueBao = $("#<%=txtSoThueBao.ClientID %>").val("");
            var NguoiTiepNhan = $("#<%=txtNguoiTiepNhan.ClientID %>").val("");
            var NguoiXuLy = $("#<%=txtNguoiXuLy.ClientID %>").val("");
            var maKhieuNai = $("#<%=txtMaKhieuNai.ClientID %>").val("");
            var loaiKhieuNai = $("#ddlLoaiKhieuNai").val("0");
            var linhVucChung = $("#ddlLinhVucChung").val("0");
            var linhVucCon = $("#ddlLinhVucCon").val("0");
            var trangThai = $("#ddlTrangThai").val("-1");
            var NgayTiepNhan_tu = $("#<%=txtNgayTiepNhan_tu.ClientID %>").val("");
            var NgayTiepNhan_den = $("#<%=txtNgayTiepNhan_den.ClientID %>").val("");
            fnSetSizeDiv();
        }
        function AutocompleteNguoiTiepNhan() {
            $("#<%=txtNguoiTiepNhan.ClientID %>").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
                dataType: "json",
                width: 300,
                max: 15,
                parse: function (data) {
                    return $.map(data, function (row, index) {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.TenTruyCap
                        };
                    });
                },
                formatItem: function (item) {
                    return format(item);
                }
            }).result(function (e, item) {
                $("#<%=txtNguoiTiepNhan.ClientID %>").val(item.TenTruyCap);
            });

            $("#<%=txtNguoiTiepNhan.ClientID %>").focus();
        }
        function AutocompleteNguoiXuLy() {
            $("#<%=txtNguoiXuLy.ClientID %>").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
                dataType: "json",
                width: 300,
                max: 15,
                parse: function (data) {
                    return $.map(data, function (row, index) {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.TenTruyCap
                        };
                    });
                },
                formatItem: function (item) {
                    return format(item);
                }
            }).result(function (e, item) {
                $("#<%=txtNguoiXuLy.ClientID %>").val(item.TenTruyCap);
            });

            $("#<%=txtNguoiXuLy.ClientID %>").focus();
        }
        function AutocompletemaKhieuNai() {
            $("#<%=txtMaKhieuNai.ClientID %>").autocomplete("/Views/QLKhieuNai/Handler/Autocom.ashx?key=1", {
                dataType: "json",
                width: 300,
                max: 15,
                parse: function (data) {
                    return $.map(data, function (row, index) {
                        return {
                            data: row,
                            value: index.toString(),
                            result: row.TenTruyCap
                        };
                    });
                },
                formatItem: function (item) {
                    return format(item);
                }
            }).result(function (e, item) {
                $("#<%=txtMaKhieuNai.ClientID %>").val(item.TenTruyCap);
            });

            $("#<%=txtMaKhieuNai.ClientID %>").focus();
        }
        function format(item) {
            return "<span class='ac_keyword'>" + item.TenTruyCap + "</span>";
        }
    </script>
</asp:Content>
