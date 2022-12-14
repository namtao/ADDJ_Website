<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_DmHeThongYCHotro.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_DmHeThongYCHotro" %>

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

        .auto-style1 {
            height: 17px;
        }
    </style>
    <script type="text/javascript">
        function xoaMucHeThongYCHT(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaMucHeThongYCHT',
                    data: "{ id_hethong: " + val + " }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');

                            grvDanhMucHeThongYCHT.PerformCallback();
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
        function suaMucHeThongYCHT(val) {
            ASPxHiddenField1.Set('id_hethong', val);
             napSoBuocTrongLuongXL();
            popupDongHopThoaiSua.Show();
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucHeThongYCHT',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    cboTenHeThongSua.SetText(objret[0].TENHETHONG);
                    cboTenHeThongSua.SetValue(objret[0].ID_HETHONG);
                    txtMaHeThongSua.SetText(objret[0].MA_HETHONG);
                    txtMoTaHeThongSua.SetText(objret[0].MOTA);
                    cboSoBuocSua.SetValue(objret[0].MUCDO);
                    chkTrangThaiSua.SetChecked(objret[0].TRANGTHAI);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function themMucHeThongYCHT() {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themMucHeThongYCHT',
                data: "{ tenhethong: '" + txtTenHeThong.GetValue() + "',mahethong: '" + txtMaHeThong.GetText() + "',mota: '" + txtMoTaHeThong.GetValue() + "',mucdo: '" + cboSoBuocXuLy.GetValue() + "',trangthai: '" + cboTrangThai.GetChecked() + "',nguoitao: '" + ASPxHiddenField1.Get('username') + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        popupThemMoiHeThong.Hide();
                        grvDanhMucHeThongYCHT.PerformCallback();
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

        function themMoiHeThongYCHT() {
            napSoBuocTrongLuongXL();
            popupThemMoiHeThong.Show();
        }

        function suaThongTin() {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaMucHeThongYCHT',
                data: "{ id_hethong: '" + ASPxHiddenField1.Get('id_hethong') + "',tenhethong: '" + cboTenHeThongSua.GetValue() + "',mahethong: '" + txtMaHeThongSua.GetText() + "',mota: '" + txtMoTaHeThongSua.GetValue() + "',mucdo: '" + cboSoBuocSua.GetValue() + "',trangthai: " + chkTrangThaiSua.GetChecked() + ",nguoitao: '" + ASPxHiddenField1.Get('username') + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        popupDongHopThoaiSua.Hide();
                        grvDanhMucHeThongYCHT.PerformCallback();
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

        function napSoBuocTrongLuongXL() {
            cboSoBuocXuLy.ClearItems();
            cboSoBuocSua.ClearItems();
            cboSoBuocXuLy.AddItem('-- chọn mức độ --', 0);
            cboSoBuocSua.AddItem('-- chọn mức độ --', 0);
            for (var i = 0; i < 100; i++) {
                cboSoBuocXuLy.AddItem('mức ' + (i + 1), i + 1);
                cboSoBuocSua.AddItem('mức ' + (i + 1), i + 1);
            }
            cboSoBuocXuLy.SetSelectedIndex(0);
            cboSoBuocSua.SetSelectedIndex(0);
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
            <li><a onclick="themMoiHeThongYCHT()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
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
                        grvDanhMucHeThongYCHT.PerformCallback();
                    });

                </script>

                <dx:ASPxGridView ID="grvDanhMucHeThongYCHT" runat="server" Theme="Office2010Blue"
                    ClientInstanceName="grvDanhMucHeThongYCHT" KeyFieldName="ID"
                    OnCustomCallback="grvDanhMucHeThongYCHT_CustomCallback"
                    OnCustomColumnDisplayText="grvDanhMucHeThongYCHT_CustomColumnDisplayText"
                    OnDataBinding="grvDanhMucHeThongYCHT_DataBinding"
                    OnHtmlDataCellPrepared="grvDanhMucHeThongYCHT_HtmlDataCellPrepared"
                    OnPageIndexChanged="grvDanhMucHeThongYCHT_PageIndexChanged" AutoGenerateColumns="False" EnablePagingGestures="False">
                    <ClientSideEvents CustomButtonClick="function(s, e) {
                            if(e.buttonID=='Sua')
                            {
                            suaMucHeThongYCHT(grvDanhMucHeThongYCHT.GetRowKey(e.visibleIndex));	
                            }
                            if(e.buttonID=='Xoa')
                            {
                            xoaMucHeThongYCHT(grvDanhMucHeThongYCHT.GetRowKey(e.visibleIndex));	
                            }	     
                    }" />
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="ID" Caption="Mã HT"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TENHETHONG" Caption="Tên HT"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="MA_HETHONG" Caption="Mã HT"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="MOTA" Caption="Mô tả"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="MUCDO" Caption="Mức độ"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TRANGTHAI" Caption="Trạng thái"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="NGAYTAO" Caption="Ngày tạo"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="NGUOITAO" Caption="Người tạo"></dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn>
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="Sua" Text="Sửa" Image-Url="../../HTHTKT/icons/edit_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="Xoa" Text="Xóa" Image-Url="../../HTHTKT/icons/delete_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                    </Columns>
                </dx:ASPxGridView>

                <br />
                <dx:ASPxPopupControl ID="popupThemMoiHeThong" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới hệ thống" Height="250px" Modal="True" Theme="Office2010Blue" Width="500px" ClientInstanceName="popupThemMoiHeThong" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên hệ thống</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenHeThong" runat="server" ClientInstanceName="txtTenHeThong" Width="300px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mã hệ thống</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaHeThong" runat="server" ClientInstanceName="txtMaHeThong" Width="300px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaHeThong" runat="server" ClientInstanceName="txtMoTaHeThong" Height="71px" Width="300px">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                 <tr>
                                    <td>Mức độ</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSoBuocXuLy" ClientInstanceName="cboSoBuocXuLy" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
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
                                        <dx:ASPxButton ID="btnThemMoi" runat="server" Text="Thêm" Theme="Office2010Blue" ClientInstanceName="btnThemMoi" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            themMucHeThongYCHT();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupThemMoiHeThong.Hide();
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
                <dx:ASPxPopupControl ID="popupDongHopThoaiSua" runat="server" AllowDragging="True" ClientInstanceName="popupDongHopThoaiSua" CloseAction="CloseButton" HeaderText="Sửa thông tin hệ thống" Height="250px" Modal="True" Theme="Office2010Blue" Width="500px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td class="auto-style1">Tên hệ thống</td>
                                    <td class="auto-style1">
                                        <dx:ASPxTextBox ID="cboTenHeThongSua" runat="server" ClientInstanceName="cboTenHeThongSua" Width="300px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Mã hệ thống</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtMaHeThongSua" runat="server" Width="300px" ClientInstanceName="txtMaHeThongSua">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaHeThongSua" runat="server" Height="71px" Width="300px" ClientInstanceName="txtMoTaHeThongSua">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mức độ</td>
                                    <td>
                                        <dx:ASPxComboBox ID="cboSoBuocSua" ClientInstanceName="cboSoBuocSua" runat="server" Theme="Office2010Blue">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThaiSua" ClientInstanceName="chkTrangThaiSua" runat="server" CheckState="Unchecked">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnSua" ClientInstanceName="btnSua" runat="server" Text="Sửa" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                            suaThongTin();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" ClientInstanceName="btnDongSua" runat="server" Text="Đóng" AutoPostBack="False" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupDongHopThoaiSua.Hide();
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
                <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1">
                </dx:ASPxHiddenField>
                <dx:ASPxLoadingPanel ID="loadingdata2" runat="server" ClientInstanceName="loadingdata2" Text="Đang nạp&amp;hellip;" Theme="Office2010Blue">
                </dx:ASPxLoadingPanel>
                <br />
            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
</asp:Content>
