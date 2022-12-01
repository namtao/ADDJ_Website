<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChuyenKN.ascx.cs" Inherits="Website.Views.QLKhieuNai.UserControls.ChuyenKN" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/UcTopContent.ascx" TagName="UcTopContent"
    TagPrefix="UcTopContent" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ChiTietKN.ascx" TagName="ChiTietKN"
    TagPrefix="ChiTietKN" %>
<%@ Register Src="~/Views/QLKhieuNai/UserControls/ListKNSelect.ascx" TagName="ListKNSelect"
    TagPrefix="ListKNSelect" %>
<script src="/JS/jquery.pagination.js" type="text/javascript"></script>
<script type="text/javascript">
    var pageSize = 15;
    $(document).ready(function () {
        fnSetSizeDiv();
        fnSetViewPoupChiTiet();
        var optInit = getOptionsFromForm();
        var catid = fnGetUrlParameter('catid');
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=3&catid=' + catid + '&pageSize=' + pageSize + '&startPageIndex=1', '', function (totalRecords) {
            if (totalRecords != '') {
                GetTitle(totalRecords);
                
                if (totalRecords == 0) {
                    $("#Pagination").pagination(0, optInit);
                }
                else {
                    $("#Pagination").pagination(totalRecords, optInit);
                }
            }

        });
    });

    function pageselectCallback(page_index) {
        var curentPages = page_index + 1;
        var catid = fnGetUrlParameter('catid');
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=20&catid=' + catid + '&pageSize=' + pageSize + '&startPageIndex=' + curentPages, '', function (result) {
            if (result != '') {
                $('#grid-data').html(result);
            }
        });
        return false;
    }
    function getOptionsFromForm() {
        var opt = { callback: pageselectCallback };
        $("input:text").each(function () {
            opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
        });
        return opt;
    }

    function fnSetSizeDiv() {
        var d = screen.width - 60;
        $("#divScroll").css("width", d);

    }
    function fnSetViewPoupChiTiet() {

        var d = screen.width;

        if (d >= 1360) {
            $("#divPoup").css("left", "15%");
            $("#divPoup").css("right", "15%");
        } else {
            if (d >= 1280) {

                $("#divPoup").css("left", "10%");
                $("#divPoup").css("right", "10%");
            } else {
                if (d >= 1024) {
                    $("#divPoup").css("left", "3%");
                    $("#divPoup").css("right", "3%");
                }
            }
        }


    }
    function GetTitle(totalRecords) {
        var catid = fnGetUrlParameter('catid');
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=10&catid=' + catid, '', function (result) {
            if (result != '') {
                $('#titleChuyenKN').html('Chuyển khiếu nại-' + result + " <span style=\"color: #FF0000;\">(" + addCommas(totalRecords) + ")</span>");
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
    function fnGetUrlParameter(sParam) {
        var values = '';
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                values = sParameterName[1];
            }
        }

        return values;
    }
    function fnGetSetUrl(ctrl) {

        var paramTab = fnGetUrlParameter('tab');
        var catid = fnGetUrlParameter('catid');
        if (paramTab != '') {
            if (catid != '') {
                window.location = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?tab=" + paramTab + "&ctrl=" + ctrl + "&catid=" + catid;
            } else {
                window.location = "/Views/QLKhieuNai/QuanLyKhieuNai.aspx?tab=" + paramTab + "&ctrl=" + ctrl;
            }

        } else {
            alert('ko co tab');

        }

    }
    var listID = '';
    function ShowPoupPhongBan() {
        $(".checkbox-item").each(function () {
            if (this.checked) {
                listID += $(this).val() + ",";
            }

        })
        if (listID != '') {
            $('#divPoupPhongBan').show();
            $('.divOpacity').css('display', 'block');
        } else {
            alert('Vui lòng chọn nội dung cần chuyển !');
        }        
    }
    function ValidatePhongBan() {

        var RB1 = document.getElementById("<%=RadPhongBan.ClientID%>");
        var radio = RB1.getElementsByTagName("input");
        var isChecked = false;
        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked) {
                isChecked = true;
                break;
            }
        }
        if (!isChecked) {

            alert("Vui lòng chọn phòng ban");

        }

        return isChecked;

    }
    function ChuyenKhieuNai() {
        if (listID != '') {
            if (ValidatePhongBan()) {
                var phongBan = $("#<%= RadPhongBan.ClientID %> input:radio:checked").val();
                var note = $("#<%= txtNote.ClientID %>").val();                
                $.post('/Views/QLKhieuNai/Handler/Handler.ashx?key=21&listID=' + listID + '&phongban=' + phongBan, { data: note },
                function (result) {
                    if (result != '0') {
                        alert('Chuyển khiếu nại thành công !');
                        ClosePoup();
                    } else {
                        alert('Chuyển khiếu nại không thành công ! Vui lòng kiểm tra lại');
                    }
                });
                   
            }
        }
    }
    function DongKhieuNai() {
        var list = '';
        $(".checkbox-item").each(function () {
            if (this.checked) {
                list += $(this).val() + ",";
            }

        });
        if (list != '') {
            if (confirm("Bạn chắc chắn thực hiện đóng khiếu nại ?")) { // Clic sur OK
                $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=24&listID=' + list, '', function (result) {
                    //                    var listItem = list.split(",");
                    //                    for (var i = 0; i < listItem.length; i++) {
                    //                        if (listItem[i] != '') {
                    //                            $('#row-' + listItem[i]).hide();
                    //                        }
                    //                    }
                    var pathname = window.location.pathname + window.location.search;
                    window.location.href = pathname;
                    //console.log(pathname);
                });

            }

        } else {
            alert('Vui lòng chọn nội dung cần đóng !');
        }
    }
    function ShowPoup(id) {
        $('#divPoup').show();
        $('.divOpacity').css('display', 'block');
        $.getJSON('/Views/QLKhieuNai/Handler/Handler.ashx?key=5&id=' + id, '', function (result) {
            if (result != '') {
                //var obj = eval("(" + result + ")");
                //var sodienthoai = result.SoThueBao;
                $("#info-SoThueBao").html(result.SoThueBao);
                $("#info-MaKhieuNai").html(result.MaKhieuNai);
                $("#info-LoaiKhieuNai").html(result.LoaiKhieuNai);
                $("#info-LinhVucChung").html(result.LinhVucChung);
                $("#info-LinhVucCon").html(result.LinhVucCon);
                $("#info-NguoiXuLy").html(result.NguoiXuLy);
                $("#info-NgayCapNhat").html(result.LDate);
                $("#info-HoTen").html(result.HoTenLienHe);
                $("#info-DoUuTien").html(result.DoUuTien);
                $("#info-ThoiHan").html(result.NgayQuaHan);
                $("#info-DienThoaiLienHe").html(result.SDTLienHe);
                $("#info-NguoiTiepNhan").html(result.NguoiTiepNhan);
                $("#info-NgayTiepNhan").html(result.NgayTiepNhan);
                $("#info-ThoiGianXayRaSuCo").html(result.ThoiGianXayRa);
                $("#info-DiaChi").html(result.DiaChiLienHe);
                $("#info-TrangThai").html(result.TrangThai);
                $("#info-NgayDong").html(result.NgayDongKN);
                $("#info-DiaDiemXayRaSuCo").html(result.DiaDiemXayRa);
                $("#info-GhiChu").html(result.GhiChu);
                $("#info-FileKHGui").html(result.FileDinhKemKH);
                $("#info-FileGQKNGui").html(result.FileDinhKemGQKN);
                $("#info-NoiDungPhanAnh").html(result.NoiDungPA);
            }

        });
    }

    function Update(jobID, content) {

        ClosePoup();
    }
    function ClosePoup() {
        listID = '';
        $('.divOpacity').css('display', 'none');
        $('#divPoup').hide();
        $('#divPoupPhongBan').hide();
    }
