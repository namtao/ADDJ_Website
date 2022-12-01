﻿var lastUrl = '';
var currentUrl = '';
var imageLoading = '<div class="img-loading" style=\"font:12px Arial\"><img src="/UserControls/AdminCP/images/ajax-loader.gif" /><span>Đang tải dữ liệu...</span></div>';
var htmlConfirm = '';

var urlLists = "";
var urlPostAction = "";
var urlForm = "";
var urlView = "";
var urlViewDM = "";
var urlFormReply = "";

var webId = "";
var listId = "";
var formHeight = 0;
var formWidth = 0;
var viewHeight = 0;
var viewWidth = 0

var SoBanGhiTrenTrang = 10;
var SoTrangHienThi = 3;
var urlListProcess = "";

var IsUploadImage = "0";

function getEditorContent(instanceName) {
    var oEditor = FCKeditorAPI.GetInstance(instanceName);
    return oEditor.GetHTML(true);
}

function createUploader() {
    var uploader = new qq.FileUploader({
        element: document.getElementById('btnChosse'),
        action: '/UserControls/UploadFile.aspx',
        debug: true,
        onSubmit: function (id, fileName) {
            // check trùng file
            var exits = false;
            //check trong trường hợp mới upload
            $("#listFileAttach li").each(function (index, item) {
                if (fileName == $(this).children("span").attr("title"))
                    exits = true;
            });
            //check trên file đã upload
            $("#listFileAttachRemove li").each(function (index, item) {
                if (fileName == $(this).children("span").attr("title"))
                    exits = true;
            });

            if (exits) {
                createMessage("Thông báo", "File " + fileName + " đã tồn tại.Bạn hãy chọn file khác");
                return false;
            }
        },
        onComplete: function (id, fileName, responseJSON) {
            if (responseJSON.upload) {
                $("#listFileAttach").append(getHTMLDeleteLink(responseJSON));
                $("#listValueFileAttach").val(changeHiddenInput())
            } else {
                createMessage("Thông báo", responseJSON.message);
            }
        }
    });
}
function createUploaderUnique(btnupload, ullistFileAttach, ullistFileAttachRemove, hdlistValueFileAttach) {
    var uploader = new qq.FileUploader({
        element: document.getElementById(btnupload),
        action: '/UserControls/UploadFile.aspx?img=' + IsUploadImage,
        debug: true,
        onSubmit: function (id, fileName) {
            // check trùng file
            var exits = false;
            //check trong trường hợp mới upload
            $("#" + ullistFileAttach + " li").each(function (index, item) {
                if (fileName == $(this).children("span").attr("title"))
                    exits = true;
            });
            //check trên file đã upload
            $("#" + ullistFileAttachRemove + " li").each(function (index, item) {
                if (fileName == $(this).children("span").attr("title"))
                    exits = true;
            });

            if (exits) {
                createMessage("Thông báo", fileName + " Đã tồn tại.");
                return false;
            }
        },
        onComplete: function (id, fileName, responseJSON) {
            if (responseJSON.upload) {
                $("#" + ullistFileAttach + "").append(getHTMLDeleteLinkUnique(responseJSON, btnupload, ullistFileAttach, hdlistValueFileAttach));
                $("#" + hdlistValueFileAttach + "").val(changeHiddenInputUnique(ullistFileAttach))

            } else {
                createMessage("Thông báo", responseJSON.message);
            }
        }
    });
}
function DeleteFileUpdateUnique(file, btnUpload, ullistFileAttachRemove) {
    var linkDelete = $("#listValueFileAttachRemove").val();
    $("#listValueFileAttachRemove").val(linkDelete + file + "#");
    $("#" + ullistFileAttachRemove + " span[id=" + file + "]").parent().remove();
    $("#" + btnUpload).css("display", "");
}
//lấy dữ liệu từ list Unique
function changeHiddenInputUnique(ollistFileAttach) {
    var valueFile = '[';
    var total = $("#" + ollistFileAttach + " li").length;
    $("#" + ollistFileAttach + " li").each(function (i) {
        valueFile += '{"FileServer": "' + $(this).children("span").attr("id") + '"\,';
        valueFile += '"FileName": "' + $(this).children("span").attr("title") + '"\}';
        if (i + 1 < total)
            valueFile += ',';
    });
    valueFile += "]";
    return valueFile;
}
//Lấy về html file của upload unique
function getHTMLDeleteLinkUnique(data, btnupdate, ollistFileAttach, hdlistValueFileAttach) {
    $("#" + btnupdate).css("display", "none");
    return "<li><span id=\"" + data.fileserver + "\" title=\"" + data.filename + "\">" + data.filename
    + "</span><a href=\"javascript:DeleteFileUnique('" + data.fileserver + "', '"
    + btnupdate + "', '" + ollistFileAttach + "','" + hdlistValueFileAttach + "' );\"><img src=\"/UserControls/Images/new_icon/act_filedelete.png\" title=\"Xóa file đính kèm\" border=\"0\"></a></li>";
}
//xóa file Unique
function DeleteFileUnique(file, btnupdate, ollistFileAttach, hdlistValueFileAttach) {
    $.post('/UserControls/DeleteFile.aspx', { del: file });
    $("#" + ollistFileAttach + " span[id=" + file + "]").parent().remove();
    $("#" + hdlistValueFileAttach).val(changeHiddenInputUnique(ollistFileAttach));
    $("#" + btnupdate).css("display", "");
}
/*file đính kèm*/
//Lấy về html file
function getHTMLDeleteLink(data) {
    return "<li><span id=\"" + data.fileserver + "\" title=\"" + data.filename + "\">" + data.filename + "</span><a href=\"javascript:DeleteFile('" + data.fileserver + "');\"><img src=\"/UserControls/Images/new_icon/act_filedelete.png\" title=\"Xóa file đính kèm\" border=\"0\"></a></li>";
}

