(function ($, moduleName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">22 - 02 - 2014.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="DichVuCP">namespace of application.</param>
    'use strict';

    moduleName.Edit = function () {
        var self = this;
        self.LoadJS = function () {
            if ($('.date-input').length > 0)
                $('.date-input').datepick({ dateFormat: 'dd/mm/yyyy' });
        };


        self.okay = function() {
            //var UIMODE = $('#hdnWindowUIMODE').value;
            if (EditMode == "EDIT" || EditMode == "CREATE") {
                 window.parent.document.getElementById('btnOkayEdit').click();
            }
        };

        self.cancel = function() {
            if (EditMode == "EDIT" || EditMode == "CREATE")
                window.parent.document.getElementById('btnCancelEdit').click(); // 06/04/2016:truongvv gọi đến nút cha của form này để đóng lại popup dùng ModalPopupExtender
        };

        return self;
    };

    //Document Ready
    $(function () {
        // your code here
        moduleName.Edit().LoadJS();

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(runThisAfterEachAsyncPostback);
        // runs each time postback is initiated by any update panel on the page
        function runThisAfterEachAsyncPostback() {
            // your code here
        }
    });

}(jQuery, window.DichVuCP = window.DichVuCP || {}));