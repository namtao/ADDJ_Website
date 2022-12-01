var common = {
    loading: function (e) {
        $("#divOpacityDashBoard").show();
        $(".loadding").removeClass("off").addClass("on");
    },
    unLoading: function (e) {
        $("#divOpacityDashBoard").hide();
        $(".loadding").removeClass("on").addClass("off");
    },

    showBodyMark: function () {
        $("#divOpacityDashBoard").show();
    },
    unShowBodyMark: function () {
        $("#divOpacityDashBoard").hide();
    },
    showBodyMarkWait: function () {
        $("#divOpacityDashBoard").show();
        $(".loadding").removeClass("off").addClass("on");
    },
    unshowBodyMarkWait: function () {
        $("#divOpacityDashBoard").hide();
        $(".loadding").removeClass("off").addClass("on");
    },
    showBodyMarkTimer: function (e) {
        $("#divOpacityDashBoard").show();
        $(".loadding").removeClass("off").addClass("on");
        $("#ajaxTimer").removeClass("off").addClass("on");
        common.timeCountUp("#ajaxTimer .h", "#ajaxTimer .m", "#ajaxTimer .s");
    },
    unShowBodyMarkTimer: function (e) {
        $("#divOpacityDashBoard").hide();
        $(".loadding").removeClass("on").addClass("off");
        $("#ajaxTimer").removeClass("on").addClass("off");
    },
    timeCountUp: function (tagH, tagM, tagS) {
        try {
            sec = -1;
            $(tagS).html('00');
            $(tagM).html('00');
            $(tagH).html('00');

            function pad(val) { return val > 9 ? val : "0" + val; }

            setInterval(function () {
                $(tagS).html(pad(++sec % 60));
                $(tagM).html(pad(parseInt(sec / 60, 10) % 60));
                $(tagH).html(pad(parseInt(sec / 3600, 10)));
            }, 1000);
        }
        catch (e) {
            if (console && console.log) console.log(e);
        }
    },
    showWinMark: function () {
        var body = $('body');
        var html = '<div id="winMark" class="divOpacityDashBoard" style="opacity: .4; -moz-opacity: 0.4; filter: alpha(opacity=70); background: #999999; width: 100%; height: 100%; position: fixed; left: 0; top: 0px; display: block; z-index: 999 !important;"></div>';
        html += '<div id="iconWaiting" class="loadding on"><div class="loading"><div class="loadAjax"><img runat="server" src="/images/loading.gif" alt="Đang tải ..." /></div></div></div>';
        body.append(html);
    },
    hideWinMark: function () {
        // var body = $('body');
        $('#iconWaiting').remove();
        $('#winMark').remove();

    }
}

var Common = {

    PagerHtml: function (pid, pageindex, pagesize, totalrecord, callback) {
        if (totalrecord < pagesize)
            return "";

        var totalpage = totalrecord / pagesize == 0 ? totalrecord / pagesize : Math.floor(totalrecord / pagesize) + 1;

        //Logger.Info(totalpage);

        if (totalpage < 2)
            return "";
        var prev = pageindex - 1;
        if (prev < 0) prev = 0;
        var next = pageindex + 1;
        if (next > totalpage - 1) next = totalpage - 1;
        var htmlx = "<div id=\"" + pid + "pagerx\" class=\"pagelist clearfix\" style=\"clear: both;\">";
        htmlx += "<a href=\"javascript:void(0)\" onclick='" + callback + "(" + prev + "," + pagesize + ")' class=\"page\" title=\"Previous\">Previous</a>";
        for (var i = 0; i < totalpage; i++) {
            if (i == pageindex) {
                htmlx += "<span class=\"p_current\">" + (i + 1) + "</span>";
            }
            else {
                htmlx += "<a href=\"javascript:void(0)\" onclick='" + callback + "(" + i + "," + pagesize + ")' class=\"page\" title=\"2\">" + (i + 1) + "</a>";
            }
        }
        htmlx += "<a href=\"javascript:void(0)\" onclick='" + callback + "(" + next + "," + pagesize + ")' class=\"page\" title=\"Next\">Next</a>";

        htmlx += "</div>";
        return htmlx;
    },
    FormatDateHtml: function (d) {
        return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
    },
    FormatDateTimeHtml: function (d) {
        return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes();
    },
    Loading: function () {
        common.loading();
    },
    UnLoading: function () {
        common.unLoading();

    }
};

