<%@ Page Title="" Language="C#" MasterPageFile="~/AdminNotAJAX.Master" AutoEventWireup="true" CodeBehind="SuaKhieuNai.aspx.cs" Inherits="Website.Views.ChiTietKhieuNai.SuaKhieuNai" %>

<%@ Register TagPrefix="uc" TagName="ChiTietKhieuNai" Src="~/Views/ChiTietKhieuNai/UC/ucChiTietKhieuNai.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <style type="text/css">
                #divChiTietKN-Title h1 { font-size: 14px; font-weight: bold; padding-left: 10px; padding-right: 10px; padding-bottom: 5px; border-bottom: 1px solid #CCC; line-height: 33px; margin-bottom: 10px; }

                #divChiTietKN-Info table tr { line-height: 30px; padding-left: 5px; }

                #divChiTietKN-Info table tr td { padding-left: 5px; padding-right: 2px; }
            </style>
            <style type="text/css">
                #contain { position: static; }

                .popup_Container { background-color: #f0f0f0; border: 2px solid #000000; padding: 0px 0px 0px 0px; }

                .popup_Titlebar { background: url(/Images/titlebar_bg.jpg); height: 29px; }

                .popup_Body { padding: 15px 15px 15px 15px; font-family: Arial; font-weight: bold; font-size: 12px; color: #000000; line-height: 15pt; clear: both; padding: 20px; }


                .TitlebarLeft { float: left; padding-left: 5px; padding-top: 5px; font-family: Arial, Helvetica, sans-serif; font-weight: bold; font-size: 12px; color: #FFFFFF; }

                .TitlebarRight { background: url(/Images/cross_icon_normal.png); background-position: right; background-repeat: no-repeat; height: 15px; width: 16px; float: right; cursor: pointer; margin-right: 5px; margin-top: 5px; }

                .infoBox { background-color: #e0efff; background-image: url(/Images/icons/information.gif); background-repeat: no-repeat; border: 1px solid #9eb6d4; padding-left: 2.5em; text-align: left; }
            </style>
            <script language="javascript" type="text/javascript">
                function ClearUI() {
                    $find("modalChuyenXuLy").hide();
                }

                function cancel() {
                    $get('btnCancel').click();
                }

                function fnDongKhieuNai() {
                    var mode = Utility.GetUrlParam("Mode");
                    if (mode == "Edit") {
                        $.messager.confirm('Thông báo', "Bạn chắc chắn muốn đóng khiếu nại?", function (result) {
                            if (result) {
                                var MaKN = Utility.GetUrlParam("MaKN");
                                var strURL = Utility.GetUrlParam("ctrl");
                                $.ajax({
                                    beforeSend: function () {

                                    },
                                    type: "POST",
                                    dataType: "text",
                                    url: "../Ajax/Ajax.ashx",
                                    data: { type: "DongKN", MaKN: MaKN },
                                    success: function (text) {
                                        if (text != "") {
                                            MessageAlert.AlertNormal(text, 'error');
                                        }
                                        else {
                                            MessageAlert.AlertRedirect('Đóng khiếu nại thành công.', 'info', '/Views/QLKhieuNai/MyKhieuNai.aspx?ctrl=' + strURL + '');
                                        }
                                    },
                                    error: function () {
                                    }
                                });
                            }
                        });
                    }
                }

                function fnChuyenXuLy() {
                    var mode = Utility.GetUrlParam("Mode");
                    if (mode == "Edit") {
                        $.messager.confirm('Thông báo', "Bạn chắc chắn muốn chuyển xử lý?", function (result) {
                            if (result) {
                                var MaKN = Utility.GetUrlParam("MaKN");
                                var strURL = Utility.GetUrlParam("ctrl");
                                $.ajax({
                                    beforeSend: function () {
                                    },
                                    type: "POST",
                                    dataType: "text",
                                    url: "../Ajax/Ajax.ashx",
                                    data: { type: "ChuyenKN", MaKN: MaKN, Mode: mode },
                                    success: function (text) {
                                        if (text != "") {
                                            MessageAlert.AlertNormal(text, 'error');
                                        }
                                        else {
                                            MessageAlert.AlertRedirect('Chuyển khiếu nại thành công', 'info', '/Views/QLKhieuNai/MyKhieuNai.aspx?ctrl=' + strURL + '');
                                        }
                                    },
                                    error: function () {
                                    }
                                });
                            }
                        });
                    }
                }

                function PrintPhieuKhieuNai() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuKhieuNai.aspx?MaKN=" + MaKN;
                    window.open(url);
                }

                function PrintPhieuXacMinh() {
                    var MaKN = Utility.GetUrlParam("MaKN");
                    var url = "PhieuXacMinh.aspx?MaKN=" + MaKN;
                    window.open(url);
                }
            </script>


            <div class="nav_btn">
                <ul>
                    <li><a href="javascript:history.back()"><span class="back">Quay về</span></a></li>
                    <li><a href="#" onclick="fnChuyenXuLy();"><span class="move_file">Chuyển xử lý
                    </span></a>
                    </li>
                    <li><a href="#" onclick="fnDongKhieuNai();"><span class="lock">Đóng khiếu nại
                    </span></a>
                    </li>
                    <li><a href="#" onclick="PrintPhieuKhieuNai();"><span class="print">In phiếu KN</span></a></li>
                    <li><a href="#" onclick="PrintPhieuXacMinh();"><span class="print">In phiếu xác minh</span></a></li>
                </ul>
            </div>

            <div id="divChiTietKN-Title">
                <h1 id="info-MaKhieuNai">Mã khiếu nại:
                    <asp:Literal ID="ltMaKhieuNai" runat="server"></asp:Literal>
                </h1>
            </div>
            <div id="divChiTietKN-Info" style="">
                <table cellpadding="0" cellspacing="0" style="white-space: nowrap;" width="100%">
                    <tr style="line-height: 35px; background: #AEAEAE;">
                        <td colspan="4" style="padding-left: 15px;"><span style="font: 14px; font-weight: bold; color: #fff;">Thông tin khiếu nại</span></td>
                        <td colspan="4" style="padding-left: 15px; border-left: 1px solid #ccc;"><span style="font: 14px; font-weight: bold; color: #fff;">Thông tin xử lý</span></td>
                    </tr>
                    <tr>
                        <td style="width: 140px">Số thuê bao<strong>(<asp:Literal ID="ltLoaiThueBao" runat="server"></asp:Literal>)</strong>:</td>
                        <td style="width: 16%">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtSoThueBao" runat="server" Width="80%"></asp:TextBox>
                                </div>
                            </div>
                        </td>

                        <td style="width: 110px">Loại khiếu nại:</td>
                        <td style="width: 18%">
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiKhieuNai" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiKhieuNai_Changed"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc; width: 110px">Người xử lý:</td>
                        <td style="width: 13%">
                            <asp:Literal ID="ltNguoiXuLy" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 120px">T/g cập nhật:</td>
                        <td>
                            <asp:Literal ID="ltNgayCapNhat" Text="" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>Họ tên liên hệ:</td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtHoTen" runat="server" AutoPostBack="true" OnTextChanged="txtHoTen_TextChanged" Width="90%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td>Lĩnh vực chung:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLinhVucChung" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChung_Changed" Width="90%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;">Độ ưu tiên:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlDoUuTien" AutoPostBack="true" OnSelectedIndexChanged="ddlDoUuTien_SelectedIndexChanged" runat="server" Width="90%"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>Thời hạn</td>
                        <td>
                            <asp:Literal ID="ltThoiHan" Text="" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>Điện thoại liên hệ:</td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDienThoaiLienHe" AutoPostBack="true" OnTextChanged="txtDienThoaiLienHe_TextChanged" runat="server" Width="90%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td>Lĩnh vực con:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLinhVucCon" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucCon_Changed" Width="90%"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;">Người tiếp nhận:</td>
                        <td>
                            <asp:Literal ID="ltNguoiTiepNhan" runat="server"></asp:Literal></td>
                        <td>T/g tiếp nhận:</td>
                        <td>
                            <asp:Literal ID="ltNgayTiepNhan" Text="" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>Thời gian xảy ra sự cố:</td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianSuCo" AutoPostBack="true" OnTextChanged="txtThoiGianSuCo_TextChanged" runat="server" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td>Hình thức tiếp nhận:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlHTTiepNhan" AutoPostBack="true" OnSelectedIndexChanged="ddlHTTiepNhan_SelectedIndexChanged" runat="server" Width="90%"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;">Trạng thái:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:Literal ID="ltTrangThai" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </td>
                        <td>T/g đóng:</td>
                        <td>
                            <asp:Literal ID="ltNgayDong" Text="" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>Tỉnh/Thành phố sự cố:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlTinh" OnSelectedIndexChanged="ddlTinh_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="90%"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>Quận/Huyện sự cố:</td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlHuyen" OnSelectedIndexChanged="ddlHuyen_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="90%">
                                        <asp:ListItem Value="0">--Chọn Quận/Huyện--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;">Khiếu nại chờ đóng:</td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkKNChoDong" AutoPostBack="true" OnCheckedChanged="chkKNChoDong_CheckedChanged" runat="server" />
                        </td>

                    </tr>
                    <tr>
                        <td>Địa điểm xảy ra sự cố:</td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaDiemSuCo" AutoPostBack="true" OnTextChanged="txtDiaDiemSuCo_TextChanged" runat="server" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;">KN hàng loạt:</td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkHangLoat" AutoPostBack="true" OnCheckedChanged="chkHangLoat_CheckedChanged" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Địa chỉ liên hệ:</td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaChiLienHe" AutoPostBack="true" OnTextChanged="txtDiaChiLienHe_TextChanged" runat="server" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td valign="top" style="border-left: 1px solid #ccc;">Ghi chú:</td>
                        <td valign="top" colspan="3" rowspan="4">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtGhiChu" runat="server" AutoPostBack="true" OnTextChanged="txtGhiChu_TextChanged" TextMode="MultiLine" Rows="7" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>File KH gửi:</td>
                        <td colspan="3">
                            <asp:Literal ID="ltFileKH" runat="server"></asp:Literal></td>
                        <td valign="top" style="border-left: 1px solid #ccc;"></td>
                    </tr>
                    <tr>
                        <td valign="top">Nội dung phản ánh:</td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtNoiDung" AutoPostBack="true" OnTextChanged="txtNoiDung_TextChanged" TextMode="MultiLine" Rows="5" runat="server" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;"></td>
                    </tr>
                    <tr>
                        <td valign="top">Nội dung hỗ trợ:</td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtNoiDungCanHoTro" AutoPostBack="true" OnTextChanged="txtNoiDungCanHoTro_TextChanged" TextMode="MultiLine" Rows="3" runat="server" Width="96%"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="border-left: 1px solid #ccc;"></td>
                    </tr>
                    <tr>
                        <td style="height: 15px;"></td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:HiddenField ID="hdKhieuNaiId" runat="server" Value="0" />
                            <uc:ChiTietKhieuNai ID="ucChiTietKhieuNai" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
