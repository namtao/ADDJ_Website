<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachHoTroXuLyHT.ascx.cs" Inherits="Website.HeThongHoTro.Dashboards.WebUserControl1" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>


<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" EnableTheming="True" HeaderText="Danh sách các yêu cầu hỗ trợ cần phải xử lý chuyển đến" Height="237px" ShowCollapseButton="True" Theme="Office2010Blue" Width="100%">
    <PanelCollection>
        <dx:PanelContent runat="server">
            <script>
                function ChonDonVi(s, e) {
                    var tendv = '';
                    var key = treelist_donvi_cc.GetFocusedNodeKey();
                    treelist_donvi_cc.GetNodeValues(key, "linhvuc", function (value) {
                        LoaiHoTro.SetText(value);
                        tenDonVi = value;
                        LoaiHoTro.SetKeyValue(key);
                        LoaiHoTro.HideDropDown();
                        //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
                    });
                }
                var postponedCallbackRequired = false;
                //Chọn hệ thống hỗ trợ kỹ thuật
                function OnListBoxIndexChanged(s, e) {
                    debugger;
                    if (s.GetValue() == '0') {
                        LoaiHoTro.SetText('');
                        LoaiHoTro.SetKeyValue('0');
                    }


                    HiddenField.Set('hidden_value', s.GetValue());
                    HiddenField.Set('hidden_value2', 1);
                    HiddenField.Set('res_value', 1);
                    //treelist_donvi_cc.PerformCallback('refresh');
                    if (CallbackPanel.InCallback())
                        postponedCallbackRequired = true;
                    else
                        CallbackPanel.PerformCallback();

                }
                function OnEndCallback(s, e) {
                    if (postponedCallbackRequired) {
                        CallbackPanel.PerformCallback();
                        postponedCallbackRequired = false;
                    }
                }



                $(function () {
                    //loadMucDoYeuCauHeThongXL();
                    DsYeuCauHoTroXuLy.PerformCallback();
                });

                function napthongtinHeThong() {
                    if (LoaiHoTro.GetText() == '')
                        LoaiHoTro.SetKeyValue('');
                    DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue() + '|' + cboMucDoYeuCauXLSearch.GetValue() + '|' + LoaiHoTro.GetKeyValue() + '|' + txtSoDienThoaiXL.GetText());
                }

                function loadTTHoTroXuLy(val) {
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadThongTinXuLyHT',
                        data: "{ idchitietxlhotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            hiddenIDHoTroChiTietXuLy.Set('idhethong_yc_hotro', objret.ID_HETHONG_YCHT);
                            hiddenIDHoTroChiTietXuLy.Set('idluonghotro', objret.ID_LUONG_HOTRO);
                            hiddenIDHoTroChiTietXuLy.Set('iddonvi', objret.ID_DONVI);
                        },
                        error: function () {
                        }
                    });
                }

                function loadToanBoThongTinYeuCauHoTro(val) {
                    debugger;
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadToanBoThongTinYeuCauHoTro',
                        data: "{ idchitietxlhotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            lblSoDienThoaiXL.SetText(objret.SODIENTHOAI);
                            lblSoDienThoaiXL1.SetText(objret.SODIENTHOAI);
                            lblMucDoYeuCauXL.SetText(objret.TENMUCDO);
                            lblMucDoYeuCauXL1.SetText(objret.TENMUCDO);
                            lblTenLuongHoTro.SetText(objret.TEN_LUONG);
                            lblTenLuongHoTro1.SetText(objret.TEN_LUONG);
                            lblMoTaLuongHoTro.SetText(objret.MOTA);
                            lblMoTaLuongHoTro1.SetText(objret.MOTA);
                            lblTenHeThongHoTroKyThuat.SetText(objret.TENHETHONG);
                            lblTenHeThongHoTroKyThuat1.SetText(objret.TENHETHONG);
                            lblNoiDungYeuCauHoTroGoc.SetText(objret.NOIDUNG_YEUCAU);
                            lblNoiDungYeuCauHoTroGoc1.SetText(objret.NOIDUNG_YEUCAU);
                            lblLinhVucCon.SetText(objret.LINHVUC);
                            lblLinhVucCon1.SetText(objret.LINHVUC);

                            loadTimelineHoTroXuLy(objret.ID_YEU_CAU_HOTRO, 1);
                        },
                        error: function () {
                        }
                    });
                }

                function loadTimelineHoTroXuLy(val, val2) {
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadTimeLineXuLyHT',
                        data: "{ idyeucauhotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        beforeSend: function () {
                            loadingdata.Show();
                        },
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            $("#timeLine tbody").html("");
                            var table = '';
                            var tr;
                            for (var i = 0; i < objret.length; i++) {
                                tr = $('<tr/>');
                                tr.append("<td><strong>" + objret[i].NGAYXULY + "<strong></td>");
                                tr.append("<td>" + objret[i].ID_NODE_LUONG_HOTRO + "</td>");
                                tr.append("<td>" + objret[i].NOIDUNGXULY + "</td>");
                                tr.append("<td>" + objret[i].ID_DONVI_TO + "</td>");
                                tr.append("<td>" + objret[i].DONVIXULY + "</td>");
                                tr.append("<td>" + objret[i].NGUOIXULY + "</td>");
                                tr.append("<td>" + objret[i].TEN_NGUOIXULY + "</td>");
                                tr.append("<td>" + objret[i].DT_NGUOIXULY + "</td>");
                                tr.append("<td><a onclick=\"xemThongTinChiTietDaPhuongTien('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/view_16x16.gif' alt='Smiley face'> Xem</a></td>");
                                tr.append("<td><a onclick=\"loadFileAttach('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/file_16x16.gif' alt='Smiley face'> File</a></td>");
                                $("#timeLine tbody").append(tr);
                            }
                            loadingdata.Hide();
                        },
                        error: function () {
                            loadingdata.Hide();
                            alert('có lỗi xảy ra khi lấy timeline luồng xử lý');
                        }
                    });
                }

                function luongxulyHayChuyenTiepHT(val) {

                    // xóa nội dung xử lý (nếu có)
                    txtNoiDungXuLyHT0.SetText('');
                    txtNoiDungXuLyHT1.SetText('');

                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/checkLuongXuLyHT',
                        data: "{ id_yeucau_xuly_hotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        beforeSend: function () {
                            // setting a timeout
                            loadingdata.Show();
                        },
                        success: function (jsonData) {
                            if (jsonData.d == '2' || jsonData.d == '1') // nếu chuyển tiếp
                            {
                                debugger;
                                checkQuyenDongHoTro(val);
                                popupXuLyPhanHoiChuyenTiepDongHT.ShowAtPos(100, 100);       // hiện popup chuyển tiếp

                                // nếu là gốc thì k có nút phản hồi
                                if (jsonData.d == '1') {
                                    btnChuyenPhanHoiChoDonViKhacXuLy.SetVisible(false);
                                    btnChuyenPhanHoiChoDonViKhacXuLyCT.SetVisible(false);
                                }
                                else {
                                    btnChuyenPhanHoiChoDonViKhacXuLy.SetVisible(true);
                                    btnChuyenPhanHoiChoDonViKhacXuLyCT.SetVisible(true);
                                }

                                // nạp danh sách các đơn vị chuyển tiếp theo
                                // lấy thông tin về đơn vị cần chuyển đến
                                loadDonViChuyenDenTheoIdXuLyHoTro(hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy'));
                            }
                            else if (jsonData.d == '3')  // nếu xử lý luôn
                            {
                                popupXuLyPhanHoiVaXuLyLuonHT.ShowAtPos(100, 100);
                            } else // nếu chỉ xem
                            {
                                // hide
                                btnXuLyHoTroLuon.SetVisible(false);
                                popupXuLyPhanHoiVaXuLyLuonHT.ShowAtPos(100, 100);  // hiện popup xử lý luôn
                                // mặc định chuyển về đơn vị yêu cầu (trạng thái 3)
                            }
                            loadingdata.Hide();
                        },
                        error: function () {
                            loadingdata.Hide();
                        }
                    });
                }

                function XuLyLuon() {
                    if (!confirm('YCHT sẽ được chuyển về đơn vị ban đầu yêu cầu để đơn vị đó kiểm tra và xác nhận kết quả, Bạn muốn tiếp tục.?')) {
                        return;
                    }



                    // xử lý luôn này nằm ở form node cuối cùng của luồng
                    if (txtNoiDungXuLyHT0.GetText() == '') {
                        alert('Bạn phải nhập nội dung xử lý');
                        return;
                    }
                 
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/xuLyLuonHoTro',
                        data: "{ id_yeucau_xuly_hotro: " + hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy') + " , noidungxuly: '" + txtNoiDungXuLyHT0.GetValue() + "', noidungxulychitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "', iddonvi_from: '" + hiddenIDHoTroChiTietXuLy.Get('iddonvi_from') + "', iddonvi_to:'', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "', id_nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('id_nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var arr_info = jsonData.d.split('|');
                            debugger;
                            // idhethong, idyeucauhotro, idxuly_ycht
                            updateSubmissionFileAttach(arr_info[0], arr_info[1], arr_info[2]);

                            alert('Đã xử lý thành công, yêu cầu hỗ trợ đã chuyển đến đơn vị yêu cầu kiểm tra và xác nhận.');

                            popupXuLyPhanHoiVaXuLyLuonHT.Hide();

                            //location.reload(true);
                            DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());

                            DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi chuyển thông tin luồng hỗ trợ, vui lòng thử lại sau!');
                        }
                    });
                }

                function ChuyenPhanHoi(loaichuyen) {
                    if (!confirm('YCHT sẽ được chuyển lại đơn vị vừa chuyển YCHT cho bạn. Bạn muốn thực hiện?'))
                        return;

                    var noidungchuyen = '';
                    if (loaichuyen == '1') {
                        noidungchuyen = txtNoiDungXuLyHT0.GetText();   // nếu phản hổi ở nút cuối luồng
                    }
                    else {
                        noidungchuyen = txtNoiDungXuLyHT1.GetText();  // nếu phản hồi ở giữa luồng
                    }
                    if (noidungchuyen == '') {
                        alert('Bạn phải nhập nội dung xử lý!');
                        return;
                    }

                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/chuyenPhanHoi',
                        data: "{ id_ht_xuly_hotro: " + hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy') + " , noidungxuly: '" + noidungchuyen + "', noidungxulychitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "', iddonvi_from: '" + hiddenIDHoTroChiTietXuLy.Get('iddonvi_from') + "', iddonvi_to:'', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "', id_nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('id_nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            alert('Đã chuyển phản hồi thành công');
                            var arr_info = jsonData.d.split('|');
                            debugger;
                            // idhethong, idyeucauhotro, idxuly_ycht
                            updateSubmissionFileAttach(arr_info[0], arr_info[1], arr_info[2]);

                            popupXuLyPhanHoiVaXuLyLuonHT.Hide();

                            if (loaichuyen == '1') { // đóng ở
                                popupXuLyPhanHoiVaXuLyLuonHT.Hide();
                            }
                            else {
                                popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            }

                            //location.reload(true);
                            DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());
                            DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                        },
                        error: function () {
                            alert('Có lỗi khi thực hiện chuyển phản hồi, bạn thử lại sau');
                        }
                    });
                }
                function ChuyenTiepXuLy() {
                    //debugger;
                    if (!confirm('Bạn muốn chuyển tiếp yêu cầu hỗ trợ cho đơn vị tiếp theo xử lý?'))
                        return;

                    if (cboDonViChuyenDen.GetValue() == 0 || cboDonViChuyenDen.GetValue() == null) {
                        alert('Bạn phải chọn đơn vị chuyển đến!');
                        return;
                    }
                    if (txtNoiDungXuLyHT1.GetText() == '') {
                        alert('Bạn phải nhập nội dung xử lý!');
                        return;
                    }
                    var v1 = hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy');
                    var v2 = txtNoiDungXuLyHT1.GetValue();
                    var v3 = cboDonViChuyenDen.GetSelectedItem().value;
                    var v4 = hiddenIDHoTroChiTietXuLy.Get('nguoixuly');
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/chuyenTiepHoTro',
                        data: "{ id_xuly_yeucau_hotro: " + hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy') + " , noidungxuly: '" + txtNoiDungXuLyHT1.GetValue() + "', noidungxulychitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "', iddonvi_from: '" + hiddenIDHoTroChiTietXuLy.Get('iddonvi_from') + "', iddonvi_to:'" + cboDonViChuyenDen.GetSelectedItem().value + "', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "', id_nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('id_nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var arr_info = jsonData.d.split('|');
                            debugger;
                            // idhethong, idyeucauhotro, idxuly_ycht
                            updateSubmissionFileAttach(arr_info[0], arr_info[1], arr_info[2]);

                            alert('Đã tạo chuyển xử lý thành công!.');
                            popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            //location.reload(true);
                            DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());
                            DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi chuyển thông tin luồng hỗ trợ, vui lòng thử lại sau!');
                        }
                    });
                }

                function loadDonViChuyenDen() {
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadDonViChuyenDen',
                        data: "{ idhethong_yc_hotro: " + hiddenIDHoTroChiTietXuLy.Get('idhethong_yc_hotro') + " , idluonghotro: '" + hiddenIDHoTroChiTietXuLy.Get('idluonghotro') + "', iddonvi:'" + hiddenIDHoTroChiTietXuLy.Get('iddonvi') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            for (var i = 0; i < objret.length; i++) {
                                cboDonViChuyenDen.AddItem(objret[i].TENDONVI, objret[i].ID_DONVI);
                            }
                        },
                        error: function () {
                        }
                    });
                }

                function loadDonViChuyenDenTheoIdXuLyHoTro(val) {
                    //debugger;
                    cboDonViChuyenDen.ClearItems();
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadDonViChuyenDenTheoIdXuLyHoTro',
                        data: "{ id_xuly_yc_hotro: '" + val + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            for (var i = 0; i < objret.length; i++) {
                                cboDonViChuyenDen.AddItem(objret[i].TENDONVI, objret[i].ID_DONVI);
                            }
                        },
                        error: function () {
                        }
                    });
                }
                function checkQuyenDongHoTro(val) {
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/checkQuyenDongHoTro',
                        data: "{ username: '" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "',id_xuly_yc_hotro: '" + hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy') + "'  }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            if (jsonData.d == '1') {
                                btnDongYeuCauHoTro.SetVisible(true);
                            } else {
                                btnDongYeuCauHoTro.SetVisible(false);
                            }
                        },
                        error: function () {
                            alert('Có lỗi khi lấy thông tin đóng hỗ trợ');
                        }
                    });
                }
                function xulyDongYeuCauHoTro() {
                    if (txtNoiDungXuLyHT1.GetText() == '') {
                        alert('Bạn phải nhập nội dung đóng hỗ trợ');
                        return;
                    }
                    if (confirm('Bạn muốn thực hiện đóng yêu cầu hỗ trợ này?. Bạn hãy kiểm tra lại chính xác về việc xử lý hỗ trợ này đã đáp ứng được chưa, nếu chưa hãy thực hiện chuyển tiếp. Bạn tiếp tục đóng?')) {
                        $.ajax({
                            type: 'POST',
                            url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/xulyDongYeuCauHoTro',
                            data: "{ id_xuly_yc_hotro: '" + hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy') + "',noidungdong: '" + txtNoiDungXuLyHT1.GetValue() + "',noidungdongchitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "',id_donvidong: '" + hiddenIDHoTroChiTietXuLy.Get('iddonvi_from') + "',nguoidong: '" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "',id_nguoidong: '" + hiddenIDHoTroChiTietXuLy.Get('id_nguoixuly') + "'  }",
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (jsonData) {
                                if (jsonData.d == '1') {
                                    alert('Đã đóng yêu cầu hỗ trợ thành công!');
                                    popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                                    //location.reload(true);
                                    DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());
                                    DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                                } else {
                                    alert('Có lỗi khi đóng hỗ trợ. Mã lỗi:' + jsonData.d);
                                }
                            },
                            error: function () {
                                alert('Có lỗi khi đóng hỗ trợ');
                            }
                        });
                    }
                }

                function xemThongTinChiTietDaPhuongTien(val) {
                    popupNoiDungDaPhuongTienHT.Show();
                }


                // Xử lý upload attach files
                var uploadInProgress = false,
                    submitInitiated = false,
                    uploadErrorOccurred = false;
                uploadedFiles = [];
                function onFileUploadComplete(s, e) {
                    var callbackData = e.callbackData.split("|"),
                        uploadedFileName = callbackData[0],
                        isSubmissionExpired = callbackData[1] === "True";

                    uploadedFiles.push(uploadedFileName);

                    if (e.errorText.length > 0 || !e.isValid)
                        uploadErrorOccurred = true;

                    // control 0001
                    if (isSubmissionExpired && UploadedFilesTokenBox0001.GetText().length > 0) {
                        var removedAfterTimeoutFiles = UploadedFilesTokenBox0001.GetTokenCollection().join("\n");
                        alert("The following files have been removed from the server due to the defined 5 minute timeout: \n\n" + removedAfterTimeoutFiles);
                        UploadedFilesTokenBox0001.ClearTokenCollection();
                    }

                    // control 0002
                    if (isSubmissionExpired && UploadedFilesTokenBox0002.GetText().length > 0) {
                        var removedAfterTimeoutFiles = UploadedFilesTokenBox0002.GetTokenCollection().join("\n");
                        alert("The following files have been removed from the server due to the defined 5 minute timeout: \n\n" + removedAfterTimeoutFiles);
                        UploadedFilesTokenBox0002.ClearTokenCollection();
                    }


                    // control 0003
                    if (isSubmissionExpired && UploadedFilesTokenBox0003.GetText().length > 0) {
                        var removedAfterTimeoutFiles = UploadedFilesTokenBox0003.GetTokenCollection().join("\n");
                        alert("The following files have been removed from the server due to the defined 5 minute timeout: \n\n" + removedAfterTimeoutFiles);
                        UploadedFilesTokenBox0003.ClearTokenCollection();
                    }


                    // control 0004
                    if (isSubmissionExpired && UploadedFilesTokenBox0004.GetText().length > 0) {
                        var removedAfterTimeoutFiles = UploadedFilesTokenBox0004.GetTokenCollection().join("\n");
                        alert("The following files have been removed from the server due to the defined 5 minute timeout: \n\n" + removedAfterTimeoutFiles);
                        UploadedFilesTokenBox0004.ClearTokenCollection();
                    }
                }
                function onFileUploadStart(s, e) {
                    uploadInProgress = true;
                    uploadErrorOccurred = false;

                    // control 0001
                    UploadedFilesTokenBox0001.SetIsValid(true);
                    // control 0002
                    UploadedFilesTokenBox0002.SetIsValid(true);
                    // control 0003
                    UploadedFilesTokenBox0003.SetIsValid(true);
                    // control 0004
                    UploadedFilesTokenBox0004.SetIsValid(true);
                }
                function onFilesUploadComplete(s, e) {
                    uploadInProgress = false;
                    for (var i = 0; i < uploadedFiles.length; i++) {
                        // control 0001
                        UploadedFilesTokenBox0001.AddToken(uploadedFiles[i]);
                        // control 0002
                        UploadedFilesTokenBox0002.AddToken(uploadedFiles[i]);
                        // control 0003
                        UploadedFilesTokenBox0003.AddToken(uploadedFiles[i]);
                        // control 0004
                        UploadedFilesTokenBox0004.AddToken(uploadedFiles[i]);
                    }

                    updateTokenBoxVisibility0001();
                    updateTokenBoxVisibility0002();
                    updateTokenBoxVisibility0003();
                    updateTokenBoxVisibility0004();
                    uploadedFiles = [];
                    //if(submitInitiated) {
                    //    SubmitButton.SetEnabled(true);
                    //    SubmitButton.DoClick();
                    //}
                }

                function onTokenBoxValidation0001(s, e) {
                    var isValid = DocumentsUploadControl0001.GetText().length > 0 || UploadedFilesTokenBox0001.GetText().length > 0;
                    e.isValid = isValid;
                    if (!isValid) {
                        e.errorText = "No files have been uploaded. Upload at least one file.";
                    }
                }
                function onTokenBoxValidation0002(s, e) {
                    var isValid = DocumentsUploadControl0002.GetText().length > 0 || UploadedFilesTokenBox0002.GetText().length > 0;
                    e.isValid = isValid;
                    if (!isValid) {
                        e.errorText = "No files have been uploaded. Upload at least one file.";
                    }
                }

                function onTokenBoxValidation0003(s, e) {
                    var isValid = DocumentsUploadControl0003.GetText().length > 0 || UploadedFilesTokenBox0003.GetText().length > 0;
                    e.isValid = isValid;
                    if (!isValid) {
                        e.errorText = "No files have been uploaded. Upload at least one file.";
                    }
                }

                function onTokenBoxValidation0004(s, e) {
                    var isValid = DocumentsUploadControl0004.GetText().length > 0 || UploadedFilesTokenBox0004.GetText().length > 0;
                    e.isValid = isValid;
                    if (!isValid) {
                        e.errorText = "No files have been uploaded. Upload at least one file.";
                    }
                }
                function onTokenBoxValueChanged0001(s, e) {
                    updateTokenBoxVisibility0001();
                }
                function onTokenBoxValueChanged0002(s, e) {
                    updateTokenBoxVisibility0002();
                }
                function onTokenBoxValueChanged0003(s, e) {
                    updateTokenBoxVisibility0003();
                }
                function onTokenBoxValueChanged0004(s, e) {
                    updateTokenBoxVisibility0004();
                }

                // control 0001
                function updateTokenBoxVisibility0001() {
                    var isTokenBoxVisible = UploadedFilesTokenBox0001.GetTokenCollection().length > 0;
                    UploadedFilesTokenBox0001.SetVisible(isTokenBoxVisible);
                }
                // control 0002
                function updateTokenBoxVisibility0002() {
                    var isTokenBoxVisible = UploadedFilesTokenBox0002.GetTokenCollection().length > 0;
                    UploadedFilesTokenBox0002.SetVisible(isTokenBoxVisible);
                }
                // control 0003
                function updateTokenBoxVisibility0003() {
                    var isTokenBoxVisible = UploadedFilesTokenBox0003.GetTokenCollection().length > 0;
                    UploadedFilesTokenBox0003.SetVisible(isTokenBoxVisible);
                }
                // control 0004
                function updateTokenBoxVisibility0004() {
                    var isTokenBoxVisible = UploadedFilesTokenBox0004.GetTokenCollection().length > 0;
                    UploadedFilesTokenBox0004.SetVisible(isTokenBoxVisible);
                }

                function updateSubmissionFileAttach(val, val1, val2) {
                    var sbis = HiddenField.Get('SubmissionID');
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_tapTinDinhKem.asmx/updateSubmisisonInfo',
                        data: "{ submissionid: '" + HiddenField.Get('SubmissionID') + "',idhethong: '" + val + "',idyeucauhotro: '" + val1 + "',idxuly_ycht: '" + val2 + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            //alert('update thành công');
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi lưu file, vui lòng thử lại sau!');
                        }
                    });
                }

                function loadFileAttach(val) {
                    popupDanhSachFileAttach.Show();

                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_taptindinhkem.asmx/danhSachFileAttach',
                        data: "{ id_xuly_ycht: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        beforeSend: function () {
                            loadingdata.Show();
                        },
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            $("#dsFileAttach tbody").html("");
                            var table = '';
                            var tr;
                            for (var i = 0; i < objret.length; i++) {
                                tr = $('<tr/>');
                                tr.append("<td><strong>" + (i + 1) + "<strong></td>");
                                tr.append("<td><strong>" + objret[i].TENFILE + "<strong></td>");
                                tr.append("<td>" + objret[i].NGAYTAO + "</td>");
                                tr.append("<td><a href=\"/HTHTKT/UploadAttachFiles/" + objret[i].TENFILETAIVE + "\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/check-in_16x16.gif' alt='Smiley face'> Tải về</a></td>");
                                $("#dsFileAttach tbody").append(tr);
                            }
                            loadingdata.Hide();
                        },
                        error: function () {
                            loadingdata.Hide();
                            alert('có lỗi xảy ra khi lấy thông tin file attach');
                        }
                    });
                }

                // function loadMucDoYeuCauHeThongXL() {
                //    debugger;
                //    cboMucDoYeuCauXLSearch.ClearItems();
                //    $.ajax({
                //        type: 'POST',
                //        url: '/HeThongHoTro/Services/ws_thamSo.asmx/thongtinMucDoSuCo',
                //        data: "{ id: '" + 0 + "' }",
                //        contentType: 'application/json; charset=utf-8',
                //        dataType: 'json',
                //        success: function (jsonData) {
                //            var objret = JSON.parse(jsonData.d);
                //            cboMucDoYeuCauXLSearch.AddItem('---Chọn mức độ---', '0');
                //            for (var i = 0; i < objret.length; i++) {
                //                cboMucDoYeuCauXLSearch.AddItem(objret[i].TENMUCDO, objret[i].ID);
                //            }
                //            cboMucDoYeuCauXLSearch.SetValue("0");
                //        },
                //        error: function () {
                //        }
                //    });
                //}


                // lấy danh sách các ID xử lý yêu cầu hỗ trợ
                function OnGetSelectedFieldValues(selectedValues) {
                    //listBox.ClearItems();
                    if (selectedValues.length == 0) return;
                    var lstID = [];
                    for (i = 0; i < selectedValues.length; i++) {
                        s = "";
                        for (j = 0; j < selectedValues[i].length; j++) {
                            s = s + selectedValues[i][j] + "&nbsp;";
                        }
                        lstID.push(selectedValues[i])
                        //listBox.AddItem(s);
                    }
                    hiddenIDHoTroChiTietXuLy.Set('lst_di_xuly_ht_hangloat', lstID);

                    // hiển thị hộp thoại xử lý hàng hoạt
                    popupXuLyPhanHoiChuyenTiepDongHangLoatHT.Show();
                }

                function chuyenTiepHangLoat(arr_id_xl_hotro) {
                    //debugger;
                    if (!confirm('Bạn muốn chuyển tiếp yêu cầu hỗ trợ hàng loạt?'))
                        return;

                    if (cboDonViChuyenDen.GetValue() == 0 || cboDonViChuyenDen.GetValue() == null) {
                        alert('Bạn phải chọn đơn vị chuyển đến!');
                        return;
                    }
                    if (txtNoiDungXuLyHT1.GetText() == '') {
                        alert('Bạn phải nhập nội dung xử lý!');
                        return;
                    }
                    var v1 = hiddenIDHoTroChiTietXuLy.Get('hiddenIDHoTroChiTietXuLy');
                    var v2 = txtNoiDungXuLyHT1.GetValue();
                    var v3 = cboDonViChuyenDen.GetSelectedItem().value;
                    var v4 = hiddenIDHoTroChiTietXuLy.Get('nguoixuly');
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/chuyenTiepHangLoat',
                        data: "{ id_xuly_yeucau_hotro: " + hiddenIDHoTroChiTietXuLy.Get('lst_di_xuly_ht_hangloat') + " , noidungxuly: '" + txtNoiDungXuLyHT1.GetValue() + "', noidungxulychitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "', donvichuyenden:'" + cboDonViChuyenDen.GetSelectedItem().value + "', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var arr_info = jsonData.d.split('|');
                            debugger;
                            // idhethong, idyeucauhotro, idxuly_ycht
                            updateSubmissionFileAttach(arr_info[0], arr_info[1], arr_info[2]);

                            alert('Đã tạo chuyển xử lý thành công!.');
                            popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            //location.reload(true);
                            DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());
                            DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi chuyển thông tin luồng hỗ trợ hàng loạt, vui lòng thử lại sau!');
                        }
                    });
                }

                function chuyenPhanHoiHangLoat() {
                    if (!confirm('Bạn muốn chuyển phản hồi lại yêu cầu hỗ trợ hàng loạt?'))
                        return;

                    var noidungchuyen = '';
                    if (loaichuyen == '1') {
                        noidungchuyen = txtNoiDungXuLyHT0.GetText();   // nếu phản hổi ở nút cuối luồng
                    }
                    else {
                        noidungchuyen = txtNoiDungXuLyHT1.GetText();  // nếu phản hồi ở giữa luồng
                    }
                    if (noidungchuyen == '') {
                        alert('Bạn phải nhập nội dung xử lý!');
                        return;
                    }

                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/chuyenPhanHoiHangLoat',
                        data: "{ id_ht_xuly_hotro: " + hiddenIDHoTroChiTietXuLy.Get('lst_di_xuly_ht_hangloat') + " , noidungxuly: '" + noidungchuyen + "', noidungxulychitiet: '" + htmlNoiDungXuLyDaPhuongTienChiTietHT.GetHtml() + "', donvichuyenden:'', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "', nguoixuly:'" + hiddenIDHoTroChiTietXuLy.Get('nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            alert('Đã chuyển phản hồi thành công');
                            var arr_info = jsonData.d.split('|');
                            debugger;
                            // idhethong, idyeucauhotro, idxuly_ycht
                            updateSubmissionFileAttach(arr_info[0], arr_info[1], arr_info[2]);

                            popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            //location.reload(true);
                            DsYeuCauHoTroXuLy.PerformCallback(cboChonHeThongYCHT.GetValue());
                            DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue()); // gọi callback từ user control khác
                        },
                        error: function () {
                            alert('Có lỗi khi thực hiện chuyển phản hồi, bạn thử lại sau');
                        }
                    });
                }
            </script>

            <style>
                .divCommand {
                }
            </style>
            <div class="divCommand" style="padding-bottom: 10px">
                <table style="width: 100%">
                    <tr>
                        <td>Chọn hệ thống:</td>
                        <td>
                            <dx:ASPxComboBox ID="cboChonHeThongYCHT" runat="server" ValueType="System.String"
                                ClientInstanceName="cboChonHeThongYCHT" Theme="Office2010Blue">
                                <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChanged" />
                            </dx:ASPxComboBox>
                        </td>
                        <td>Mức độ yêu cầu:</td>
                        <td>
                            <dx:ASPxComboBox ID="cboMucDoYeuCauXLSearch" runat="server" Theme="Office2010Blue" ClientInstanceName="cboMucDoYeuCauXLSearch">
                            </dx:ASPxComboBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Số điện thoại:</td>
                        <td>
                            <dx:ASPxTextBox ID="txtSoDienThoaiXL" runat="server" Theme="Office2010Blue" Width="170px" ClientInstanceName="txtSoDienThoaiXL">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Lĩnh vực:</td>
                        <td>
                            <%--     <dx:ASPxDropDownEdit ID="dropLinhVucXL" runat="server" Theme="Office2010Blue" ClientInstanceName="dropLinhVucXL">
                            </dx:ASPxDropDownEdit>--%>

                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px"
                                ClientInstanceName="CallbackPanel">
                                <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                        <dx:ASPxDropDownEdit ID="LoaiHoTro" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTro" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div>
                                                    <dx:ASPxTreeList ID="treelist_donvi_cc" runat="server" Width="350px" ClientInstanceName="treelist_donvi_cc"
                                                        Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                        OnVirtualModeCreateChildren="treelist_donvi_cc_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreating="treelist_donvi_cc_VirtualModeNodeCreating"
                                                        OnCustomCallback="treelist_donvi_cc_OnCustomCallback" AutoGenerateColumns="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                        <ClientSideEvents FocusedNodeChanged="ChonDonVi" NodeExpanding="function(s, e) {
	                                                          HiddenField.Set('hidden_value2', 0);
                                                        }" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn FieldName="linhvuc" Caption="Lĩnh vực" Width="350px">
                                                            </dx:TreeListTextColumn>
                                                        </Columns>
                                                    </dx:ASPxTreeList>
                                                </div>
                                            </DropDownWindowTemplate>
                                        </dx:ASPxDropDownEdit>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>

                        </td>
                        <td>
                            <dx:ASPxButton ID="btnTimKiem" runat="server" AutoPostBack="False" ClientInstanceName="btnTimKiem" EnableTheming="True" Text="Tìm kiếm" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) {
	                                napthongtinHeThong();
                                }" />
                                <Image Url="~/HTHTKT/icons/search_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                        </td>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="btnXuLyHangLoat" runat="server" ClientInstanceName="btnXuLyHangLoat" Text="Xử lý hàng loạt" Theme="Office2010Blue" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
	  alert('Chức năng đang phát triển, vui lòng đợi thêm chút thời gian nữa để cập nhật');
	  DsYeuCauHoTroXuLy.GetSelectedFieldValues('ID', OnGetSelectedFieldValues);

}" />
                                <Image Url="~/HTHTKT/icons/fast_forward_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
            <dx:ASPxGridView ID="ASPxGridView2" ClientInstanceName="DsYeuCauHoTroXuLy"
                runat="server" AutoGenerateColumns="False" EnableTheming="True"
                KeyFieldName="ID" OnCustomCallback="ASPxGridView2_CustomCallback"
                OnDataBinding="ASPxGridView2_DataBinding"
                OnPageIndexChanged="ASPxGridView2_PageIndexChanged"
                OnCustomButtonInitialize="ASPxGridView2_CustomButtonInitialize" Theme="Office2010Blue" EnablePagingGestures="False" OnCustomColumnDisplayText="ASPxGridView2_CustomColumnDisplayText" OnHtmlDataCellPrepared="ASPxGridView2_HtmlDataCellPrepared">
                <ClientSideEvents CustomButtonClick="function(s, e) {
                        hiddenIDHoTroChiTietXuLy.Set('hiddenIDHoTroChiTietXuLy',DsYeuCauHoTroXuLy.GetRowKey(e.visibleIndex));
                        loadTTHoTroXuLy(DsYeuCauHoTroXuLy.GetRowKey(e.visibleIndex));
                                loadToanBoThongTinYeuCauHoTro(DsYeuCauHoTroXuLy.GetRowKey(e.visibleIndex));
                        luongxulyHayChuyenTiepHT(DsYeuCauHoTroXuLy.GetRowKey(e.visibleIndex));	            
                    }"
                    BeginCallback="function(s, e) {
	                    loadingdata2.Show();
                    }"
                    EndCallback="function(s, e) {
	                    loadingdata2.Hide();
                    }" />
                <SettingsPager PageSize="5">
                    <NextPageButton Text="Sau"></NextPageButton>
                    <PrevPageButton Text="Trước"></PrevPageButton>
                    <Summary Text="" />
                </SettingsPager>
                <SettingsResizing ColumnResizeMode="Control" />
                <SettingsLoadingPanel Mode="Disabled" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="#" FieldName="ID" ShowInCustomizationForm="True" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Số ĐT" CellStyle-Font-Bold="true" FieldName="SODIENTHOAI" ShowInCustomizationForm="True" VisibleIndex="1">
                        <CellStyle Font-Bold="True"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Mức độ y/c" CellStyle-Font-Bold="true" FieldName="TENMUCDO" ShowInCustomizationForm="True" VisibleIndex="2">
                        <CellStyle Font-Bold="True"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Trạng thái x/l" FieldName="TRANGTHAI_YC_XULY" ShowInCustomizationForm="True" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ngày tiếp nhận" CellStyle-Font-Bold="true" FieldName="NGAYTIEPNHAN" ShowInCustomizationForm="True" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy hh:mm:ss"></PropertiesTextEdit>
                        <CellStyle Font-Bold="True"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Số ngày" FieldName="SONGAY" ShowInCustomizationForm="True" VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn Caption="Tình trạng xl" FieldName="TINH_TRANG_XL" ShowInCustomizationForm="True" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <%--<dx:GridViewDataTextColumn Caption="Mã yêu cầu" FieldName="MA_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn Caption="Đơn vị tạo y/c" FieldName="DONVI_TAO_YC" ShowInCustomizationForm="True" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <%--<dx:GridViewDataTextColumn Caption="Nội dung y/c" FieldName="NOIDUNG_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn Caption="Nội dung y/c" FieldName="NOIDUNGXULY" ShowInCustomizationForm="True" VisibleIndex="8">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Bước xl" FieldName="BUOCXULY" Width="20px" ShowInCustomizationForm="True" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Tên hệ thống" FieldName="TENHETHONG" ShowInCustomizationForm="True" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Mô tả" FieldName="MOTA" ShowInCustomizationForm="True" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Lĩnh vực" FieldName="LINHVUC" ShowInCustomizationForm="True" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Đơn vị tiếp nhận" FieldName="TENDONVI" ShowInCustomizationForm="True" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn VisibleIndex="14">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="Xem" Text="Xử lý">
                                <Image Url="../../HTHTKT/icons/user2-edit-16x16.gif"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True"
                        ShowClearFilterButton="true"
                        VisibleIndex="15" SelectAllCheckboxMode="Page" />
                </Columns>
            </dx:ASPxGridView>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
<dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="loadingdata2" runat="server"
    Modal="True" Text="Đang tải...">
</dx:ASPxLoadingPanel>
<dx:ASPxPopupControl ID="popupXuLyPhanHoiVaXuLyLuonHT" runat="server" EnableTheming="True" HeaderText="Thông tin xử lý yêu cầu hỗ trợ"
    Height="500px" Theme="Office2010Blue" Width="700px" AllowDragging="True"
    ClientInstanceName="popupXuLyPhanHoiVaXuLyLuonHT" CloseAction="CloseButton"
    CloseOnEscape="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">

            <table style="width: 100%;" class="tblKhaiBaoHT">
                <tr>
                    <td class="labelhienthi">Thông tin luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblTenLuongHoTro" ClientInstanceName="lblTenLuongHoTro" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mô tả luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMoTaLuongHoTro" ClientInstanceName="lblMoTaLuongHoTro" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật</td>
                    <td>
                        <dx:ASPxLabel ID="lblTenHeThongHoTroKyThuat" runat="server" Text="ASPxLabel" ClientInstanceName="lblTenHeThongHoTroKyThuat">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Lĩnh vực:</td>
                    <td>&nbsp;<dx:ASPxLabel runat="server" Text="ASPxLabel" ClientInstanceName="lblLinhVucCon" ID="lblLinhVucCon"></dx:ASPxLabel>

                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="labelhienthi">Số điện thoại:</td>
                    <td>
                        <dx:ASPxLabel ID="lblSoDienThoaiXL" runat="server" ClientInstanceName="lblSoDienThoaiXL" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mức độ yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMucDoYeuCauXL" runat="server" ClientInstanceName="lblMucDoYeuCauXL" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="lblNoiDungYeuCauHoTroGoc" ClientInstanceName="lblNoiDungYeuCauHoTroGoc" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi" style="color: darkred">Dòng sự kiện xử lý:</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Height="32px" Width="538px" ClientInstanceName="txtNoiDungYHHoTro">
                        </dx:ASPxTextBox>--%>
                        <div class="table-responsive" style="height: 200px">
                            <table id="timeLine" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>Ngày</th>
                                        <%--  <th>Hệ thống</th>
                                        <th>Luồng</th>
                                        <th>Lĩnh vực chung</th>
                                        <th>Lĩnh vực con</th>
                                        <th>Nội dung yc</th>--%>
                                        <th>idnode </th>
                                        <th>Nội dung y/c</th>
                                        <th>id dv</th>
                                        <th>Đơn vị xử lý</th>
                                         <th>Username</th>
                                         <th>Tên người XL</th>
                                         <th>ĐT người XL</th>
                                        <th>Xem chi tiết</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung xử lý</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="txtNoiDungXuLyHT0" runat="server" ClientInstanceName="txtNoiDungXuLyHT0" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <dx:ASPxMemo ID="txtNoiDungXuLyHT0" runat="server" ClientInstanceName="txtNoiDungXuLyHT0" Height="32px" Width="538px">
                        </dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Nội dung xử lý chi tiết (video, hình ảnh, audio,....):</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnNhapDaPhuongTien" runat="server" AutoPostBack="False" ClientInstanceName="btnNhapDaPhuongTien" Text="Nhập đa phương tiện" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                               popupNoiDungDaPhuongTienHT.Show();
                                }" />
                            <Image Url="~/HTHTKT/icons/wizard_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Tập tin đính kèm (doc, docx, xls, xlsx, jpg, png, gif, ...):</td>
                    <td>
                        <dx:ASPxFormLayout ID="FormLayout0001" runat="server" ColCount="2" UseDefaultPaddings="false">
                            <Items>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup Caption="Danh sách tập tin">
                                            <Items>
                                                <dx:LayoutItem ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <div id="dropZone0001">
                                                                <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl0001" ClientInstanceName="DocumentsUploadControl0001" Width="100%"
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Office2010Blue">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm..."></BrowseButton>

                                                                    <AdvancedModeSettings
                                                                        EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone0001">
                                                                    </AdvancedModeSettings>
                                                                    <ValidationSettings
                                                                        AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png"
                                                                        MaxFileSize="4194304">
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents
                                                                        FileUploadComplete="onFileUploadComplete"
                                                                        FilesUploadComplete="onFilesUploadComplete"
                                                                        FilesUploadStart="onFileUploadStart" />
                                                                </dx:ASPxUploadControl>
                                                                <br />
                                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox0001" ClientInstanceName="UploadedFilesTokenBox0001"
                                                                    NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                    <ClientSideEvents Init="updateTokenBoxVisibility0001" ValueChanged="onTokenBoxValueChanged0001" Validation="onTokenBoxValidation0001" />
                                                                    <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                </dx:ASPxTokenBox>
                                                                <br />
                                                                <p class="Note">
                                                                    <dx:ASPxLabel ID="AllowedFileExtensionsLabel0001" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                    <br />
                                                                    <dx:ASPxLabel ID="MaxFileSizeLabel0001" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                </p>
                                                                <dx:ASPxValidationSummary runat="server" ID="ValidationSummary0001" ClientInstanceName="ValidationSummary0001"
                                                                    RenderMode="Table" Width="250px" ShowErrorAsLink="false">
                                                                </dx:ASPxValidationSummary>
                                                            </div>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnChuyenPhanHoiChoDonViKhacXuLy" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenPhanHoiChoDonViKhacXuLy" CssClass="my" Text="Chuyển phản hồi" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenPhanHoi(1);
                            }" />
                            <Image Url="~/HTHTKT/icons/undo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Xử lý" CssClass="my" Theme="Office2010Blue"
                            ClientInstanceName="btnXuLyHoTroLuon" EnableTheming="True" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {
	                            XuLyLuon();
                            }" />
                            <Image Url="~/HTHTKT/icons/ok_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongPopup0" runat="server" Text="Đóng" CssClass="my" ClientInstanceName="btnDongPopup" AutoPostBack="False" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            popupXuLyPhanHoiVaXuLyLuonHT.Hide();
                            }" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">

                        <div class="alert alert-info">
                            <strong>Click Chuyển phản hồi:</strong> để chuyển yêu cầu xử lý về đơn vị trước<br />
                            <strong>Click Xử lý luôn</strong> để chuyển yêu cầu xử lý về đơn vị ban đầu yêu cầu để kiểm tra
                        </div>

                    </td>
                    
                </tr>
            </table>


        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>


