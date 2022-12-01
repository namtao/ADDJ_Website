<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="BC_BaoCaoTongHopSoLieu_YCHT_TheoPhongBan.aspx.cs" Inherits="Website.HeThongHoTro.Report.BC_BaoCaoTongHopSoLieu_YCHT_TheoPhongBan1" %>

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


        function loadBaoCao(val, val2, val3) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_baoCao.asmx/bc_baoCaoTongHopSoLieu_YCHT_TheoPhongBan',
                data: "{ tungay: '" + val + "',denngay: '" + val2 + "',id_donvi: " + val3 + " }",
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
                        tr.append("<td>" + objret[i].ID + "</td>");
                        tr.append("<td>" + objret[i].MAHOTRO + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNG_YEUCAU + "</td>");
                        tr.append("<td>" + objret[i].NGAYTAO + "</td>");
                        tr.append("<td>" + objret[i].TRANGTHAI + "</td>");
                        tr.append("<td>" + objret[i].NOIDUNG_XL_DONG_HOTRO + "</td>");
                        tr.append("<td>" + objret[i].TEN_LUONG + "</td>");
                        tr.append("<td>" + objret[i].MOTA + "</td>");
                        tr.append("<td>" + objret[i].SOBUOC + "</td>");
                        tr.append("<td>" + objret[i].TENHETHONG + "</td>");
                        tr.append("<td>" + objret[i].LOAI + "</td>");
                        tr.append("<td>" + objret[i].LINHVUCCHUNG + "</td>");
                        tr.append("<td>" + objret[i].LINHVUCCON + "</td>");
                        $("#timeLine tbody").append(tr);
                    }
                    //if (val2 == 1) {
                    //    aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                    //    loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(val);
                    //} else if (val2 == 2) {
                    //    aspxPopupTimelineXuLyHoTroNodeCuoi.ShowAtPos(100, 100);
                    //}
                    if (objret.length > 0) {
                        $('#hienThiTongSoBanGhi').show();
                        $('#tongSoBanGhi').html(objret.length + 1);
                    }
                    loadingdata.Hide();
                },
                error: function () {
                    loadingdata.Hide();
                    alert('có lỗi xảy ra khi lấy timeline luồng xử lý');
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
            if (ASPxComboBox1.GetValue() == null || ASPxComboBox1.GetValue() == '' || ASPxComboBox1.GetValue() == 0) {
                alert('Bạn phải chọn đơn vị!');
                return false;
            }
            return true;
        }
        function baocao() {
            if (!checkValid())
                return;
            var tungay = txtTuNgay.GetText();
            var denngay = txtDenNgay.GetText();           
            var idphongban = ddlChonDonViAdd.GetKeyValue(); // ASPxComboBox2.GetValue();
            loadBaoCao(tungay, denngay, idphongban);
        }
        function xuatExcel() {
            $('#timeLine').tableExport({ type: 'excel', escape: 'false' });
        }        	 
    </script>
    <div style="padding: 10px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">Báo cáo tổng hợp số liệu Yêu cầu hệ thống theo Phòng ban</div>
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
                                <tr>
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
                                    <td></td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingdata" runat="server" Modal="True" Text="Đang tải&amp;hellip;" Theme="Office2010Silver">
                                        </dx:ASPxLoadingPanel>
                                    </td>
                                    <td>
                                        <a onclick="baocao()" class="btn btn-primary"><i class="fa fa-pie-chart" aria-hidden="true"></i> Báo cáo</a>
                                        <a onclick="xuatExcel()" class="btn btn-success"><i class="fa fa-file-excel-o" aria-hidden="true"></i> Xuất Excel</a>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        <div style="padding:5px; display:none" id="hienThiTongSoBanGhi">Có tổng số: <span class="badge"><span id="tongSoBanGhi"></span></span></div>
                        <div class="table-responsive" style="height: 500px">
                            <table id="timeLine" class="table table-hover table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Mã hỗ trợ</th>
                                        <th>Nội dung y/c</th>
                                        <th>Ngày tạo</th>
                                        <th>Trạng thái</th>
                                        <th>Nội dung xl đóng ht</th>
                                        <th>Tên luồng </th>
                                        <th>Mô tả</th>
                                        <th>Số bước</th>
                                        <th>Tên hệ thống</th>
                                        <th>Loại</th>
                                        <th>Lĩnh vực chung</th>
                                        <th>Lĩnh vực con</th>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
