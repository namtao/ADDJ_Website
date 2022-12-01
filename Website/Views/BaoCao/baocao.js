var Baocao = {
    BindDataToKhieuNaiReportInfo: function (obj) {
        
        Baocao.BindLinhVucChung(obj.LoaiKhieuNai, obj.LinhVucChung, obj.LinhVucCon);
        
    },
    BindLinhVucChung: function (loaikhieunaiID, linhVucChungID, linhVucConID) {
        $.ajax({
            beforeSend: function () {
            },
            type: "POST",
            dataType: "text",
            url: "/Views/BaoCao/Ajax/BaoCao.ashx",
            data: { type: "linhvucchung", loaikhieunaiID: loaikhieunaiID },
            success: function (text) {
                $('#dsLinhVucChung').html(text);
                if (linhVucChungID != 'undefined') $('#dsLinhVucChung').val(linhVucChungID);
                Baocao.BindLinhVucCon(linhVucChungID, linhVucConID);
            },
            error: function () {
            }
        });
    },
    BindLinhVucCon: function (linhVucChungID, linhVucConID) {
        $.ajax({
            beforeSend: function () {
            },
            type: "POST",
            dataType: "text",
            url: "/Views/BaoCao/Ajax/BaoCao.ashx",
            data: { type: "linhvuccon", linhVucChungID: linhVucChungID },
            success: function (text) {
                $('#dsLinhVucCon').html(text);
                if (linhVucChungID != 'undefined') $('#dsLinhVucCon').val(linhVucChungID);
            },
            error: function () {
            }
        });
    }
};

//$(document).ready(function () {
//    $('.fromdate').datepick({ dateFormat: 'dd/mm/yyyy' });
//    $('.todate').datepick({ dateFormat: 'dd/mm/yyyy' });

//    $('#txtFromDate').datepick({ dateFormat: 'dd/mm/yyyy' });
//    $('#txtToDate').datepick({ dateFormat: 'dd/mm/yyyy' });

//    $('#txtFromDate_BaoCaoTongHopTheoKhieuNai').datepick({ dateFormat: 'dd/mm/yyyy' });
//    $('#txtToDate_BaoCaoTongHopTheoKhieuNai').datepick({ dateFormat: 'dd/mm/yyyy' });

//    var d = new Date();
//    var fromDate = '01/' + (d.getMonth() + 1) + '/' + d.getFullYear();
//    var toDate = d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();

//    $('.fromdate').val(fromDate);
//    $('.todate').val(toDate);

//    $('#txtFromDate').val(fromDate);
//    $('#txtToDate').val(toDate);

//    $('#txtFromDate_BaoCaoTongHopTheoKhieuNai').val(fromDate);
//    $('#txtToDate_BaoCaoTongHopTheoKhieuNai').val(toDate);

//    $('#dsLoaiKhieuNai').change(function () {
//        var loaiKhieuNaiID = $('#dsLoaiKhieuNai option:selected').attr('code');
//        Baocao.BindLinhVucChung(loaiKhieuNaiID, '-1', '-1');
//    });

//    $('#dsLinhVucChung').change(function () {
//        var linhVucChungID = $('#dsLinhVucChung option:selected').attr('code');
//        Baocao.BindLinhVucCon(linhVucChungID, '-1');
//    });

//    //alert();
//});

//An, hien DIV onchange select
var lastDiv = "";
function showDiv(divName) {
    // hide last div

    if (divName == "bc31") {        
        $(".reportType1").addClass("hiddenDiv");
    }
    else {
        $(".reportType1").removeClass("hiddenDiv");
    }
    
	if (lastDiv) {
		document.getElementById(lastDiv).className = "hiddenDiv";
	}
	//if value of the box is not nothing and an object with that name exists, then change the class
	if (divName && document.getElementById(divName)) {
		document.getElementById(divName).className = "visibleDiv";
		lastDiv = divName;
	}
}

