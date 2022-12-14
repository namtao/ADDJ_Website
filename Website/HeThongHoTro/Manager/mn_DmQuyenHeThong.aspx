<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_DmQuyenHeThong.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_DmQuyenHeThong" %>


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

        function suaMucNhomQuyen(val) {
            popupSuaThongTinNhomQuyen.Show();
          
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucNhomQuyenHT',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id', objret[0].Id);
                    txtTenNhomQuyenEdit.SetText(objret[0].Name);
                    txtMoTaNhomQuyenEdit.SetText(objret[0].Description);
                    chkTrangThaiEdit.SetChecked(objret[0].Status);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function xoaMucNhomQuyen(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaMucNhomQuyenHT',
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
                            grvDmQuyenHeThong.PerformCallback();
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

        function themMoiNhomQuyen() {
            popupThemMoiNhomQuyen.Show();
                      
        }
        function suaNhomQuyen() {

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaMucNhomQuyenHT',
                data: "{ id: '" + ASPxHiddenField1.Get('id') + "',tennhomquyen: '" + txtTenNhomQuyenEdit.GetValue() + "',mota: '" + txtMoTaNhomQuyenEdit.GetText() + "',trangthai: '" + chkTrangThaiEdit.GetChecked() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        popupSuaThongTinNhomQuyen.Hide();
                        grvDmQuyenHeThong.PerformCallback();
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

        function themMoiNhomQuyenHT() {

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themMucNhomQuyenHT',
                data: "{ tennhomquyen: '" + txtTenNhomQuyenAdd.GetValue() + "',mota: '" + txtMoTaNhomQuyenAdd.GetValue() + "',trangthai: '" + chkTrangThaiAdd.GetChecked() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        popupThemMoiNhomQuyen.Hide();
                        grvDmQuyenHeThong.PerformCallback();
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
            <li><a onclick="themMoiNhomQuyen()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
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
                        grvDmQuyenHeThong.PerformCallback();
                    });

                </script>

                <div class="container" style="margin-left: 0px">
                    <div class="row">
                        <div class="col-md-2"><strong>Chọn hệ thống:</strong></div>
                        <div class="col-md-2">
                             
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
                                <dx:ASPxGridView ID="grvDmQuyenHeThong" runat="server" ClientInstanceName="grvDmQuyenHeThong"
                                    OnCustomCallback="grvDmQuyenHeThong_CustomCallback" KeyFieldName="Id"
                                    OnCustomColumnDisplayText="grvDmQuyenHeThong_CustomColumnDisplayText"
                                    OnDataBinding="grvDmQuyenHeThong_DataBinding" OnHtmlDataCellPrepared="grvDmQuyenHeThong_HtmlDataCellPrepared"
                                    OnPageIndexChanged="grvDmQuyenHeThong_PageIndexChanged"
                                    Theme="Office2010Blue" AutoGenerateColumns="False"
                                    OnCustomButtonInitialize="grvDmQuyenHeThong_CustomButtonInitialize">
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
            if(e.buttonID=='Sua')
            {
                suaMucNhomQuyen(grvDmQuyenHeThong.GetRowKey(e.visibleIndex));
            }
            if(e.buttonID=='Xoa')
            {
                xoaMucNhomQuyen(grvDmQuyenHeThong.GetRowKey(e.visibleIndex));	
            }	    
        }" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="Id" Caption="Mã nhóm"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Name" Caption="Tên nhóm"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Description" Caption="Mô tả"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Status" Caption="Trạng thái"></dx:GridViewDataTextColumn>
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




                <dx:ASPxPopupControl ID="popupThemMoiNhomQuyen" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới nhóm quyền" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" ClientInstanceName="popupThemMoiNhomQuyen" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên nhóm</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenNhomQuyenAdd" runat="server" Width="170px" ClientInstanceName="txtTenNhomQuyenAdd">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaNhomQuyenAdd" runat="server" Height="71px" Width="170px" ClientInstanceName="txtMoTaNhomQuyenAdd">
                                        </dx:ASPxMemo>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Trạng thái</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkTrangThaiAdd" runat="server" CheckState="Unchecked" ClientInstanceName="chkTrangThaiAdd">
                                        </dx:ASPxCheckBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dx:ASPxButton ID="btnThemMoi" runat="server" ClientInstanceName="btnThemMoi" Text="Thêm" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
themMoiNhomQuyenHT();	
}" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongThem" runat="server" ClientInstanceName="btnDongThem" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupThemMoiNhomQuyen.Hide();
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
                <dx:ASPxPopupControl ID="popupSuaThongTinNhomQuyen" runat="server" AllowDragging="True" CloseAction="CloseButton" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" HeaderText="Sửa nhóm quyền" ClientInstanceName="popupSuaThongTinNhomQuyen" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên nhóm</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenNhomQuyenEdit" runat="server" Width="170px" ClientInstanceName="txtTenNhomQuyenEdit">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Mô tả</td>
                                    <td>
                                        <dx:ASPxMemo ID="txtMoTaNhomQuyenEdit" runat="server" Height="71px" Width="170px" ClientInstanceName="txtMoTaNhomQuyenEdit">
                                        </dx:ASPxMemo>
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
                                        <dx:ASPxButton ID="btnCapNhat" runat="server" ClientInstanceName="btnCapNhat" Text="Cập nhật" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	suaNhomQuyen();
}" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" runat="server" ClientInstanceName="btnDongSua" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupSuaThongTinNhomQuyen.Hide();
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
