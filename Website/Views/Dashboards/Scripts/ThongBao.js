(function ($, Dartboards) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 02 - 2014.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="Dartboards">namespace of application.</param>
    'use strict';

    Dartboards.ThongBao = function () {
        var self = this;
        var constant = Dartboards.Constants;
        var baseUrl = "/Views/Dashboards/Handler/Handler.ashx?key=" + constant.Keys.ThongBao;
        var hasPaged = false;

        self.getPaged = function (pageIndex) {
            pageIndex = pageIndex + 1;
            var urlRequest = baseUrl + "&pageIndex=" + pageIndex + "&pageSize=" + constant.ThongBao.PageSize;

            $.getJSON(urlRequest, '',
                function (result) {
                    bindData(result);
                });
            return false;
        };

        self.ShowThongBao = function (s) {
            var $tieude = $(s).find(constant.ThongBao.popup_span_tieude)[0];
            var $noiDung = $(s).find(constant.ThongBao.popup_span_noidung)[0];
            var tieuDe = $($tieude).text();
            var noiDung = $($noiDung).html();
            //if()
            $(constant.ThongBao.popup_div_tieude_content).text(tieuDe);
            $(constant.ThongBao.popup_div_noidung_content).html(noiDung);
            //hide title region because it is not necessary
            $(constant.ThongBao.popup_div_tieude).hide();

            $(constant.ThongBao.popup_divShowWindow).window({
                title: tieuDe
            });
            $(constant.ThongBao.popup_divShowWindow).show();
        };

        function getOptionsThongBao() {
            var opt = { callback: self.getPaged };
            $("input:text").each(function () {
                opt[this.name] = this.className.match(/numeric/) ? parseInt(this.value) : this.value;
            });
            return opt;
        }

        function getDate(value) {
            if (typeof value === 'string') {
                var a = /\/Date\((\d*)\)\//.exec(value);
                if (a) {
                    return new Date(+a[1]);
                }
            }
            return "";
        }

        function bindData(result) {
            var optInit = getOptionsThongBao();
            var idRegionThongBao = constant.ThongBao.idRegionThongBao;
            var idPaging = constant.ThongBao.idPaging;
            if (result == null) {
                return;
            }
            result = $.parseJSON(result);
            var count = result.Count;
            var items = $.parseJSON(result.Items);
            $(idRegionThongBao).html('');
            if (count > 0) {
                for (var i = 0; i < items.length; i++) {
                    var date = getDate(items[i].CDate);
                    var li = "<li class='li-thongbao'><a href='#' id='" + items[i].Id + "' class='a-thongbao' onclick='Dartboards.ThongBao().ShowThongBao(this)'>";
                    li += "<b><span class='span-tieude'>" + items[i].TieuDe + "</span></b>";
                    if (date != '') li += "<span class='span-ngay'>&nbsp;(" + Common.FormatDateHtml(date) + ")</span>";

                    if (items[i].IsNew) li += "&nbsp;&nbsp;<img src='/images/new.gif'>";

                    li += "<span style='display:none;' class='span-noidung'>" + items[i].NoiDung + "</span>";
                    li += "</a></li>";
                    $(idRegionThongBao).append(li);
                }
            }
            //prevent paging again
            if (!hasPaged) {
                $(idPaging).pagination(count, optInit);
                hasPaged = true;
            }
        }

        self.init = function () {
            self.getPaged(0);
        };

        return self;
    };

    //Document Ready
    $(function () {
        // your code here
        Dartboards.ThongBao().init();
    });


}(jQuery, window.Dartboards = window.Dartboards || {}));