<dx:ASPxHiddenField ID="hiddenIDHoTroChiTietXuLy" runat="server" ClientInstanceName="hiddenIDHoTroChiTietXuLy">
</dx:ASPxHiddenField>




<dx:ASPxPopupControl ID="popupXuLyPhanHoiChuyenTiepDongHT" runat="server" Theme="Office2010Blue" Width="700px" Height="500px"
    ClientInstanceName="popupXuLyPhanHoiChuyenTiepDongHT" AllowDragging="True" Modal="True" HeaderText="Thông tin chuyển tiếp yêu cầu hỗ trợ" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table class="tblKhaiBaoHT" style="width: 100%;">
                <tr>
                    <td class="labelhienthi">Thông tin luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="lblTenLuongHoTro1">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mô tả luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMoTaLuongHoTro1" runat="server" Text="ASPxLabel" ClientInstanceName="lblMoTaLuongHoTro1">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" ClientInstanceName="lblTenHeThongHoTroKyThuat1" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Lĩnh vực:</td>
                    <td>
                        <dx:ASPxLabel ID="lblLinhVucCon1" runat="server" Text="ASPxLabel" ClientInstanceName="lblLinhVucCon1">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Số điện thoại:</td>
                    <td>
                        <dx:ASPxLabel ID="lblSoDienThoaiXL1" runat="server" ClientInstanceName="lblSoDienThoaiXL1" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mức độ yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMucDoYeuCauXL1" runat="server" ClientInstanceName="lblMucDoYeuCauXL1" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="ASPxLabel" ClientInstanceName="lblNoiDungYeuCauHoTroGoc1">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi" style="color: darkred">Dòng sự kiện xử lý:</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ClientInstanceName="txtNoiDungYHHoTro1" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <div class="table-responsive" style="height: 200px">
                            <table id="timeLine" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>Ngày</th>
                                        <%--  <th>Hệ thống</th>
                                        <th>Luồng</th>
                                        <th>Lĩnh vực chung</th>
                                        <th>Lĩnh vực con</th>
                                        <th>Nội dung yc</th>--%>
                                        <th>idnode </th>
                                        <th>Nội dung y/c</th>
                                        <th>id dv</th>
                                        <th>Đơn vị xử lý</th>
                                        <th>Xem chi tiết</th>
                                            <th>Username</th>
                                         <th>Tên người XL</th>
                                         <th>ĐT người XL</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung xử lý</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="txtNoiDungXuLyHT1" runat="server" ClientInstanceName="txtNoiDungXuLyHT1" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <dx:ASPxMemo ID="txtNoiDungXuLyHT1" runat="server" ClientInstanceName="txtNoiDungXuLyHT1" Height="32px" Width="538px">
                        </dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Nội dung xử lý chi tiết (video, hình ảnh, audio,....):</td>
                </tr>
                <tr>
                    <td class="labelhienthi">&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnNhapDaPhuongTienXL" runat="server" AutoPostBack="False" ClientInstanceName="btnNhapDaPhuongTienXL" Text="Nhập đa phương tiện" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                               popupNoiDungDaPhuongTienHT.Show();
                                }" />
                            <Image Url="~/HTHTKT/icons/wizard_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Tập tin đính kèm (doc, docx, xls, xlsx, jpg, png, gif, ...):</td>
                    <td>
                        <dx:ASPxFormLayout ID="FormLayout0002" runat="server" ColCount="2" UseDefaultPaddings="false">
                            <Items>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup Caption="Danh sách tập tin">
                                            <Items>
                                                <dx:LayoutItem ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <div id="dropZone0002">
                                                                <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl0002" ClientInstanceName="DocumentsUploadControl0002" Width="100%"
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Office2010Blue">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm..."></BrowseButton>

                                                                    <AdvancedModeSettings
                                                                        EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone0002">
                                                                    </AdvancedModeSettings>
                                                                    <ValidationSettings
                                                                        AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png"
                                                                        MaxFileSize="4194304">
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents
                                                                        FileUploadComplete="onFileUploadComplete"
                                                                        FilesUploadComplete="onFilesUploadComplete"
                                                                        FilesUploadStart="onFileUploadStart" />
                                                                </dx:ASPxUploadControl>
                                                                <br />
                                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox0002" ClientInstanceName="UploadedFilesTokenBox0002"
                                                                    NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                    <ClientSideEvents Init="updateTokenBoxVisibility0002" ValueChanged="onTokenBoxValueChanged0002" Validation="onTokenBoxValidation0002" />
                                                                    <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                </dx:ASPxTokenBox>
                                                                <br />
                                                                <p class="Note">
                                                                    <dx:ASPxLabel ID="AllowedFileExtensionsLabel0002" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                    <br />
                                                                    <dx:ASPxLabel ID="MaxFileSizeLabel0002" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                </p>
                                                                <dx:ASPxValidationSummary runat="server" ID="ValidationSummary0002" ClientInstanceName="ValidationSummary0002"
                                                                    RenderMode="Table" Width="250px" ShowErrorAsLink="false">
                                                                </dx:ASPxValidationSummary>
                                                            </div>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Đơn vị chuyển đến</td>
                    <td>
                        <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" ClientInstanceName="cboDonViChuyenDen" Theme="Office2010Blue">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                            hiddenIDHoTroChiTietXuLy.Set('donvixuly',cboDonViChuyenDen.GetValue());
                            }" />
                        </dx:ASPxComboBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <dx:ASPxButton ID="btnChuyenPhanHoiChoDonViKhacXuLyCT" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenPhanHoiChoDonViKhacXuLyCT" CssClass="my" Text="Chuyển phản hồi" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenPhanHoi(0);
                            }" />
                            <Image Url="~/HTHTKT/icons/undo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenTiepChoDonViKhacXuLy" CssClass="my" Text="Chuyển tiếp hỗ trợ" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenTiepXuLy();
                            }" />
                            <Image Url="~/HTHTKT/icons/redo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongYeuCauHoTro" runat="server" AutoPostBack="False" ClientInstanceName="btnDongYeuCauHoTro" CssClass="my" Text="Đóng yêu cầu HT" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
                                xulyDongYeuCauHoTro();
                            }" />
                            <Image Url="~/HTHTKT/icons/lock_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongPopup1" runat="server" AutoPostBack="False" ClientInstanceName="btnDongPopup1" CssClass="my" Text="Đóng" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            }" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                         <div class="alert alert-info">
                            <strong>Click "Chuyển phản hồi":</strong> để chuyển yêu cầu xử lý về đơn vị trước<br />
                            <strong>Click "Chuyển tiếp":</strong> để chuyển yêu cầu xử lý đến đơn vị tiếp theo<br />
                            <strong>Click "Đóng yêu cầu HT:"</strong> để đóng yêu cầu sau khi xác nhận đã xử lý xong
                        </div>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



