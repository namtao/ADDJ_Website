<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_DmLuongXuLy.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_DMLuongXuLy" %>

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

             //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChanged(s, e) {
            debugger;
            // gán giá trị id cho hidden
            ASPxHiddenField1.Set('ThongHTKTID', s.GetValue());
            //cboDSHeThongYCHT.SetValue(ASPxHiddenField1.Get('ThongHTKTID'));
            grvDmLuongXuLy.PerformCallback();
        }


        function suaMucLuongXuLy(val) {
            popupSuaThongTinLuongXL.Show();
            napDanhSachHeThongYCHT();
            napSoBuocTrongLuongXL();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucLuongXuLy',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id_luonght', objret[0].ID);

                    cboChonHeThongSua.SetValue(objret[0].ID_HETHONG_YCHT);
                    txtTenLuongSua.SetText(objret[0].TEN_LUONG);
                    cboTrangThaiSua.SetChecked(objret[0].TRANGTHAI);
                    txtMoTaSua.SetText(objret[0].MOTA);
                    cboSoBuocSua.SetValue(objret[0].SOBUOC);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function xoaMucLuongXuLy(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaMucLuongXuLy',
                    data: "{ id: " + val + " }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');
                            grvDmLuongXuLy.PerformCallback();
                        }
                        else if (jsonData.d == '0') {
                            alert('Có lỗi trong quá trình xóa');
                        }
                        loadingdata2.Hide();
                    },
                    error: function () {
                        loadingdata2.Hide();
                    }
                });
            }
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
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    cboDSHeThongYCHT.AddItem('-- chọn hệ thống --', 0);
                    cboChonHeThongSua.AddItem('-- chọn hệ thống --', 0);
                    for (var i = 0; i < objret.length; i++) {
                        cboDSHeThongYCHT.AddItem(objret[i].TENHETHONG, objret[i].ID);
                        cboChonHeThongSua.AddItem(objret[i].TENHETHONG, objret[i].ID);
                    }
                     cboDSHeThongYCHT.SetValue(ASPxHiddenField1.Get('ThongHTKTID'));
                    //cboDSHeThongYCHT.SetSelectedIndex(0);
                    //cboChonHeThongSua.SetSelectedIndex(0);
                },
                error: function () {
                }
            });
        }
        function napSoBuocTrongLuongXL() {
            cboSoBuocXuLy.ClearItems();
            cboSoBuocSua.ClearItems();
            cboSoBuocXuLy.AddItem('-- chọn số bước --', 0);
            cboSoBuocSua.AddItem('-- chọn số bước --', 0);
            for (var i = 0; i < 100; i++) {
                cboSoBuocXuLy.AddItem((i + 1) + ' bước', i + 1);
                cboSoBuocSua.AddItem((i + 1) + ' bước', i + 1);
            }
            cboSoBuocXuLy.SetSelectedIndex(0);
            cboSoBuocSua.SetSelectedIndex(0);
        }

        function themMoiLuongXuly() {
            popupThemMoiLuongXuLy.Show();
            napDanhSachHeThongYCHT();
            napSoBuocTrongLuongXL();
                      
        }
        function suaLuongXuLy() {
            if (cboChonHeThongSua.GetValue() == '0') {
                alert('Bạn phải chọn hệ thống');
                return;
            }
            if (cboSoBuocSua.GetValue() == '0') {
                alert('Bạn phải chọn số bước xử lý');
                return;
            }

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaMucLuongXuLy',
                data: "{ id_luonght: '" + ASPxHiddenField1.Get('id_luonght') + "',id_hethong: '" + cboChonHeThongSua.GetValue() + "',tenluong: '" + txtTenLuongSua.GetText() + "',trangthai: '" + cboTrangThaiSua.GetChecked() + "',mota: '" + txtMoTaSua.GetText() + "',sobuoc: '" + cboSoBuocSua.GetValue() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        popupSuaThongTinLuongXL.Hide();
                        grvDmLuongXuLy.PerformCallback();
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

        function themMoiLuongXL() {
            if (cboDSHeThongYCHT.GetValue() == '0') {
                alert('Bạn phải chọn hệ thống');
                return;
            }
            if (cboSoBuocXuLy.GetValue() == '0') {
                alert('Bạn phải chọn số bước xử lý');
                return;
            }


            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themMucLuongXuLy',
                data: "{ id_hethong: '" + cboDSHeThongYCHT.GetValue() + "',tenluong: '" + txtTenLuongXuLy.GetValue() + "',trangthai: '" + chkTrangThai.GetChecked() + "',mota:'" + txtMoTaLuongXL.GetValue() + "',sobuoc: '" + cboSoBuocXuLy.GetValue() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        popupThemMoiLuongXuLy.Hide();
                        grvDmLuongXuLy.PerformCallback();
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
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>&nbsp;Quay về</a></li>
            <li><a onclick="themMoiLuongXuly()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
        </ul>
    </div>
    <!-- end panel nav boot -->
    <div class="p8">
        <!-- begin panel boot -->
        <div class="panel panel-default">
            <div class="panel-heading"><span style="font-size: 12pt"></span></div>
            <div class="panel-body" style="border: none">
                <!-- begin body boot -->


                <script type="text/javascript">
                    $(function () {
                        grvDmLuongXuLy.PerformCallback();
                    });

                </script>

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
                                <dx:ASPxGridView ID="grvDmLuongXuLy" runat="server" ClientInstanceName="grvDmLuongXuLy"
                                    OnCustomCallback="grvDmLuongXuLy_CustomCallback" KeyFieldName="ID"
                                    OnCustomColumnDisplayText="grvDmLuongXuLy_CustomColumnDisplayText"
                                    OnDataBinding="grvDmLuongXuLy_DataBinding" OnHtmlDataCellPrepared="grvDmLuongXuLy_HtmlDataCellPrepared"
                                    OnPageIndexChanged="grvDmLuongXuLy_PageIndexChanged"
                                    Theme="Office2010Blue" AutoGenerateColumns="False"
                                    OnCustomButtonInitialize="grvDmLuongXuLy_CustomButtonInitialize" EnablePagingGestures="False">
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
            if(e.buttonID=='Sua')
            {
                suaMucLuongXuLy(grvDmLuongXuLy.GetRowKey(e.visibleIndex));
            }
            if(e.buttonID=='Xoa')
            {
                xoaMucLuongXuLy(grvDmLuongXuLy.GetRowKey(e.visibleIndex));	
            }	    
        }" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ID" Caption="Mã luồng"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ID_HETHONG_YCHT" Caption="Mã HT"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TENHETHONG" Caption="Tên HT"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TEN_LUONG" Caption="Tên luồng"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TRANGTHAI" Caption="Trạng thái"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="MOTA" Caption="Mô tả"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="SOBUOC" Caption="Số bước"></dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="Sua" Text="Sửa" Image-Url="../../HTHTKT/icons/edit_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                                                <dx:GridViewCommandColumnCustomButton ID="Xoa" Text="Xóa" Image-Url="../../HTHTKT/icons/delete_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <CellStyle HorizontalAlign="Left"></CellStyle>
                                        </dx:GridViewCommandColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>

                </div>




                <dx:ASPxPopupControl ID="popupThemMoiLuongXuLy" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới luồng xử lý" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" ClientInstanceName="popupThemMoiLuongXuLy" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Chọn hệ thống</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboDSHeThongYCHT" ClientInstanceName="cboDSHeThongYCHT" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Tên luồng</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenLuongXuLy" runat="server" Width="170px" ClientInstanceName="txtTenLuongXuLy">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaLuongXL" runat="server" Height="71px" Width="170px" ClientInstanceName="txtMoTaLuongXL">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Số bước</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSoBuocXuLy" ClientInstanceName="cboSoBuocXuLy" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThai" runat="server" CheckState="Unchecked" ClientInstanceName="chkTrangThai">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnThemMoi" runat="server" ClientInstanceName="btnThemMoi" Text="Thêm" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
themMoiLuongXL();	
}" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongThem" runat="server" ClientInstanceName="btnDongThem" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupThemMoiLuongXuLy.Hide();
}" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <br />
                <dx:ASPxPopupControl ID="popupSuaThongTinLuongXL" runat="server" AllowDragging="True" CloseAction="CloseButton" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" HeaderText="Sửa thông tin luồng xử lý" ClientInstanceName="popupSuaThongTinLuongXL" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Chọn hệ thống</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboChonHeThongSua" ClientInstanceName="cboChonHeThongSua" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Tên luồng</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenLuongSua" runat="server" Width="170px" ClientInstanceName="txtTenLuongSua">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaSua" runat="server" Height="71px" Width="170px" ClientInstanceName="txtMoTaSua">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Số bước</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSoBuocSua" ClientInstanceName="cboSoBuocSua" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="cboTrangThaiSua" runat="server" CheckState="Unchecked" ClientInstanceName="cboTrangThaiSua">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnCapNhat" runat="server" ClientInstanceName="btnCapNhat" Text="Cập nhật" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	suaLuongXuLy();
}" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" runat="server" ClientInstanceName="btnDongSua" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupSuaThongTinLuongXL.Hide();
}" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <dx:ASPxLoadingPanel ID="loadingdata2" runat="server" ClientInstanceName="loadingdata2" Text="Đang nạp&amp;hellip;">
                </dx:ASPxLoadingPanel>

                <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1">
                </dx:ASPxHiddenField>

            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
