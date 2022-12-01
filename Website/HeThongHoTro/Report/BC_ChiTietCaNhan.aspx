<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="BC_ChiTietCaNhan.aspx.cs" Inherits="Website.HeThongHoTro.Report.BC_ChiTietCaNhan" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">

        function chonDonViAdd(s, e) {

            var tendv = '';
            var key = treeListDanhSachDonViAdd.GetFocusedNodeKey();
            treeListDanhSachDonViAdd.GetNodeValues(key, "TENDONVI", function (value) {
                ddlChonDonViAdd.SetText(value);
                ddlChonDonViAdd.SetKeyValue(key);
                ddlChonDonViAdd.HideDropDown();
            });
            treeListDanhSachDonViAdd.GetNodeValues(key, "ID", function (value) {
                ASPxHiddenField1.Set('ID_DONVI_ADD', value);
            });
        }


        function loadBaoCao(val, val2, val3, val4) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_baoCao.asmx/bc_chitietCaNhan',
                data: "{ tungay: '" + val + "',denngay: '" + val2 + "',idnguoidung: '" + val3 + "',tentruycap: '" + val4 + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    loadingdata.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    $("#timeLineND tbody").html("");
                    var table = '';
                    var tr;
                    for (var i = 0; i < objret.length; i++) {
                        tr = $('<tr/>');
                        tr.append("<td>" + objret[i].ID + "</td>");
                        tr.append("<td>" + objret[i].MA_YEUCAU + "</td>");
                        tr.append("<td>" + objret[i].SODIENTHOAI + "</td>");
                        tr.append("<td>" + objret[i].TENMUCDO + "</td>");
                        tr.append("<td>" + objret[i].LINHVUC + "</td>");
                        tr.append("<td>" + objret[i].MA_LINHVUC + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNG_YEUCAU + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNG_XL_DONG_HOTRO + "</td>");
                        tr.append("<td>" + objret[i].MOTA + "</td>");
                        tr.append("<td>" + objret[i].SOBUOC + "</td>");
                        tr.append("<td>" + objret[i].TENHETHONG + "</td>");
                        tr.append("<td>" + objret[i].MA_HETHONG + "</td>");
                        tr.append("<td>" + objret[i].TRANGTHAI + "</td>");
                        tr.append("<td>" + objret[i].NGUOITAO + "</td>");
                        tr.append("<td>" + objret[i].TENDONVI + "</td>");
                        tr.append("<td>" + objret[i].MADONVI + "</td>");
                        tr.append("<td>" + objret[i].NGAYTAO + "</td>");
                        tr.append("<td>" + objret[i].DONVIPHOIHOP + "</td>");
                        tr.append("<td><a onclick=\"xemThongTinChiTietCaNhan('" + objret[i].ID + "')\" style=\"cursor: pointer;\"><img src='../../HTHTKT/icons/view_16x16.gif' alt='Smiley face'> Xem</a></td>");
                        $("#timeLineND tbody").append(tr);
                    }
                    //if (val2 == 1) {
                    //    aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                    //    loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(val);
                    //} else if (val2 == 2) {
                    //    aspxPopupTimelineXuLyHoTroNodeCuoi.ShowAtPos(100, 100);
                    //}
                    if (objret.length > 0) {
                        $('#hienThiTongSoBanGhi').show();
                        $('#tongSoBanGhi').html(objret.length);
                    }
                    else {
                        tr = $('<tr/>');
                        tr.append("<td colspan='10'>Không có dữ liệu</td>");
                        $("#timeLine tbody").append(tr);
                    }
                    loadingdata.Hide();
                },
                error: function () {
                    loadingdata.Hide();
                    alert('có lỗi xảy ra khi lấy báo cáo');
                }
            });
        }
        function checkValid() {
            if (txtTuNgay.GetText() == null || txtTuNgay.GetText() == '') {
                alert('Bạn phải nhập từ ngày!');
                return false;
            }
            if (txtDenNgay.GetText() == null || txtDenNgay.GetText() == '') {
                alert('Bạn phải nhập đến ngày!');
                return false;
            }
            //if (ddlChonDonViAdd.GetKeyValue() == null || ddlChonDonViAdd.GetKeyValue() == '' || ddlChonDonViAdd.GetKeyValue() == 0) {
            //    alert('Bạn phải chọn đơn vị!');
            //    return false;
            //}
            return true;
        }

        function baocao() {
            if (!checkValid())
                return;

            var tungay = txtTuNgay.GetText();
            var denngay = txtDenNgay.GetText();
            var iddonvi = ddlChonDonViAdd.GetKeyValue(); //ASPxComboBox1.GetValue();
            //var idphongban = ASPxComboBox2.GetValue();

            var lstUser = [];
            var chkBox = document.getElementById('');
            if (chkBox != null) {
                var options = chkBox.getElementsByTagName('input');
                for (var i = 0; i < options.length; i++) {
                    if (options[i].checked) {
                        lstUser.push(options[i].attributes["value"].value);
                    }
                }
            }
            var idnguoidung = ASPxHiddenField1.Get('idnguoidung');
            var tentruycap = ASPxHiddenField1.Get('tentruycap');
            loadBaoCao(tungay, denngay, idnguoidung, tentruycap);
        }
        function xuatExcel() {
            $('#timeLineND').tableExport({ type: 'excel', escape: 'false' });
        }

         function loadTimelineHoTro(val, val2) {
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
                                tr.append("<td>" + objret[i].NGAYXULY + "</td>");
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
                            
                             aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);

                            loadingdata.Hide();
                        },
                        error: function () {
                            loadingdata.Hide();
                            alert('có lỗi xảy ra khi lấy timeline luồng xử lý, vui lòng thử lại sau!');
                        }
                    });
                }
        function loadThongTinYeuCauHoTro(val) {
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadThongTinYeuCauHoTro',
                        data: "{ id: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);

                            //tenHeThongHoTro.SetValue(objret[0].TENHOTRO);
                            //tenYeuCauHoTro.SetValue(objret[0].NOIDUNG_YEUCAU);
                            ///lbltenhethong.SetValue(objret[0].TENHOTRO);
                            //lbltenyeucauhotro.SetValue(objret[0].NOIDUNG_YEUCAU);
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi lấy thông tin về yêu cầu hỗ trợ,vui lòng thử lại sau');
                            return;
                        }
                    });
                }

                function loadToanBoThongTinYeuCauHoTroTheoID(val) {
                    debugger;
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadToanBoThongTinYeuCauHoTroTheoID',
                        data: "{ idchitietxlhotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            lblSoDienThoaiYC.SetText(objret.SODIENTHOAI);
                            lblMucDoYeuCauYC.SetText(objret.TENMUCDO);

                            lblTenLuongHoTroHTYC.SetText(objret.TEN_LUONG);
                            lblMoTaLuongHoTroHTYC.SetText(objret.MOTA);
                            lblTenHeThongHoTroKyThuatHTYC.SetText(objret.TENHETHONG);
                            lblNoiDungYeuCauHoTroGocHTYC.SetText(objret.NOIDUNG_YEUCAU);
                            //lblLinhVucChungHTYC.SetText(objret.LINHVUCCHUNG);
                            //lblLinhVucChungHTYC1.SetText(objret.LINHVUCCHUNG);
                            lblLinhVucConHTYC.SetText(objret.LINHVUC);

                            //loadTimelineHoTroXuLy(objret.ID_YEU_CAU_HOTRO, 1);
                        },
                        error: function () {
                        }
                    });
        }

                function xemThongTinChiTietCaNhan(val) {
                    loadThongTinYeuCauHoTro(val);
                    loadToanBoThongTinYeuCauHoTroTheoID(val);
                     loadTimelineHoTro(val, 1);
                }


    </script>
    <div style="padding: 10px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Báo cáo cá nhân</div>
                        <div class="panel-body">
                            <table class="table_baocao" width="100%">
                                <tr>
                                    <td>Từ ngày
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit1" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="txtTuNgay" runat="server" Theme="Office2010Silver">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>Đến ngày
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="ASPxDateEdit2" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="txtDenNgay" runat="server" Theme="Office2010Silver">
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>Đơn vị&nbsp;</td>
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
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingdata" runat="server" Modal="True" Text="Đang tải&amp;hellip;" Theme="Office2010Silver">
                                        </dx:ASPxLoadingPanel>
                                    </td>
                                    <td>
                                        <a onclick="baocao()" class="btn btn-primary"><i class="fa fa-pie-chart" aria-hidden="true"></i>Báo cáo</a>
                                        <a onclick="xuatExcel()" class="btn btn-success"><i class="fa fa-file-excel-o" aria-hidden="true"></i>Xuất Excel</a>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        <div style="padding: 5px; display: none" id="hienThiTongSoBanGhi">Có tổng số: <span class="badge"><span id="tongSoBanGhi"></span></span></div>
                        <div class="table-responsive" style="height: 500px">
                            <table id="timeLineND" class="table table-hover table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Mã y/c</th>
                                        <th>Số ĐT</th>
                                        <th>Tên mức độ</th>
                                        <th>Lĩnh vực</th>
                                        <th>Mã lĩnh vực</th>
                                        <th>Nội dung y/c</th>
                                        <th>Nội dung x.l đóng y/c</th>
                                        <th>Mô tả</th>
                                        <th>Số bước</th>
                                        <th>Tên hệ thống</th>
                                        <th>Mã HT</th>
                                        <th>Trạng thái</th>
                                        <th>Người tạo</th>
                                        <th>Tên đơn vị</th>
                                        <th>Mã đơn vị</th>
                                        <th>Ngày tạo</th>
                                        <th>Đơn vị phối hợp</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <%-- <div class="panel-footer"></div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1"></dx:ASPxHiddenField>

    <br />
                <dx:ASPxPopupControl ID="aspxPopupTimelineXuLyHoTro" runat="server" CloseAction="CloseButton" HeaderText="Thông tin xử lý hỗ trơ" Height="192px" Theme="Office2010Silver" Width="800px"
                ClientInstanceName="aspxPopupTimelineXuLyHoTro" AllowDragging="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table style="width: 100%;" class="tblKhaiBaoHT">
                            <tr>
                                <td class="labelhienthi">Thông tin luồng xử lý:</td>
                                <td>&nbsp;
                                <dx:ASPxLabel ID="lblTenLuongHoTroHTYC" ClientInstanceName="lblTenLuongHoTroHTYC" runat="server" Text="lblTenLuongHoTro">
                                </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Mô tả luồng xử lý:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblMoTaLuongHoTroHTYC" runat="server" ClientInstanceName="lblMoTaLuongHoTroHTYC" Text="lblMoTaLuongHoTroHTYC">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblTenHeThongHoTroKyThuatHTYC" runat="server" ClientInstanceName="lblTenHeThongHoTroKyThuatHTYC" Text="lblTenHeThongHoTroKyThuatHTYC">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Lĩnh vực:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblLinhVucConHTYC" runat="server" ClientInstanceName="lblLinhVucConHTYC" Text="lblLinhVucConHTYC">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Số điện thoại:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblSoDienThoaiYC" runat="server" ClientInstanceName="lblSoDienThoaiYC" Text="ASPxLabel">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Mức độ yêu cầu:</td>
                                <td>
                                    <dx:ASPxLabel ID="lblMucDoYeuCauYC" runat="server" ClientInstanceName="lblMucDoYeuCauYC" Text="ASPxLabel">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Nội dung yêu cầu</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblNoiDungYeuCauHoTroGocHTYC" ClientInstanceName="lblNoiDungYeuCauHoTroGocHTYC" runat="server" Text="lblNoiDungYeuCauHoTroGocHTYC">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi" style="color: darkred">Dòng sự kiện xử lý</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="table-responsive" style="height: 200px">
                                        <table id="timeLine" class="table table-hover table-striped">
                                            <%--header-fixed--%>
                                            <thead>
                                                <tr>
                                                    <th>Ngày</th>
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
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>&nbsp;</td>
                                <td>
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                        <ClientSideEvents Click="function(s, e) {
	                                        aspxPopupTimelineXuLyHoTro.Hide();
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
