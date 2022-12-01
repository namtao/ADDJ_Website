<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhieuNai_ThemMoi.aspx.cs"  Inherits="Website.Views.QLKhieuNai.KNThemMoi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Khiếu nại - Thêm mới</title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/reset.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.cus.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/content/easyui.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Js/jquery1.8.3.min.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/plugin/ajaxfileupload.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("/JS/Utility.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/Content/easyui/jquery.easyui.min.js"), Website.AppCode.Common.Ver)); %>

    <style type="text/css">
        #contain { position: static; }
        .col1 { width: 20%; }
        .col2 { width: 25%; }
        .col3 { width: 20%; }
        .col4 { width: 25%; }
        .p-5px { padding: 5px; }
        .col-label { text-indent: 10px; }
        .popup_Container { background-color: white; border-radius: 15px; }
        .popup_Body { padding-top: 0px; padding-left: 10px; padding-right: 10px; }
        .selectstyle_longlx { padding-left: 0px; }
        .width-location { width: 150px; }
        .custom-panel { overflow: initial; }
        .panel-body-noheader { border: none; }
        .custom-panel .textbox-text { border: none; padding: 3px; border-left: 1px solid #a4bed4; margin: 0px !important; border-radius: 0 4px 4px 0px; }
        .panel.combo-p { border: 1px solid #a4bed4; border-top: 0px; }
        .combo { border-radius: 4px; }
        .custom-panel { position: relative; }
        .input-remove { position: absolute; right: 0px; top: 4px; width: 16px; height: 16px; cursor: pointer; }
        .input-loading { position: absolute; right: 35px; top: 4px; width: 16px; height: 16px; cursor: pointer; }
        .panel.combo-p { border: 1px solid #a4bed4; border-radius: 5px; }
        .local-process { padding-top: 2px; margin-top: 2px; border-top: 1px dotted #ccc; }
        .foot_nav_btn { padding-top: 5px; }
        .popup_Buttons { padding-top: 7px; border-top: none; padding-right: 2px; }
        .cs-file { padding-bottom: 3px; }
        .lp-2 { padding-top: 3px; margin-top: 3px; }
        .inputstyle_longlx textarea.m-l-v10 { margin-left: -2px; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".input-remove").click(function () {
                window.combo.combo("clear");
                $(".easyui-panel .textbox-text").focus();
            });
        });
    </script>
</head>
<body>
    <form runat="server">
        <asp:HiddenField ID="hdIsThueBao" runat="server" Value="0" />
        <asp:HiddenField ID="hdIsCallService" runat="server" Value="0" />
        <div class="popup_Container">
            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 5px; height: 21px;">
                <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Thêm mới khiếu nại
                </h3>
                <span style="float: right;"><a href="javascript:CloseForm();">
                    <img alt="Đóng" runat="server" src="~/Images/x.png" />
                </a></span>
            </div>
            <div class="popup_Body">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="col1">Loại thuê bao</td>
                        <td class="col2">
                            <div class="selectstyle_longlx" style="max-width: 150px;">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlLoaiThueBao" runat="server">
                                        <asp:ListItem Value="0">Trả trước</asp:ListItem>
                                        <asp:ListItem Value="1">Trả sau</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td class="col3 col-label">Tìm kiếm</td>
                        <td class="col4">
                            <div class="easyui-panel custom-panel" style="width: 200px;">
                                <div class="inner">
                                    <input id="myCombo" class="easyui-combobox" name="language" style="width: 90%" />
                                </div>
                                <div class="input-remove">
                                    <a href="javascript:void(0)" title="Xóa ô tìm kiếm">
                                        <img alt="Đóng" src="/content/images/no.png" /></a>
                                </div>
                                <div class="input-loading hide">
                                    <img alt="Đóng" src="/content/images/panel_loading.gif" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Số thuê bao
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                <tr>
                                    <td>
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtDauSo" ReadOnly="true" Text="84" Width="20px" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 90%">
                                        <div class="inputstyle_longlx">
                                            <div class="bg">
                                                <asp:TextBox ID="txtSoThueBao" runat="server" Text="" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btTraCuu" OnClientClick="return LayThongTinThueBao();" CssClass="btn_style_button" runat="server" Text="..." />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="col-label">Loại khiếu nại <span style="color: red">*</span>
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg" style="position: relative">
                                    <select id="ddlLoaiKhieuNai" name="ddlLoaiKhieuNai">
                                    </select>
                                </div>
                                <input type="hidden" id="hdLoaiKhieuNai" runat="server" value="0" text="" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Họ tên
                        </td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtHoTen" runat="server" Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td class="col-label">Lĩnh vực chung
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <select id="ddlLinhVucChung" name="ddlLinhVucChung">
                                        <option value="0">-- Lĩnh vực chung --</option>
                                    </select>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdLinhVucChung" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td>Điện thoại liên hệ <span style="color: red">*</span>
                        </td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDienThoai" runat="server" Text="" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td class="col-label">Lĩnh vực con
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <select id="ddlLinhVucCon" name="ddlLinhVucCon">
                                        <option value="0">-- Lĩnh vực con --</option>
                                    </select>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdLinhVucCon" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td>Địa chỉ
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaChi" runat="server" Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 2px;"></td>
                    </tr>
                    <tr>
                        <td>Thời gian xảy ra sự cố
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtThoiGianSuCo" runat="server" Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Địa chỉ liên hệ
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaChiLienHe" runat="server" Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Độ ưu tiên
                        </td>
                        <td>
                            <div class="selectstyle_longlx width-location">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlDoUuTien" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>HT tiếp nhận
                        </td>
                        <td>
                            <div class="selectstyle_longlx width-location">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlHTTiepNhan" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="local-process">
                                Địa điểm xảy ra sự cố
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Tỉnh/Thành phố <span style="color: red">
                            <asp:Literal ID="ltRequiedTinh" runat="server">*</asp:Literal></span></td>
                        <td colspan="3">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div class="selectstyle_longlx width-location">
                                            <div class="bg">
                                                <select id="ddlTinh" name="ddlTinh">
                                                    <option value="0">--Tỉnh/Thành phố--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                    <td>Quận/Huyện<span style="color: red">
                                        <asp:Literal ID="ltRequiedQuan" Visible="false" runat="server">*</asp:Literal></span>
                                    </td>
                                    <td>
                                        <div class="selectstyle_longlx width-location">
                                            <div class="bg">
                                                <select id="ddlQuanHuyen" name="ddlQuanHuyen">
                                                    <option value="0">--Quận/Huyện--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                    <td>Phường/Xã<span style="color: red">
                                        <asp:Literal ID="ltRequiedPhuong" Visible="false" runat="server">*</asp:Literal></span>
                                    </td>
                                    <td>
                                        <div class="selectstyle_longlx width-location">
                                            <div class="bg">
                                                <select id="ddlPhuongXa" name="ddlPhuongXa">
                                                    <option value="0">--Phường/Xã--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Địa chỉ xảy ra sự cố <span style="color: red">
                            <asp:Literal ID="ltRequiedDiaChi" Visible="false" runat="server">*</asp:Literal></span>
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtDiaDiemSuCo" runat="server" Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="local-process lp-2">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>File KH gửi
                        </td>
                        <td colspan="3">
                            <div class="cs-file">
                                <input type="file" id="fUploadKhieuNai" name="fUploadKhieuNai" accept=".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx, .csv, .mp3" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">Nội dung phản ánh <span style="color: red">*</span>
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtNoiDungPA" CssClass="p-5px" TextMode="MultiLine" Height="50px" runat="server" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">Nội dung cần hỗ trợ
                        </td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtNoiDungCanHoTro" CssClass="p-5px" TextMode="MultiLine" Height="40px" runat="server" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="vertical-align: top; text-indent: 20px;" class="col3">Ghi chú
                        </td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtGhiChu" CssClass="p-5px m-l-v10" TextMode="MultiLine" Height="40px" runat="server" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="foot_nav_btn">
                    <div class="popup_Buttons">
                        <asp:CheckBox ID="chkIsChuyenTiep" Checked="false" Text="Chuyển tiếp khiếu nại" runat="server" />
                        <a href="#1"><span>
                            <input type="button" class="button_eole save_ctrl" value="Cập nhật" id="btnAdd" onclick="ThemMoiKhieuNai();" />
                        </span></a>
                        <a href="#"><span>
                            <input id="btnCancel" class="button_eole cancel_ctrl" value="Hủy bỏ" type="button" onclick="CloseForm();" /></span></a>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            var flagCall = false;
            function ThemMoiKhieuNai() {
                if (flagCall) return;
                flagCall = true;

                var typeKhieuNai = Utility.GetUrlParam('type');
                if (validForm()) {
                    common.showWinMark();
                    $.ajax({
                        type: "POST",
                        url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=Add",
                        data: {
                            SoThueBao: $('#<%=txtSoThueBao.ClientID%>').val().trim(),
                            IsCallService: $('#hdIsCallService').val(),
                            IsThueBao: $('#hdIsThueBao').val(),
                            LoaiThueBao: $('#<%=ddlLoaiThueBao.ClientID%>').val(),
                            LoaiKhieuNaiId: $('#hdLoaiKhieuNai').val(),
                            LoaiKhieuNai: $('#hdLoaiKhieuNai').attr('text'),
                            LinhVucChungId: $('#hdLinhVucChung').val(),
                            LinhVucChung: $('#hdLinhVucChung').attr('text'),
                            LinhVucConId: $('#hdLinhVucCon').val(),
                            LinhVucCon: $('#hdLinhVucCon').attr('text'),
                            HoTen: $('#<%=txtHoTen.ClientID%>').val(),
                            DTLienHe: $('#<%=txtDienThoai.ClientID%>').val(),
                            DiaChi: $('#<%=txtDiaChi.ClientID%>').val(),
                            ThoiGianSuCo: $('#<%=txtThoiGianSuCo.ClientID%>').val(),
                            DiaChiLienHe: $('#<%=txtDiaChiLienHe.ClientID%>').val(),
                            DoUuTien: $('#<%=ddlDoUuTien.ClientID%>').val(),
                            HTTN: $('#<%=ddlHTTiepNhan.ClientID%>').val(),
                            DiaDiemSuCo: $('#<%=txtDiaDiemSuCo.ClientID%>').val(),
                            TinhId: $('#ddlTinh').val(),
                            Tinh: $("#ddlTinh option:selected").text(),
                            QuanHuyenId: $('#ddlQuanHuyen').val(),
                            QuanHuyen: $("#ddlQuanHuyen option:selected").text(),
                            PhuongXaId: $('#ddlPhuongXa').val(),
                            PhuongXa: $('#ddlPhuongXa option:selected').text(),
                            NoiDung: $('#<%=txtNoiDungPA.ClientID%>').val(),
                            NoiDungHoTro: $('#<%=txtNoiDungCanHoTro.ClientID%>').val(),
                            GhiChu: $('#<%=txtGhiChu.ClientID%>').val(),
                            IsChuyenTiep: $('#<%=chkIsChuyenTiep.ClientID%>').is(':checked'),
                            KhieuNaiFrom: typeKhieuNai
                        },
                        dataType: 'json',
                        success: function (outputItem) {
                            if (outputItem.ErrorId == 0) {
                                UploadFile(outputItem);
                            }
                            else if (outputItem.ErrorId == 99) {
                                MessageAlert.AlertNormal(outputItem.Message, 'error');
                                flagCall = false;
                                common.hideWinMark();
                            }
                            else if (outputItem.ErrorId == -1000) {
                                MessageAlert.AlertJSON(outputItem.ErrorId);
                                flagCall = false;
                                common.hideWinMark();
                            }
                            else {
                                MessageAlert.AlertNormal(outputItem.Message, 'error');
                                flagCall = false;
                                common.hideWinMark();
                            }
                        },
                        error: function (e) {
                            Common.UnLoading();
                            flagCall = false;
                            CloseForm();
                        },
                        complete: function () {
                            common.hideWinMark();
                        }
                    });
                }
                else {
                    flagCall = false;
                }
            }

            function UploadFile(outputItem) {
                $.ajaxFileUpload({
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=UploadFile&KhieuNaiId=" + outputItem.Content,
                    secureuri: false,
                    sfileElementId: 'fUploadKhieuNai',
                    dataType: 'text',
                    success: function () {
                        window.parent.ThemKNSuccess(outputItem.Message);
                    },
                    error: function (e) {
                    },
                    complete: function () {
                        flagCall = false;
                        Common.UnLoading();
                        CloseForm();
                    }
                });
            }

            function validForm() {
                var str = $('#<%=txtSoThueBao.ClientID %>');
                if (str.val() == "") {
                    MessageAlert.AlertNormal('Bạn chưa nhập số thuê bao.', 'error', str.attr('id'));
                    return false;
                }

                var rg = "^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7})|(88[0123456789]([0-9]){6}))$";
                str = $('#<%=txtDauSo.ClientID %>').val() + $('#<%=txtSoThueBao.ClientID %>').val().trim();
                var found = str.match(rg);
                if (found == null) {
                    MessageAlert.AlertNormal('Số thuê bao chưa hợp lệ.', 'error', $('#<%=txtSoThueBao.ClientID %>').attr('id'));
                    return false;
                }

                if ($('#hdLoaiKhieuNai').val() == undefined || $('#hdLoaiKhieuNai').val() == "0") {
                    MessageAlert.AlertNormal('Bạn chưa chọn loại khiếu nại.', 'error');
                    return false;
                }


                str = $('#<%=txtDienThoai.ClientID %>');
                if (str.val() == "") {
                    MessageAlert.AlertNormal('Bạn chưa nhập điện thoại liên hệ.', 'error', str.attr('id'));
                    return false;
                }

                str = $('#<%=txtNoiDungPA.ClientID %>');
                if (str.val() == "") {
                    MessageAlert.AlertNormal('Bạn chưa nhập nội dung phản ánh.', 'error', str.attr('id'));
                    return false;
                }
                //var strLinhVuc = $('#hdLinhVucChung').val();
                if ($('#hdLinhVucChung').val() == "1001") {
                    if ($('#hdLinhVucCon').val() == undefined || $('#hdLinhVucCon').val() == "0") {
                        MessageAlert.AlertNormal('Bạn chưa chọn lĩnh vực chung con.', 'error');
                        return false;
                    }
                }

                if ($('#ddlTinh').val() == "0") {
                    MessageAlert.AlertNormal('Bạn chưa chọn tỉnh\thành phố.', 'error');
                    return false;
                }

                // Khi loại khiếu nại là chất lượng mạng thì bắt buộc phải nhập 
                //      - quận huyện, phường/xã, địa chỉ xảy ra sự cố
                //      - Lĩnh vực chung, Lĩnh vực con
                if ($('#hdLoaiKhieuNai').val() == "71") {
                    if ($('#hdLinhVucChung').val() == "0") {
                        MessageAlert.AlertNormal('Bạn chưa chọn Lĩnh vực chung.<br/> Khiếu nại về chất lượng mạng phải nhập thêm đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Quận/huyện, Phường/Xã, Địa điểm xảy ra sự cố', 'error');
                        return false;
                    }

                    if ($('#hdLinhVucCon').val() == "0") {
                        MessageAlert.AlertNormal('Bạn chưa chọn Lĩnh vực con.<br/> Khiếu nại về chất lượng mạng phải nhập thêm đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Quận/huyện, Phường/Xã, Địa điểm xảy ra sự cố', 'error');
                        return false;
                    }

                    if ($('#ddlQuanHuyen').val() == "0") {
                        MessageAlert.AlertNormal('Bạn chưa chọn quận/huyện.<br/> Khiếu nại về chất lượng mạng phải nhập thêm đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Quận/huyện, Phường/Xã, Địa điểm xảy ra sự cố', 'error');
                        return false;
                    }

                    if ($('#ddlPhuongXa').val() == "0") {
                        MessageAlert.AlertNormal('Bạn chưa chọn phường/xã.<br/> Khiếu nại về chất lượng mạng phải nhập thêm đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Quận/huyện, Phường/Xã, Địa điểm xảy ra sự cố', 'error');
                        return false;
                    }

                    if ($('#<%=txtDiaDiemSuCo.ClientID %>').val() == "") {
                        MessageAlert.AlertNormal('Bạn phải nhập địa chỉ xảy ra sự cố.<br/> Khiếu nại về chất lượng mạng phải nhập thêm đầy đủ các thông tin : Lĩnh vực chung, Lĩnh vực con, Quận/huyện, Phường/Xã, Địa điểm xảy ra sự cố', 'error');
                        return false;
                    }
                }

                var chkRequied = true;
                $('.requied').each(function () {
                    if (chkRequied) {

                        if ($(this).attr('type') == 'select') {
                            if ($(this).val() == '0') {
                                MessageAlert.AlertNormal($(this).attr('msg'), 'error', $(this).attr('id'));
                                chkRequied = false;
                                return;
                            }
                        }
                        else if ($(this).attr('type') == 'text' || $(this).attr('type') == 'textbox') {
                            if ($(this).val() == '') {
                                MessageAlert.AlertNormal($(this).attr('msg'), 'error', $(this).attr('id'));
                                chkRequied = false;
                                return;
                            }
                        }
                    }
                });


                if (chkRequied) {
                    return true;
                }
                return false;
            }

            function LayThongTinThueBao() {
                var str = $('#<%=txtSoThueBao.ClientID %>');
                if (str.val() == "") {
                    MessageAlert.AlertNormal('Bạn chưa nhập số thuê bao.', 'error', str.attr('id'));
                    return false;
                }

                var rg = "^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7})|(88[0123456789]([0-9]){6}))$";
                str = $('#<%=txtDauSo.ClientID %>').val() + $('#<%=txtSoThueBao.ClientID %>').val().trim();
                var found = str.match(rg);
                if (found == null) {
                    MessageAlert.AlertNormal('Số thuê bao chưa hợp lệ.', 'error', $('#<%=txtSoThueBao.ClientID %>').attr('id'));
                    return false;
                }

                var tb = $('#<%=txtSoThueBao.ClientID%>').val().trim();

                $.ajax({
                    beforeSend: function () {
                        $('#<%=txtHoTen.ClientID%>').val("Đang lấy dữ liệu...");
                        $('#<%=txtDiaChi.ClientID%>').val("Đang lấy dữ liệu...");
                        $('#<%=txtHoTen.ClientID%>').attr("readonly", "readonly");
                        $('#<%=txtDiaChi.ClientID%>').attr("readonly", "readonly");
                    },
                    type: "POST",
                    dataType: "json",
                    url: "/Views/TTKH/Handler/ThongTinKhachHang.ashx",
                    data: { type: "GetInfo", tb: tb },
                    success: function (obj) {
                        if (obj.ErrorId == -1) {
                            MessageAlert.AlertNormal(obj.Message, obj.Title, $('#<%=txtSoThueBao.ClientID%>').attr('id'));
                        }
                        else {
                            BindDataToEle(obj);
                        }
                    },
                    error: function () {
                        MessageAlert.AlertJSON("-1000");
                    },
                    complete: function () {
                        if ($('#<%=txtHoTen.ClientID%>').val() == "Đang lấy dữ liệu...")
                            $('#<%=txtHoTen.ClientID%>').val("");
                        if ($('#<%=txtDiaChi.ClientID%>').val() == "Đang lấy dữ liệu...")
                            $('#<%=txtDiaChi.ClientID%>').val("");
                        $('#<%=txtHoTen.ClientID%>').removeAttr("readonly");
                        $('#<%=txtDiaChi.ClientID%>').removeAttr("readonly");
                        $('#<%=hdIsCallService.ClientID%>').val(1);
                    }
                });
                return false;
            }

            function BindDataToEle(obj) {
                if (obj != null) {
                    if (obj.TEN_LOAI != undefined && (obj.TEN_LOAI == 'Post'
                        || (obj.TEN_LOAI.indexOf("iTouch") != -1))
                        || (obj.TEN_LOAI.toLowerCase() == 'ezpost')
                        || (obj.TEN_LOAI.toLowerCase() == 'iplus2')) {
                        $('#<%=txtHoTen.ClientID%>').val(obj.TEN_TB);
                        $('#<%=txtDiaChi.ClientID%>').val(obj.SOTT_NHA);
                        $('#<%=txtDiaChiLienHe.ClientID%>').val(obj.SOTT_NHA);
                        $('#<%=hdIsThueBao.ClientID%>').val(1);
                        $('#<%=ddlLoaiThueBao.ClientID%>').val(1);

                        $('#ddlTinh').val(obj.MA_TINH);
                        BindQuanHuyen();
                    }
                    else {
                        $('#<%=hdIsThueBao.ClientID%>').val(0);
                        $('#<%=txtHoTen.ClientID%>').val(obj.FULLNAME);
                        $('#<%=txtDiaChi.ClientID%>').val(obj.ADDRESS);
                        $('#<%=txtDiaChiLienHe.ClientID%>').val(obj.ADDRESS);
                    }
                }
            }

            function CloseForm() {
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
                $('textarea').each(function () {
                    $(this).val('');
                });
                $('select').each(function () {
                    $(this).prop('selectedIndex', 0);
                });
                $("#txtDauSo").val('84');

                BindLinhVucCon(0, 0);

                parent.parent.closePopupAddKN();
            }
        </script>

        <script type="text/javascript">
            $(document).ready(function () {
                BindLoaiKhieuNaiChange();
                $.ajax({
                    type: "GET",
                    url: '<%= ResolveClientUrl("~/Views/Popup/Ajax/Handler.ashx") %>',
                    dataType: "json",
                    data: {
                        type: "ThemMoiKhieuNai",
                        command: "LayDanhSachLoaiKN"
                    },
                    success: function (data) {
                        if (data.Code == 1) $('#ddlLoaiKhieuNai').html(data.Data);
                        else alert(data.Message);
                    },
                    error: function (e) {
                        if (console && console.log) console.log(e);
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=4",
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlTinh').html(data);
                        EventChangeTinh();
                    }
                });

            });

            function EventChangeTinh() {
                $('#ddlTinh').change(function (e) {
                    BindQuanHuyen($(this).val());

                });
            }

            function BindQuanHuyen(tinhId) {
                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=5",
                    data: {
                        TinhId: tinhId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlQuanHuyen').html(data);
                        EventChangeQuanHuyen();
                    }
                });
            }

            function EventChangeQuanHuyen() {
                $('#ddlQuanHuyen').change(function (e) {
                    BindPhuongXa($(this).val());

                });
            }

            function BindPhuongXa(quanHuyenId) {
                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=6",
                    data: {
                        QuanHuyenId: quanHuyenId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlPhuongXa').html(data);
                    }
                });
            }

            function BindLoaiKhieuNaiChange() {
                $('#ddlLoaiKhieuNai').change(function (e) {
                    console.log("Loại KN change");
                    $('#hdLoaiKhieuNai').val($(this).val());
                    $('#hdLoaiKhieuNai').attr('text', $("#ddlLoaiKhieuNai option:selected").text());
                    $('#hdLinhVucChung').val(0);
                    $('#hdLinhVucCon').val(0);
                    BindLinhVucChung($(this).val());
                    BindLinhVucCon($(this).val(), 0);
                });
            }

            function EventChangeLinhVucChung() {
                $('#ddlLinhVucChung').change(function (e) {
                    $('#hdLinhVucChung').val($(this).val());
                    $('#hdLinhVucChung').attr('text', $("#ddlLinhVucChung option:selected").text());
                    $('#hdLinhVucCon').val(0);
                    BindLinhVucCon($('#hdLoaiKhieuNai').val(), $(this).val());
                });
            }

            function EventChangeLinhVucCon() {
                $('#ddlLinhVucCon').change(function (e) {
                    //console.log($("#ddlLinhVucCon option:selected").attr('loaikhieunaiid'));
                    $('#ddlLoaiKhieuNai').val($("#ddlLinhVucCon option:selected").attr('loaikhieunaiid'));
                    $('#hdLoaiKhieuNai').val($('#ddlLoaiKhieuNai').val());
                    $('#hdLoaiKhieuNai').attr('text', $("#ddlLoaiKhieuNai option:selected").text());

                    BindLinhVucChungNotRefresh($("#ddlLinhVucCon option:selected").attr('loaikhieunaiid'), $("#ddlLinhVucCon option:selected").attr('linhvucchungid'));

                    $('#hdLinhVucCon').val($(this).val());
                    $('#hdLinhVucCon').attr('text', $("#ddlLinhVucCon option:selected").text());
                });
            }

            function BindLinhVucChung(LoaiKhieuNaiId, selectValue) {
                $.ajax({

                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=2",
                    data: {
                        LoaiKhieuNaiId: LoaiKhieuNaiId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlLinhVucChung').html(data);
                        if (typeof selectValue !== undefined) {
                            $("#ddlLinhVucChung option[value=" + selectValue + "]").prop("selected", true);
                        }
                        EventChangeLinhVucChung();
                    }
                });

            }

            function BindLinhVucChungNotRefresh(LoaiKhieuNaiId, LinhVucChungId) {
                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=2",
                    data: {
                        LoaiKhieuNaiId: LoaiKhieuNaiId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlLinhVucChung').html(data);
                        $('#ddlLinhVucChung').val(LinhVucChungId);
                        $('#hdLinhVucChung').val(LinhVucChungId);
                        $('#hdLinhVucChung').attr('text', $("#ddlLinhVucChung option:selected").text());
                        EventChangeLinhVucChung();
                    }
                });
            }

            function BindLinhVucCon(LoaiKhieuNaiId, LinhVucChungId, selectValue) {
                $.ajax({
                    // async: false,
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=3",
                    data: {
                        LoaiKhieuNaiId: LoaiKhieuNaiId,
                        LinhVucChungId: LinhVucChungId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlLinhVucCon').html(data);
                        // $("#ddlLinhVucCon").val(selectValue);
                        if (typeof selectValue !== undefined) {
                            $("#ddlLinhVucCon option[value=" + selectValue + "]").prop("selected", true);
                        }
                        EventChangeLinhVucCon();
                    }
                });
            }

            function optionSelected(selectedValue) {
                document.title = selectedValue;
            }
        </script>

        <script type="text/javascript">

            function formatItem(row) {

                // Tìm thấy mỗi tên loại khiếu nại
                if (row.type == 1) { // Tìm thấy mỗi tên loại khiếu nại
                    return '<span type="' + row.type + '" data-id="' + row.id + '" style="font-weight:bold; color: #444;">' + row.name + '</span>';
                }
                else if (row.type == 2) // Tìm thấy là lĩnh vực chung => hiển thị thêm "Loại khiếu nại"
                {
                    var objRet = '<span type="' + row.type + '" data-parentId="' + row.parentId + '" data-id="' + row.id + '" style="font-weight:bold; color: #444;">' + row.name + '</span>'
                    objRet += '<br/><span style="color: #D00000 ">' + row.parentName + '</span>';
                    return objRet;
                }
                else // Tìm thấy lĩnh vực con => Có đủ luôn
                {
                    var objRet = '<span type="' + row.type + '" data-loaiKhieuNaiId="' + row.loaiKhieuNaiId + '" data-parentId="' + row.parentId + '" data-id="' + row.id + '" style="font-weight:bold; color: #444;">' + row.name + '</span>';
                    objRet += '<br/><span style="color: #2e1cff">' + row.parentName + '</span>'
                    objRet += '<br/><span style="color: #D00000 ">' + row.tenLoaiKhieuNai + '</span>'
                    return objRet;
                }
            }
            $(document).ready(function () {
                window.combo = $("#myCombo").combobox({
                    loader: myloader,
                    valueField: 'id',
                    mode: 'remote',
                    textField: 'name',
                    panelWidth: 265,
                    panelHeight: 300,
                    onSelect: function (rec) { xuLyTimKiem(rec); },
                    // onBeforeLoad: function (obj) { console.log(obj); },
                    // iconCls: 'icon-clear',
                    formatter: formatItem,
                    labelPosition: 'top'
                }).combo("clear");
            });

            function xuLyTimKiem(obj) {
                if (console && console.log) console.log(obj);
                if (obj.type == 1) {  // Chọn chính nó là Loại khiếu nại
                    $("#ddlLoaiKhieuNai").val(obj.id);
                    $("#hdLoaiKhieuNai").val(obj.id).attr("text", obj.name);

                    BindLinhVucChung(obj.id); // Điền dữ liệu Lĩnh vực chung

                    // Dọn dẹp lĩnh vực con
                    $("#ddlLinhVucCon").find("option").remove();
                    $("#ddlLinhVucCon").append('<option value = "0">-- Lĩnh vực con --</option>');
                    $("#hdLinhVucCon").val(0).attr("text", "-- Lĩnh vực con --");
                }
                else if (obj.type == 2) // Tìm được lĩnh vực chung
                {
                    $("#ddlLoaiKhieuNai").val(obj.parentId);
                    $("#hdLoaiKhieuNai").val(obj.parentId).attr("text", obj.parentName);

                    BindLinhVucChung(obj.parentId, obj.id);
                    // Đưa dữ liệu vào hidden field
                    $("#hdLinhVucChung").val(obj.id).attr("text", obj.name);

                    // Dọn dẹp lĩnh vực con
                    $("#ddlLinhVucCon").find("option").remove();
                    $("#ddlLinhVucCon").append('<option value = "0">-- Lĩnh vực con --</option>');
                    $("#hdLinhVucCon").val(0).attr("text", "-- Lĩnh vực con --");

                }
                else // Tìm được lĩnh vực con
                {
                    $("#ddlLoaiKhieuNai").val(obj.loaiKhieuNaiId);
                    $("#hdLoaiKhieuNai").val(obj.loaiKhieuNaiId).attr("text", obj.tenLoaiKhieuNai);

                    BindLinhVucChung(obj.loaiKhieuNaiId, obj.parentId);
                    $("#hdLinhVucChung").val(obj.parentId).attr("text", obj.parentName);

                    BindLinhVucCon(obj.loaiKhieuNaiId, obj.parentId, obj.id);
                    $("#hdLinhVucCon").val(obj.id).attr("text", obj.name);

                }
            }

            var myloader = function (param, success, error) {

                window.needShow = true;
                $(".input-loading").show();
                var q = param.q || '';
                if (q.length <= 0) {
                    window.needShow = false;
                    setTimeout(function () {
                        if (window.needShow == false) $(".input-loading").hide();
                    }, 500);
                    return false
                }
                $.ajax({
                    url: '<%= ResolveClientUrl("~/Views/Popup/Ajax/Handler.ashx") %>',
                    dataType: 'json',
                    data: {
                        type: "ThemMoiKhieuNai",
                        command: "TimKiem",
                        keyword: q,
                    },
                    success: function (data) {
                        if (data.Code == 1) {
                            var items = $.map(data.Data, function (item, index) {
                                return {
                                    rowNumber: index,
                                    id: item.id,
                                    name: item.name,
                                    type: item.type, // 1: Loại khiếu nại, 2: Lĩnh vực chung, 3: Lĩnh vực con
                                    parentId: item.parentId,
                                    parentName: item.parentName,
                                    loaiKhieuNaiId: item.loaiKhieuNaiId,
                                    tenLoaiKhieuNai: item.tenLoaiKhieuNai
                                };
                            });
                            success(items);
                            window.needShow = false;
                            setTimeout(function () {
                                if (window.needShow == false) $(".input-loading").hide();
                            }, 500);
                        }
                        else { alert(data.Message); } // Có lỗi gì đó xảy ra
                    },
                    error: function () {
                        error.apply(this, arguments);
                        window.needShow = false;
                        setTimeout(function () {
                            if (window.needShow == false) $(".input-loading").hide();
                        }, 500);
                    }
                });
            }
        </script>
    </form>
</body>
</html>