<br />

<dx:ASPxPopupControl ID="popupNoiDungDaPhuongTienHT" runat="server" ClientInstanceName="popupNoiDungDaPhuongTienHT"
    HeaderText="Nội dung chi tiết.." Theme="Office2010Blue"
    Width="630px" Height="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ClientSideEvents AfterResizing="function(s, e) {
	  htmlNoiDungXuLyDaPhuongTienChiTietHT.SetHeight(document.getElementById('myDiv').clientHeight); 
         htmlNoiDungXuLyDaPhuongTienChiTietHT.SetWidth(document.getElementById('myDiv').clientWidth);
}"
        Shown="function(s, e) {
	 htmlNoiDungXuLyDaPhuongTienChiTietHT.SetHeight(document.getElementById('myDiv').clientHeight); 
                    htmlNoiDungXuLyDaPhuongTienChiTietHT.SetWidth(document.getElementById('myDiv').clientWidth);
}" />
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <div>
                <div class="container" style="width: 100%; height: 100%">
                    <div class="row">
                        <div class="col-md-12">
                            <div id="myDiv" style="width: 100%; height: 100%;">



                                <dx:ASPxHtmlEditor ID="htmlNoiDungXuLyDaPhuongTienChiTietHT" ClientInstanceName="htmlNoiDungXuLyDaPhuongTienChiTietHT" runat="server" Width="645px" Height="360px" Theme="Office2010Blue">
                                    <SettingsHtmlEditing AllowHTML5MediaElements="true" AllowObjectAndEmbedElements="true" AllowYouTubeVideoIFrames="true" />
                                    <Toolbars>
                                        <dx:HtmlEditorToolbar Name="Toolbar0">
                                            <Items>
                                                <dx:ToolbarCutButton>
                                                </dx:ToolbarCutButton>
                                                <dx:ToolbarCopyButton>
                                                </dx:ToolbarCopyButton>
                                                <dx:ToolbarPasteButton>
                                                </dx:ToolbarPasteButton>
                                                <dx:ToolbarPasteFromWordButton Visible="False">
                                                </dx:ToolbarPasteFromWordButton>
                                                <dx:ToolbarUndoButton BeginGroup="True">
                                                </dx:ToolbarUndoButton>
                                                <dx:ToolbarRedoButton>
                                                </dx:ToolbarRedoButton>
                                                <dx:ToolbarRemoveFormatButton BeginGroup="True" Visible="False">
                                                </dx:ToolbarRemoveFormatButton>
                                                <dx:ToolbarSuperscriptButton BeginGroup="True">
                                                </dx:ToolbarSuperscriptButton>
                                                <dx:ToolbarSubscriptButton>
                                                </dx:ToolbarSubscriptButton>
                                                <dx:ToolbarInsertOrderedListButton BeginGroup="True">
                                                </dx:ToolbarInsertOrderedListButton>
                                                <dx:ToolbarInsertUnorderedListButton>
                                                </dx:ToolbarInsertUnorderedListButton>
                                                <dx:ToolbarIndentButton BeginGroup="True">
                                                </dx:ToolbarIndentButton>
                                                <dx:ToolbarOutdentButton>
                                                </dx:ToolbarOutdentButton>
                                                <dx:ToolbarInsertLinkDialogButton BeginGroup="True" Visible="False">
                                                </dx:ToolbarInsertLinkDialogButton>
                                                <dx:ToolbarUnlinkButton Visible="False">
                                                </dx:ToolbarUnlinkButton>
                                                <dx:ToolbarCheckSpellingButton BeginGroup="True">
                                                </dx:ToolbarCheckSpellingButton>


                                                <dx:ToolbarInsertImageDialogButton Visible="true">
                                                </dx:ToolbarInsertImageDialogButton>



                                                <dx:ToolbarTableOperationsDropDownButton BeginGroup="True" Visible="False">
                                                </dx:ToolbarTableOperationsDropDownButton>


                                                <dx:ToolbarInsertFlashDialogButton BeginGroup="true">
                                                </dx:ToolbarInsertFlashDialogButton>
                                                <dx:ToolbarInsertVideoDialogButton>
                                                </dx:ToolbarInsertVideoDialogButton>
                                                <dx:ToolbarInsertAudioDialogButton>
                                                </dx:ToolbarInsertAudioDialogButton>
                                                <dx:ToolbarInsertYouTubeVideoDialogButton>
                                                </dx:ToolbarInsertYouTubeVideoDialogButton>
                                                <dx:ToolbarInsertImageDialogButton>
                                                </dx:ToolbarInsertImageDialogButton>

                                            </Items>
                                        </dx:HtmlEditorToolbar>
                                        <dx:HtmlEditorToolbar Name="Toolbar">
                                            <Items>




                                                <dx:ToolbarParagraphFormattingEdit Visible="false">
                                                    <Items>
                                                        <dx:ToolbarListEditItem Text="Normal" Value="p" />
                                                        <dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
                                                        <dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
                                                        <dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
                                                        <dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
                                                        <dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
                                                        <dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
                                                        <dx:ToolbarListEditItem Text="Address" Value="address" />
                                                        <dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
                                                    </Items>
                                                </dx:ToolbarParagraphFormattingEdit>
                                                <dx:ToolbarFontNameEdit>
                                                    <Items>
                                                        <dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
                                                        <dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
                                                        <dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
                                                        <dx:ToolbarListEditItem Text="Arial" Value="Arial" />
                                                        <dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
                                                        <dx:ToolbarListEditItem Text="Courier" Value="Courier" />
                                                    </Items>
                                                </dx:ToolbarFontNameEdit>
                                                <dx:ToolbarFontSizeEdit>
                                                    <Items>
                                                        <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                                        <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                                        <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                                        <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                                        <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                                        <dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
                                                        <dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
                                                    </Items>
                                                </dx:ToolbarFontSizeEdit>
                                                <dx:ToolbarBoldButton BeginGroup="True">
                                                </dx:ToolbarBoldButton>
                                                <dx:ToolbarItalicButton>
                                                </dx:ToolbarItalicButton>
                                                <dx:ToolbarUnderlineButton>
                                                </dx:ToolbarUnderlineButton>
                                                <dx:ToolbarStrikethroughButton>
                                                </dx:ToolbarStrikethroughButton>
                                                <dx:ToolbarJustifyLeftButton BeginGroup="True">
                                                </dx:ToolbarJustifyLeftButton>
                                                <dx:ToolbarJustifyCenterButton>
                                                </dx:ToolbarJustifyCenterButton>
                                                <dx:ToolbarJustifyRightButton>
                                                </dx:ToolbarJustifyRightButton>
                                                <dx:ToolbarBackColorButton BeginGroup="True">
                                                </dx:ToolbarBackColorButton>
                                                <dx:ToolbarFontColorButton>
                                                </dx:ToolbarFontColorButton>


                                                <dx:ToolbarFullscreenButton BeginGroup="true">
                                                </dx:ToolbarFullscreenButton>

                                                <%--<dx:ToolbarUndoButton>
                        </dx:ToolbarUndoButton>
                        <dx:ToolbarRedoButton>
                        </dx:ToolbarRedoButton>
                        <dx:ToolbarBoldButton BeginGroup="True">
                        </dx:ToolbarBoldButton>
                        <dx:ToolbarItalicButton>
                        </dx:ToolbarItalicButton>
                        <dx:ToolbarUnderlineButton>
                        </dx:ToolbarUnderlineButton>
                        <dx:ToolbarStrikethroughButton>
                        </dx:ToolbarStrikethroughButton>
                        <dx:ToolbarInsertFlashDialogButton BeginGroup="true">
                        </dx:ToolbarInsertFlashDialogButton>
                        <dx:ToolbarInsertVideoDialogButton>
                        </dx:ToolbarInsertVideoDialogButton>
                        <dx:ToolbarInsertAudioDialogButton>
                        </dx:ToolbarInsertAudioDialogButton>
                        <dx:ToolbarInsertYouTubeVideoDialogButton>
                        </dx:ToolbarInsertYouTubeVideoDialogButton>
                        <dx:ToolbarInsertImageDialogButton>
                        </dx:ToolbarInsertImageDialogButton>
                        <dx:ToolbarFullscreenButton BeginGroup="true">
                        </dx:ToolbarFullscreenButton>
                                                --%>
                                            </Items>
                                        </dx:HtmlEditorToolbar>
                                    </Toolbars>
                                    <SettingsDialogs>
                                        <InsertFlashDialog>
                                            <SettingsFlashUpload UploadFolder="~/HTHTKT/UploadFiles/FlashFiles">
                                                <ValidationSettings AllowedFileExtensions=".swf" MaxFileSize="500000">
                                                </ValidationSettings>
                                            </SettingsFlashUpload>
                                            <SettingsFlashSelector Enabled="True">
                                                <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/FlashFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                    InitialFolder="" />
                                            </SettingsFlashSelector>
                                        </InsertFlashDialog>
                                        <InsertVideoDialog>
                                            <SettingsVideoUpload UploadFolder="~/HTHTKT/UploadFiles/VideoFiles">
                                                <ValidationSettings AllowedFileExtensions=".mp4, .ogg" MaxFileSize="1000000">
                                                </ValidationSettings>
                                            </SettingsVideoUpload>
                                            <SettingsVideoSelector Enabled="True">
                                                <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/VideoFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                    InitialFolder="" />
                                            </SettingsVideoSelector>
                                        </InsertVideoDialog>
                                        <InsertAudioDialog>
                                            <SettingsAudioUpload UploadFolder="~/HTHTKT/UploadFiles/AudioFiles">
                                                <ValidationSettings AllowedFileExtensions=".mp3, .ogg" MaxFileSize="500000">
                                                </ValidationSettings>
                                            </SettingsAudioUpload>
                                            <SettingsAudioSelector Enabled="True">
                                                <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/AudioFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                    InitialFolder="" />
                                            </SettingsAudioSelector>
                                        </InsertAudioDialog>
                                        <InsertImageDialog>
                                            <SettingsImageUpload UploadFolder="~/HTHTKT/UploadFiles/Images/">
                                                <ValidationSettings AllowedFileExtensions=".jpe,.jpeg,.jpg,.gif,.png" MaxFileSize="500000">
                                                </ValidationSettings>
                                            </SettingsImageUpload>
                                        </InsertImageDialog>
                                    </SettingsDialogs>
                                </dx:ASPxHtmlEditor>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">&nbsp;</div>
                        <div class="col-md-6">
                            <dx:ASPxButton ID="xemdapt" runat="server" ClientInstanceName="xemdapt" AutoPostBack="false" Text="Đồng ý" Theme="Office2010Blue">

                                <ClientSideEvents Click="function(s, e) {
                               popupNoiDungDaPhuongTienHT.Hide();
                                }" />  <Image Url="../../HTHTKT/icons/ok_16x16.gif">
                            </Image>

                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnDong" runat="server" AutoPostBack="false" Text="Đóng" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) {
	                                    popupNoiDungDaPhuongTienHT.Hide();
                                    }" />
                                <Image Url="~/HTHTKT/icons/delete_16x16.gif"></Image>
                            </dx:ASPxButton>
                        </div>
                    </div>
                </div>
            </div>


        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>