//xóa file
function DeleteFile(file) {
    $.post('/UserControls/DeleteFile.aspx', { del: file });
    $("#listFileAttach span[id=" + file + "]").parent().remove();
    $("#listValueFileAttach").val(changeHiddenInput());
}

function DeleteFileUpdate(file) {
    var linkDelete = $("#listValueFileAttachRemove").val();
    $("#listValueFileAttachRemove").val(linkDelete + file + "#");
    $("#listFileAttachRemove span[id=" + file + "]").parent().remove();
}

//lấy dữ liệu từ list 
function changeHiddenInput() {
    var valueFile = '[';
    var total = $("#listFileAttach li").length;
    $("#listFileAttach li").each(function (i) {
        valueFile += '{"FileServer": "' + $(this).children("span").attr("id") + '"\,';
        valueFile += '"FileName": "' + $(this).children("span").attr("title") + '"\}';
        if (i + 1 < total)
            valueFile += ',';
    });
    valueFile += "]";
    return valueFile;
}

//hàm lấy về danh sách các ID được chọn qua checkbox
function getArrIDCheckBox() {
    var arrID = '';
    $("#gid input[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
        arrID += $(this).val() + ",";
    });
    arrID = (arrID.length > 0) ? arrID.substring(0, arrID.length - 1) : arrID;
    return arrID;
}

function createMessage(title, message) {
    $("#dialog-message").attr("title", title);
    $("#dialog-message").html("<p>" + message + "</p>");
    $("#dialog-message").dialog({
        resizable: false,
        height: 160,
        modal: true,
        buttons: {
            "Đóng lại": function () {
                $(this).dialog("close");
            }
        }
    });
}

function createMessageRespone(title, message, url) {
    $("#dialog-message").attr("title", title);
    $("#dialog-message").html("<p>" + message + "</p>");
    $("#dialog-message").dialog({
        resizable: false,
        height: 160,
        modal: true,
        close: function () { window.location = "/"; },
        buttons: {
            "Đóng lại": function () {
                $(this).dialog("close");
                window.location = "/";
            }
        }
    });
}
function createComfirm(title, message) {
    $("#dialog-confirm").attr("title", title);
    $("#dialog-confirm").html("<p>" + message + "</p>");
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 160,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                $(this).dialog("close");
                return true;
            },
            "Hủy": function () {
                $(this).dialog("close");
                return false;
            }
        }
    });
}

function Receipt(arrID) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        $.post(encodeURI(urlForm), { "do": "Receipt", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
            $("#dialog-form").html(data).dialog(
		            { title: "Tiếp nhận bài viết từ cộng tác viên", height: formHeight, width: formWidth }
	            ).dialog("open");
        });
    }
}

function View(arrID) {
    $.post(encodeURI(urlView), { "do": "View", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Xem chi tiết nội dung", height: viewHeight, width: viewWidth }
	    ).dialog("open");
    });
}