var MessageAlert = {
    AlertJSON: function (errorId) {
        var code = parseInt(errorId);
        switch (code) {
            case 0:
                parent.$.messager.alert('Thông báo', 'Dữ liệu không hợp lệ.');
                break;
            case -1:
                parent.$.messager.alert('Thông báo', 'Không tồn tại tài khoản này.');
                break;
            case -2:
                parent.$.messager.alert('Thông báo', 'Bạn không được sửa người dùng cùng cấp.');
                break;
            case -999:
                parent.$.messager.alert('Thông báo', 'Bạn không có quyền thực hiện chức năng này.');
                break;
            case -1000:
                parent.$.messager.alert('Thông báo', 'Bạn chưa đăng nhập hoặc đã mất session.');
                break;
        }
    },
    AlertNormal: function (text, title) {
        $.messager.alert('Thông báo', text, title);
    },

    AlertNormal: function (text, title, fnContent) {
        if (fnContent == undefined || fnContent == "") {
            $.messager.alert('Thông báo', text, title);
        }
        else {
            $.messager.alert('Thông báo', text, title, function () { document.getElementById(fnContent).focus(); });
        }
    },

    FocusControl: function (id) {
        document.getElementById(id).focus();
    },

    AlertRedirect: function (text, title, url) {
        //console.log(url);
        $.messager.alert('Thông báo', text, title, function () { window.location.href = url; });
    }
};

var Utility = {
    KeepSession: function () {
        var timeKeep = 1 * 30; // -> 30"
        var keepSession = setInterval(function () {
            $.ajax({
                beforeSend: function () {
                },
                type: "get",
                timeout: 5000,
                dataType: "text",
                url: "/Views/Share/keep.ashx",
                data: "type=keep",
                success: function (text) {
                    $('#idkeepx').html(text);
                },
                error: function () {
                }
            });
        }, timeKeep * 1000);
    },
    LTrim: function (str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
    },
    RTrim: function (str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
    },
    Trim: function (str, chars) {
        return this.LTrim(this.RTrim(str, chars), chars);
    },
    GetUrlParam: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null) return "";
        else return results[1];
    },
    CheckAllCheckBox: function (name) {
        $('input[name=' + name + ']').each(function () {
            this.checked = true;
        });
    },
    RemoveAllCheckBox: function (name) {
        $('input[name=' + name + ']').each(function () {
            this.checked = false;
        });
    },
    AutoScroll: function (anchor, top) {
        var $target = $('#' + anchor);
        $target = $target.length && $target || $('[name=' + anchor.slice(1) + ']');

        if ($target.length) {
            var targetOffset = $target.offset().top - top;
            $('html,body').animate({ scrollTop: targetOffset }, 1000);
            return false;
        }
    },
    Notify: function (msg) {
        var msgbox = $(document.createElement('div')).attr('id', 'cafexalert');
        var w = screen.width;
        msgbox.css({ 'z-index': '999', 'padding': '10px', "display": "block", 'position': 'absolute', 'top': '100px', 'left': (w / 2) - 50 + 'px', 'background-color': '#E5E5E5' });
        msgbox.html(msg);
        $('.x_wrap').append(msgbox);
    },
    NotifyRemove: function () {

        $('#cafexalert').remove();

    },
    GetImageBase64: function (x) {
        //s? d?ng khi l?y ?nh c?a t? postfile
        obj = $("#" + x);
        var path = obj.val();
        Logger.Info(path);
        if (path != null && path != "") {
            // Fix for FF3
            if (($.browser.mozilla == true) && (($.browser.version).match("1.9"))) {
                Logger.Info("mozilla ff3");
                fullPath = obj[0].files.item(0).getAsDataURL();
                path = "url(" + fullPath + ")";
            }
            else {
                Logger.Info("mozilla no");
                for (i = 0; i < path.length; i++) {
                    path = path.replace("\\", "/")
                }
                path = "url(file:///" + path + ")";
            }
            path = encodeURI(path);
            return path;
        }
        return "";
    },
    StripTagHTML: function (input) {
        return input.replace(/<\/?[^>]+>/gi, '');
    },
    SetShortString: function (str, len) {
        var x = str.length;
        if (x < len)
            return str;
        else
            return str.substring(0, len - 1) + "...";
    },
    StartWith: function (str, sstr) {
        return str.indexOf(sstr) == 0;
    }
};
function PagerCommon(pageurl, paramtype, pageindex, divdata, callback) {
    var cache_key = pageurl + paramtype + pageindex;
    var xhtml = "";
    if ($.jCache.hasItem(cache_key)) {
        xhtml = $.jCache.getItem(cache_key);
        $(divdata).html(xhtml);
        if (!($.browser.msie && $.browser.version.substr(0, 1) < 7))
            $(divdata).fadeTo("slow", 1);
    }
    else {
        $.ajax({
            beforeSend: function () {
                if (!($.browser.msie && $.browser.version.substr(0, 1) < 7))
                    $(divdata).fadeTo("slow", 0.1);
            },
            type: 'Get',
            dataType: 'text',
            url: pageurl,
            data: 'type=' + paramtype + '&pageIndex=' + pageindex + '&edu=' + (new Date()).getTime(),
            success: function (text) {
                if (typeof (callback) == 'undefined')
                    $(divdata).html(text);
                else
                    text = callback(text);

                if (!($.browser.msie && $.browser.version.substr(0, 1) < 7))
                    $(divdata).fadeTo("slow", 1);
                $.jCache.setItem(cache_key, text);
            }
        });

    }


}
var Logger = {
    Info: function (str) {
    }
};