//$('#btnReport').click(function () {    
//    alert('chua vao')
//    var title = '';
//    var donViID = $('#dsPhongBan option:selected').attr('code');
//    var donVi = $('#dsPhongBan option:selected').attr('value');
//    var fromDate = $('#txtFromDate').val();
//    var toDate = $('#txtToDate').val();
//    var loaiKhieuNai = $('#dsLoaiKhieuNai option:selected').attr('value');
//    var loaiKhieuNaiID = $('#dsLoaiKhieuNai option:selected').attr('code');
//    var linhVucChung = $('#dsLinhVucChung option:selected').attr('value');
//    var linhVucChungID = $('#dsLinhVucChung option:selected').attr('code');
//    var linhVucCon = $('#dsLinhVucCon option:selected').attr('value');
//    var linhVucConID = $('#dsLinhVucCon option:selected').attr('code');
//    var loaibc_th = $("input:radio[name=rdoType]:checked").val();
//    var reportType = $('#dsReportType').val();
//    var page = '';
//    if (reportType == 'bc11') {
//        page = 'baocaochitietgiamtru.aspx';
//        title = 'Báo cáo chi tiết giảm trừ';
//    }
//    else if (reportType == 'bc21') {
//        page = 'baocaotonghopgiamtru.aspx';
//        title = 'Báo cáo tổng hợp giảm trừ';
//    }
//    else if (reportType == 'bc31') {
//        page = '';
//    }
//    else if (reportType == 'bc41') {
//        page = 'baocaochitietpps.aspx';
//        title = 'Báo cáo chi tiết PPS';
//    }
//    else if (reportType == 'bc51') {
//        page = 'baocaochitietpost.aspx';
//        title = 'Báo cáo chi tiết POST';
//    }
//    else if (reportType == 'bc61') {
//        page = '';
//    }
//    else if (reportType == 'bc71') {
//        page = 'danhsachkhieunai.aspx';
//        title = 'Danh sách khiếu nại';
//    }

//    if (page != '') {
//        if (fromDate != "" && toDate != "") {
//            if (compareDates(fromDate, 'dd/MM/yyyy', toDate, 'dd/MM/yyyy') == 1) {
//                alert("Ngày tháng chưa hợp lệ");
//            }
//            else {
//                if (loaibc_th == "html") {
//                    parent.$.messager.alertAuto(title, '<iframe style="border:none" width="980px" height="550px" src="/Views/BaoCao/Popup/' + page + '?donViID=' + donViID + '&donVi=' + donVi + '&fromDate=' + fromDate + '&toDate=' + toDate + '&loaiKhieuNaiID=' + loaiKhieuNaiID + '&loaiKhieuNai=' + loaiKhieuNai + '&linhVucChungID=' + linhVucChungID + '&linhVucChung=' + linhVucChung + '&linhVucConID=' + linhVucConID + '&linhVucCon=' + linhVucCon + '&loaibc=' + loaibc_th + '">');
//                } else {
//                    window.open("/Views/BaoCao/Popup/" + page + "?donViID=" + donViID + "&donVi=" + donVi + "&fromDate=" + fromDate + "&toDate=" + toDate + "&loaiKhieuNaiID=" + loaiKhieuNaiID + "&loaiKhieuNai=" + loaiKhieuNai + "&linhVucChungID=" + linhVucChungID + "&linhVucChung=" + linhVucChung + "&linhVucConID=" + linhVucConID + "&linhVucCon=" + linhVucCon + "&loaibc=" + loaibc_th);
//                }
//            }
//        }
//        else {
//            alert("Bạn phải chọn đầy đủ ngày báo cáo");
//        }
//    }
//    else {
//        alert("Bạn phải chọn báo cáo");
//    }

//});


