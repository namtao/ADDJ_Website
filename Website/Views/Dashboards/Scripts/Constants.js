(function ($, Dartboards) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 02 - 2014.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="Dartboards">namespace of application.</param>
    'use strict';
    //Document Ready
    $(function () {
        // your code here
    });

    Dartboards.Constants = {
        Keys: {
            ThongBao: 9
        },
        ThongBao: {
            PageSize: 5,
            popup_span_tieude: ".span-tieude",
            popup_span_noidung: ".span-noidung",
            popup_div_tieude:"#div-tieude",
            popup_div_tieude_content:"#div-tieude-content",
            popup_div_noidung:"#div-noidung",
            popup_div_noidung_content:"#div-noidung-content",
            popup_divShowWindow: "#DivEditWindow",

            idRegionThongBao: "#ul-thongbao",
            idPaging: "#Pagination-ThongBao"            
        }
    };

}(jQuery, window.Dartboards = window.Dartboards || {}));