<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_HtQuanLyNguoiDung.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_HtQuanLyNguoiDung" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
    <script type="text/javascript">
        function chonDonViAdd(s, e) {

            var tendv = '';
            var key = treeListDanhSachDonViAdd.GetFocusedNodeKey();
            treeListDanhSachDonViAdd.GetNodeValues(key, "TENDONVI", function (value) {
                ddlChonDonViAdd.SetText(value);
                ddlChonDonViAdd.SetKeyValue(key);
                ddlChonDonViAdd.HideDropDown();
            });
            // khong can
            //treeListDanhSachDonViAdd.GetNodeValues(key, "ID", function (value) {
            //    ASPxHiddenField1.Set('ID_DONVI_ADD', value);
            //});
        }
        function chonDonViEdit(s, e) {
            var tendv = '';
            var key = treeListDanhSachDonViEdit.GetFocusedNodeKey();
            treeListDanhSachDonViEdit.GetNodeValues(key, "TENDONVI", function (value) {
                ddlChonDonViEdit.SetText(value);
                ddlChonDonViEdit.SetKeyValue(key);
                ddlChonDonViEdit.HideDropDown();
            });
            // khong can
            //treeListDanhSachDonViAdd.GetNodeValues(key, "ID", function (value) {
            //    ASPxHiddenField1.Set('ID_DONVI_EDIT', value);
            //});
        }


        $(function () {
            treeListDanhSachDonVi.PerformCallback();
        });
        function OnNodeDbClick(s, e) {
            //alert(e.nodeKey);
            //s.StartEdit(e.nodeKey);
            ASPxHiddenField1.Set('ID_DONVI', e.nodeKey);
            // lblTenDonVi.SetText(e.nodeKey);
            //var key = s.GetFocusedNodeKey();
            //alert(key);
            // var keyValue = ASPxTreeList1.GetFocusedNodeKey();
            treeListDanhSachDonVi.GetNodeValues(e.nodeKey, "ID", function (value) {
                ddlChonDonViAdd.SetKeyValue(value);
                ddlChonDonViEdit.SetKeyValue(value);
            });
            treeListDanhSachDonVi.GetNodeValues(e.nodeKey, "TENDONVI", function (value) {
                ddlChonDonViAdd.SetText(value);
                ddlChonDonViEdit.SetText(value);
            });

            treeListDanhSachDonVi.GetNodeValues(e.nodeKey, 'TENDONVI', ProcessValue);
            grvNguoiDung.PerformCallback();
        }

        function ProcessValue(val) {
            //alert(val);
            lblTenDonVi.SetText(val);
        }


        function xemNguoiDungDonVi(val) {
            //alert(val);
            ASPxHiddenField1.Set('ID_DONVI', val);
            grvNguoiDung.PerformCallback();
        }

        function themMoiNguoDungTuGoc(val) {
            ASPxHiddenField1.Set('ID_DONVI', val);
             treeListDanhSachDonVi.GetNodeValues(val, "ID", function (value) {
                ddlChonDonViAdd.SetKeyValue(value);
                ddlChonDonViEdit.SetKeyValue(value);
            });
            treeListDanhSachDonVi.GetNodeValues(val, "TENDONVI", function (value) {
                ddlChonDonViAdd.SetText(value);
                ddlChonDonViEdit.SetText(value);
             });

            txtTenTruyCapAdd.SetText('');
            txtMatKhauAdd.SetText('');
            txtHoTenAdd.SetText('');
            txtDiaChiAdd.SetText('');
            txtDienThoaiAdd.SetText('');
            cboTrangThai.SetChecked(false);
            cboGiuCuVaThemMoi.SetChecked(false);
            txtEmailAdd.SetText('');

            popupThemMoiNguoiDung.Show();
        }

        // thêm theo ID gốc cha
        function themMucYeuCauHoTro(val) {
            napDanhSachHeThongYCHT();
            popupThemMoiLoaiYeuCau.Show();


            // trường hợp thêm mới này thì id cha = id hiện chọn thêm và hệ thống ứng với hệ thống
            // Nạp thông tin xử lý
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinNguoiDungTheoID',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id_nguoidung', objret[0].Id);

                    //cboDSHeThongYCHT.SetValue(objret[0].ID_HETHONG_YCHT);  // disabled
                    //cboDSHeThongYCHT.SetEnabled(false);
                    //cboLinhVucChung.SetValue(objret[0].LINHVUCCHUNG);
                    //cboLinhVucChung.SetEnabled(false);
                    //cboLinhVucCha.SetEnabled(false);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function themMoiNguoiDung() {
            cboNhomNguoiDungAdd.SetValue();
            txtTenTruyCapAdd.SetText('');
            txtMatKhauAdd.SetText('');
            txtHoTenAdd.SetText('');
            txtDiaChiAdd.SetText('');
            txtDienThoaiAdd.SetText('');
            cboTrangThai.SetChecked(false);
            cboGiuCuVaThemMoi.SetChecked(false);
            txtEmailAdd.SetText('');

            popupThemMoiNguoiDung.Show();
        }

        function suaThongTinNguoiDung(val, val2) {
            grvNguoiDung.GetRowValues(val2, 'ID_DONVI_NGUOIDUNG;ID_DONVI_ND', function (values) {
                //alert(values);
                ASPxHiddenField1.Set('id_donvi_nguoidung', values[0]);
                ASPxHiddenField1.Set('ID_DONVI_ND', values[1]);
                suaMucYeuCauHoTro(val);
            });
            popupSuaThongTinNguoiDung.Show();
        }



        function layThongTinDonviTheoID(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinDonViTheoID',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);

                    ddlChonDonViAdd.SetText(objret[0].TENDONVI);
                    ddlChonDonViAdd.SetKeyValue(objret[0].ID);


                    ddlChonDonViEdit.SetText(objret[0].TENDONVI);
                    ddlChonDonViEdit.SetKeyValue(objret[0].ID);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        // sửa theo ID
        function suaMucYeuCauHoTro(val) {

            // Nạp thông tin xử lý
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinNguoiDungTheoID',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id_nguoidung', objret[0].Id);
                    ASPxHiddenField1.Set('ID_DONVI_EDIT', objret[0].ID_DONVI);
                    layThongTinDonviTheoID(ASPxHiddenField1.Get('ID_DONVI_ND'));

                    txtHoTenEdit.SetValue(objret[0].TenDayDu);
                    txtTenTruyCapEdit.SetValue(objret[0].TenTruyCap);

                    txtTenTruyCapEdit.GetInputElement().setAttribute('style', 'background:#CCCCCC;');
                    txtTenTruyCapEdit.GetInputElement().readOnly = true;

                    //txtMatKhauEdit.SetValue(objret[0].LINHVUCCON);
                    var dt = new Date(objret[0].NgaySinh);
                    dateNgaySinhEdit.SetDate(dt);
                    rdoGioiTinhEdit.SetValue(objret[0].Sex);
                    txtDiaChiEdit.SetValue(objret[0].DiaChi);
                    txtDienThoaiEdit.SetText(objret[0].DiDong);
                    txtEmailEdit.SetText(objret[0].Email);
                    cboTrangThaiEdit.SetChecked(objret[0].TrangThai);

                    cboNhomNguoiDungEdit.SetValue(objret[0].NhomNguoiDung);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function convert(str) {
            var date = new Date(str),
                mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                day = ("0" + date.getDate()).slice(-2);
            return [date.getFullYear(), mnth, day].join("-");
        }


        
        function kiemTraTenTruyCapNguoiDung(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/kiemtraTenDangNhapNguoiDung',
                data: "{ tentruycap: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Đã tồn tại người dùng với tên truy cập bạn nhập vào, vui lòng nhập tên khác.');
                    }
                    
                    else {
                        // thực hiện thêm mới
                        themThongTin();
                    }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function themThongTin() {
            if (cboGiuCuVaThemMoi.GetChecked()) {
                if (ddlChonDonViAdd.GetKeyValue() !== ASPxHiddenField1.Get('ID_DONVI'))
                    if (!confirm('Bạn đã chọn giữ người dùng này ở đơn vị cũ và chuyển đến cả đơn vị mới, tức là có đồng thời ở nhiều đơn vị, bạn muốn tiếp tục?')) {
                        return;
                    }
            }
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themNguoiDung',
                data: "{ tendoitac: '" + 0 +
                "',doitacid: '" + 0 +
                "',khuvucid: '" + 0 +
                "',nhomnguoidung: '" + cboNhomNguoiDungAdd.GetValue() +
                "',tentruycap: '" + txtTenTruyCapAdd.GetText() +
                "',matkhau: '" + txtMatKhauAdd.GetText() +
                "',tendaydu: '" + txtHoTenAdd.GetText() +
                "',manhanvien_cmt: '" + txtMaNhanVienCMTAdd.GetText() +
                "',ngaysinh: '" + convert(dateNgaySinhAdd.GetDate()) +
                "',diachi: '" + txtDiaChiAdd.GetText() +
                "',didong: '" + txtDienThoaiAdd.GetText() +
                "',codinh: '" + 0 +
                "',sex: '" + rdoGioiTinhAdd.GetValue() +
                "',sex: '" + 0 +
                "',email: '" + txtEmailAdd.GetText() +
                "',congty: '" + 0 +
                "',diachicongty: '" + 0 +
                "',faxcongty: '" + 0 +
                "',dienthoaicongty: '" + 0 +
                "',trangthai: '" + cboTrangThai.GetChecked() +
                "',sudungldap: '" + 0 +
                "',logincount: '" + 0 +
                "',lastlogin: '" + 0 +
                "',islogin: '" + 1 +
                "',cuser: '" + 0 +
                "',luser: '" + 0 +
                "',iddonvi: '" + ddlChonDonViAdd.GetKeyValue() +
                "',iddonvi_cu: '" + ASPxHiddenField1.Get('ID_DONVI') +
                "',is_giucu_vathemmoi: '" + cboGiuCuVaThemMoi.GetChecked() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        grvNguoiDung.PerformCallback();
                        popupThemMoiNguoiDung.Hide();
                        loadingdata2.Hide();
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
            if (cboGiuCuVaThemMoi.GetChecked()) {
                if (ddlChonDonViAdd.GetKeyValue() !== ASPxHiddenField1.Get('ID_DONVI'))
                    if (!confirm('Bạn đã chọn giữ người dùng này ở đơn vị cũ và chuyển đến cả đơn vị mới, tức là có đồng thời ở nhiều đơn vị, bạn muốn tiếp tục?')) {
                        return;
                    }
            }
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaNguoiDung',
                data: "{ id: '" + ASPxHiddenField1.Get('id_nguoidung') +
                "',tendoitac: '" + 0 +
                "',doitacid: '" + 0 +
                "',khuvucid: '" + 0 +
                "',nhomnguoidung: '" + cboNhomNguoiDungEdit.GetValue() +
                "',tentruycap: '" + txtTenTruyCapEdit.GetText() +
                "',matkhau: '" + txtMatKhauEdit.GetText() +
                "',tendaydu: '" + txtHoTenEdit.GetText() +
                "',manhanvien_cmt: '" + txtMaNhanVienCMTEdit.GetText() +
                "',ngaysinh: '" + convert(dateNgaySinhEdit.GetDate()) +
                "',diachi: '" + txtDiaChiEdit.GetText() +
                "',didong: '" + txtDienThoaiEdit.GetText() +
                "',codinh: '" + 0 +
                "',sex: '" + rdoGioiTinhEdit.GetValue() +
                "',email: '" + txtEmailEdit.GetText() +
                "',congty: '" + 0 +
                "',diachicongty: '" + txtDiaChiEdit.GetText() +
                "',faxcongty: '" + 0 +
                "',dienthoaicongty: '" + 0 +
                "',trangthai: '" + cboTrangThaiEdit.GetChecked() +
                "',sudungldap: '" + 0 +
                "',logincount: '" + 0 +
                "',lastlogin: '" + 0 +
                "',islogin: '" + 1 +
                "',cuser: '" + 0 +
                "',luser: '" + 0 +
                "',iddonvi: '" + ddlChonDonViEdit.GetKeyValue() +
                "',iddonvi_cu: '" + ASPxHiddenField1.Get('ID_DONVI') +
                "',id_donvi_nguoidung: '" + ASPxHiddenField1.Get('id_donvi_nguoidung') +
                "',is_giucu_vathemmoi: '" + cboGiuCuVaThemMoi.GetChecked() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        grvNguoiDung.PerformCallback();
                        popupSuaThongTinNguoiDung.Hide();
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

        function xoaThongTinNguoiDung(val, val2) {
            grvNguoiDung.GetRowValues(val2, 'ID_DONVI_NGUOIDUNG', function (values) {
                //alert(values);
                ASPxHiddenField1.Set('id_donvi_nguoidung', values);
            });


            // e.nodeKey: lấy id của row
            if (confirm('Bạn muốn xóa mục này?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaNguoiDungVaDonViNguoiDung',
                    data: "{ id: " + val + ", id_donvi: " + ASPxHiddenField1.Get('ID_DONVI') + ", id_donvi_nguoidung: " + ASPxHiddenField1.Get('id_donvi_nguoidung') + " }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');
                            grvNguoiDung.PerformCallback();
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

        function timkiem(s, e)
        {

        }
    </script>
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
            <li><a onclick="themMoiNguoiDung()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Thêm mới</span></a></li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt"></span></div>
            <div class="panel-body" style="border: none">
                <div class="container-fluid" style="margin-left: 0px">
                    <div class="row">
                        <div class="col-md-5"></div>
                        <div class="col-md-2">
                            <strong>Tên đăng nhập:</strong>
                           
                        </div>
                          <div class="col-md-2">
                               <dx:ASPxTextBox ID="txtTenDangNhap" runat="server" ClientInstanceName="txtTenDangNhap"
                                 Theme="Office2010Blue"></dx:ASPxTextBox>
                              </div>
                         <div class="col-md-3">
                            <dx:ASPxButton ID="btnTimKiem" runat="server" ClientInstanceName="btnTimKiem"
                                 Theme="Office2010Blue" Text="Tìm kiếm" OnClick="btnTimKiem_Click">
                                <Image Url="../../HTHTKT/icons/search_16x16.gif"></Image>
                                <ClientSideEvents Click="timkiem" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5"><strong>Chọn đơn vị:</strong></div>
                        <div class="col-md-7">
                            <strong>Danh sách người dùng đơn vị:
                                <dx:ASPxLabel ID="lblTenDonVi" ForeColor="LightCoral" ClientInstanceName="lblTenDonVi" runat="server" Text="(Chọn đơn vị để xem danh sách)"></dx:ASPxLabel>
                            </strong>
                        </div>
                    </div>
                    <div class="row">
                        <!-- Đơn vị -->
                        <div class="col-md-5">
                            <div style="height: 500px; overflow: scroll">
                                <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonVi" AutoGenerateColumns="False" ClientInstanceName="treeListDanhSachDonVi" EnableTheming="True"
                                    KeyFieldName="ID"
                                    OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                    OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                    OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                    OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                    OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                    Theme="Aqua" EnablePagingGestures="False">
                                    <Styles>
                                        <FocusedNode Cursor="pointer" BackColor="#ff9900"></FocusedNode>
                                        <Node Cursor="pointer"></Node>
                                        <Indent Cursor="default"></Indent>
                                    </Styles>
                                    <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                    <Columns>
                                        <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2" Visible="false">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="Mã đơn vị" FieldName="MADONVI" VisibleIndex="1">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="Tên đơn vị" FieldName="TENDONVI" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListCommandColumn>
                                            <CustomButtons>
                                                <dx:TreeListCommandColumnCustomButton ID="Xem" Text="Xem">
                                                    <Image Url="../../HTHTKT/icons/view_16x16.gif"></Image>
                                                </dx:TreeListCommandColumnCustomButton>
                                                <dx:TreeListCommandColumnCustomButton ID="Them" Text="Thêm">
                                                    <Image Url="../../HTHTKT/icons/add_16x16.gif"></Image>
                                                </dx:TreeListCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:TreeListCommandColumn>
                                    </Columns>
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
                                                if(e.buttonID=='Xem')
                                                {
                                                    xemNguoiDungDonVi(e.nodeKey);
                                                } 
                                                if(e.buttonID=='Them')
                                                {
                                                    themMoiNguoDungTuGoc(e.nodeKey);
                                                } 
                                            }"
                                        NodeDblClick="OnNodeDbClick" />
                                </dx:ASPxTreeList>
                            </div>
                        </div>
                        <!-- Người dùng thuộc đơn vị -->
                        <div class="col-md-7">
                            <div style="height: 500px; overflow: scroll">
                                <dx:ASPxGridView ID="grvNguoiDung" runat="server" ClientInstanceName="grvNguoiDung"
                                    AutoGenerateColumns="False" EnablePagingGestures="False" KeyFieldName="Id"
                                    OnCustomButtonInitialize="grvNguoiDung_CustomButtonInitialize"
                                    OnCustomColumnDisplayText="grvNguoiDung_CustomColumnDisplayText"
                                    OnCustomDataCallback="grvNguoiDung_CustomDataCallback"
                                    OnDataBinding="grvNguoiDung_DataBinding"
                                    OnHtmlDataCellPrepared="grvNguoiDung_HtmlDataCellPrepared"
                                    OnPageIndexChanged="grvNguoiDung_PageIndexChanged" Theme="Office2010Blue" OnCustomCallback="grvNguoiDung_CustomCallback">
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tên truy cập" FieldName="TenTruyCap" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Họ tên" FieldName="TenDayDu" VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Mã nhân viên/CMT" FieldName="MaNhanVienCMT" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" VisibleIndex="5">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Điện thoại" FieldName="DiDong" VisibleIndex="6">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Visible="false" Caption="Id DV_ND" FieldName="ID_DONVI_NGUOIDUNG" VisibleIndex="7">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Thuộc nhóm" FieldName="TenNhom" VisibleIndex="8">
                                        </dx:GridViewDataTextColumn>
                                         <dx:GridViewDataTextColumn Caption="ID DV" FieldName="ID_DONVI_ND" VisibleIndex="9">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn>
                                            <DataItemTemplate>
                                                <dx:ASPxHyperLink Font-Size="11px" ForeColor="Blue" ID="ASPxHyperLinkTest" Target="_blank" runat="server" Text="Cấp quyền"
                                                    NavigateUrl='<%#string.Format("mn_ThietLapQuyenNguoiDung.aspx?idnguoidung={0}",Eval("Id"))%>'>
                                                </dx:ASPxHyperLink>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="Sua" Text="Sửa" Image-Url="../../HTHTKT/icons/edit_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                                                <dx:GridViewCommandColumnCustomButton ID="Xoa" Text="Xóa" Image-Url="../../HTHTKT/icons/delete_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <CellStyle HorizontalAlign="Left"></CellStyle>
                                        </dx:GridViewCommandColumn>
                                    </Columns>
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
                                            if(e.buttonID=='Sua')
                                            {
                                                suaThongTinNguoiDung(grvNguoiDung.GetRowKey(e.visibleIndex), e.visibleIndex);	
                                            }
                                            if(e.buttonID=='Xoa')
                                            {
                                                xoaThongTinNguoiDung(grvNguoiDung.GetRowKey(e.visibleIndex), e.visibleIndex);	
                                            }	     
                                    }" />
                                </dx:ASPxGridView>
                            </div>
                            <dx:ASPxHiddenField ID="ASPxHiddenField1" ClientInstanceName="ASPxHiddenField1" runat="server"></dx:ASPxHiddenField>
                        </div>
                    </div>
                </div>
                <!-- begin body boot -->

                <dx:ASPxPopupControl ID="popupThemMoiNguoiDung" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới người dùng" Modal="True" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupThemMoiNguoiDung" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Thuộc đơn vị</td>
                                    <td>
                                        <dx:ASPxDropDownEdit ID="ddlChonDonViAdd" runat="server" Theme="Office2010Blue"
                                            ClientInstanceName="ddlChonDonViAdd" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div style="height: 200px; overflow: scroll;">
                                                    <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonViAdd" AutoGenerateColumns="False"
                                                        ClientInstanceName="treeListDanhSachDonViAdd" Width="350px" EnableTheming="True"
                                                        KeyFieldName="ID"
                                                        OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                                        OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                                        OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                                        OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                                        Theme="Aqua" EnablePagingGestures="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2" Visible="false">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Mã đơn vị" FieldName="MADONVI" VisibleIndex="1">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Tên đơn vị" FieldName="TENDONVI" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                                                            </dx:TreeListTextColumn>
                                                        </Columns>
                                                        <ClientSideEvents FocusedNodeChanged="chonDonViAdd" />
                                                    </dx:ASPxTreeList>
                                                </div>
                                            </DropDownWindowTemplate>
                                        </dx:ASPxDropDownEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Họ tên</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtHoTenAdd" runat="server" Width="170px" ClientInstanceName="txtHoTenAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Tên truy cập</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenTruyCapAdd" runat="server" Width="170px" ClientInstanceName="txtTenTruyCapAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mật khẩu</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMatKhauAdd" runat="server" Width="170px" ClientInstanceName="txtMatKhauAdd" Password="True">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã nhân viên/CMT</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaNhanVienCMTAdd" runat="server" AccessibilityCompliant="True" ClientInstanceName="txtMaNhanVienCMTAdd" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Ngày sinh</td>
                                    <td>
                                        <dx:ASPxDateEdit ID="dateNgaySinhAdd" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" runat="server" ClientInstanceName="dateNgaySinhAdd">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                 <tr>
                                    <td>Giới tính</td>
                                    <td>
                                        <dx:ASPxRadioButtonList ID="rdoGioiTinhAdd" ClientInstanceName="rdoGioiTinhAdd" runat="server" RepeatDirection="Horizontal">
                                            <Items>
                                                <dx:ListEditItem Text="Nam" Value="1" Selected="true" />
                                                <dx:ListEditItem Text="Nữ" Value="0" />
                                            </Items>
                                        </dx:ASPxRadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Địa chỉ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDiaChiAdd" runat="server" Width="170px" ClientInstanceName="txtDiaChiAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Điện thoại</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDienThoaiAdd" runat="server" Width="170px" ClientInstanceName="txtDienThoaiAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Email</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtEmailAdd" runat="server" Width="170px" ClientInstanceName="txtEmailAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Thuộc nhóm</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboNhomNguoiDungAdd" ClientInstanceName="cboNhomNguoiDungAdd" runat="server"
                                            Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="cboTrangThai" runat="server" CheckState="Unchecked" ClientInstanceName="cboTrangThai">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnThem" ClientInstanceName="btnThem" runat="server" AutoPostBack="False" Text="Thêm" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            kiemTraTenTruyCapNguoiDung(txtTenTruyCapAdd.GetValue());
                                            }" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongThem" ClientInstanceName="btnDongThem" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupThemMoiNguoiDung.Hide();
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
                <dx:ASPxPopupControl ID="popupSuaThongTinNguoiDung" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Sửa thông tin người dùng" Modal="True" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupSuaThongTinNguoiDung" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Thuộc đơn vị</td>
                                    <td>
                                        <dx:ASPxDropDownEdit ID="ddlChonDonViEdit" runat="server" Theme="Office2010Blue"
                                            ClientInstanceName="ddlChonDonViEdit" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div style="height: 200px; overflow: scroll;">
                                                    <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonViEdit" AutoGenerateColumns="False"
                                                        ClientInstanceName="treeListDanhSachDonViEdit" Width="350px" EnableTheming="True"
                                                        KeyFieldName="ID"
                                                        OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                                        OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                                        OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                                        OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                                        Theme="Aqua" EnablePagingGestures="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2" Visible="false">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Mã đơn vị" FieldName="MADONVI" VisibleIndex="1">
                                                            </dx:TreeListTextColumn>
                                                            <dx:TreeListTextColumn Caption="Tên đơn vị" FieldName="TENDONVI" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                                                            </dx:TreeListTextColumn>
                                                        </Columns>
                                                        <ClientSideEvents FocusedNodeChanged="chonDonViEdit" />
                                                    </dx:ASPxTreeList>
                                                </div>
                                            </DropDownWindowTemplate>
                                        </dx:ASPxDropDownEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Họ tên</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtHoTenEdit" runat="server" Width="170px" ClientInstanceName="txtHoTenEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Tên truy cập</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenTruyCapEdit" runat="server" Width="170px" ClientInstanceName="txtTenTruyCapEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mật khẩu</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMatKhauEdit" runat="server" Width="170px" ClientInstanceName="txtMatKhauEdit" Password="True">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã nhân viên/CMT</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaNhanVienCMTEdit" runat="server" ClientInstanceName="txtMaNhanVienCMTEdit" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Ngày sinh</td>
                                    <td>
                                        <dx:ASPxDateEdit ID="dateNgaySinhEdit" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" runat="server" ClientInstanceName="dateNgaySinhEdit">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Giới tính</td>
                                    <td>
                                        <dx:ASPxRadioButtonList ID="rdoGioiTinhEdit" ClientInstanceName="rdoGioiTinhEdit" runat="server" RepeatDirection="Horizontal">
                                            <Items>
                                                <dx:ListEditItem Text="Nam" Value="1"  Selected="true"/>
                                                <dx:ListEditItem Text="Nữ" Value="0" />
                                            </Items>
                                        </dx:ASPxRadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Địa chỉ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDiaChiEdit" runat="server" Width="170px" ClientInstanceName="txtDiaChiEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Điện thoại</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDienThoaiEdit" runat="server" Width="170px" ClientInstanceName="txtDienThoaiEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Email</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtEmailEdit" runat="server" Width="170px" ClientInstanceName="txtEmailEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Thuộc nhóm</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboNhomNguoiDungEdit" ClientInstanceName="cboNhomNguoiDungEdit" runat="server"
                                            Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="cboTrangThaiEdit" runat="server" CheckState="Unchecked" ClientInstanceName="cboTrangThaiEdit">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr style="display:none">
                                    <td>Giữ ở đơn vị cũ và tạo ở đơn vị mới</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="cboGiuCuVaThemMoi" runat="server" CheckState="Unchecked" ClientInstanceName="cboGiuCuVaThemMoi">
                                        </dx:ASPxCheckBox>
                                        <span style="color: coral">(tùy chọn này người dùng sẽ vừa ở đơn vị cũ và vừa ở đơn vị mới)</span></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnSua" ClientInstanceName="btnSua" runat="server" AutoPostBack="False" Text="Sửa" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            suaThongTin();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" ClientInstanceName="btnDongSua" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupSuaThongTinNguoiDung.Hide();
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
                <dx:ASPxLoadingPanel ID="loadingdata2" runat="server" ClientInstanceName="loadingdata2">
                </dx:ASPxLoadingPanel>
            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
