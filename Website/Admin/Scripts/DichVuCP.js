(function ($, moduleName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">22 - 02 - 2014.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="DichVuCP">namespace of application.</param>
    'use strict';

    moduleName.List = function () {
        var self = this;
        self.LoadJS = function () {
            $("#selectall").click(function () {
                $('.case').find("input").attr('checked', this.checked);
            });
            $(".case").click(function () {
                if ($(".case").find("//input[checked='checked']").length == $(".case").find("input").length) {
                    $("#selectall").attr("checked", "checked");
                } else {
                    $("#selectall").removeAttr("checked");
                }
            });

            $("input[name='chkDeactive']").each(function (index) {
                if ($(this).val() == "1") {
                    $(this).attr("checked", "checked");
                } else {
                    $(this).removeAttr("checked");
                }
            });
        };

        self.ShowEditModal = function (id) {
            var uiMode = "EDIT";
            if (typeof id == "undefined") {
                id = "";
                uiMode = "CREATE";
            }
            var frame = $get('IframeEdit');
            frame.src = "/admin/DichVuCP_Add.aspx?UIMODE=" + uiMode + "&ID=" + id;
            $find('EditModalPopup').show();
        };

        self.NewExpanseOkay = function () {
            MessageAlert.AlertNormal('Thêm mới dịch vụ CP thành công', 'info');
            $('#<%=btFilter.ClientID %>').click();
        };

        self.EditOkayScript = function () {
            MessageAlert.AlertNormal('Cập nhật dịch vụ CP thành công, click refresh để xem kết quả', 'info');
            $('#<%=btFilter.ClientID %>').click();
        };

        self.BindDeactive = function (value) {
            if (typeof value == 'undefined') {
                return "";
            }
            if (value == "0") {
                return "";
            }
            return "checked";
        };

        return self;
    };

    //Document Ready
    $(function () {
        // your code here
        moduleName.List().LoadJS();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(runThisAfterEachAsyncPostback);
        // runs each time postback is initiated by any update panel on the page
        function runThisAfterEachAsyncPostback() {
            moduleName.List().LoadJS();
        }
    });

}(jQuery, window.DichVuCP = window.DichVuCP || {}));