var Validate = {
    PassWord: function (pass) {
        var strValidChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_!@#$%^&*()";
        var strChar;
        var blnResult = true;
        if (pass.length == 0) return false;
        for (i = 0; i < pass.length && blnResult == true; i++) {
            strChar = pass.charAt(i);
            if (strValidChars.indexOf(strChar) == -1) {
                blnResult = false;
            }
        }
        return blnResult;
    },
    Email: function (email) {
        var atpos = email.indexOf("@");
        var dotpos = email.lastIndexOf(".");
        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
            return false;
        }
        return true;
    },
    IsDate: function (txtDate) {
        var currVal = txtDate;
        if (currVal == '')
            return false;

        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray == null)
            return false;

        //Checks for dd/mm/yyyy format.
        var dtMonth = dtArray[3];
        var dtDay = dtArray[1];
        var dtYear = dtArray[5];

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

};

jQuery.fn.LoadMore = function (urlHandler, idMoreItem) {
    var moreItem = $('#' + idMoreItem);
    var temp = moreItem.html();
    moreItem.html(ImageLoading.size1);
    $.ajax({
        type: "get",
        dataType: "text",
        url: urlHandler,
        success: function (text) {
            $(this).append("<div class=" + $(this).attr('class') + ">");
            $(this).append(text);
            $(this).append("</div>");
            moreItem.html(temp);
            $.scrollTo(165, {
                duration: 350
            });
        },
        error: function () {
            //consol.log("có lỗi xảy ra");
        }
    });
};
jQuery.fn.CountDownText = function (idCount) {
    $(this).keyup(function () {
        var lengthTxt = $(this).val().length;
        var max = $(this).attr('maxlength');
        $('#' + idCount).html(max - Utility.StripTagHTML($(this).val()).length);
        //if (lengthTxt >= max) $(this).html().substring(0, max);
    });
};

function ConvertJsonDateToStringFormat(jsonDate, format) {
    var date = new Date(parseInt(jsonDate.substr(6)));
    return formatDate(date, format);
}

function GetDomainPort() {
    var domain = document.location.hostname;
    if (location.port > 0)
        domain += ':' + location.port;
    return domain;
}