<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_HtQuanLyDonVi.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_HtQuanLyDonVi" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


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


        // view chính hiển thị
        $(function () {
            treeListDanhSachDonVi.PerformCallback();
        });

        function themMoiDonvi() {
            popupThemMoiDonVi.Show();
        }

        function themMoiDonViTuGoc(val) {
            //themMucYeuCauHoTro(val)
            layThongTinDonviTheoID(val);

            txtMaDonViAdd.SetText('');
            txtTenDonViAdd.SetText('');
            ddlChonDonViAdd.SetText('');
            ddlChonDonViAdd.SetKeyValue(0);
            txtMoTaAdd.SetText('');
            txtDienThoaiAdd.SetText('');
            txtDiaChiAdd.SetText('');
            chkTrangThaiAdd.SetChecked(false);

            popupThemMoiDonVi.Show();
        }

        // thêm theo ID gốc cha
        function themMucYeuCauHoTro(val) {

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
                    ASPxHiddenField1.Set('id_donvi', objret[0].ID);

                    //txtMaDonViAdd.SetValue(objret[0].MADONVI);
                    //txtTenDonViAdd.SetValue(objret[0].TENDONVI);
                    //cboDonViChaAdd.SetValue(objret[0].ID_CHA);
                    //layThongTinDonviTheoID(objret[0].ID_CHA);

                    //txtMoTaAdd.SetValue(objret[0].MOTA);
                    //txtDienThoaiAdd.SetValue(objret[0].DIENTHOAI);
                    //txtDiaChiAdd.SetValue(objret[0].DIACHI);
                    //chkTrangThaiAdd.SetChecked(objret[0].TRANGTHAI);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });

        }


        function suaThongTinDonVi(val) {
            //alert(val);
            popupSuaThongTinDonVi.Show();

            suaMucYeuCauHoTro(val)
        }
        // sửa theo ID
        function suaMucYeuCauHoTro(val) {
            // Nạp thông tin xử lý
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
                    ASPxHiddenField1.Set('id_donvi', objret[0].ID);

                    txtMaDonViEdit.SetValue(objret[0].MADONVI);
                    txtTenDonViEdit.SetValue(objret[0].TENDONVI);


                    layThongTinDonviTheoID(objret[0].ID_CHA);

                    txtMoTaEdit.SetValue(objret[0].MOTA);
                    txtDienThoaiEdit.SetValue(objret[0].DIENTHOAI);
                    txtDiaChiEdit.SetValue(objret[0].DIACHI);
                    chkTrangThaiEdit.SetChecked(objret[0].TRANGTHAI);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
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


        function themThongTin() {
            //alert("thêm thông tin");
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themDonVi',
                data: "{ madonvi: '" + txtMaDonViAdd.GetText() +
                "',tendonvi: '" + txtTenDonViAdd.GetText() +
                "',idcha: '" + ddlChonDonViAdd.GetKeyValue() +
                "',mota: '" + txtMoTaAdd.GetText() +
                "',loaidonvi: '" + 0 +
                "',ghichu: '" + 0 +
                "',xoa: '" + 0 +
                "',dienthoai: '" + txtDienThoaiAdd.GetText() +
                "',fax: '" + 0 +
                "',diachi: '" + txtDiaChiAdd.GetText() +
                "',trangthai: '" + chkTrangThaiAdd.GetChecked() +
                "',idtinhthanh: '" + 0 +
                "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        treeListDanhSachDonVi.PerformCallback();
                        popupThemMoiDonVi.Hide();
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
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaDonVi',
                data: "{ id: '" + ASPxHiddenField1.Get('id_donvi') +
                "',madonvi: '" + txtMaDonViEdit.GetText() +
                "',tendonvi: '" + txtTenDonViEdit.GetText() +
                "',idcha:'" + ddlChonDonViEdit.GetKeyValue() +
                "',mota: '" + txtMoTaEdit.GetText() +
                "',loaidonvi: '" + 0 +
                "',ghichu: '" + 0 +
                "',xoa: '" + 0 +
                "',dienthoai: '" + txtDienThoaiEdit.GetText() +
                "',fax: '" + 0 +
                "',diachi: '" + txtDiaChiEdit.GetText() +
                "',trangthai: '" + chkTrangThaiEdit.GetChecked() +
                "',idtinhthanh: '" + 0 +
                "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        popupSuaThongTinDonVi.Hide();
                        treeListDanhSachDonVi.PerformCallback();
                        loadingdata2.Hide();
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


        function xoaThongTinDonVi(val) {
            // e.nodeKey: lấy id của row
            if (confirm('Bạn muốn xóa mục này?, chú ý nếu bạn xóa mục gốc thì tất cả mục con của gốc cũng sẽ bị xóa. Bạn muốn tiếp tục?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaDonVi',
                    data: "{ id: " + val + " }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            treeListDanhSachDonVi.PerformCallback();
                            alert('Xóa thành công');
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
            <li><a onclick="themMoiDonvi()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Thêm mới</span></a></li>
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
                        <div class="col-md-2"><strong>Quản lý đơn vị:</strong></div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-8">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonVi" AutoGenerateColumns="False" ClientInstanceName="treeListDanhSachDonVi" EnableTheming="True"
                                    KeyFieldName="ID" 
                                    OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                    OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                    OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                    OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                    OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                    OnHtmlRowPrepared="treeListDanhSachDonVi_HtmlRowPrepared"
                                    Theme="Aqua" EnablePagingGestures="False">
                                    <Styles>
                                        <FocusedNode Cursor="pointer" BackColor="LightBlue"></FocusedNode>
                                        <Node Cursor="pointer"></Node>
                                        <Indent Cursor="default"></Indent>
                                    </Styles>
                                     <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                    <Columns>
                                        <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="Mã đơn vị" FieldName="MADONVI" VisibleIndex="1">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="Tên đơn vị" FieldName="TENDONVI" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="Trạng thái" FieldName="TRANGTHAI" VisibleIndex="3" CellStyle-HorizontalAlign="Left">
                                        </dx:TreeListTextColumn>
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
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
                                        if(e.buttonID=='Them')
                                        {
                                            themMoiDonViTuGoc(e.nodeKey);
                                        } 
                                        if(e.buttonID=='Sua')
                                        {
                                            suaThongTinDonVi(e.nodeKey);
                                        } 
                                        if(e.buttonID=='Xoa')
                                        {
                                            xoaThongTinDonVi(e.nodeKey);
                                        }                       
                                    }" />
                                </dx:ASPxTreeList>
                            </div>
                        </div>
                    </div>
                </div>
                <dx:ASPxPopupControl ID="popupThemMoiDonVi" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới đơn vị" Modal="True" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupThemMoiDonVi" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên đơn vị</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenDonViAdd" runat="server" Width="170px" ClientInstanceName="txtTenDonViAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã đơn vị</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaDonViAdd" runat="server" Width="170px" ClientInstanceName="txtMaDonViAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Đơn vị cha</td>
                                    <td>


                                        <dx:ASPxDropDownEdit ID="ddlChonDonViAdd" runat="server" Theme="Office2010Blue"
                                            ClientInstanceName="ddlChonDonViAdd" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div style="height: 200px; overflow: scroll;">

                                                    <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonViAdd" AutoGenerateColumns="False"
                                                        ClientInstanceName="treeListDanhSachDonViAdd" EnableTheming="True"
                                                        KeyFieldName="ID"
                                                        OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                                        OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                                        OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                                        OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                                        Theme="Aqua" EnablePagingGestures="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2">
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
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMoTaAdd" runat="server" Width="170px" ClientInstanceName="txtMoTaAdd">
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
                                    <td>Địa chỉ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDiaChiAdd" runat="server" Width="170px" ClientInstanceName="txtDiaChiAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThaiAdd" runat="server" CheckState="Unchecked" Theme="Office2010Blue" ClientInstanceName="chkTrangThaiAdd">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnThemAdd" runat="server" AutoPostBack="False" Text="Thêm" Theme="Office2010Blue" ClientInstanceName="btnThemAdd">
                                            <ClientSideEvents Click="function(s, e) {
	                                            themThongTin();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupThemMoiDonVi.Hide();
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
                <dx:ASPxPopupControl ID="popupSuaThongTinDonVi" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Sửa thông tin đơn vị" Modal="True" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupSuaThongTinDonVi" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên đơn vị</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenDonViEdit" runat="server" Width="170px" ClientInstanceName="txtTenDonViEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã đơn vị</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaDonViEdit" runat="server" Width="170px" ClientInstanceName="txtMaDonViEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Đơn vị cha</td>
                                    <td>

                                        <dx:ASPxDropDownEdit ID="ddlChonDonViEdit" runat="server" Theme="Office2010Blue"
                                            ClientInstanceName="ddlChonDonViEdit" AnimationType="None" Width="100%">
                                            <DropDownWindowTemplate>
                                                <div style="height: 200px; overflow: scroll;">
                                                    <dx:ASPxTreeList runat="server" ID="treeListDanhSachDonViEdit" AutoGenerateColumns="False"
                                                        ClientInstanceName="treeListDanhSachDonViEdit" EnableTheming="True"
                                                        KeyFieldName="ID"
                                                        OnCustomCallback="treeListDanhSachDonVi_CustomCallback"
                                                        OnHtmlDataCellPrepared="treeListDanhSachDonVi_HtmlDataCellPrepared"
                                                        OnVirtualModeCreateChildren="treeListDanhSachDonVi_VirtualModeCreateChildren"
                                                        OnVirtualModeNodeCreated="treeListDanhSachDonVi_VirtualModeNodeCreated"
                                                        OnVirtualModeNodeCreating="treeListDanhSachDonVi_VirtualModeNodeCreating"
                                                        Theme="Aqua" EnablePagingGestures="False">
                                                        <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                        <Columns>
                                                            <dx:TreeListTextColumn Caption="ID" FieldName="ID" VisibleIndex="2">
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
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMoTaEdit" runat="server" Width="170px" ClientInstanceName="txtMoTaEdit">
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
                                    <td>Địa chỉ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDiaChiEdit" runat="server" Width="170px" ClientInstanceName="txtDiaChiEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThaiEdit" runat="server" CheckState="Unchecked" ClientInstanceName="chkTrangThaiEdit">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnSua" runat="server" AutoPostBack="False" Text="Sửa" Theme="Office2010Blue" ClientInstanceName="btnSua">
                                            <ClientSideEvents Click="function(s, e) {
        suaThongTin();	
}" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongEdit" runat="server" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False" ClientInstanceName="btnDongEdit">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupSuaThongTinDonVi.Hide();
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
                <!-- begin body boot -->

                <dx:ASPxLoadingPanel ID="loadingdata2" runat="server" ClientInstanceName="loadingdata2">
                </dx:ASPxLoadingPanel>

                <dx:ASPxHiddenField ID="ASPxHiddenField1" ClientInstanceName="ASPxHiddenField1" runat="server">
                </dx:ASPxHiddenField>

            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