</script>
<UcTopContent:UcTopContent ID="UcTopContent1" runat="server" />
<div class="nav_btn" style='border-top: 0px'>
    <ul>
        <li style="background: none;"><span id="titleChuyenKN" style="color: #4D709A; font-size: 15px;
            font-weight: bold;">Danh sách chuyển khiếu nại</span></li>
        <li style="float: right;"><a href="javascript:ShowPoupPhongBan();"><span class="delete">
            Chuyển bộ phận khác </span></a></li>
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
                <td style="height: 10px">
                </td>
            </tr>
            <tr valign="top">
                <td style="text-align: center">
                    <ListKNSelect:ListKNSelect ID="ListKNSelect2" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="divPoup" style="width: auto; height: auto; background: #fff; margin: 0 auto;
    z-index: 200; position: absolute; top: 5%; left: 20px; right: 20px; border: 1px solid #4D709A;
    border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px;
        height: 25px;">
        <h3 id="divTitle" style="float: left; color: #fff; font-weight: bold;">
            Thông tin chi tiết khiếu nại
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="divContent" style="margin: 5px;">
        <ChiTietKN:ChiTietKN ID="ChiTietKN1" runat="server" />
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div id="divPoupPhongBan" style="width: auto; height: auto; background: #fff; margin: 0 auto;
    z-index: 200; position: absolute; top: 5%; left: 30%; right: 30%; border: 1px solid #4D709A;
    border-radius: 5px 5px 5px 5px; box-shadow: 0 1px 3px #999999 inset; display: none;">
    <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 10px;
        height: 25px;">
        <h3 id="H1" style="float: left; color: #fff; font-weight: bold;">
            Chọn phòng ban cần chuyển đến
        </h3>
        <span style="float: right;"><a href="javascript:ClosePoup();">
            <img src="/Images/x.png" />
        </a></span>
    </div>
    <div id="div2" style="">
        <div class="nav_btn" style='background: none;'>
            <div style="margin-top: 10px; margin-left: 10px; height: 250px; border-top: 1px solid #CCC;
                border-bottom: 1px solid #CCC; background: none; overflow-y: scroll;">
                <asp:RadioButtonList ID="RadPhongBan" runat="server" CssClass="tbl_style">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Don't Know" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="margin-top: 10px; height: 100px; background: none;">
                <span style="font-size: 13px; font-weight: bold;">Nội dung xử lý</span>
                <div class="inputstyle">
                    <div class="bg">
                        <%--<textarea rows="4" cols="50" width = "100%" id="txtInfo" maxlength="500" />--%>
                        <asp:TextBox ID="txtNote" MaxLength="500" TextMode="MultiLine" Height="50" Width="100%"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <ul>
                <li style="float: right;"><a href="javascript:ClosePoup();"><span class="delete">Hủy
                </span></a></li>
                <li style="float: right;"><a href="javascript:ChuyenKhieuNai();"><span class="delete">
                    Đồng ý </span></a></li>
            </ul>
        </div>
    </div>
    <div style="clear: both; height: 1px;">
    </div>
</div>
<div class="divOpacity" style="opacity: 0.4; background: #000; height: 100%; width: 100%;
    position: absolute; left: 0; top: 0; display: none; z-index: 100;">
</div>