<dx:ASPxPopupControl ID="popupXuLyPhanHoiChuyenTiepDongHangLoatHT" runat="server" Theme="Office2010Blue" Width="700px" Height="500px"
    ClientInstanceName="popupXuLyPhanHoiChuyenTiepDongHangLoatHT" AllowDragging="True" Modal="True" HeaderText="Thông tin chuyển tiếp yêu cầu hỗ trợ hàng loạt" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table class="tblKhaiBaoHT" style="width: 100%;">
                <tr>
                    <td class="labelhienthi">Thông tin luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel7333" runat="server" Text="ASPxLabel" ClientInstanceName="ASPxLabel7333">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mô tả luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMoTaLuongHoTro1333" runat="server" Text="ASPxLabel" ClientInstanceName="lblMoTaLuongHoTro1333">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel8333" runat="server" ClientInstanceName="ASPxLabel8333" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Lĩnh vực:</td>
                    <td>
                        <dx:ASPxLabel ID="lblLinhVucCon1333" runat="server" Text="ASPxLabel" ClientInstanceName="lblLinhVucCon1333">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel9333" runat="server" Text="ASPxLabel" ClientInstanceName="ASPxLabel9333">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi" colspan="2">Danh sách các yêu cầu cần xử lý:</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ClientInstanceName="txtNoiDungYHHoTro1" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <div class="table-responsive" style="height: 200px">
                            <table id="timeLine" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>Ngày</th>
                                        <%--  <th>Hệ thống</th>
                                        <th>Luồng</th>
                                        <th>Lĩnh vực chung</th>
                                        <th>Lĩnh vực con</th>
                                        <th>Nội dung yc</th>--%>
                                        <th>idnode </th>
                                        <th>Nội dung y/c</th>
                                        <th>id dv</th>
                                        <th>Đơn vị xử lý</th>
                                            <th>Username</th>
                                         <th>Tên người XL</th>
                                         <th>ĐT người XL</th>
                                        <th>Xem timeline</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung xử lý</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="txtNoiDungXuLyHT1" runat="server" ClientInstanceName="txtNoiDungXuLyHT1" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <dx:ASPxMemo ID="txtNoiDungXuLyHT1333" runat="server" ClientInstanceName="txtNoiDungXuLyHT1333" Height="32px" Width="538px">
                        </dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Nội dung xử lý chi tiết (video, hình ảnh, audio,....):</td>
                </tr>
                <tr>
                    <td class="labelhienthi">&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnNhapDaPhuongTienXL333" runat="server" AutoPostBack="False" ClientInstanceName="btnNhapDaPhuongTienXL" Text="Nhập đa phương tiện" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                               popupNoiDungDaPhuongTienHT.Show();
                                }" />
                            <Image Url="~/HTHTKT/icons/wizard_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Tập tin đính kèm (doc, docx, xls, xlsx, jpg, png, gif, ...):</td>
                    <td>
                        <dx:ASPxFormLayout ID="FormLayout0003" runat="server" ColCount="2" UseDefaultPaddings="false">
                            <Items>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup Caption="Danh sách tập tin">
                                            <Items>
                                                <dx:LayoutItem ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <div id="dropZone0003">
                                                                <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl0003" ClientInstanceName="DocumentsUploadControl0003" Width="100%"
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Office2010Blue">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm..."></BrowseButton>

                                                                    <AdvancedModeSettings
                                                                        EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone0003">
                                                                    </AdvancedModeSettings>
                                                                    <ValidationSettings
                                                                        AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png"
                                                                        MaxFileSize="4194304">
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents
                                                                        FileUploadComplete="onFileUploadComplete"
                                                                        FilesUploadComplete="onFilesUploadComplete"
                                                                        FilesUploadStart="onFileUploadStart" />
                                                                </dx:ASPxUploadControl>
                                                                <br />
                                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox0003" ClientInstanceName="UploadedFilesTokenBox0003"
                                                                    NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                    <ClientSideEvents Init="updateTokenBoxVisibility0003"
                                                                        ValueChanged="onTokenBoxValueChanged0003" Validation="onTokenBoxValidation0003" />
                                                                    <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                </dx:ASPxTokenBox>
                                                                <br />
                                                                <p class="Note">
                                                                    <dx:ASPxLabel ID="AllowedFileExtensionsLabel0003" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                    <br />
                                                                    <dx:ASPxLabel ID="MaxFileSizeLabel0003" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                </p>
                                                                <dx:ASPxValidationSummary runat="server" ID="ValidationSummary11" ClientInstanceName="ValidationSummary"
                                                                    RenderMode="Table" Width="250px" ShowErrorAsLink="false">
                                                                </dx:ASPxValidationSummary>
                                                            </div>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Đơn vị chuyển đến</td>
                    <td>
                        <dx:ASPxComboBox ID="ASPxComboBox1333" runat="server" ClientInstanceName="cboDonViChuyenDenHangLoat" Theme="Office2010Blue">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                            hiddenIDHoTroChiTietXuLy.Set('donvixuly',cboDonViChuyenDen.GetValue());
                            }" />
                        </dx:ASPxComboBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <dx:ASPxButton ID="ASPxButton1333" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenPhanHoiChoDonViKhacXuLy3333" CssClass="my" Text="Chuyển phản hồi" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenPhanHoi(0);
                            }" />
                            <Image Url="~/HTHTKT/icons/undo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButton63333" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenTiepChoDonViKhacXuLy3333" CssClass="my" Text="Chuyển tiếp hỗ trợ" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenTiepXuLy();
                            }" />
                            <Image Url="~/HTHTKT/icons/redo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongYeuCauHoTro3333" runat="server" AutoPostBack="False" ClientInstanceName="btnDongYeuCauHoTro3333" CssClass="my" Text="Đóng yêu cầu HT" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
                                xulyDongYeuCauHoTro();
 
                                 }" />
                            <Image Url="~/HTHTKT/icons/lock_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongPopup1333" runat="server" AutoPostBack="False" ClientInstanceName="btnDongPopup1333" CssClass="my" Text="Đóng" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            popupXuLyPhanHoiChuyenTiepDongHT.Hide();
                            }" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="popupDanhSachFileAttach" runat="server" ClientInstanceName="popupDanhSachFileAttach" HeaderText="Danh sách tệp tin đính kèm" Height="300px" Theme="Office2010Blue" Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table style="width: 100%;">
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="table-responsive" style="height: 200px">
                            <table id="dsFileAttach" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>STT</th>
                                        <th>Tên tập tin</th>
                                        <th>Ngày cập nhật</th>
                                        <th>Tải về</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td style="text-align: right">
                        <dx:ASPxButton ID="ASPxButton63334" runat="server" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {
	popupDanhSachFileAttach.Hide();
}" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<br />
<dx:ASPxPopupControl ID="popupXuLyPhanHoiVaXuLyLuonHangLoatHT" runat="server" EnableTheming="True" HeaderText="Thông tin xử lý yêu cầu hỗ trợ hàng loạt"
    Height="500px" Theme="Office2010Blue" Width="700px" AllowDragging="True"
    ClientInstanceName="popupXuLyPhanHoiVaXuLyLuonHangLoatHT" CloseAction="CloseButton"
    CloseOnEscape="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table style="width: 100%;" class="tblKhaiBaoHT">
                <tr>
                    <td class="labelhienthi">Thông tin luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblTenLuongHoTro444" ClientInstanceName="lblTenLuongHoTro444" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Mô tả luồng xử lý:</td>
                    <td>
                        <dx:ASPxLabel ID="lblMoTaLuongHoTro444" ClientInstanceName="lblMoTaLuongHoTro444" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật</td>
                    <td>
                        <dx:ASPxLabel ID="lblTenHeThongHoTroKyThuat444" runat="server" Text="ASPxLabel" ClientInstanceName="lblTenHeThongHoTroKyThuat444">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Lĩnh vực:</td>
                    <td>
                        <dx:ASPxLabel ID="lblLinhVucCon444" runat="server" Text="ASPxLabel" ClientInstanceName="lblLinhVucCon444">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung yêu cầu:</td>
                    <td>
                        <dx:ASPxLabel ID="lblNoiDungYeuCauHoTroGoc444" ClientInstanceName="lblNoiDungYeuCauHoTroGoc444" runat="server" Text="ASPxLabel">
                        </dx:ASPxLabel>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi" colspan="2">Danh sách các yêu cầu cần xử lý:</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Height="32px" Width="538px" ClientInstanceName="txtNoiDungYHHoTro">
                        </dx:ASPxTextBox>--%>
                        <div class="table-responsive" style="height: 200px">
                            <table id="timeLine" class="table table-hover table-striped">
                                <%--header-fixed--%>
                                <thead>
                                    <tr>
                                        <th>Ngày</th>
                                        <%--  <th>Hệ thống</th>
                                        <th>Luồng</th>
                                        <th>Lĩnh vực chung</th>
                                        <th>Lĩnh vực con</th>
                                        <th>Nội dung yc</th>--%>
                                        <th>idnode </th>
                                        <th>Nội dung y/c</th>
                                        <th>id dv</th>
                                        <th>Đơn vị xử lý</th>
                                            <th>Username</th>
                                         <th>Tên người XL</th>
                                         <th>ĐT người XL</th>
                                        <th>Xem timeline</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelhienthi">Nội dung xử lý</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<dx:ASPxTextBox ID="txtNoiDungXuLyHT0" runat="server" ClientInstanceName="txtNoiDungXuLyHT0" Height="32px" Width="538px">
                        </dx:ASPxTextBox>--%>
                        <dx:ASPxMemo ID="txtNoiDungXuLyHT0444" runat="server" ClientInstanceName="txtNoiDungXuLyHT0444" Height="32px" Width="538px">
                        </dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">Nội dung xử lý chi tiết (video, hình ảnh, audio,....):</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnNhapDaPhuongTien444" runat="server" AutoPostBack="False" ClientInstanceName="btnNhapDaPhuongTien444" Text="Nhập đa phương tiện" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                               popupNoiDungDaPhuongTien.Show();
                                }" />
                            <Image Url="~/HTHTKT/icons/wizard_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td class="labelhienthi">Tập tin đính kèm (doc, docx, xls, xlsx, jpg, png, gif, ...):</td>
                    <td>
                        <dx:ASPxFormLayout ID="FormLayout0004" runat="server" ColCount="2" UseDefaultPaddings="false">
                            <Items>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup Caption="Danh sách tập tin">
                                            <Items>
                                                <dx:LayoutItem ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <div id="dropZone0004">
                                                                <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl0004" ClientInstanceName="DocumentsUploadControl0004" Width="100%"
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Office2010Blue">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm..."></BrowseButton>

                                                                    <AdvancedModeSettings
                                                                        EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone0004">
                                                                    </AdvancedModeSettings>
                                                                    <ValidationSettings
                                                                        AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png"
                                                                        MaxFileSize="4194304">
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents
                                                                        FileUploadComplete="onFileUploadComplete"
                                                                        FilesUploadComplete="onFilesUploadComplete"
                                                                        FilesUploadStart="onFileUploadStart" />
                                                                </dx:ASPxUploadControl>
                                                                <br />
                                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox0004" ClientInstanceName="UploadedFilesTokenBox0004"
                                                                    NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                    <ClientSideEvents Init="updateTokenBoxVisibility0004" ValueChanged="onTokenBoxValueChanged0004" Validation="onTokenBoxValidation0004" />
                                                                    <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                </dx:ASPxTokenBox>
                                                                <br />
                                                                <p class="Note">
                                                                    <dx:ASPxLabel ID="AllowedFileExtensionsLabel0004" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                    <br />
                                                                    <dx:ASPxLabel ID="MaxFileSizeLabel0004" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                </p>
                                                                <dx:ASPxValidationSummary runat="server" ID="ValidationSummary0004" ClientInstanceName="ValidationSummary0004"
                                                                    RenderMode="Table" Width="250px" ShowErrorAsLink="false">
                                                                </dx:ASPxValidationSummary>
                                                            </div>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnChuyenPhanHoiChoDonViKhacXuLy444" runat="server" AutoPostBack="False" ClientInstanceName="btnChuyenPhanHoiChoDonViKhacXuLy444" CssClass="my" Text="Chuyển phản hồi" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            ChuyenPhanHoi(1);
                            }" />
                            <Image Url="~/HTHTKT/icons/undo1_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButton3444" runat="server" Text="Xử lý" CssClass="my" Theme="Office2010Blue"
                            ClientInstanceName="btnXuLyHoTroLuon" EnableTheming="True" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {
	                            XuLyLuon();
                            }" />
                            <Image Url="~/HTHTKT/icons/ok_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnDongPopup04444" runat="server" Text="Đóng" CssClass="my" ClientInstanceName="btnDongPopup" AutoPostBack="False" Theme="Office2010Blue">
                            <ClientSideEvents Click="function(s, e) {
	                            popupXuLyPhanHoiVaXuLyLuonHT.Hide();
                            }" />
                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxHiddenField runat="server" ID="HiddenField" ClientInstanceName="HiddenField"></dx:ASPxHiddenField>








