<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KhieuNai_ThemMoi_Old.aspx.cs"
    Inherits="Website.Views.QLKhieuNai.KhieuNai_ThemMoi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title>Khiếu nại - Thêm mới</title>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/reset.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/css/style.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/content/jquery-ui.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/content/easyui.css"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/jquery-1.7.2.min.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/jquery.easyui.min.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/jquery-ui.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("~/JS/plugin/ajaxfileupload.js"), Website.AppCode.Common.Ver)); %>
    <% Response.Write(string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", ResolveClientUrl("/JS/Utility.js"), Website.AppCode.Common.Ver)); %>
    <style type="text/css">
        #contain { position: static; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="false"></asp:ScriptManager>
        <input type="hidden" value="" runat="server" id="hdnWindowUIMODE" />
        <asp:HiddenField ID="hdIsThueBao" runat="server" Value="0" />
        <asp:HiddenField ID="hdIsCallService" runat="server" Value="0" />
        <div class="popup_Container">
            <div style="border-bottom: 1px solid #2A4A50; background: #4D709A; padding: 5px; height: 21px;">
                <h3 id="H2" style="float: left; color: #fff; font-weight: bold;">Thêm mới khiếu nại
                </h3>
                <span style="float: right;"><a href="javascript:CloseForm();">
                    <img src="/Images/x.png">
                </a></span>
            </div>
            <div class="popup_Body">
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="170px">
                        <col width="230px">
                        <col width="130px">
                        <col width="270px">
                    </colgroup>
                    <tr>
                        <td>Số thuê bao&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlLoaiThueBao" runat="server">
                                <asp:ListItem Value="0">Trả trước</asp:ListItem>
                                <asp:ListItem Value="1">Trả sau</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="padding: 0px;">
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
                        <td style="padding-left: 15px;">Loại khiếu nại <span style="color: red">*</span>
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg" style="position: relative">
                                    <select id="ddlLoaiKhieuNai" name="ddlLoaiKhieuNai">
                                    </select>
                                    <input type="hidden" id="hdLoaiKhieuNai" runat="server" value="0" text="" />
                                    <%--<asp:HiddenField ID="hdLoaiKhieuNai" runat="server" Value="0" />--%>
                                    <%-- <asp:DropDownList ID="ddlLoaiKhieuNai" CssClass="ie7selauto" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaiKhieuNai_Changed"
                                                runat="server">
                                                <asp:ListItem>--Loại khiếu nại--</asp:ListItem>
                                            </asp:DropDownList>--%>
                                </div>
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
                        <td style="padding-left: 15px;">Lĩnh vực chung
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg" style="position: relative">
                                    <select id="ddlLinhVucChung" name="ddlLinhVucChung">
                                        <option value="0">--Lĩnh vực chung--</option>
                                    </select>
                                    <asp:HiddenField ID="hdLinhVucChung" runat="server" Value="0" />
                                    <%--<asp:DropDownList ID="ddlLinhVucChung" CssClass="testlonglx" AutoPostBack="true" OnSelectedIndexChanged="ddlLinhVucChung_Changed"
                                                runat="server">
                                                <asp:ListItem Value="0">--Lĩnh vực chung--</asp:ListItem>
                                            </asp:DropDownList>--%>
                                </div>
                            </div>
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
                        <td style="padding-left: 15px;">Lĩnh vực con
                        </td>
                        <td>
                            <select id="ddlLinhVucCon" style="width: 263px" name="ddlLinhVucCon">
                                <option value="0">--Lĩnh vực con--</option>
                            </select>
                            <asp:HiddenField ID="hdLinhVucCon" runat="server" Value="0" />
                            <%-- <asp:DropDownList ID="ddlLinhVucCon" CssClass="ie7selauto" runat="server">
                                                <asp:ListItem Value="0">--Lĩnh vực con--</asp:ListItem>
                                                <asp:ListItem Value="1">Lĩnh vực con</asp:ListItem>
                                                <asp:ListItem Value="2">Nhóm Dịch vụ Data trên nền công nghệ chuyển mạch gói GPRS</asp:ListItem>
                                            </asp:DropDownList>--%>
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
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlDoUuTien" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td style="padding-left: 15px;">HT tiếp nhận
                        </td>
                        <td>
                            <div class="selectstyle_longlx">
                                <div class="bg">
                                    <asp:DropDownList ID="ddlHTTiepNhan" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding-top: 2px; border-top: 1pt dotted #ccc">Địa điểm xảy ra sự cố
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td>Tỉnh/Thành phố <span style="color: red">
                                        <asp:Literal ID="ltRequiedTinh" runat="server">*</asp:Literal></span>
                                    </td>
                                    <td>
                                        <div class="selectstyle_longlx">
                                            <div class="bg">
                                                <select id="ddlTinh" name="ddlTinh">
                                                    <option value="0">--Tỉnh/Thành phố--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="padding-left: 15px;">Quận/Huyện<span style="color: red">
                                        <asp:Literal ID="ltRequiedQuan" Visible="false" runat="server">*</asp:Literal></span>
                                    </td>
                                    <td>
                                        <div class="selectstyle_longlx">
                                            <div class="bg">
                                                <select id="ddlQuanHuyen" name="ddlQuanHuyen">
                                                    <option value="0">--Quận/Huyện--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="padding-left: 15px;">Phường/Xã<span style="color: red">
                                        <asp:Literal ID="ltRequiedPhuong" Visible="false" runat="server">*</asp:Literal></span>
                                    </td>
                                    <td>
                                        <div class="selectstyle_longlx">
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
                            <div style="margin: 5px 0px; border-bottom: 1pt dotted #ccc; height: 1px; overflow: hidden;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>File KH gửi
                        </td>
                        <td colspan="3">
                            <input type="file" id="fUploadKhieuNai" name="fUploadKhieuNai" accept=".jpg, .png, .pdf, .doc, .docx, .xls, .xlsx, .rar, .zip, .7z, .ppt, .pptx, .csv, .mp3" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 2px;"></td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">Nội dung phản ánh <span style="color: red">*</span>
                        </td>
                        <td colspan="3">
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtNoiDungPA" TextMode="MultiLine" Height="50px" runat="server" Text=""
                                        MaxLength="500"></asp:TextBox>
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
                                    <asp:TextBox ID="txtNoiDungCanHoTro" TextMode="MultiLine" Height="50px" runat="server"
                                        Text="" MaxLength="500"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="vertical-align: top">Ghi chú
                        </td>
                        <td>
                            <div class="inputstyle_longlx">
                                <div class="bg">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Height="50px" runat="server" Text=""
                                        MaxLength="500"></asp:TextBox>
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

        <script language="javascript" type="text/javascript">
            var flagCall = false;
            function ThemMoiKhieuNai() {
                if (flagCall) return;
                flagCall = true;

                //console.log($('#<%=ddlLoaiThueBao.ClientID%>').val());
                var typeKhieuNai = Utility.GetUrlParam('type');
                if (validForm()) {
                    Common.Loading();
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
                                Common.UnLoading();
                            }
                            else if (outputItem.ErrorId == -1000) {
                                MessageAlert.AlertJSON(outputItem.ErrorId);
                                flagCall = false;
                                Common.UnLoading();
                            }
                            else {
                                MessageAlert.AlertNormal(outputItem.Message, 'error');
                                flagCall = false;
                                Common.UnLoading();
                            }
                        },
                        error: function (e) {
                            flagCall = false;
                            Common.UnLoading();
                            CloseForm();
                        },
                        complete: function () {
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
                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=BindLoaiKhieuNaiNew",
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlLoaiKhieuNai').html(data);
                        EventChangeLoaiKhieuNai();
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=3",
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        $('#ddlLinhVucCon').html(data);
                        //console.log('longlx');
                        //console.log(navigator);
                        //console.log(navigator.userAgent);
                        //var isIE11 = !!navigator.userAgent.match(/Trident.*rv\:11\./);
                        //alert(checkIEVersion());
                        //console.log(navigator.appName);
                        //checkIEVersion();
                        //if(navigator.appName.indexOf("Internet Explorer")!=-1){     //yeah, he's using IE
                        if (checkIEVersion())
                            $("#ddlLinhVucCon").combobox();
                        else
                            EventChangeLinhVucCon();
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

            function getInternetExplorerVersion()
                // Returns the version of Windows Internet Explorer or a -1
                // (indicating the use of another browser).
            {
                var rv = -1; // Return value assumes failure.
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    var ua = navigator.userAgent;
                    var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                    if (re.exec(ua) != null)
                        rv = parseFloat(RegExp.$1);
                }
                return rv;
            }
            function checkIEVersion() {
                var isIE11 = !!navigator.userAgent.match(/Trident.*rv\:11\./);
                if (isIE11) {
                    return false;
                }
                else {
                    if (navigator.appName == 'Microsoft Internet Explorer') {
                        return false;
                    }
                    return true;
                }
            }

            //function LoadComboboxLinhVucCon()
            //{
            //    $("#ddlLinhVucCon").combobox();
            //}

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

            function EventChangeLoaiKhieuNai() {
                $('#ddlLoaiKhieuNai').change(function (e) {
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

            function BindLinhVucChung(LoaiKhieuNaiId) {
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

            function BindLinhVucCon(LoaiKhieuNaiId, LinhVucChungId) {
                $.ajax({
                    type: "GET",
                    url: "/Views/QLKhieuNai/Handler/HandlerThemMoi.ashx?key=3",
                    data: {
                        LoaiKhieuNaiId: LoaiKhieuNaiId,
                        LinhVucChungId: LinhVucChungId
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (data) {
                        //console.log(1);
                        $('#ddlLinhVucCon').html(data);

                        //$("#ddlLinhVucCon").change();
                        //$("#ddlLinhVucCon").val("--Lĩnh vực con--").trigger("change");
                        //$('#ddlLinhVucCon').text("--Lĩnh vực con--");
                        $("#ddlLinhVucCon").val(0);

                        if (checkIEVersion())
                            $("#ddlLinhVucCon").combobox();
                        else
                            EventChangeLinhVucCon();

                        $('.ui-autocomplete-input').focus().val($("#ddlLinhVucCon option:first").text());
                    }
                });
            }
        </script>

        <script type="text/javascript">

            function optionSelected(selectedValue) {
                document.title = selectedValue;
            }


            (function ($) {
                //console.log(112);
                $.widget("ui.combobox", {

                    _create: function () {
                        //console.log(11);
                        var self = this,
                                   select = this.element.hide(),
                                   selected = select.children(":selected"),
                                   value = selected.val() ? selected.text() : "";
                        //console.log(self);
                        var input = this.input = $("<input>")
                                   .insertAfter(select)
                                   .val(value)
                                   .autocomplete({
                                       delay: 0,
                                       minLength: 0,
                                       source: function (request, response) {
                                           //console.log('src');
                                           //value = 'IDD1714';
                                           //this.search(value, null);
                                           //alert(1);
                                           var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                                           response(select.children("option").map(function () {
                                               var text = $(this).text();
                                               var strLoaiKhieuNai = $(this).attr("loaikhieunai");
                                               if (strLoaiKhieuNai == undefined)
                                                   strLoaiKhieuNai = "";
                                               if (this.value && (!request.term || matcher.test(text)))
                                                   return {
                                                       label: text.replace(
                                                                            new RegExp(
                                                                                   "(?![^&;]+;)(?!<[^<>]*)(" +
                                                                                   $.ui.autocomplete.escapeRegex(request.term) +
                                                                                   ")(?![^<>]*>)(?![^&;]+;)", "gi"
                                                                            ), "<strong>$1</strong>") + '<span style="float: right; font-style: italic;">' + strLoaiKhieuNai + '</span>',
                                                       value: text,
                                                       option: this
                                                   };
                                           }));
                                           //input.autocomplete("search", "");
                                       },

                                       select: function (event, ui) {
                                           //console.log("select");
                                           //Làm ở đây nè
                                           ui.item.option.selected = true;
                                           self._trigger("selected", event, {
                                               item: ui.item.option
                                           });
                                           //JK
                                           //console.log($(ui.item.option).attr('loaikhieunaiid'));
                                           if ($(ui.item.option).attr('loaikhieunaiid') != "0") {
                                               $('#ddlLoaiKhieuNai').val($(ui.item.option).attr('loaikhieunaiid'));
                                               BindLinhVucChungNotRefresh($(ui.item.option).attr('loaikhieunaiid'), $(ui.item.option).attr('linhvucchungid'));
                                               //$('#ddlLinhVucChung').val($(ui.item.option).attr('linhvucchungid'));

                                               $('#hdLoaiKhieuNai').val($(ui.item.option).attr('loaikhieunaiid'));
                                               $('#hdLoaiKhieuNai').attr('text', $("#ddlLoaiKhieuNai option:selected").text());
                                           }

                                           $('#hdLinhVucCon').val(ui.item.option.value);
                                           $('#hdLinhVucCon').attr('text', ui.item.option.text);
                                           optionSelected(ui.item.option.value);

                                       },
                                       change: function (event, ui) {
                                           //console.log("change");
                                           if (!ui.item) {
                                               var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
                                                               valid = false;
                                               select.children("option").each(function () {
                                                   if ($(this).text().match(matcher)) {
                                                       this.selected = valid = true;
                                                       return false;
                                                   }
                                               });
                                               if (!valid) {
                                                   // remove invalid value, as it didn't match anything
                                                   $(this).val("");
                                                   select.val("");
                                                   input.data("autocomplete").term = "";
                                                   return false;
                                               }
                                           }
                                       }
                                   })
                                   .addClass("ui-widget ui-widget-content ui-corner-left");


                        input.data("autocomplete")._suggest = function (items) {
                            var ul = this.menu.element
			                        .empty()
			                        .zIndex(this.element.zIndex() + 1);
                            this._renderMenu(ul, items);
                            // TODO refresh should check if the active item is still in the dom, removing the need for a manual deactivate
                            this.menu.deactivate();
                            this.menu.refresh();

                            // size and position menu
                            ul.show();

                            ul.position($.extend({
                                of: this.element
                            }, this.options.position));
                            $('.ui-autocomplete').css("left", "240px");
                            $('.ui-autocomplete').css("overflow", "scroll");
                            $('.ui-autocomplete').css("height", "300px");
                            ul.outerWidth(550);
                        };

                        input.data("autocomplete")._resizeMenu = function (ul, items) {
                        };

                        input.data("autocomplete")._renderMenu = function (ul, items) {
                            var self = this;
                            $.each(items, function (index, item) {
                                self._renderItem(ul, item);
                            });
                        };

                        input.data("autocomplete")._renderItem = function (ul, item) {
                            return $("<li></li>")
                                          .data("item.autocomplete", item)
                                          .append("<a>" + item.label + "</a>")
                                           .appendTo(ul);
                        };

                        this.button = $("<button type='button'>&nbsp;</button>")
                                   .attr("tabIndex", -1)
                                   .attr("title", "Show All Items")
                                   .insertAfter(input)
                                   .button({
                                       icons: {
                                           primary: "ui-icon-triangle-1-s"
                                       },
                                       text: false
                                   })
                                   .removeClass("ui-corner-all")
                                   .addClass("ui-corner-right ui-button-icon")
                                   .click(function () {
                                       // close if already visible
                                       //console.log(1);
                                       if (input.autocomplete("widget").is(":visible")) {
                                           input.autocomplete("close");
                                           return;
                                       }

                                       // pass empty string as value to search for, displaying all results
                                       input.autocomplete("search", "");
                                       input.focus();

                                       //console.log($('.ui-autocomplete-input').val());
                                   });
                    },

                    destroy: function () {
                        this.input.remove();
                        this.button.remove();
                        this.element.show();
                        $.Widget.prototype.destroy.call(this);
                    }
                });
            })(jQuery);

        </script>

        <style>
            .ui-button { margin-left: -1px; }

            .ui-button-icon-only .ui-button-text { padding: 0.35em; }

            .ui-autocomplete-input { margin: 0; padding: 0.30em 0 0.15em 0.30em; width: 84%; }
        </style>
    </form>
</body>
</html>