$('#btnReport_TongHopTheoKhieuNai').click(function () {
    var title = '';
    var donViID = $('#dsPhongBan option:selected').attr('code');
    var donVi = $('#dsPhongBan option:selected').attr('value');
    var fromDate = $('#txtFromDate_BaoCaoTongHopTheoKhieuNai').val();
    var toDate = $('#txtToDate_BaoCaoTongHopTheoKhieuNai').val();
    var doiTac = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_cblDoiTac input[type="checkbox"]:checked').map(function () { return this.value; }).get().join(",");
    var loaiKhieuNai = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_tvLoaiKhieuNai input[type="checkbox"]:checked a').map(function () { GetNodeValue(this) }).get().join(",");
    alert(loaiKhieuNai);
    var loaibc_th = $("input:radio[name=rdoType]:checked").val();
    var reportType = $('#dsReportType').val();
    var page = '';
    if (reportType == 'bc31') {
        page = 'baocaotonghoptheokhieunai.aspx';
        title = 'Báo cáo tổng hợp theo khiếu nại';
    }

    if (page != '') {
        if (fromDate != "" && toDate != "") {
            if (compareDates(fromDate, 'dd/MM/yyyy', toDate, 'dd/MM/yyyy') == 1) {
                alert("Ngày tháng chưa hợp lệ");
            }
            else {
                if (loaibc_th == "html") {
                    parent.$.messager.alertAuto(title, '<iframe style="border:none" width="980px" height="550px" src="/Views/BaoCao/Popup/' + page + '?donViID=' + donViID + '&donVi=' + donVi + '&fromDate=' + fromDate + '&toDate=' + toDate + '&doitac=' + doiTac + '&loaiKhieuNai=' + loaiKhieuNai + '&loaibc=' + loaibc_th + '">');
                } else {
                    window.open("/Views/BaoCao/Popup/" + page + "?donViID=" + donViID + "&donVi=" + donVi + "&fromDate=" + fromDate + "&toDate=" + toDate + "&loaiKhieuNaiID=" + loaiKhieuNaiID + "&loaiKhieuNai=" + loaiKhieuNai + "&linhVucChungID=" + linhVucChungID + "&linhVucChung=" + linhVucChung + "&linhVucConID=" + linhVucConID + "&linhVucCon=" + linhVucCon + "&loaibc=" + loaibc_th);
                }
            }
        }
        else {
            alert("Bạn phải chọn đầy đủ ngày báo cáo");
        }
    }
    else {
        alert("Bạn phải chọn báo cáo");
    }

});


