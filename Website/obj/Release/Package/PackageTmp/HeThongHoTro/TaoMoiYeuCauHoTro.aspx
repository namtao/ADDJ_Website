﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master_Default.Master" AutoEventWireup="true" CodeBehind="TaoMoiYeuCauHoTro.aspx.cs" Inherits="Website.HeThongHoTro.TaoMoiYeuCauHoTro"
    EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderCss" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderJs" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Text" runat="server">
    <script type="text/javascript">
        function ChonDonVi(s, e) {
            var tendv = '';
            var key = treelist_donvi_cc.GetFocusedNodeKey();
            treelist_donvi_cc.GetNodeValues(key, "linhvuc", function (value) {
                LoaiHoTro.SetText(value);
                tenDonVi = value;
                LoaiHoTro.SetKeyValue(key);
                LoaiHoTro.HideDropDown();
                //cmb_nhanvien_gsbh.PerformCallback(DropDownEdit.GetKeyValue());
            });
            // gán id loai hỗ trợ cho hidden
            LoaiHTKTID.Set('LoaiHTKTID', key);
            noiChuyenDen.PerformCallback();
        }
        var postponedCallbackRequired = false;
        //Chọn hệ thống hỗ trợ kỹ thuật
        function OnListBoxIndexChanged(s, e) {
            debugger;
            IDLoaiHoTro.Set('hidden_value', s.GetValue());
            IDLoaiHoTro2.Set('hidden_value2', 1);
            isReshresd.Set('res_value', 1);
            //treelist_donvi_cc.PerformCallback('refresh');
            if (CallbackPanel.InCallback())
                postponedCallbackRequired = true;
            else
                CallbackPanel.PerformCallback();
            // gán giá trị id cho hidden
            ThongHTKTID.Set('ThongHTKTID', s.GetValue());
            ThongTinMucHeThongYCHT(s.GetValue());
            //XuLyLayIdLuongHoTro(s.GetValue(), NoiChuyenDenID.Get('iddonvitao'));
        }
        function OnEndCallback(s, e) {
            if (postponedCallbackRequired) {
                CallbackPanel.PerformCallback();
                postponedCallbackRequired = false;
            }
        }

        function ThongTinMucHeThongYCHT(val) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_quanLy.asmx/thongtinMucHeThongYCHT',
                data: "{ id: " + val + " }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                       var objret = JSON.parse(jsonData.d);
                    $('#motaHeThong').text(objret[0].MOTA);
                },
                error: function () {
                    alert('Có lỗi xảy ra khi lấy thông tin luồng hỗ trợ, vui lòng thử lại sau!');
                }
            });
        }

        function XuLyLayIdLuongHoTro(val, val2) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/layThongTinIDluongHoTro',
                data: "{ id_ht_xuly_hotro: " + val + " , iddonvi: '" + val2 + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                    // gán giá trị id cho hidden
                    aspxHiddenIDLuongHoTro.Set('idluonghotro', jsonData.d);
                },
                error: function () {
                    alert('Có lỗi xảy ra khi lấy thông tin luồng hỗ trợ, vui lòng thử lại sau!');
                }
            });
        }

        function TaoMoiYeuCauHoTro() {
            if (cboChonHeThongHoTro.GetValue() == null) {
                alert('Bạn phải chọn hệ thống để thực hiện yêu cầu hỗ trợ');
                return;
            }

            if (aspxHiddenIDLuongHoTro.Get('idluonghotro') == null) {
                alert('Luồng xử lý này chưa được cấu hình, bạn liên hệ với quản trị để cấu hình.');
                return;
            }
            if (LoaiHTKTID.Get('LoaiHTKTID') == null) {
                alert('Bạn phải chọn Loại hỗ trợ');
                return;
            }

            if (!txtSoThueBaoYeuCau.GetValue()) {
                alert('Bạn phải nhập số thuê bao.');
                return;
            }
            else {
                // check thue bao
                var regex = /^((09[14]([0-9]){7})|(012[34579]([0-9]){7})|(088)([0-9]){7})$/;
                if (!regex.test(txtSoThueBaoYeuCau.GetValue())) {
                    alert("Số thuê bao phải đúng định dạng!");
                    txtSoThueBaoYeuCau.Focus();
                    return;
                }
            }

            if (cboMucDoSuCo.GetValue() == null || cboMucDoSuCo.GetValue() == '0') {
                alert('Bạn phải chọn mức độ sự cố.');
                return;
            }

            if (!noiDungYCHoTro.GetValue()) {
                alert('Bạn phải nhập nội dung thông tin hỗ trợ.');
                return;
            }

            if (noiChuyenDen.GetValue() == '0') {
                alert('Bạn phải chọn nơi chuyển đến!');
                noiDungYCHoTro.Focus();
                return;
            }
            var noidunghotrochitiet = '';
            if (htmlNoiDungYeuCauChiTiet.GetHtml() !== null) {
                noidunghotrochitiet = htmlNoiDungYeuCauChiTiet.GetHtml();
            }


            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/Services/ws_thongTinHoTro.asmx/taoMoiLuongHoTroXL',
                data: "{ idhethong: " + ThongHTKTID.Get('ThongHTKTID') + " , idluonghotro: '" + aspxHiddenIDLuongHoTro.Get('idluonghotro') + "' , idloaihotro: '" + LoaiHTKTID.Get('LoaiHTKTID') + "', noidunghotro:'" + noiDungYCHoTro.GetValue() + "', noidunghotrochitiet:'" + noidunghotrochitiet + "', iddonvi_from:'" + NoiChuyenDenID.Get('iddonvitao') + "', iddonvi_to:'" + NoiChuyenDenID.Get('NoiChuyenDenID') + "', nguoitao:'" + NoiChuyenDenID.Get('nguoixuly') + "', id_nguoitao:'" + NoiChuyenDenID.Get('id_nguoixuly') + "', sodienthoai:'" + txtSoThueBaoYeuCau.GetValue() + "', id_mucdo_suco:'" + cboMucDoSuCo.GetValue() + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                    var arr_info = jsonData.d.split('|');

                    if (arr_info[0] == '1')
                        testSaveFileAttachaaa(arr_info[1], arr_info[2]);

                    alert('Đã tạo yêu cầu hỗ trợ và chuyển xử lý thành công!. Đơn vị xử lý là: ' + noiChuyenDen.GetText());


                    if (arr_info[0] == '100001')
                        alert('Chưa cấu hình luồng chuyển đến cho phòng ban này, bạn liên hệ với quản trị hệ thống để yêu cầu cấu hình cho luồng chuyển đến phòng ban này.');
                    if (arr_info[0] == '100002')
                        alert('Có lỗi khi tạo.');


                    document.location.href = "/";
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert('Có lỗi xảy ra khi tạo, vui lòng thử lại sau! [' + err.Message +']');
                }
            });
        }



        var uploadInProgress = false,
            submitInitiated = false,
            uploadErrorOccurred = false;
        uploadedFiles = [];
        function onFileUploadComplete(s, e) {
            var callbackData = e.callbackData.split("|"),
                uploadedFileName = callbackData[0],
                isSubmissionExpired = callbackData[1] === "True";

            uploadedFiles.push(uploadedFileName);

            if (e.errorText.length > 0 || !e.isValid)
                uploadErrorOccurred = true;
            if (isSubmissionExpired && UploadedFilesTokenBox.GetText().length > 0) {
                var removedAfterTimeoutFiles = UploadedFilesTokenBox.GetTokenCollection().join("\n");
                alert("The following files have been removed from the server due to the defined 5 minute timeout: \n\n" + removedAfterTimeoutFiles);
                UploadedFilesTokenBox.ClearTokenCollection();
            }
        }
        function onFileUploadStart(s, e) {
            uploadInProgress = true;
            uploadErrorOccurred = false;
            UploadedFilesTokenBox.SetIsValid(true);
        }
        function onFilesUploadComplete(s, e) {
            uploadInProgress = false;
            for (var i = 0; i < uploadedFiles.length; i++)
                UploadedFilesTokenBox.AddToken(uploadedFiles[i]);
            updateTokenBoxVisibility();
            uploadedFiles = [];
            //if(submitInitiated) {
            //    SubmitButton.SetEnabled(true);
            //    SubmitButton.DoClick();
            //}
        }

        function onTokenBoxValidation(s, e) {
            var isValid = DocumentsUploadControl.GetText().length > 0 || UploadedFilesTokenBox.GetText().length > 0;
            e.isValid = isValid;
            if (!isValid) {
                e.errorText = "No files have been uploaded. Upload at least one file.";
            }
        }
        function onTokenBoxValueChanged(s, e) {
            updateTokenBoxVisibility();
        }
        function updateTokenBoxVisibility() {
            var isTokenBoxVisible = UploadedFilesTokenBox.GetTokenCollection().length > 0;
            UploadedFilesTokenBox.SetVisible(isTokenBoxVisible);
        }


        function testSaveFileAttachaaa(val, val1) {
            $.ajax({
                type: 'POST',
                url: '/HeThongHoTro/TaoMoiYeuCauHoTro.aspx/saveFileAttach',
                data: "{ idhethong: '" + ThongHTKTID.Get('ThongHTKTID') + "',idyeucauhotro: '" + val + "',idxuly_ycht: '" + val1 + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {

                    UploadedFilesTokenBox.ClearTokenCollection();
                },
                error: function () {
                    alert('Có lỗi xảy ra khi lưu file, vui lòng thử lại sau!');
                }
            });
        }
    </script>
    <div style="padding: 20px;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowCollapseButton="true" Height="300px" Width="800px" Theme="Office2010Silver" HeaderText="Tạo mới yêu cầu hỗ trợ">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <table style="width: 100%;" class="tblKhaiBaoHT">
                        <tr>
                            <td class="labelhienthi"  style="width:60%">Chọn hệ thống hỗ trợ kỹ thuật</td>
                            <td style="width:20%">
                                <dx:ASPxComboBox ID="ASPxComboBox1" ClientInstanceName="cboChonHeThongHoTro" runat="server" ValueType="System.String" Theme="Office2010Blue"
                                    AutoPostBack="false" CssClass="hindHT">
                                    <ClientSideEvents SelectedIndexChanged="OnListBoxIndexChanged" />
                                </dx:ASPxComboBox>
                                <dx:ASPxHint ID="ASPxHint1" runat="server" TargetSelector=".hindHT" Position="Right"
                                    Content="Bạn chọn những hệ thống nào cần yêu cầu xử lý vấn đề nào đó">
                                </dx:ASPxHint>
                            </td>
                            <td colspan="2" rowspan="5" style="width:50%" valign="top">
                                <div class="alert alert-warning">
                                <span id="motaHeThong"></span>.
                            </div></td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">Chọn lĩnh vực</td>
                            <td>
                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px"
                                    ClientInstanceName="CallbackPanel">
                                    <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <dx:ASPxDropDownEdit CssClass="hintLV" ID="LoaiHoTro" runat="server" Theme="Office2010Blue" ClientInstanceName="LoaiHoTro" AnimationType="None" Width="100%">
                                                <DropDownWindowTemplate>
                                                    <div>
                                                        <dx:ASPxTreeList ID="treelist_donvi_cc" runat="server" Width="350px" ClientInstanceName="treelist_donvi_cc"
                                                            Theme="Aqua" Font-Names="arial" Font-Size="12px" KeyFieldName="id"
                                                            OnVirtualModeCreateChildren="treelist_donvi_cc_VirtualModeCreateChildren"
                                                            OnVirtualModeNodeCreating="treelist_donvi_cc_VirtualModeNodeCreating"
                                                            OnCustomCallback="treelist_donvi_cc_OnCustomCallback" AutoGenerateColumns="False">
                                                            <SettingsBehavior AllowFocusedNode="true" FocusNodeOnExpandButtonClick="false" FocusNodeOnLoad="false" />
                                                            <Settings HorizontalScrollBarMode="Visible" ShowTreeLines="true" SuppressOuterGridLines="true" VerticalScrollBarMode="Visible" />
                                                            <ClientSideEvents FocusedNodeChanged="ChonDonVi" NodeExpanding="function(s, e) {
	                                                          IDLoaiHoTro2.Set('hidden_value2', 0);
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
                                <dx:ASPxHint ID="ASPxHint2" runat="server" TargetSelector=".hintLV" Position="Right"
                                    Content="Bạn chọn lĩnh vực thuộc hệ thống đã chọn ở trên">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">Số điện thoại (là số TB phản ánh lỗi, yc hỗ trợ hoặc liên hệ)</td>
                            <td>
                                <dx:ASPxTextBox ID="txtSoThueBaoYeuCau" CssClass="hintDT" runat="server" Theme="Office2010Blue" Width="170px" ClientInstanceName="txtSoThueBaoYeuCau">
                                </dx:ASPxTextBox>
                                <dx:ASPxHint ID="ASPxHint3" runat="server" TargetSelector=".hintDT" Position="Right"
                                    Content="Là số điện thoại của bạn yêu cầu hoặc của khách hàng nếu có phát sinh khiếu nại...">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">Mức độ sự cố:</td>
                            <td>
                                <dx:ASPxComboBox ID="cboMucDoSuCo" runat="server" CssClass="hintMD" ClientInstanceName="cboMucDoSuCo" Theme="Office2010Blue">
                                </dx:ASPxComboBox>
                                <dx:ASPxHint ID="ASPxHint4" runat="server" TargetSelector=".hintMD" Position="Right"
                                    Content="Bạn chọn mức độ ảnh hưởng của vấn đề...">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">Nội dung yêu cầu hỗ trợ</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <%--<dx:ASPxTextBox ID="noiDungYCHoTro" runat="server" ClientInstanceName="noiDungYCHoTro" Height="101px" Width="495px" Theme="Office2010Blue">
                            </dx:ASPxTextBox>--%>
                                <dx:ASPxMemo ID="noiDungYCHoTro" ClientInstanceName="noiDungYCHoTro" runat="server" Height="71px" Width="526px">
                                </dx:ASPxMemo>
                                <dx:ASPxHint ID="ASPxHint5" runat="server" TargetSelector=".hintND" Position="Right"
                                    Content="Bạn nhập nội dung mô tả vấn đề cần yêu cầu hay phản ánh, khiếu nại của khách hàng...">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Nội dung mô tả chi tiết (hình ảnh, video,...hoặc đa phương tiện nếu có):</td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">&nbsp;</td>
                            <td colspan="3">
                                <dx:ASPxButton ID="btnNhapDaPhuongTien" CssClass="hintDPT" runat="server" AutoPostBack="False" ClientInstanceName="btnNhapDaPhuongTien" Text="Nhập đa phương tiện" Theme="Office2010Blue">
                                    <ClientSideEvents Click="function(s, e) {
	                               popupNoiDungDaPhuongTien.Show();
                                }" />
                                    <Image Url="~/HTHTKT/icons/wizard_16x16.gif">
                                    </Image>
                                </dx:ASPxButton>
                                <dx:ASPxHint ID="ASPxHint6" runat="server" TargetSelector=".hintDPT" Position="Right"
                                    Content="Bạn nhập nội dung hình ảnh, video, và nhiều hơn thế...">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelhienthi">Tập tin đính kèm (doc, docx, xls, xlsx, jpg, png, gif, ...):</td>
                            <td colspan="4">
                                <dx:ASPxFormLayout ID="FormLayout" runat="server" ColCount="2" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                            <Items>
                                                <dx:LayoutGroup Caption="Danh sách tập tin">
                                                    <Items>
                                                        <dx:LayoutItem ShowCaption="False">
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <div id="dropZone">
                                                                        <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl" ClientInstanceName="DocumentsUploadControl" Width="100%"
                                                                            AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                            OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Office2010Blue">

                                                                            <BrowseButton Text="Chọn tập tin đính kèm..."></BrowseButton>

                                                                            <AdvancedModeSettings
                                                                                EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone">
                                                                            </AdvancedModeSettings>
                                                                            <ValidationSettings
                                                                                AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png"
                                                                                MaxFileSize="4194304">
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents
                                                                                FileUploadComplete="onFileUploadComplete"
                                                                                FilesUploadComplete="onFilesUploadComplete"
                                                                                FilesUploadStart="onFileUploadStart" />
                                                                        </dx:ASPxUploadControl>
                                                                        <br />
                                                                        <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox" ClientInstanceName="UploadedFilesTokenBox"
                                                                            NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                            <ClientSideEvents Init="updateTokenBoxVisibility" ValueChanged="onTokenBoxValueChanged" Validation="onTokenBoxValidation" />
                                                                            <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                        </dx:ASPxTokenBox>
                                                                        <br />
                                                                        <p class="Note">
                                                                            <dx:ASPxLabel ID="AllowedFileExtensionsLabel" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                            </dx:ASPxLabel>
                                                                            <br />
                                                                            <dx:ASPxLabel ID="MaxFileSizeLabel" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <dx:ASPxValidationSummary runat="server" ID="ValidationSummary" ClientInstanceName="ValidationSummary"
                                                                            RenderMode="Table" Width="250px" ShowErrorAsLink="false">
                                                                        </dx:ASPxValidationSummary>
                                                                    </div>
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                    </Items>
                                                </dx:LayoutGroup>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </td>                         
                        </tr>
                        <tr>
                            <td class="labelhienthi">Nơi chuyển đến</td>
                            <td colspan="3">
                                <dx:ASPxComboBox ID="noiChuyenDen" CssClass="hintNCD" runat="server" ClientInstanceName="noiChuyenDen" OnCallback="noiChuyenDen_Callback" Theme="Office2010Blue">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                NoiChuyenDenID.Set('NoiChuyenDenID',s.GetValue());

                                    XuLyLayIdLuongHoTro(ThongHTKTID.Get('ThongHTKTID'), s.GetValue());
                                    }" />
                                </dx:ASPxComboBox>
                                <dx:ASPxHint ID="ASPxHint7" runat="server" TargetSelector=".hintNCD" Position="Right"
                                    Content="Bạn chọn nơi chuyển đến yêu cầu, phản ánh khiếu nại này, chú ý chọn đúng nơi chuyển đến...">
                                </dx:ASPxHint>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <dx:ASPxButton ID="btnChuyenDenHT" runat="server" CssClass="hintCHUYEN" Text="Tạo và Chuyển yêu cầu" Theme="Office2010Blue" AutoPostBack="False">
                                    <ClientSideEvents Click="function(s, e) {
	                                    TaoMoiYeuCauHoTro();
                                    }" />
                                    <Image Url="~/HTHTKT/icons/redo1_16x16.gif">
                                    </Image>
                                </dx:ASPxButton>
                                <dx:ASPxHint ID="ASPxHint8" runat="server" TargetSelector=".hintCHUYEN" Position="Right"
                                    Content="Click vào để thực hiện chuyển yêu cầu, phản ánh, khiếu nại đến đơn vị cần chuyển...">
                                </dx:ASPxHint>
                                <dx:ASPxButton ID="testSaveFileAttach" runat="server" AutoPostBack="False" ClientInstanceName="testSaveFileAttach" EnableTheming="True" Text="ASPxButton" Theme="Office2010Blue" Visible="False">
                                    <ClientSideEvents Click="function(s, e) {
	                                    testSaveFileAttachaaa();
                                    }" />
                                </dx:ASPxButton>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="IDLoaiHoTro">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="ASPxHiddenField3" runat="server" ClientInstanceName="IDLoaiHoTro2">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="ASPxHiddenField2" runat="server" ClientInstanceName="isReshresd">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="ThongHTKTID" runat="server" ClientInstanceName="ThongHTKTID">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="LoaiHTKTID" runat="server" ClientInstanceName="LoaiHTKTID">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="NoiChuyenDenID" runat="server" ClientInstanceName="NoiChuyenDenID">
                    </dx:ASPxHiddenField>
                    <dx:ASPxHiddenField ID="aspxHiddenIDLuongHoTro" runat="server" ClientInstanceName="aspxHiddenIDLuongHoTro">
                    </dx:ASPxHiddenField>

                    <dx:ASPxHiddenField runat="server" ID="HiddenField" ClientInstanceName="HiddenField"></dx:ASPxHiddenField>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <dx:ASPxPopupControl ID="popupNoiDungDaPhuongTien" runat="server" ClientInstanceName="popupNoiDungDaPhuongTien"
            HeaderText="Nội dung chi tiết" Theme="Office2010Blue"
            Width="500px" AllowDragging="True" CloseAction="CloseButton" Modal="True"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div>
                        <div style="width: 670px">
                            <div class="row">
                                <div class="col-md-12">
                                    <dx:ASPxHtmlEditor ID="htmlNoiDungYeuCauChiTiet" runat="server" Width="645px" Height="360px" Theme="Office2010Blue" ClientInstanceName="htmlNoiDungYeuCauChiTiet">
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
                                    <dx:ASPxButton ID="btnDong" runat="server" AutoPostBack="false" Text="Đóng" Theme="Office2010Blue">
                                        <ClientSideEvents Click="function(s, e) {
	popupNoiDungDaPhuongTien.Hide();
}" />
                                        <Image Url="../HTHTKT/icons/delete_16x16.gif"></Image>
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>


                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
</asp:Content>
