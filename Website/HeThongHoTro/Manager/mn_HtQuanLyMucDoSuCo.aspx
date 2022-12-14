<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_HtQuanLyMucDoSuCo.aspx.cs" Inherits="Website.HeThongHoTro.Manager.HtQuanLyMucDoSuCo" %>


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

        function suaMucMucDoSuCo(val) {
            popupSuaThongTinMucDoSuCo.Show();
          
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucMucDoSuCo',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('id', objret[0].ID);
                    txtTenMucDoEdit.SetText(objret[0].TENMUCDO);
                    chkTrangThaiEdit.SetChecked(objret[0].TRANGTHAI);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function xoaMucMucDoSuCo(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaMucMucDoSuCo',
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
                            grvDmMucDoSuCo.PerformCallback();
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

        function themMoiMucDoSuCo() {
            popupThemMoiMucDoSuCo.Show();
                      
        }
        function suaMucDoSuCoHT() {

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/suaMucMucDoSuCo',
                data: "{ id: '" + ASPxHiddenField1.Get('id') + "',tenmucdo: '" + txtTenMucDoEdit.GetValue() + "',trangthai: '" + chkTrangThaiEdit.GetChecked() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Sửa thành công');
                        popupSuaThongTinMucDoSuCo.Hide();
                        grvDmMucDoSuCo.PerformCallback();
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

        function themMoiMucDoSuCoHT() {

            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/themMucMucDoSuCo',
                data: "{ tenmucdo: '" + txtTenMucDoAdd.GetValue() + "',trangthai: '" + chkTrangThaiAdd.GetChecked() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        alert('Thêm thành công');
                        popupThemMoiMucDoSuCo.Hide();
                        grvDmMucDoSuCo.PerformCallback();
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
            <li><a onclick="themMoiMucDoSuCo()" class="btn btn-primary"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
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
                        grvDmMucDoSuCo.PerformCallback();
                    });

                </script>

                <div class="container" style="margin-left: 0px">
                    <div class="row">
                        <div class="col-md-4"><strong>Danh sách các mức độ sự cố:</strong></div>
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
                                <dx:ASPxGridView ID="grvDmMucDoSuCo" runat="server" ClientInstanceName="grvDmMucDoSuCo"
                                    OnCustomCallback="grvDmMucDoSuCo_CustomCallback" KeyFieldName="ID"
                                    OnCustomColumnDisplayText="grvDmMucDoSuCo_CustomColumnDisplayText"
                                    OnDataBinding="grvDmMucDoSuCo_DataBinding" OnHtmlDataCellPrepared="grvDmMucDoSuCo_HtmlDataCellPrepared"
                                    OnPageIndexChanged="grvDmMucDoSuCo_PageIndexChanged"
                                    Theme="Office2010Blue" AutoGenerateColumns="False"
                                    OnCustomButtonInitialize="grvDmMucDoSuCo_CustomButtonInitialize">
                                    <ClientSideEvents CustomButtonClick="function(s, e) {
            if(e.buttonID=='Sua')
            {
                suaMucMucDoSuCo(grvDmMucDoSuCo.GetRowKey(e.visibleIndex));
            }
            if(e.buttonID=='Xoa')
            {
                xoaMucMucDoSuCo(grvDmMucDoSuCo.GetRowKey(e.visibleIndex));	
            }	    
        }" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ID" Caption="#"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TENMUCDO" Caption="Tên mức độ"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TRANGTHAI" Caption="Trạng thái"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="NGAYTAO" Caption="Ngày tạo"></dx:GridViewDataDateColumn>
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




                <dx:ASPxPopupControl ID="popupThemMoiMucDoSuCo" runat="server" AllowDragging="True" CloseAction="CloseButton" HeaderText="Thêm mới nhóm quyền" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" ClientInstanceName="popupThemMoiMucDoSuCo" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên mức độ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenMucDoAdd" runat="server" Width="170px" ClientInstanceName="txtTenMucDoAdd">
                                        </dx:ASPxTextBox>
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
themMoiMucDoSuCoHT();	
}" />
                                            <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongThem" runat="server" ClientInstanceName="btnDongThem" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupThemMoiMucDoSuCo.Hide();
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
                <dx:ASPxPopupControl ID="popupSuaThongTinMucDoSuCo" runat="server" AllowDragging="True" CloseAction="CloseButton" Height="300px" Modal="True" Theme="Office2010Blue" Width="400px" HeaderText="Sửa nhóm quyền" ClientInstanceName="popupSuaThongTinMucDoSuCo" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%;" class="tblKhaiBaoHT">
                                <tr>
                                    <td>Tên mức độ</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTenMucDoEdit" runat="server" Width="170px" ClientInstanceName="txtTenMucDoEdit">
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
                                        <dx:ASPxButton ID="btnCapNhat" runat="server" ClientInstanceName="btnCapNhat" Text="Cập nhật" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	suaMucDoSuCoHT();
}" />
                                            <Image Url="~/HTHTKT/icons/edit_16x16.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnDongSua" runat="server" ClientInstanceName="btnDongSua" EnableTheming="True" Text="Đóng" Theme="Office2010Blue" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	popupSuaThongTinMucDoSuCo.Hide();
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