// Phi Hoang Hai : Biến tvLoaiKhieuNaiId hiện tại không có giá trị sử dụng (nếu xóa đối này đi thì sẽ ảnh hưởng đến nhiều trang do đó đang tạm thời để lại)
// khi nào có thời gian sẽ bỏ đối tvLoaiKhieuNaiId này đi
function jsBaoCaoThongKeOnLoad(tvLoaiKhieuNaiId) {
    
    $(".treeViewLoaiKhieuNai input[type=checkbox]").click(function () {
        if ($('.chkAutoCheckChildren_TreeViewLoaiKhieuNai').find("input[type=checkbox]").attr('checked')) {
            $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
        }
    });    

    $(".chkCheckAll_TreeViewLoaiKhieuNai").find("input[type=checkbox]").click(function () {
        $(".treeViewLoaiKhieuNai").find("input[type=checkbox]").attr("checked", this.checked);
    });

    // Check/Uncheck all checkbox loại khiếu nại ở ucReportType2.ascx
    $(".chkAllFirstItem_TreeViewLoaiKhieuNai").find("input[type=checkbox]").click(function () {
        $(".treeViewLoaiKhieuNai > table").find("input[type=checkbox]").attr("checked", this.checked);
    });    

    $('.fromdate').datepick({ dateFormat: 'dd/mm/yyyy' });
    $('.todate').datepick({ dateFormat: 'dd/mm/yyyy' });

    var d = new Date();
    var fromDate = '01/' + (d.getMonth() + 1) + '/' + d.getFullYear();
    var toDate = d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();

    $('.fromdate').each(function (i, obj) {
        if ($(this).val() == '') {
            $(this).val(fromDate)
        }
    });

    $('.todate').each(function (i, obj) {
        if ($(this).val() == '') {
            $(this).val(toDate)
        }
    });
    

    $('#dsLoaiKhieuNai').change(function () {
        var loaiKhieuNaiID = $('#dsLoaiKhieuNai option:selected').attr('code');
        Baocao.BindLinhVucChung(loaiKhieuNaiID, '-1', '-1');
    });

    $('#dsLinhVucChung').change(function () {
        var linhVucChungID = $('#dsLinhVucChung option:selected').attr('code');
        Baocao.BindLinhVucCon(linhVucChungID, '-1');
    });

    /*
    $('#btnReport').click(function () {
        var title = '';
        var khuVucID = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlKhuVuc_ReportType1 option:selected').attr('value');
        var khuVuc = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlKhuVuc_ReportType1 option:selected').text();
        var donViID = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlPhongBan option:selected').attr('value');
        var donVi = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlPhongBan option:selected').text();
        var fromDate = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtFromDate').val();
        var toDate = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_txtToDate').val();
        var loaiKhieuNai = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLoaiKhieuNai option:selected').text();
        var loaiKhieuNaiID = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLoaiKhieuNai option:selected').attr('value');
        var linhVucChung = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucChung option:selected').text();
        var linhVucChungID = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucChung option:selected').attr('value');
        var linhVucCon = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucCon option:selected').text();
        var linhVucConID = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlLinhVucCon option:selected').attr('value');
        var loaibc_th = $("input:radio[name=rdoType]:checked").val();
        var reportType = $('#ContentPlaceHolder_Main_ContentPlaceHolder_Text_ddlReportType option:selected').attr('value');

        var page = '';
        if (reportType == 'bc11') {
            page = 'baocaochitietgiamtru.aspx';
            title = 'Báo cáo chi tiết giảm trừ trả trước';
        }
        else if (reportType == 'bc21') {
            page = 'baocaotonghopgiamtru.aspx';
            title = 'Báo cáo tổng hợp giảm trừ';
        }
        else if (reportType == 'bc31') {
            page = '';
        }
        else if (reportType == 'bc41') {
            page = 'baocaochitietpps.aspx';
            title = 'Báo cáo chi tiết PPS';
        }
        else if (reportType == 'bc51') {
            page = 'baocaochitietpost.aspx';
            title = 'Báo cáo chi tiết POST';
        }
        else if (reportType == 'bc61') {
            page = '';
        }
        else if (reportType == 'bc71') {
            page = 'danhsachkhieunai.aspx';
            title = 'Danh sách khiếu nại';
        }
        if (reportType == 'bc81') {
            page = 'baocaochitietgiamtrutrasau.aspx';
            title = 'Báo cáo chi tiết giảm trừ trả sau';
        }

        if (page != '') {
            if (fromDate != "" && toDate != "") {
                if (!isValidateDate(fromDate)) {
                    alert('Từ ngày không hợp lệ');
                }
                else if (!isValidateDate(toDate)) {
                    alert('Đến ngày không hợp lệ');
                }
                else {
                    if (loaibc_th == "html") {
                        parent.$.messager.alertAuto(title, '<iframe style="border:none" width="980px" height="550px" src="/Views/BaoCao/Popup/' + page + '?khuVucID=' + khuVucID + '&khuVuc=' + khuVuc + '&donViID=' + donViID + '&donVi=' + donVi + '&fromDate=' + fromDate + '&toDate=' + toDate + '&loaiKhieuNaiID=' + loaiKhieuNaiID + '&loaiKhieuNai=' + loaiKhieuNai + '&linhVucChungID=' + linhVucChungID + '&linhVucChung=' + linhVucChung + '&linhVucConID=' + linhVucConID + '&linhVucCon=' + linhVucCon + '&loaibc=' + loaibc_th + '">');
                    } else {
                        window.open("/Views/BaoCao/Popup/" + page + "?khuVucID=' + khuVucID + '&khuVuc=' + khuVuc + '&donViID=" + donViID + "&donVi=" + donVi + "&fromDate=" + fromDate + "&toDate=" + toDate + "&loaiKhieuNaiID=" + loaiKhieuNaiID + "&loaiKhieuNai=" + loaiKhieuNai + "&linhVucChungID=" + linhVucChungID + "&linhVucChung=" + linhVucChung + "&linhVucConID=" + linhVucConID + "&linhVucCon=" + linhVucCon + "&loaibc=" + loaibc_th);
                    }
                }
            }
            else {
                alert("Bạn phải chọn đầy đủ ngày báo cáo");
            }
        }
        else {
            alert("Bạn phải chọn báo cáo");
        }

    });   
    */

    function isValidateDate(txtDate, format) {
        var dateFormat = '';
        var currVal = txtDate;
        if (currVal == '')
            return false;

        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray == null)
            return false;

        if (format === undefined) {
            dateFormat = 'dd/mm/yyyy';
        }
        else {
            dateFormat = format;
        }

        if (dateFormat == 'mm/dd/yyyy') {
            //Checks for mm/dd/yyyy format.
            dtMonth = dtArray[1];
            dtDay = dtArray[3];
            dtYear = dtArray[5];
        }
        else if (dateFormat == 'dd/mm/yyyy') {
            //Checks for dd/mm/yyyy format.
            dtDay = dtArray[1];
            dtMonth = dtArray[3];            
            dtYear = dtArray[5];
        }

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        return true;
    }
    
}