function Edit(arrID) {
    $.post(encodeURI(urlForm), { "do": "Edit", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
//Hàm gọi danh sách
function List(arrID) {
    $.post(encodeURI(urlOrderLists), { "do": "order", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
function Reply(CauHoiID) {
    $.post(encodeURI(urlFormReply), { "do": "Reply", "CauHoiID": "" + CauHoiID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Trả lời câu hỏi", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}

function Edit(arrID, parentUrlProcess) {
    $.post(encodeURI(urlForm), { "do": "Edit", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "",
        "ParentUrlProcess": "" + parentUrlProcess + ""
    }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
function Approved(arrID) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        postAction("approved", arrID);
    }
}

function Up(arrID) {
    postAction("Up", arrID);
}

function Down(arrID) {
    postAction("Down", arrID);
}

function Hot(arrID) {
    postAction("Hot", arrID);
}

function Active(arrID) {
    postAction("Active", arrID);
}

function Deactive(arrID) {
    postAction("DeActive", arrID);
}

function PostActive(arrID, Name) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn " + Name + " nào");
    }
    else {
        $("#ui-dialog-title-dialog-confirm").text("Gán quyền sử dụng");
        $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn gán quyền sử dụng?</p>");
        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("Active", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function PostDeactive(arrID, Name) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        $("#ui-dialog-title-dialog-confirm").text("Thu hồi quyền sử dụng");
        $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn thu hồi quyền sử dụng?</p>");
        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("Deactive", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function DeleteAll() {
    arrID = '';
    $("#ui-dialog-title-dialog-confirm").text("Xóa tất cả");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa tất cả các bản ghi?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("DeleteAll", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}

function AutoUpdate() {
    arrID = '';
    $("#dialog-confirm").attr("title", "Cập nhật lại dữ liệu");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn cập nhật lại dữ liệu?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("AutoUpdate", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}

function AutoIndex() {
    arrID = '';
    $("#dialog-confirm").attr("title", "Cập nhật lại dữ liệu - Ghi lại số thứ tự");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn cập nhật lại số thứ tự các bản ghi?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("AutoIndex", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}

function Pending(arrID) {
    var titleDiag = '';
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        if (arrID.indexOf(',') > 0) {
            titleDiag = "Chuyển các bản ghi về trạng thái chờ duyệt";
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn chuyển các bản ghi đã chọn sang trạng thái chờ duyệt?</p>");
        }
        else {
            titleDiag = "Chuyển bản ghi về trạng thái chờ duyệt";
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn chuyển bản ghi đã chọn sang trạng thái chờ duyệt?</p>");
        }

        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            title: titleDiag,
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("pending", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function Rejected(arrID) {
    var titleDiag = '';
    var contDiag = '';
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        if (arrID.indexOf(',') > 0) {
            titleDiag = "Hủy duyệt các bản ghi đã chọn";
            contDiag = "<p>Bạn có chắc chắn muốn hủy duyệt các bản ghi đã chọn?</p>"
        }
        else {
            titleDiag = "Hủy duyệt bản ghi đã chọn";
            contDiag = "<p>Bạn có chắc chắn muốn hủy duyệt bản ghi đã chọn?</p>"
        }
        var comfirmReturn = false;
        $("#dialog-confirm").html(contDiag);
        $("#dialog-confirm").dialog({
            title: titleDiag,
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("rejected", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function Delete(arrID) {
    var titleDiag = '';
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        if (arrID.indexOf(',') > 0) {
            titleDiag = "Xóa các bản ghi đã chọn";
            //$("#dialog-confirm").attr("title", "Xóa các bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa các bản ghi đã chọn?</p>");
        }
        else {
            //$("#dialog-confirm").attr("title", "Xóa bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa bản ghi đã chọn?</p>");
            titleDiag = "Xóa bản ghi đã chọn";
        }
        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            title: titleDiag,
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("delete", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function DeleteLuatSu(arrID) {
    $("#dialog-confirm").attr("title", "Xóa các bản ghi đã chọn");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa các bản ghi đã chọn?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postActionLuatSu("delete", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}


function Show(arrID) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        postAction("show", arrID);
    }
}

function Hide(arrID) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        if (arrID.indexOf(',') > 0) {
            $("#ui-dialog-title-dialog-confirm").text("Ẩn các bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn ẩn các bản ghi đã chọn?</p>");
        }
        else {
            $("#ui-dialog-title-dialog-confirm").text("Ẩn bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn ẩn bản ghi đã chọn?</p>");
        }

        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("hide", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function postAction(method, arrID) {
    $.post(encodeURI(urlPostAction), { "do": "" + method + "", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        if (data.Erros) {
            createMessage("Thông báo", "Lỗi được thông báo: " + data.Message);
            reLoadListLuatSuContent();
        }
        else {
            createMessage("Thông báo", data.Message);
            reLoadListLuatSuContent();
        }
    });
}

function postActionLuatSu(method, arrID) {
    $.post(encodeURI(urlPostAction), { "do": "" + method + "", "ItemID": "" + arrID + "" }, function (data) {
        if (data.Erros) {
            createMessage("Thông báo", "Lỗi được thông báo: " + data.Message);
            reLoadListLuatSuContent();
        }
        else {
            createMessage("Thông báo", data.Message);
            reLoadListLuatSuContent();
        }
    });
}

function reLoadListContent() {
    var urlReload;
    if (getUrlReload() != null)
        urlReload = getUrlReload();
    else
        urlReload = urlLists;
    LoadListContent(urlReload);
}

function reLoadListLuatSuContent() {
    var urlReload;
    if (getUrlReload() != null)
        urlReload = urlLists + getSpliter(urlLists) + getDefaultQueryValue() + "&" + getUrlReload();
    else
        urlReload = urlLists + getSpliter(urlLists) + getDefaultQueryValue();
    LoadListContent(urlReload);
}

function reLoadListLuatSuContentFix(prefix) {
    var urlReload;
    if (getUrlReload() != null)
        urlReload = urlLists + "&" + getDefaultQueryValue() + "&" + getUrlReload();
    else
        urlReload = urlLists + "&" + getDefaultQueryValue();

    $("#list-container-" + prefix).html(imageLoading);
    loadAjaxContent(urlReload, '#list-container-' + prefix + '');
}

function getListDanhMuc(urlLoad) {
    urlLoad = urlLoad + "?" + getDefaultQueryValue();
    $("#list-container").html(imageLoading);
    loadAjaxContent(urlLoad, "#list-container");
}

function reLoadListListContent() {
    var urlReload;
    if (getUrlReload() != null) {
        var urlTemp = getUrlReload();
        if (urlTemp.indexOf('?') > 0)
            urlReload = getUrlReload() + "&" + getDefaultQueryValue();
        else
            urlReload = getUrlReload() + "?" + getDefaultQueryValue();
    }
    else
        urlReload = urlList + "?" + getDefaultQueryValue();
    $("#list-container").html(imageLoading);
    loadAjaxContent(urlReload, "#list-container");
}

function getDefaultQueryValue() {
    return "RowPerPage=" + SoBanGhiTrenTrang + "&PageStep=" + SoTrangHienThi;
}

function UpdateMessge(message) {
    $("#message-container").html(message);
}

function thowMessageOnError(message) {
    $(".validation").html(message).addClass("ui-state-error");
}

function urlCheck() {
    url = window.location.href;
    if (url.indexOf('#') != -1) {
        url = url.split('#');
        if (url[1]) window.location.href = href + '#' + url[1];
        return;
    }
}

function getUrlReload() {
    url = window.location.href;
    if (url.indexOf('#') != -1) {
        url = url.split('#');
        return url[1];
    }
}

function getUrlPathReload() {
    url = window.location.pathname;
    if (url.indexOf('#') != -1) {
        url = url.split('#');
        return url[0];
    }
}

function LoadListContent(urlContent) {
    $("#list-container").html(imageLoading);
    loadAjaxContent(urlContent, '#list-container')
}

function LoadListListContent(urlContent) {
    urlContent = urlContent + "&" + getDefaultQueryValue();
    $("#list-container").html(imageLoading);
    loadAjaxContent(urlContent, '#list-container')
}


function loadAjaxContent(urlContent, container) {
    $.ajax({
        url: encodeURI(urlContent),
        cache: false,
        type: "POST",
        success: function (data) {
            $(container).html(data);
        }
    });
}

function getSpliter(urlcheck) {
    var strReturn = "?";
    if (urlcheck.indexOf(strReturn) > -1)
        strReturn = "&";
    return strReturn;
}
//Hàm phân trang viết cho JSON
function Paging(linkPage, pageStep, currentPage, rowPerPage, totalRow) {

    var strPageHTML = "<div class=\"paging\">";
    if (currentPage > pageStep + 1) {
        strPageHTML += "<a href=\"" + linkPage + 1 + "\">« Đầu</a>";
        strPageHTML += "<a href=\"" + linkPage + (currentPage - 1) + "\">Trước</a>";
        strPageHTML += "<span>...</span>";
    }
    var TotalPage = (totalRow % rowPerPage == 0) ? totalRow / rowPerPage : ((totalRow - (totalRow % rowPerPage)) / rowPerPage) + 1;

    var BeginFor = ((currentPage - pageStep) > 1) ? (currentPage - pageStep) : 1;
    var EndFor = ((currentPage + pageStep) > TotalPage) ? TotalPage : (currentPage + pageStep);
    for (var pNumber = BeginFor; pNumber <= EndFor; pNumber++) {
        if (pNumber == currentPage)
            strPageHTML += "<a href=\"javascript:;\" class=\"current\">" + pNumber + "</a>";
        else
            strPageHTML += "<a href=\"" + linkPage + pNumber + "\">" + pNumber + "</a>";
    }
    if (currentPage < (TotalPage - pageStep)) {
        strPageHTML += "<span>...</span>";
        strPageHTML += "<a href=\"" + linkPage + (currentPage + 1) + "\">Sau</a>";
        strPageHTML += "<a href=\"" + linkPage + TotalPage + "\">Cuối »</a>";
    }
    strPageHTML += "</div>";
    if (TotalPage > 1) {
        $("#paging").html(strPageHTML);
    }
}

function ReLoadContent() {
    var urlReload;
    if (urlLists.indexOf("&RowPerPage=") > -1 && urlLists.indexOf("&PageStep=") > -1) {
        urlReload = urlLists;
    }
    else {
        if (urlLists.indexOf("&RowPerPage=") > -1) {
            urlReload = urlLists + "&PageStep=" + SoTrangHienThi;
        }
        else {
            urlReload = urlLists + "&RowPerPage=" + SoBanGhiTrenTrang + "&PageStep=" + SoTrangHienThi;
        }
    }

    LoadListContent(urlReload);
}
function LoadListContentPaging(urlContent) {
    var urlReload;
    $("#list-container").html(imageLoading);
    if (urlContent.indexOf("&RowPerPage=") > -1 && urlContent.indexOf("&PageStep=") > -1) {
        urlReload = urlContent;
    }
    else {
        if (urlContent.indexOf("&RowPerPage=") > -1) {
            urlReload = urlContent + "&PageStep=" + SoTrangHienThi;
        }
        else {
            urlReload = urlContent + "&RowPerPage=" + SoBanGhiTrenTrang + "&PageStep=" + SoTrangHienThi;
        }
    }
    loadAjaxContent(urlReload, '#list-container')
}
//<!--Dịch vụ công-->
function PheDuyet(arrID) {
    $("#ui-dialog-title-dialog-confirm").text("Phê duyệt các hồ sơ đã chọn");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn phê duyệt các hồ sơ đã chọn?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("pheduyet", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}
function BoSung(arrID) {
    $("#dialog-confirm").attr("title", "Chuyển trạng thái");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn chuyển các hồ sơ đã chọn về trạng thái cần bổ sung giấy tờ?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("bosung", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}
function EditHoSo(arrID, UrlListProcessHinhThuc, UrlListProcessTrangThai) {
    $.post(encodeURI(urlForm), { "do": "Edit", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "",
        "UrlListProcessHinhThuc": "" + UrlListProcessHinhThuc + "",
        "UrlListProcessTrangThai": "" + UrlListProcessTrangThai + ""
    }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
function createCustomUploader(title, btnupload, ullistFileAttach, hdlistValueFileAttach) {
    var uploader = new qq.FileUploader({
        title: title,
        element: document.getElementById(btnupload),
        action: '/UserControls/UploadFile.aspx',
        debug: true,
        onSubmit: function (id, fileName) {
            // check trùng file
            var exits = false;
            //check trong trường hợp mới upload
            $("#" + ullistFileAttach + " li").each(function (index, item) {
                if (fileName == $(this).children("span").attr("title"))
                    exits = true;
            });

            if (exits) {
                createMessage("Thông báo", fileName + " đã tồn tại.");
                return false;
            }
        },
        onComplete: function (id, fileName, responseJSON) {
            if (responseJSON.upload) {
                $("#" + ullistFileAttach + "").append(getHTMLDeleteLinkUnique(responseJSON, btnupload, ullistFileAttach, hdlistValueFileAttach));
                $("#" + hdlistValueFileAttach + "").val(changeHiddenInputUnique(ullistFileAttach))

            } else {
                createMessage("Thông báo", responseJSON.message);
            }
        }
    });
}
function DeleteFileAttachHoSo(fileName, btnUpload, listFileAttachRemove, hdListFileAttachRemove) {
    $("#" + listFileAttachRemove + " span[id=" + fileName + "]").parent().remove();
    $("#" + hdListFileAttachRemove).val(fileName);
    $("#" + btnUpload).css("display", "");
}
function throwMessageOnErrorDialog(message) {
    $("#dialog-message").attr("title", "Có lỗi xảy ra");
    $("#ui-dialog-title-dialog-message").attr("title", "Có lỗi xảy ra");
    $("#ui-dialog-title-dialog-message").text("Có lỗi xảy ra");
    $("#dialog-message").html("<p>" + message + "</p>");
    $("#dialog-message").dialog({
        resizable: false,
        height: 160,
        modal: true,
        buttons: {
            "Đóng lại": function () {
                $(this).dialog("close");
            }
        }
    });
}
function BoSung(arrID) {
    $("#dialog-confirm").attr("title", "Chuyển trạng thái các hồ sơ đã chọn");
    $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn chuyển các hồ sơ đã chọn sang trạng thái cần bổ sung?</p>");
    var comfirmReturn = false;
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Tiếp tục": function () {
                postAction("bosung", arrID);
                $(this).dialog("close");
            },
            "Hủy": function () {
                $(this).dialog("close");
            }
        }
    });
}
function PheDuyet(arrID) {
    postAction("pheduyet", arrID);
}
function DeleteHoSo(arrID) {
    if (arrID == '') {
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    }
    else {
        if (arrID.indexOf(',') > 0) {
            $("#ui-dialog-title-dialog-confirm").text("Xóa các bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa các bản ghi đã chọn?</p>");
        }
        else {
            $("#dialog-confirm").attr("title", "Xóa bản ghi đã chọn");
            $("#dialog-confirm").html("<p>Bạn có chắc chắn muốn xóa bản ghi đã chọn?</p>");
        }
        var comfirmReturn = false;
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    postAction("delete", arrID);
                    $(this).dialog("close");
                },
                "Hủy": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}
function PXLHoSo(arrID, UrlListNguoiXuLy, UrlListProcessTrangThai, UrlListProcessTienTrinhXuLyHoSo) {
    $.post(encodeURI(urlForm), { "do": "Edit", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "",
        "UrlListNguoiXuLy": "" + UrlListNguoiXuLy + "",
        "UrlListProcessTrangThai": "" + UrlListProcessTrangThai + "", "UrlListProcessTienTrinhXuLyHoSo": "" + UrlListProcessTienTrinhXuLyHoSo + ""
    }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
function EditHoiDong(arrID, UrlListNguoiXuLy, UrlListProcessNguoiXuLyTrongHoiDong) {
    $.post(encodeURI(urlForm), { "do": "Edit", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "",
        "UrlListProcessNguoiXuLy": "" + UrlListNguoiXuLy + "",
        "UrlListProcessNguoiXuLyTrongHoiDong": "" + UrlListProcessNguoiXuLyTrongHoiDong + ""
    }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}
//<!--Hết Dịch vụ công-->
//<!--Quản trị hệ thống-->
function Reset(arrID) {
    $.post(encodeURI(urlFormReset), { "do": "Reset", "ItemID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        $("#dialog-form").html(data).dialog(
		    { title: "Cập nhật nội dung", height: formHeight, width: formWidth }
	    ).dialog("open");
    });
}

//<!--Hết Quản trị hệ thống-->
//Fckfinder
function Openfckfinder(title, btnupload,Urlweb) {
    $("#" + btnupload + "").button({
        icons: { primary: 'ui-icon-circle-check' },
        text: true
    }).click(function () {
        window.open("/UserControls/AnhDaiDien/fckFinder.aspx?type=Images&UrlWeb=" + Urlweb + "&option=0", null, "width=900,height=400,toolbar=no,location=no");
    });
}
function DeleteImage(btnXoaAnh, imgAnhdaiDien, hdfUrlImages) {
    $("#" + btnXoaAnh + "").button({
        icons: { primary: 'ui-icon-circle-check' },
        text: true
    }).click(function () {
        $("#" + imgAnhdaiDien + "").empty();
        $("#" + hdfUrlImages + "").val("");
    });
}
function ViewDM(arrID) {
    $.post(encodeURI(urlViewDM), { "do": "ViewDM", "DMID": "" + arrID + "", "UrlListProcess": "" + urlListProcess + "" }, function (data) {
        urlReload = urlViewDM + "&DMID=" + arrID;
        loadAjaxContent(urlReload, '#list-container')

    });
}
//end fckfinder