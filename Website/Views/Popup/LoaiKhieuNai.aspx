<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoaiKhieuNai.aspx.cs" Inherits="Website.Views.Popup.LoaiKhieuNai" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loại khiếu nại</title>
    <%= string.Format("<link href=\"{0}?ver={1}\" type=\"text/css\" rel=\"stylesheet\" />", ResolveClientUrl("~/content/easyui.css"), Website.AppCode.Common.Ver) %>
    <%= string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", ResolveUrl("~/Js/jquery1.8.3.min.js")) %>
    <%= string.Format("<script src=\"{0}?ver={1}\" type=\"text/javascript\"></script>", "http://www.jeasyui.com/easyui/jquery.easyui.min.js", Website.AppCode.Common.Ver) %>
    <style type="text/css">
        .custom-panel { overflow: initial; }
        .panel-body-noheader { border: none; }
        .custom-panel .textbox-text { border: none; padding: 3px; border-left: 1px solid #a4bed4; }
        .panel.combo-p { border: 1px solid #a4bed4; border-top: 0px; }
    </style>
</head>
<body>
    <form runat="server">
        <div class="easyui-panel custom-panel" style="width: 100%; max-width: 300px;">
            <div class="inner">
                <input class="easyui-combobox" name="language" style="width: 100%;" data-options="
                    loader: myloader,                    
                    valueField: 'id',
                    mode: 'remote',
                    textField: 'text',
                    panelWidth: 300,
                    panelHeight: 300,
                    formatter: formatItem,                    
                    labelPosition: 'top'
                    " />
            </div>
        </div>
        <script type="text/javascript">
            function formatItem(row) {
                var objRet = '<span data-id="' + row.id + '" style="font-weight:bold; color: #444;">' + row.name + '</span>';
                if (row.linhvucchung != row.loaikhieunai) { // Tìm được lĩnh vực con
                    if (row.linhvucchung != null) { objRet += '<br/><span style="color: #2e1cff">' + row.linhvucchung + '</span>' }
                    if (row.loaikhieunai != null) { objRet += '<br/><span style="color: #D00000 ">' + row.loaikhieunai + '</span>' }
                }
                else { // Tìm được lĩnh vực chung
                    if (row.loaikhieunai != null) { objRet += '<br/><span style="color: #D00000">' + row.loaikhieunai + '</span>' }
                }
                return objRet;
            }
            var myloader = function (param, success, error) {
                var q = param.q || '';
                if (q.length <= 2) { return false }
                $.ajax({
                    url: 'Ajax/Handler.ashx',
                    dataType: 'json',
                    data: {
                        q: q
                    },
                    success: function (data) {
                        var items = $.map(data, function (item, index) {
                            return {
                                rowNumber: index,
                                id: item.id,
                                name: item.name,
                                linhvucchung: item.linhvucchung,
                                loaikhieunai: item.loaikhieunai
                            };
                        });
                        success(items);
                    },
                    error: function () {
                        error.apply(this, arguments);
                    }
                });
            }
        </script>
    </form>
</body>
</html>
