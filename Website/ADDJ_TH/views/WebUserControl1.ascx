<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="Website.ADDJ_TH.views.WebUserControl1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<div>
    <style>
        .thongtin_hs td {
            padding: 4px;
        }

        .highlightme{
            color:red;
            background-color:yellow;
            font-weight:bold;
        }
    </style>
    <script>

        function suaThongTinHoSo(val) {
            popupSuaThongTin.Show();

            $.ajax({
                type: 'POST',
                url: '/ADDJ_TH/services/addj.asmx/thongtinHoSoChinhLy',
                data: "{ guid: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    ASPxHiddenField1.Set('guid', objret[0].guid);
                    txtSoHopEdit.SetValue(objret[0].hopso);     
                    txtSoHoSoEdit.SetValue(objret[0].hoso_so);
                    txtTieuDeHoSoEdit.SetValue(objret[0].trichyeunoidung);
                    lblTrichYeuNoiDungVBEdit.SetValue(objret[0].trichyeunoidung_mlhs);
                    txtToSoEdit.SetValue(objret[0].soto);     // Tờ số
                    txtPhongBanEdit.SetValue(objret[0].phongdoi);
                    txtThoiGianEdit.SetValue(objret[0].thoigian);
                    txtMLHSEdit.SetValue(objret[0].mlhs);
                    txtNamEdit.SetValue(objret[0].nam);
                    txtMSTEdit.SetValue(objret[0].mst);   //MST (CMT)
                    txtThoiHanBaoQuanEdit.SetValue(objret[0].thoihanbaoquan);
                    txtDoMatKhanEdit.SetValue(objret[0].domatkhan);  //tên loại
                    txtMemoGhiChuEdit.SetValue(objret[0].ghichu);
                    txtMaToKhaiEdit.SetValue(objret[0].matokhai);   //Tác giả văn bản
                    txtBoSung1Edit.SetValue(objret[0].bosung1);
                    txtDiaChiEdit.SetValue(objret[0].diachi);
                    txtBoSung2Edit.SetValue(objret[0].bosung2);
                    txtSoGiayCNEdit.SetValue(objret[0].sogiaycn);
                    txtSoKyHieuVBEdit.SetValue(objret[0].sokyhieuvb);
                    txtSTTEdit.SetValue(objret[0].stt);

                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function xoaThongTinHoSo(val) {
            if (confirm('Bạn có chắc chắn muốn xóa?')) {
                $.ajax({
                    type: 'POST',
                    url: '/ADDJ_TH/services/addj.asmx/xoaHoSoChinhLy',
                    data: "{ guid: '" + val + "' }",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    beforeSend: function () {
                        // setting a timeout
                        loadingdata2.Show();
                    },
                    success: function (jsonData) {
                        if (jsonData.d == '1') {
                            alert('Xóa thành công');
                            grvCustomer.PerformCallback();
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

        function themMoiThongTinHoSo() {
            popupThemMoiThongTin.Show();
        }
        function suaHoSoChinhLy() {

            $.ajax({
                type: 'POST',
                url: '/ADDJ_TH/services/addj.asmx/suaHoSoChinhLy',
                data: "{ guid: '" + ASPxHiddenField1.Get('guid') + "',hopso: '" + txtSoHopEdit.GetValue() + "',hoso_so: '" + txtSoHoSoEdit.GetValue() + "',tieudehoso: '" + lblTieuDeHoSoEdit.GetValue() + "',trichyeunoidung: '" + lblTrichYeuNoiDungVBEdit.GetValue() + "',soto: '" + txtToSoEdit.GetValue() +
                    "',thoigian: '" + txtThoiGianEdit.GetValue() + "',nam: '" + txtNamEdit.GetValue() + "',thoihanbaoquan: '" + txtThoiHanBaoQuanEdit.GetValue() + "',ghichu: '" + txtMemoGhiChuEdit.GetValue() + "',bosung1: '" + txtBoSung1Edit.GetValue() + "',bosung2: '" + txtBoSung2Edit.GetValue() +
                    "',sokyhieuvb: '" + txtSoKyHieuVBEdit.GetValue() + "',phongdoi: '" + txtPhongBanEdit.GetValue() + "',mlhs: '" + txtMLHSEdit.GetValue() + "',mst: '" + txtMSTEdit.GetValue() + "',domatkhan: '" + txtDoMatKhanEdit.GetValue() + "',matokhai: '" + txtMaToKhaiEdit.GetValue() +
                    "',diachi: '',sogiaycn: '" + txtSoGiayCNEdit.GetValue() + "',stt: '" + txtSTTEdit.GetValue() + "',url: '',trangthai_muontra: '',nguoilap_hs:'" +
                    "',tuychon1: '',tuychon2: '',tuychon3: '',madulieu: '',mavanban: '',mahop: '" +
                    "',mahoso: '',sohoso_tam: '',tuychon4: '',tuychon5: '',gia: '',day: '" +
                    "',khoang: '',tang: '',vitri: ''}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    if (jsonData.d == '1') {
                        testSaveFileAttachaaa(ASPxHiddenField1.Get('guid'));

                        alert('Sửa thành công');
                        popupSuaThongTin.Hide();
                        grvCustomer.PerformCallback();
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

        function themHoSoChinhLy() {

            $.ajax({
                type: 'POST',
                url: '/ADDJ_TH/services/addj.asmx/themHoSoChinhLy',
                data: "{ hopso: '" + txtSoHopAdd.GetValue() + "',hoso_so: '" + txtSoHoSoAdd.GetValue() + "',tieudehoso: '" + txtTieuDeHoSoAdd.GetValue() + "',trichyeunoidung: '" + txtTrichYeuNoiDungVBAdd.GetValue() + "',soto: '" + txtToSoAdd.GetValue() +
                    "',thoigian: '" + txtThoiGianAdd.GetValue() + "',nam: '" + txtNamAdd.GetValue() + "',thoihanbaoquan: '" + txtThoiHanBaoQuanAdd.GetValue() + "',ghichu: '" + txtMemoGhiChuAdd.GetValue() + "',bosung1: '" + txtBoSung1Add.GetValue() + "',bosung2: '" + txtBoSung2Add.GetValue() +
                    "',sokyhieuvb: '" + txtSoKyHieuVBAdd.GetValue() + "',phongdoi: '" + txtPhongBanAdd.GetValue() + "',mlhs: '" + txtMLHSAdd.GetValue() + "',mst: '" + txtMSTAdd.GetValue() + "',domatkhan: '" + txtDoMatKhanAdd.GetValue() + "',matokhai: '" + txtMaToKhaiAdd.GetValue() +
                    "',diachi: '',sogiaycn: '" + txtSoGiayCNAdd.GetValue() + "',stt: '" + txtSTTAdd.GetValue() + "',url: '',trangthai_muontra: '',nguoilap_hs:'" +
                    "',tuychon1: '',tuychon2: '',tuychon3: '',madulieu: '',mavanban: '',mahop: '" +
                    "',mahoso: '',sohoso_tam: '',tuychon4: '',tuychon5: '',gia: '',day: '" +
                    "',khoang: '',tang: '',vitri: ''}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                        var arr_info = jsonData.d.split('|');
                        if (arr_info[0] == '"1') {
                            testSaveFileAttachaaa(arr_info[1]);
                            alert('Thêm thành công');
                            popupThemMoiThongTin.Hide();
                            grvCustomer.PerformCallback();
                        }
                        else {
                            alert('Có lỗi trong quá trình thêm');
                        }
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }



        function hienThiThongTinHS(val) {           
            var kkkk = ASPxHiddenField1.Get('quyenXem');
            if (ASPxHiddenField1.Get('quyenXem') != '0') {
                grvCustomer.GetRowValues(val, 'guid', OnGetRowValues);
            }
        }

        function OnGetRowValues(values) {
            //alert(values.replace(/\\/g, "/"));
            //"ADDJ_TH/pdf.js/web/viewer.html?file=/PdfFiles/1.pdf"
            //xempdfcss
            popupThongTinChiTiet.Show();
            //loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer.html?file=/ADDJ_TH/PdfFiles/" + values.replace(/\\/g, "/"));
            xemThongTinHS(values);
        }

        function loadIframe(iframeName, url) {
            var $iframe = $('#' + iframeName);
            if ($iframe.length) {
                $iframe.attr('src', url);
                return false;
            }
            return true;
        }

        function xemThongTinHS(val) {
            // Nạp thông tin xử lý
            $.ajax({
                type: 'POST',
                url: '/ADDJ_TH/services/addj.asmx/thongTinChiTietXuLyDaPhuongTien',
                data: "{ guid: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    // setting a timeout
                    loadingdata2.Show();
                },
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    napDanhSachHeThongYCHT(objret[0].guid);

                    txtSoHop.SetValue(objret[0].hopso);
                    txtSoHoSo.SetValue(objret[0].hoso_so);
                    txtTieuDeHoSo.SetValue(objret[0].trichyeunoidung);
                    txtTrichYeuNoiDungVB.SetValue(objret[0].trichyeunoidung_mlhs);
                    txtToSo.SetValue(objret[0].soto);
                    txtPhongBan.SetValue(objret[0].phongdoi);
                    txtThoiGian.SetValue(objret[0].thoigian);
                    txtMLHS.SetValue(objret[0].mlhs);
                    txtNam.SetValue(objret[0].nam);
                    txtMST.SetValue(objret[0].mst);
                    txtThoiHanBaoQuan.SetValue(objret[0].thoihanbaoquan);
                    txtDoMatKhan.SetValue(objret[0].domatkhan);
                    txtMemoGhiChu.SetValue(objret[0].ghichu);
                    txtMaToKhai.SetValue(objret[0].matokhai);
                    txtBoSung1.SetValue(objret[0].bosung1);
                    txtDiaChi.SetValue(objret[0].diachi);
                    txtBoSung2.SetValue(objret[0].bosung2);
                    txtSoGiayCN.SetValue(objret[0].sogiaycn);
                    txtSoKyHieuVB.SetValue(objret[0].sokyhieuvb);
                    txtSTT.SetValue(objret[0].stt);

                    //loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer.html?file=/ADDJ_TH/PdfFiles/" + objret[0].url.replace(/\\/g, "/"));
                    loadingdata2.Hide();
                },
                error: function () {
                    loadingdata2.Hide();
                }
            });
        }

        function dongXemChiTiet() {
            popupThongTinChiTiet.Hide();
        }



        function napDanhSachHeThongYCHT(val) {
            cboDanhSachFileDinhKem.ClearItems();
            $.ajax({
                type: 'POST',
                url: '/ADDJ_TH/services/addj.asmx/danhSachFileDinhKem',
                data: "{ guid: '" + val + "' }",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {
                    var objret = JSON.parse(jsonData.d);
                    //cboDanhSachFileDinhKem.AddItem('-- chọn tệp --', 0);
                    for (var i = 0; i < objret.length; i++) {
                        cboDanhSachFileDinhKem.AddItem(objret[i].tenfile, objret[i].guid);
                    }

                    if (cboDanhSachFileDinhKem.GetItemCount() > 0) {
                        cboDanhSachFileDinhKem.SetSelectedIndex(0);

                        var pane1 = splitter.GetPaneByName('sp_panel1');
                        var pane2 = splitter.GetPaneByName('sp_panel2');
                        pane2.Expand();
                        $('#xempdfcss').show();
                    
                        //loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_in.html?file=/ADDJ_TH/PdfFiles/" + cboDanhSachFileDinhKem.GetText().replace(/\\/g, "/"));

                        if (ASPxHiddenField1.Get('quyenIn') == '1') {
                            loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_in.html?file=/ADDJ_TH/PdfFiles/" + cboDanhSachFileDinhKem.GetText().replace(/\\/g, "/"));
                        }
                        if (ASPxHiddenField1.Get('quyenTaive') == '1') {
                            loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_taive.html?file=/ADDJ_TH/PdfFiles/" + cboDanhSachFileDinhKem.GetText().replace(/\\/g, "/"));
                        }
                        if (ASPxHiddenField1.Get('quyenIn') == '1' && ASPxHiddenField1.Get('quyenTaive') == '1') {
                            loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer.html?file=/ADDJ_TH/PdfFiles/" + cboDanhSachFileDinhKem.GetText().replace(/\\/g, "/"));
                        }
                        if (ASPxHiddenField1.Get('quyenIn') == '0' && ASPxHiddenField1.Get('quyenTaive') == '0') {
                            loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_khongquyen.html?file=/ADDJ_TH/PdfFiles/" + cboDanhSachFileDinhKem.GetText().replace(/\\/g, "/"));
                        }
                    }
                    else {
                        var pane1 = splitter.GetPaneByName('sp_panel1');
                        var pane2 = splitter.GetPaneByName('sp_panel2');
                        pane2.Collapse(pane1);
                        $('#xempdfcss').hide();
                    }
                },
                error: function () {
                }
            });
        }



        function chonHienThiFileDinhKem(s, e) {
            $('#xempdfcss').show();
            if (ASPxHiddenField1.Get('quyenIn') == '1') {
                loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_in.html?file=/ADDJ_TH/PdfFiles/" + s.GetText().replace(/\\/g, "/"));
            }
            if (ASPxHiddenField1.Get('quyenTaive') == '1') {
                loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_taive.html?file=/ADDJ_TH/PdfFiles/" + s.GetText().replace(/\\/g, "/"));
            }
            if (ASPxHiddenField1.Get('quyenIn') == '1' && ASPxHiddenField1.Get('quyenTaive') == '1') {
                loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer.html?file=/ADDJ_TH/PdfFiles/" + s.GetText().replace(/\\/g, "/"));
            }
            if (ASPxHiddenField1.Get('quyenIn') == '0' && ASPxHiddenField1.Get('quyenTaive') == '0') {
                loadIframe('xempdfcss', "ADDJ_TH/pdf.js/web/viewer_khongquyen.html?file=/ADDJ_TH/PdfFiles/" + s.GetText().replace(/\\/g, "/"));
            }
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
                UploadedFilesTokenBox1.ClearTokenCollection();
            }
        }
        function onFileUploadStart(s, e) {
            uploadInProgress = true;
            uploadErrorOccurred = false;
            UploadedFilesTokenBox.SetIsValid(true);
            UploadedFilesTokenBox1.SetIsValid(true);
        }
        function onFilesUploadComplete(s, e) {
            uploadInProgress = false;
            for (var i = 0; i < uploadedFiles.length; i++) {
                UploadedFilesTokenBox.AddToken(uploadedFiles[i]);
                UploadedFilesTokenBox1.AddToken(uploadedFiles[i]);
            }
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
            UploadedFilesTokenBox1.SetVisible(isTokenBoxVisible);
        }


        function testSaveFileAttachaaa(val) {
            $.ajax({
                type: 'POST',
                url: '/Default.aspx/saveFileAttach',
                data: "{ guid: '" + val + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (jsonData) {

                    UploadedFilesTokenBox.ClearTokenCollection();
                    UploadedFilesTokenBox1.ClearTokenCollection();
                },
                error: function () {
                    alert('Có lỗi xảy ra khi lưu file, vui lòng thử lại sau!');
                }
            });
        }









    </script>

     <script type="text/javascript">
        var textSeparator = ";";
        function updateTextHopSo() {
            var selectedItems = checkListBoxHopSo.GetSelectedItems();
            checkComboBoxHopSo.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValuesHopSo(dropDown, args) {
            checkListBoxHopSo.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTextsHopSo(texts);
            checkListBoxHopSo.SelectValues(values);
            updateTextHopSo(); // for remove non-existing texts
         }

           function getValuesByTextsHopSo(texts) {
            var actualValues = [];
            var item;
            for(var i = 0; i < texts.length; i++) {
                item = checkListBoxHopSo.FindItemByText(texts[i]);
                if(item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
         ///////////////////////////////////////////////////////////////////////////////////////
        function updateTextHoSoSo() {
            var selectedItems = checkListBoxHoSoSo.GetSelectedItems();
            checkComboBoxHoSoSo.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValuesHoSoSo(dropDown, args) {
            checkListBoxHoSoSo.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTextsHoSoSo(texts);
            checkListBoxHoSoSo.SelectValues(values);
            updateTextHoSoSo(); // for remove non-existing texts
         }
          function getValuesByTextsHoSoSo(texts) {
            var actualValues = [];
            var item;
            for(var i = 0; i < texts.length; i++) {
                item = checkListBoxHoSoSo.FindItemByText(texts[i]);
                if(item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
         ///////////////////////////////////////////////////////////////////////////////////////
        function updateTextPhongBan() {
            var selectedItems = checkListBoxPhongBan.GetSelectedItems();
            checkComboBoxPhongBan.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValuesPhongBan(dropDown, args) {
            checkListBoxPhongBan.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTextsPhongBan(texts);
            checkListBoxPhongBan.SelectValues(values);
            updateTextPhongBan(); // for remove non-existing texts
        }
         function getValuesByTextsPhongBan(texts) {
            var actualValues = [];
            var item;
            for(var i = 0; i < texts.length; i++) {
                item = checkListBoxPhongBan.FindItemByText(texts[i]);
                if(item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
         ///////////////////////////////////////////////////////////////////////////////////////
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++) 
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
      
    </script>
     <script type="text/javascript">
         function DoProcessEnterKey(htmlEvent, editName) {
             if (htmlEvent.keyCode == 13) {
                 ASPxClientUtils.PreventEventAndBubble(htmlEvent);
                 //if (editName) {
                 //    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                 //} else {
                 //    btnTimKiem.DoClick();
                 //}
                 btnTimKiem.DoClick();
             }
         }

    </script>


     <script type="text/javascript">
         // xử lý các sự kiện load tìm kiếm
         function beforeAsyncPostBack() {
             //var curtime = new Date();
             //alert('Time before PostBack:   ' + curtime);

             loadingdata2.Show();
         }

         function afterAsyncPostBack() {
             //var curtime = new Date();
             //document.getElementById('Label1').innerHTML = 'Time after PostBack:    ' + curtime;

             loadingdata2.Hide();
         }

         Sys.Application.add_init(appl_init);

         function appl_init() {
             var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
             pgRegMgr.add_beginRequest(BeginHandler);
             pgRegMgr.add_endRequest(EndHandler);
         }

         function BeginHandler() {
             beforeAsyncPostBack();
         }

         function EndHandler() {
             afterAsyncPostBack();
         }


    </script>
     <style type="text/css">
        .formLayout {
            max-width: 500px;
            margin: auto;
        }
    </style>
    <dx:ASPxGlobalEvents ID="glob" runat="server">
        <ClientSideEvents ControlsInitialized="function (s, e) { loadingdata2.Hide(); }" />
    </dx:ASPxGlobalEvents>
    <div>
        <!-- begin panel nav boot -->
        <div class="nav_btn_bootstrap">
            <ul>
                <li><a href="javascript:history.back()" class="btn btn-primary"><span class="glyphicon glyphicon-circle-arrow-left" aria-hidden="true"></span>&nbsp;Quay về</a></li>
                <li><a onclick="themMoiThongTinHoSo()" class="btn btn-primary" id="btnThemMoi" runat="server"><span class="new"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>&nbsp;Thêm mới</span></a></li>
            </ul>
        </div>

  


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>


      <div style="width: 100%; margin: 0 auto;">

            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowHeader="false" ShowCollapseButton="true" 
                Theme="Moderno" View="GroupBox" Width="100%" ContentPaddings-PaddingTop="0px">
                <ContentPaddings PaddingTop="0px" />
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxFormLayout runat="server" CssClass="formLayout" RequiredMarkDisplayMode="Auto">
                            <Items>
                                <dx:LayoutGroup Caption="Tìm kiếm" ColumnCount="3" GroupBoxDecoration="HeadingLine"
                                    UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                    <GroupBoxStyle>
                                        <Caption Font-Bold="true" Font-Size="12"></Caption>
                                    </GroupBoxStyle>
                                    <Items>
                                        <dx:LayoutItem ShowCaption="False" ColumnSpan="3" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="txtNoiDungTimKiem" runat="server" Width="100%" ClientInstanceName="txtNoiDungTimKiem" NullText="Nhập nội dung tìm kiếm" Theme="Glass">
                                                    
                                                     <ClientSideEvents KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'txtNoiDungTimKiem'); }" />

                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    
                                        <dx:LayoutItem Caption="Hộp số" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    
                                                    <dx:ASPxDropDownEdit ClientInstanceName="checkComboBoxHopSo" ID="checkComboBoxHopSo" Width="185px" runat="server" AnimationType="None" Theme="Glass">
                                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                                        <DropDownWindowTemplate>
                                                            <dx:ASPxListBox Width="100%" ID="listBoxHopSo" ClientInstanceName="checkListBoxHopSo" SelectionMode="CheckColumn"
                                                                runat="server" Height="200" EnableSelectAll="true" SelectAllText="Chọn tất">
                                                                <FilteringSettings ShowSearchUI="true" EditorNullText="Nhập nội dung để lọc..."   />
                                                                <Border BorderStyle="None" />
                                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                                <ClientSideEvents SelectedIndexChanged="updateTextHopSo" Init="updateTextHopSo" />
                                                            </dx:ASPxListBox>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="padding: 4px">
                                                                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Đóng" Style="float: right">
                                                                            <ClientSideEvents Click="function(s, e){ checkComboBoxHopSo.HideDropDown(); }" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </DropDownWindowTemplate>
                                                        <ClientSideEvents TextChanged="synchronizeListBoxValuesHopSo" DropDown="synchronizeListBoxValuesHopSo"
                                                            KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'checkComboBoxHopSo'); }"/>
                                                    </dx:ASPxDropDownEdit>
                                                
                                                
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                           <dx:LayoutItem Caption="Hồ sơ số" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                  
                                                    
                                                <dx:ASPxDropDownEdit ClientInstanceName="checkComboBoxHoSoSo" ID="checkComboBoxHoSoSo" Width="185px" runat="server" AnimationType="None" Theme="Glass">
                                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                                        <DropDownWindowTemplate>
                                                            <dx:ASPxListBox Width="100%" ID="listBoxHoSoSo" ClientInstanceName="checkListBoxHoSoSo" SelectionMode="CheckColumn"
                                                                runat="server" Height="200" EnableSelectAll="true" SelectAllText="Chọn tất">
                                                                <FilteringSettings ShowSearchUI="true" EditorNullText="Nhập nội dung để lọc..." />
                                                                <Border BorderStyle="None" />
                                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                                <ClientSideEvents SelectedIndexChanged="updateTextHoSoSo" Init="updateTextHoSoSo" />
                                                            </dx:ASPxListBox>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="padding: 4px">
                                                                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Đóng" Style="float: right">
                                                                            <ClientSideEvents Click="function(s, e){ checkComboBoxHoSoSo.HideDropDown(); }" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </DropDownWindowTemplate>
                                                        <ClientSideEvents TextChanged="synchronizeListBoxValuesHoSoSo" DropDown="synchronizeListBoxValuesHoSoSo" 
                                                            KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'checkComboBoxHoSoSo'); }"/>
                                                    </dx:ASPxDropDownEdit>
                                                
                                                
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                           <dx:LayoutItem Caption="Phòng ban" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                   
                                                    
                                                
                                                 <dx:ASPxDropDownEdit ClientInstanceName="checkComboBoxPhongBan" ID="checkComboBoxPhongBan" Width="185px" runat="server" AnimationType="None" Theme="Glass">
                                                        <DropDownWindowStyle BackColor="#EDEDED" />
                                                        <DropDownWindowTemplate>
                                                            <dx:ASPxListBox Width="100%" ID="listBoxPhongBan" ClientInstanceName="checkListBoxPhongBan" SelectionMode="CheckColumn"
                                                                runat="server" Height="200" EnableSelectAll="true" SelectAllText="Chọn tất">
                                                                <FilteringSettings ShowSearchUI="true" EditorNullText="Nhập nội dung để lọc..." />
                                                                <Border BorderStyle="None" />
                                                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                                <ClientSideEvents SelectedIndexChanged="updateTextPhongBan" Init="updateTextPhongBan" />
                                                            </dx:ASPxListBox>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="padding: 4px">
                                                                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Đóng" Style="float: right">
                                                                            <ClientSideEvents Click="function(s, e){ checkComboBoxPhongBan.HideDropDown(); }" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </DropDownWindowTemplate>
                                                        <ClientSideEvents TextChanged="synchronizeListBoxValuesPhongBan" DropDown="synchronizeListBoxValuesPhongBan"
                                                             KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'checkComboBoxPhongBan'); }"/>
                                                    </dx:ASPxDropDownEdit>


                                                
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>

                                         <dx:LayoutItem  ShowCaption="False" ColumnSpan="3" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                 
                                                  
                                                       <dx:ASPxButton ID="btnTimKiem" Width="100px" runat="server" Text="Tìm kiếm" Theme="Aqua" ClientInstanceName="btnTimKiem" OnClick="btnTimKiem_Click">
                                                            <Image IconID="iconbuilder_actions_zoom_svg_16x16">
                                                            </Image>
                                                        </dx:ASPxButton>
                                                
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                    <SettingsItemCaptions Location="Top" />
                                </dx:LayoutGroup>
 
                            </Items>
                        </dx:ASPxFormLayout>

 
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <br />
        </div>


            <dx:ASPxGridView ID="grvCustomer" ClientInstanceName="grvCustomer" runat="server"
                AutoGenerateColumns="False" KeyFieldName="guid"
                Width="100%" Theme="Aqua" OnCustomCallback="grvCustomer_CustomCallback" OnCustomButtonInitialize="grvCustomer_CustomButtonInitialize" OnCustomColumnDisplayText="grvCustomer_CustomColumnDisplayText">
                <ClientSideEvents RowDblClick="function(s,e){hienThiThongTinHS(e.visibleIndex);
                    }" CustomButtonClick="function(s, e) {          
                            if(e.buttonID=='Xem')
                            {
                                hienThiThongTinHS(e.visibleIndex);
                            }
                            if(e.buttonID=='Sua')
                            {
                                suaThongTinHoSo(grvCustomer.GetRowKey(e.visibleIndex));
                            }
                            if(e.buttonID=='Xoa')
                            {
                                xoaThongTinHoSo(grvCustomer.GetRowKey(e.visibleIndex));	
                            }
                    }"
                    BeginCallback="function(s, e) {
	                    loadingdata2.Show();
                    }"
                    EndCallback="function(s, e) {
	                    loadingdata2.Hide();
                    }" />
                <SettingsPager Visible="False">
                </SettingsPager>
                <Border BorderWidth="1px" BorderStyle="Solid"></Border>
                <Styles>
                    <RowHotTrack BackColor="Gold"></RowHotTrack>
                </Styles>
                <SettingsBehavior EnableRowHotTrack="true" AllowSelectByRowClick="true" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="1" Caption="ID" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="guid" VisibleIndex="1" Caption="guid" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="hopso" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="2" Caption="Hộp số" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="hoso_so" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="3" Caption="Hồ sơ số" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sokyhieuvb" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="4" Caption="Số và ký hiệu văn bản" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="thoigian" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="5" Caption="Ngày, tháng văn bản" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="matokhai" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="6" Caption="Tác giả văn bản" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="domatkhan" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="7" Caption="Tên loại" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="trichyeunoidung" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="9" Caption="Tiêu đề hồ sơ">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="trichyeunoidung_mlhs" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="8" Caption="Trích yếu nội dung VB">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="mst" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="10" Caption="MST (CMT)" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="soto" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="11" Caption="Tờ số" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="phongdoi" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="12" Caption="Phòng ban" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="thoihanbaoquan" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="13" Caption="THBQ" CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ghichu" PropertiesTextEdit-EncodeHtml="false" VisibleIndex="14" Caption="Ghi chú">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewCommandColumn VisibleIndex="15">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="Xem" Text="Xem" Visibility="Invisible">
                                <Image Url="../../HTHTKT/icons/view_16x16.gif"></Image>
                            </dx:GridViewCommandColumnCustomButton>
                            <dx:GridViewCommandColumnCustomButton ID="Sua" Text="Sửa" Image-Url="../../HTHTKT/icons/edit_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                            <dx:GridViewCommandColumnCustomButton ID="Xoa" Text="Xóa" Image-Url="../../HTHTKT/icons/delete_16x16.gif"></dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <div style="width: 100%; height: 30px; border: solid 1px #999; border-top: none;">
                <div style="float: right;">
                    <dx:ASPxPager ID="ASPxPager1" ItemCount="3" ItemsPerPage="1" runat="server" NumericButtonCount="5"
                        CurrentPageNumberFormat="{0}" OnPageIndexChanged="ASPxPager1_PageIndexChanged" Theme="Glass">
                        <LastPageButton Visible="True">
                        </LastPageButton>
                        <AllButton Text="Tất cả">
                        </AllButton>
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <Summary Position="Inside" Text="Trang {0} của {1} " />
                        <CurrentPageNumberStyle BackColor="#FFFF99" ForeColor="Red">
                            <Paddings PaddingLeft="5" PaddingRight="5" PaddingTop="2" PaddingBottom="2" />
                            <border bordercolor="#CC0000" borderstyle="Solid" borderwidth="1px" />
                        </CurrentPageNumberStyle>
                    </dx:ASPxPager>
                </div>
            </div>


                <br />
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                   <ProgressTemplate> 
                    <img alt="Đang xử lý..." src="Images/loader.gif" />
                   </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div style="width: 100%; margin: 0 auto;">

        </div>
    </div>
</div>

<dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="loadingdata2" runat="server"   Theme="Material"
    Modal="True" Text="Đang xử lý...">
</dx:ASPxLoadingPanel>

<dx:ASPxPopupControl ID="popupThongTinChiTiet" runat="server" AllowDragging="True" ClientInstanceName="popupThongTinChiTiet"
    CloseAction="CloseButton" Height="550px" Modal="True" Theme="Office2010Blue" Width="1099px"
    HeaderText="Thông tin hồ sơ" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    AllowResize="True" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True" ScrollBars="Vertical">
    <ClientSideEvents  AfterResizing="function(s, e) {
    splitter.AdjustControl();}" />
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">

            <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Width="100%" Height="100%" 
                ClientInstanceName="splitter" SeparatorSize="20px" ShowCollapseBackwardButton="true" ShowCollapseForwardButton="true">
                <Panes>
                    <dx:SplitterPane Name="sp_panel1">

                        <ContentCollection>
                            <dx:SplitterContentControl runat="server">

                                <dx:ASPxPanel ID="ASPxPanel2" runat="server" Width="100%" Height="100%">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">
                                            <table style="width: 100%; height: 100%;" class="thongtin_hs">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblSoHop" ClientInstanceName="lblSoHop" runat="server" Text="Số hộp" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtSoHop" ClientInstanceName="txtSoHop" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblSoHoSo" ClientInstanceName="lblSoHoSo" runat="server" Text="Số hồ sơ" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtSoHoSo" ClientInstanceName="txtSoHoSo" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblTieuDe0" ClientInstanceName="lblTrichYeuNoiDungVB" runat="server" Text="Tiêu đề hồ sơ" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td colspan="3">
                                                        <dx:ASPxMemo ID="txtTieuDeHoSo" runat="server" ClientInstanceName="txtTieuDeHoSo" Height="51px" Theme="Glass" Width="100%">
                                                        </dx:ASPxMemo>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblTrichYeuNoiDungVB" runat="server" ClientInstanceName="lblTrichYeuNoiDungVB" Text="Trích yếu nội dung văn bản" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td colspan="3">
                                                        <dx:ASPxMemo ID="txtTrichYeuNoiDungVB" runat="server" ClientInstanceName="txtTrichYeuNoiDungVB" Height="51px" Theme="Glass" Width="100%">
                                                        </dx:ASPxMemo>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblToSo" ClientInstanceName="lblToSo" runat="server" Text="Tờ số" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtToSo" ClientInstanceName="txtToSo" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Phòng ban" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtPhongBan" runat="server" Width="170px" ClientInstanceName="txtPhongBan" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Thời gian" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtThoiGian" ClientInstanceName="txtThoiGian" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Mục lục HS" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtMLHS" ClientInstanceName="txtMLHS" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                                                                        <td>
                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Số và ký hiệu VB" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtSoKyHieuVB" ClientInstanceName="txtSoKyHieuVB" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>


                                                  
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="MST (CMT)" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtMST" ClientInstanceName="txtMST" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Thời hạn bảo quản" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtThoiHanBaoQuan" ClientInstanceName="txtThoiHanBaoQuan" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Tên loại" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtDoMatKhan" ClientInstanceName="txtDoMatKhan" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Ghi chú" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td colspan="3">
                                                        <dx:ASPxMemo ID="txtMemoGhiChu" ClientInstanceName="txtMemoGhiChu" runat="server" Height="71px" Width="100%" Theme="Glass"></dx:ASPxMemo>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Tác giả văn bản" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtMaToKhai" ClientInstanceName="txtMaToKhai" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr style="display:none">
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Bổ sung 1" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtBoSung1" ClientInstanceName="txtBoSung1" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Địa chỉ" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtDiaChi" ClientInstanceName="txtDiaChi" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr style="display:none">
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Bổ sung 2" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtBoSung2" ClientInstanceName="txtBoSung2" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Số giấy CN" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtSoGiayCN" ClientInstanceName="txtSoGiayCN" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr style="display:none">
                                                      <td>
                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Năm" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtNam" ClientInstanceName="txtNam" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="STT" Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxTextBox ID="txtSTT" ClientInstanceName="txtSTT" runat="server" Width="170px" Theme="Glass">
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxLabel ID="ASPxLabel57" runat="server" Text="Tệp đính kèm"  Theme="Glass">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="cboDanhSachFileDinhKem" runat="server" ClientInstanceName="cboDanhSachFileDinhKem"  Theme="Glass">
                                                            <ClientSideEvents SelectedIndexChanged="chonHienThiFileDinhKem" />
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <dx:ASPxButton ID="btnDong" ClientInstanceName="btnDong" runat="server" AutoPostBack="False" Text="Đóng" Theme="Aqua">
                                                            <ClientSideEvents Click="function(s,e){  popupThongTinChiTiet.Hide(); }" />
                                                            <Image IconID="scheduling_delete_svg_16x16">
                                                            </Image>
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                            </dx:SplitterContentControl>
                        </ContentCollection>

                    </dx:SplitterPane>
                    <dx:SplitterPane Name="sp_panel2">
                          <Separator>
                             
                            <BackwardCollapseButtonImage Url="../../Images/left.png" Width="18" Height="18" UrlHottracked="../../Images/left.png"></BackwardCollapseButtonImage>
                            <ForwardCollapseButtonImage Url="../../Images/right.png" Width="18" Height="18" UrlHottracked="../../Images/right.png"></ForwardCollapseButtonImage>
                        </Separator>

                        <ContentCollection>
                            <dx:SplitterContentControl runat="server">
                                <iframe id="xempdfcss" frameborder="0" style="overflow: hidden; height: 100%; width: 100%" height="100%" width="100%"></iframe>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                </Panes>
            </dx:ASPxSplitter>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



<dx:ASPxPopupControl ID="popupSuaThongTin" runat="server" AllowDragging="True"
    CloseAction="CloseButton" HeaderText="Sửa thông tin hồ sơ" Modal="True" Height="550px"
    Theme="Office2010Blue" Width="599px" AllowResize="True" ClientInstanceName="popupSuaThongTin" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCollapseButton="True"
    ShowMaximizeButton="True" ShowPinButton="True">

    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table style="width: 100%; height: 100%;" class="thongtin_hs">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" ClientInstanceName="lblSoHop" runat="server" Text="Số hộp">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoHopEdit" ClientInstanceName="txtSoHopEdit" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel2" ClientInstanceName="lblSoHoSo" runat="server" Text="Số hồ sơ">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoHoSoEdit" ClientInstanceName="txtSoHoSoEdit" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="lblTieuDeHoSoEdit" ClientInstanceName="lblTieuDeHoSoEdit" runat="server" Text="Tiêu đề hồ sơ">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">

                        <dx:ASPxMemo ID="txtTieuDeHoSoEdit" runat="server" ClientInstanceName="txtTieuDeHoSoEdit" Height="51px" Width="100%">
                        </dx:ASPxMemo>

                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" ClientInstanceName="lblTrichYeuNoiDungVB" Text="Trích yếu nội dung văn bản">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">
                        <dx:ASPxMemo ID="lblTrichYeuNoiDungVBEdit" runat="server" ClientInstanceName="lblTrichYeuNoiDungVBEdit" Height="51px" Width="100%">
                        </dx:ASPxMemo>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel4" ClientInstanceName="lblToSo" runat="server" Text="Tờ số">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtToSoEdit" ClientInstanceName="txtToSoEdit" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Phòng ban">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtPhongBanEdit" runat="server" Width="170px" ClientInstanceName="txtPhongBanEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Thời gian">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtThoiGianEdit" runat="server" Width="170px" ClientInstanceName="txtThoiGianEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Mục lục HS">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtMLHSEdit" runat="server" Width="170px" ClientInstanceName="txtMLHSEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>


                      <td>
                        <dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Số và ký hiệu VB">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoKyHieuVBEdit" runat="server" Width="170px" ClientInstanceName="txtSoKyHieuVBEdit">
                        </dx:ASPxTextBox>
                    </td>

                    <td>
                        <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="MST (CMT)">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtMSTEdit" runat="server" Width="170px" ClientInstanceName="txtMSTEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Thời hạn bảo quản">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtThoiHanBaoQuanEdit" runat="server" Width="170px" ClientInstanceName="txtThoiHanBaoQuanEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Tên loại">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtDoMatKhanEdit" runat="server" Width="170px" ClientInstanceName="txtDoMatKhanEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Ghi chú" Theme="Glass">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">
                        <dx:ASPxMemo ID="txtMemoGhiChuEdit" ClientInstanceName="txtMemoGhiChuEdit" runat="server" Height="71px" Width="100%" Theme="Glass"></dx:ASPxMemo>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Tác giả văn bản">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtMaToKhaiEdit" runat="server" Width="170px" ClientInstanceName="txtMaToKhaiEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Bổ sung 1">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtBoSung1Edit" runat="server" Width="170px" ClientInstanceName="txtBoSung1Edit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Địa chỉ">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtDiaChiEdit" runat="server" Width="170px" ClientInstanceName="txtDiaChiEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="Bổ sung 2">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtBoSung2Edit" runat="server" Width="170px" ClientInstanceName="txtBoSung2Edit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Số giấy CN">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoGiayCNEdit" runat="server" Width="170px" ClientInstanceName="txtSoGiayCNEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
                  
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Năm">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtNamEdit" runat="server" Width="170px" ClientInstanceName="txtNamEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="STT">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSTTEdit" runat="server" Width="170px" ClientInstanceName="txtSTTEdit">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
               


                 <tr>
                    <td class="labelhienthi">Tập tin đính kèm<br /> (doc, docx, xls,<br /> &nbsp;xlsx, jpg, png,<br /> &nbsp;gif, ...):</td>
                    <td colspan="4">
                        <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="2" UseDefaultPaddings="false">
                            <Items>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="400px" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutGroup Caption="Danh sách tập tin">
                                            <Items>
                                                <dx:LayoutItem ShowCaption="False">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer>
                                                            <div id="dropZone1">
                                                                <dx:ASPxUploadControl runat="server" ID="ASPxUploadControl1" ClientInstanceName="DocumentsUploadControl" Width="100%"
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="False" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Aqua">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm...">
                                                                        <Image IconID="businessobjects_bo_fileattachment_svg_16x16">
                                                                        </Image>
                                                                    </BrowseButton>

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
                                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox1" ClientInstanceName="UploadedFilesTokenBox1"
                                                                    NullText="Chọn một tập tin để tải lên..." AllowCustomTokens="false" ClientVisible="false" Theme="Office2010Blue">
                                                                    <ClientSideEvents Init="updateTokenBoxVisibility" ValueChanged="onTokenBoxValueChanged" Validation="onTokenBoxValidation" />
                                                                    <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                                                </dx:ASPxTokenBox>
                                                                <br />
                                                                <p class="Note">
                                                                    <dx:ASPxLabel ID="ASPxLabel55" runat="server" Text="Chỉ cho phép những định dạng: pdf, xls, xlsx, doc, doxc, .jpg, .jpeg, .gif, .png...." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                    <br />
                                                                    <dx:ASPxLabel ID="ASPxLabel56" runat="server" Text="Dung lượng lớn nhất: 4 MB." Font-Size="8pt">
                                                                    </dx:ASPxLabel>
                                                                </p>
                                                                <dx:ASPxValidationSummary runat="server" ID="ASPxValidationSummary1" ClientInstanceName="ValidationSummary"
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
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnSua" runat="server" AutoPostBack="False" ClientInstanceName="btnSua" Text="Sửa" Theme="Aqua">
                            <ClientSideEvents Click="function(s, e) {
	                                                                                    suaHoSoChinhLy();
                                                                                    }" />
                            <Image IconID="iconbuilder_actions_edit_svg_16x16">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnDongEdit" ClientInstanceName="btnDongEdit" runat="server" AutoPostBack="False" Text="Đóng" Theme="Aqua">
                            <ClientSideEvents Click="function(s,e){  popupSuaThongTin.Hide(); }" />
                            <Image IconID="scheduling_delete_svg_16x16">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="popupThemMoiThongTin" runat="server" AllowDragging="True"
    CloseAction="CloseButton" HeaderText="Thêm mới thông tin hồ sơ" Modal="True" Height="550px"
    Theme="Office2010Blue" Width="599px" AllowResize="True"
    ClientInstanceName="popupThemMoiThongTin" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" ShowCollapseButton="True" ShowMaximizeButton="True" ShowPinButton="True">

    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table style="width: 100%; height: 100%;" class="thongtin_hs">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel36" ClientInstanceName="lblSoHop" runat="server" Text="Số hộp">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoHopAdd" ClientInstanceName="txtSoHopAdd" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel37" ClientInstanceName="lblSoHoSo" runat="server" Text="Số hồ sơ">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoHoSoAdd" ClientInstanceName="txtSoHoSoAdd" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="lblTieuDeHoSoAdd" runat="server" ClientInstanceName="lblTieuDeHoSoAdd" Text="Tiêu đề hồ sơ">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">
                        <dx:ASPxMemo ID="txtTieuDeHoSoAdd" runat="server" ClientInstanceName="txtTieuDeHoSoAdd" Height="51px" Width="100%">
                        </dx:ASPxMemo>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel38" runat="server" ClientInstanceName="lblTrichYeuNoiDungVB" Text="Trích yếu nội dung văn bản">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">
                        <dx:ASPxMemo ID="txtTrichYeuNoiDungVBAdd" runat="server" ClientInstanceName="txtTrichYeuNoiDungVBAdd" Height="71px" Width="100%">
                        </dx:ASPxMemo>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel39" ClientInstanceName="lblToSo" runat="server" Text="Tờ số">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtToSoAdd" ClientInstanceName="txtToSoAdd" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Phòng ban">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtPhongBanAdd" runat="server" Width="170px" ClientInstanceName="txtPhongBanAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Thời gian">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtThoiGianAdd" runat="server" Width="170px" ClientInstanceName="txtThoiGianAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Mục lục HS">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtMLHSAdd" runat="server" Width="170px" ClientInstanceName="txtMLHSAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>


                          <td>
                        <dx:ASPxLabel ID="ASPxLabel53" runat="server" Text="Số và ký hiệu VB">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoKyHieuVBAdd" runat="server" Width="170px" ClientInstanceName="txtSoKyHieuVBAdd">
                        </dx:ASPxTextBox>
                    </td>



               
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel44" runat="server" Text="MST (CMT)">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtMSTAdd" runat="server" Width="170px" ClientInstanceName="txtMSTAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Thời hạn bảo quản">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtThoiHanBaoQuanAdd" runat="server" Width="170px" ClientInstanceName="txtThoiHanBaoQuanAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel46" runat="server" Text="Tên loại">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtDoMatKhanAdd" runat="server" Width="170px" ClientInstanceName="txtDoMatKhanAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Ghi chú" Theme="Glass">
                        </dx:ASPxLabel>
                    </td>
                    <td colspan="3">
                        <dx:ASPxMemo ID="txtMemoGhiChuAdd" ClientInstanceName="txtMemoGhiChuAdd" runat="server" Height="71px" Width="100%" Theme="Glass"></dx:ASPxMemo>
                    </td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>
                           <dx:ASPxLabel ID="ASPxLabel48" runat="server" Text="Tác giả văn bản">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                      <dx:ASPxTextBox ID="txtMaToKhaiAdd" runat="server" Width="170px" ClientInstanceName="txtMaToKhaiAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
               
                    </td>
                    <td>
                   
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel49" runat="server" Text="Bổ sung 1">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtBoSung1Add" runat="server" Width="170px" ClientInstanceName="txtBoSung1Add">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel50" runat="server" Text="Địa chỉ">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtDiaChiAdd" runat="server" Width="170px" ClientInstanceName="txtDiaChiAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel51" runat="server" Text="Bổ sung 2">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtBoSung2Add" runat="server" Width="170px" ClientInstanceName="txtBoSung2Add">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel52" runat="server" Text="Số giấy CN">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSoGiayCNAdd" runat="server" Width="170px" ClientInstanceName="txtSoGiayCNAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="display:none">
              
                         <td>
                        <dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Năm">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtNamAdd" runat="server" Width="170px" ClientInstanceName="txtNamAdd">
                        </dx:ASPxTextBox>
                    </td>



                    <td>
                        <dx:ASPxLabel ID="ASPxLabel54" runat="server" Text="STT">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtSTTAdd" runat="server" Width="170px" ClientInstanceName="txtSTTAdd">
                        </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="labelhienthi">Tập tin đính kèm<br /> (doc, docx, xls,<br /> &nbsp;xlsx, jpg, png,<br /> &nbsp;gif, ...):</td>
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
                                                                    AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="False" BrowseButton-Text="Add documents" FileUploadMode="OnPageLoad"
                                                                    OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete" Theme="Aqua">

                                                                    <BrowseButton Text="Chọn tập tin đính kèm...">
                                                                        <Image IconID="businessobjects_bo_fileattachment_svg_16x16">
                                                                        </Image>
                                                                    </BrowseButton>

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
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnThem" runat="server" AutoPostBack="False" ClientInstanceName="btnThem" Text="Thêm" Theme="Aqua">
                            <ClientSideEvents Click="function(s, e) {
	                                                                                    themHoSoChinhLy();
                                                                                    }" />
                            <Image IconID="iconbuilder_actions_addcircled_svg_16x16">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnDongAdd" ClientInstanceName="btnDongAdd" runat="server" AutoPostBack="False" Text="Đóng" Theme="Aqua">
                            <ClientSideEvents Click="function(s,e){ popupThemMoiThongTin.Hide(); }" />
                            <Image IconID="scheduling_delete_svg_16x16">
                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>

<dx:ASPxHiddenField ID="ASPxHiddenField1" ClientInstanceName="ASPxHiddenField1" runat="server"></dx:ASPxHiddenField>
<dx:ASPxHiddenField runat="server" ID="HiddenField" ClientInstanceName="HiddenField"></dx:ASPxHiddenField>
