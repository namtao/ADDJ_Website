<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_DmCayThuMucYeuCau.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_DmCayThuMucYeuCau" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
    <style type="text/css">
        .inputstyle.cus input[type=text] {
            padding: 0 5px;
            width: 200px;
        }

        .btn_style_button {
            padding: 5px 15px;
        }

        .DDPager {
            margin: 10px 0px;
        }

        .tbl_style a {
            color: #034AFC;
        }

        .msg {
            font-size: 12px;
            font-weight: bold;
            color: red;
        }

        div.DDPager {
            border: 1px solid #ccc;
            padding: 5px;
            padding-top: 10px;
        }

        .DDControl {
            text-align: center;
        }

        #mainmenu {
            position: relative;
            z-index: 9999;
        }

        .tbl_style .tooltip {
            color: black;
            cursor: help;
        }
    </style>
    <script type="text/javascript">
        var postponedCallbackRequired = false;
        //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChanged(s, e) {
            debugger;
            ASPxHiddenField1.Set('hidden_value', s.GetValue());
            ASPxHiddenField1.Set('hidden_value2', 1);
            ASPxHiddenField1.Set('res_value', 1);

            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanel.InCallback())
                postponedCallbackRequired = true;
            else
                CallbackPanel.PerformCallback();
            // gán giá trị id cho hidden
            ASPxHiddenField1.Set('ThongHTKTID', s.GetValue());


            cboDSHeThongYCHT.SetText(cboChonHeThongHTKT.GetText());
            cboDSHeThongYCHT.SetValue(cboChonHeThongHTKT.GetValue());
            cboChonHeThongSua.SetText(cboChonHeThongHTKT.GetText());
            cboChonHeThongSua.SetValue(cboChonHeThongHTKT.GetValue());
        }
        function OnEndCallback(s, e) {
            if (postponedCallbackRequired) {
                CallbackPanel.PerformCallback();
                postponedCallbackRequired = false;
            }
        }


        // Popup add
        function ChonDonViADD(s, e) {
            var tendv = '';
            var key = treelist_donvi_ccADD.GetFocusedNodeKey();
            treelist_donvi_ccADD.GetNodeValues(key, "linhvuc", function (value) {
                LoaiHoTroADD.SetText(value);
                tenDonVi = value;
                LoaiHoTroADD.SetKeyValue(key);
                LoaiHoTroADD.HideDropDown();
                //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
            });
        }
        var postponedCallbackRequiredADD = false;
        //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChangedADD(s, e) {
            debugger;
            if (s.GetValue() == '0') {
                LoaiHoTroADD.SetText('');
                LoaiHoTroADD.SetKeyValue('0');
            }


            HiddenField.Set('hidden_valueADD', s.GetValue());
            HiddenField.Set('hidden_value2ADD', 1);
            HiddenField.Set('res_valueADD', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanelADD.InCallback())
                postponedCallbackRequiredADD = true;
            else
                CallbackPanelADD.PerformCallback();

        }
        function OnEndCallbackADD(s, e) {
            if (postponedCallbackRequiredADD) {
                CallbackPanelADD.PerformCallback();
                postponedCallbackRequiredADD = false;
            }

             //napDanhSachHeThongYCHT();
            popupThemMoiLoaiYeuCau.Show();


            txtLinhVucADD.SetText('');
            txtMaLinhVucADD.SetText('');
            txtGhiChu.SetText('');
            chkTrangThai.SetChecked(false);

            // trường hợp thêm mới này thì id cha = id hiện chọn thêm và hệ thống ứng với hệ thống
            // Nạp thông tin xử lý
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucLoaiYeuCauHoTroHeThong',
                data: "{ id: " +  HiddenField.Get('id_linhvuc_add') + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id_caythumuc', objret[0].ID);  // là id cần sửa

                    cboDSHeThongYCHT.SetValue(objret[0].ID_HETHONG_YCHT);  // disabled
                    cboDSHeThongYCHT.SetEnabled(false);

                    // lấy thông tin node cha
                    //if (objret[0].ID_CHA !== null && objret[0].ID_CHA != '0') {
                    //    thongTinChaLoaiHoTroADD(objret[0].ID_CHA);
                    //}
                    //else {
                    //    LoaiHoTroADD.SetKeyValue('0');
                    //    LoaiHoTroADD.SetText('');
                    //}


                    // khi thêm từ mục chọn thì đó làm node gốc
                    LoaiHoTroADD.SetKeyValue(objret[0].ID);
                    LoaiHoTroADD.SetText(objret[0].LINHVUC);


                    //cboLinhVucChung.SetValue(objret[0].LINHVUC);
                    //cboLinhVucChung.SetEnabled(false);
                    //cboLinhVucCon.SetValue(objret[0].LINHVUCCON);
                    //cboLinhVucCha.SetValue(objret[0].ID); //objret[0].ID_CHA id cha là id của id hiện hành   // disabled
                    //cboLinhVucCha.SetEnabled(false);
                    //cboDonViTiepNhanXL.SetValue(objret[0].DONVI_TIEPNHAN_XL);
                    //txtGhiChu.SetText(objret[0].GHICHU);
                    //chkTrangThai.SetChecked(objret[0].TRANGTHAI);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }


        // end add


        // Popup edit

        function ChonDonViEDIT(s, e) {
            var tendv = '';
            var key = treelist_donvi_ccEDIT.GetFocusedNodeKey();
            treelist_donvi_ccEDIT.GetNodeValues(key, "linhvuc", function (value) {
                LoaiHoTroEDIT.SetText(value);
                tenDonVi = value;
                LoaiHoTroEDIT.SetKeyValue(key);
                LoaiHoTroEDIT.HideDropDown();
                //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
            });
        }
        var postponedCallbackRequiredEDIT = false;
        //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChangedEDIT(s, e) {
            debugger;
            if (s.GetValue() == '0') {
                LoaiHoTroEDIT.SetText('');
                LoaiHoTroEDIT.SetKeyValue('0');
            }


            HiddenField.Set('hidden_valueEDIT', s.GetValue());
            HiddenField.Set('hidden_value2EDIT', 1);
            HiddenField.Set('res_valueEDIT', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanelEDIT.InCallback())
                postponedCallbackRequiredEDIT = true;
            else
                CallbackPanelEDIT.PerformCallback();

        }



        function OnEndCallbackEDIT(s, e) {
            if (postponedCallbackRequiredEDIT) {
                CallbackPanelEDIT.PerformCallback();
                postponedCallbackRequiredEDIT = false;
            }
            //napDanhSachHeThongYCHT();
            popupSuaLoaiYeuCau.Show();

            // Nạp thông tin xử lý
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucLoaiYeuCauHoTroHeThong',
                data: "{ id: " + HiddenField.Get('id_linhvuc_edit') + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id_caythumuc', objret[0].ID);

                    cboChonHeThongSua.SetValue(objret[0].ID_HETHONG_YCHT);

                    if (objret[0].ID_CHA !== null && objret[0].ID_CHA != '0') {
                        thongTinChaLoaiHoTroEDIT(objret[0].ID_CHA);
                    }
                    else {
                        LoaiHoTroEDIT.SetKeyValue('0');
                        LoaiHoTroEDIT.SetText('');
                    }

                    txtLinhVucEDIT.SetText(objret[0].LINHVUC);
                    txtMaLinhVucEDIT.SetText(objret[0].MA_LINHVUC);
                    txtGhiChuSua.SetText(objret[0].GHICHU);
                    chkTrangThaiSua.SetChecked(objret[0].TRANGTHAI);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }
        // end edit



        function XoaMucYeuCauHoTro(valkey) {
            // e.nodeKey: lấy id của row
            if (confirm('Bạn muốn xóa mục này?, chú ý nếu bạn xóa mục gốc thì tất cả mục con của gốc cũng sẽ bị xóa. Bạn muốn tiếp tục?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaAllMucTrongCayYeuCauHTByID',
                    data: "{ id_caythumuc: " + valkey + " }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');
                            //treelist_donvi_cc.PerformCallback('clear');
                            //treelist_donvi_cc.PerformCallback('refresh');
                            //treelist_donvi_cc.SetFocusedNodeKey(null);

                            ASPxHiddenField1.Set('hidden_value2', 1);
                            ASPxHiddenField1.Set('res_value', 1);// flag để đánh dấu refresh
                            CallbackPanel.PerformCallback();
                        }
                        else if (jsonData.d == '0') {
                            alert('Có lỗi trong quá trình xóa, vui lòng thử lại sau.');
                        }
                        loadingdata2.Hide();
                    },
                    error: function () {
                        loadingdata2.Hide();
                    }
                });
            }
        }



        var key = "";
        function OnContextMenu(s, e) {
            if (e.objectType == "Node") {
                key = e.objectKey;
                /* prepare popup menu */
                var state = asptree_caythumuchethong.GetNodeState(e.objectKey);
                popupMenu.GetItem(0).SetEnabled(state != "Child" && state != "NotFound");
                popupMenu.GetItem(1).SetEnabled(state == "Child" || state == "NotFound");
                popupMenu.ShowAtPos(ASPxClientUtils.GetEventX(e.htmlEvent), ASPxClientUtils.GetEventY(e.htmlEvent));
            }
        }

        function menu_OnItemClick(s, e) {
            switch (e.item.index) {
                case 0:
                    var state = asptree_caythumuchethong.GetNodeState(key);
                    if (state == "Expanded")
                        asptree_caythumuchethong.CollapseNode(key);
                    else
                        asptree_caythumuchethong.ExpandNode(key);
                    break;
                case 1:
                    alert("Node key is: " + key);
                    break;
                case 2:
                    alert("ASPxTreeList v2010 vol 1.5");
                    break;
            }
        }
        function OnClick(s, e) {
            popupMenu2.ShowAtElement(s.GetMainElement());
        }

        // thêm mới tinh, cho phép chọn hệ thống tùy ý, thông tin edit
        function themMoiLoaiYeuCauHoTro() {
            //napDanhSachHeThongYCHT();

            // lấy giá trị theo điều kiện tìm kiếm
            cboDSHeThongYCHT.SetValue(cboChonHeThongHTKT.GetValue());
            popupThemMoiLoaiYeuCau.Show();


            // xử lý nạp loại lĩnh vực ứng với thêm mới theo chọn loại hệ thống
            if (cboChonHeThongHTKT.GetValue() == '0') {
                LoaiHoTroADD.SetText('');
                LoaiHoTroADD.SetKeyValue('0');
            }

            HiddenField.Set('hidden_valueADD', cboChonHeThongHTKT.GetValue());
            HiddenField.Set('hidden_value2ADD', 1);
            HiddenField.Set('res_valueADD', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanelADD.InCallback())
                postponedCallbackRequiredADD = true;
            else
                CallbackPanelADD.PerformCallback();
        }

        // thêm theo ID gốc cha
        function themMucYeuCauHoTro(val) {
            HiddenField.Set('id_linhvuc_add', val);

            HiddenField.Set('hidden_valueADD', cboChonHeThongHTKT.GetValue());
            HiddenField.Set('hidden_value2ADD', 1);
            HiddenField.Set('res_valueADD', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanelADD.InCallback())
                postponedCallbackRequiredADD = true;
            else
                CallbackPanelADD.PerformCallback();

           

        }
        function thongTinChaLoaiHoTroADD(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucLoaiYeuCauHoTroHeThong',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    LoaiHoTroADD.SetKeyValue(objret[0].ID);
                    LoaiHoTroADD.SetText(objret[0].LINHVUC);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function thongTinChaLoaiHoTroEDIT(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucLoaiYeuCauHoTroHeThong',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    LoaiHoTroEDIT.SetKeyValue(objret[0].ID);
                    LoaiHoTroEDIT.SetText(objret[0].LINHVUC);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }


        // sửa theo ID
        function suaMucYeuCauHoTro(val) {
            HiddenField.Set('id_linhvuc_edit', val);

            HiddenField.Set('hidden_valueEDIT', cboChonHeThongHTKT.GetValue());
            HiddenField.Set('hidden_value2EDIT', 1);
            HiddenField.Set('res_valueEDIT', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanelEDIT.InCallback())
                postponedCallbackRequiredEDIT = true;
            else
                CallbackPanelEDIT.PerformCallback();
        }

        function themThongTin() {
            //alert("thêm thông tin");

            // xử lý nếu không có cha thì là cấp cao nhất
            var idcha = 0;
            if (LoaiHoTroADD.GetText() != '')
            {
                idcha = LoaiHoTroADD.GetKeyValue();
            }

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themMucCayThuMucHeThongYCHTNew',
                data: "{ id_hethong_htkt: '" + cboDSHeThongYCHT.GetValue() +
                "',linhvuc: '" + txtLinhVucADD.GetText() +
                "',malinhvuc: '" + txtMaLinhVucADD.GetText() +
                "',id_cha: '" + idcha +
                "',ghichu: '" + txtGhiChu.GetText() +
                "',trangthai: '" + chkTrangThai.GetChecked() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        ASPxHiddenField1.Set('hidden_value2', 1);
                        ASPxHiddenField1.Set('res_value', 1);// flag để đánh dấu refresh
                        CallbackPanel.PerformCallback();
                        alert('Thêm thành công');
                        popupThemMoiLoaiYeuCau.Hide();
                    }
                    else if (jsonData.d == '0') {
                        alert('Có lỗi trong quá trình thêm');
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function suaThongTin() {
            // xử lý nếu không có cha thì là cấp cao nhất
            var idcha = 0;
            if (LoaiHoTroEDIT.GetText() != '') {
                idcha = LoaiHoTroEDIT.GetKeyValue();
            }

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaMucCayThuMucHeThongYCHTNew',
                data: "{ id_hethong_htkt: '" + cboChonHeThongSua.GetValue() +
                "',linhvuc:'" + txtLinhVucEDIT.GetText() +
                "',malinhvuc:'" + txtMaLinhVucEDIT.GetText() +
                "',id_cha: '" + idcha +
                "',ghichu: '" + txtGhiChuSua.GetText() +
                "',trangthai: '" + chkTrangThaiSua.GetChecked() +
                "',id_muccaythumuc: '" + HiddenField.Get('id_linhvuc_edit') +
                "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {

                        ASPxHiddenField1.Set('hidden_value2', 1);
                        ASPxHiddenField1.Set('res_value', 1);// flag để đánh dấu refresh
                        alert('Sửa thành công');
                        popupSuaLoaiYeuCau.Hide();
                        CallbackPanel.PerformCallback();
                    }
                    else if (jsonData.d == '0') {
                        alert('Có lỗi trong quá trình sửa');
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }


        function napDanhSachHeThongYCHT() {
            cboDSHeThongYCHT.ClearItems();
            cboChonHeThongSua.ClearItems();

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/napDanhSachHeThongYCHT',
                data: "{ id: '" + 1 + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    cboDSHeThongYCHT.AddItem('-- chọn hệ thống --', 0);
                    cboChonHeThongSua.AddItem('-- chọn hệ thống --', 0);
                    for (var i = 0; i < objret.length; i++) {
                        cboDSHeThongYCHT.AddItem(objret[i].TENHETHONG, objret[i].ID);
                        cboChonHeThongSua.AddItem(objret[i].TENHETHONG, objret[i].ID);
                    }
                    //cboDSHeThongYCHT.SetSelectedIndex(0);
                    //cboChonHeThongSua.SetSelectedIndex(0);

                    cboDSHeThongYCHT.SetValue(ASPxHiddenField1.Get('hidden_value')); // gán mặc định nếu chọn hệ thống ban đầu
                    cboChonHeThongSua.SetValue(ASPxHiddenField1.Get('hidden_value')); // gán mặc định nếu chọn hệ thống ban đầu


                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function napDanhSachThongTinCayThuMuc(val) {
            cboLoaiYeuCauHT.ClearItems();
            cboLoaiYeuCauHTSua.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDanhMucLoaiCayThuMucYCHT',
                data: "{ id: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    for (var i = 0; i < objret.length; i++) {
                        cboLoaiYeuCauHT.AddItem(objret[i].LOAI, objret[i].LOAI);
                        cboLoaiYeuCauHTSua.AddItem(objret[i].LOAI, objret[i].LOAI);
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });

            cboLinhVucChung.ClearItems();
            cboLinhVucChungSua.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDanhMucLinhVucChungCayThuMucYCHT',
                data: "{ id: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    for (var i = 0; i < objret.length; i++) {
                        cboLinhVucChung.AddItem(objret[i].LINHVUCCHUNG, objret[i].LINHVUCCHUNG);
                        cboLinhVucChungSua.AddItem(objret[i].LINHVUCCHUNG, objret[i].LINHVUCCHUNG);
                    }

                    loadingdata2.Hide();
                },
                error: function () {

                    loadingdata2.Hide();
                }
            });


            cboLinhVucCon.ClearItems();
            cboLinhVucConSua.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDanhMucLinhVucConCayThuMucYCHT',
                data: "{ id: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    for (var i = 0; i < objret.length; i++) {
                        cboLinhVucCon.AddItem(objret[i].LINHVUCCON, objret[i].LINHVUCCON);
                        cboLinhVucConSua.AddItem(objret[i].LINHVUCCON, objret[i].LINHVUCCON);
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });


            cboLinhVucCha.ClearItems();
            cboLinhVucChaSua.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDanhMucChaCayThuMucYCHT',
                data: "{ id: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    for (var i = 0; i < objret.length; i++) {
                        cboLinhVucCha.AddItem(objret[i].LINHVUCCON, objret[i].ID);
                        cboLinhVucChaSua.AddItem(objret[i].LINHVUCCON, objret[i].ID);
                    }

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });

            cboDonViTiepNhanXL.ClearItems();
            cboDonViTiepNhanXLSua.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDanhMucDonViTNXLCayThuMucYCHT',
                data: "{ id: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    for (var i = 0; i < objret.length; i++) {
                        cboDonViTiepNhanXL.AddItem(objret[i].DONVI_TIEPNHAN_XL, objret[i].DONVI_TIEPNHAN_XL);
                        cboDonViTiepNhanXLSua.AddItem(objret[i].DONVI_TIEPNHAN_XL, objret[i].DONVI_TIEPNHAN_XL);
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
            <li><a onclick="themMoiLoaiYeuCauHoTro()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Thêm mới</span></a></li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt"></span></div>
            <div class="panel-body" style="border: none">
                <div class="container" style="margin-left: 0px">
                    <div class="row">
                        <div class="col-md-2"><strong>Chọn hệ thống:</strong></div>
                        <div class="col-md-2">
                            <dx:ASPxComboBox ID="cboChonHeThongHTKT" runat="server" ValueType="System.String" ClientInstanceName="cboChonHeThongHTKT" Theme="Office2010Blue">
                                <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChanged" />
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-8">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">&nbsp;</div>
                        <div class="col-md-4">&nbsp;</div>
                        <div class="col-md-4">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%"
                                    ClientInstanceName="CallbackPanel">
                                    <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">


                                            <dx:ASPxTreeList ID="asptree_caythumuchethong" ClientInstanceName="asptree_caythumuchethong" runat="server"
                                                KeyFieldName="ID" AutoGenerateColumns="False"
                                                OnCustomCallback="asptree_caythumuchethong_CustomCallback"
                                                OnHtmlDataCellPrepared="asptree_caythumuchethong_HtmlDataCellPrepared"
                                                OnVirtualModeCreateChildren="asptree_caythumuchethong_VirtualModeCreateChildren"
                                                OnVirtualModeNodeCreating="asptree_caythumuchethong_VirtualModeNodeCreating"
                                                OnVirtualModeNodeCreated="asptree_caythumuchethong_VirtualModeNodeCreated"
                                                EnablePagingGestures="False" EnableTheming="True" Theme="Aqua">
                                                <Columns>
                                                    <dx:TreeListTextColumn FieldName="ID" CellStyle-HorizontalAlign="Left" Caption="#" VisibleIndex="1">
                                                    </dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="MA_HETHONG" Caption="Mã hệ thống yc"></dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="LINHVUC" Caption="Lĩnh vực" VisibleIndex="0">
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                    </dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="MA_LINHVUC" Caption="Mã lĩnh vực" VisibleIndex="0">
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                    </dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="ID_CHA" Caption="Id cha"></dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="GHICHU" Caption="Ghi chú"></dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="TRANGTHAI" Caption="Trạng thái"></dx:TreeListTextColumn>
                                                    <dx:TreeListTextColumn FieldName="NGAYCAPNHAT" Caption="Ngày cập nhật"></dx:TreeListTextColumn>
                                                    <dx:TreeListCommandColumn>
                                                        <CustomButtons>
                                                            <dx:TreeListCommandColumnCustomButton ID="Them" Text="Thêm">
                                                                <Image Url="../../HTHTKT/icons/add_16x16.gif"></Image>
                                                            </dx:TreeListCommandColumnCustomButton>
                                                            <dx:TreeListCommandColumnCustomButton ID="Sua" Text="Sửa">
                                                                <Image Url="../../HTHTKT/icons/edit_16x16.gif"></Image>
                                                            </dx:TreeListCommandColumnCustomButton>
                                                            <dx:TreeListCommandColumnCustomButton ID="Xoa" Text="Xóa">
                                                                <Image Url="../../HTHTKT/icons/delete_16x16.gif"></Image>
                                                            </dx:TreeListCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dx:TreeListCommandColumn>

                                                </Columns>
                                                <SettingsPager Mode="ShowPager" PageSize="20">
                                                </SettingsPager>
                                                <SettingsSelection Enabled="True" />
                                                <SettingsLoadingPanel Text="Đang nạp&amp;hellip;" />
                                                <SettingsText LoadingPanelText="Đang nạp&amp;hellip;" />
                                                <ClientSideEvents CustomButtonClick="function(s, e) {
                                                   if(e.buttonID=='Them')
                                                        {
                                                            themMucYeuCauHoTro(e.nodeKey);
                                                        } 
                                                        if(e.buttonID=='Sua')
                                                        {
                                                            suaMucYeuCauHoTro(e.nodeKey);
                                                        } 
                                                        if(e.buttonID=='Xoa')
                                                        {
                                                            XoaMucYeuCauHoTro(e.nodeKey);
                                                        }                       
                                                    }"
                                                    ContextMenu="OnContextMenu" />
                                            </dx:ASPxTreeList>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- begin body boot -->
                <dx:ASPxLoadingPanel ID="loadingdata2" ClientInstanceName="loadingdata2" runat="server" Text="Đang nạp&amp;hellip;"></dx:ASPxLoadingPanel>
                <dx:ASPxPopupMenu ID="popupMenu" runat="server" ClientInstanceName="popupMenu">
                    <ClientSideEvents ItemClick="menu_OnItemClick" />
                    <Items>
                        <dx:MenuItem Text="Mở / Thu" />
                        <dx:MenuItem Text="Get node info..." />
                        <dx:MenuItem Text="Thông tin..." />
                    </Items>
                </dx:ASPxPopupMenu>
                <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="ASPxButton" Visible="False">
                    <ClientSideEvents Click="OnClick" />
                </dx:ASPxButton>
                <dx:ASPxPopupMenu ID="ASPxPopupMenu1" runat="server" ClientInstanceName="popupMenu2"
                    PopupHorizontalAlign="OutsideRight">
                    <Items>
                        <dx:MenuItem>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxPopupMenu>
                <!-- popup add -->
                <dx:ASPxPopupControl ID="popupThemMoiLoaiYeuCau" runat="server" ClientInstanceName="popupThemMoiLoaiYeuCau" HeaderText="Thêm mới node" Theme="Office2010Blue" Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Chọn hệ thống</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboDSHeThongYCHT" runat="server" ClientInstanceName="cboDSHeThongYCHT" Theme="Office2010Blue">
                                            <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChangedADD" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td>Lĩnh vực cha</td>
                                    <td>
                                        <%--<dx:ASPxComboBox ID="cboLinhVucCha" runat="server" ClientInstanceName="cboLinhVucCha" DropDownStyle="DropDown" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>--%>
                                        <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1ADD" runat="server" Width="200px"
                                            ClientInstanceName="CallbackPanelADD">
                                            <ClientSideEvents EndCallback="OnEndCallbackADD"></ClientSideEvents>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContentADD" runat="server">
                                                    <dx:ASPxDropDownEdit ID="LoaiHoTroADD" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTroADD" AnimationType="None" Width="100%">
                                                        <DropDownWindowTemplate>
                                                            <div>
                                                                <dx:ASPxTreeList ID="treelist_donvi_ccADD" runat="server" Width="350px" ClientInstanceName="treelist_donvi_ccADD"
                                                                    Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                                    OnVirtualModeCreateChildren="treelist_donvi_cc_ADD_VirtualModeCreateChildren"
                                                                    OnVirtualModeNodeCreating="treelist_donvi_cc_ADD_VirtualModeNodeCreating"
                                                                    OnCustomCallback="treelist_donvi_cc_ADD_OnCustomCallback" AutoGenerateColumns="False">
                                                                    <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                                    <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                                    <ClientSideEvents FocusedNodeChanged="ChonDonViADD" NodeExpanding="function(s, e) {
	                                                                      HiddenField.Set('hidden_value2ADD', 0);
                                                                    }" />
                                                                    <Columns>
                                                                        <dx:TreeListTextColumn FieldName="linhvuc" Caption="Mục hỗ trợ" Width="350px">
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
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td>Lĩnh vực</td>
                                    <td>
                                        <%--<dx:ASPxComboBox ID="cboLinhVucCon" runat="server" ClientInstanceName="cboLinhVucCon" DropDownStyle="DropDown" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>--%>
                                        <dx:ASPxTextBox ID="txtLinhVucADD" ClientInstanceName="txtLinhVucADD" runat="server" Width="300px"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã lĩnh vực</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaLinhVucADD" ClientInstanceName="txtMaLinhVucADD" runat="server" Width="300px"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Ghi chú</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtGhiChu" runat="server" Height="71px" Width="300px" ClientInstanceName="txtGhiChu" Theme="Office2010Blue">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThai" runat="server" CheckState="Unchecked" ClientInstanceName="chkTrangThai" Theme="Office2010Blue">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnThemMoi" runat="server" ClientInstanceName="btnThemMoi" Text="Thêm" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            themThongTin();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDong" runat="server" ClientInstanceName="btnDong" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupThemMoiLoaiYeuCau.Hide();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>


                <!-- popup edit -->
                <dx:ASPxPopupControl ID="popupSuaLoaiYeuCau" runat="server" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupSuaLoaiYeuCau" HeaderText="Sửa thông tin yêu cầu" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True" CloseAction="CloseButton" Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Chọn hệ thống</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboChonHeThongSua" runat="server" ClientInstanceName="cboChonHeThongSua" Theme="Office2010Blue">
                                            <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChangedEDIT" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Lĩnh vực cha</td>
                                    <td>
                                        <%--<dx:ASPxComboBox ID="cboLinhVucChaSua" runat="server" ClientInstanceName="cboLinhVucChaSua" DropDownStyle="DropDown" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>--%>

                                        <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1EDIT" runat="server" Width="200px"
                                            ClientInstanceName="CallbackPanelEDIT">
                                            <ClientSideEvents EndCallback="OnEndCallbackEDIT"></ClientSideEvents>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContentEDIT" runat="server">
                                                    <dx:ASPxDropDownEdit ID="LoaiHoTroEDIT" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTroEDIT" AnimationType="None" Width="100%">
                                                        <DropDownWindowTemplate>
                                                            <div>
                                                                <dx:ASPxTreeList ID="treelist_donvi_ccEDIT" runat="server" Width="350px" ClientInstanceName="treelist_donvi_ccEDIT"
                                                                    Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                                    OnVirtualModeCreateChildren="treelist_donvi_cc_EDIT_VirtualModeCreateChildren"
                                                                    OnVirtualModeNodeCreating="treelist_donvi_cc_EDIT_VirtualModeNodeCreating"
                                                                    OnCustomCallback="treelist_donvi_cc_EDIT_OnCustomCallback" AutoGenerateColumns="False">
                                                                    <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                                    <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                                    <ClientSideEvents FocusedNodeChanged="ChonDonViEDIT" NodeExpanding="function(s, e) {
	                                                                      HiddenField.Set('hidden_value2EDIT', 0);
                                                                    }" />
                                                                    <Columns>
                                                                        <dx:TreeListTextColumn FieldName="linhvuc" Caption="Mục hỗ trợ" Width="350px">
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
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td>Lĩnh vực</td>
                                    <td>
                                        <%--<dx:ASPxComboBox ID="cboLinhVucConSua" runat="server" ClientInstanceName="cboLinhVucConSua" DropDownStyle="DropDown" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>--%>
                                        <dx:ASPxTextBox ID="txtLinhVucEDIT" ClientInstanceName="txtLinhVucEDIT" runat="server" Width="300px"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã lĩnh vực</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaLinhVucEDIT" ClientInstanceName="txtMaLinhVucEDIT" runat="server" Width="300px"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Ghi chú</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtGhiChuSua" runat="server" Height="71px" Width="300px" ClientInstanceName="txtGhiChuSua" Theme="Office2010Blue">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThaiSua" runat="server" CheckState="Unchecked" ClientInstanceName="chkTrangThaiSua" Theme="Office2010Blue">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnSuaThongTin" runat="server" ClientInstanceName="btnSuaThongTin" Text="Sửa" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            suaThongTin();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" runat="server" ClientInstanceName="btnDongSua" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupSuaLoaiYeuCau.Hide();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <br />
                <br />
                <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1">
                </dx:ASPxHiddenField>

                <dx:ASPxHiddenField ID="HiddenField" runat="server" ClientInstanceName="HiddenField">
                </dx:ASPxHiddenField>
            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
