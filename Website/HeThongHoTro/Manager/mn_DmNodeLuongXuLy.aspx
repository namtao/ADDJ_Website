<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.master" AutoEventWireup="true" CodeBehind="mn_DmNodeLuongXuLy.aspx.cs" Inherits="Website.HeThongHoTro.Manager.mn_DmNodeLuongXuLy" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>


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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">
        // nạp danh sách các luồng hỗ trợ
        function DanhSachLuongHoTro(val) {
            debugger;
            //alert('ID he thong xu ly ' + val);
            ASPxHiddenField1.Set('id_hethong', val);
            cboDanhSachLuongYeuCauHoTro.PerformCallback(val);
        }

        function ChonDonVi(s, e) {
            debugger;
            var tendv = '';
            var key = treelist_donvi_cc.GetFocusedNodeKey();
            treelist_donvi_cc.GetNodeValues(key, "linhvuccon", function (value) {
                //LoaiHoTro.SetText(value);
                tenDonVi = value;
                //LoaiHoTro.SetKeyValue(key);
                //LoaiHoTro.HideDropDown();
                //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
            });
            // gán id loai hỗ trợ cho hidden
            //LoaiHTKTID.Set('LoaiHTKTID', key);
            //noiChuyenDen.PerformCallback();
        }

        // nạp danh sách các bước xử lý (là danh sách số thứ tự các bước)
        var postponedCallbackRequired = false;
        function DanhSachCacBuocXuLy(val) {
            debugger;
            //alert('idluonghotro ' + val);
            ASPxHiddenField1.Set('idluonghotro', val);
            ASPxHiddenField1.Set('hidden_value', val);
            ASPxHiddenField1.Set('hidden_value2', 1);
            ASPxHiddenField1.Set('res_value', 1);

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

        /////////////////////////////////////////////////////////////////////////////////////////
        /// wait callback to complete to run next step
        PendingCallbacks =  { };
        function DoCallback(sender, callback) {
            if(sender.InCallback()) {
                PendingCallbacks[sender.name] = callback;
                sender.EndCallback.RemoveHandler(DoEndCallback);
                sender.EndCallback.AddHandler(DoEndCallback);
            } else {
                callback();
            }
        }

        function DoEndCallback(s, e) {
            var pendingCallback = PendingCallbacks[s.name];
            if(pendingCallback) {
                pendingCallback();
                delete PendingCallbacks[s.name];
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////

        // Nạp danh sách các đơn vị trong bước xử lý này
        function NapThongTinDonViTrongBuocXuLyNay(val) {
            debugger;
     
            //alert('buoc xu ly thu ' + val);
            var id_hethong = ASPxHiddenField1.Get('id_hethong');
            var idluonghotro = ASPxHiddenField1.Get('idluonghotro');
            thongtinLuongXuLy(idluonghotro);
            ASPxHiddenField1.Set('idbuocxuly', val);
            var idbuocxuly = val;
             // DoCallback(grvNodeLuongXuLy, function () { grvNodeLuongXuLy.PerformCallback(); });
             // DoCallback(treelist_donvi_cc, function () {    treelist_donvi_cc.PerformCallback('refresh'); });

            grvNodeLuongXuLy.PerformCallback(id_hethong + '|' + idluonghotro + '|' + idbuocxuly);
            //treelist_donvi_cc.PerformCallback('clear');
            //treelist_donvi_cc.PerformCallback('refresh');
            //treelist_donvi_cc.SetFocusedNodeKey(null);
        }
        function loadCheckTree() {
            treelist_donvi_cc.PerformCallback('clear');
            treelist_donvi_cc.PerformCallback('refresh');
            treelist_donvi_cc.SetFocusedNodeKey(null);
        }
        function thongtinLuongXuLy(val)
        {
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
                    //ASPxHiddenField1.Set('id_luonght', objret[0].ID);

                    //cboChonHeThongSua.SetValue(objret[0].ID_HETHONG_YCHT);
                    //txtTenLuongSua.SetText(objret[0].TEN_LUONG);
                    //cboTrangThaiSua.SetChecked(objret[0].TRANGTHAI);
                    $('#motaLuongXuLy').text(objret[0].MOTA);
                    //cboSoBuocSua.SetValue(objret[0].SOBUOC);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }



        function xoaDonViTrongNodeXuLy(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaDonViTrongNode',
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
                            grvNodeLuongXuLy.PerformCallback(ASPxHiddenField1.Get('id_hethong') + '|' + ASPxHiddenField1.Get('idluonghotro') + '|' + ASPxHiddenField1.Get('idbuocxuly'));
                            treelist_donvi_cc.PerformCallback('clear');
                            treelist_donvi_cc.PerformCallback('refresh');
                            treelist_donvi_cc.SetFocusedNodeKey(null);
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

        function OnCheckedChanged(s, e) {
            label.SetText("Checked nodes:");
            setTimeout(function () { treelist_donvi_cc.GetSelectedNodeValues('ID', GetValues) }, 3000);
            //treelist_donvi_cc.GetSelectedNodeValues('ID', GetValues)
        }
        function GetValues(values) {
            var lstIDDonVi = '';
            for (var i = 0; i < values.length; i++) {
                lstIDDonVi = lstIDDonVi + " " + values[i];
                label.SetText(label.GetText() + " " + values[i]);
            }
            ASPxHiddenField1.Set('lstIDDonVi', lstIDDonVi);
        }

        function CapNhatDonViVaoDanhSach() {
            cbpanellabel.PerformCallback();
        }
        function capnhatdonvivaodanhsachEndcallback() {
            //alert(ASPxHiddenField1.Get('lstIDDonVi'));
            if (confirm('Bạn có chắc chắn muốn thêm?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/themDonViTrongNode',
                    data: "{  id_hethong: '" + ASPxHiddenField1.Get('id_hethong') + "',idluonghotro: '" + ASPxHiddenField1.Get('idluonghotro') + "',idbuocxuly: '" + ASPxHiddenField1.Get('idbuocxuly') + "',arr_iddonvi: '" + label.GetText() + "' }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Thêm đơn vị thành công');
                            grvNodeLuongXuLy.PerformCallback(ASPxHiddenField1.Get('id_hethong') + '|' + ASPxHiddenField1.Get('idluonghotro') + '|' + ASPxHiddenField1.Get('idbuocxuly'));
                            treelist_donvi_cc.PerformCallback('clear');
                            treelist_donvi_cc.PerformCallback('refresh');
                            treelist_donvi_cc.SetFocusedNodeKey(null);
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
        }
        function XemLuongTongQuan() {
            popupXemLuongTongQuan.Show();
            //rbl.Items.Count
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanly.asmx/loadLuongTongQuan',
                data: "{ id_hethong: " + ASPxHiddenField1.Get('id_hethong') + ",idluonghotro: " + ASPxHiddenField1.Get('idluonghotro') + ",tongsobuoc: " + rdoDanhSachCacBuocXuLy.items.length + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    $("#timeLine thead").html(objret.Header);
                    $("#timeLine tbody").html(objret.NoiDungMap);
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                    alert('có lỗi xảy ra khi lấy thông tin, vui lòng thử lại sau!');
                }
            });
        }

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
            //alert(lstID);
            if (lstID.length == 0)
            {
                alert('Bạn phải chọn ít nhất 1 đơn vị để xóa');
                return;
            }

            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/HeThongHoTro/Services/ws_quanLy.asmx/xoaNhieuDonViTrongNode',
                    data: "{ arr_id:'" + lstID + "' }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');
                            grvNodeLuongXuLy.PerformCallback(ASPxHiddenField1.Get('id_hethong') + '|' + ASPxHiddenField1.Get('idluonghotro') + '|' + ASPxHiddenField1.Get('idbuocxuly'));
                            treelist_donvi_cc.PerformCallback('clear');
                            treelist_donvi_cc.PerformCallback('refresh');
                            treelist_donvi_cc.SetFocusedNodeKey(null);
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

        function GetNamesClick(s, e) {
            cbpanellabel.PerformCallback();
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">

    <!-- begin panel nav boot -->
    <div class="nav_btn_bootstrap">
        <ul>
            <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>Quay về</a></li>
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
                        grvNodeLuongXuLy.PerformCallback();
                    });

                </script>

                <table style="width: 600px;" id="tbl_nodeLuongXuLy">
                    <tr>
                        <td><strong>Chọn hệ thống cần yêu cầu hỗ trợ:</strong></td>
                        <td>
                            <dx:ASPxComboBox ID="cboDanhSachHeThongCanYeuCauHoTro" ClientInstanceName="cboDanhSachHeThongCanYeuCauHoTro" runat="server" EnableTheming="True" Theme="Office2010Blue">
                                <ClientSideEvents ValueChanged="function(s, e) {
	                                DanhSachLuongHoTro(s.GetValue());
                                }" />
                            </dx:ASPxComboBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td rowspan="6">
                         
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><strong>Chọn luồng hỗ trợ:</strong>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cboDanhSachLuongYeuCauHoTro" ClientInstanceName="cboDanhSachLuongYeuCauHoTro" runat="server" Theme="Office2010Blue" OnCallback="cboDanhSachLuongYeuCauHoTro_Callback1" Width="380px">
                                <ClientSideEvents ValueChanged="function(s, e) {
	                                DanhSachCacBuocXuLy(s.GetValue());
                                }" />
                            </dx:ASPxComboBox>

                        </td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><strong>Chọn bước xử lý trong luồng:</strong></td>
                        <td>


                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="CallbackPanel" runat="server" Width="100%">
                                <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                        <dx:ASPxRadioButtonList CssClass="radioWithProperWrap" ID="rdoDanhSachCacBuocXuLy" runat="server"
                                            ClientInstanceName="rdoDanhSachCacBuocXuLy" RepeatColumns="5" Theme="Office2010Blue" Font-Bold="True" RepeatDirection="Horizontal">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                    var index = s.GetSelectedIndex();
                                    var item = s.GetItem(index);
                                    ASPxHiddenField1.Set('hidden_value2', 0);
	                                NapThongTinDonViTrongBuocXuLyNay(index+1);// item.text (nếu lấy giá trị label)
                                    }"
                                                Init="function(s, e) {
	                                    var index = s.GetSelectedIndex();
                                    var item = s.GetItem(index);
                                    ASPxHiddenField1.Set('hidden_value2', 0);
	                                NapThongTinDonViTrongBuocXuLyNay(index+1);// item.text (nếu lấy giá trị label)

                                        }" />
                                        </dx:ASPxRadioButtonList>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>

                        </td>
                        <td>


                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><strong>Chọn đơn vị cho vào bước xử lý:</strong></td>
                        <td><strong>Danh sách các đơn vị trong bước xử lý</strong></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <%-- <dx:ASPxDropDownEdit ID="LoaiHoTro" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTro" AnimationType="None" Width="350px">
                                                <DropDownWindowTemplate>
                                                    <div>--%>
                            <dx:ASPxTreeList ID="treelist_donvi_cc" runat="server" Width="350px" ClientInstanceName="treelist_donvi_cc"
                                Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="ID"
                                OnVirtualModeCreateChildren="treelist_donvi_cc_VirtualModeCreateChildren"
                                OnVirtualModeNodeCreating="treelist_donvi_cc_VirtualModeNodeCreating"
                                OnCustomCallback="treelist_donvi_cc_OnCustomCallback" AutoGenerateColumns="False" Height="324px"
                                OnVirtualModeNodeCreated="treelist_donvi_cc_VirtualModeNodeCreated"
                                OnHtmlDataCellPrepared="treelist_donvi_cc_HtmlDataCellPrepared"
                                OnHtmlRowPrepared="treelist_donvi_cc_HtmlRowPrepared">
                                <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" ScrollableHeight="300" />
                                <SettingsSelection Enabled="True" AllowSelectAll="true" Recursive="true"  />
                                <ClientSideEvents NodeExpanding="function(s, e) {
	                                                          ASPxHiddenField1.Set('hidden_value2', 0);
                                                        }" />
                                <Columns>
                                    <dx:TreeListTextColumn FieldName="TENDONVI" Caption="Tên đơn vị" Width="350px">
                                    </dx:TreeListTextColumn>

                                    <dx:TreeListTextColumn FieldName="ID" Caption="ID" Width="50px">
                                    </dx:TreeListTextColumn>
                                </Columns>

                            </dx:ASPxTreeList>
                            <%--    </div>
                                                </DropDownWindowTemplate>
                                            </dx:ASPxDropDownEdit>
                            --%>
                        </td>
                        <td style="vertical-align: top;">

                            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingdata2" runat="server" Text="Đang nạp&amp;hellip;">
                            </dx:ASPxLoadingPanel>

                            <dx:ASPxGridView ID="grvNodeLuongXuLy" runat="server" ClientInstanceName="grvNodeLuongXuLy"
                                OnCustomCallback="grvNodeLuongXuLy_CustomCallback"
                                OnCustomColumnDisplayText="grvNodeLuongXuLy_CustomColumnDisplayText"
                                OnDataBinding="grvNodeLuongXuLy_DataBinding"
                                OnHtmlDataCellPrepared="grvNodeLuongXuLy_HtmlDataCellPrepared" Theme="Office2010Blue"
                                OnPageIndexChanged="grvNodeLuongXuLy_PageIndexChanged" EnablePagingGestures="False"
                                AutoGenerateColumns="False" KeyFieldName="ID" OnCustomJSProperties="grvNodeLuongXuLy_CustomJSProperties">
                                <ClientSideEvents CustomButtonClick="function(s, e) {
            
                                                xoaDonViTrongNodeXuLy(grvNodeLuongXuLy.GetRowKey(e.visibleIndex));	            
                                    }"
                                    BeginCallback="function(s, e) {
	                                    loadingdata2.Show();
                                    }"
                                    EndCallback="function(s, e) {
	                                    loadingdata2.Hide();
                                        loadCheckTree();
                                    }" />
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="0" Caption="Mã">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MADONVI" VisibleIndex="1" Caption="Mã đơn vị">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="TENDONVI" VisibleIndex="2" Caption="Tên đơn vị">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn VisibleIndex="3">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="Xoa" Text="Xóa">
                                                <Image Url="../../HTHTKT/icons/delete_16x16.gif"></Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True"
                                        ShowClearFilterButton="true"
                                        VisibleIndex="4" SelectAllCheckboxMode="Page" />
                                </Columns>
                            </dx:ASPxGridView>
                        </td>
                        <td style="vertical-align: top;">
                            <div class="alert alert-warning" style="width: 200px">
                                <strong>Mô tả luồng:</strong> <span id="motaLuongXuLy"></span>.
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Get Names" AutoPostBack="false" Visible="False">
                                <ClientSideEvents Click="GetNamesClick" />
                            </dx:ASPxButton>
                            <dx:ASPxCallbackPanel runat="server" ID="cbpanellabel" Width="200px" ClientInstanceName="cbpanellabel"
                                OnCallback="cbpanellabel_Callback">
                                <ClientSideEvents EndCallback="function(s, e) {
	                                capnhatdonvivaodanhsachEndcallback();
                                }" />
                                <PanelCollection>
                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="label" ClientIDMode="AutoID" Text="Names: " ForeColor="White">
                                        </dx:ASPxLabel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnThemDonViVao" ClientInstanceName="btnThemDonViVao" runat="server" Text="Thêm vào" Theme="Office2010Blue" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
                                      ASPxHiddenField1.Set('hidden_value2', 1);
	                                        CapNhatDonViVaoDanhSach();
                                        }" />
                                <Image Url="~/HTHTKT/icons/add_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnXoaDonViDaChon" runat="server" AutoPostBack="False" ClientInstanceName="btnXoaDonViDaChon" EnableTheming="True" Text="Xóa" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) {
	                                  grvNodeLuongXuLy.GetSelectedFieldValues('ID', OnGetSelectedFieldValues);
                                }" />
                                <Image Url="~/HTHTKT/icons/delete_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnXemLuongTongQuan" runat="server" Text="Xem luồng tổng quan" Theme="Office2010Blue" ClientInstanceName="btnXemLuongTongQuan" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
	                                XemLuongTongQuan();
                                }" />
                                <Image Url="~/HTHTKT/icons/view_16x16.gif">
                                </Image>
                            </dx:ASPxButton>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>


                <br />
                <dx:ASPxPopupControl ID="popupXemLuongTongQuan" runat="server" HeaderText="Luồng tổng quan"
                    Height="300px" Modal="True" Theme="Office2010Blue" Width="800px"
                    ClientInstanceName="popupXemLuongTongQuan" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True" AllowResize="True" CloseAction="CloseButton" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table style="width: 100%; height: 300px" class="tblKhaiBaoHT">
                                <tr>
                                    <td colspan="2">
                                        <div class="table-responsive" style="height: 400px">
                                            <table id="timeLine" class="table table-hover table-striped">
                                                <thead></thead>
                                                <tbody style="height: 300px !important"></tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>

                                </tr>
                                <tr>
                                    <td></td>

                                    <td align="right">
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                        popupXemLuongTongQuan.Hide();
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


            </div>
            <!-- end body boot -->
        </div>
    </div>
    <!-- end panel boot -->
    <dx:ASPxHiddenField ID="ASPxHiddenField1" ClientInstanceName="ASPxHiddenField1" runat="server">
    </dx:ASPxHiddenField>
</asp:Content>
