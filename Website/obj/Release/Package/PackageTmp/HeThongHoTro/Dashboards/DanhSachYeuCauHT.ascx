<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachYeuCauHT.ascx.cs" Inherits="Website.HeThongHoTro.Dashboards.DanhSachYeuCauHT" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>


<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Danh sách các yêu cầu hỗ trợ của tôi"
    ShowCollapseButton="true" Theme="Office2010Blue" Width="100%" Height="220px">
    <PanelCollection>
        <dx:PanelContent runat="server">
            <script>
                function ChonDonViYC(s, e) {
                    var tendv = '';
                    var key = treelist_donvi_ccYC.GetFocusedNodeKey();
                    treelist_donvi_ccYC.GetNodeValues(key, "linhvuc", function (value) {
                        LoaiHoTroYC.SetText(value);
                        tenDonVi = value;
                        LoaiHoTroYC.SetKeyValue(key);
                        LoaiHoTroYC.HideDropDown();
                        //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
                    });
                }
                var postponedCallbackRequiredYC = false;
                //Chọn hệ thống hỗ trợ kỹ thuật
                function OnListBoxIndexChangedYC(s, e) {
                    debugger;
                    if (s.GetValue() == '0') {
                        LoaiHoTroYC.SetText('');
                        LoaiHoTroYC.SetKeyValue('0');
                    }


                    HiddenFieldYC.Set('hidden_value', s.GetValue());
                    HiddenFieldYC.Set('hidden_value2', 1);
                    HiddenFieldYC.Set('res_value', 1);
                    //treelist_donvi_cc.PerformCallback('refresh');
                    if (CallbackPanelYC.InCallback())
                        postponedCallbackRequiredYC = true;
                    else
                        CallbackPanelYC.PerformCallback();

                }
                function OnEndCallbackYC(s, e) {
                    if (postponedCallbackRequiredYC) {
                        CallbackPanelYC.PerformCallback();
                        postponedCallbackRequiredYC = false;
                    }
                }



                $(function () {
                    //loadMucDoYeuCauHeThongHT();
                    DsYeuCauHoTro.PerformCallback();
                });

                function napthongtinHeThongYC() {
                    if (LoaiHoTroYC.GetText() == '')
                        LoaiHoTroYC.SetKeyValue('');
                    //DsYeuCauHoTro.PerformCallback(cboChonHeThongYCHTHT.GetValue() + '|' + cboMucDoYeuCauHT.GetValue() + '|' + LoaiHoTroYC.GetKeyValue() + '|' + txtSoDienThoaiHT.GetText());
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
                            if (val2 == 1) {
                                aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                                //loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(val);
                            } else if (val2 == 2) {
                                aspxPopupTimelineXuLyHoTroNodeCuoi.ShowAtPos(100, 100);
                            }

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
                            lblSoDienThoaiYC1.SetText(objret.SODIENTHOAI);
                            lblMucDoYeuCauYC.SetText(objret.TENMUCDO);
                            lblMucDoYeuCauYC1.SetText(objret.TENMUCDO);

                            lblTenLuongHoTroHTYC.SetText(objret.TEN_LUONG);
                            lblTenLuongHoTroHTYC1.SetText(objret.TEN_LUONG);
                            lblMoTaLuongHoTroHTYC.SetText(objret.MOTA);
                            lblMoTaLuongHoTroHTYC1.SetText(objret.MOTA);
                            lblTenHeThongHoTroKyThuatHTYC.SetText(objret.TENHETHONG);
                            lblTenHeThongHoTroKyThuatHTYC1.SetText(objret.TENHETHONG);
                            lblNoiDungYeuCauHoTroGocHTYC.SetText(objret.NOIDUNG_YEUCAU);
                            lblNoiDungYeuCauHoTroGocHTYC1.SetText(objret.NOIDUNG_YEUCAU);
                            //lblLinhVucChungHTYC.SetText(objret.LINHVUCCHUNG);
                            //lblLinhVucChungHTYC1.SetText(objret.LINHVUCCHUNG);
                            lblLinhVucConHTYC.SetText(objret.LINHVUC);
                            lblLinhVucConHTYC1.SetText(objret.LINHVUC);

                            //loadTimelineHoTroXuLy(objret.ID_YEU_CAU_HOTRO, 1);
                        },
                        error: function () {
                        }
                    });
                }
                function checkXuLyNodeGiuaHayNodeCuoi(val) {
                    //debugger;
                    //loadThongTinYeuCauHoTro(val);
                    loadToanBoThongTinYeuCauHoTroTheoID(val);
                    loadTimelineHoTro(val, 2);
                    return;

                    var nodegiua = 0;
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/checkLuongXuLyHT',
                        data: "{ id_yeucau_xuly_hotro: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            if (jsonData.d == '1' || jsonData.d == '2') // nếu chuyển tiếp
                            {
                                loadThongTinYeuCauHoTro(val);
                                loadToanBoThongTinYeuCauHoTroTheoID(val);
                                loadTimelineHoTro(val, 1);
                                //aspxPopupTimelineXuLyHoTro.ShowAtPos(100, 100);
                                //loadDonViChuyenDenTheoIdXuLyHoTro(val);
                            }
                            else if (jsonData.d == '3')  // nếu xử lý luôn
                            {
                                loadThongTinYeuCauHoTro(val);
                                loadToanBoThongTinYeuCauHoTroTheoID(val);
                                loadTimelineHoTro(val, 2);
                                //aspxPopupTimelineXuLyHoTroNodeCuoi.ShowAtPos(100, 100);
                            }
                            else // nếu chỉ xem
                            {
                                // hide
                                btnXuLyHoTroLuon.SetVisible(false);
                                popupThongTinXuLyHT.ShowAtPos(100, 100);
                            }
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi lấy thông tin về luồng xử lý,vui lòng thử lại sau');
                            return;
                        }
                    });
                }


                function loadDonViChuyenDenTiepTheoByIDYeuCauHoTro(val) {
                    //debugger;
                    cbodonvichuyenden.ClearItems();
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/loadDonViChuyenDenTiepTheoByIDYeuCauHoTro',
                        data: "{ id_yeucau_hotro: '" + val + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            for (var i = 0; i < objret.length; i++) {
                                cbodonvichuyenden.AddItem(objret[i].TENDONVI, objret[i].ID_DONVI);
                            }
                        },
                        error: function () {
                        }
                    });
                }

                function chuyenHoTroSangDonViTiep() {
                    //hiddenYCHT.Get('id_yeucauhotro');
                    //debugger;

                    if (cbodonvichuyenden.GetValue() == 0) {
                        alert('Bạn phải chọn đơn vị chuyển đến!');
                        return;
                    }

                    var v1 = hiddenYCHT.Get('id_yeucauhotro');
                    var v2 = noidungXuLy.GetValue();
                    var v3 = cbodonvichuyenden.GetSelectedItem().value;
                    var v4 = hiddenYCHT.Get('nguoixuly');
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/chuyenTiepHoTroXuLy',
                        data: "{ id_yeucauhotro: " + hiddenYCHT.Get('id_yeucauhotro') + " , noidungxuly: '" + noidungXuLy.GetValue() + "', donvichuyenden:'" + cbodonvichuyenden.GetSelectedItem().value + "', nguoixuly:'" + hiddenYCHT.Get('nguoixuly') + "' }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            alert('Đã tạo chuyển xử lý thành công!.');
                            aspxPopupTimelineXuLyHoTro.Hide();
                        },
                        error: function () {
                            alert('Có lỗi xảy ra khi chuyển thông tin luồng hỗ trợ, vui lòng thử lại sau!');
                        }
                    });
                }


                function xemThongTinChiTietDaPhuongTien(val) {
                    //alert(val);
                    //htmlXemNoiDungXLChiTiet.SetHtml('<img src="/HTHTKT/UploadFiles/Images/2017-05-09_10-38-27(4).png" alt=""/>');
                    $.ajax({
                        type: 'POST',
                        url: '/HeThongHoTro/Services/ws_xemThongTin.asmx/thongTinChiTietXuLyDaPhuongTien',
                        data: "{ id: " + val + " }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (jsonData) {
                            var objret = JSON.parse(jsonData.d);
                            htmlXemNoiDungXLChiTiet.SetHtml(objret[0].NOIDUNGXLCHITIET);
                        },
                        error: function () {
                        }
                    });
                    popupXemNoiDungDaPhuongTien.Show();
                }

                //function loadMucDoYeuCauHeThongHT() {
                //    debugger;
                //    cboMucDoYeuCauHT.ClearItems();
                //    $.ajax({
                //        type: 'POST',
                //        url: '/HeThongHoTro/Services/ws_thamSo.asmx/thongtinMucDoSuCo',
                //        data: "{ id: '" + 0 + "' }",
                //        contentType: 'application/json; charset=utf-8',
                //        dataType: 'json',
                //        success: function (jsonData) {
                //            var objret = JSON.parse(jsonData.d);
                //            cboMucDoYeuCauHT.AddItem('---Chọn mức độ---', '0');
                //            for (var i = 0; i < objret.length; i++) {
                //                cboMucDoYeuCauHT.AddItem(objret[i].TENMUCDO, objret[i].ID);
                //            }
                //            cboMucDoYeuCauHT.SetValue("0");
                //        },
                //        error: function () {
                //        }
                //    });
                //}
            </script>

            <asp:UpdatePanel ID="updatePanelDSYCHT" runat="server">
                <ContentTemplate>
                    <div class="divCommand" style="padding-bottom: 10px">
                        <table style="width: 100%;">
                            <tr>
                                <td>Chọn hệ thống:</td>
                                <td>
                                    <dx:ASPxComboBox ID="cboChonHeThongYCHTHT" runat="server" ValueType="System.String"
                                        ClientInstanceName="cboChonHeThongYCHTHT" Theme="Office2010Blue">
                                        <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChangedYC" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td>Mức độ yêu cầu:</td>
                                <td>
                                    <dx:ASPxComboBox ID="cboMucDoYeuCauHT" runat="server" Theme="Office2010Blue" ClientInstanceName="cboMucDoYeuCauHT">
                                    </dx:ASPxComboBox>
                                </td>
                                <td>&nbsp;</td>
                                <td style="text-align: right;"></td>
                            </tr>
                            <tr>
                                <td>Số điện thoại:</td>
                                <td>
                                    <dx:ASPxTextBox ID="txtSoDienThoaiHT" runat="server" Theme="Office2010Blue" Width="170px" ClientInstanceName="txtSoDienThoaiHT">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>Lĩnh vực:</td>
                                <td>
                                    <%--        <dx:ASPxDropDownEdit ID="dropLinhVucHT" runat="server" Theme="Office2010Blue" ClientInstanceName="dropLinhVucHT">
                            </dx:ASPxDropDownEdit>--%>
                                    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px"
                                        ClientInstanceName="CallbackPanelYC">
                                        <ClientSideEvents EndCallback="OnEndCallbackYC"></ClientSideEvents>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <dx:ASPxDropDownEdit ID="LoaiHoTroYC" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTroYC" AnimationType="None" Width="100%">
                                                    <DropDownWindowTemplate>
                                                        <div>
                                                            <dx:ASPxTreeList ID="treelist_donvi_ccYC" runat="server" Width="350px" ClientInstanceName="treelist_donvi_ccYC"
                                                                Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                                OnVirtualModeCreateChildren="treelist_donvi_cc_VirtualModeCreateChildren"
                                                                OnVirtualModeNodeCreating="treelist_donvi_cc_VirtualModeNodeCreating"
                                                                OnCustomCallback="treelist_donvi_cc_OnCustomCallback" AutoGenerateColumns="False">
                                                                <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                                <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                                <ClientSideEvents FocusedNodeChanged="ChonDonViYC" NodeExpanding="function(s, e) {
	                                                          HiddenFieldYC.Set('hidden_value2', 0);
                                                        }" />
                                                                <Columns>
                                                                    <dx:TreeListTextColumn FieldName="linhvuc" Caption="Lĩnh vực" Width="350px">
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
                                <td>
                                    <dx:ASPxButton ID="btnTimKiemYC" runat="server" AutoPostBack="False"
                                        ClientInstanceName="btnTimKiemYC" EnableTheming="True" Text="Tìm kiếm" Theme="Office2010Blue"
                                        OnClick="btnTimKiemYC_Click">
                                        <ClientSideEvents Click="function(s, e) {
	                                        napthongtinHeThongYC();
                                        }" />
                                        <Image Url="~/HTHTKT/icons/search_16x16.gif">
                                        </Image>
                                    </dx:ASPxButton>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                    <dx:ASPxHiddenField ID="hiddenYCHT" runat="server" ClientInstanceName="hiddenYCHT">
                    </dx:ASPxHiddenField>


                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                        KeyFieldName="ID"
                        ClientInstanceName="DsYeuCauHoTro"
                        OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize"
                        OnCustomCallback="ASPxGridView1_CustomCallback"
                        OnDataBinding="ASPxGridView1_DataBinding"
                        OnPageIndexChanged="ASPxGridView1_PageIndexChanged" Theme="Office2010Blue"
                        OnCustomColumnDisplayText="ASPxGridView1_CustomColumnDisplayText" OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared" EnablePagingGestures="False">
                        <ClientSideEvents CustomButtonClick="function(s, e) {
                                       hiddenYCHT.Set('id_yeucauhotro', DsYeuCauHoTro.GetRowKey(e.visibleIndex));
                                       checkXuLyNodeGiuaHayNodeCuoi(DsYeuCauHoTro.GetRowKey(e.visibleIndex));
                                  }"
                            BeginCallback="function(s, e) {
	                             loadingdata.Show();
                            }"
                            EndCallback="function(s, e) {
	                             loadingdata.Hide();
                            }" />
                        <SettingsPager Visible="False">
                        </SettingsPager>
                        <SettingsResizing ColumnResizeMode="Control" />
                        <SettingsLoadingPanel Mode="Disabled" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ShowInCustomizationForm="True" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Số ĐT" CellStyle-Font-Bold="true" FieldName="SODIENTHOAI" ShowInCustomizationForm="True" VisibleIndex="1">
                                <CellStyle Font-Bold="True"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Mã y/c" FieldName="MA_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Mức độ y/c" CellStyle-Font-Bold="true" FieldName="TENMUCDO" ShowInCustomizationForm="True" VisibleIndex="2">
                                <CellStyle Font-Bold="True"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Nội dung y/c" FieldName="NOIDUNG_YEUCAU" ShowInCustomizationForm="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Nội dung xl" FieldName="NOIDUNG_XL_DONG_HOTRO" ShowInCustomizationForm="True" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Tên luồng" FieldName="TEN_LUONG" ShowInCustomizationForm="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Mô tả" FieldName="MOTA" ShowInCustomizationForm="True" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Số bước" FieldName="SOBUOC" ShowInCustomizationForm="True" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Tên hệ thống" FieldName="TENHETHONG" ShowInCustomizationForm="True" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Lĩnh vực" FieldName="LINHVUC" ShowInCustomizationForm="True" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Trang thái xử lý" FieldName="TRANGTHAI" ShowInCustomizationForm="True" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Người tạo" FieldName="NGUOITAO" ShowInCustomizationForm="True" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="12">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="Xem" Text="Xem">
                                        <Image Url="../../HTHTKT/icons/view_16x16.gif"></Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <dx:ASPxPanel ID="panelPaging" runat="server" ClientInstanceName="panelPaging">
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                <div style="width: 100%; height: 30px; border: solid 0px #fff; border-top: none; padding: 5px;">
                                    <div style="float: left;">
                                        <dx:ASPxPager ID="ASPxPager1" ItemCount="3" ItemsPerPage="1" runat="server" NumericButtonCount="5"
                                            CurrentPageNumberFormat="{0}" OnPageIndexChanged="ASPxPager1_PageIndexChanged" Theme="Office2010Blue">
                                            <LastPageButton Visible="True">
                                            </LastPageButton>
                                            <AllButton Text="Tất cả">
                                            </AllButton>
                                            <FirstPageButton Visible="True">
                                            </FirstPageButton>
                                            <Summary Position="Inside" Text="Trang {0} của {1} " />
                                            <CurrentPageNumberStyle BackColor="#FFFF99" ForeColor="Red">
                                                <Paddings PaddingLeft="5" PaddingRight="5" PaddingTop="5" PaddingBottom="5" />
                                                <border bordercolor="#CC0000" borderstyle="Solid" borderwidth="1px" />
                                            </CurrentPageNumberStyle>
                                        </dx:ASPxPager>
                                    </div>
                                    <div style="float: right">
                                        <dx:ASPxComboBox ID="cboPageSize" Width="50px" ClientInstanceName="cboPageSize" runat="server" Theme="Office2010Blue"
                                            OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged" AutoPostBack="true">
                                            <Items>
                                                <dx:ListEditItem Text="2" Value="2" />
                                                <dx:ListEditItem Text="10" Value="10" Selected="true" />
                                                <dx:ListEditItem Text="20" Value="20" />
                                                <dx:ListEditItem Text="30" Value="30" />
                                                <dx:ListEditItem Text="50" Value="50" />
                                                <dx:ListEditItem Text="100" Value="100" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                    <br />
                    <asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="updatePanelDSYCHT" DisplayAfter="0" DynamicLayout="false">
                        <ProgressTemplate>
                            <img alt="Đang xử lý..." src="../../HTHTKT/icons/progress.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="loadingdata" runat="server" Modal="True" Theme="Office2010Blue" Text="Đang tải&amp;hellip;">
            </dx:ASPxLoadingPanel>
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
            <dx:ASPxPopupControl ID="aspxPopupTimelineXuLyHoTroNodeCuoi" runat="server" CloseAction="CloseButton"
                HeaderText="Thông tin xử lý hỗ trợ" Height="103px" Theme="Office2010Silver" Width="800px"
                ClientInstanceName="aspxPopupTimelineXuLyHoTroNodeCuoi" AllowDragging="True" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True"
                ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table style="width: 100%;" class="tblKhaiBaoHT">
                            <tr>
                                <td class="labelhienthi">Thông tin luồng xử lý:</td>
                                <td>&nbsp;
                                <dx:ASPxLabel ID="lblTenLuongHoTroHTYC1" ClientInstanceName="lblTenLuongHoTroHTYC1" runat="server" Text="lblTenLuongHoTroHTYC1">
                                </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Mô tả luồng xử lý:</td>
                                <td>&nbsp;
                                <dx:ASPxLabel ID="lblMoTaLuongHoTroHTYC1" ClientInstanceName="lblMoTaLuongHoTroHTYC1" runat="server" Text="lblMoTaLuongHoTroHTYC1">
                                </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Hệ thống hỗ trợ kỹ thuật:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblTenHeThongHoTroKyThuatHTYC1" runat="server" ClientInstanceName="lblTenHeThongHoTroKyThuatHTYC1" Text="lblTenHeThongHoTroKyThuatHTYC1">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Lĩnh vực:</td>
                                <td>&nbsp;
                                        <dx:ASPxLabel ID="lblLinhVucConHTYC1" runat="server" ClientInstanceName="lblLinhVucConHTYC1" Text="lblLinhVucConHTYC1">
                                        </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Số điện thoại:</td>
                                <td>&nbsp;
                        <dx:ASPxLabel ID="lblSoDienThoaiYC1" runat="server" ClientInstanceName="lblSoDienThoaiYC1" Text="ASPxLabel">
                        </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Mức độ yêu cầu:</td>
                                <td>&nbsp;
                        <dx:ASPxLabel ID="lblMucDoYeuCauYC1" runat="server" ClientInstanceName="lblMucDoYeuCauYC1" Text="ASPxLabel">
                        </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="labelhienthi">Nội dung yêu cầu:</td>
                                <td>&nbsp;
                                    <dx:ASPxLabel ID="lblNoiDungYeuCauHoTroGocHTYC1" runat="server" ClientInstanceName="lblNoiDungYeuCauHoTroGocHTYC1" Text="lblNoiDungYeuCauHoTroGocHTYC1">
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
                                <td>
                                    <dx:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="False" Text="Đóng" Theme="Office2010Blue">
                                        <ClientSideEvents Click="function(s, e) {
	                                        aspxPopupTimelineXuLyHoTroNodeCuoi.Hide();
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

            <br />
            <dx:ASPxPopupControl ID="popupXemNoiDungDaPhuongTien" runat="server" ClientInstanceName="popupXemNoiDungDaPhuongTien"
                HeaderText="Nội dung chi tiết" Theme="Office2010Blue"
                Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True"
                ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div>
                            <div class="container" style="width: 670px">
                                <div class="row">
                                    <div class="col-md-12">
                                        <dx:ASPxHtmlEditor ID="htmlXemNoiDungXLChiTiet" runat="server" Width="645px" Height="360px" Theme="Office2010Blue" ClientInstanceName="htmlXemNoiDungXLChiTiet">
                                            <SettingsHtmlEditing AllowHTML5MediaElements="true" AllowObjectAndEmbedElements="true" AllowYouTubeVideoIFrames="true" />
                                            <Toolbars>
                                                <dx:HtmlEditorToolbar Name="Toolbar">
                                                    <Items>
                                                        <dx:ToolbarFullscreenButton BeginGroup="true">
                                                        </dx:ToolbarFullscreenButton>
                                                    </Items>
                                                </dx:HtmlEditorToolbar>
                                            </Toolbars>

                                        </dx:ASPxHtmlEditor>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">&nbsp;</div>
                                    <div class="col-md-2" style="padding-top: 10px;">
                                        <dx:ASPxButton ID="btnDongXem" runat="server" AutoPostBack="false" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupXemNoiDungDaPhuongTien.Hide();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif"></Image>
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="popupNoiDungDaPhuongTien" runat="server" ClientInstanceName="popupNoiDungDaPhuongTien"
                HeaderText="Nội dung chi tiết" Theme="Office2010Blue"
                Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowResize="True"
                ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div>
                            <div class="container" style="width: 670px">
                                <div class="row">
                                    <div class="col-md-12">
                                        <dx:ASPxHtmlEditor ID="htmlNoiDungXLChiTiet" runat="server" Width="645px" Height="360px" Theme="Office2010Blue" ClientInstanceName="htmlNoiDungXLChiTiet">
                                            <SettingsHtmlEditing AllowHTML5MediaElements="true" AllowObjectAndEmbedElements="true" AllowYouTubeVideoIFrames="true" />
                                            <Toolbars>
                                                <dx:HtmlEditorToolbar Name="Toolbar0">
                                                    <Items>
                                                        <dx:ToolbarCutButton>
                                                        </dx:ToolbarCutButton>
                                                        <dx:ToolbarCopyButton>
                                                        </dx:ToolbarCopyButton>
                                                        <dx:ToolbarPasteButton>
                                                        </dx:ToolbarPasteButton>
                                                        <dx:ToolbarPasteFromWordButton Visible="False">
                                                        </dx:ToolbarPasteFromWordButton>
                                                        <dx:ToolbarUndoButton BeginGroup="True">
                                                        </dx:ToolbarUndoButton>
                                                        <dx:ToolbarRedoButton>
                                                        </dx:ToolbarRedoButton>
                                                        <dx:ToolbarRemoveFormatButton BeginGroup="True" Visible="False">
                                                        </dx:ToolbarRemoveFormatButton>
                                                        <dx:ToolbarSuperscriptButton BeginGroup="True">
                                                        </dx:ToolbarSuperscriptButton>
                                                        <dx:ToolbarSubscriptButton>
                                                        </dx:ToolbarSubscriptButton>
                                                        <dx:ToolbarInsertOrderedListButton BeginGroup="True">
                                                        </dx:ToolbarInsertOrderedListButton>
                                                        <dx:ToolbarInsertUnorderedListButton>
                                                        </dx:ToolbarInsertUnorderedListButton>
                                                        <dx:ToolbarIndentButton BeginGroup="True">
                                                        </dx:ToolbarIndentButton>
                                                        <dx:ToolbarOutdentButton>
                                                        </dx:ToolbarOutdentButton>
                                                        <dx:ToolbarInsertLinkDialogButton BeginGroup="True" Visible="False">
                                                        </dx:ToolbarInsertLinkDialogButton>
                                                        <dx:ToolbarUnlinkButton Visible="False">
                                                        </dx:ToolbarUnlinkButton>
                                                        <dx:ToolbarCheckSpellingButton BeginGroup="True">
                                                        </dx:ToolbarCheckSpellingButton>


                                                        <dx:ToolbarInsertImageDialogButton Visible="true">
                                                        </dx:ToolbarInsertImageDialogButton>



                                                        <dx:ToolbarTableOperationsDropDownButton BeginGroup="True" Visible="False">
                                                        </dx:ToolbarTableOperationsDropDownButton>


                                                        <dx:ToolbarInsertFlashDialogButton BeginGroup="true">
                                                        </dx:ToolbarInsertFlashDialogButton>
                                                        <dx:ToolbarInsertVideoDialogButton>
                                                        </dx:ToolbarInsertVideoDialogButton>
                                                        <dx:ToolbarInsertAudioDialogButton>
                                                        </dx:ToolbarInsertAudioDialogButton>
                                                        <dx:ToolbarInsertYouTubeVideoDialogButton>
                                                        </dx:ToolbarInsertYouTubeVideoDialogButton>
                                                        <dx:ToolbarInsertImageDialogButton>
                                                        </dx:ToolbarInsertImageDialogButton>

                                                    </Items>
                                                </dx:HtmlEditorToolbar>
                                                <dx:HtmlEditorToolbar Name="Toolbar">
                                                    <Items>
                                                        <dx:ToolbarParagraphFormattingEdit Visible="false">
                                                            <Items>
                                                                <dx:ToolbarListEditItem Text="Normal" Value="p" />
                                                                <dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
                                                                <dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
                                                                <dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
                                                                <dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
                                                                <dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
                                                                <dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
                                                                <dx:ToolbarListEditItem Text="Address" Value="address" />
                                                                <dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
                                                            </Items>
                                                        </dx:ToolbarParagraphFormattingEdit>
                                                        <dx:ToolbarFontNameEdit>
                                                            <Items>
                                                                <dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
                                                                <dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
                                                                <dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
                                                                <dx:ToolbarListEditItem Text="Arial" Value="Arial" />
                                                                <dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
                                                                <dx:ToolbarListEditItem Text="Courier" Value="Courier" />
                                                            </Items>
                                                        </dx:ToolbarFontNameEdit>
                                                        <dx:ToolbarFontSizeEdit>
                                                            <Items>
                                                                <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                                                <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                                                <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                                                <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                                                <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                                                <dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
                                                                <dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
                                                            </Items>
                                                        </dx:ToolbarFontSizeEdit>
                                                        <dx:ToolbarBoldButton BeginGroup="True">
                                                        </dx:ToolbarBoldButton>
                                                        <dx:ToolbarItalicButton>
                                                        </dx:ToolbarItalicButton>
                                                        <dx:ToolbarUnderlineButton>
                                                        </dx:ToolbarUnderlineButton>
                                                        <dx:ToolbarStrikethroughButton>
                                                        </dx:ToolbarStrikethroughButton>
                                                        <dx:ToolbarJustifyLeftButton BeginGroup="True">
                                                        </dx:ToolbarJustifyLeftButton>
                                                        <dx:ToolbarJustifyCenterButton>
                                                        </dx:ToolbarJustifyCenterButton>
                                                        <dx:ToolbarJustifyRightButton>
                                                        </dx:ToolbarJustifyRightButton>
                                                        <dx:ToolbarBackColorButton BeginGroup="True">
                                                        </dx:ToolbarBackColorButton>
                                                        <dx:ToolbarFontColorButton>
                                                        </dx:ToolbarFontColorButton>

                                                        <dx:ToolbarFullscreenButton BeginGroup="true">
                                                        </dx:ToolbarFullscreenButton>

                                                        <%--<dx:ToolbarUndoButton>
                        </dx:ToolbarUndoButton>
                        <dx:ToolbarRedoButton>
                        </dx:ToolbarRedoButton>
                        <dx:ToolbarBoldButton BeginGroup="True">
                        </dx:ToolbarBoldButton>
                        <dx:ToolbarItalicButton>
                        </dx:ToolbarItalicButton>
                        <dx:ToolbarUnderlineButton>
                        </dx:ToolbarUnderlineButton>
                        <dx:ToolbarStrikethroughButton>
                        </dx:ToolbarStrikethroughButton>
                        <dx:ToolbarInsertFlashDialogButton BeginGroup="true">
                        </dx:ToolbarInsertFlashDialogButton>
                        <dx:ToolbarInsertVideoDialogButton>
                        </dx:ToolbarInsertVideoDialogButton>
                        <dx:ToolbarInsertAudioDialogButton>
                        </dx:ToolbarInsertAudioDialogButton>
                        <dx:ToolbarInsertYouTubeVideoDialogButton>
                        </dx:ToolbarInsertYouTubeVideoDialogButton>
                        <dx:ToolbarInsertImageDialogButton>
                        </dx:ToolbarInsertImageDialogButton>
                        <dx:ToolbarFullscreenButton BeginGroup="true">
                        </dx:ToolbarFullscreenButton>
                                                        --%>
                                                    </Items>
                                                </dx:HtmlEditorToolbar>
                                            </Toolbars>
                                            <SettingsDialogs>
                                                <InsertFlashDialog>
                                                    <SettingsFlashUpload UploadFolder="~/HTHTKT/UploadFiles/FlashFiles">
                                                        <ValidationSettings AllowedFileExtensions=".swf" MaxFileSize="500000">
                                                        </ValidationSettings>
                                                    </SettingsFlashUpload>
                                                    <SettingsFlashSelector Enabled="True">
                                                        <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/FlashFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                            InitialFolder="" />
                                                    </SettingsFlashSelector>
                                                </InsertFlashDialog>
                                                <InsertVideoDialog>
                                                    <SettingsVideoUpload UploadFolder="~/HTHTKT/UploadFiles/VideoFiles">
                                                        <ValidationSettings AllowedFileExtensions=".mp4, .ogg" MaxFileSize="1000000">
                                                        </ValidationSettings>
                                                    </SettingsVideoUpload>
                                                    <SettingsVideoSelector Enabled="True">
                                                        <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/VideoFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                            InitialFolder="" />
                                                    </SettingsVideoSelector>
                                                </InsertVideoDialog>
                                                <InsertAudioDialog>
                                                    <SettingsAudioUpload UploadFolder="~/HTHTKT/UploadFiles/AudioFiles">
                                                        <ValidationSettings AllowedFileExtensions=".mp3, .ogg" MaxFileSize="500000">
                                                        </ValidationSettings>
                                                    </SettingsAudioUpload>
                                                    <SettingsAudioSelector Enabled="True">
                                                        <CommonSettings RootFolder="~/HTHTKT/Content/FileManager/AudioFiles" ThumbnailFolder="~/HTHTKT/Content/FileManager/Thumbnails"
                                                            InitialFolder="" />
                                                    </SettingsAudioSelector>
                                                </InsertAudioDialog>
                                                <InsertImageDialog>
                                                    <SettingsImageUpload UploadFolder="~/HTHTKT/UploadFiles/Images/">
                                                        <ValidationSettings AllowedFileExtensions=".jpe,.jpeg,.jpg,.gif,.png" MaxFileSize="500000">
                                                        </ValidationSettings>
                                                    </SettingsImageUpload>
                                                </InsertImageDialog>
                                            </SettingsDialogs>
                                        </dx:ASPxHtmlEditor>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-10">&nbsp;</div>
                                    <div class="col-md-2" style="padding-top: 10px;">
                                        <dx:ASPxButton ID="btnDongThem" runat="server" AutoPostBack="false" Text="Đóng" Theme="Office2010Blue">
                                            <ClientSideEvents Click="function(s, e) {
	                                            popupNoiDungDaPhuongTien.Hide();
                                            }" />
                                            <Image Url="~/HTHTKT/icons/delete_16x16.gif"></Image>
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>

<dx:ASPxHiddenField runat="server" ID="HiddenFieldYC" ClientInstanceName="HiddenFieldYC"></dx:ASPxHiddenField>
